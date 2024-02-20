## AnalyticsNET (Analytics Library for C#)

AnalyticsNET library is designed to empower your application with robust analytical capabilities, including data logging, error metrics logging, error reporting, and insightful metrics collection.

## Features

- **Easy Integration**: Inject the library into your application effortlessly and start collecting valuable analytics data right away.
- **Error Reporting**: Quickly identify and address errors with built-in error reporting functionality.
- **Customizable Options**: Tailor the library to your specific needs with customizable options for app settings, API endpoints, and more.
- **Device Heartbeats**: Keep track of your devices' health with automated device heartbeats.

## Installation

To get started AnalyticsNET, add it as a Nuget Library:

*NuGet Package*
```
nuget Install-Package AnalyticsNET
```

### Init Service

```cs
// Init service
var analyticService = new AnalyticsService(new AnalyticsOptions
{
    AppSecretKey = "<<someHashKeyForYourApp>>",
    AppName = "<<YourAppName>>",
    DeviceId = "<<device-id-or-macAddress>",
    AnalyticsAPIEndpoint = "https://your-analytics-api.com/",
    SendDeviceHeartBeats = true
});

// Start service
await analyticService.StartAsync();

// Track events
analyticService.Track("health", "app started successfully")
// Track events 
analyticService.Track("error", "Issue with Server, 404 Response");
```

### AspNetCore Integration

Library is also able to ran as a *Hosted Service* by creating a class and extending from IHostedService

```cs
public class AnalyticsNETHostedService : AnalyticsService, IHostedService
{
    public AnalyticsNETHostedService(AnalyticsOptions analyticsDeviceOptions, IAnalyticsLogger analyticsLogger = null)
    : base(analyticsDeviceOptions, analyticsLogger)
    {

    }
}
```

Then registering the class like this;

```cs
services.AddHostedService(svc => new AnalyticsNETHostedService(new AnalyticsOptions
{
    AppSecretKey = "<<someHashKeyForYourApp>>",
    AppName = "<<YourAppName>>",
    DeviceId = "<<device-id-or-macAddress>",
    AnalyticsAPIEndpoint = "https://your-analytics-api.com/",
    SendDeviceHeartBeats = true
}));
```


### Collecting Analytics Data

There is an example of an Analytics API that shows how you can receive metrics here 
[AnalyticsNet.API](https://github.com/swagfin/AnalyticsNET/tree/master/AnalyticsNET.API)

## Contributions
Contributions are welcome! If you have ideas for improvements, new features, or bug fixes, feel free to open an issue or submit a pull request on [Project Repository](https://github.com/swagfin/AnalyticsNET/tree/master/)

## To Do

I am actively adding more features as time goes by and here is the Project Milestones;
1. Integration with Prometheus and Grafana Dashboards
2. Ability to add Encryption and Decryption factoring in performance (Done)
3. More additional features...


## License
This project is licensed under the MIT License. See the [LICENSE](https://github.com/swagfin/AnalyticsNET/blob/master/LICENSE) file for details.
