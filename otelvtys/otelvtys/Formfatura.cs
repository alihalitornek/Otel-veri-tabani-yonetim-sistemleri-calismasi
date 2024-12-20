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
    public partial class Formfatura : Form
    {
        public Formfatura()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM fatura";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int rezervasyon_id;
            decimal toplam_ucret;
            DateTime odeme_tarihi;

            if (!int.TryParse(rezervasyonidcomboBox.Text, out rezervasyon_id))
            {
                MessageBox.Show("Geçerli bir rezervasyon_id girin!");
                islembasarili = false;
            }

            if (!decimal.TryParse(toplamucrettextBox.Text, out toplam_ucret))
            {
                MessageBox.Show("Geçerli bir toplam_ücret girin!");
                islembasarili = false;
            }

            if (!DateTime.TryParse(odemetarihitextBox.Text, out odeme_tarihi))
            {
                MessageBox.Show("Geçerli bir ödeme_tarihi girin! (YYYY-MM-DD)");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(odemeyontemitextBox.Text))
            {
                MessageBox.Show("Ödeme yöntemi boş olamaz!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(aciklamatextBox.Text))
            {
                MessageBox.Show("Açıklama boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO fatura (rezervasyon_id, toplam_ücret, ödeme_tarihi, ödeme_yöntemi, açıklama) VALUES (@p1, @p2, @p3, @p4, @p5)", baglanti);
                ekle.Parameters.AddWithValue("@p1", rezervasyon_id);
                ekle.Parameters.AddWithValue("@p2", toplam_ucret);
                ekle.Parameters.AddWithValue("@p3", odeme_tarihi);
                ekle.Parameters.AddWithValue("@p4", odemeyontemitextBox.Text);
                ekle.Parameters.AddWithValue("@p5", aciklamatextBox.Text);
                ekle.ExecuteNonQuery();
                MessageBox.Show("Fatura başarıyla eklendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int fatura_id;
            if (!int.TryParse(faturaidtextBox.Text, out fatura_id))
            {
                MessageBox.Show("Geçerli bir fatura_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM fatura WHERE fatura_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", fatura_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Fatura başarıyla silindi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int fatura_id, rezervasyon_id;
            decimal toplam_ucret;
            DateTime odeme_tarihi;

            if (!int.TryParse(faturaidtextBox.Text, out fatura_id))
            {
                MessageBox.Show("Geçerli bir fatura_id girin!");
                islembasarili = false;
            }

            if (!int.TryParse(rezervasyonidcomboBox.Text, out rezervasyon_id))
            {
                MessageBox.Show("Geçerli bir rezervasyon_id girin!");
                islembasarili = false;
            }

            if (!decimal.TryParse(toplamucrettextBox.Text, out toplam_ucret))
            {
                MessageBox.Show("Geçerli bir toplam_ücret girin!");
                islembasarili = false;
            }

            if (!DateTime.TryParse(odemetarihitextBox.Text, out odeme_tarihi))
            {
                MessageBox.Show("Geçerli bir ödeme_tarihi girin! (YYYY-MM-DD)");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(odemeyontemitextBox.Text))
            {
                MessageBox.Show("Ödeme yöntemi boş olamaz!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(aciklamatextBox.Text))
            {
                MessageBox.Show("Açıklama boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE fatura SET rezervasyon_id=@p2, toplam_ücret=@p3, ödeme_tarihi=@p4, ödeme_yöntemi=@p5, açıklama=@p6 WHERE fatura_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", fatura_id);
                guncelle.Parameters.AddWithValue("@p2", rezervasyon_id);
                guncelle.Parameters.AddWithValue("@p3", toplam_ucret);
                guncelle.Parameters.AddWithValue("@p4", odeme_tarihi);
                guncelle.Parameters.AddWithValue("@p5", odemeyontemitextBox.Text);
                guncelle.Parameters.AddWithValue("@p6", aciklamatextBox.Text);
                guncelle.ExecuteNonQuery();
                MessageBox.Show("Fatura başarıyla güncellendi.");
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

            string sorgu = "SELECT * FROM fatura WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int fatura_id;
            if (int.TryParse(faturaidtextBox.Text, out fatura_id))
            {
                sorgu += " AND fatura_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", fatura_id));
            }

            int rezervasyon_id;
            if (int.TryParse(rezervasyonidcomboBox.Text, out rezervasyon_id))
            {
                sorgu += " AND rezervasyon_id = @p2";
                parameters.Add(new NpgsqlParameter("@p2", rezervasyon_id));
            }

            decimal toplam_ucret;
            if (decimal.TryParse(toplamucrettextBox.Text, out toplam_ucret))
            {
                sorgu += " AND toplam_ücret = @p3";
                parameters.Add(new NpgsqlParameter("@p3", toplam_ucret));
            }

            DateTime odeme_tarihi;
            if (DateTime.TryParse(odemetarihitextBox.Text, out odeme_tarihi))
            {
                sorgu += " AND ödeme_tarihi = @p4";
                parameters.Add(new NpgsqlParameter("@p4", odeme_tarihi));
            }

            if (!string.IsNullOrEmpty(odemeyontemitextBox.Text))
            {
                sorgu += " AND ödeme_yöntemi ILIKE @p5";
                parameters.Add(new NpgsqlParameter("@p5", "%" + odemeyontemitextBox.Text + "%"));
            }

            if (!string.IsNullOrEmpty(aciklamatextBox.Text))
            {
                sorgu += " AND açıklama ILIKE @p6";
                parameters.Add(new NpgsqlParameter("@p6", "%" + aciklamatextBox.Text + "%"));
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

        private void Formfatura_Load(object sender, EventArgs e)
        {
            baglanti.Open();
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
