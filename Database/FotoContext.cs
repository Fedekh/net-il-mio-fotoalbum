using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Database
{
    public class FotoContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Foto> Foto { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Message> Message { get; set; }


        //private string sqlString = "Server=GAMMA;Database=Foto;TrustServerCertificate=True";
        private string sqlString = "Server=DESKTOP-1DGOME6;Database=Foto;Trusted_Connection=True;TrustServerCertificate=True";


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(sqlString);

    }
}
