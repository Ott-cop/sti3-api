using dotenv.net;
using Microsoft.EntityFrameworkCore;
using sti3_api.Application.Services;
using sti3_api.Domain.Entities;
using sti3_api.Infrastructure.Persistence.Configurations;

namespace sti3_api.Infrastructure
{
    public class AppDbContext: DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductsConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string url = $"Server={GlobalConfig.Dotenv["SERVER"]};Database={GlobalConfig.Dotenv["DATABASE"]};User Id={GlobalConfig.Dotenv["USER"]};Password={GlobalConfig.Dotenv["USER_PASSWORD"]};TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(url);
            base.OnConfiguring(optionsBuilder);
        }
    }
}