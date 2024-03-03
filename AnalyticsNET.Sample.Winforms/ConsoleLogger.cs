using System;
using System.Windows.Forms;

namespace AnalyticsNET.Sample.Winforms
{
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
