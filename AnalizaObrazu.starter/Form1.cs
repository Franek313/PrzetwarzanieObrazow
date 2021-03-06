using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnalizaObrazu
{
    public partial class Form1 : Form
    {        
        private Bitmap _bitmapa;

        public Form1()
        {
            InitializeComponent();
        }

        private void otworzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _bitmapa = new Bitmap(this.openFileDialog.FileName);
                this.mainPictureBox.Image = _bitmapa;
            }
        }

        private void negatywToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Negatyw(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void jasnośćToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ToDo
            Bitmap bitmapaWynikowa = Efekty.Jasnosc(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }


        private void jasnośćWażonaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.JasnoscWazona(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.ZmianaJasnosci(_bitmapa,50);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.ZmianaJasnosci(_bitmapa, 25);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.ZmianaJasnosci(_bitmapa, -25);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.ZmianaJasnosci(_bitmapa, -50);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.ZmianaJasnosci(_bitmapa, 0);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void logarytmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Logarytm(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void histogramJasnościToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.HistogramJasnosci(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void lutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.ZmianaJasnosciLut(_bitmapa, 25);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void wyrównywanieHistogramuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.WyrownywanieHistogramu(_bitmapa, 25);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void czerńIBielToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void progowanieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Progowanie(_bitmapa, 50);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void progowanie100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Progowanie(_bitmapa, 100);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void progowanie150ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Progowanie(_bitmapa, 150);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void progowanie200ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Progowanie(_bitmapa, 200);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void progowanie50KeepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.ProgowanieKeep(_bitmapa, 100);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void ErozjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
           byte prog = Efekty.WyznaczProgowanieOtsu(_bitmapa);
            Bitmap bitmapaCzarnoBiala = Efekty.Progowanie(_bitmapa, prog);

            Bitmap bitmapaWynikowa = Efekty.Erozja(bitmapaCzarnoBiala, 1);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void dylatacjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte prog = Efekty.WyznaczProgowanieOtsu(_bitmapa);
            Bitmap bitmapaCzarnoBiala = Efekty.Progowanie(_bitmapa, prog);

            Bitmap bitmapaWynikowa = Efekty.Dylatacja(bitmapaCzarnoBiala, 1);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void erozjaDylatacjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte prog = Efekty.WyznaczProgowanieOtsu(_bitmapa);
            Bitmap bitmapaCzarnoBiala = Efekty.Progowanie(_bitmapa, prog);
            Bitmap bitmapaWynikowa = Efekty.Erozja(bitmapaCzarnoBiala, 1);
            bitmapaWynikowa = Efekty.Dylatacja(bitmapaWynikowa, 1);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void dylatacjaErozjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte prog = Efekty.WyznaczProgowanieOtsu(_bitmapa);
            Bitmap bitmapaCzarnoBiala = Efekty.Progowanie(_bitmapa, prog);
            Bitmap bitmapaWynikowa = Efekty.Dylatacja(bitmapaCzarnoBiala, 1);
            bitmapaWynikowa = Efekty.Erozja(bitmapaWynikowa, 1);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void konturWewnętrznyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte prog = Efekty.WyznaczProgowanieOtsu(_bitmapa);
            Bitmap bitmapaCzarnoBiala = Efekty.Progowanie(_bitmapa, prog);

            Bitmap bitmapaWynikowa = Efekty.KonturWewnetrzny(bitmapaCzarnoBiala, 1);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void zmianaKontrastuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.ZmianaKontrastu(_bitmapa, 0.7);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void ścienianieToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Scienianie(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void szkieletToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Szkielet(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void usredniajacyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.FiltrUsredniajacy(_bitmapa, 3);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void usredniajacyMaskaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.FiltrUsredniajacy(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void gaussToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.FiltrGaussa(_bitmapa);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void medianowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.FiltrMedianowy(_bitmapa, 5);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void zmianaHSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // Bitmap bitmapaWynikowa = Efekty.ZmianaHSV(_bitmapa, 40, 20, 10);
           // this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void x2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Skalowanie(_bitmapa, 2.0f);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void x25ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Skalowanie(_bitmapa, 2.5f);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmapaWynikowa = Efekty.Skalowanie(_bitmapa, 3.0f);
            this.mainPictureBox.Image = bitmapaWynikowa;
        }
    }
}
