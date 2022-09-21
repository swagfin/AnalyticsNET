using System;
using System.ComponentModel.DataAnnotations;

namespace AnalyticsNET.API.Entity
{
    public class AnalyticUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [StringLength(255)]
        public string FullName { get; set; } = string.Empty;
        [StringLength(90)]
        public string MobilePhone { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public int InvalidLogins { get; set; } = 0;
        public string ProfileImage { get; set; } = "no_image.png";

        public DateTime? LastLogin { get; set; }

        [StringLength(255)]
        public string OauthKey { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsMobilePhoneConfirmed { get; set; } = false;
        public DateTime DateRegisteredUtc { get; set; } = DateTime.UtcNow;
    }
}
