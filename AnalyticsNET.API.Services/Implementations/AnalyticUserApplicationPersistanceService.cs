using AnalyticsNET.API.Entity;
using LiteDB.Async;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AnalyticsNET.API.Services.Implementations
{
    public class AnalyticUserApplicationPersistanceService : IAnalyticUserApplicationPersistanceService
    {
        private readonly LiteDatabaseAsync _db;
        private static ConcurrentDictionary<string, AnalyticUserApplication> AnalyticApps { get; set; } = new ConcurrentDictionary<string, AnalyticUserApplication>();


        public AnalyticUserApplicationPersistanceService(ILiteDbContext context)
        {
            this._db = context.Database;
        }

        public async Task<AnalyticUserApplication> GetByAppSecretAsync(string appSecret)
        {
            if (AnalyticApps.TryGetValue(appSecret, out AnalyticUserApplication app))
                return app;
            app = await _db.GetCollection<AnalyticUserApplication>().Query().Where(x => x.AppSecretKey == appSecret).FirstOrDefaultAsync();
            if (app != null)
                AnalyticApps.TryAdd(appSecret, app);
            return app;
        }

        public async Task<bool> AddOrUpdateAsync(AnalyticUserApplication record)
        {
            return await _db.GetCollection<AnalyticUserApplication>().UpsertAsync(record);
        }
    }
}
