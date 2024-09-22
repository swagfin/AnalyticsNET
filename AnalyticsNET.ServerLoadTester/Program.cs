using System.Diagnostics;

namespace AnalyticsNET.ServerLoadTester
{
    class Program
    {
        public static CancellationTokenSource CancellationTokenSource = new();
        static async Task Main(string[] args)
        {
            int numberOfDevices = 50; // Number of unique devices
            int waitTimeInMilliseconds = 200; // Time to wait before sending another call
            string endpoint = "https://localhost:5001/api/analytics"; // Your server endpoint
            string appName = "TestApp";
            string appSecret = "someHashHashKey125456";
            string[] traitValues = ["Value1", "Value2", "Value3"];

            List<Task> tasks = [];
            for (int i = 0; i < numberOfDevices; i++)
            {
                int deviceIndex = i;
                tasks.Add(Task.Run(async () =>
                {
                    //create device http client
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Add("AppSecret", appSecret);
                    //proceed
                    while (!CancellationTokenSource.Token.IsCancellationRequested)
                    {
                        try
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Dictionary<string, string> parameters = new()
                            {
                                { "appName",appName },
                                { "appVersion", "1.0.0.1" },
                                { "deviceId", $"Device{deviceIndex:D3}" },
                                { "deviceName", $"DeviceName{deviceIndex:D3}" },
                                { "traitKey", "cryptData" },
                                { "traitValue",AnalyticsService.StuEncrypt(traitValues[Random.Shared.Next(traitValues.Length)],2) }
                            };
                            //Sending POST Request
                            //post request
                            Stopwatch stopwatch = Stopwatch.StartNew();
                            HttpResponseMessage response = await httpClient.PostAsync(endpoint, new FormUrlEncodedContent(parameters), CancellationTokenSource.Token);
                            stopwatch.Stop();
                            if (!response.IsSuccessStatusCode)
                                throw new HttpRequestException($"DeviceId: Device{deviceIndex:D3}, Error: {await response.Content.ReadAsStringAsync()}, Response Time: {stopwatch.ElapsedMilliseconds} ms");
                            //logg
                            Console.WriteLine($"DeviceId: Device{deviceIndex:D3}, Response: {response.StatusCode}, Response Time: {stopwatch.ElapsedMilliseconds} ms");
                            await Task.Delay(waitTimeInMilliseconds);
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }
    }

}
