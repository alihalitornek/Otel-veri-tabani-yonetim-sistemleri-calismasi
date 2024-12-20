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
    public partial class Formhizmet : Form
    {
        public Formhizmet()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM hizmet";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            // Kontroller
            if (string.IsNullOrEmpty(hizmetismitextBox.Text))
            {
                MessageBox.Show("İsim alanı boş olamaz!");
                islembasarili = false;
            }

            int ucretDeger;
            if (!int.TryParse(ucrettextBox.Text, out ucretDeger))
            {
                MessageBox.Show("Geçerli bir ücret giriniz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO hizmet (hizmet_ismi, ücret) VALUES (@p1, @p2)", baglanti);
                ekle.Parameters.AddWithValue("@p1", hizmetismitextBox.Text);
                ekle.Parameters.AddWithValue("@p2", ucretDeger);
                ekle.ExecuteNonQuery();
                MessageBox.Show("Hizmet başarıyla eklendi.");
            }

            baglanti.Close();
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int hizmet_id;
            if (!int.TryParse(hizmetidtextBox.Text, out hizmet_id))
            {
                MessageBox.Show("Geçerli bir hizmet_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM hizmet WHERE hizmet_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", hizmet_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Hizmet başarıyla silindi.");
            }

            baglanti.Close();
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int hizmet_id;
            if (!int.TryParse(hizmetidtextBox.Text, out hizmet_id))
            {
                MessageBox.Show("Geçerli bir hizmet_id girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(hizmetismitextBox.Text))
            {
                MessageBox.Show("İsim boş olamaz!");
                islembasarili = false;
            }

            int ucretDeger;
            if (!int.TryParse(ucrettextBox.Text, out ucretDeger))
            {
                MessageBox.Show("Geçerli bir ücret girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE hizmet SET hizmet_ismi=@p2, ücret=@p3 WHERE hizmet_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", hizmet_id);
                guncelle.Parameters.AddWithValue("@p2", hizmetismitextBox.Text);
                guncelle.Parameters.AddWithValue("@p3", ucretDeger);
                guncelle.ExecuteNonQuery();
                MessageBox.Show("Hizmet bilgileri başarıyla güncellendi.");
            }

            baglanti.Close();
        }

        private void arabutton_Click(object sender, EventArgs e)
        {
            if (baglanti.State != ConnectionState.Open)
            {
                baglanti.Open();
            }

            string sorgu = "SELECT * FROM hizmet WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int hizmet_id;
            if (int.TryParse(hizmetidtextBox.Text, out hizmet_id))
            {
                sorgu += " AND hizmet_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", hizmet_id));
            }

            if (!string.IsNullOrEmpty(hizmetismitextBox.Text))
            {
                sorgu += " AND hizmet_ismi ILIKE @p2";
                parameters.Add(new NpgsqlParameter("@p2", "%" + hizmetismitextBox.Text + "%"));
            }

            int ucretDeger;
            if (int.TryParse(ucrettextBox.Text, out ucretDeger))
            {
                sorgu += " AND ücret = @p3";
                parameters.Add(new NpgsqlParameter("@p3", ucretDeger));
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
