using System.Collections.Generic;

namespace AnalyticsNET
{
    /// <summary>
    /// Analytics Options 
    /// </summary>
    public class AnalyticsOptions
    {
        /// <summary>
        /// Analytics Server API Endpoint (Required)
        /// </summary>
        public string AnalyticsAPIEndpoint { get; set; } = null;
        /// <summary>
        /// The Device ID or Unique Generated Code for the Device
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// The Software Application Name. Usually the current app being used
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// The Secret Key to Authenticate Application
        /// </summary>
        public string AppSecretKey { get; set; }
        /// <summary>
        /// The Maximum Failed Analytics Delivery Requests
        /// </summary>
        public int MaxFailedToAbort { get; set; } = 50;
        /// <summary>
        /// Additional Request Headers to be appended in the Header of the POST Request
        /// </summary>
        public Dictionary<string, string> DefaultRequestHeaders { get; set; }
        /// <summary>
        /// Enable Heartbeats | Sends heartbeat signals
        /// </summary>
        public bool SendDeviceHeartBeats { get; set; } = true;
        /// <summary>
        /// Initial wait time before the Service can sent its first analytics, Default is 10 seconds = 10,000 milliseconds
        /// </summary>
        public int InitialCallBackInMilliseconds { get; set; } = 30000;
    }
}
