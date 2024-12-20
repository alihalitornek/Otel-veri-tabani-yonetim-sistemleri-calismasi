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
    public partial class Formcalisan : Form
    {
        public Formcalisan()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbotel; user ID=postgres; password=1234");

        private void Formcalisan_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlDataAdapter dataadapter = new NpgsqlDataAdapter("select * from kişi", baglanti);
            DataTable datatable = new DataTable();
            dataadapter.Fill(datatable);
            kisiidcomboBox.DisplayMember = "kişi_id";
            kisiidcomboBox.ValueMember = "kişi_id";
            kisiidcomboBox.DataSource = datatable;
            baglanti.Close();
        }

        private void listelebutton_Click(object sender, EventArgs e)
        {
            string sorgu = "SELECT * FROM çalışan";
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];
        }

        private void eklebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int kisi_id;
            if (!int.TryParse(kisiidcomboBox.Text, out kisi_id))
            {
                MessageBox.Show("Geçerli bir kişi_id girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(kimliknotextBox.Text))
            {
                MessageBox.Show("Kimlik no boş olamaz!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(pozisyontextBox.Text))
            {
                MessageBox.Show("Pozisyon boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand ekle = new NpgsqlCommand("INSERT INTO çalışan (kişi_id, kimlik_no, pozisyon) VALUES (@p1, @p2, @p3)", baglanti);
                ekle.Parameters.AddWithValue("@p1", kisi_id);
                ekle.Parameters.AddWithValue("@p2", kimliknotextBox.Text);
                ekle.Parameters.AddWithValue("@p3", pozisyontextBox.Text);
                ekle.ExecuteNonQuery();
                MessageBox.Show("Çalışan başarıyla eklendi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void silbutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int kisi_id;
            if (!int.TryParse(kisiidcomboBox.Text, out kisi_id))
            {
                MessageBox.Show("Geçerli bir kişi_id girin!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand sil = new NpgsqlCommand("DELETE FROM çalışan WHERE kişi_id=@p1", baglanti);
                sil.Parameters.AddWithValue("@p1", kisi_id);
                sil.ExecuteNonQuery();
                MessageBox.Show("Çalışan başarıyla silindi.");
            }

            baglanti.Close();
            listelebutton_Click(null, null);
        }

        private void güncellebutton_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            bool islembasarili = true;

            int kisi_id;
            if (!int.TryParse(kisiidcomboBox.Text, out kisi_id))
            {
                MessageBox.Show("Geçerli bir kişi_id girin!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(kimliknotextBox.Text))
            {
                MessageBox.Show("Kimlik no boş olamaz!");
                islembasarili = false;
            }

            if (string.IsNullOrEmpty(pozisyontextBox.Text))
            {
                MessageBox.Show("Pozisyon boş olamaz!");
                islembasarili = false;
            }

            if (islembasarili)
            {
                NpgsqlCommand guncelle = new NpgsqlCommand("UPDATE çalışan SET kimlik_no=@p2, pozisyon=@p3 WHERE kişi_id=@p1", baglanti);
                guncelle.Parameters.AddWithValue("@p1", kisi_id);
                guncelle.Parameters.AddWithValue("@p2", kimliknotextBox.Text);
                guncelle.Parameters.AddWithValue("@p3", pozisyontextBox.Text);
                guncelle.ExecuteNonQuery();
                MessageBox.Show("Çalışan bilgileri başarıyla güncellendi.");
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

            string sorgu = "SELECT * FROM çalışan WHERE 1=1";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            int kisi_id;
            if (int.TryParse(kisiidcomboBox.Text, out kisi_id))
            {
                sorgu += " AND kişi_id = @p1";
                parameters.Add(new NpgsqlParameter("@p1", kisi_id));
            }

            if (!string.IsNullOrEmpty(kimliknotextBox.Text))
            {
                // Kimlik no metinsel bir alan, ILIKE ile kısmi arama yapıyoruz.
                sorgu += " AND kimlik_no ILIKE @p2";
                parameters.Add(new NpgsqlParameter("@p2", "%" + kimliknotextBox.Text + "%"));
            }

            if (!string.IsNullOrEmpty(pozisyontextBox.Text))
            {
                sorgu += " AND pozisyon ILIKE @p3";
                parameters.Add(new NpgsqlParameter("@p3", "%" + pozisyontextBox.Text + "%"));
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
