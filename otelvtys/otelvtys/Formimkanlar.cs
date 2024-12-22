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
    public partial class Formimkanlar : Form
    {
        public Formimkanlar()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM imkanlar";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            if (string.IsNullOrEmpty(imkanismitextBox.Text))
            {
                MessageBox.Show("İmkan ismi boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO imkanlar (imkan_isimi) VALUES (@p1)", baglanti);
                ekle.Parameters.AddWithValue("@p1", imkanismitextBox.Text);
                ekle.ExecuteNonQuery();
                MessageBox.Show("İmkan başarıyla eklendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int imkan_id;
            if (!int.TryParse(imkanidtextBox.Text, out imkan_id))
            {
                MessageBox.Show("Geçerli bir imkan_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM imkanlar WHERE imkan_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", imkan_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("İmkan başarıyla silindi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int imkan_id;
            if (!int.TryParse(imkanidtextBox.Text, out imkan_id))
            {
                MessageBox.Show("Geçerli bir imkan_id girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(imkanismitextBox.Text))
            {
                MessageBox.Show("İmkan ismi boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE imkanlar SET imkan_isimi=@p2 WHERE imkan_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", imkan_id);
                guncelle.Parameters.AddWithValue("@p2", imkanismitextBox.Text);
                guncelle.ExecuteNonQuery();
                MessageBox.Show("İmkan başarıyla güncellendi.");
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

            string sorgu = "SELECT * FROM imkanlar WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int imkan_id;
            if (int.TryParse(imkanidtextBox.Text, out imkan_id))
            {
                sorgu += " AND imkan_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", imkan_id));
            }

            if (!string.IsNullOrEmpty(imkanismitextBox.Text))
            {
                sorgu += " AND imkan_isimi ILIKE @p2";
                parameters.Add(new NpgsqlParameter("@p2", "%" + imkanismitextBox.Text + "%"));
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
