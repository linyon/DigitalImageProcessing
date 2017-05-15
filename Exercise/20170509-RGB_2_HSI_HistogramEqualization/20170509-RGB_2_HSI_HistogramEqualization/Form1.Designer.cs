namespace _20170509_RGB_2_HSI_HistogramEqualization
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.開檔ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.開檔ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.直方圖ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.均衡化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.縮小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.擴展ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(494, 376);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // chart1
            // 
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(12, 428);
            this.chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(494, 381);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(512, 30);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(494, 376);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // chart2
            // 
            chartArea4.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart2.Legends.Add(legend4);
            this.chart2.Location = new System.Drawing.Point(512, 428);
            this.chart2.Name = "chart2";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chart2.Series.Add(series4);
            this.chart2.Size = new System.Drawing.Size(494, 381);
            this.chart2.TabIndex = 7;
            this.chart2.Text = "chart2";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.開檔ToolStripMenuItem,
            this.直方圖ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1243, 28);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 開檔ToolStripMenuItem
            // 
            this.開檔ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.開檔ToolStripMenuItem1});
            this.開檔ToolStripMenuItem.Name = "開檔ToolStripMenuItem";
            this.開檔ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.開檔ToolStripMenuItem.Text = "選項";
            // 
            // 開檔ToolStripMenuItem1
            // 
            this.開檔ToolStripMenuItem1.Name = "開檔ToolStripMenuItem1";
            this.開檔ToolStripMenuItem1.Size = new System.Drawing.Size(114, 26);
            this.開檔ToolStripMenuItem1.Text = "開檔";
            this.開檔ToolStripMenuItem1.Click += new System.EventHandler(this.開檔ToolStripMenuItem1_Click);
            // 
            // 直方圖ToolStripMenuItem
            // 
            this.直方圖ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.均衡化ToolStripMenuItem,
            this.縮小ToolStripMenuItem,
            this.擴展ToolStripMenuItem});
            this.直方圖ToolStripMenuItem.Name = "直方圖ToolStripMenuItem";
            this.直方圖ToolStripMenuItem.Size = new System.Drawing.Size(66, 24);
            this.直方圖ToolStripMenuItem.Text = "直方圖";
            this.直方圖ToolStripMenuItem.Click += new System.EventHandler(this.直方圖ToolStripMenuItem_Click);
            // 
            // 均衡化ToolStripMenuItem
            // 
            this.均衡化ToolStripMenuItem.Name = "均衡化ToolStripMenuItem";
            this.均衡化ToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.均衡化ToolStripMenuItem.Text = "均衡化";
            this.均衡化ToolStripMenuItem.Click += new System.EventHandler(this.均衡化ToolStripMenuItem_Click);
            // 
            // 縮小ToolStripMenuItem
            // 
            this.縮小ToolStripMenuItem.Name = "縮小ToolStripMenuItem";
            this.縮小ToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.縮小ToolStripMenuItem.Text = "縮小";
            this.縮小ToolStripMenuItem.Click += new System.EventHandler(this.縮小ToolStripMenuItem_Click);
            // 
            // 擴展ToolStripMenuItem
            // 
            this.擴展ToolStripMenuItem.Name = "擴展ToolStripMenuItem";
            this.擴展ToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.擴展ToolStripMenuItem.Text = "擴展";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1243, 821);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 開檔ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 直方圖ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 均衡化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 開檔ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 縮小ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 擴展ToolStripMenuItem;
    }
}

