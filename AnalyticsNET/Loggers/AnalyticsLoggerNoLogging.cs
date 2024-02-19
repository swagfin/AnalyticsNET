namespace AnalyticsNET
{
    public class AnalyticsLoggerNoLogging : IAnalyticsLogger
    {
        public virtual void LogInformation(string log)
        {
        }
        public virtual void LogWarning(string log)
        {
        }
        public virtual void LogError(string log)
        {
        }
    }
}
