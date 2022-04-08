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

            BitmapData bmWeData = bitmapaWe.LockBits(new Rectangle(0, 0, szerokosc, wysokosc),ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmWyData = bitmapaWy.LockBits(new Rectangle(0, 0, szerokosc, wysokosc),ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

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

        public static Bitmap ZmianaJasnosci(Bitmap bitmapaWe,int p)
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
            bitmapaWe.UnlockBits(bmWeData); return histogram;
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
                for (int i = 0; i < 256; i++){
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
                for(int j = 0; j<=i; j++)
                {
                    suma += histogram[j];
                }
                dystrybuanta[i] = suma / (double)(wysokosc*szerokosc);
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
            Rgb white  = new Rgb() { r = 255, g = 255, b = 255 };

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
                        else pixelWynikowy = white;

                        ((Rgb*)pWy)[x] = pixelWynikowy;
                    }
                }
            }

            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);
            return bitmapaWy;
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

                        ((Rgb*)pWe)[x] = pixelWynikowy;
                    }
                }
            }

            bitmapaWy.UnlockBits(bmWyData);
            bitmapaWe.UnlockBits(bmWeData);
            return bitmapaWy;
        }
    }



    

}

