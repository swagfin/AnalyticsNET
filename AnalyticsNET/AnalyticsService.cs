using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyticsNET
{
    /// <summary>
    /// Main analytics service, This service should be declared as s Singleton (Single Instance)
    /// </summary>
    public class AnalyticsService
    {
        private readonly AnalyticsOptions _options;
        private readonly IAnalyticsLogger _logger;
        private Thread AnalyticThread;
        private HttpClient _client;
        private List<Trait> PendingTraits { get; set; } = new List<Trait>();

        private int NextCallBackInMilliseconds { get; set; } = 10000;
        private string StatusRead { get; set; } = "Stopped";
        private string AnalyticsStatus { get { return StatusRead; } set { StatusRead = value; } }

        public AnalyticsService(AnalyticsOptions analyticsDeviceOptions, IAnalyticsLogger analyticsLogger = null)
        {
            _options = analyticsDeviceOptions;
            _logger = analyticsLogger ?? new AnalyticsLoggerNoLogging();
            PendingTraits = new List<Trait>();
            _client = new HttpClient();
            _client.Timeout = TimeSpan.FromMinutes(3);
            _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "SPOS-Analytics");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("appSecret", _options.AppSecretKey);
            foreach (KeyValuePair<string, string> param in _options.DefaultRequestHeaders ?? new Dictionary<string, string>())
                _client.DefaultRequestHeaders.TryAddWithoutValidation(param.Key, param.Value);
            AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Stopped.ToString();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("initializing...");
            try
            {
                if (AnalyticsStatus == AnalyticsNET.AnalyticsStatus.Running.ToString() || AnalyticsStatus == AnalyticsNET.AnalyticsStatus.Seeding.ToString())
                    return Task.CompletedTask;
                AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Starting.ToString();
                if (string.IsNullOrWhiteSpace(_options.AnalyticsAPIEndpoint))
                    throw new Exception("Analytics Server API has not been configured, start failed");
                //** start thread
                StartAnalyticBackgroundThread(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to Start Analytics Servie: Error: {ex.Message}");
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// Stop Service to End the Analytics process
        /// </summary>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Stopping.ToString();
                AnalyticThread?.Interrupt();
                AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Stopped.ToString();
                _logger.LogInformation("stopped service");
            }
            catch { }
            return Task.CompletedTask;
        }

        public string GetAnalyticsStatus() => AnalyticsStatus;

        public void Track(string traitKey, string traitValue) => Track(new Trait { Key = traitKey, Value = traitValue });
        public void Track(Trait trait) => PendingTraits.Add(trait);

        private void StartAnalyticBackgroundThread(CancellationToken cancellationToken)
        {
            //Set Status
            AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Running.ToString();
            AnalyticThread = new Thread(async () =>
            {
                while (!cancellationToken.IsCancellationRequested && (AnalyticsStatus == AnalyticsNET.AnalyticsStatus.Running.ToString() || AnalyticsStatus == AnalyticsNET.AnalyticsStatus.Seeding.ToString()))
                {

                    AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Running.ToString();
                    //Sleep Thread
                    Thread.Sleep(NextCallBackInMilliseconds < 3000 ? 3000 : NextCallBackInMilliseconds);
                    //Track Last Seen
                    if (_options.SendDeviceHeartBeats)
                        this.Track(new Trait { Key = "heartBeat", Value = DateTime.Now.ToString() });
                    //Proceed
                    List<Trait> readyToSendTraits = PendingTraits.Where(x => x.SentSuccesfully != true && x.NextSending < DateTime.Now).Take(100).ToList();
                    //Check if Null
                    if (readyToSendTraits.Count > 0)
                    {
                        _logger.LogInformation($"dispatching analytics...");
                        try
                        {
                            AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Seeding.ToString();
                            //Parameters
                            Dictionary<string, string> parameters = new Dictionary<string, string>
                            {
                                { "appName", _options.AppName },
                                { "deviceId", _options.DeviceID },
                                { "deviceName", Environment.MachineName },
                                { "traitKey", "cryptData" },
                                { "traitValue", ToStuEncodedString(readyToSendTraits) }
                            };
                            //Sending POST Request
                            HttpResponseMessage response = await _client.PostAsync(_options.AnalyticsAPIEndpoint.Trim('/'), new FormUrlEncodedContent(parameters), cancellationToken);
                            response.EnsureSuccessStatusCode();
                            string responseMsg = await response.Content.ReadAsStringAsync();
                            AnalyticsStatus = AnalyticsNET.AnalyticsStatus.Running.ToString();
                            //Update
                            foreach (Trait trait in readyToSendTraits)
                            {
                                trait.SentSuccesfully = true;
                                trait.FailedCount = 0;
                                try { PendingTraits.Remove(trait); } catch { }
                            }
                            _logger.LogInformation($"successfully dispatched ({readyToSendTraits.Count}) traits, Response: {responseMsg}");
                            //** proceed
                            TryConsumeServerResponse(responseMsg, cancellationToken);
                            RequeueAllUnSent();
                        }
                        catch (Exception ex)
                        {
                            foreach (Trait trait in readyToSendTraits)
                            {
                                _logger.LogError($"error sending trait: {trait.Id}, Exception: {ex.Message}");
                                //Mark to be Requeued
                                trait.FailedCount++;
                                trait.NextSending = trait.NextSending.AddMinutes(trait.FailedCount);
                                //cool off
                                NextCallBackInMilliseconds = 60000;
                            }
                            //Maximum Fails to Stop
                            int maxFailed = GetFailedTraitsCount();
                            if (maxFailed >= _options.MaxFailedToAbort)
                            {
                                _logger.LogWarning($"maximum failed traits ({maxFailed}) reached!, re-scheduling");
                                //cool off
                                NextCallBackInMilliseconds = 60 * 30 * 1000;
                                PendingTraits.Clear();
                            }
                        }
                    }

                }
            });
            AnalyticThread.Start();
        }


        private void TryConsumeServerResponse(string response, CancellationToken cancellationToken)
        {
            try
            {
                string[] segments = (response ?? string.Empty).Split('|');
                if (segments.Count() > 0 && int.TryParse(segments[0], out int nextCallback))
                {
                    _logger.LogInformation($"next callback in : {nextCallback:N0} milliseconds");
                    NextCallBackInMilliseconds = nextCallback;
                }
                //** future processing
            }
            catch (Exception ex)
            {
                _logger.LogError($"consuming server response failed: {ex.Message}");
            }
        }

        private void RequeueAllUnSent()
        {
            List<Trait> allUnsent = PendingTraits.Where(x => x.SentSuccesfully != true && x.FailedCount > 0).ToList();
            if (allUnsent.Count > 0)
            {
                _logger.LogInformation($"requeing ({allUnsent.Count()}) traits...");
                foreach (Trait trait in allUnsent)
                {
                    trait.NextSending = DateTime.Now.AddSeconds(trait.FailedCount);
                    trait.FailedCount = 0;
                }
            }
        }

        public int GetFailedTraitsCount() => PendingTraits.Where(x => x.SentSuccesfully != true && x.FailedCount > 0).Count();

        #region fast-crypt-string
        private static string ToStuEncodedString(List<Trait> readyToSendTraits)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Trait trait in readyToSendTraits)
            {
                stringBuilder.Append(trait.Key);
                stringBuilder.Append(" : ");
                stringBuilder.Append(trait.Value);
                stringBuilder.Append(Environment.NewLine);
            }
            return StuEncrypt(stringBuilder.ToString(), 2);
        }
        public static string StuEncrypt(string text, int k)
        {
            string response = string.Empty;
            int v;
            string vString;
            for (int loop = 0; loop < text.Length; loop++)
            {
                v = text.ToCharArray()[loop];
                v = v ^ k;
                vString = Convert.ToString(v, 16);
                vString = Padit(vString);
                response = response + vString;
            }
            response = response.ToUpper();
            return response;
        }
        private static string Padit(string x)
        {
            string tmp = x;
            while (tmp.Length < 2)
                tmp = '0' + x;
            return tmp;
        }
        #endregion
    }
}
