using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Etüt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Data Source =.\SQLEXPRESS;Initial Catalog = ETÜT; Integrated Security = True
        SqlConnection baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ETÜT;Integrated Security=True");

        void derslistesi()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Dersler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbxDers.ValueMember = "DersID";
            cmbxDers.DisplayMember = "DersAd";
            cmbxDers.DataSource = dt;
        }

        void etutlistesi()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("execute ETÜT", baglanti);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            derslistesi();
            etutlistesi();
        }

        private void cmbxDers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataAdapter da2 = new SqlDataAdapter("select * from OGRETMEN where BransID=" + cmbxDers.SelectedValue, baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            cmbxÖğretmen.ValueMember = "OgrtID";
            cmbxÖğretmen.DisplayMember = "Ad";
            cmbxÖğretmen.DataSource = dt2;

        }

        private void btnEtütOluştur_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Insert into ETUT (DersID,OgretmenID,Tarih,Saat) values (@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", cmbxDers.SelectedValue);
            komut.Parameters.AddWithValue("@p2", cmbxÖğretmen.SelectedValue);
            komut.Parameters.AddWithValue("@p3", masktxtTarih.Text);
            komut.Parameters.AddWithValue("@p4", masktxtSaat.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Etüt Oluşturuldu", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtEtütID.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
        }

        private void btnEtütVer_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update ETUT set OgrencıID=@P1,Durum=@p2 where ID=@p3", baglanti);
            komut.Parameters.AddWithValue("@p1", txtÖğrenci.Text);
            komut.Parameters.AddWithValue("@p2", "True");
            komut.Parameters.AddWithValue("@p3", txtEtütID.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Etüt Öğrenciye Verildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnFotografYükle_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox1.ImageLocation = openFileDialog1.FileName;
        }

        private void btnÖğrenciEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into OGRENCI (Ad,Soyad,Fotograf,Sınıf,Telefon,Maıl) values (@p1,@p2,@p3,@p4,@p5,@p6)", baglanti);
            komut.Parameters.AddWithValue("@p1", txtAd.Text);
            komut.Parameters.AddWithValue("@p2", txtSoyad.Text);
            komut.Parameters.AddWithValue("@p3", pictureBox1.ImageLocation);
            komut.Parameters.AddWithValue("@p4", txtSınıf.Text);
            komut.Parameters.AddWithValue("@p5", masktxtTelefon.Text);
            komut.Parameters.AddWithValue("@p6", txtMail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğrenci Sisteme Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnÖğretmenEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into OGRETMEN (Ad,Soyad,BransID) values (@p1,@p2,@p3)", baglanti);
            komut.Parameters.AddWithValue("@p1", txtÖğretmenAd.Text);
            komut.Parameters.AddWithValue("@p2", txtÖğretmenSoyad.Text);
            komut.Parameters.AddWithValue("@p3", txtders.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Öğretmen Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDersEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into Dersler (DersAd) values (@p1)", baglanti);
            komut.Parameters.AddWithValue("@p1", txtDersAdı.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ders Kaydı Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
