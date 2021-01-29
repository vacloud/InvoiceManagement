using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SourceCode.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SourceCode.Models
{
    public class AppDBContext :IdentityDbContext
    {
        // We used this class to create our DataBase & Tables
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<General> GeneralSettings { get; set; }
        public DbSet<InvoiceHistory> InvoiceHistory { get; set; }
        public DbSet<InvoiceHistoryProducts> InvoiceHistoryProducts { get; set; }
        public DbSet<EmailSettings> EmailSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Initial Data Seeding in to DataBase Using Custom Extenstion Method Seed
            modelBuilder.Seed();


        }
    }
}
