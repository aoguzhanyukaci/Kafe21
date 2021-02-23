using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafe21.Data
{
    public class KafeVeri:DbContext
    {
        public KafeVeri(): base ("name=KafeVeri")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<SiparisDetay>()
                .HasRequired(x => x.Urun)
                .WithMany(x => x.SiparisDetaylar)
                .HasForeignKey(x => x.UrunId)
                .WillCascadeOnDelete(false); 
        }

        public int MasaAdet { get; set; } = 20;

        public DbSet<Urun> Urunler { get; set; }

        public DbSet<Siparis> Siparisler { get; set; }

        public DbSet<SiparisDetay> SiparisDetaylar { get; set; }
    }
}
