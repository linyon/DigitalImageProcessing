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
using System.Windows.Forms.DataVisualization.Charting;
using _20170412_work04_HoughTransform.Properties;

namespace _20170412_work04_HoughTransform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap ori;
        private const double Deg2Rad = Math.PI / 90.0;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
            ori = pictureBox1.Image as Bitmap;
            //gray = pictureBox1.Image as Bitmap;
            pictureBox1.Image = ori;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var imageRect = new Rectangle(0, 0, ori.Width, ori.Height); // Image rectangle.
            var newBitmap = new Bitmap(imageRect.Width, imageRect.Height);// New bitmap for the image with sobel
            var gray = new Bitmap(imageRect.Width, imageRect.Height);
            var ori_data = ori.LockBits(imageRect, ImageLockMode.ReadWrite, ori.PixelFormat);
            var gray_data = gray.LockBits(imageRect, ImageLockMode.ReadWrite, gray.PixelFormat);
            var newBitmapData = newBitmap.LockBits(imageRect, ImageLockMode.ReadWrite, ori.PixelFormat);
            var byteCount = ori_data.Stride * ori_data.Height;// Stride is the amount of bytes in a row

            int diagonal = (int)(Math.Sqrt(ori.Width * ori.Width + ori.Height * ori.Height)) + 1; //-√2*D 到 √2*D
            var acc_Rect = new Rectangle(0, 0, 180, 2 * diagonal);
            var accumulator = new Bitmap(180, 2 * diagonal);
            var dataAccumulator = accumulator.LockBits(acc_Rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            unsafe
            {
                var ori_bmp = (byte*)ori_data.Scan0;
                var gray_bmp = (byte*)gray_data.Scan0;
                var newb_bmp = (byte*)newBitmapData.Scan0;
                var hough_bmp = (byte*)dataAccumulator.Scan0;
                GrayLevel(ori.Height, ori.Width, ori_data.Stride, ori_bmp, gray_bmp);
                SobelOperator(ori.Height, ori.Width, ori_data.Stride, newb_bmp, gray_bmp);
                int h = ori.Height;
                int w = ori.Width;
                var sin_tab = new double[181];
                var cos_tab = new double[181];
                for (int angle = 0; angle < 181; angle++) //-90 到 90
                {
                    var theta = (angle - 90) * Math.PI / 180.0;
                    cos_tab[angle] = Math.Cos(theta);
                    sin_tab[angle] = Math.Sin(theta);
                }
                var hough_accumulation = new int[2 * diagonal, 181]; //累加器
                var max = 0;
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        int p_Index = i * ori_data.Stride + j * 3;
                        if (newb_bmp[p_Index] > 25)
                        {
                            for (int angle = 0; angle < 181; angle++)// -90 到 90
                            {
                                var r = i * sin_tab[angle] + j * cos_tab[angle];
                                var nr = (int)(r + diagonal);                     //shift r to positive value
                                hough_accumulation[nr, angle]++;           // accumulation
                               
                                if (max < hough_accumulation[nr, angle])
                                {
                                    max = hough_accumulation[nr, angle];
                                }
                            }
                        }
                    }
                }
                double alpha = 255.0 / max;
                int size = (int)(max * alpha);
                var tmp_h = new int[size+1];
                for (int i = 0; i < accumulator.Height; i++)
                {
                    for (int j = 0; j < accumulator.Width; j++)
                    {
                        var a = i * dataAccumulator.Stride + j * 3;
                        int tmp = (int)(hough_accumulation[i, j]);// * alpha);
                        if(tmp * alpha != 0) tmp_h[(int)(tmp * alpha)]++;
                        //if (tmp != 0) tmp += 50;
                        hough_bmp[a] = (byte)(tmp);
                        hough_bmp[a + 1] = (byte)(tmp);
                        hough_bmp[a + 2] = (byte)(tmp);
                    }
                }
                
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        int p_Index = i * ori_data.Stride + j * 3;
                            for (int angle = 0; angle < 181; angle++)
                            {
                                var r = i * sin_tab[angle] + j * cos_tab[angle];
                                var nr = (int)(r + diagonal);
                                double tmp = hough_accumulation[nr, angle]* alpha;                   
                                if (tmp > 85.5) //門檻值需要隨著圖片的hough_acc 做改變
                                {
                                    ori_bmp[p_Index] = 0; //B
                                    ori_bmp[p_Index + 1] = 255; //G 
                                    ori_bmp[p_Index + 2] = 255; //R
                                }
                            }
                    }
                }

                chart1.Series["Series1"].LegendText = "y";
                chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0;
                chart1.ChartAreas["ChartArea1"].AxisX.Maximum = size;
                for (int i = 0; i <= size; i++)
                {
                    chart1.Series["Series1"].Points.AddXY(i, tmp_h[i]);
                }
                accumulator.UnlockBits(dataAccumulator);
            }//unsafe    
            ori.UnlockBits(ori_data);
            gray.UnlockBits(gray_data);
            newBitmap.UnlockBits(newBitmapData);
            pictureBox1.Image = ori ;
            pictureBox2.Image = newBitmap ;
            pictureBox3.Image = accumulator;
            
        }
        public unsafe void GrayLevel(int h, int w, int stride, byte* ori_bmp, byte* gray_bmp)
        {
            for (int i = 0; i < h; i++) //轉成gray level
            {
                for (int j = 0; j < w; j++)
                {
                    int p_Index = i * stride + j * 3;
                    byte pixel = Convert.ToByte((ori_bmp[p_Index + 0] + ori_bmp[p_Index + 1] + ori_bmp[p_Index + 2]) / 3);
                    gray_bmp[p_Index + 0] = pixel; //R 
                    gray_bmp[p_Index + 1] = pixel; //G 
                    gray_bmp[p_Index + 2] = pixel; //B 
                }
            }
        }
        public unsafe void SobelOperator(int h, int w, int stride, byte* newb_bmp, byte* ori_bmp)
        {
            var Gx = new sbyte[9] { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
            var Gy = new sbyte[9] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            int tmp = 3 / 2;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    int p_index = i * stride + j * 3;
                    if (i < tmp || i >= (h - tmp) || j < tmp || j >= (w - tmp))
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            int a = i * stride + j * 3 + k;
                            newb_bmp[a] = ori_bmp[a];
                        }
                        continue;
                    }
                    int x = 0;
                    var sum = new int[3] { 0, 0, 0 };
                    for (int ii = i - tmp; ii < (i - tmp) + 3; ii++)
                    {
                        for (int jj = j - tmp; jj < (j - tmp) + 3; jj++)
                        {
                            int p_in = ii * stride + jj * 3;
                            sum[0] += Gx[x] * ori_bmp[p_in];
                            sum[1] += Gy[x] * ori_bmp[p_in];
                            x++;
                        }
                    }
                    int f = Math.Abs(sum[0]) + Math.Abs(sum[1]);
                    if (f > 255)
                        f = 255;
                    else f = 0;
                    newb_bmp[p_index] = Convert.ToByte(f);
                    newb_bmp[p_index + 1] = Convert.ToByte(f);
                    newb_bmp[p_index + 2] = Convert.ToByte(f);
                }
            }// for (int i = 0; i < ori.Height; i++)
        }
    }
}
