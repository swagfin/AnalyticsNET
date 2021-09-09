namespace AnalyticsNET
{
    public class AnalyticsDeviceOptions
    {
        public string DeviceName { get; set; }
        public string DeviceID { get; set; }
        public string AppName { get; set; }
        public string AppSecretKey { get; set; }
        public int MaxFailedToAbort { get; set; } = 50;
        public string CurrentAnalyticTrackingId { get; set; } = string.Empty;
        public bool TrackDeviceHeartBeat { get; set; } = true;
    }
}
