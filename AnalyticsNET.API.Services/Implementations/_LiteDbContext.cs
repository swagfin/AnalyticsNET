using LiteDB;
using LiteDB.Async;
using System.IO;
using System;

namespace AnalyticsNET.API.Services.Implementations
{
    public class LiteDbContext : ILiteDbContext
    {
        private readonly string _dbDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DATA");
        public LiteDatabaseAsync Database { get; }
        public LiteDbContext()
        {
            Database = new LiteDatabaseAsync(new ConnectionString(Path.Combine(this._dbDirectory, "analytics.secured.db"))
            {
                Password = "12345678",
                Connection = ConnectionType.Shared
            });
            //Set Configurations
            Database.PragmaAsync("UTC_DATE", true).GetAwaiter().GetResult();
        }

        public string GetDbFolder() => _dbDirectory;
    }
}
