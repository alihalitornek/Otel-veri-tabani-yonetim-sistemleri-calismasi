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
    public partial class Formoda : Form
    {
        public Formoda()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void Formoda_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from oda_tipi", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            odatipicomboBox.DisplayMember = "odatipi_id";
            odatipicomboBox.ValueMember = "odatipi_id";
            odatipicomboBox.DataSource = dt;
            baglanti.Close();
        }

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM oda";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int oda_tipi, kat;
            decimal gecelik_ucret;

            if (string.IsNullOrEmpty(odaismitextBox.Text))
            {
                MessageBox.Show("Oda ismi boş olamaz!");
                islembasarili = false;
            }

            if (!int.TryParse(odatipicomboBox.Text, out oda_tipi))
            {
                MessageBox.Show("Geçerli bir oda_tipi girin (int)!");
                islembasarili = false;
            }

            if (!int.TryParse(kattextBox.Text, out kat))
            {
                MessageBox.Show("Geçerli bir kat girin (int)!");
                islembasarili = false;
            }

            if (!decimal.TryParse(gecelikucrettextBox.Text, out gecelik_ucret))
            {
                MessageBox.Show("Geçerli bir gecik_ücret girin (örn. 100.50)!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(durumtextBox.Text))
            {
                MessageBox.Show("Durum boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO oda (oda_ismi, oda_tipi, kat, gecelik_ücret, durum) VALUES (@p1, @p2, @p3, @p4, @p5)", baglanti);
                ekle.Parameters.AddWithValue("@p1", odaismitextBox.Text);
                ekle.Parameters.AddWithValue("@p2", oda_tipi);
                ekle.Parameters.AddWithValue("@p3", kat);
                ekle.Parameters.AddWithValue("@p4", gecelik_ucret);
                ekle.Parameters.AddWithValue("@p5", durumtextBox.Text);
                ekle.ExecuteNonQuery();
                MessageBox.Show("Oda başarıyla eklendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int oda_id;
            if (!int.TryParse(odaidtextBox.Text, out oda_id))
            {
                MessageBox.Show("Geçerli bir oda_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM oda WHERE oda_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", oda_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Oda başarıyla silindi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int oda_id, oda_tipi, kat;
            decimal gecik_ucret;

            if (!int.TryParse(odaidtextBox.Text, out oda_id))
            {
                MessageBox.Show("Geçerli bir oda_id girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(odaismitextBox.Text))
            {
                MessageBox.Show("Oda ismi boş olamaz!");
                islembasarili = false;
            }

            if (!int.TryParse(odatipicomboBox.Text, out oda_tipi))
            {
                MessageBox.Show("Geçerli bir oda_tipi girin (int)!");
                islembasarili = false;
            }

            if (!int.TryParse(kattextBox.Text, out kat))
            {
                MessageBox.Show("Geçerli bir kat girin (int)!");
                islembasarili = false;
            }

            if (!decimal.TryParse(gecelikucrettextBox.Text, out gecik_ucret))
            {
                MessageBox.Show("Geçerli bir gecik_ücret girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(durumtextBox.Text))
            {
                MessageBox.Show("Durum boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE oda SET oda_ismi=@p2, oda_tipi=@p3, kat=@p4, gecelik_ücret=@p5, durum=@p6 WHERE oda_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", oda_id);
                guncelle.Parameters.AddWithValue("@p2", odaismitextBox.Text);
                guncelle.Parameters.AddWithValue("@p3", oda_tipi);
                guncelle.Parameters.AddWithValue("@p4", kat);
                guncelle.Parameters.AddWithValue("@p5", gecik_ucret);
                guncelle.Parameters.AddWithValue("@p6", durumtextBox.Text);
                guncelle.ExecuteNonQuery();
                MessageBox.Show("Oda başarıyla güncellendi.");
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

            string sorgu = "SELECT * FROM oda WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int oda_id;
            if (int.TryParse(odaidtextBox.Text, out oda_id))
            {
                sorgu += " AND oda_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", oda_id));
            }

            if (!string.IsNullOrEmpty(odaismitextBox.Text))
            {
                sorgu += " AND oda_ismi ILIKE @p2";
                parameters.Add(new NpgsqlParameter("@p2", "%" + odaismitextBox.Text + "%"));
            }

            int oda_tipi;
            if (int.TryParse(odatipicomboBox.Text, out oda_tipi))
            {
                sorgu += " AND oda_tipi = @p3";
                parameters.Add(new NpgsqlParameter("@p3", oda_tipi));
            }

            int kat;
            if (int.TryParse(kattextBox.Text, out kat))
            {
                sorgu += " AND kat = @p4";
                parameters.Add(new NpgsqlParameter("@p4", kat));
            }

            decimal gecik_ucret;
            if (decimal.TryParse(gecelikucrettextBox.Text, out gecik_ucret))
            {
                sorgu += " AND gecelik_ücret = @p5";
                parameters.Add(new NpgsqlParameter("@p5", gecik_ucret));
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
    }
}
