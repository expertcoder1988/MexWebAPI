using Microsoft.EntityFrameworkCore;
using MexCoreWebAPI.Controllers;
using VISA.Models;

namespace MexCoreWebAPI.Model
{
    public class VisaDbContext:DbContext
    {
        public VisaDbContext(DbContextOptions<VisaDbContext> visaoptions) : base(visaoptions) { }
        public DbSet<VisaCountry> VisaCountries { get; set; }
        public DbSet<Visa> Visas { get; set; }
        public DbSet<VisaDocuments> VisaDocuments { get; set; }
        public DbSet<Faq> Faqs { get; set; }

        public DbSet<VisaRequests> VisaRequests { get; set; }
        public DbSet<VisaRequestDocuments> VisaRequestDocuments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<FilterDescriptor>().HasNoConstructor();

        }
    }
}
