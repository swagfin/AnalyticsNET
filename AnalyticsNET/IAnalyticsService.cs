using System.Collections.Generic;

namespace AnalyticsNET
{
    public interface IAnalyticsService
    {
        string GetAnalyticsStatus();
        string GetCurrentSessionTrackingID();
        int GetFailedTraitsCount();
        int GetSentTraitsCount();
        void StartService();
        void StopService();
        void Track(IList<Trait> traits);
        void Track(string traitKey, string traitValue);
        void Track(Trait trait);
    }
}