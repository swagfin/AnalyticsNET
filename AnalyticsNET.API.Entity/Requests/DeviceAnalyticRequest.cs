using System.ComponentModel.DataAnnotations;

namespace AnalyticsNET.API.Entity.Requests
{
    public class DeviceAnalyticRequest
    {
        public string SessionId { get; set; }
        [Required, StringLength(220)]
        public string DeviceId { get; set; }
        [Required, StringLength(220)]
        public string DeviceName { get; set; }
        [Required, StringLength(220)]
        public string TraitKey { get; set; }
        public string TraitValue { get; set; }
    }
}
