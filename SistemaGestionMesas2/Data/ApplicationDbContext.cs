using Microsoft.EntityFrameworkCore;
using SistemaGestionMesas2.Models;

namespace SistemaGestionMesas2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Menu> CookableProduct { get; set; }
        public DbSet<Table> Table { get; set; }
        public DbSet<TableProduct> TableProduct { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;" +
                                        "Database=SistemaGestionMesasDb;" +
                                        "Trusted_Connection=True;" +
                                        "Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TableProduct>()
                .HasKey(tp => new { tp.TableId, tp.ProductId });

            modelBuilder.Entity<TableProduct>()
                .HasOne(tp => tp.Table)
                .WithMany(t => t.TableProducts)
                .HasForeignKey(tp => tp.TableId);

            modelBuilder.Entity<TableProduct>()
                .HasOne(tp => tp.Product)
                .WithMany(p => p.TableProducts)
                .HasForeignKey(tp => tp.ProductId);
        }
    }
}
