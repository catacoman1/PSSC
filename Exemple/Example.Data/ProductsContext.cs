using Example.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Example.Data
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {
        }

        public DbSet<ProductDto> Products { get; set; }

        public DbSet<OrderLineDto> Orders { get; set; }

        public DbSet<OrderHeaderDto> Headers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDto>().ToTable("Product").HasKey(s => s.ProductId);
            modelBuilder.Entity<OrderHeaderDto>().ToTable("OrderHeader").HasKey(s => s.OrderId);
            modelBuilder.Entity<OrderLineDto>().ToTable("OrderLine").HasKey(s => s.OrderLineId);
        }

    }
}
