using Npgsql;
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
    public partial class Formpromosyon : Form
    {
        public Formpromosyon()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM promosyon";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            decimal indirim_orani;
            DateTime baslama_tarihi, bitis_tarihi;

            if (string.IsNullOrEmpty(promosyonismitextBox.Text))
            {
                MessageBox.Show("Promosyon ismi boş olamaz!");
                islembasarili = false;
            }

            if (!decimal.TryParse(indirimoranitextBox.Text, out indirim_orani))
            {
                MessageBox.Show("Geçerli bir indirim oranı girin! (Örn: 0.20 veya 20)");
                islembasarili = false;
            }

            if (!DateTime.TryParse(baslamatarihitextBox.Text, out baslama_tarihi))
            {
                MessageBox.Show("Geçerli bir başlama tarihi girin! (YYYY-MM-DD)");
                islembasarili = false;
            }

            if (!DateTime.TryParse(bitistarihitextBox.Text, out bitis_tarihi))
            {
                MessageBox.Show("Geçerli bir bitiş tarihi girin! (YYYY-MM-DD)");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO promosyon (promosyon_ismi, indirim_oranı, başlama_tarihi, bitiş_tarihi) VALUES (@p1, @p2, @p3, @p4)", baglanti);
                ekle.Parameters.AddWithValue("@p1", promosyonismitextBox.Text);
                ekle.Parameters.AddWithValue("@p2", indirim_orani);
                ekle.Parameters.AddWithValue("@p3", baslama_tarihi);
                ekle.Parameters.AddWithValue("@p4", bitis_tarihi);
                ekle.ExecuteNonQuery();
                MessageBox.Show("Promosyon başarıyla eklendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int promosyon_id;
            if (!int.TryParse(promosyonidtextBox.Text, out promosyon_id))
            {
                MessageBox.Show("Geçerli bir promosyon_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM promosyon WHERE promosyon_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", promosyon_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Promosyon başarıyla silindi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int promosyon_id;
            decimal indirim_orani;
            DateTime baslama_tarihi, bitis_tarihi;

            if (!int.TryParse(promosyonidtextBox.Text, out promosyon_id))
            {
                MessageBox.Show("Geçerli bir promosyon_id girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(promosyonismitextBox.Text))
            {
                MessageBox.Show("Promosyon ismi boş olamaz!");
                islembasarili = false;
            }

            if (!decimal.TryParse(indirimoranitextBox.Text, out indirim_orani))
            {
                MessageBox.Show("Geçerli bir indirim oranı girin!");
                islembasarili = false;
            }

            if (!DateTime.TryParse(baslamatarihitextBox.Text, out baslama_tarihi))
            {
                MessageBox.Show("Geçerli bir baslama_tarihi girin! (YYYY-MM-DD)");
                islembasarili = false;
            }

            if (!DateTime.TryParse(bitistarihitextBox.Text, out bitis_tarihi))
            {
                MessageBox.Show("Geçerli bir bitis_tarihi girin! (YYYY-MM-DD)");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE promosyon SET promosyon_ismi=@p2, indirim_oranı=@p3, başlama_tarihi=@p4, bitiş_tarihi=@p5 WHERE promosyon_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", promosyon_id);
                guncelle.Parameters.AddWithValue("@p2", promosyonismitextBox.Text);
                guncelle.Parameters.AddWithValue("@p3", indirim_orani);
                guncelle.Parameters.AddWithValue("@p4", baslama_tarihi);
                guncelle.Parameters.AddWithValue("@p5", bitis_tarihi);
                guncelle.ExecuteNonQuery();
                MessageBox.Show("Promosyon başarıyla güncellendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void arabutton_Click(object sender, EventArgs e)
        {
            if (baglanti.State != ConnectionState.Open)
            {
                baglanti.Open();
            }

            string sorgu = "SELECT * FROM promosyon WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int promosyon_id;
            if (int.TryParse(promosyonidtextBox.Text, out promosyon_id))
            {
                sorgu += " AND promosyon_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", promosyon_id));
            }

            if (!string.IsNullOrEmpty(promosyonismitextBox.Text))
            {
                sorgu += " AND promosyon_ismi ILIKE @p2";
                parameters.Add(new NpgsqlParameter("@p2", "%" + promosyonismitextBox.Text + "%"));
            }

            decimal indirim_orani;
            if (decimal.TryParse(indirimoranitextBox.Text, out indirim_orani))
            {
                // Tam eşleşme ile arama yapıyoruz
                sorgu += " AND indirim_oranı = @p3";
                parameters.Add(new NpgsqlParameter("@p3", indirim_orani));
            }

            DateTime baslama_tarihi;
            if (DateTime.TryParse(baslamatarihitextBox.Text, out baslama_tarihi))
            {
                sorgu += " AND başlama_tarihi = @p4";
                parameters.Add(new NpgsqlParameter("@p4", baslama_tarihi));
            }

            DateTime bitis_tarihi;
            if (DateTime.TryParse(bitistarihitextBox.Text, out bitis_tarihi))
            {
                sorgu += " AND bitiş_tarihi = @p5";
                parameters.Add(new NpgsqlParameter("@p5", bitis_tarihi));
            }

            NpgsqlCommand komut = new NpgsqlCommand(sorgu, baglanti);
            foreach (var param in parameters)
            {
                komut.Parameters.Add(param);
            }

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(komut);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];

            baglanti.Close();
        }
    }
}
