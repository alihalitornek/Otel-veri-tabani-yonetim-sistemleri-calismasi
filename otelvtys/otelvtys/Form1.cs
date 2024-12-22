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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void adrestextBox_TextChanged(object sender, EventArgs e)
        {

        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM kişi";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            string isim = isimtextBox.Text;
            string soyisim = soyisimtextBox.Text;
            string kisitipi = kisitipicomboBox.Text;

            long telefon;
            if (string.IsNullOrEmpty(isim))
            {
                MessageBox.Show("İsim boş olamaz!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(soyisim))
            {
                MessageBox.Show("Soyisim boş olamaz!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(kisitipi))
            {
                MessageBox.Show("Kişi tipi boş olamaz!");
                islembasarili = false;
            }

            if (!long.TryParse(telefontextBox.Text, out telefon))
            {
                MessageBox.Show("Geçerli bir telefon girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO kişi (isim, soyisim, telefon, kişi_tipi) VALUES (@p1, @p2, @p3, @p4)", baglanti);
                ekle.Parameters.AddWithValue("@p1", isim);
                ekle.Parameters.AddWithValue("@p2", soyisim);
                ekle.Parameters.AddWithValue("@p3", telefon);
                ekle.Parameters.AddWithValue("@p4", kisitipi);
                ekle.ExecuteNonQuery();
                MessageBox.Show("Kişi başarıyla eklendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);

        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int kisi_id;
            if (!int.TryParse(kisiidtextBox.Text, out kisi_id))
            {
                MessageBox.Show("Geçerli bir kişi_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM kişi WHERE kişi_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", kisi_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Kişi başarıyla silindi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);

        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int kisi_id;
            long telefon;
            if (!int.TryParse(kisiidtextBox.Text, out kisi_id))
            {
                MessageBox.Show("Geçerli bir kişi_id girin!");
                islembasarili = false;
            }

            string isim = isimtextBox.Text;
            string soyisim = soyisimtextBox.Text;
            string kisitipi = kisitipicomboBox.Text;

            if (string.IsNullOrEmpty(isim))
            {
                MessageBox.Show("İsim boş olamaz!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(soyisim))
            {
                MessageBox.Show("Soyisim boş olamaz!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(kisitipi))
            {
                MessageBox.Show("Kişi tipi boş olamaz!");
                islembasarili = false;
            }

            if (!long.TryParse(telefontextBox.Text, out telefon))
            {
                MessageBox.Show("Geçerli bir telefon girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE kişi SET isim=@p2, soyisim=@p3, telefon=@p4, kişi_tipi=@p5 WHERE kişi_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", kisi_id);
                guncelle.Parameters.AddWithValue("@p2", isim);
                guncelle.Parameters.AddWithValue("@p3", soyisim);
                guncelle.Parameters.AddWithValue("@p4", telefon);
                guncelle.Parameters.AddWithValue("@p5", kisitipi);
                guncelle.ExecuteNonQuery();
                MessageBox.Show("Kişi başarıyla güncellendi.");
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

            string sorgu = "SELECT * FROM kişi WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int kisi_id;
            if (int.TryParse(kisiidtextBox.Text, out kisi_id))
            {
                sorgu += " AND kişi_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", kisi_id));
            }

            if (!string.IsNullOrEmpty(isimtextBox.Text))
            {
                sorgu += " AND isim ILIKE @p2";
                parameters.Add(new NpgsqlParameter("@p2", "%" + isimtextBox.Text + "%"));
            }

            if (!string.IsNullOrEmpty(soyisimtextBox.Text))
            {
                sorgu += " AND soyisim ILIKE @p3";
                parameters.Add(new NpgsqlParameter("@p3", "%" + soyisimtextBox.Text + "%"));
            }

            long telefon;
            if (long.TryParse(telefontextBox.Text, out telefon))
            {
                sorgu += " AND telefon = @p4";
                parameters.Add(new NpgsqlParameter("@p4", telefon));
            }

            if (!string.IsNullOrEmpty(kisitipicomboBox.Text))
            {
                sorgu += " AND kişi_tipi ILIKE @p5";
                parameters.Add(new NpgsqlParameter("@p5", "%" + kisitipicomboBox.Text + "%"));
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
