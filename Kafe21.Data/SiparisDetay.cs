﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafe21.Data
{
    public class SiparisDetay
    {
        public string UrunAd { get; set; }
        public decimal BirimFiyat { get; set; }
        public int Adet { get; set; }
        public string TutarTL { get  { return Tutar().ToString("c2"); } }
        public decimal Tutar()
        {
            return Adet * BirimFiyat;
        }

    }
}
