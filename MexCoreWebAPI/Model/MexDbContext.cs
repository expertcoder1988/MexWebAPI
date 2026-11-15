using MexCoreWebAPI.Controllers;
using MexCoreWebAPI.Model.CIS;
using Microsoft.EntityFrameworkCore;


namespace MexCoreWebAPI.Model
{
    public class MexDbContext : DbContext
    {
        public MexDbContext(DbContextOptions<MexDbContext> options) : base(options) { }
        public DbSet<Prospect> Prospects    { get; set; }   
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<AirPriceRsp> AirPriceRsps { get; set; }
        public DbSet<PassengerDetail> PassengerDetails { get; set; }
        public  DbSet<ServiceClass> ServiceClasses { get; set; }
        public DbSet<FlightSegment> FlightSegments { get; set; }
        public  DbSet<BookingInfo> BookingInfoes { get; set; }
        public DbSet<AirPriceSol> AirPriceSols { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LowFareRequest> LowFareRequests { get; set; }        
        public DbSet<UmrahHotel> UmrahHotels { get; set; }
        public DbSet<AirPricingInfoKey> AirPricingInfoKeys { get; set; }
        
        public DbSet<HitItResult> HitItResults { get; set; }
        public DbSet<FareBasisCode> FareBasisCodes { get; set; }
        //     public virtual DbSet<AirItineraryPricingInfo> AirItineraryPricingInfos { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<TravelInsurance> TravelInsurances { get; set; }
        public DbSet<InsuranceBenefit> InsuranceBenefits { get; set; }
        public DbSet<InsurancePax> InsurancePaxs { get; set; }
        public DbSet<UicBooking> UicBookings { get; set; }
        public DbSet<InsuranceBooking> InsuranceBookings { get; set; }
        public DbSet<InsuranceEnquiry> InsuranceEnquiries { get; set; }

        public DbSet<UmrahInquiry> UmrahInquiries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<FilterDescriptor>().HasNoConstructor();

            modelBuilder.Entity<City>(
                b =>
                {
                    b.HasKey("cityID");
                    b.Property(e => e.cityID);
                    b.Property(e => e.destinationCode);
                    b.Property(e => e.cityName);
                    b.Property(e => e.cityProfilePath);
                    b.Property(e => e.cityDailingCode);
                    b.Property(e => e.countryID);
                    b.Property(e => e.IATA);
                    b.Property(e => e.State);
                    b.Property(e => e.Airport);
                    b.Property(e => e.Latitude);
                    b.Property(e => e.Lognitude);
                    b.Property(e => e.Elevation);
                    b.Property(e => e.countryCode);
                    b.Property(e => e.isActive);
                    b.Property(e => e.timeZone);
                    b.Property(e => e.cityCode);
                    b.Property(e => e.numAirports);

                });

        
        }
    }

}
