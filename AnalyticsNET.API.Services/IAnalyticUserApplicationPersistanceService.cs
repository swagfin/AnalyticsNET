using AnalyticsNET.API.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsNET.API.Services
{
    public interface IAnalyticUserApplicationPersistanceService
    {
        Task<bool> AddOrUpdateAsync(AnalyticUserApplication record);
        Task<AnalyticUserApplication> GetByAppSecretAsync(string appSecret);
    }
}
