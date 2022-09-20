using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyticsNET.Logic
{
    /// <summary>
    /// Main analytics service, This service should be declared as s Singleton (Single Instance)
    /// </summary>
    public class AnalyticsService : IDisposable, IAnalyticsService
    {
        private readonly AnalyticsDeviceOptions _options;
        private readonly IAnalyticsLogger _logger;
        private Thread AnalyticThread;
        private int _allSuccessfullySentTraits = 0;
        private HttpClient _client;
        private ConcurrentQueue<Trait> pendingToSendTraits { get; set; } = new ConcurrentQueue<Trait>();

        private int NextCallBackInMilliseconds { get; set; } = 3000;
        private string StatusRead { get; set; } = "Stopped";
        private string AnalyticsStatus
        {
            get { return StatusRead; }
            set { StatusRead = value; }
        }


        public AnalyticsService(AnalyticsDeviceOptions analyticsDeviceOptions, IAnalyticsLogger analyticsLogger = null)
        {
            this._options = analyticsDeviceOptions;
            _logger = analyticsLogger ?? new AnalyticsLogger();
            pendingToSendTraits = new ConcurrentQueue<Trait>();
            AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Stopped.ToString();
            if (_options.StartServiceAutomatically)
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
                if (AnalyticsStatus == AnalyticsNET.AnalyticsStatus.Running.ToString() || AnalyticsStatus == AnalyticsNET.AnalyticsStatus.Seeding.ToString())
                    return;

                _logger.LogInformation("Starting Analytics Service");
                AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Starting.ToString();
                if (string.IsNullOrWhiteSpace(this._options.AnalyticsAPIEndpoint))
                    throw new Exception("Analytics Server API has not been configured, start failed");
                this.NextCallBackInMilliseconds = 3000;
                //Device Http Single Client (Re-using Connection)
                this._client = new HttpClient();
                this._client.Timeout = TimeSpan.FromMinutes(3);
                //!Important > Append AppSecret in Header
                this._client.DefaultRequestHeaders.Add("appSecret", this._options.AppSecretKey);
                //Append Additional Headers for Each Request
                if (this._options.DefaultRequestHeaders != null)
                    foreach (KeyValuePair<string, string> param in this._options.DefaultRequestHeaders)
                        this._client.DefaultRequestHeaders.Add(param.Key, param.Value);
                //Create an Instance of Timer
                StartAnalyticBackgroundThread();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to Start Analytics Servie: Error: {ex.Message}");
            }

        }
        /// <summary>
        /// Stop Service to End the Analytics process
        /// </summary>
        public void StopService()
        {
            try
            {
                _logger.LogInformation("Stopping Analytics Service");
                AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Stopping.ToString();
                AnalyticThread.Abort();
                this._client.CancelPendingRequests();
                this._client.Dispose();
                AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Stopped.ToString();
                //Unprocessed
                _logger.LogInformation("Stopped Analytics Service");
            }
            catch { }
        }

        public string GetAnalyticsStatus() => AnalyticsStatus;

        public void Track(string traitKey, string traitValue) => Track(new Trait { Key = traitKey, Value = traitValue });
        public void Track(Trait trait) => pendingToSendTraits.Enqueue(trait);

        public void Track(IList<Trait> traits)
        {
            foreach (Trait trait in traits)
                Track(trait);
        }

        private void StartAnalyticBackgroundThread()
        {
            //Set Status
            AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Running.ToString();
            AnalyticThread = new Thread(async () =>
            {
                while (AnalyticsStatus == AnalyticsNET.AnalyticsStatus.Running.ToString() || AnalyticsStatus == AnalyticsNET.AnalyticsStatus.Seeding.ToString())
                {

                    AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Running.ToString();
                    //Track Last Seen
                    if (_options.TrackDeviceHeartBeat)
                        Track(new Trait { Key = "heartBeat", Value = DateTime.Now.ToString() });
                    //Proceed
                    var readyToSendTraits = pendingToSendTraits.Where(x => x.SentSuccesfully != true && x.NextSending < DateTime.Now).ToList().Take(100);
                    //Check if Null
                    if (readyToSendTraits.Any())
                    {
                        _logger.LogInformation($"Dispatching Analytics");
                        //Append All Traits
                        string allTraitsAppend = string.Empty;
                        foreach (Trait trait in readyToSendTraits)
                            allTraitsAppend = $"{allTraitsAppend}{trait.Key} : {trait.Value}{Environment.NewLine}";

                        try
                        {
                            AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Seeding.ToString();
                            //Parameters
                            Dictionary<string, string> parameters = new Dictionary<string, string>();
                            parameters.Add("traitKey", "rawData");
                            parameters.Add("traitValue", allTraitsAppend);
                            //Add Configs
                            parameters.Add("appName", "**secret**"); //~Absolete
                            parameters.Add("deviceName", _options.DeviceName);
                            parameters.Add("deviceId", _options.DeviceID);
                            parameters.Add("sessionId", _options.CurrentAnalyticTrackingId);
                            //Sending POST Request
                            var request = new HttpRequestMessage(HttpMethod.Post, this._options.AnalyticsAPIEndpoint);
                            request.Content = new FormUrlEncodedContent(parameters);
                            var requestResponse = await _client.SendAsync(request);
                            if (!requestResponse.IsSuccessStatusCode)
                                throw new Exception($"Server Responded with Error: Status Code: {requestResponse.StatusCode}, ReasonPhrase: {requestResponse.ReasonPhrase}");
                            string response = await requestResponse.Content.ReadAsStringAsync();
                            //Proceed and Read Response 
                            AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Running.ToString();
                            //Update
                            int sentSuccess = 0;
                            foreach (Trait trait in readyToSendTraits)
                            {
                                trait.SentSuccesfully = true;
                                trait.FailedCount = 0;
                                sentSuccess++;
                                _allSuccessfullySentTraits++;
                                Trait _key = trait;
                                pendingToSendTraits.TryDequeue(out _key);
                            }
                            if (sentSuccess > 0)
                                _logger.LogInformation($"Successfully dispatched ({sentSuccess}) Traits, Response: {requestResponse}");

                            //Set Next Call Back
                            SetNextCallBackFromServerResponse(response);
                            //Detected One has been Sent try to Reque
                            RequeueAllUnSent();
                        }
                        catch (Exception ex)
                        {
                            foreach (Trait trait in readyToSendTraits)
                            {
                                _logger.LogError($"Error Sending Trait: {trait.Id}, Exception: {ex.Message}");
                                //Mark to be Requeued
                                trait.FailedCount++;
                                trait.NextSending = trait.NextSending.AddMinutes(trait.FailedCount);
                                _logger.LogInformation($"Added Trait: {trait.Id} to Queue To Send Later at: {trait.NextSending}");
                            }
                            //Maximum Fails to Stop
                            int maxFailed = GetFailedTraitsCount();
                            if (maxFailed >= _options.MaxFailedToAbort)
                                OnMaxFailedToSendTraitsReached(maxFailed);
                        }

                    }
                    //Sleep Thread
                    Thread.Sleep(this.NextCallBackInMilliseconds < 3000 ? 3000 : this.NextCallBackInMilliseconds);
                }
            });
            AnalyticThread.Start();
        }

        private void SetNextCallBackFromServerResponse(string response)
        {
            try
            {
                var dataX = response.Split('|');
                if (dataX.Count() < 2)
                    throw new Exception("Unable to Decode or Retrieve Expected Server Response");
                int.TryParse(dataX[0], out int nextCallback);
                //Check
                if (nextCallback < 3000)
                    nextCallback = 3000;
                //Check if CallBack Changed
                if (nextCallback != this.NextCallBackInMilliseconds)
                {
                    this.NextCallBackInMilliseconds = nextCallback;
                    _logger.LogInformation($"CallBack Changed To: {nextCallback}");
                }
                //Get Session ID
                string nextSessionId = string.Empty;
                nextSessionId = dataX[1];
                //Check if Empty
                if (!string.IsNullOrWhiteSpace(nextSessionId) && nextSessionId != _options.CurrentAnalyticTrackingId)
                {
                    _options.CurrentAnalyticTrackingId = nextSessionId;
                    _logger.LogInformation($"Session ID Renewed To: {nextSessionId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: FAILED Get Session ID and Next CallBack from Response: {ex.Message}");
            }
        }

        protected virtual void OnMaxFailedToSendTraitsReached(int failedCount)
        {
            _logger.LogInformation($"Maximum Failed Traits ({failedCount}) reached, Terminating Analytics service");
            StopService();
        }
        private void RequeueAllUnSent()
        {
            var allUnsent = pendingToSendTraits.Where(x => x.SentSuccesfully != true && x.FailedCount > 0);
            if (allUnsent.Any())
            {
                _logger.LogInformation($"Requeing ({allUnsent.Count()}) Analytics");
                foreach (Trait trait in allUnsent)
                {
                    trait.NextSending = DateTime.Now.AddSeconds(trait.FailedCount); //Based on Failure Counts
                    trait.FailedCount = 0;
                }
            }
        }


        public string GetCurrentSessionTrackingID() => _options.CurrentAnalyticTrackingId;
        public int GetFailedTraitsCount() => pendingToSendTraits.Where(x => x.SentSuccesfully != true && x.FailedCount > 0).Count();
        public int GetSentTraitsCount() => _allSuccessfullySentTraits;

        public void Dispose() => StopService();

    }
}
