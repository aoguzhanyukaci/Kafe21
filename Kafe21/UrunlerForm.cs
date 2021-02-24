using Kafe21.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kafe21
{
    public partial class UrunlerForm : Form
    {
        KafeVeri db;

        public UrunlerForm(KafeVeri kafeVeri)
        {
            db = kafeVeri;
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false; // otomatik sütun oluşturma
            dgvUrunler.DataSource = db.Urunler.ToList();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string urunAd = txtUrunAd.Text.Trim();

            if (urunAd == "")
            {
                MessageBox.Show("Ürün adı giriniz.");
                return;
            }

            if (duzenlenen==null) // Ekleme modu
            {
                db.Urunler.Add(new Urun()
                {
                    UrunAd = urunAd,
                    BirimFiyat = nudBirimFiyat.Value
                });
            }
            else // Düzenleme modu
            {
                duzenlenen.UrunAd = urunAd;
                duzenlenen.BirimFiyat = nudBirimFiyat.Value;
            }


            db.SaveChanges();
            dgvUrunler.DataSource = db.Urunler.ToList();
            FormuResetle();
        }

        private void FormuResetle()
        {
            txtUrunAd.Clear();
            nudBirimFiyat.Value = 0;
            btnIptal.Visible = false;
            btnEkle.Text = "EKLE";
            duzenlenen = null;
            txtUrunAd.Focus();
        }

        Urun duzenlenen;
        private void dgvUrunler_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var satir = dgvUrunler.Rows[e.RowIndex];
            Urun urun = (Urun)satir.DataBoundItem;
            txtUrunAd.Text = urun.UrunAd;
            nudBirimFiyat.Value = urun.BirimFiyat;
            btnEkle.Text = "KAYDET";
            btnIptal.Show();
            duzenlenen = urun;
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            FormuResetle();
        }

        private void UrunlerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgvUrunler.SelectedRows.Count>0)
            {
                var seciliSatir = dgvUrunler.SelectedRows[0];
                var urun = (Urun)seciliSatir.DataBoundItem;

                if (urun.SiparisDetaylar.Count > 0)
                {
                    MessageBox.Show("Bu ürün daha önce sipariş verildiği için silinemez.");
                    return;
                }

                db.Urunler.Remove(urun);
                db.SaveChanges();
                dgvUrunler.DataSource = db.Urunler.ToList();
            }
        }
    }
}
