namespace DateTime
{
    partial class MainForm
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
            this.lblDate = new System.Windows.Forms.Label();
            this.lblYi = new System.Windows.Forms.Label();
            this.lblJi = new System.Windows.Forms.Label();
            this.lblFortune = new System.Windows.Forms.Label();
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDate.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDate.Location = new System.Drawing.Point(28, 114);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(62, 31);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "日期";
            this.lblDate.DoubleClick += new System.EventHandler(this.lblDate_DoubleClick);
            // 
            // lblYi
            // 
            this.lblYi.AutoSize = true;
            this.lblYi.BackColor = System.Drawing.Color.Transparent;
            this.lblYi.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblYi.ForeColor = System.Drawing.Color.Green;
            this.lblYi.Location = new System.Drawing.Point(28, 177);
            this.lblYi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblYi.Name = "lblYi";
            this.lblYi.Size = new System.Drawing.Size(52, 27);
            this.lblYi.TabIndex = 1;
            this.lblYi.Text = "宜：";
            // 
            // lblJi
            // 
            this.lblJi.AutoSize = true;
            this.lblJi.BackColor = System.Drawing.Color.Transparent;
            this.lblJi.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblJi.ForeColor = System.Drawing.Color.Red;
            this.lblJi.Location = new System.Drawing.Point(28, 240);
            this.lblJi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblJi.Name = "lblJi";
            this.lblJi.Size = new System.Drawing.Size(52, 27);
            this.lblJi.TabIndex = 2;
            this.lblJi.Text = "忌：";
            // 
            // lblFortune
            // 
            this.lblFortune.AutoSize = true;
            this.lblFortune.BackColor = System.Drawing.Color.Transparent;
            this.lblFortune.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFortune.Location = new System.Drawing.Point(28, 327);
            this.lblFortune.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFortune.Name = "lblFortune";
            this.lblFortune.Size = new System.Drawing.Size(231, 36);
            this.lblFortune.TabIndex = 3;
            this.lblFortune.Text = "今日运势：待抽取";
            // 
            // uiPanel1
            // 
            this.uiPanel1.BackColor = System.Drawing.Color.Transparent;
            this.uiPanel1.FillColor = System.Drawing.Color.Transparent;
            this.uiPanel1.FillColor2 = System.Drawing.Color.Transparent;
            this.uiPanel1.FillDisableColor = System.Drawing.Color.Empty;
            this.uiPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.ForeColor = System.Drawing.Color.Empty;
            this.uiPanel1.ForeDisableColor = System.Drawing.Color.Empty;
            this.uiPanel1.Location = new System.Drawing.Point(2, 0);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.RectColor = System.Drawing.Color.Transparent;
            this.uiPanel1.RectDisableColor = System.Drawing.Color.Transparent;
            this.uiPanel1.Size = new System.Drawing.Size(598, 86);
            this.uiPanel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiPanel1.TabIndex = 5;
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(28, 417);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 54);
            this.label1.TabIndex = 3;
            this.label1.Text = "抽取今日运势";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.btnDrawLot_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = global::DateTime.Properties.Resources.黄历运势2;
            this.ClientSize = new System.Drawing.Size(600, 565);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblFortune);
            this.Controls.Add(this.lblJi);
            this.Controls.Add(this.lblYi);
            this.Controls.Add(this.lblDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "黄历";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblYi;
        private System.Windows.Forms.Label lblJi;
        private System.Windows.Forms.Label lblFortune;
        private Sunny.UI.UIPanel uiPanel1;
        private System.Windows.Forms.Label label1;
    }
}