using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using ImageProcessing;
namespace _20170509_RGB_2_HSI_HistogramEqualization
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap ori_image;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
            ori_image = pictureBox1.Image as Bitmap;
            pictureBox1.Image = ori_image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int w = ori_image.Width;
            int h = ori_image.Height;
            var imageRect = new Rectangle(0, 0, w, h);
            var ori_data = ori_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);
            var hsi_image = new Bitmap(imageRect.Width, imageRect.Height);
            var hsi_data = hsi_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);

            System.IntPtr ori_Ptr = ori_data.Scan0;
            System.IntPtr hsi_Ptr = hsi_data.Scan0;

            int ori_bytes = ori_data.Stride * h;
            int hsi_bytes = hsi_data.Stride * h;
            byte[] ori_Values = new byte[ori_bytes];
            byte[] hsi_Values = new byte[hsi_bytes];
            System.Runtime.InteropServices.Marshal.Copy(ori_Ptr, ori_Values, 0, ori_bytes);//複製GRB信息到byte數組
            System.Runtime.InteropServices.Marshal.Copy(hsi_Ptr, hsi_Values, 0, hsi_bytes);//複製GRB信息到byte數組
            var hsi_h = new List<int>();
            var hsi_s = new List<double>();
            var hsi_i = new List<double>();
            var count = new int[256];
            Method.RGB2HSI(ori_Values, h, w, ori_data.Stride, hsi_h, hsi_s, hsi_i, count); // RGB 轉 HSI
            var eqHist = new int[256];
            var count_eq = new int[256];
            int size = ori_bytes / 3;
            Method.HE(size, hsi_i, eqHist);
            Method.HSI2RGB(hsi_Values, eqHist, size, hsi_h, hsi_s, hsi_i, count_eq);
            System.Runtime.InteropServices.Marshal.Copy(hsi_Values, 0, hsi_Ptr, hsi_bytes);

            chart1.Series["Series1"].LegendText = "原圖的I";
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 255;
            for (int i = 0; i <= 255; i++)
            {
                chart1.Series["Series1"].Points.AddXY(i, count[i]);
            }
            chart2.Series["Series1"].LegendText = "I均衡化後";
            chart2.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            chart2.ChartAreas["ChartArea1"].AxisX.Maximum = 255;
            for (int i = 0; i <= 255; i++)
            {
                chart2.Series["Series1"].Points.AddXY(i, count_eq[i]);
            }
            hsi_image.UnlockBits(hsi_data);
            ori_image.UnlockBits(ori_data);
            pictureBox1.Image = ori_image;
            pictureBox2.Image = hsi_image;
        }
        
    }
}
