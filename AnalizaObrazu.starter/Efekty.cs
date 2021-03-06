using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace AnalizaObrazu
{
    public struct Rgb
    {
        #region pola
        public byte b, g, r;
        #endregion pola
        public Rgb Negatyw()
        {
            Rgb rob;
            rob.r = (byte) (255 - this.r);
            rob.g = (byte) (255 - this.g);
            rob.b = (byte) (255 - this.b);
            return rob;
        }
        public Rgb Jasnosc()
        {
            Rgb rob;
            byte srednia = (byte)((this.r + this.g + this.b) / 3);
            rob.r = srednia;
            rob.g = srednia;
            rob.b = srednia;
            return rob;
        }
        public Rgb Jasnosc_waz()
        {
            Rgb rob;
            byte sredniaWazona = (byte)(this.r *0.299+ this.g *0.587+ this.b*0.114) ;
            rob.r = sredniaWazona;
            rob.g = sredniaWazona;
            rob.b = sredniaWazona;
            return rob;
        }
        public Rgb ZmianaJasnosci(int p)
        {
            Rgb rob;
          
            if ((this.r+p )< 0) rob.r = 0;
            else if ((this.r + p) > 255) rob.r = 255;
            else rob.r = (byte)(this.r +p);

            if ((this.g + p) < 0) rob.g = 0;
            else if ((this.g + p) > 255) rob.g = 255;
            else rob.g = (byte)(this.g + p);

            if ((this.b + p) < 0) rob.b = 0;
            else if((this.b + p) > 255) rob.b = 255;
            else rob.b= (byte)(this.b + p);
            return rob;
        }
    }
    static class Efekty
    {
        public static Bitmap Negatyw(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int strideWe = bmWeData.Stride;
            int strideWy = bmWyData.Stride;

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 0; x < szerokosc; x++)
                    {

                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy = pikselWejsciowy.Negatyw();
                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap Skalowanie(Bitmap bitmapaWe, double ratio)
        {
            int wysokoscWe = bitmapaWe.Height;
            int szerokoscWe = bitmapaWe.Width;

            int wysokoscWy = (int)(wysokoscWe * ratio);
            int szerokoscWy = (int)(szerokoscWe * ratio);

            double ratioWys = ((double)(wysokoscWy - 1)) / (wysokoscWe - 1);
            double ratioSzer = ((double)(szerokoscWy - 1)) / (szerokoscWe - 1);

            Bitmap bitmapaWy = new Bitmap(szerokoscWy, wysokoscWy, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokoscWe, wysokoscWe), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokoscWy, wysokoscWy), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int strideWe = bmWeData.Stride;
            int strideWy = bmWyData.Stride;

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            unsafe
            {
                for (int y = 0; y < wysokoscWy; y++)
                {
                    double b = (y / ratioWys) - (int)(y / ratioWys);

                    //byte* pWe = (byte*)(void*)scanWe + (int)Math.Round(y / ratio) * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 0; x < szerokoscWy; x++)
                    {
                        double a = (x / ratioSzer) - (int)(x / ratioSzer);

                        double suma_r = 0;
                        double suma_g = 0;
                        double suma_b = 0;

                        if ((1 - b) * (1 - a) != 0)
                        {
                            int xi = (int)(x / ratioSzer);
                            int yi = (int)(y / ratioWys);

                            Rgb p_lewa_gora = ((Rgb*)((byte*)(void*)scanWe +  yi * strideWe))[xi];
                            suma_r += (1 - b) * (1 - a) * p_lewa_gora.r;
                            suma_g += (1 - b) * (1 - a) * p_lewa_gora.g;
                            suma_b += (1 - b) * (1 - a) * p_lewa_gora.b;
                        }

                        if ((1 - b) * a != 0)
                        {
                            int xi = (int)(x / ratioSzer)+1;
                            int yi = (int)(y / ratioWys);

                            Rgb p_praw_gora = ((Rgb*)((byte*)(void*)scanWe + yi * strideWe))[xi];
                            suma_r += (1 - b) * a * p_praw_gora.r;
                            suma_g += (1 - b) * a * p_praw_gora.g;
                            suma_b += (1 - b) * a * p_praw_gora.b;
                        }

                        if (b * (1 - a) != 0)
                        {
                            int xi = (int)(x / ratioSzer);
                            int yi = (int)(y / ratioWys) + 1;

                            Rgb p_lewy_dol = ((Rgb*)((byte*)(void*)scanWe + yi * strideWe))[xi];
                            suma_r += b * (1 - a) * p_lewy_dol.r;
                            suma_g += b * (1 - a) * p_lewy_dol.g;
                            suma_b += b * (1 - a) * p_lewy_dol.b;
                        }

                        if (b * a != 0)
                        {
                            int xi = (int)(x / ratioSzer) + 1;
                            int yi = (int)(y / ratioWys) + 1;

                            Rgb p_prawy_dol = ((Rgb*)((byte*)(void*)scanWe + yi * strideWe))[xi];
                            suma_r += b * a * p_prawy_dol.r;
                            suma_g += b * a * p_prawy_dol.g;
                            suma_b += b * a * p_prawy_dol.b;
                        }

                        if (suma_r < 0) suma_r = 0; if (suma_r > 255) suma_r = 255;
                        if (suma_g < 0) suma_g = 0; if (suma_g > 255) suma_g = 255;
                        if (suma_b < 0) suma_b = 0; if (suma_b > 255) suma_b = 255;

                        Rgb pikselWynikowy;
                        pikselWynikowy.r = (byte)suma_r;
                        pikselWynikowy.g = (byte)suma_g;
                        pikselWynikowy.b = (byte)suma_b;

                        //Rgb pikselWejsciowy = ((Rgb*)pWe)[(int)Math.Round(x / ratio)];
                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }


        public static Bitmap Jasnosc(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 0; x < szerokosc; x++)
                    {

                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy = pikselWejsciowy.Jasnosc();
                        ((Rgb*)pWy)[x] = pikselWynikowy;
                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }



        public static Bitmap JasnoscWazona(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int strideWe = bmWeData.Stride;
            int strideWy = bmWyData.Stride;

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 0; x < szerokosc; x++)
                    {

                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy = pikselWejsciowy.Jasnosc_waz();
                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap ZmianaJasnosci(Bitmap bitmapaWe, int p)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int strideWe = bmWeData.Stride;
            int strideWy = bmWyData.Stride;

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 0; x < szerokosc; x++)
                    {

                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy = pikselWejsciowy.ZmianaJasnosci(p);
                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap Scienianie(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;


            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            Rgb obiekt = new Rgb() { r = 0, g = 0, b = 0 };
            Rgb tlo = new Rgb() { r = 255, g = 255, b = 255 };

            unsafe
            {
                for (int y = 1; y < wysokosc - 1; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 1; x < szerokosc - 1; x++)
                    {
                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy;

                        bool czyWarunekSpelniony = pikselWejsciowy.r == obiekt.r && pikselWejsciowy.g == obiekt.g && pikselWejsciowy.b == obiekt.b;

                        if (czyWarunekSpelniony)
                        {
                            for (int yi = y - 1; yi <= y + 1; yi++)
                            {
                                byte* pWeOtoczenie = (byte*)(void*)scanWe + yi * strideWe;
                                for (int xi = x - 1; xi <= x + 1; xi++)
                                {
                                    Rgb pikselOtoczenia = ((Rgb*)pWeOtoczenie)[xi];
                                    czyWarunekSpelniony = czyWarunekSpelniony &&
                                        pikselOtoczenia.r == obiekt.r && pikselOtoczenia.g == obiekt.g && pikselOtoczenia.b == obiekt.b;
                                }
                            }


                            if (czyWarunekSpelniony)
                            {
                                pikselWynikowy = tlo;
                            }
                            else
                            {
                                pikselWynikowy = obiekt;
                            }
                        }
                        else pikselWynikowy = tlo;

                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap FiltrUsredniajacy(Bitmap bitmapaWe, int n)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;


            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            int w = (2 * n + 1) * (2 * n + 1);

            unsafe
            {
                for (int y = n; y < wysokosc - n; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                    for (int x = n; x < szerokosc - n; x++)
                    {
                        Rgb pikselWynikowy;
                        int suma_r = 0, suma_g = 0, suma_b = 0;

                        for (int yi = y - n; yi <= y + n; yi++)
                        {
                            byte* pWeOtoczenie = (byte*)(void*)scanWe + yi * strideWe;
                            for (int xi = x - n; xi <= x + n; xi++)
                            {
                                Rgb pikselOtoczenia = ((Rgb*)pWeOtoczenie)[xi];
                                suma_r += pikselOtoczenia.r;
                                suma_g += pikselOtoczenia.g;
                                suma_b += pikselOtoczenia.b;
                            }
                        }

                        pikselWynikowy.r = (byte)(suma_r / w);
                        pikselWynikowy.g = (byte)(suma_g / w);
                        pikselWynikowy.b = (byte)(suma_b / w);

                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap FiltrUsredniajacy(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;


            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            int[,] wsp = new int[3,3] {
            {1,1,1},
            {1,0,1},
            {1,1,1}
            };

            int w = 0;

            for(int i=0; i<3; i++)
            {
                for(int j=0; j<3; j++)
                {
                    w += wsp[i, j];
                }
            }

            unsafe
            {
                for (int y = 1; y < wysokosc - 1; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                    for (int x = 1; x < szerokosc - 1; x++)
                    {
                        Rgb pikselWynikowy;
                        int suma_r = 0, suma_g = 0, suma_b = 0;

                        for (int yi = y - 1; yi <= y + 1; yi++)
                        {
                            byte* pWeOtoczenie = (byte*)(void*)scanWe + yi * strideWe;
                            for (int xi = x - 1; xi <= x + 1; xi++)
                            {
                                Rgb pikselOtoczenia = ((Rgb*)pWeOtoczenie)[xi];
                                suma_r += pikselOtoczenia.r * wsp[x - xi + 1, y - yi + 1];
                                suma_g += pikselOtoczenia.g * wsp[x - xi + 1, y - yi + 1];
                                suma_b += pikselOtoczenia.b * wsp[x - xi + 1, y - yi + 1];
                            }
                        }

                        pikselWynikowy.r = (byte)(suma_r / w);
                        pikselWynikowy.g = (byte)(suma_g / w);
                        pikselWynikowy.b = (byte)(suma_b / w);

                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap FiltrMedianowy(Bitmap bitmapaWe, int n)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;


            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;
            List<byte> otoczenie_r = new List<byte>();
            List<byte> otoczenie_g = new List<byte>();
            List<byte> otoczenie_b = new List<byte>();

            int w = (2 * n + 1) * (2 * n + 1)/2;

            unsafe
            {
                for (int y = n; y < wysokosc - n; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                    for (int x = n; x < szerokosc - n; x++)
                    {
                        Rgb pikselWynikowy;
                        otoczenie_r.Clear();
                        otoczenie_g.Clear();
                        otoczenie_b.Clear();

                        int suma_r = 0, suma_g = 0, suma_b = 0;

                        for (int yi = y - n; yi <= y + n; yi++)
                        {
                            byte* pWeOtoczenie = (byte*)(void*)scanWe + yi * strideWe;
                            for (int xi = x - n; xi <= x + n; xi++)
                            {
                                Rgb pikselOtoczenia = ((Rgb*)pWeOtoczenie)[xi];
                                otoczenie_r.Add(pikselOtoczenia.r);
                                otoczenie_g.Add(pikselOtoczenia.g);
                                otoczenie_b.Add(pikselOtoczenia.b);
                            }
                        }

                        pikselWynikowy.r = (otoczenie_r.OrderBy(t => t).ToArray()[w]);
                        pikselWynikowy.g = (otoczenie_g.OrderBy(t => t).ToArray()[w]);
                        pikselWynikowy.b = (otoczenie_b.OrderBy(t => t).ToArray()[w]);

                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap FiltrGaussa(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;


            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            int[,] wsp = new int[3, 3] {
            {1,2,1},
            {2,4,2},
            {1,2,1}
            };

            int w = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    w += wsp[i, j];
                }
            }

            unsafe
            {
                for (int y = 1; y < wysokosc - 1; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                    for (int x = 1; x < szerokosc - 1; x++)
                    {
                        Rgb pikselWynikowy;
                        int suma_r = 0, suma_g = 0, suma_b = 0;

                        for (int yi = y - 1; yi <= y + 1; yi++)
                        {
                            byte* pWeOtoczenie = (byte*)(void*)scanWe + yi * strideWe;
                            for (int xi = x - 1; xi <= x + 1; xi++)
                            {
                                Rgb pikselOtoczenia = ((Rgb*)pWeOtoczenie)[xi];
                                suma_r += pikselOtoczenia.r * wsp[x - xi + 1, y - yi + 1];
                                suma_g += pikselOtoczenia.g * wsp[x - xi + 1, y - yi + 1];
                                suma_b += pikselOtoczenia.b * wsp[x - xi + 1, y - yi + 1];
                            }
                        }

                        pikselWynikowy.r = (byte)(suma_r / w);
                        pikselWynikowy.g = (byte)(suma_g / w);
                        pikselWynikowy.b = (byte)(suma_b / w);

                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap Logarytm(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int strideWe = bmWeData.Stride;
            int strideWy = bmWyData.Stride;

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 0; x < szerokosc; x++)
                    {

                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy;
                        double rob = (Math.Log(1 + pikselWejsciowy.r) / Math.Log(1 + 255) * 255);
                        rob = pikselWynikowy.r = (byte)(rob);
                        rob = (Math.Log(1 + pikselWejsciowy.g) / Math.Log(1 + 255) * 255);
                        rob = pikselWynikowy.g = (byte)(rob);
                        rob = (Math.Log(1 + pikselWejsciowy.b) / Math.Log(1 + 255) * 255);
                        rob = pikselWynikowy.b = (byte)(rob);
                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }
        public static Bitmap HistogramJasnosci(Bitmap bitmapaWe)
        {

            int[] histogram = WyliczHistogramJasnosci(bitmapaWe);
            return NarysujHistogram(histogram);
        }

        private static int[] WyliczHistogramJasnosci(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;
            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            int[] histogram = new int[256];
            unsafe {
                for (int y = 0; y < wysokosc; y++) {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    for (int x = 0; x < szerokosc; x++) {
                        byte jasnosc = ((Rgb*)pWe)[x].Jasnosc().r;
                        histogram[jasnosc]++;
                    }
                }
            }
            bitmapaWe.UnlockBits(bmWeData);
            return histogram;
        }
        private static Bitmap NarysujHistogram(int[] histogram) {
            //Bitmap histogramBitmap = new Bitmap(256, 256, PixelFormat.Format24bppRgb); 
            //Graphics g = Graphics.FromImage(histogramBitmap); 
            //Pen pen = new Pen(Color.FromArgb(125, 125, 125)); 
            //int max = 0; 
            //for (int i = 0; i < 256; i++) {
            //    if (histogram[i] > max) {
            //        max = histogram[i]; 
            //    } 
            //} 
            //for (int i = 0; i < 256; i++) {
            //    g.DrawLine(pen, i, 255, i, 255 - ((255 * histogram[i]) / max));
            //} 
            //return histogramBitmap;


            Bitmap histogramBitmap = new Bitmap(256, 256, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(histogramBitmap);
            int max = 0;
            for (int i = 0; i < 256; i++) {
                if (histogram[i] > max) {
                    max = histogram[i];
                }
            }
            for (int i = 0; i < 256; i++) {
                Pen pen = new Pen(Color.FromArgb(255 - i, 0, 0));
                g.DrawLine(pen, i, 255, i, 255 - ((255 * histogram[i]) / max));
                pen = new Pen(Color.FromArgb(i, i, i));
                g.DrawLine(pen, i, 255 - ((255 * histogram[i]) / max), i, 0);
            }
            return histogramBitmap;
        }
        public static Bitmap ZmianaJasnosciLut(Bitmap bitmapaWe, int param) {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;
            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);
            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            byte[] LUT = new byte[256];
            for (int i = 0; i < 256; i++) {
                int rob = i + param;
                if (rob < 0) rob = 0;
                if (rob > 255) rob = 255;
                LUT[i] = (byte)(rob);
            }
            unsafe {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                    for (int x = 0; x < szerokosc; x++) {
                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy;
                        pikselWynikowy.r = LUT[pikselWejsciowy.r];
                        pikselWynikowy.g = LUT[pikselWejsciowy.g];
                        pikselWynikowy.b = LUT[pikselWejsciowy.b];
                        ((Rgb*)pWy)[x] = pikselWynikowy;
                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy; }

        public static Bitmap ZmianaKontrastu(Bitmap bitmapaWe, double param)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;
            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);
            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            byte[] LUT = new byte[256];

            for (int i = 0; i < 256; i++)
            {
                double rob = param * (i - (255 / 2)) + 255 / 2;
                if (rob < 0) rob = 0;
                if (rob > 255) rob = 255;
                LUT[i] = (byte)(rob);
            }
            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                    for (int x = 0; x < szerokosc; x++)
                    {
                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy;
                        pikselWynikowy.r = LUT[pikselWejsciowy.r];
                        pikselWynikowy.g = LUT[pikselWejsciowy.g];
                        pikselWynikowy.b = LUT[pikselWejsciowy.b];
                        ((Rgb*)pWy)[x] = pikselWynikowy;
                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap WyrownywanieHistogramu(Bitmap bitmapaWe, int param)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;
            int[] histogram = WyliczHistogramJasnosci(bitmapaWe);

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);
            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;



            double[] dystrybuanta = new double[256];

            for (int i = 0; i < 256; i++)
            {
                int suma = 0;
                for (int j = 0; j <= i; j++)
                {
                    suma += histogram[j];
                }
                dystrybuanta[i] = suma / (double)(wysokosc * szerokosc);
            }

            byte[] LUT = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                int rob = (int)Math.Round(((dystrybuanta[i] - dystrybuanta[0]) / (1 - dystrybuanta[0])) * (255));
                if (rob < 0) rob = 0;
                if (rob > 255) rob = 255;
                LUT[i] = (byte)(rob);
            }
            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                    for (int x = 0; x < szerokosc; x++)
                    {
                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy;
                        pikselWynikowy.r = LUT[pikselWejsciowy.r];
                        pikselWynikowy.g = LUT[pikselWejsciowy.g];
                        pikselWynikowy.b = LUT[pikselWejsciowy.b];
                        ((Rgb*)pWy)[x] = pikselWynikowy;
                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap Progowanie(Bitmap bitmapaWe, int prog)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);
            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            Rgb black = new Rgb() { r = 0, g = 0, b = 0 };
            Rgb white = new Rgb() { r = 255, g = 255, b = 255 };

            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 0; x < szerokosc; x++)
                    {
                        var pixelWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pixelWynikowy;
                        if (pixelWejsciowy.Jasnosc().r < prog)
                        {
                            pixelWynikowy = white;
                        }
                        else pixelWynikowy = black;

                        ((Rgb*)pWy)[x] = pixelWynikowy;
                    }
                }
            }

            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);
            return bitmapaWy;
        }

        internal static byte WyznaczProgowanieOtsu(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;

            double iloscPikseli = wysokosc * szerokosc;

            int[] histogram = WyliczHistogramJasnosci(bitmapaWe);
            double[] sigma = new double[256];

            for (int t = 0; t < 256; t++)
            {
                double p0 = 0;
                double mi0 = 0;
                for (int i = 0; i < t; i++)
                {
                    p0 += histogram[i] / iloscPikseli;

                    mi0 += i * histogram[i] / iloscPikseli;
                }

                if (p0 != 0)
                    mi0 = mi0 / p0;
                else mi0 = 0;

                double p1 = 1 - p0;
                double mi1 = 0;
                for (int i = t; i < 256; i++)
                {
                    mi1 += i * histogram[i] / iloscPikseli;
                }

                if (p1 != 0)
                    mi1 = mi1 / p1;
                else mi1 = 0;

                sigma[t] = p0 * p1 * (mi0 - mi1) * (mi0 - mi1);

            }

            double maxsigma = sigma[0];
            int maxIndex = 0;
            for (int i = 0; i < 256; i++)
            {
                if (sigma[i] > maxsigma)
                {
                    maxsigma = sigma[i];
                    maxIndex = i;
                }
            }

            return (byte)maxIndex;
        }

        public static Bitmap ProgowanieKeep(Bitmap bitmapaWe, int prog)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;

            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);
            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            Rgb black = new Rgb() { r = 0, g = 0, b = 0 };
            Rgb white = new Rgb() { r = 255, g = 255, b = 255 };

            unsafe
            {
                for (int y = 0; y < wysokosc; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                    for (int x = 0; x < szerokosc; x++)
                    {
                        var pixelWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pixelWynikowy;
                        if (pixelWejsciowy.Jasnosc().r < prog)
                        {
                            pixelWynikowy = black;
                        }
                        else pixelWynikowy = pixelWejsciowy;

                        ((Rgb*)pWy)[x] = pixelWynikowy;
                    }
                }
            }

            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);
            return bitmapaWy;
        }

        public static Bitmap Erozja(Bitmap bitmapaWe, int okno)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;


            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            Rgb tlo = new Rgb() { r = 0, g = 0, b = 0 };
            Rgb obiekt = new Rgb() { r = 255, g = 255, b = 255 };

            unsafe
            {
                for (int y = okno; y < wysokosc - okno; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                    for (int x = okno; x < szerokosc - okno; x++)
                    {
                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy;

                        bool czyWarunekSpelniony = true;
                        for (int yi = y - 1; yi <= y + 1; yi++)
                        {
                            byte* pWeOtoczenie = (byte*)(void*)scanWe + yi * strideWe;
                            for (int xi = x - 1; xi <= x + 1; xi++)
                            {
                                Rgb pikselOtoczenia = ((Rgb*)pWeOtoczenie)[xi];
                                czyWarunekSpelniony = czyWarunekSpelniony &&
                                    pikselOtoczenia.r == obiekt.r && pikselOtoczenia.g == obiekt.g && pikselOtoczenia.b == obiekt.b;
                            }
                        }

                        if (czyWarunekSpelniony == true)
                        {
                            pikselWynikowy = obiekt;
                        }
                        else
                        {
                            pikselWynikowy = tlo;
                        }

                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap Dylatacja(Bitmap bitmapaWe, int okno)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;


            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;

            Rgb tlo = new Rgb() { r = 0, g = 0, b = 0 };
            Rgb obiekt = new Rgb() { r = 255, g = 255, b = 255 };

            unsafe
            {
                for (int y = okno; y < wysokosc - okno; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                    for (int x = okno; x < szerokosc - okno; x++)
                    {
                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy;

                        bool czyWarunekSpelniony =
                                pikselWejsciowy.r == tlo.r &&
                                pikselWejsciowy.g == tlo.g &&
                                pikselWejsciowy.b == tlo.b;

                        if (czyWarunekSpelniony)
                        {
                            czyWarunekSpelniony = false;
                            for (int yi = y - 1; yi <= y + 1; yi++)
                            {
                                byte* pWeOtoczenie = (byte*)(void*)scanWe + yi * strideWe;
                                for (int xi = x - 1; xi <= x + 1; xi++)
                                {
                                    Rgb pikselOtoczenia = ((Rgb*)pWeOtoczenie)[xi];
                                    czyWarunekSpelniony = czyWarunekSpelniony &&
                                        pikselOtoczenia.r == obiekt.r && pikselOtoczenia.g == obiekt.g && pikselOtoczenia.b == obiekt.b;
                                }
                            }

                            if (czyWarunekSpelniony) pikselWynikowy = obiekt;
                            else pikselWynikowy = tlo;


                        }
                        else
                        {
                            pikselWynikowy = obiekt;
                        }

                        ((Rgb*)pWy)[x] = pikselWynikowy;
                    }

                }
            }

            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        public static Bitmap KonturWewnetrzny(Bitmap bitmapaWe, int okno)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;


            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);
           

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
           

            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
           

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;
           

            Rgb tlo = new Rgb() { r = 0, g = 0, b = 0 };
            Rgb obiekt = new Rgb() { r = 255, g = 255, b = 255 };

            unsafe
            {
                for (int y = okno; y < wysokosc - okno; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                    for (int x = okno; x < szerokosc - okno; x++)
                    {
                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        Rgb pikselWynikowy;

                        bool czyWarunekSpelniony = true;
                        for (int yi = y - okno; yi <= y + okno; yi++)
                        {
                            pWe = (byte*)(void*)scanWe + yi * strideWe;
                            for (int xi = x - okno; xi <= x + okno; xi++)
                            {
                                Rgb pikselOtoczenia = ((Rgb*)pWe)[xi];

                                czyWarunekSpelniony = czyWarunekSpelniony &&
                                pikselOtoczenia.r == obiekt.r &&
                                pikselOtoczenia.g == obiekt.g &&
                                pikselOtoczenia.b == obiekt.b;
                            }
                        }

                        if (czyWarunekSpelniony == true)
                        {
                            pikselWynikowy = obiekt;
                        }
                        else
                        {
                            pikselWynikowy = tlo;
                        }

                        ((Rgb*)pWy)[x] = pikselWynikowy;

                    }
                }
            }
            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);

            return bitmapaWy;
        }

        internal static Bitmap Szkielet(Bitmap bitmapaWe)
        {
            int wysokosc = bitmapaWe.Height;
            int szerokosc = bitmapaWe.Width;


            Bitmap bitmapaWy = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);
            Bitmap bitmapaPom = new Bitmap(szerokosc, wysokosc, PixelFormat.Format24bppRgb);

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmPomData = bitmapaPom.LockBits(new Rectangle(0, 0, szerokosc, wysokosc), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int strideWe = bmWeData.Stride;
            int strideWy = bmWeData.Stride;
            int stridePom = bmPomData.Stride;

            IntPtr scanWe = bmWeData.Scan0;
            IntPtr scanWy = bmWyData.Scan0;
            IntPtr scanPom = bmPomData.Scan0;

            Rgb obiekt = new Rgb() { r = 0, g = 0, b = 0 };
            Rgb tlo = new Rgb() { r = 255, g = 255, b = 255 };

            unsafe
            {
                for (int y = 0 ; y < wysokosc ; y++)
                {
                    byte* pWe = (byte*)(void*)scanWe + y * strideWe;
                    byte* pPom = (byte*)(void*)scanPom + y * stridePom;

                    for (int x = 0; x < szerokosc; x++)
                    {
                        Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                        ((Rgb*)pPom)[x] = pikselWejsciowy;
                    }
                }

                while(true)
                {
                    bool czyPiselZostalUsuniety = false;

                    for (int y = 1; y < wysokosc - 1; y++)
                    {
                        byte* pWe = (byte*)(void*)scanPom + y * stridePom;
                        byte* pWy = (byte*)(void*)scanWy + y * strideWy;

                        for (int x = 1; x < szerokosc - 1; x++)
                        {
                            Rgb pikselWejsciowy = ((Rgb*)pWe)[x];
                            Rgb pikselWynikowy;

                            bool czyObiekt = pikselWejsciowy.r == obiekt.r &&
                                             pikselWejsciowy.g == obiekt.g &&
                                             pikselWejsciowy.b == obiekt.b;

                            if (czyObiekt)
                            {
                                Rgb lewy_gorny = ((Rgb*)((byte*)(void*)scanPom + (y - 1) * stridePom))[x - 1];
                                Rgb lewy_srodek = ((Rgb*)((byte*)(void*)scanPom + (y) * stridePom))[x - 1];
                                Rgb lewy_dol = ((Rgb*)((byte*)(void*)scanPom + (y + 1) * stridePom))[x - 1];

                                Rgb srodek_gorny = ((Rgb*)((byte*)(void*)scanPom + (y - 1) * stridePom))[x];
                                Rgb srodek_dol = ((Rgb*)((byte*)(void*)scanPom + (y + 1) * stridePom))[x];

                                Rgb prawy_gorny = ((Rgb*)((byte*)(void*)scanPom + (y - 1) * stridePom))[x + 1];
                                Rgb prawy_srodek = ((Rgb*)((byte*)(void*)scanPom + (y) * stridePom))[x + 1];
                                Rgb prawy_dol = ((Rgb*)((byte*)(void*)scanPom + (y + 1) * stridePom))[x + 1];

                                bool czyUsuwamy0 =
                                                 srodek_gorny.r == tlo.r && srodek_gorny.g == tlo.g && srodek_gorny.b == tlo.b &&
                                                 lewy_dol.r == obiekt.r && lewy_dol.g == obiekt.g && lewy_dol.b == obiekt.b &&
                                                 srodek_dol.r == obiekt.r && srodek_dol.g == obiekt.g && srodek_dol.b == obiekt.b &&
                                                 prawy_dol.r == obiekt.r && prawy_dol.g == obiekt.g && prawy_dol.b == obiekt.b;

                                bool czyUsuwamy90 =
                                                 prawy_srodek.r == tlo.r && prawy_srodek.g == tlo.g && prawy_srodek.b == tlo.b &&
                                                 lewy_gorny.r == obiekt.r && lewy_gorny.g == obiekt.g && lewy_gorny.b == obiekt.b &&
                                                 lewy_srodek.r == obiekt.r && lewy_srodek.g == obiekt.g && lewy_srodek.b == obiekt.b &&
                                                 lewy_dol.r == obiekt.r && lewy_dol.g == obiekt.g && lewy_dol.b == obiekt.b;

                                bool czyUsuwamy180 =
                                                 srodek_dol.r == tlo.r && srodek_dol.g == tlo.g && srodek_dol.b == tlo.b &&
                                                 lewy_gorny.r == obiekt.r && lewy_gorny.g == obiekt.g && lewy_gorny.b == obiekt.b &&
                                                 srodek_gorny.r == obiekt.r && srodek_gorny.g == obiekt.g && srodek_gorny.b == obiekt.b &&
                                                 prawy_gorny.r == obiekt.r && prawy_gorny.g == obiekt.g && prawy_gorny.b == obiekt.b;

                                bool czyUsuwamy270 =
                                                lewy_srodek.r == tlo.r && lewy_srodek.g == tlo.g && lewy_srodek.b == tlo.b &&
                                                prawy_gorny.r == obiekt.r && prawy_gorny.g == obiekt.g && prawy_gorny.b == obiekt.b &&
                                                prawy_srodek.r == obiekt.r && prawy_srodek.g == obiekt.g && prawy_srodek.b == obiekt.b &&
                                                prawy_dol.r == obiekt.r && prawy_dol.g == obiekt.g && prawy_dol.b == obiekt.b;

                                if (czyUsuwamy0 || czyUsuwamy90 || czyUsuwamy180 || czyUsuwamy270)
                                {
                                    pikselWynikowy = tlo;
                                    czyPiselZostalUsuniety = true;
                                }
                                else
                                {
                                    pikselWynikowy = obiekt;
                                }

                            }
                            else
                            {
                                pikselWynikowy = tlo;
                            }

                            ((Rgb*)pWy)[x] = pikselWynikowy;
                        }
                    }

                    if (czyPiselZostalUsuniety)
                    {
                        for (int y = 0; y < wysokosc; y++)
                        {
                            byte* pWy = (byte*)(void*)scanWy + y * strideWy;
                            byte* pPom = (byte*)(void*)scanPom + y * stridePom;

                            for (int x = 0; x < szerokosc; x++)
                            {
                                Rgb pikselZWynikamiCzesciowymi = ((Rgb*)pWy)[x];
                                ((Rgb*)pPom)[x] = pikselZWynikamiCzesciowymi;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                    bitmapaPom.UnlockBits(bmPomData);
                    bitmapaWy.UnlockBits(bmWyData);
                    bitmapaWe.UnlockBits(bmWeData);

                    return bitmapaWy;
                }




            }
        }
    }



    



