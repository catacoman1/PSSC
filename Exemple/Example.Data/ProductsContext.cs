using Example.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Data
{
    public class CartAppDbContext : DbContext
    {
        public CartAppDbContext()
        {
        }
        public CartAppDbContext(DbContextOptions<CartAppDbContext> options) : base(options)
        {
        }

        public DbSet<ProductDto> Products { get; set; }

        public DbSet<OrderHeaderDto> OrderHeaders { get; set; }

        public DbSet<OrderLineDto> OrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDto>().ToTable("Products").HasKey(s => s.ProductId);
            modelBuilder.Entity<OrderHeaderDto>().ToTable("OrderHeaders").HasKey(s => s.OrderId);
            modelBuilder.Entity<OrderLineDto>().ToTable("OrderLines").HasKey(s => s.OrderLineId);

            modelBuilder.Entity<OrderHeaderDto>()
                .HasMany(e => e.OrderLines)
                .WithOne(e => e.OrderHeader)
                .HasForeignKey(e => e.OrderId)
                .HasPrincipalKey(e => e.OrderId);

            modelBuilder.Entity<ProductDto>()
                .HasMany(e => e.OrderLines)
                .WithOne(e => e.Product)
                .HasForeignKey(e => e.ProductId)
                .HasPrincipalKey(e => e.ProductId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Magazin1;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

    }
}
