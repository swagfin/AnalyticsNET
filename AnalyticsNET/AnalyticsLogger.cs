using System;

namespace AnalyticsNET
{
    public class AnalyticsLogger : IAnalyticsLogger
    {
        public virtual void LogInformation(string log)
        {
            Console.WriteLine($"{Environment.NewLine} {log} At {DateTime.Now:yyyy-MM-dd hh:mm:ss}");
        }

        public virtual void LogWarning(string log)
        {
            Console.WriteLine($"WARNING: {Environment.NewLine} {log} At {DateTime.Now:yyyy-MM-dd hh:mm:ss}");
        }
        public virtual void LogError(string log)
        {
            Console.WriteLine($"ERROR: {Environment.NewLine} {log} At {DateTime.Now:yyyy-MM-dd hh:mm:ss}");
        }
    }
}
