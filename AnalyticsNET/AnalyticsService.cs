using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AnalyticsNET.Services
{
    /// <summary>
    /// Main analytics service, This service should be declared as s Singleton (Single Instance)
    /// </summary>
    public class AnalyticsService : IDisposable
    {
        private AnalyticConfiguration configurations { get; set; }

        private AnalyticsDeviceOptions AnalyticsDeviceOptions { get; set; }
        private readonly IAnalyticsLogger logger;
        private Thread _analyticThread;
        private int _allSuccessfullySentTraits = 0;

        private ConcurrentQueue<Trait> pendingToSendTraits { get; set; } = new ConcurrentQueue<Trait>();

        private string _statusRead { get; set; } = "Stopped";
        private string analyticsStatus
        {
            get { return _statusRead; }
            set { _statusRead = value; }
        }


        public AnalyticsService(AnalyticsDeviceOptions analyticsDeviceOptions, IAnalyticsLogger analyticsLogger = null)
        {
            this.AnalyticsDeviceOptions = analyticsDeviceOptions;
            this.logger = analyticsLogger ?? new AnalyticsLogger();
            this.pendingToSendTraits = new ConcurrentQueue<Trait>();
            this.analyticsStatus = AnalyticsStatus.Stopped.ToString();
            if (this.AnalyticsDeviceOptions.StartServiceAutomatically)
                StartService();
        }

        /// <summary>
        /// This will bootup the service in the background. Note: Start Service otherwise traits will not work
        /// </summary>
        public void StartService()
        {
            try
            {
                //Check Status
                if (this.analyticsStatus == AnalyticsStatus.Running.ToString() || this.analyticsStatus == AnalyticsStatus.Seeding.ToString())
                    return;

                logger.LogInformation("Starting Analytics Service");
                this.analyticsStatus = AnalyticsStatus.Starting.ToString();
                //Request to Initiate Configuration
                this.configurations = new AnalyticConfiguration
                {
                    EndPointUrl =(string.IsNullOrWhiteSpace(this.AnalyticsDeviceOptions.OverridedAnalyticsServerEndpoint)) ? "https://analytics.crudsofttechnologies.com/": this.AnalyticsDeviceOptions.OverridedAnalyticsServerEndpoint,
                    AdditionalRequestHeaders = new Dictionary<string, string>(),
                    AdditionalRequestParameters = new Dictionary<string, string>(),
                    NextCallBackInMilliseconds = 3000
                };
                //Create an Instance of Timer
                StartAnalyticBackgroundThread();
            }
            catch (Exception ex)
            {
                logger.LogError($"Unable to Start Analytics Servie: Error: {ex.Message}");
            }

        }
        /// <summary>
        /// Stop Service to End the Analytics process
        /// </summary>
        public void StopService()
        {
            try
            {
                logger.LogInformation("Stopping Analytics Service");
                this.analyticsStatus = AnalyticsStatus.Stopping.ToString();
                this._analyticThread.Abort();
                this.analyticsStatus = AnalyticsStatus.Stopped.ToString();
                //Unprocessed
                logger.LogInformation("Stopped Analytics Service");
            }
            catch { }
        }

        public string GetAnalyticsStatus() => this.analyticsStatus;

        public void Track(string traitKey, string traitValue) => Track(new Trait { Key = traitKey, Value = traitValue });
        public void Track(Trait trait) => this.pendingToSendTraits.Enqueue(trait);

        public void Track(IList<Trait> traits)
        {
            foreach (Trait trait in traits)
                this.Track(trait);
        }

        private void StartAnalyticBackgroundThread()
        {
            //Set Status
            this.analyticsStatus = AnalyticsStatus.Running.ToString();
            this._analyticThread = new Thread(async () =>
            {
                while (this.analyticsStatus == AnalyticsStatus.Running.ToString() || this.analyticsStatus == AnalyticsStatus.Seeding.ToString())
                {

                    this.analyticsStatus = AnalyticsStatus.Running.ToString();
                    //Track Last Seen
                    if (this.AnalyticsDeviceOptions.TrackDeviceHeartBeat)
                        this.Track(new Trait { Key = "heartBeat", Value = DateTime.Now.ToString() });
                    //Proceed
                    var readyToSendTraits = this.pendingToSendTraits.Where(x => x.SentSuccesfully != true && x.NextSending < DateTime.Now).ToList().Take(100);
                    //Check if Null
                    if (readyToSendTraits.Any())
                    {
                        logger.LogInformation($"Dispatching Analytics");
                        //Append All Traits
                        string allTraitsAppend = string.Empty;
                        foreach (Trait trait in readyToSendTraits)
                            allTraitsAppend = $"{allTraitsAppend}{trait.Key} : {trait.Value}{Environment.NewLine}";

                        try
                        {
                            this.analyticsStatus = AnalyticsStatus.Seeding.ToString();
                            //Parameters
                            Dictionary<string, string> parameters = new Dictionary<string, string>();
                            parameters.Add("traitKey", "rawData");
                            parameters.Add("traitValue", allTraitsAppend);
                            //Add Configs
                            parameters.Add("appName", this.AnalyticsDeviceOptions.AppName);
                            parameters.Add("deviceName", this.AnalyticsDeviceOptions.DeviceName);
                            parameters.Add("deviceId", this.AnalyticsDeviceOptions.DeviceID);
                            parameters.Add("sessionId", this.AnalyticsDeviceOptions.CurrentAnalyticTrackingId);
                            //Add Default
                            foreach (var param in configurations.AdditionalRequestParameters)
                                parameters.Add(param.Key, param.Value);
                            //Headers
                            Dictionary<string, string> headers = new Dictionary<string, string>();
                            headers.Add("appSecret", this.AnalyticsDeviceOptions.AppSecretKey);
                            //Add Default
                            foreach (var header in configurations.AdditionalRequestHeaders)
                                headers.Add(header.Key, header.Value);
                            //Call Http Client
                            string response = await CoreLib.RequestAsync(configurations.EndPointUrl, System.Net.Http.HttpMethod.Post, parameters, headers);
                            this.analyticsStatus = AnalyticsStatus.Running.ToString();
                            //Update
                            int sentSuccess = 0;
                            foreach (Trait trait in readyToSendTraits)
                            {
                                trait.SentSuccesfully = true;
                                trait.FailedCount = 0;
                                sentSuccess++;
                                this._allSuccessfullySentTraits++;
                                Trait _key = trait;
                                this.pendingToSendTraits.TryDequeue(out _key);
                            }
                            if (sentSuccess > 0)
                                logger.LogInformation($"Successfully dispatched ({sentSuccess}) Traits, Response: {response}");

                            //Set Next Call Back
                            SetNextCallBackFromServerResponse(response);
                            //Detected One has been Sent try to Reque
                            RequeueAllUnSent();
                        }
                        catch (Exception ex)
                        {
                            foreach (Trait trait in readyToSendTraits)
                            {
                                logger.LogError($"Error Sending Trait: {trait.Id}, Exception: {ex.Message}");
                                //Mark to be Requeued
                                trait.FailedCount++;
                                trait.NextSending = trait.NextSending.AddMinutes(trait.FailedCount);
                                logger.LogInformation($"Added Trait: {trait.Id} to Queue To Send Later at: {trait.NextSending}");
                            }
                            //Maximum Fails to Stop
                            int maxFailed = GetFailedTraitsCount();
                            if (maxFailed >= this.AnalyticsDeviceOptions.MaxFailedToAbort)
                                OnMaxFailedToSendTraitsReached(maxFailed);
                        }

                    }


                    //Sleep Thread
                    Thread.Sleep((this.configurations.NextCallBackInMilliseconds) < 3000 ? 3000 : this.configurations.NextCallBackInMilliseconds);
                }
            });
            this._analyticThread.Start();
        }

        private void SetNextCallBackFromServerResponse(string response)
        {
            try
            {
                var dataX = response.Split('|');
                int nextCallback = 0;
                //Check
                if (dataX.Count() < 2)
                    throw new Exception("Unable to Decode or Retrieve Expected Server Response");

                int.TryParse(dataX[0], out nextCallback);
                //Check
                if (nextCallback < 3000)
                    nextCallback = 3000;
                //Check if CallBack Changed
                if (nextCallback != this.configurations.NextCallBackInMilliseconds)
                {
                    this.configurations.NextCallBackInMilliseconds = nextCallback;
                    logger.LogInformation($"CallBack Changed To: {nextCallback}");
                }
                //Get Session ID
                string nextSessionId = string.Empty;
                nextSessionId = dataX[1];
                //Check if Empty
                if (!string.IsNullOrWhiteSpace(nextSessionId) && nextSessionId != this.AnalyticsDeviceOptions.CurrentAnalyticTrackingId)
                {
                    this.AnalyticsDeviceOptions.CurrentAnalyticTrackingId = nextSessionId;
                    logger.LogInformation($"Session ID Renewed To: {nextSessionId}");
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR: FAILED Get Session ID and Next CallBack from Response: {ex.Message}");
            }
        }

        protected virtual void OnMaxFailedToSendTraitsReached(int failedCount)
        {
            logger.LogInformation($"Maximum Failed Traits ({failedCount}) reached, Terminating Analytics service");
            StopService();
        }
        private void RequeueAllUnSent()
        {
            var allUnsent = this.pendingToSendTraits.Where(x => x.SentSuccesfully != true && x.FailedCount > 0);
            if (allUnsent.Any())
            {
                logger.LogInformation($"Requeing ({allUnsent.Count()}) Analytics");
                foreach (Trait trait in allUnsent)
                {
                    trait.NextSending = DateTime.Now.AddSeconds(trait.FailedCount); //Based on Failure Counts
                    trait.FailedCount = 0;
                }
            }
        }

        public string GetCurrentSessionTrackingID() => this.AnalyticsDeviceOptions.CurrentAnalyticTrackingId;
        public int GetFailedTraitsCount() => this.pendingToSendTraits.Where(x => x.SentSuccesfully != true && x.FailedCount > 0).Count();
        public int GetSentTraitsCount() => _allSuccessfullySentTraits;

        public void Dispose() => StopService();

    }
}
