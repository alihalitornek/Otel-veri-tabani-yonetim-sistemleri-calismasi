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
    public partial class Formbakimtalebi : Form
    {
        public Formbakimtalebi()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM bakım_talebi";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int oda_id;
            DateTime talep_tarihi;

            if (!int.TryParse(odaidcomboBox.Text, out oda_id))
            {
                MessageBox.Show("Geçerli bir oda_id girin!");
                islembasarili = false;
            }

            if (!DateTime.TryParse(taleptarihitextBox.Text, out talep_tarihi))
            {
                MessageBox.Show("Geçerli bir talep_tarihi girin! (YYYY-MM-DD)");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(durumtextBox.Text))
            {
                MessageBox.Show("Durum boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO bakım_talebi (oda_id, talep_tarihi, durum) VALUES (@p1, @p2, @p3)", baglanti);
                ekle.Parameters.AddWithValue("@p1", oda_id);
                ekle.Parameters.AddWithValue("@p2", talep_tarihi);
                ekle.Parameters.AddWithValue("@p3", durumtextBox.Text);
                ekle.ExecuteNonQuery();
                MessageBox.Show("Bakım talebi başarıyla eklendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int talep_id;
            if (!int.TryParse(talepidtextBox.Text, out talep_id))
            {
                MessageBox.Show("Geçerli bir talep_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM bakım_talebi WHERE talep_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", talep_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Bakım talebi başarıyla silindi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int talep_id, oda_id;
            DateTime talep_tarihi;

            if (!int.TryParse(talepidtextBox.Text, out talep_id))
            {
                MessageBox.Show("Geçerli bir talep_id girin!");
                islembasarili = false;
            }

            if (!int.TryParse(odaidcomboBox.Text, out oda_id))
            {
                MessageBox.Show("Geçerli bir oda_id girin!");
                islembasarili = false;
            }

            if (!DateTime.TryParse(taleptarihitextBox.Text, out talep_tarihi))
            {
                MessageBox.Show("Geçerli bir talep_tarihi girin! (YYYY-MM-DD)");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(durumtextBox.Text))
            {
                MessageBox.Show("Durum boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE bakım_talebi SET oda_id=@p2, talep_tarihi=@p3, durum=@p4 WHERE talep_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", talep_id);
                guncelle.Parameters.AddWithValue("@p2", oda_id);
                guncelle.Parameters.AddWithValue("@p3", talep_tarihi);
                guncelle.Parameters.AddWithValue("@p4", durumtextBox.Text);
                guncelle.ExecuteNonQuery();
                MessageBox.Show("Bakım talebi başarıyla güncellendi.");
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

            string sorgu = "SELECT * FROM bakım_talebi WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int talep_id;
            if (int.TryParse(talepidtextBox.Text, out talep_id))
            {
                sorgu += " AND talep_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", talep_id));
            }

            int oda_id;
            if (int.TryParse(odaidcomboBox.Text, out oda_id))
            {
                sorgu += " AND oda_id = @p2";
                parameters.Add(new NpgsqlParameter("@p2", oda_id));
            }

            DateTime talep_tarihi;
            if (DateTime.TryParse(taleptarihitextBox.Text, out talep_tarihi))
            {
                sorgu += " AND talep_tarihi = @p3";
                parameters.Add(new NpgsqlParameter("@p3", talep_tarihi));
            }

            if (!string.IsNullOrEmpty(durumtextBox.Text))
            {
                sorgu += " AND durum ILIKE @p4";
                parameters.Add(new NpgsqlParameter("@p4", "%" + durumtextBox.Text + "%"));
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

        private void Formbakimtalebi_Load(object sender, EventArgs e)
        {
            baglanti.Open();
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
