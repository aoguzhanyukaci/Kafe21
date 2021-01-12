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
        BindingList<Urun> blUrunler;

        public UrunlerForm(KafeVeri kafeVeri)
        {
            db = kafeVeri;
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false; // otomatik sütun oluşturma
            blUrunler = new BindingList<Urun>(db.Urunler);
            dgvUrunler.DataSource = blUrunler;
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
                blUrunler.Add(new Urun()
                {
                    UrunAd = urunAd,
                    BirimFiyat = nudBirimFiyat.Value
                });
            }
            else // Düzenleme modu
            {
                duzenlenen.UrunAd = urunAd;
                duzenlenen.BirimFiyat = nudBirimFiyat.Value;
                blUrunler.ResetBindings();
            }

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
    }
}
