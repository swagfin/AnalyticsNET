namespace AnalyticsNET
{
    public interface IAnalyticsLogger
    {
        void LogError(string log);
        void LogInformation(string log);
        void LogWarning(string log);
    }
}