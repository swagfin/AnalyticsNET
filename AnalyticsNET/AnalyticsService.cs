using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace AnalyticsNET.Services
{
    /// <summary>
    /// Main analytics service, This service should be declared as s Singleton (Single Instance)
    /// </summary>
    public class AnalyticsService
    {
        private AnalyticConfiguration configurations { get; set; }

        private AnalyticsDeviceOptions AnalyticsDeviceOptions { get; set; }
        private readonly IAnalyticsLogger logger;
        private ConcurrentQueue<Trait> pendingToSendTraits { get; set; } = new ConcurrentQueue<Trait>();

        private string _statusRead { get; set; } = "Stopped";
        private string analyticsStatus
        {
            get { return _statusRead; }
            set { _statusRead = value; }
        }
        private Timer analyticsTimer { get; set; }


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
                    EndPointUrl = "https://analytics.crudsofttechnologies.com/",
                    AdditionalRequestHeaders = new Dictionary<string, string>(),
                    AdditionalRequestParameters = new Dictionary<string, string>(),
                    NextCallBackInMilliseconds = 3000
                };

                //Create an Instance of Timer
                analyticsTimer = new Timer();
                analyticsTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                analyticsTimer.Interval = this.configurations.NextCallBackInMilliseconds;
                analyticsTimer.Enabled = true;
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
            logger.LogInformation("Stopping Analytics Service");
            this.analyticsStatus = AnalyticsStatus.Stopping.ToString();
            if (analyticsTimer != null)
                analyticsTimer.Enabled = false;
            analyticsTimer = null;
            this.analyticsStatus = AnalyticsStatus.Stopped.ToString();
            //Unprocessed
        }

        public string GetAnalyticsStatus() => this.analyticsStatus;

        public void Track(string traitKey, string traitValue) => Track(new Trait { Key = traitKey, Value = traitValue });
        public void Track(Trait trait) => this.pendingToSendTraits.Enqueue(trait);

        public void Track(IList<Trait> traits)
        {
            foreach (Trait trait in traits)
                this.Track(trait);
        }

        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            analyticsTimer.Stop();
            this.analyticsStatus = AnalyticsStatus.Running.ToString();
            //Track Last Seen
            if (this.AnalyticsDeviceOptions.TrackDeviceHeartBeat)
                this.Track(new Trait { Key = "heartBeat", Value = DateTime.Now.ToString() });
            //Proceed
            var readyToSendTraits = this.pendingToSendTraits.Where(x => x.SentSuccesfully != true && x.NextSending < DateTime.Now).ToList().Take(20);
            //Check if Null
            if (readyToSendTraits != null && readyToSendTraits.Count() > 0)
            {

                logger.LogInformation($"Executing ({readyToSendTraits.Count()}) Analytics");
                //Append All Traits
                string allTraitsAppend = string.Empty;
                foreach (Trait trait in readyToSendTraits)
                    allTraitsAppend = $"{allTraitsAppend}{trait.Key} : {trait.Value}{Environment.NewLine}";

                try
                {
                    logger.LogInformation($"Sending All Traits");
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
                    foreach (Trait trait in readyToSendTraits)
                    {
                        trait.SentSuccesfully = true;
                        trait.FailedCount = 0;
                        logger.LogInformation($"Trait: {trait.Id} Sent Successfully, Response: {response}");
                        RemoveTraitFromQueue(trait);
                    }

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
            //RENABLE THE CLOCK
            if (analyticsTimer != null)
                analyticsTimer.Start();
        }

        private void SetNextCallBackFromServerResponse(string response)
        {
            try
            {
                logger.LogInformation($"Attempting to Get Session ID and Next CallBack from Response");

                var dataX = response.Split('|');
                int nextCallback = 0;
                //Check
                if (dataX.Count() < 2)
                    throw new Exception("Unable to Decode or Retrieve Expected Server Response");

                int.TryParse(dataX[0], out nextCallback);
                //Check
                if (nextCallback < 1000)
                    nextCallback = 1000;
                //Check if CallBack Changed
                if (nextCallback != this.configurations.NextCallBackInMilliseconds)
                {
                    this.configurations.NextCallBackInMilliseconds = nextCallback;
                    analyticsTimer.Interval = this.configurations.NextCallBackInMilliseconds;
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

        public string GetCurrentSessionTrackingID() => this.AnalyticsDeviceOptions.CurrentAnalyticTrackingId;

        protected virtual void OnMaxFailedToSendTraitsReached(int failedCount)
        {
            logger.LogInformation($"Maximum Failed Traits ({failedCount}) reached, Terminating Analytics service");
            StopService();
        }
        private void RemoveTraitFromQueue(Trait trait)
        {
            logger.LogInformation($"Attempting to Remove Trait: {trait.Id}");
            //Remove Trait
            bool removed = this.pendingToSendTraits.TryDequeue(out trait);
            if (removed)
                logger.LogInformation($"Trait: {trait.Id} REMOVED from Queue");
            else
                logger.LogWarning($"Trait: {trait.Id} WAS NOT REMOVED from Queue");
        }

        private void RequeueAllUnSent()
        {
            var allUnsent = this.pendingToSendTraits.Where(x => x.SentSuccesfully != true && x.FailedCount > 0);
            if (allUnsent != null && allUnsent.Count() > 0)
            {
                logger.LogInformation($"Requeing ({allUnsent.Count()}) Analytics");
                foreach (Trait trait in allUnsent)
                {
                    trait.NextSending = DateTime.Now.AddSeconds(trait.FailedCount); //Based on Failure Counts
                    trait.FailedCount = 0;
                }
            }
        }

        public int GetFailedTraitsCount() => this.pendingToSendTraits.Where(x => x.SentSuccesfully != true && x.FailedCount > 0).Count();
    }
}
