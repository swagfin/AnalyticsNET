using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this._cancellationTokenSource = new CancellationTokenSource();
            this._analyticLogger = new ConsoleLogger(responseBox);
        }

        private async void startServiceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                startServiceBtn.Enabled = false;
                this._analyticService = new AnalyticsService(new AnalyticsOptions
                {
                    AppSecretKey = "someHashHashKey125456",
                    AppName = "TestApp",
                    DeviceId = "EMUD-A001-B001-C001-D001",
                    AnalyticsAPIEndpoint = "https://localhost:5001/api/Analytics",
                    SendDeviceHeartBeats = true,
                    MaxFailedToAbort = 2,
                    InitialCallBackInMilliseconds = 5000
                }, _analyticLogger);

                //Start
                await this._analyticService.StartAsync(this._cancellationTokenSource.Token);
                stopServiceBtn.Enabled = true;
                SendAnalyticGroupBox.Enabled = true;
                //Simple Track
                this._analyticService.Track("Health", "Application initialized");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void stopServiceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                stopServiceBtn.Enabled = false;
                await this._analyticService.StopAsync(default);
                startServiceBtn.Enabled = true;
                SendAnalyticGroupBox.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void TrackAnalyticBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!stopServiceBtn.Enabled)
                    return;
                else if (string.IsNullOrEmpty(TraitBox.Text.Trim()))
                {
                    TraitBox.Select();
                    return;
                }
                else if (string.IsNullOrEmpty(TraitValueBox.Text.Trim()))
                {
                    TraitValueBox.Select();
                    return;
                }
                TrackAnalyticBtn.Enabled = false;
                //queue trait
                this._analyticService.Track(TraitBox.Text.Trim(), TraitValueBox.Text.Trim());
                await Task.Delay(500);
                TrackAnalyticBtn.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void GenerateRndMetricLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GenerateAndAssignRandomMetric();
        }

        private void GenerateAndAssignRandomMetric()
        {
            KeyValuePair<string, string> metricExample = MetricsExamples.GetRandomMetric();
            TraitBox.Text = metricExample.Key;
            TraitValueBox.Text = metricExample.Value;
        }
    }
}
