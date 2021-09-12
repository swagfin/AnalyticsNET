
namespace AnalyticsNET.Sample.Winforms
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startServiceBtn = new System.Windows.Forms.Button();
            this.stopServiceBtn = new System.Windows.Forms.Button();
            this.responseBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.analyticsFailedLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.sentAnalyticLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startServiceBtn
            // 
            this.startServiceBtn.Location = new System.Drawing.Point(378, 18);
            this.startServiceBtn.Name = "startServiceBtn";
            this.startServiceBtn.Size = new System.Drawing.Size(279, 95);
            this.startServiceBtn.TabIndex = 0;
            this.startServiceBtn.Text = "Start Service";
            this.startServiceBtn.UseVisualStyleBackColor = true;
            this.startServiceBtn.Click += new System.EventHandler(this.startServiceBtn_Click);
            // 
            // stopServiceBtn
            // 
            this.stopServiceBtn.Enabled = false;
            this.stopServiceBtn.Location = new System.Drawing.Point(765, 18);
            this.stopServiceBtn.Name = "stopServiceBtn";
            this.stopServiceBtn.Size = new System.Drawing.Size(279, 95);
            this.stopServiceBtn.TabIndex = 0;
            this.stopServiceBtn.Text = "Stop Service";
            this.stopServiceBtn.UseVisualStyleBackColor = true;
            this.stopServiceBtn.Click += new System.EventHandler(this.stopServiceBtn_Click);
            // 
            // responseBox
            // 
            this.responseBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.responseBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.responseBox.ForeColor = System.Drawing.Color.White;
            this.responseBox.Location = new System.Drawing.Point(0, 140);
            this.responseBox.Multiline = true;
            this.responseBox.Name = "responseBox";
            this.responseBox.ReadOnly = true;
            this.responseBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.responseBox.Size = new System.Drawing.Size(1942, 975);
            this.responseBox.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sentAnalyticLabel);
            this.panel1.Controls.Add(this.analyticsFailedLabel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.stopServiceBtn);
            this.panel1.Controls.Add(this.startServiceBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1942, 140);
            this.panel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(1096, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Failed Analytics:";
            // 
            // analyticsFailedLabel
            // 
            this.analyticsFailedLabel.AutoSize = true;
            this.analyticsFailedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analyticsFailedLabel.ForeColor = System.Drawing.Color.Red;
            this.analyticsFailedLabel.Location = new System.Drawing.Point(1331, 73);
            this.analyticsFailedLabel.Name = "analyticsFailedLabel";
            this.analyticsFailedLabel.Size = new System.Drawing.Size(32, 32);
            this.analyticsFailedLabel.TabIndex = 1;
            this.analyticsFailedLabel.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(1096, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sent Analytics:";
            // 
            // sentAnalyticLabel
            // 
            this.sentAnalyticLabel.AutoSize = true;
            this.sentAnalyticLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sentAnalyticLabel.ForeColor = System.Drawing.Color.Green;
            this.sentAnalyticLabel.Location = new System.Drawing.Point(1331, 28);
            this.sentAnalyticLabel.Name = "sentAnalyticLabel";
            this.sentAnalyticLabel.Size = new System.Drawing.Size(32, 32);
            this.sentAnalyticLabel.TabIndex = 1;
            this.sentAnalyticLabel.Text = "0";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1942, 1115);
            this.Controls.Add(this.responseBox);
            this.Controls.Add(this.panel1);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MAIN WINDOW";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startServiceBtn;
        private System.Windows.Forms.Button stopServiceBtn;
        private System.Windows.Forms.TextBox responseBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label analyticsFailedLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label sentAnalyticLabel;
        private System.Windows.Forms.Label label1;
    }
}

