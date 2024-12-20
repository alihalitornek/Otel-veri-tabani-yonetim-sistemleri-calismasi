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
    public partial class Formetkinlik : Form
    {
        public Formetkinlik()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM etkinlik";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            // Giriş kontrolleri
            if (string.IsNullOrEmpty(etkinlikismitextBox.Text))
            {
                MessageBox.Show("Etkinlik ismi boş olamaz!");
                islembasarili = false;
            }

            DateTime etkinlikTarihi;
            if (!DateTime.TryParse(etkinliktarihitextBox.Text, out etkinlikTarihi))
            {
                MessageBox.Show("Etkinlik tarihi boş olamaz!");
                islembasarili = false;
            }

            int kapasite;
            if (!int.TryParse(kapasitetextBox.Text, out kapasite))
            {
                MessageBox.Show("Geçerli bir kapasite girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO etkinlik (etkinlik_ismi, etkinlik_tarihi, kapasite) VALUES (@p1, @p2, @p3)", baglanti);
                ekle.Parameters.AddWithValue("@p1", etkinlikismitextBox.Text);
                ekle.Parameters.AddWithValue("@p2", etkinlikTarihi);
                ekle.Parameters.AddWithValue("@p3", kapasite);
                ekle.ExecuteNonQuery();
                MessageBox.Show("Etkinlik başarıyla eklendi.");
            }

            baglanti.Close();
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int etkinlik_id;
            if (!int.TryParse(etkinlikidtextBox.Text, out etkinlik_id))
            {
                MessageBox.Show("Geçerli bir etkinlik_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM etkinlik WHERE etkinlik_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", etkinlik_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Etkinlik başarıyla silindi.");
            }

            baglanti.Close();
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int etkinlik_id;
            if (!int.TryParse(etkinlikidtextBox.Text, out etkinlik_id))
            {
                MessageBox.Show("Geçerli bir etkinlik_id girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(etkinlikismitextBox.Text))
            {
                MessageBox.Show("Etkinlik ismi boş olamaz!");
                islembasarili = false;
            }

            DateTime etkinlikTarihi;
            if (!DateTime.TryParse(etkinliktarihitextBox.Text, out etkinlikTarihi))
            {
                MessageBox.Show("Etkinlik tarihi boş olamaz!");
                islembasarili = false;
            }


            int kapasite;
            if (!int.TryParse(kapasitetextBox.Text, out kapasite))
            {
                MessageBox.Show("Geçerli bir kapasite girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE etkinlik SET etkinlik_ismi=@p2, etkinlik_tarihi=@p3, kapasite=@p4 WHERE etkinlik_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", etkinlik_id);
                guncelle.Parameters.AddWithValue("@p2", etkinlikismitextBox.Text);
                guncelle.Parameters.AddWithValue("@p3", etkinlikTarihi);
                guncelle.Parameters.AddWithValue("@p4", kapasite);
                guncelle.ExecuteNonQuery();
                MessageBox.Show("Etkinlik başarıyla güncellendi.");
            }

            baglanti.Close();
        }

        private void arabutton_Click(object sender, EventArgs e)
        {
            if (baglanti.State != ConnectionState.Open)
            {
                baglanti.Open();
            }

            string sorgu = "SELECT * FROM etkinlik WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int etkinlik_id;
            if (int.TryParse(etkinlikidtextBox.Text, out etkinlik_id))
            {
                sorgu += " AND etkinlik_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", etkinlik_id));
            }

            if (!string.IsNullOrEmpty(etkinlikismitextBox.Text))
            {
                sorgu += " AND etkinlik_ismi ILIKE @p2";
                parameters.Add(new NpgsqlParameter("@p2", "%" + etkinlikismitextBox.Text + "%"));
            }

            if (!string.IsNullOrEmpty(etkinliktarihitextBox.Text))
            {
                sorgu += " AND CAST(etkinlik_tarihi AS TEXT) ILIKE @p3";
                parameters.Add(new NpgsqlParameter("@p3", "%" + etkinliktarihitextBox.Text + "%"));

            }

            int kapasite;
            if (int.TryParse(kapasitetextBox.Text, out kapasite))
            {
                sorgu += " AND kapasite = @p4";
                parameters.Add(new NpgsqlParameter("@p4", kapasite));
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
