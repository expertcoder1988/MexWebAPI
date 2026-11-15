using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MexCoreWebAPI.Model
{
    public class MexOldDBContext : DbContext
    {
        public MexOldDBContext(DbContextOptions<MexOldDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AirlineDefaultServiceFee> AirlineDefaultServiceFees { get; set; }
        public DbSet<AirlineSpecificServiceFee> AirlineSpecificServiceFees { get; set; }
        public DbSet<Incentive> Incentives { get; set; }
     
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           

        }
    }
}
