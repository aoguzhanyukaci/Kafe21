using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafe21.Data
{
    public class Urun
    {
        public string UrunAd { get; set; }
        public decimal BirimFiyat { get; set; }
        public override string ToString()
        {
            return string.Format("{0} ({1:n2})", UrunAd, BirimFiyat);

        }

    }
}
