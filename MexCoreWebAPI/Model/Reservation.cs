using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace MexCoreWebAPI.Model
{
    public class Reservation
    {
        public int Id { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Infants { get; set; }
        public string? PNR { get; set; }
        public decimal? AdultBasicFare { get; set; }
        public decimal? ChildBasicFare { get; set; }
        public decimal? InfantBasicFare { get; set; }
        public decimal? AdultTax { get; set; }
        public decimal? ChildTax { get; set; }
        public decimal? InfantTax { get; set; }
        public decimal? TotalFare { get; set; }
        public decimal? TotalTaxes { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? CustomerComission { get; set; }
        public decimal? CustomerOComission { get; set; }
        public decimal? AirlineComission { get; set; }
        public decimal? Incentive { get; set; }
        public string? IncentiveScale { get; set; }
        public DateTime? BookingDate { get; set; }
        public string? BookingStatus { get; set; }
        public int? BookingTypeID { get; set; }
        public string? LeadFirstName { get; set; }
        public string? LeadLastName { get; set; }
        public int? UserID { get; set; }
        public string? Email { get; set; }
        public string? PrimaryPhone { get; set; }
        public string? SecondaryPhone { get; set; }
        public int? FlightBookingID { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
    public string? Origin { get; set; }
        public string? Destination { get; set; }
        public string? ExpiryDateTime {get; set; }
        public decimal? OriginalCost { get; set; }
        public string? Vendor {get; set; }
        public string? CancelPenalty { get; set; }
        public string? ChangePenalty { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool? PaymentStatus { get; set; }
        public string? Penalty {get; set; }
    }
}
