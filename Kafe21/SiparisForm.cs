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
    public partial class SiparisForm : Form
    {
        public event EventHandler<MasaTasindiEventArgs> MasaTasindi;

        KafeVeri db;
        Siparis siparis;

        public SiparisForm(KafeVeri kafeVeri, Siparis siparis)
        {
            db = kafeVeri;
            this.siparis = siparis; // this local değişkendeki değeri al
            InitializeComponent();
            tsslAcilisZamani.Text = siparis.AcilisZamani.ToString();
            dgvSiparisDetaylar.AutoGenerateColumns = false; // kendi sütün oluşturmayacak
            MasaNolariGunvelle();
            OdemeTutariGuncelle();
            UrunlerYukle();
            SiparisDetaylariYukle();
        }

        private void SiparisDetaylariYukle()
        {
            dgvSiparisDetaylar.DataSource = siparis.SiparisDetaylari.ToList();
        }

        private void UrunlerYukle()
        {
            cboUrun.DataSource = db.Urunler.ToList();
        }

        private void OdemeTutariGuncelle()
        {
            lblOdemeTutari.Text = siparis.ToplamTutarTL;
        }

        private void MasaNolariGunvelle()
        {
            Text = $"Masa {siparis.MasaNo:00}- Sipariş";
            lblMasaNo.Text = siparis.MasaNo.ToString("00");

            cboMasaNo.DataSource = Enumerable
                .Range(1, db.MasaAdet)
                .Where(x => !db.Siparisler.Any(s => s.MasaNo == x && s.Durum==SiparisDurum.Aktif))
                .ToList();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Urun urun = (Urun)cboUrun.SelectedItem;
            int adet = (int)nudAdet.Value;

            siparis.SiparisDetaylari.Add(new SiparisDetay()
            {
                UrunAd = urun.UrunAd,
                BirimFiyat = urun.BirimFiyat,
                Adet = adet,
                UrunId=urun.Id
            });
            
            db.SaveChanges();
            dgvSiparisDetaylar.DataSource = siparis.SiparisDetaylari.ToList();
            OdemeTutariGuncelle();

        }

        private void dgvSiparisDetaylar_KeyDown(object sender, KeyEventArgs e)
        {
            // Delete'ye basıldıysa ve dgv üzerinde  en az bir seçili satır varsa
            if (e.KeyCode == Keys.Delete && dgvSiparisDetaylar.SelectedRows.Count > 0)
            {
                DialogResult dr = MessageBox.Show
                    (
                    "Seçili detay siparişten kaldırılacaktır. Onaylıyor musunuz?",
                    "Silme Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                    );
                if (dr == DialogResult.Yes)
                {
                    DataGridViewRow satir = dgvSiparisDetaylar.SelectedRows[0];
                    SiparisDetay sd = (SiparisDetay)satir.DataBoundItem;
                    db.SiparisDetaylar.Remove(sd);
                    db.SaveChanges();
                    dgvSiparisDetaylar.DataSource = siparis.SiparisDetaylari.ToList();
                    OdemeTutariGuncelle();
                }

            }
        }

        private void btnAnasayfa_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;// içinde bulunan formun close metodunu çağır.
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            SiparisKapat(SiparisDurum.Iptal);
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            SiparisKapat(SiparisDurum.Odendi);
        }

        private void SiparisKapat(SiparisDurum durum)
        {
            if (durum == SiparisDurum.Odendi)
                siparis.OdenenTutar = siparis.ToplamTutar();

            siparis.KapanisZamani = DateTime.Now;
            siparis.Durum = durum;
            db.SaveChanges();
            DialogResult = DialogResult.OK; // bu formda işim bitti
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
            if (cboMasaNo.SelectedIndex > -1)
            {
                int eskiMasaNo = siparis.MasaNo;
                int yeniMasaNo = (int)cboMasaNo.SelectedItem;
                siparis.MasaNo = yeniMasaNo;
                db.SaveChanges();
                MasaNolariGunvelle();

                if (MasaTasindi != null)
                {
                    MasaTasindi(this, new MasaTasindiEventArgs(eskiMasaNo, yeniMasaNo));

                }
            }
        }
    }
}
