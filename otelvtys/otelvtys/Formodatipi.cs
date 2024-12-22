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
    public partial class Formodatipi : Form
    {
        public Formodatipi()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM oda_tipi";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int kapasite;
            if (string.IsNullOrEmpty(tipismitextBox.Text))
            {
                MessageBox.Show("Tip ismi boş olamaz!");
                islembasarili = false;
            }

            if (!int.TryParse(kapasitetextBox.Text, out kapasite))
            {
                MessageBox.Show("Geçerli bir kapasite girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO oda_tipi (tip_ismi, kapasite) VALUES (@p1, @p2)", baglanti);
                ekle.Parameters.AddWithValue("@p1", tipismitextBox.Text);
                ekle.Parameters.AddWithValue("@p2", kapasite);
                ekle.ExecuteNonQuery();
                MessageBox.Show("Oda tipi başarıyla eklendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int odatip_id;
            if (!int.TryParse(odatipiidtextBox.Text, out odatip_id))
            {
                MessageBox.Show("Geçerli bir odatip_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM oda_tipi WHERE odatipi_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", odatip_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Oda tipi başarıyla silindi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int odatip_id, kapasite;

            if (!int.TryParse(odatipiidtextBox.Text, out odatip_id))
            {
                MessageBox.Show("Geçerli bir odatip_id girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(tipismitextBox.Text))
            {
                MessageBox.Show("Tip ismi boş olamaz!");
                islembasarili = false;
            }

            if (!int.TryParse(kapasitetextBox.Text, out kapasite))
            {
                MessageBox.Show("Geçerli bir kapasite girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE oda_tipi SET tip_ismi=@p2, kapasite=@p3 WHERE odatipi_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", odatip_id);
                guncelle.Parameters.AddWithValue("@p2", tipismitextBox.Text);
                guncelle.Parameters.AddWithValue("@p3", kapasite);
                guncelle.ExecuteNonQuery();
                MessageBox.Show("Oda tipi başarıyla güncellendi.");
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

            string sorgu = "SELECT * FROM oda_tipi WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int odatip_id;
            if (int.TryParse(odatipiidtextBox.Text, out odatip_id))
            {
                sorgu += " AND odatipi_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", odatip_id));
            }

            if (!string.IsNullOrEmpty(tipismitextBox.Text))
            {
                sorgu += " AND tip_ismi ILIKE @p2";
                parameters.Add(new NpgsqlParameter("@p2", "%" + tipismitextBox.Text + "%"));
            }

            int kapasite;
            if (int.TryParse(kapasitetextBox.Text, out kapasite))
            {
                sorgu += " AND kapasite = @p3";
                parameters.Add(new NpgsqlParameter("@p3", kapasite));
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
