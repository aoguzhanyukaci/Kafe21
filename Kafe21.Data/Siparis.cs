using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafe21.Data
{
    [Table("Siparisler")]
    public class Siparis 
    {
        public int Id { get; set; }
        public int MasaNo { get; set; }
        public SiparisDurum Durum { get; set; }
        public decimal OdenenTutar { get; set; }
        public DateTime? AcilisZamani { get; set; } = DateTime.Now;
        public DateTime? KapanisZamani { get; set; }
        public string ToplamTutarTL => ToplamTutar().ToString("c2");

        public virtual ICollection<SiparisDetay> SiparisDetaylari { get; set; } = new HashSet<SiparisDetay>(); // boş liste olsun.

        public decimal ToplamTutar()
        {
            return SiparisDetaylari.Sum(x=>x.Tutar()) ;
        }
    }
}
