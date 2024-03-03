using System;
using System.Collections.Generic;

namespace AnalyticsNET.Sample.Winforms
{
    internal class MetricsExamples
    {

        public static KeyValuePair<string, string> GetRandomMetric()
        {
            return Metrics.ToArray()[new Random().Next(0, Metrics.Count - 1)];
        }



        public static List<KeyValuePair<string, string>> Metrics = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("User", "Registered successfully"),
            new KeyValuePair<string, string>("User", "Completed profile"),
            new KeyValuePair<string, string>("User", "Started session"),
            new KeyValuePair<string, string>("User", "Viewed page/screen"),
            new KeyValuePair<string, string>("User", "Performed search query"),
            new KeyValuePair<string, string>("User", "Added to favorites/bookmarks"),
            new KeyValuePair<string, string>("User", "Submitted feedback/contacted support"),
            new KeyValuePair<string, string>("User", "Shared content on social media"),
            new KeyValuePair<string, string>("User", "Downloaded/installed application"),
            new KeyValuePair<string, string>("User", "Made in-app purchase"),
            new KeyValuePair<string, string>("User", "Used specific feature"),
            new KeyValuePair<string, string>("User", "Encountered error/exception"),
            new KeyValuePair<string, string>("User", "Changed application settings"),
            new KeyValuePair<string, string>("User", "Accessed application from device/platform"),
            new KeyValuePair<string, string>("User", "Engaged with content (likes/comments/shares)"),
            new KeyValuePair<string, string>("System", "Encountered error/exception"),
            new KeyValuePair<string, string>("System", "Started successfully"),
            new KeyValuePair<string, string>("System", "Shut down unexpectedly"),
            new KeyValuePair<string, string>("System", "Deployed new version"),
            new KeyValuePair<string, string>("System", "Reached memory threshold"),
            new KeyValuePair<string, string>("System", "Reached CPU threshold"),
            new KeyValuePair<string, string>("System", "Disk space low"),
            new KeyValuePair<string, string>("System", "Backup completed"),
            new KeyValuePair<string, string>("System", "Database connection lost/recovered"),
            new KeyValuePair<string, string>("System", "Security breach detected"),
            new KeyValuePair<string, string>("System", "Service outage detected/resolved"),
            new KeyValuePair<string, string>("System", "Performance degradation detected/resolved"),
            new KeyValuePair<string, string>("System", "Scheduled maintenance completed"),
            new KeyValuePair<string, string>("System", "Certificate expired/renewed"),
            new KeyValuePair<string, string>("System", "Configuration change applied"),
            new KeyValuePair<string, string>("System", "Resource allocation changed"),
            new KeyValuePair<string, string>("System", "Network connectivity lost/recovered")
        };

    }
}
