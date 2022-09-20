using AnalyticsNET.Logic;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalyticsNET.Sample.Winforms
{
    public partial class MainWindow : Form
    {
        public AnalyticsService _analyticService;
        private ConsoleLogger _analyticLogger;
        private Thread _monitorThread;

        public MainWindow()
        {
            CheckForIllegalCrossThreadCalls = false; //DON'T USE THIS IN PRODUCTION
            InitializeComponent();
            _analyticLogger = new ConsoleLogger(responseBox);
        }

        private async void startServiceBtn_Click(object sender, EventArgs e)
        {

            this._analyticService = new AnalyticsService(new AnalyticsDeviceOptions
            {
                AppSecretKey = "someHashHashKey12545678",
                DeviceID = "EMUD-A001-B001-C001-D001",
                DeviceName = Environment.MachineName,
                TrackDeviceHeartBeat = true,
                AnalyticsAPIEndpoint = "http://localhost/index.php"
            }, _analyticLogger);

            //Start
            this._analyticService.StartService();
            startServiceBtn.Enabled = false;
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
                        sentAnalyticLabel.Text = string.Format("{0:N0}", this._analyticService.GetSentTraitsCount());
                    }
                    Thread.Sleep(1000);
                }
            });
            this._monitorThread.Start();
        }

        private void stopServiceBtn_Click(object sender, EventArgs e)
        {
            this._analyticService.Dispose();
            this._monitorThread.Abort();
            stopServiceBtn.Enabled = false;
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
