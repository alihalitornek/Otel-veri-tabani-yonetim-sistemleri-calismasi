namespace otelvtys
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.isimlabel = new System.Windows.Forms.Label();
            this.isimtextBox = new System.Windows.Forms.TextBox();
            this.listelebutton = new System.Windows.Forms.Button();
            this.soyisimtextBox = new System.Windows.Forms.TextBox();
            this.soyisimlabel = new System.Windows.Forms.Label();
            this.eklebutton = new System.Windows.Forms.Button();
            this.silbutton = new System.Windows.Forms.Button();
            this.güncellebutton = new System.Windows.Forms.Button();
            this.arabutton = new System.Windows.Forms.Button();
            this.telefontextBox = new System.Windows.Forms.TextBox();
            this.telefonlabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.kisitipicomboBox = new System.Windows.Forms.ComboBox();
            this.kisiidtextBox = new System.Windows.Forms.TextBox();
            this.kisiidlabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 12);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(477, 314);
            this.dataGridView2.TabIndex = 0;
            // 
            // isimlabel
            // 
            this.isimlabel.AutoSize = true;
            this.isimlabel.Location = new System.Drawing.Point(527, 63);
            this.isimlabel.Name = "isimlabel";
            this.isimlabel.Size = new System.Drawing.Size(31, 16);
            this.isimlabel.TabIndex = 1;
            this.isimlabel.Text = "isim";
            // 
            // isimtextBox
            // 
            this.isimtextBox.Location = new System.Drawing.Point(627, 57);
            this.isimtextBox.Name = "isimtextBox";
            this.isimtextBox.Size = new System.Drawing.Size(100, 22);
            this.isimtextBox.TabIndex = 2;
            // 
            // listelebutton
            // 
            this.listelebutton.Location = new System.Drawing.Point(530, 191);
            this.listelebutton.Name = "listelebutton";
            this.listelebutton.Size = new System.Drawing.Size(75, 23);
            this.listelebutton.TabIndex = 3;
            this.listelebutton.Text = "listele";
            this.listelebutton.UseVisualStyleBackColor = true;
            this.listelebutton.Click += new System.EventHandler(this.listelebutton_Click);
            // 
            // soyisimtextBox
            // 
            this.soyisimtextBox.Location = new System.Drawing.Point(627, 85);
            this.soyisimtextBox.Name = "soyisimtextBox";
            this.soyisimtextBox.Size = new System.Drawing.Size(100, 22);
            this.soyisimtextBox.TabIndex = 5;
            // 
            // soyisimlabel
            // 
            this.soyisimlabel.AutoSize = true;
            this.soyisimlabel.Location = new System.Drawing.Point(527, 91);
            this.soyisimlabel.Name = "soyisimlabel";
            this.soyisimlabel.Size = new System.Drawing.Size(53, 16);
            this.soyisimlabel.TabIndex = 4;
            this.soyisimlabel.Text = "soyisim";
            // 
            // eklebutton
            // 
            this.eklebutton.Location = new System.Drawing.Point(652, 191);
            this.eklebutton.Name = "eklebutton";
            this.eklebutton.Size = new System.Drawing.Size(75, 23);
            this.eklebutton.TabIndex = 6;
            this.eklebutton.Text = "ekle";
            this.eklebutton.UseVisualStyleBackColor = true;
            this.eklebutton.Click += new System.EventHandler(this.eklebutton_Click);
            // 
            // silbutton
            // 
            this.silbutton.Location = new System.Drawing.Point(530, 235);
            this.silbutton.Name = "silbutton";
            this.silbutton.Size = new System.Drawing.Size(75, 23);
            this.silbutton.TabIndex = 7;
            this.silbutton.Text = "sil";
            this.silbutton.UseVisualStyleBackColor = true;
            this.silbutton.Click += new System.EventHandler(this.silbutton_Click);
            // 
            // güncellebutton
            // 
            this.güncellebutton.Location = new System.Drawing.Point(586, 281);
            this.güncellebutton.Name = "güncellebutton";
            this.güncellebutton.Size = new System.Drawing.Size(75, 23);
            this.güncellebutton.TabIndex = 8;
            this.güncellebutton.Text = "güncelle";
            this.güncellebutton.UseVisualStyleBackColor = true;
            this.güncellebutton.Click += new System.EventHandler(this.güncellebutton_Click);
            // 
            // arabutton
            // 
            this.arabutton.Location = new System.Drawing.Point(652, 235);
            this.arabutton.Name = "arabutton";
            this.arabutton.Size = new System.Drawing.Size(75, 23);
            this.arabutton.TabIndex = 9;
            this.arabutton.Text = "ara";
            this.arabutton.UseVisualStyleBackColor = true;
            this.arabutton.Click += new System.EventHandler(this.arabutton_Click);
            // 
            // telefontextBox
            // 
            this.telefontextBox.Location = new System.Drawing.Point(627, 113);
            this.telefontextBox.Name = "telefontextBox";
            this.telefontextBox.Size = new System.Drawing.Size(100, 22);
            this.telefontextBox.TabIndex = 11;
            // 
            // telefonlabel
            // 
            this.telefonlabel.AutoSize = true;
            this.telefonlabel.Location = new System.Drawing.Point(527, 119);
            this.telefonlabel.Name = "telefonlabel";
            this.telefonlabel.Size = new System.Drawing.Size(47, 16);
            this.telefonlabel.TabIndex = 10;
            this.telefonlabel.Text = "telefon";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(527, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "kişi tipi";
            // 
            // kisitipicomboBox
            // 
            this.kisitipicomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kisitipicomboBox.FormattingEnabled = true;
            this.kisitipicomboBox.Items.AddRange(new object[] {
            "müşteri",
            "çalışan"});
            this.kisitipicomboBox.Location = new System.Drawing.Point(627, 141);
            this.kisitipicomboBox.Name = "kisitipicomboBox";
            this.kisitipicomboBox.Size = new System.Drawing.Size(100, 24);
            this.kisitipicomboBox.TabIndex = 15;
            // 
            // kisiidtextBox
            // 
            this.kisiidtextBox.Location = new System.Drawing.Point(627, 29);
            this.kisiidtextBox.Name = "kisiidtextBox";
            this.kisiidtextBox.Size = new System.Drawing.Size(100, 22);
            this.kisiidtextBox.TabIndex = 17;
            // 
            // kisiidlabel
            // 
            this.kisiidlabel.AutoSize = true;
            this.kisiidlabel.Location = new System.Drawing.Point(527, 35);
            this.kisiidlabel.Name = "kisiidlabel";
            this.kisiidlabel.Size = new System.Drawing.Size(41, 16);
            this.kisiidlabel.TabIndex = 16;
            this.kisiidlabel.Text = "kişi id";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(797, 430);
            this.Controls.Add(this.kisiidtextBox);
            this.Controls.Add(this.kisiidlabel);
            this.Controls.Add(this.kisitipicomboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.telefontextBox);
            this.Controls.Add(this.telefonlabel);
            this.Controls.Add(this.arabutton);
            this.Controls.Add(this.güncellebutton);
            this.Controls.Add(this.silbutton);
            this.Controls.Add(this.eklebutton);
            this.Controls.Add(this.soyisimtextBox);
            this.Controls.Add(this.soyisimlabel);
            this.Controls.Add(this.listelebutton);
            this.Controls.Add(this.isimtextBox);
            this.Controls.Add(this.isimlabel);
            this.Controls.Add(this.dataGridView2);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label isimlabel;
        private System.Windows.Forms.TextBox isimtextBox;
        private System.Windows.Forms.Button listelebutton;
        private System.Windows.Forms.TextBox soyisimtextBox;
        private System.Windows.Forms.Label soyisimlabel;
        private System.Windows.Forms.Button eklebutton;
        private System.Windows.Forms.Button silbutton;
        private System.Windows.Forms.Button güncellebutton;
        private System.Windows.Forms.Button arabutton;
        private System.Windows.Forms.TextBox telefontextBox;
        private System.Windows.Forms.Label telefonlabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox kisitipicomboBox;
        private System.Windows.Forms.TextBox kisiidtextBox;
        private System.Windows.Forms.Label kisiidlabel;
    }
}

