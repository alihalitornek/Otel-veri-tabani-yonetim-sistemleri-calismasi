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
    public partial class Formgeribildirim : Form
    {
        public Formgeribildirim()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM geri_bildirim";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int kisi_id, rezervasyon_id, puan;

            if (!int.TryParse(kisiidcomboBox.Text, out kisi_id))
            {
                MessageBox.Show("Geçerli bir kişi_id girin!");
                islembasarili = false;
            }

            if (!int.TryParse(rezervasyonidcomboBox.Text, out rezervasyon_id))
            {
                MessageBox.Show("Geçerli bir rezervasyon_id girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(yorumtextBox.Text))
            {
                MessageBox.Show("Yorum boş olamaz!");
                islembasarili = false;
            }

            if (!int.TryParse(puantextBox.Text, out puan))
            {
                MessageBox.Show("Geçerli bir puan girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO geri_bildirim (kişi_id, rezervasyon_id, yorum, puan) VALUES (@p1, @p2, @p3, @p4)", baglanti);
                ekle.Parameters.AddWithValue("@p1", kisi_id);
                ekle.Parameters.AddWithValue("@p2", rezervasyon_id);
                ekle.Parameters.AddWithValue("@p3", yorumtextBox.Text);
                ekle.Parameters.AddWithValue("@p4", puan);
                ekle.ExecuteNonQuery();
                MessageBox.Show("Geri bildirim başarıyla eklendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int geri_bildirim_id;
            if (!int.TryParse(geribildirimidtextBox.Text, out geri_bildirim_id))
            {
                MessageBox.Show("Geçerli bir geri_bildirim_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM geri_bildirim WHERE geri_bildirim_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", geri_bildirim_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Geri bildirim başarıyla silindi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int geri_bildirim_id, kisi_id, rezervasyon_id, puan;

            if (!int.TryParse(geribildirimidtextBox.Text, out geri_bildirim_id))
            {
                MessageBox.Show("Geçerli bir geri_bildirim_id girin!");
                islembasarili = false;
            }

            if (!int.TryParse(kisiidcomboBox.Text, out kisi_id))
            {
                MessageBox.Show("Geçerli bir kişi_id girin!");
                islembasarili = false;
            }

            if (!int.TryParse(rezervasyonidcomboBox.Text, out rezervasyon_id))
            {
                MessageBox.Show("Geçerli bir rezervasyon_id girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(yorumtextBox.Text))
            {
                MessageBox.Show("Yorum boş olamaz!");
                islembasarili = false;
            }

            if (!int.TryParse(puantextBox.Text, out puan))
            {
                MessageBox.Show("Geçerli bir puan girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE geri_bildirim SET kişi_id=@p2, rezervasyon_id=@p3, yorum=@p4, puan=@p5 WHERE geri_bildirim_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", geri_bildirim_id);
                guncelle.Parameters.AddWithValue("@p2", kisi_id);
                guncelle.Parameters.AddWithValue("@p3", rezervasyon_id);
                guncelle.Parameters.AddWithValue("@p4", yorumtextBox.Text);
                guncelle.Parameters.AddWithValue("@p5", puan);

                guncelle.ExecuteNonQuery();
                MessageBox.Show("Geri bildirim başarıyla güncellendi.");
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

            string sorgu = "SELECT * FROM geri_bildirim WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int geri_bildirim_id;
            if (int.TryParse(geribildirimidtextBox.Text, out geri_bildirim_id))
            {
                sorgu += " AND geri_bildirim_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", geri_bildirim_id));
            }

            int kisi_id;
            if (int.TryParse(kisiidcomboBox.Text, out kisi_id))
            {
                sorgu += " AND kişi_id = @p2";
                parameters.Add(new NpgsqlParameter("@p2", kisi_id));
            }

            int rezervasyon_id;
            if (int.TryParse(rezervasyonidcomboBox.Text, out rezervasyon_id))
            {
                sorgu += " AND rezervasyon_id = @p3";
                parameters.Add(new NpgsqlParameter("@p3", rezervasyon_id));
            }

            if (!string.IsNullOrEmpty(yorumtextBox.Text))
            {
                // Yorum metinsel bir alan, ILIKE kullanarak kısmi arama yapıyoruz
                sorgu += " AND yorum ILIKE @p4";
                parameters.Add(new NpgsqlParameter("@p4", "%" + yorumtextBox.Text + "%"));
            }

            int puan;
            if (int.TryParse(puantextBox.Text, out puan))
            {
                sorgu += " AND puan = @p5";
                parameters.Add(new NpgsqlParameter("@p5", puan));
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

        private void Formgeribildirim_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter dataadapter = new NpgsqlDataAdapter("select * from kişi", baglanti);
            DataTable datatable = new DataTable();
            dataadapter.Fill(datatable);
            kisiidcomboBox.DisplayMember = "kişi_id";
            kisiidcomboBox.ValueMember = "kişi_id";
            kisiidcomboBox.DataSource = datatable;

            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from rezervasyon", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            rezervasyonidcomboBox.DisplayMember = "rezervasyon_id";
            rezervasyonidcomboBox.ValueMember = "rezervasyon_id";
            rezervasyonidcomboBox.DataSource = dt;
            baglanti.Close();
        }
    }
}
