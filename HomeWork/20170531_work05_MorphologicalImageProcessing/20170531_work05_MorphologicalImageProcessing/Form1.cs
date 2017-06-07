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
namespace _20170531_work05_MorphologicalImageProcessing
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
        bool check_input = false;
        private byte[,] shape
        {
            get
            {
                return new byte[,]
                {
                    { 0, 1, 0 },
                    { 1, 1, 1 },
                    { 0, 1, 0 }
                };
            }
        }
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
                IntPtr ori_Ptr = ori_data.Scan0;
                System.Runtime.InteropServices.Marshal.Copy(ori_Ptr, ori_Values, 0, ori_bytes);//複製RGB信息到byte數組
                GrayLevel(ori_image.Height, ori_image.Width, ori_data.Stride, ori_Values);
                System.Runtime.InteropServices.Marshal.Copy(ori_Values, 0, ori_Ptr, ori_bytes); //複製byte陣列到RGB
                ori_image.UnlockBits(ori_data);
                check_input = true;
                pictureBox1.Image = ori_image;
            }
        }

        private void dilationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (check_input)
            {
                ori_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);
                var D_Values = new byte[ori_bytes];
                Array.Copy(ori_Values, 0, D_Values, 0, ori_bytes); //把ori_Values陣列的數值複製到D_Values陣列
                ori_Values = Dilation_Erosion(D_Values, true, 3);
                System.Runtime.InteropServices.Marshal.Copy(ori_Values, 0, ori_data.Scan0, ori_bytes); //複製byte陣列到RGB
                ori_image.UnlockBits(ori_data);
                pictureBox2.Image = ori_image;
            }
            else MessageBox.Show("未先選擇圖檔", "ERROR",MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        private void ErosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (check_input)
            {
                ori_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);
                var E_Values = new byte[ori_bytes];
                Array.Copy(ori_Values, 0, E_Values, 0, ori_bytes); //把ori_Values陣列的數值複製到D_Values陣列
                ori_Values = Dilation_Erosion(E_Values, false, 3);
                System.Runtime.InteropServices.Marshal.Copy(ori_Values, 0, ori_data.Scan0, ori_bytes); //複製byte陣列到RGB
                ori_image.UnlockBits(ori_data);
                pictureBox2.Image = ori_image;
            }
            else MessageBox.Show("未先選擇圖檔", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        { //先侵蝕再膨脹，斷開就是(AΘB)⊕B
            if (check_input)
            {
                ori_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);
                var O_Values = new byte[ori_bytes];
                Array.Copy(ori_Values, 0, O_Values, 0, ori_bytes); //把ori_Values陣列的數值複製到D_Values陣列
                O_Values = Dilation_Erosion(O_Values, false, 3); //侵蝕
                ori_Values = Dilation_Erosion(O_Values, true, 3); //膨脹
                System.Runtime.InteropServices.Marshal.Copy(ori_Values, 0, ori_data.Scan0, ori_bytes); //複製byte陣列到RGB
                ori_image.UnlockBits(ori_data);
                pictureBox2.Image = ori_image;
            }
            else MessageBox.Show("未先選擇圖檔", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        { //先膨脹再侵蝕，閉合就是(A⊕B)ΘB
            if (check_input)
            {
                ori_image.LockBits(imageRect, ImageLockMode.ReadWrite, ori_image.PixelFormat);
                var O_Values = new byte[ori_bytes];
                Array.Copy(ori_Values, 0, O_Values, 0, ori_bytes); //把ori_Values陣列的數值複製到D_Values陣列
                O_Values = Dilation_Erosion(O_Values, true, 3); //膨脹
                ori_Values = Dilation_Erosion(O_Values, false, 3); //侵蝕
                System.Runtime.InteropServices.Marshal.Copy(ori_Values, 0, ori_data.Scan0, ori_bytes); //複製byte陣列到RGB
                ori_image.UnlockBits(ori_data);
                pictureBox2.Image = ori_image;
            }
            else MessageBox.Show("未先選擇圖檔", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
        public byte[] Dilation_Erosion(byte[] Values, bool Type, int Size)
        {
            byte[] tmp_Values = new byte[ori_bytes];
            int Offset = (Size - 1) / 2;
            byte value;
            for (int i = Offset; i < ori_image.Height - Offset; i++)
            {
                for (int j = Offset; j < ori_image.Width - Offset; j++)
                {
                    if (Type) value = 0; //膨脹
                    else value = 255;   //侵蝕
                    int index = i * ori_data.Stride + j * 3;
                    for (int ii = -Offset; ii <= Offset; ii++)
                    {
                        for (int jj = -Offset; jj <= Offset; jj++)
                        {
                            if (shape[ii + Offset, jj + Offset] == 1)
                            {
                                int calcOffset = index + ii * ori_data.Stride + jj * 3;
                                if (Type) value = Math.Max(value, Values[calcOffset]);
                                else value = Math.Min(value, Values[calcOffset]);
                            }
                            else
                                continue;
                        }
                    }
                    tmp_Values[index] = value;
                    tmp_Values[index + 1] = value;
                    tmp_Values[index + 2] = value;
                }
            }
            return tmp_Values;
        }
    }
}
