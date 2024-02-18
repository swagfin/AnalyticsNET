
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
            this.analyticsFailedLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startServiceBtn
            // 
            this.startServiceBtn.Location = new System.Drawing.Point(209, 8);
            this.startServiceBtn.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
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
            this.stopServiceBtn.Location = new System.Drawing.Point(354, 8);
            this.stopServiceBtn.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
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
            this.responseBox.Location = new System.Drawing.Point(0, 59);
            this.responseBox.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.responseBox.Multiline = true;
            this.responseBox.Name = "responseBox";
            this.responseBox.ReadOnly = true;
            this.responseBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.responseBox.Size = new System.Drawing.Size(1171, 533);
            this.responseBox.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.analyticsFailedLabel);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.stopServiceBtn);
            this.panel1.Controls.Add(this.startServiceBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1171, 59);
            this.panel1.TabIndex = 2;
            // 
            // analyticsFailedLabel
            // 
            this.analyticsFailedLabel.AutoSize = true;
            this.analyticsFailedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analyticsFailedLabel.ForeColor = System.Drawing.Color.Red;
            this.analyticsFailedLabel.Location = new System.Drawing.Point(566, 20);
            this.analyticsFailedLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.analyticsFailedLabel.Name = "analyticsFailedLabel";
            this.analyticsFailedLabel.Size = new System.Drawing.Size(14, 13);
            this.analyticsFailedLabel.TabIndex = 1;
            this.analyticsFailedLabel.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(478, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Failed Analytics:";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 592);
            this.Controls.Add(this.responseBox);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
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
    }
}

