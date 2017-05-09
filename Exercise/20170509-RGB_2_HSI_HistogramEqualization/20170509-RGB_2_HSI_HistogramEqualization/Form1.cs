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
            double radial, theta;
            double tempD, tempB;
            var hsi_h = new List<int>();
            var hsi_s = new List<double>();
            var hsi_i = new List<double>();
            int i, j;
            double r, g, b;
            for (i = 0; i < h; i++) //RGB 轉 HSI
            {
                for (j = 0; j < w; j++)
                {
                    int ori_Index = i * ori_data.Stride + j * 3;
                    var R = ori_Values[ori_Index + 2];
                    var G = ori_Values[ori_Index + 1];
                    var B = ori_Values[ori_Index];
                    double level = 255.0;//R + G + B;
                    r = (R / level);
                    g = (G / level);
                    b =(B / level);
                    //H
                    radial = Math.Acos(0.5 * ((r - g) + (r - b)) / Math.Sqrt((r - g) * (r - g) + (r - b) * (g - b)));
                    theta = (radial * 180.0 / Math.PI);
                    if (!double.IsNaN(theta)) tempD = (b <= g) ? theta:(360 - theta); 
                    else tempD = 0;
                    hsi_h.Add(Convert.ToInt32(tempD));
                    //S
                    tempB = Math.Min(r, g);
                    tempB = Math.Min(tempB, b);
                    tempD = 1.0 - 3.0 * tempB / (r + g + b);
                    if (double.IsNaN(tempD)) tempD = 0;
                    hsi_s.Add(tempD);
                    //I
                    tempD = (r + g + b) / 3.0;
                    hsi_i.Add(tempD);
                }
            }
            var hist = new int[256];
            var fpHist = new int[256];
            var eqHistTemp = new int[256];
            var eqHist = new int[256];
            var count_eq = new int[256];
            int size = ori_bytes / 3;
            int type;
            // 統計每個灰階值出現的像素數量--------
            for (i = 0; i < size; i++)
            {
                var tmp = (int)(hsi_i[i] * 255.0);
                hist[tmp]++;
            }
            //計算累計直方圖-------------------
            eqHistTemp[0] = hist[0];
            for (i = 1; i < 256; i++)
            {
                eqHistTemp[i] = eqHistTemp[i - 1] + hist[i];
            }
            //累計分布並取整數，儲存計算出來的灰階值映射關係
            for (i = 0; i < 256; i++)
            {
                eqHist[i] = (int)(255 * eqHistTemp[i] / size + 0.5);
            }
            //執行灰階值映射、均衡化
            for (i = 0; i < size; i++)
            {
                var H = hsi_h[i];
                if (H <= 120)
                {
                    type = 0;
                }
                else if (H > 120 && H <= 240)
                {
                    H = H - 120;
                    type = 1;
                }
                else
                {
                    H = H - 240;
                    type = 2;
                }
                theta = 60 - H;
                var tmp_1 = ((1 - hsi_s[i])) / 3.0;
                var tmp_2 = (1 + (hsi_s[i] * Math.Cos(H * Math.PI / 180.0) / Math.Cos(theta * Math.PI / 180.0))) / 3.0;
                var tmp_3 = (1 - (tmp_1 + tmp_2));
                if (type == 0)
                {
                    r = tmp_2;
                    g = tmp_3;
                    b = tmp_1;
                }
                else if (type == 1)
                {
                    r = tmp_1;
                    g = tmp_2;
                    b = tmp_3;
                }
                else
                {
                    r = tmp_3;
                    g = tmp_1;
                    b = tmp_2;
                }        
                var hsi_index = (int)(hsi_i[i] * 255.0);
                r = r * 3.0 * eqHist[hsi_index];
                g = g * 3.0 * eqHist[hsi_index];
                b = b * 3.0 * eqHist[hsi_index];
                if (r > 255)
                    r = 255;
                else if (r < 0)
                    r = 0;
                if (g > 255)
                    g = 255;
                else if (g < 0)
                    g = 0;
                if (b > 255)
                    b = 255;
                else if (b < 0)
                    b = 0;
                hsi_Values[i * 3] = (byte)b;
                hsi_Values[i * 3 + 1] = (byte)g;
                hsi_Values[i * 3 + 2] = (byte)r;
                count_eq[eqHist[hsi_index]]++;
            }
            //System.Runtime.InteropServices.Marshal.Copy(hsi_Ptr, hsi_Values, 0, hsi_bytes);//複製GRB信息到byte數組
            System.Runtime.InteropServices.Marshal.Copy(hsi_Values, 0, hsi_Ptr, hsi_bytes);
            chart1.Series["Series1"].LegendText = "eq";
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
            chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 255;
            for (i = 0; i <= 255; i++)
            {
                chart1.Series["Series1"].Points.AddXY(i, count_eq[i]);
            }

            hsi_image.UnlockBits(hsi_data);
            ori_image.UnlockBits(ori_data);
            pictureBox1.Image = hsi_image;
        }
        public void RGB2HSI()
        {

        }

    }
}
