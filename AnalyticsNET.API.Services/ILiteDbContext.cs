using LiteDB.Async;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnalyticsNET.API.Services
{
    public interface ILiteDbContext
    {
        string GetDbFolder();
        LiteDatabaseAsync Database { get; }
    }
}
