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

namespace HaliYikamaOtomasyonu
{
    public partial class Form1 : Form
    {
        // Bağlantı cümlemizi en üste, genel alana yazıyoruz ki her yerden ulaşılabilsin.
        string baglantiCumlesi = "Data Source=localhost;Initial Catalog=HaliYikamaDB;Integrated Security=True;";
        int secilenMusteriID = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Form açıldığında listeleme metodunu çağırıp tabloyu dolduruyoruz
            SiparisleriListele();

            // Tasarım ayarları - Yazıların okunması ve tabloyu doldurması için
            dataGridView2.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView2.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Listeleme işlemini ayrı bir metoda aldık ki butona bastığımızda da çağırabilelim
        private void SiparisleriListele()
        {
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                // İki tabloyu birleştirip tek ekranda gösteren sorgumuz
                string sorgu = @"
                    SELECT 
                        m.MusteriID, 
                        m.AdSoyad, 
                        m.Telefon, 
                        m.Adres, 
                        s.ToplamMetrekare AS [Metrekare], 
                        s.ToplamTutar AS [Tutar TL], 
                        s.SiparisDurumu AS [Durum],
                        s.SiparisTarihi AS [Tarih]
                    FROM Musteriler m
                    INNER JOIN Siparisler s ON m.MusteriID = s.MusteriID";

                SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                DataTable tablo = new DataTable();
                da.Fill(tablo);
                dataGridView2.DataSource = tablo;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
            {
                baglanti.Open();

                // 1. ADIM: Müşteriyi kaydet ve oluşan yeni MusteriID'yi geri döndür
                string musteriSorgu = @"
                    INSERT INTO Musteriler (AdSoyad, Telefon, Adres, KayitTarihi) 
                    VALUES (@AdSoyad, @Telefon, @Adres, @KayitTarihi);
                    SELECT SCOPE_IDENTITY();"; // Eklenen son ID'yi getirir

                SqlCommand cmdMusteri = new SqlCommand(musteriSorgu, baglanti);
                cmdMusteri.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
                cmdMusteri.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
                cmdMusteri.Parameters.AddWithValue("@Adres", rtxtAdres.Text); // Eklediğimiz RichTextBox
                cmdMusteri.Parameters.AddWithValue("@KayitTarihi", DateTime.Now);

                // Müşteriyi ekleyip ID'sini alıyoruz
                int yeniMusteriID = Convert.ToInt32(cmdMusteri.ExecuteScalar());

                // 2. ADIM: Yakaladığımız MusteriID ile Siparişi kaydet
                decimal metrekare = Convert.ToDecimal(txtMetrekare.Text);
                decimal birimFiyat = 50.00m;
                decimal toplamTutar = metrekare * birimFiyat;

                string siparisSorgu = @"
                    INSERT INTO Siparisler (MusteriID, HaliAdeti, ToplamMetrekare, ToplamTutar, SiparisDurumu, SiparisTarihi) 
                    VALUES (@MusteriID, @HaliAdeti, @ToplamMetrekare, @ToplamTutar, @SiparisDurumu, @SiparisTarihi)";

                SqlCommand cmdSiparis = new SqlCommand(siparisSorgu, baglanti);
                cmdSiparis.Parameters.AddWithValue("@MusteriID", yeniMusteriID);
                cmdSiparis.Parameters.AddWithValue("@HaliAdeti", 1);
                cmdSiparis.Parameters.AddWithValue("@ToplamMetrekare", metrekare);
                cmdSiparis.Parameters.AddWithValue("@ToplamTutar", toplamTutar);
                cmdSiparis.Parameters.AddWithValue("@SiparisDurumu", "Alındı");
                cmdSiparis.Parameters.AddWithValue("@SiparisTarihi", DateTime.Now);

                cmdSiparis.ExecuteNonQuery();

                MessageBox.Show("Müşteri ve Sipariş başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. ADIM: Tabloyu anında yeni verilerle güncelle
                SiparisleriListele();
                txtAdSoyad.Text = "";
                txtTelefon.Text = "";
                txtMetrekare.Text = "";
                rtxtAdres.Text = "";
            }
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        } 
            private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            { 
            // Sütun başlıklarına tıklayınca hata vermemesi için
            if (e.RowIndex >= 0)
            {
                DataGridViewRow satir = dataGridView2.Rows[e.RowIndex];

                // Tablodaki verileri kutucuklara dolduruyoruz
                secilenMusteriID = Convert.ToInt32(satir.Cells["MusteriID"].Value);
                txtAdSoyad.Text = satir.Cells["AdSoyad"].Value.ToString();
                txtTelefon.Text = satir.Cells["Telefon"].Value.ToString();
                rtxtAdres.Text = satir.Cells["Adres"].Value.ToString();
                txtMetrekare.Text = satir.Cells["Metrekare"].Value.ToString();
            }
        }
    }
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (secilenMusteriID > 0)
            {
                using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
                {
                    baglanti.Open();

                    // 1. Müşteriler tablosunu güncelle
                    SqlCommand cmdMusteri = new SqlCommand("UPDATE Musteriler SET AdSoyad=@AdSoyad, Telefon=@Telefon, Adres=@Adres WHERE MusteriID=@id", baglanti);
                    cmdMusteri.Parameters.AddWithValue("@AdSoyad", txtAdSoyad.Text);
                    cmdMusteri.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
                    cmdMusteri.Parameters.AddWithValue("@Adres", rtxtAdres.Text);
                    cmdMusteri.Parameters.AddWithValue("@id", secilenMusteriID);
                    cmdMusteri.ExecuteNonQuery();

                    // 2. Siparişler tablosunu güncelle (Metrekare ve Tutarı baştan hesapla)
                    decimal metrekare = Convert.ToDecimal(txtMetrekare.Text);
                    decimal toplamTutar = metrekare * 50.00m; // 50 TL birim fiyat

                    SqlCommand cmdSiparis = new SqlCommand("UPDATE Siparisler SET ToplamMetrekare=@Metrekare, ToplamTutar=@Tutar WHERE MusteriID=@id", baglanti);
                    cmdSiparis.Parameters.AddWithValue("@Metrekare", metrekare);
                    cmdSiparis.Parameters.AddWithValue("@Tutar", toplamTutar);
                    cmdSiparis.Parameters.AddWithValue("@id", secilenMusteriID);
                    cmdSiparis.ExecuteNonQuery();

                    MessageBox.Show("Kayıt başarıyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SiparisleriListele(); // Tabloyu yenile
                }
            }
            else
            {
                MessageBox.Show("Lütfen tablodan güncellemek istediğiniz kaydı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            if (secilenMusteriID > 0)
            {
                DialogResult onay = MessageBox.Show("Bu kaydı tamamen silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (onay == DialogResult.Yes)
                {
                    using (SqlConnection baglanti = new SqlConnection(baglantiCumlesi))
                    {
                        baglanti.Open();

                        // Önce alt tabloyu (Siparişler) siliyoruz
                        SqlCommand cmdSiparisSil = new SqlCommand("DELETE FROM Siparisler WHERE MusteriID=@id", baglanti);
                        cmdSiparisSil.Parameters.AddWithValue("@id", secilenMusteriID);
                        cmdSiparisSil.ExecuteNonQuery();

                        // Sonra ana tabloyu (Müşteriler) siliyoruz
                        SqlCommand cmdMusteriSil = new SqlCommand("DELETE FROM Musteriler WHERE MusteriID=@id", baglanti);
                        cmdMusteriSil.Parameters.AddWithValue("@id", secilenMusteriID);
                        cmdMusteriSil.ExecuteNonQuery();

                        MessageBox.Show("Kayıt sistemden silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Seçimi sıfırla ve tabloyu yenile
                        secilenMusteriID = 0;
                        txtAdSoyad.Text = ""; txtTelefon.Text = ""; rtxtAdres.Text = ""; txtMetrekare.Text = "";
                        SiparisleriListele();
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen tablodan silmek istediğiniz kaydı seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}