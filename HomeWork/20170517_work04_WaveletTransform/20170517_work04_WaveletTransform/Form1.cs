using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20170517_work04_WaveletTransform
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
        byte[] ori_Values;
        int ori_bytes;

        Bitmap wt_image;
        BitmapData wt_data;
        int wt_bytes;

        bool check_input = false;
        private void 開檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);

            }
            ori_image = pictureBox1.Image as Bitmap;
            if (ori_image != null)
            {
                imageRect = new Rectangle(0, 0, ori_image.Width, ori_image.Height);
                ori_data = ori_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);

                ori_bytes = ori_data.Stride * ori_image.Height;
                ori_Values = new byte[ori_bytes];
                System.IntPtr ori_Ptr = ori_data.Scan0;
                System.Runtime.InteropServices.Marshal.Copy(ori_Ptr, ori_Values, 0, ori_bytes);//複製RGB信息到byte數組
                GrayLevel(ori_image.Height, ori_image.Width, ori_data.Stride, ori_Values);
                System.Runtime.InteropServices.Marshal.Copy(ori_Values, 0, ori_Ptr, ori_bytes); //複製byte陣列到RGB
                ori_image.UnlockBits(ori_data);
                check_input = true;
                label1.Visible = true;
            }
        }

        private void 小波轉換ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!check_input) return;
            wt_image = new Bitmap(imageRect.Width, imageRect.Height);
            wt_data = wt_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);
            wt_bytes = wt_data.Stride * wt_image.Height;
            var tmp1_Values = new byte[wt_bytes];
            var tmp2_Values = new byte[wt_bytes];
            System.IntPtr wt_Ptr = wt_data.Scan0;
            System.Array.Copy(ori_Values, 0, tmp1_Values, 0, ori_bytes); //把ori_Values陣列的數值複製到tmp1_Values陣列
            int h = ori_image.Height;
            int w = ori_image.Width;
            WaveletTransform(h, w, 0, ori_data.Stride, tmp1_Values, tmp2_Values); //1-pass
            WaveletTransform(h, w, 1, ori_data.Stride, tmp2_Values, tmp1_Values); //2-pass
            WaveletTransform(h, w, 2, ori_data.Stride, tmp1_Values, tmp2_Values); //3-pass
            System.Runtime.InteropServices.Marshal.Copy(tmp2_Values, 0, wt_Ptr, wt_bytes); //複製byte陣列到RGB
            wt_image.UnlockBits(wt_data);
            pictureBox2.Image = wt_image;
            pictureBox2.Height = wt_image.Height;
            pictureBox2.Width = wt_image.Width;
            label2.Visible = true;
        }
        public void GrayLevel(int h, int w, int stride, byte[] ori_bmp)
        {
            for (int i = 0; i < h; i++) //轉成gray level
            {
                for (int j = 0; j < w; j++)
                {
                    int p_Index = i * stride + j * 3;
                    byte pixel = Convert.ToByte((ori_bmp[p_Index + 0] + ori_bmp[p_Index + 1] + ori_bmp[p_Index + 2]) / 3);
                    ori_bmp[p_Index + 0] = pixel; //R 
                    ori_bmp[p_Index + 1] = pixel; //G 
                    ori_bmp[p_Index + 2] = pixel; //B 
                }
            }
        }
        public void WaveletTransform(int h, int w,int c, int stride, byte[] bmp1,byte[] bmp2)
        {
            float value;
            int a;
            bool check = false;
            byte[] tmp = new byte[ori_bytes];
            var count = Math.Pow(2,c);
            for (int i = 0; i < h; i++)
            {
                a = 0;
                check = false;
                for (int j = 0; j < w; j++)
                {
                    if (j == (w / 2) / count)
                    {
                        a = 0;
                        check = true;
                    }
                    if (i < (h / count) && j < (w / count))
                    {
                        if (check == false)
                            value = (float)((bmp1[i * stride + a * 3] + bmp1[i * stride + (a + 1) * 3]) / 2);
                        else
                            value = (float)((bmp1[i * stride + a * 3] - bmp1[i * stride + (a + 1) * 3]) / 2) + 127;
                        if (value < 0) value = value - 0.5f;
                        else value = value + 0.5f;
                        tmp[i * stride + j * 3] = (byte)value;
                        tmp[i * stride + j * 3 + 1] = (byte)value;
                        tmp[i * stride + j * 3 + 2] = (byte)value;
                        a += 2;
                    }
                    else
                    {
                        tmp[i * stride + j * 3] = bmp1[i * stride + j * 3];
                        tmp[i * stride + j * 3 + 1] = bmp1[i * stride + j * 3 + 1];
                        tmp[i * stride + j * 3 + 2] = bmp1[i * stride + j * 3 + 2];
                    } 
                }
            }
            for (int i = 0; i < w; i++)
            {
                a = 0;
                check = false;
                for (int j = 0; j < h; j++)
                {
                    if (j == (h / 2) / count)
                    {
                        a = 0;
                        check = true;
                    }
                    if (i < (h / count) && j < (w / count))
                    {
                        if (check == false)
                            value = (float)((tmp[a * stride + i * 3] + tmp[(a + 1) * stride + i * 3]) / 2);
                        else
                            value = (float)((tmp[a * stride + i * 3] - tmp[(a + 1) * stride + i * 3]) / 2) + 127;
                        if (value < 0) value = value - 0.5f;
                        else value = value + 0.5f;
                        bmp2[j * stride + i * 3] = (byte)value;
                        bmp2[j * stride + i * 3 + 1] = (byte)value;
                        bmp2[j * stride + i * 3 + 2] = (byte)value;
                        a += 2;
                    }
                    else
                    {
                        bmp2[j * stride + i * 3] = tmp[j * stride + i * 3];
                        bmp2[j * stride + i * 3 + 1] = tmp[j * stride + i * 3 + 1];
                        bmp2[j * stride + i * 3 + 2] = tmp[j * stride + i * 3 + 2];
                    }
                }
            }
        }
    }
}
