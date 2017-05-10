using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class Method
    {
        public static void RGB2HSI(byte[] ori_values, int h, int w, int Stride, List<int> hsi_h, List<double> hsi_s, List<double> hsi_i,int[] count)
        {
            double r, g, b;
            double radial, theta;
            double tempD, tempB;
            int i, j;
            for (i = 0; i < h; i++) //RGB 轉 HSI
            {
                for (j = 0; j < w; j++)
                {
                    int ori_Index = i * Stride + j * 3;
                    var R = ori_values[ori_Index + 2];
                    var G = ori_values[ori_Index + 1];
                    var B = ori_values[ori_Index];
                    double level = 255.0;
                    r = (R / level);
                    g = (G / level);
                    b = (B / level);
                    radial = Math.Acos(0.5 * ((r - g) + (r - b)) / Math.Sqrt((r - g) * (r - g) + (r - b) * (g - b)));//H
                    theta = (radial * 180.0 / Math.PI);
                    if (!double.IsNaN(theta)) tempD = (b <= g) ? theta : (360 - theta);
                    else tempD = 0;
                    hsi_h.Add(Convert.ToInt32(tempD));
                    tempB = Math.Min(r, g);//S
                    tempB = Math.Min(tempB, b);
                    tempD = 1.0 - 3.0 * tempB / (r + g + b);
                    if (double.IsNaN(tempD)) tempD = 0;
                    hsi_s.Add(tempD);                   
                    tempD = (r + g + b) / 3.0;//I
                    count[(int)(tempD * 255)]++;
                    hsi_i.Add(tempD);
                }
            }
        }
        public static void HE(int size, List<double> hsi_i, int[] eqHist)
        {
            int i;
            var hist = new int[256];
            var fpHist = new int[256];
            var eqHistTemp = new int[256];
            for (i = 0; i < size; i++) // 統計每個灰階值出現的像素數量
            {
                var tmp = (int)(hsi_i[i] * 255.0);
                hist[tmp]++;
            }
            eqHistTemp[0] = hist[0];
            for (i = 1; i < 256; i++)//計算累計直方圖
            {
                eqHistTemp[i] = eqHistTemp[i - 1] + hist[i];
            }
            for (i = 0; i < 256; i++)//累計分布並取整數，儲存計算出來的灰階值映射關係
            {
                eqHist[i] = (int)(255 * eqHistTemp[i] / size + 0.5);
            }
        }
        public static void HSI2RGB(byte[] hsi_Values, int[] Hist, int size, List<int> hsi_h, List<double> hsi_s, List<double> hsi_i, int[] count_eq)
        {
            double r, g, b, theta;
            int i, type;
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
                r = r * 3.0 * Hist[hsi_index];
                g = g * 3.0 * Hist[hsi_index];
                b = b * 3.0 * Hist[hsi_index];
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
                count_eq[Hist[hsi_index]]++;
            }
        }
    }
}
