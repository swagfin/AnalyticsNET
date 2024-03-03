
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
            this.SendAnalyticGroupBox = new System.Windows.Forms.GroupBox();
            this.TrackAnalyticBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TraitValueBox = new System.Windows.Forms.TextBox();
            this.TraitBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.GenerateRndMetricLink = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.SendAnalyticGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // startServiceBtn
            // 
            this.startServiceBtn.ForeColor = System.Drawing.Color.DarkGreen;
            this.startServiceBtn.Location = new System.Drawing.Point(10, 22);
            this.startServiceBtn.Margin = new System.Windows.Forms.Padding(1);
            this.startServiceBtn.Name = "startServiceBtn";
            this.startServiceBtn.Size = new System.Drawing.Size(105, 40);
            this.startServiceBtn.TabIndex = 0;
            this.startServiceBtn.Text = "Start Service";
            this.startServiceBtn.UseVisualStyleBackColor = true;
            this.startServiceBtn.Click += new System.EventHandler(this.startServiceBtn_Click);
            // 
            // stopServiceBtn
            // 
            this.stopServiceBtn.Enabled = false;
            this.stopServiceBtn.ForeColor = System.Drawing.Color.DarkRed;
            this.stopServiceBtn.Location = new System.Drawing.Point(135, 22);
            this.stopServiceBtn.Margin = new System.Windows.Forms.Padding(1);
            this.stopServiceBtn.Name = "stopServiceBtn";
            this.stopServiceBtn.Size = new System.Drawing.Size(105, 40);
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
            this.responseBox.Location = new System.Drawing.Point(0, 77);
            this.responseBox.Margin = new System.Windows.Forms.Padding(1);
            this.responseBox.Multiline = true;
            this.responseBox.Name = "responseBox";
            this.responseBox.ReadOnly = true;
            this.responseBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.responseBox.Size = new System.Drawing.Size(1171, 515);
            this.responseBox.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SendAnalyticGroupBox);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1171, 77);
            this.panel1.TabIndex = 2;
            // 
            // SendAnalyticGroupBox
            // 
            this.SendAnalyticGroupBox.Controls.Add(this.GenerateRndMetricLink);
            this.SendAnalyticGroupBox.Controls.Add(this.TrackAnalyticBtn);
            this.SendAnalyticGroupBox.Controls.Add(this.label2);
            this.SendAnalyticGroupBox.Controls.Add(this.label1);
            this.SendAnalyticGroupBox.Controls.Add(this.TraitValueBox);
            this.SendAnalyticGroupBox.Controls.Add(this.TraitBox);
            this.SendAnalyticGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SendAnalyticGroupBox.Enabled = false;
            this.SendAnalyticGroupBox.Location = new System.Drawing.Point(275, 0);
            this.SendAnalyticGroupBox.Name = "SendAnalyticGroupBox";
            this.SendAnalyticGroupBox.Size = new System.Drawing.Size(896, 77);
            this.SendAnalyticGroupBox.TabIndex = 2;
            this.SendAnalyticGroupBox.TabStop = false;
            this.SendAnalyticGroupBox.Text = "Send Analytic";
            // 
            // TrackAnalyticBtn
            // 
            this.TrackAnalyticBtn.ForeColor = System.Drawing.Color.DarkGreen;
            this.TrackAnalyticBtn.Location = new System.Drawing.Point(486, 44);
            this.TrackAnalyticBtn.Name = "TrackAnalyticBtn";
            this.TrackAnalyticBtn.Size = new System.Drawing.Size(79, 29);
            this.TrackAnalyticBtn.TabIndex = 3;
            this.TrackAnalyticBtn.Text = "Track";
            this.TrackAnalyticBtn.UseVisualStyleBackColor = true;
            this.TrackAnalyticBtn.Click += new System.EventHandler(this.TrackAnalyticBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Trait Value";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Trait";
            // 
            // TraitValueBox
            // 
            this.TraitValueBox.BackColor = System.Drawing.SystemColors.Info;
            this.TraitValueBox.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TraitValueBox.Location = new System.Drawing.Point(146, 48);
            this.TraitValueBox.Name = "TraitValueBox";
            this.TraitValueBox.Size = new System.Drawing.Size(320, 23);
            this.TraitValueBox.TabIndex = 1;
            this.TraitValueBox.Text = "Logged in successfully";
            // 
            // TraitBox
            // 
            this.TraitBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.TraitBox.Font = new System.Drawing.Font("Bahnschrift", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TraitBox.Location = new System.Drawing.Point(22, 48);
            this.TraitBox.Name = "TraitBox";
            this.TraitBox.Size = new System.Drawing.Size(100, 23);
            this.TraitBox.TabIndex = 1;
            this.TraitBox.Text = "User";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.stopServiceBtn);
            this.groupBox2.Controls.Add(this.startServiceBtn);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(275, 77);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Service";
            // 
            // GenerateRndMetricLink
            // 
            this.GenerateRndMetricLink.AutoSize = true;
            this.GenerateRndMetricLink.Location = new System.Drawing.Point(267, 16);
            this.GenerateRndMetricLink.Name = "GenerateRndMetricLink";
            this.GenerateRndMetricLink.Size = new System.Drawing.Size(149, 16);
            this.GenerateRndMetricLink.TabIndex = 4;
            this.GenerateRndMetricLink.TabStop = true;
            this.GenerateRndMetricLink.Text = "Generate Random Metric";
            this.GenerateRndMetricLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GenerateRndMetricLink_LinkClicked);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 592);
            this.Controls.Add(this.responseBox);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MAIN WINDOW";
            this.panel1.ResumeLayout(false);
            this.SendAnalyticGroupBox.ResumeLayout(false);
            this.SendAnalyticGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startServiceBtn;
        private System.Windows.Forms.Button stopServiceBtn;
        private System.Windows.Forms.TextBox responseBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox SendAnalyticGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TraitBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button TrackAnalyticBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TraitValueBox;
        private System.Windows.Forms.LinkLabel GenerateRndMetricLink;
    }
}

