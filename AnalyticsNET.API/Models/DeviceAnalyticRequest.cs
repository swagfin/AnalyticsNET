using System.ComponentModel.DataAnnotations;

namespace AnalyticsNET.API.Models
{
    public class DeviceAnalyticRequest
    {
        [Required, StringLength(220)]
        public string AppName { get; set; }

        [Required, StringLength(220)]
        public string DeviceId { get; set; }

        [Required, StringLength(220)]
        public string DeviceName { get; set; }

        [Required, StringLength(50)]
        public string TraitKey { get; set; }
        public string TraitValue { get; set; }
    }
}
