namespace AnalyticsNET
{
    /// <summary>
    /// Analytics Options 
    /// </summary>
    public class AnalyticsDeviceOptions
    {
        /// <summary>
        /// The Name of the Device sending the Analytics
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// The Device ID or Unique Generated Code for the Device
        /// </summary>
        public string DeviceID { get; set; }
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
        /// This is the Analytics Tracking ID, Assign this if your are retrieving it from a storage place
        /// </summary>
        public string CurrentAnalyticTrackingId { get; set; } = string.Empty;
        /// <summary>
        /// Track Device heartbeats. This tracks customer usage time usually every 3 seconds
        /// </summary>
        public bool TrackDeviceHeartBeat { get; set; } = true;
        /// <summary>
        /// Enable the Service to Automatically start without waiting for the main thread to call the StartService Function
        /// </summary>
        public bool StartServiceAutomatically { get; set; } = false;
        /// <summary>
        /// Override Analytics Server Endpoint, Leave Blank to use Default
        /// </summary>
        public string OverridedAnalyticsServerEndpoint { get; set; } = null;

    }
}
