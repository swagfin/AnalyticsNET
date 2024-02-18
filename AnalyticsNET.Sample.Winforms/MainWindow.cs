using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalyticsNET.Sample.Winforms
{
    public partial class MainWindow : Form
    {
        public AnalyticsService _analyticService;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private ConsoleLogger _analyticLogger;
        private Thread _monitorThread;

        public MainWindow()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this._cancellationTokenSource = new CancellationTokenSource();
            this._analyticLogger = new ConsoleLogger(responseBox);
        }

        private async void startServiceBtn_Click(object sender, EventArgs e)
        {
            startServiceBtn.Enabled = false;
            this._analyticService = new AnalyticsService(new AnalyticsOptions
            {
                AppSecretKey = "someHashHashKey12545678",
                AppName = "TestApp",
                DeviceID = "EMUD-A001-B001-C001-D001",
                AnalyticsAPIEndpoint = "https://localhost:7001/",
                SendDeviceHeartBeats = true,
                MaxFailedToAbort = 2
            }, _analyticLogger);

            //Start
            await this._analyticService.StartAsync(this._cancellationTokenSource.Token);
            stopServiceBtn.Enabled = true;
            //Simple Track
            this._analyticService.Track("health", "login Successfully");

            await Task.Delay(3000);
            this._analyticService.Track("error", "Issue with Server, 404 Response");
            //Monitor
            MonitorTraits();
        }

        private void MonitorTraits()
        {
            this._monitorThread = new Thread(() =>
            {
                while (true)
                {
                    if (this._analyticService != null)
                    {
                        analyticsFailedLabel.Text = string.Format("{0:N0}", this._analyticService.GetFailedTraitsCount());
                    }
                    Thread.Sleep(1000);
                }
            });
            this._monitorThread.Start();
        }

        private async void stopServiceBtn_Click(object sender, EventArgs e)
        {
            stopServiceBtn.Enabled = false;
            await this._analyticService.StopAsync(this._cancellationTokenSource.Token);
            this._monitorThread.Abort();
            startServiceBtn.Enabled = true;
        }
    }
    class ConsoleLogger : IAnalyticsLogger
    {
        public TextBox _textBox { get; }
        public ConsoleLogger(TextBox textBox)
        {
            _textBox = textBox;
            _textBox.Text = string.Empty;
        }


        public void LogError(string log)
        {
            _textBox.AppendText("ERROR>>>>>" + log + Environment.NewLine);
        }

        public void LogInformation(string log)
        {
            _textBox.AppendText(log + Environment.NewLine);
        }

        public void LogWarning(string log)
        {
            _textBox.AppendText(log + Environment.NewLine);
        }
    }
}
