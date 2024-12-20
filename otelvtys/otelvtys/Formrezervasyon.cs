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
    public partial class Formrezervasyon : Form
    {
        public Formrezervasyon()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");
        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM rezervasyon";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int kisi_id = 0, oda_id = 0;
            DateTime girisTarihi, cikisTarihi;

            if (!int.TryParse(kisiidcomboBox.Text, out kisi_id))
            {
                MessageBox.Show("Geçerli bir kisi_id girin!");
                islembasarili = false;
            }

            if (!int.TryParse(odaidcomboBox.Text, out oda_id))
            {
                MessageBox.Show("Geçerli bir oda_id girin!");
                islembasarili = false;
            }

            if (!DateTime.TryParse(giristarihitextBox.Text, out girisTarihi))
            {
                MessageBox.Show("Geçerli bir giriş tarihi girin! (YYYY-MM-DD)");
                islembasarili = false;
            }

            if (!DateTime.TryParse(cikistarihitextBox.Text, out cikisTarihi))
            {
                MessageBox.Show("Geçerli bir çıkış tarihi girin! (YYYY-MM-DD)");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(durumtextBox.Text))
            {
                MessageBox.Show("Durum boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO rezervasyon (kişi_id, oda_id, giriş_tarihi, çıkış_tarihi, durum) VALUES (@p1, @p2, @p3, @p4, @p5)", baglanti);
                ekle.Parameters.AddWithValue("@p1", kisi_id);
                ekle.Parameters.AddWithValue("@p2", oda_id);
                ekle.Parameters.AddWithValue("@p3", girisTarihi);
                ekle.Parameters.AddWithValue("@p4", cikisTarihi);
                ekle.Parameters.AddWithValue("@p5", durumtextBox.Text);

                ekle.ExecuteNonQuery();
                MessageBox.Show("Rezervasyon başarıyla eklendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int rezervasyon_id;
            if (!int.TryParse(rezervasyonidtextBox.Text, out rezervasyon_id))
            {
                MessageBox.Show("Geçerli bir rezervasyon_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM rezervasyon WHERE rezervasyon_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", rezervasyon_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Rezervasyon başarıyla silindi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int rezervasyon_id, kisi_id, oda_id;
            DateTime girisTarihi, cikisTarihi;

            if (!int.TryParse(rezervasyonidtextBox.Text, out rezervasyon_id))
            {
                MessageBox.Show("Geçerli bir rezervasyon_id girin!");
                islembasarili = false;
            }

            if (!int.TryParse(kisiidcomboBox.Text, out kisi_id))
            {
                MessageBox.Show("Geçerli bir kisi_id girin!");
                islembasarili = false;
            }

            if (!int.TryParse(odaidcomboBox.Text, out oda_id))
            {
                MessageBox.Show("Geçerli bir oda_id girin!");
                islembasarili = false;
            }

            if (!DateTime.TryParse(giristarihitextBox.Text, out girisTarihi))
            {
                MessageBox.Show("Geçerli bir giriş tarihi girin!");
                islembasarili = false;
            }

            if (!DateTime.TryParse(cikistarihitextBox.Text, out cikisTarihi))
            {
                MessageBox.Show("Geçerli bir çıkış tarihi girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(durumtextBox.Text))
            {
                MessageBox.Show("Durum boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE rezervasyon SET kişi_id=@p2, oda_id=@p3, giriş_tarihi=@p4, çıkış_tarihi=@p5, durum=@p6 WHERE rezervasyon_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", rezervasyon_id);
                guncelle.Parameters.AddWithValue("@p2", kisi_id);
                guncelle.Parameters.AddWithValue("@p3", oda_id);
                guncelle.Parameters.AddWithValue("@p4", girisTarihi);
                guncelle.Parameters.AddWithValue("@p5", cikisTarihi);
                guncelle.Parameters.AddWithValue("@p6", durumtextBox.Text);

                guncelle.ExecuteNonQuery();
                MessageBox.Show("Rezervasyon başarıyla güncellendi.");
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

            string sorgu = "SELECT * FROM rezervasyon WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int rezervasyon_id;
            if (int.TryParse(rezervasyonidtextBox.Text, out rezervasyon_id))
            {
                sorgu += " AND rezervasyon_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", rezervasyon_id));
            }

            int kisi_id;
            if (int.TryParse(kisiidcomboBox.Text, out kisi_id))
            {
                sorgu += " AND kişi_id = @p2";
                parameters.Add(new NpgsqlParameter("@p2", kisi_id));
            }

            int oda_id;
            if (int.TryParse(odaidcomboBox.Text, out oda_id))
            {
                sorgu += " AND oda_id = @p3";
                parameters.Add(new NpgsqlParameter("@p3", oda_id));
            }

            DateTime girisTarihi;
            if (DateTime.TryParse(giristarihitextBox.Text, out girisTarihi))
            {
                sorgu += " AND giriş_tarihi = @p4";
                parameters.Add(new NpgsqlParameter("@p4", girisTarihi));
            }

            DateTime cikisTarihi;
            if (DateTime.TryParse(cikistarihitextBox.Text, out cikisTarihi))
            {
                sorgu += " AND çıkış_tarihi = @p5";
                parameters.Add(new NpgsqlParameter("@p5", cikisTarihi));
            }

            if (!string.IsNullOrEmpty(durumtextBox.Text))
            {
                sorgu += " AND durum ILIKE @p6";
                parameters.Add(new NpgsqlParameter("@p6", "%" + durumtextBox.Text + "%"));
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

        private void Formrezervasyon_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter dataadapter = new NpgsqlDataAdapter("select * from müşteri", baglanti);
            DataTable datatable = new DataTable();
            dataadapter.Fill(datatable);
            kisiidcomboBox.DisplayMember = "kişi_id";
            kisiidcomboBox.ValueMember = "kişi_id";
            kisiidcomboBox.DataSource = datatable;

            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from oda", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            odaidcomboBox.DisplayMember = "oda_id";
            odaidcomboBox.ValueMember = "oda_id";
            odaidcomboBox.DataSource = dt;
            baglanti.Close();
        }
    }
}
