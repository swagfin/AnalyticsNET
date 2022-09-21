using System;
using System.ComponentModel.DataAnnotations;

namespace AnalyticsNET.API.Entity
{
    public class AnalyticUserApplication
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid AnalyticUserId { get; set; }
        [StringLength(255)]
        public string AppName { get; set; } = string.Empty;
        public string AppDescription { get; set; } = string.Empty;
        [Required, StringLength(255)]
        public string AppSecretKey { get; set; }
        public DateTime LastAppSecretKeyChangedUtc { get; set; } = DateTime.UtcNow;
        public DateTime DateRegisteredUtc { get; set; } = DateTime.UtcNow;
        public AppStatus AppStatus { get; set; } = AppStatus.Active;

    }
    public enum AppStatus
    {
        Pending,
        Active,
        Disabled
    }
}
