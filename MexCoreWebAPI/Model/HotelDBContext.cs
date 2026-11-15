using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MexCoreWebAPI.Model
{
    public class HotelDBContext : DbContext
    {
        public HotelDBContext(DbContextOptions<HotelDBContext> options) : base(options) { }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomCategory> RoomCategories { get; set; }
        public virtual DbSet<RoomNumber> RoomNumbers { get; set; }
    }

    [Table("Room")]
    public partial class Room
    {
        public int ID { get; set; }
        public Nullable<int> RoomCode { get; set; }
        public Nullable<int> RoomCategory { get; set; }
        public string RoomDescription { get; set; }
        public string VideoLink { get; set; }
        public Nullable<int> propertyID { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Features { get; set; }
        public string BedType { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public Nullable<System.DateTime> dateCreated { get; set; }
        public Nullable<System.DateTime> dateModified { get; set; }
        public Nullable<int> NoOfRooms { get; set; }
        public Nullable<int> Capacity { get; set; }
        public Nullable<int> Adult { get; set; }
        public Nullable<int> NoOfChild { get; set; }
        public Nullable<int> ChildAge { get; set; }
        public Nullable<bool> blocked { get; set; }
        public string ShortCode { get; set; }
        public Nullable<int> MinimumPrice { get; set; }
        public Nullable<int> MinimumStay { get; set; }
        public Nullable<int> MaximumStay { get; set; }
        public Nullable<int> Size { get; set; }
        public Nullable<System.DateTime> EntryTime { get; set; }
        public Nullable<int> UserID { get; set; }
    }

    [Table("Booking")]
    public partial class Booking
    {
        public int BookingID { get; set; }
        public string ConfirmationNo { get; set; }
        public Nullable<int> HotelCode { get; set; }
        public Nullable<int> RatePlanId { get; set; }
        public string ThemeColor { get; set; }
        public Nullable<System.DateTime> ReservationDate { get; set; }
        public string BookingStatus { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string PaxTitle { get; set; }
        public string PaxFName { get; set; }
        public string PaxLName { get; set; }
        public Nullable<int> NoOfRooms { get; set; }
        public Nullable<int> RoomCode { get; set; }
        public Nullable<int> RoomCatCode { get; set; }
        public string RoomNum { get; set; }
        public string PaxEmail { get; set; }
        public string PaxPhoneNumber { get; set; }
        public string PaxDescription { get; set; }
        public Nullable<int> Adults { get; set; }
        public Nullable<int> Children { get; set; }
        public Nullable<System.DateTime> CheckIn { get; set; }
        public Nullable<System.DateTime> CheckOut { get; set; }
        public Nullable<int> NoofDay { get; set; }
        public Nullable<int> RoomRate { get; set; }
        public Nullable<double> RoomTax { get; set; }
        public Nullable<System.DateTime> RequestTime { get; set; }
        public string ServiceType { get; set; }
        public string ProcessID { get; set; }
        public string DestinationID { get; set; }
        public Nullable<bool> BackOfficeStatus { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<bool> B_BookingAmmend { get; set; }
        public Nullable<bool> BookingByBO { get; set; }
        public Nullable<bool> SOConfirmation { get; set; }
        public Nullable<bool> DirectManaualBooking { get; set; }
        public Nullable<bool> NotifyBit { get; set; }
        public Nullable<bool> bitIsLocal { get; set; }
        public Nullable<int> TotalRoomRate { get; set; }
        public Nullable<int> BookingSequenceNo { get; set; }
        public Nullable<double> AccomodationRent { get; set; }
        public Nullable<double> TourismTax { get; set; }
        public Nullable<double> AirportPickandDrop { get; set; }
        public Nullable<double> WelcomePack { get; set; }
        public Nullable<double> LinenTowelChange { get; set; }
        public Nullable<double> ExtraCleaning { get; set; }
        public Nullable<double> BreakageFee { get; set; }
        public Nullable<double> BreakageWaiverFee { get; set; }
        public Nullable<double> AgentCommission { get; set; }
        public Nullable<double> MiscExtrasCharges { get; set; }
        public Nullable<bool> IsByPropertyOwner { get; set; }
        public string PropertySource { get; set; }
        public Nullable<int> NoofGuest { get; set; }
        public Nullable<double> VAT { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public string RoomCategory { get; set; }
        public Nullable<bool> CheckStatusIN { get; set; }
        public Nullable<bool> CheckStatusOut { get; set; }
    }

    [Table("RoomCategory")]
    public partial class RoomCategory
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public Nullable<bool> Status { get; set; }
    }

    [Table("RoomNumber")]
    public partial class RoomNumber
    {
        public int Id { get; set; }
        public Nullable<int> RoomCode { get; set; }
        [Column("RoomNumber")]
        public string RoomNumber1 { get; set; }
        public Nullable<int> Floor { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> Booked { get; set; }
    }
}

