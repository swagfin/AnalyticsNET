using AnalyticsNET.API.Entity;
using AnalyticsNET.API.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnalyticsNET.API.Services.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void UseAnalyticsNETServices(this IServiceCollection services)
        {
            //Register Database Context
            services.AddSingleton<ILiteDbContext, LiteDbContext>();
            //Persistance
            services.AddScoped<IAnalyticUserApplicationPersistanceService, AnalyticUserApplicationPersistanceService>();
        }


        public static void InitAnalyticsNETServices(this IApplicationBuilder builder)
        {
            var processorService = (IEnumerable<IProcessorInitializable>)builder.ApplicationServices.GetService(typeof(IEnumerable<IProcessorInitializable>));
            if (processorService != null)
                foreach (IProcessorInitializable processor in processorService)
                    processor.Initialize();
            //Ensure Directory Exists
            ILiteDbContext dbService = (ILiteDbContext)builder.ApplicationServices.GetService(typeof(ILiteDbContext));
            if (!Directory.Exists(dbService.GetDbFolder()))
                Directory.CreateDirectory(dbService.GetDbFolder());
        }
        //For Development Purposes
        public static void CreateAnalyticsTestAppForDevelopment(this IApplicationBuilder builder)
        {
            //Since the ILiteDb is singleton, we can use it directly without the Service responsible for UserApp Crud
            ILiteDbContext dbService = (ILiteDbContext)builder.ApplicationServices.GetService(typeof(ILiteDbContext));
            AnalyticUserApplication defaultApp = new AnalyticUserApplication
            {
                Id = Guid.Parse("{76c5e8b3-d805-4391-907b-138176c4d5b8}"),
                AnalyticUserId = Guid.NewGuid(),
                AppDescription = "Test and Debugging Application",
                AppName = "TestApp",
                AppSecretKey = "someHashHashKey12545678",
                AppStatus = AppStatus.Active,
                DateRegisteredUtc = DateTime.UtcNow,
                LastAppSecretKeyChangedUtc = DateTime.UtcNow,
            };
            dbService.Database.GetCollection<AnalyticUserApplication>().UpsertAsync(defaultApp);
        }
    }
}
