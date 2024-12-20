namespace otelvtys
{
    partial class Formgeribildirim
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rezervasyonidcomboBox = new System.Windows.Forms.ComboBox();
            this.kisiidcomboBox = new System.Windows.Forms.ComboBox();
            this.rezervasyonlabel = new System.Windows.Forms.Label();
            this.kisiidlabel = new System.Windows.Forms.Label();
            this.geribildirimidtextBox = new System.Windows.Forms.TextBox();
            this.geribildirimidlabel = new System.Windows.Forms.Label();
            this.arabutton = new System.Windows.Forms.Button();
            this.güncellebutton = new System.Windows.Forms.Button();
            this.silbutton = new System.Windows.Forms.Button();
            this.eklebutton = new System.Windows.Forms.Button();
            this.puantextBox = new System.Windows.Forms.TextBox();
            this.puanlabel = new System.Windows.Forms.Label();
            this.listelebutton = new System.Windows.Forms.Button();
            this.yorumtextBox = new System.Windows.Forms.TextBox();
            this.yorumlabel = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // rezervasyonidcomboBox
            // 
            this.rezervasyonidcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rezervasyonidcomboBox.FormattingEnabled = true;
            this.rezervasyonidcomboBox.Items.AddRange(new object[] {
            "müşteri",
            "çalışan"});
            this.rezervasyonidcomboBox.Location = new System.Drawing.Point(624, 100);
            this.rezervasyonidcomboBox.Name = "rezervasyonidcomboBox";
            this.rezervasyonidcomboBox.Size = new System.Drawing.Size(100, 24);
            this.rezervasyonidcomboBox.TabIndex = 114;
            // 
            // kisiidcomboBox
            // 
            this.kisiidcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kisiidcomboBox.FormattingEnabled = true;
            this.kisiidcomboBox.Items.AddRange(new object[] {
            "müşteri",
            "çalışan"});
            this.kisiidcomboBox.Location = new System.Drawing.Point(624, 70);
            this.kisiidcomboBox.Name = "kisiidcomboBox";
            this.kisiidcomboBox.Size = new System.Drawing.Size(100, 24);
            this.kisiidcomboBox.TabIndex = 113;
            // 
            // rezervasyonlabel
            // 
            this.rezervasyonlabel.AutoSize = true;
            this.rezervasyonlabel.Location = new System.Drawing.Point(524, 100);
            this.rezervasyonlabel.Name = "rezervasyonlabel";
            this.rezervasyonlabel.Size = new System.Drawing.Size(95, 16);
            this.rezervasyonlabel.TabIndex = 112;
            this.rezervasyonlabel.Text = "rezervasyon id";
            // 
            // kisiidlabel
            // 
            this.kisiidlabel.AutoSize = true;
            this.kisiidlabel.Location = new System.Drawing.Point(524, 75);
            this.kisiidlabel.Name = "kisiidlabel";
            this.kisiidlabel.Size = new System.Drawing.Size(41, 16);
            this.kisiidlabel.TabIndex = 111;
            this.kisiidlabel.Text = "kişi id";
            // 
            // geribildirimidtextBox
            // 
            this.geribildirimidtextBox.Location = new System.Drawing.Point(624, 42);
            this.geribildirimidtextBox.Name = "geribildirimidtextBox";
            this.geribildirimidtextBox.Size = new System.Drawing.Size(100, 22);
            this.geribildirimidtextBox.TabIndex = 110;
            // 
            // geribildirimidlabel
            // 
            this.geribildirimidlabel.AutoSize = true;
            this.geribildirimidlabel.Location = new System.Drawing.Point(524, 42);
            this.geribildirimidlabel.Name = "geribildirimidlabel";
            this.geribildirimidlabel.Size = new System.Drawing.Size(90, 16);
            this.geribildirimidlabel.TabIndex = 109;
            this.geribildirimidlabel.Text = "geri bildirim id";
            // 
            // arabutton
            // 
            this.arabutton.Location = new System.Drawing.Point(654, 255);
            this.arabutton.Name = "arabutton";
            this.arabutton.Size = new System.Drawing.Size(75, 23);
            this.arabutton.TabIndex = 98;
            this.arabutton.Text = "ara";
            this.arabutton.UseVisualStyleBackColor = true;
            this.arabutton.Click += new System.EventHandler(this.arabutton_Click);
            // 
            // güncellebutton
            // 
            this.güncellebutton.Location = new System.Drawing.Point(595, 294);
            this.güncellebutton.Name = "güncellebutton";
            this.güncellebutton.Size = new System.Drawing.Size(75, 23);
            this.güncellebutton.TabIndex = 97;
            this.güncellebutton.Text = "güncelle";
            this.güncellebutton.UseVisualStyleBackColor = true;
            this.güncellebutton.Click += new System.EventHandler(this.güncellebutton_Click);
            // 
            // silbutton
            // 
            this.silbutton.Location = new System.Drawing.Point(529, 255);
            this.silbutton.Name = "silbutton";
            this.silbutton.Size = new System.Drawing.Size(75, 23);
            this.silbutton.TabIndex = 96;
            this.silbutton.Text = "sil";
            this.silbutton.UseVisualStyleBackColor = true;
            this.silbutton.Click += new System.EventHandler(this.silbutton_Click);
            // 
            // eklebutton
            // 
            this.eklebutton.Location = new System.Drawing.Point(654, 219);
            this.eklebutton.Name = "eklebutton";
            this.eklebutton.Size = new System.Drawing.Size(75, 23);
            this.eklebutton.TabIndex = 95;
            this.eklebutton.Text = "ekle";
            this.eklebutton.UseVisualStyleBackColor = true;
            this.eklebutton.Click += new System.EventHandler(this.eklebutton_Click);
            // 
            // puantextBox
            // 
            this.puantextBox.Location = new System.Drawing.Point(624, 158);
            this.puantextBox.Name = "puantextBox";
            this.puantextBox.Size = new System.Drawing.Size(100, 22);
            this.puantextBox.TabIndex = 94;
            // 
            // puanlabel
            // 
            this.puanlabel.AutoSize = true;
            this.puanlabel.Location = new System.Drawing.Point(524, 164);
            this.puanlabel.Name = "puanlabel";
            this.puanlabel.Size = new System.Drawing.Size(37, 16);
            this.puanlabel.TabIndex = 93;
            this.puanlabel.Text = "puan";
            // 
            // listelebutton
            // 
            this.listelebutton.Location = new System.Drawing.Point(529, 219);
            this.listelebutton.Name = "listelebutton";
            this.listelebutton.Size = new System.Drawing.Size(75, 23);
            this.listelebutton.TabIndex = 92;
            this.listelebutton.Text = "listele";
            this.listelebutton.UseVisualStyleBackColor = true;
            this.listelebutton.Click += new System.EventHandler(this.listelebutton_Click);
            // 
            // yorumtextBox
            // 
            this.yorumtextBox.Location = new System.Drawing.Point(624, 130);
            this.yorumtextBox.Name = "yorumtextBox";
            this.yorumtextBox.Size = new System.Drawing.Size(100, 22);
            this.yorumtextBox.TabIndex = 91;
            // 
            // yorumlabel
            // 
            this.yorumlabel.AutoSize = true;
            this.yorumlabel.Location = new System.Drawing.Point(524, 136);
            this.yorumlabel.Name = "yorumlabel";
            this.yorumlabel.Size = new System.Drawing.Size(44, 16);
            this.yorumlabel.TabIndex = 90;
            this.yorumlabel.Text = "yorum";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 12);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(477, 314);
            this.dataGridView2.TabIndex = 89;
            // 
            // Formgeribildirim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rezervasyonidcomboBox);
            this.Controls.Add(this.kisiidcomboBox);
            this.Controls.Add(this.rezervasyonlabel);
            this.Controls.Add(this.kisiidlabel);
            this.Controls.Add(this.geribildirimidtextBox);
            this.Controls.Add(this.geribildirimidlabel);
            this.Controls.Add(this.arabutton);
            this.Controls.Add(this.güncellebutton);
            this.Controls.Add(this.silbutton);
            this.Controls.Add(this.eklebutton);
            this.Controls.Add(this.puantextBox);
            this.Controls.Add(this.puanlabel);
            this.Controls.Add(this.listelebutton);
            this.Controls.Add(this.yorumtextBox);
            this.Controls.Add(this.yorumlabel);
            this.Controls.Add(this.dataGridView2);
            this.Name = "Formgeribildirim";
            this.Text = "Formgeribildirim";
            this.Load += new System.EventHandler(this.Formgeribildirim_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox rezervasyonidcomboBox;
        private System.Windows.Forms.ComboBox kisiidcomboBox;
        private System.Windows.Forms.Label rezervasyonlabel;
        private System.Windows.Forms.Label kisiidlabel;
        private System.Windows.Forms.TextBox geribildirimidtextBox;
        private System.Windows.Forms.Label geribildirimidlabel;
        private System.Windows.Forms.Button arabutton;
        private System.Windows.Forms.Button güncellebutton;
        private System.Windows.Forms.Button silbutton;
        private System.Windows.Forms.Button eklebutton;
        private System.Windows.Forms.TextBox puantextBox;
        private System.Windows.Forms.Label puanlabel;
        private System.Windows.Forms.Button listelebutton;
        private System.Windows.Forms.TextBox yorumtextBox;
        private System.Windows.Forms.Label yorumlabel;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}