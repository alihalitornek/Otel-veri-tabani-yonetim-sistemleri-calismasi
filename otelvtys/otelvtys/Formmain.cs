using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace otelvtys
{
    public partial class Formmain : Form
    {
        public Formmain()
        {
            InitializeComponent();
        }

        private void kisibutton_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void musteributton_Click(object sender, EventArgs e)
        {
            Formmusteri formmusteri = new Formmusteri();
            formmusteri.Show();
        }

        private void calisanbutton_Click(object sender, EventArgs e)
        {
            Formcalisan formcalisan = new Formcalisan();
            formcalisan.Show();
        }

        private void rezervasyonbutton_Click(object sender, EventArgs e)
        {
            Formrezervasyon formrezervasyon = new Formrezervasyon();
            formrezervasyon.Show();
        }

        private void etkinlikbutton_Click(object sender, EventArgs e)
        {
            Formetkinlik formetkinlik = new Formetkinlik();
            formetkinlik.Show();
        }

        private void hizmetbutton_Click(object sender, EventArgs e)
        {
            Formhizmet formhizmet = new Formhizmet();
            formhizmet.Show();
        }

        private void geribildirimbutton_Click(object sender, EventArgs e)
        {
            Formgeribildirim formgeribildirim = new Formgeribildirim();
            formgeribildirim.Show();
        }

        private void faturabutton_Click(object sender, EventArgs e)
        {
            Formfatura formfatura = new Formfatura();
            formfatura.Show();
        }

        private void promosyonbutton_Click(object sender, EventArgs e)
        {
            Formpromosyon formpromosyon = new Formpromosyon();
            formpromosyon.Show();
        }

        private void odabutton_Click(object sender, EventArgs e)
        {
            Formoda formoda = new Formoda();
            formoda.Show();
        }

        private void bakimtalebibutton_Click(object sender, EventArgs e)
        {
            Formbakimtalebi formbakimtalebi = new Formbakimtalebi();
            formbakimtalebi.Show();
        }

        private void odatipibutton_Click(object sender, EventArgs e)
        {
            Formodatipi formodatipi = new Formodatipi();
            formodatipi.Show();
        }

        private void imkanlarbutton_Click(object sender, EventArgs e)
        {
            Formimkanlar formimkanlar = new Formimkanlar();
            formimkanlar.Show();
        }
    }
}
