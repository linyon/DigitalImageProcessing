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
        BitmapData ori_data;
        Rectangle imageRect;
        Bitmap hsi_image;
        BitmapData hsi_data;
        List<int> hsi_h = new List<int>();
        List<double> hsi_s = new List<double>();
        List<double> hsi_i = new List<double>();
        byte[] ori_Values;
        int ori_bytes;
        byte[] hsi_Values;
        int hsi_bytes;
        int[] count = new int[256];


        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void 均衡化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int w = ori_image.Width;
            int h = ori_image.Height;

            //ori_data = ori_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);
            hsi_data = hsi_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);

            //System.IntPtr ori_Ptr = ori_data.Scan0;
            System.IntPtr hsi_Ptr = hsi_data.Scan0;

            //ori_bytes = ori_data.Stride * h;
            //int hsi_bytes = hsi_data.Stride * h;
            //ori_Values = new byte[ori_bytes];
            //hsi_Values = new byte[hsi_bytes];
            //System.Runtime.InteropServices.Marshal.Copy(ori_Ptr, ori_Values, 0, ori_bytes);//複製GRB信息到byte數組
            System.Runtime.InteropServices.Marshal.Copy(hsi_Ptr, hsi_Values, 0, hsi_bytes);//複製GRB信息到byte數組

            //var count = new int[256];
            //Method.RGB2HSI(ori_Values, h, w, ori_data.Stride, hsi_h, hsi_s, hsi_i, count); // RGB 轉 HSI
            var eqHist = new int[256];
            var count_eq = new int[256];
            int size = ori_bytes / 3;
            Method.HE(size, hsi_i, eqHist);
            Method.HSI2RGB(hsi_Values, eqHist, size, hsi_h, hsi_s, hsi_i, count_eq);
            System.Runtime.InteropServices.Marshal.Copy(hsi_Values, 0, hsi_Ptr, hsi_bytes);


            chart2.Series["Series1"].LegendText = "I均衡化後";
            chart2.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            chart2.ChartAreas["ChartArea1"].AxisX.Maximum = 255;
            for (int i = 0; i <= 255; i++)
            {
                chart2.Series["Series1"].Points.AddXY(i, count_eq[i]);
            }
            hsi_image.UnlockBits(hsi_data);
            //ori_image.UnlockBits(ori_data);
            pictureBox1.Image = ori_image;
            pictureBox2.Image = hsi_image;
        }

        private void 開檔ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
                hsi_h.Clear();
                hsi_s.Clear();
                hsi_i.Clear();
            }
            ori_image = pictureBox1.Image as Bitmap;
            pictureBox1.Image = ori_image;

            imageRect = new Rectangle(0, 0, ori_image.Width, ori_image.Height);
            ori_data = ori_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);
            hsi_image = new Bitmap(imageRect.Width, imageRect.Height);
            hsi_data = hsi_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);
            System.IntPtr ori_Ptr = ori_data.Scan0;

            ori_bytes = ori_data.Stride * ori_image.Height;
            hsi_bytes = hsi_data.Stride * ori_image.Height;
            ori_Values = new byte[ori_bytes];
            hsi_Values = new byte[hsi_bytes];
            System.Runtime.InteropServices.Marshal.Copy(ori_Ptr, ori_Values, 0, ori_bytes);//複製GRB信息到byte數組
            Method.RGB2HSI(ori_Values, ori_image.Height, ori_image.Width, ori_data.Stride, hsi_h, hsi_s, hsi_i, count); // RGB 轉 HSI
            hsi_i.Count();
            ori_image.UnlockBits(ori_data);
            hsi_image.UnlockBits(hsi_data);
            chart1.Series["Series1"].LegendText = "原圖的I";
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 255;
            for (int i = 0; i <= 255; i++)
            {
                chart1.Series["Series1"].Points.AddXY(i, count[i]);
            }
        }
        private void 直方圖ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void 縮小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hsi_data = hsi_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);
            System.IntPtr hsi_Ptr = hsi_data.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(hsi_Ptr, hsi_Values, 0, hsi_bytes);
            double r_max = 0;
            double r_min = 255;
            double s_max = 255;
            double s_min = 155;
            double r, tmp;
            int size = ori_bytes / 3;
            var count_shrinking = new int[256];
            var shrHist = new int[256];
            hsi_h.Count();
            var hist_shrink = new List<double>();
            for (int i = 0; i < size; i++)
            {
                tmp = hsi_i[i] * 255.0;
                if (tmp > r_max) r_max = tmp;
                if (tmp < r_min) r_min = tmp;
            }
            for (int i = 0; i < 256; i++)
            {
                shrHist[i] = i;
                count_shrinking[i] = 0;
            }
            for (int i = 0; i < size; i++)
            {
                r = hsi_i[i]*255;
                if ((r_max - r_min) == 0)
                {
                    hist_shrink[i] = s_min;
                }
                else
                {
                    tmp = (s_max - s_min) * (r - r_min) / (r_max - r_min) + s_min;
                    hist_shrink.Add(tmp/255);//(r-r_min)
                }//count_shrinking[(int)hist_shrink[index]]++;
            }

            Method.HSI2RGB(hsi_Values, shrHist, size, hsi_h, hsi_s, hist_shrink, count_shrinking);
            System.Runtime.InteropServices.Marshal.Copy(hsi_Values, 0, hsi_Ptr, hsi_bytes);
            chart2.Series["Series1"].LegendText = "I縮小後";
            chart2.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            chart2.ChartAreas["ChartArea1"].AxisX.Maximum = 255;
            for (int i = 0; i <= 255; i++)
            {
                chart2.Series["Series1"].Points.AddXY(i, count_shrinking[i]);
            }
            hsi_image.UnlockBits(hsi_data);
            pictureBox2.Image = hsi_image;
        }
    }
}
