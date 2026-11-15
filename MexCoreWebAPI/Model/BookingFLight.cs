namespace MexCoreWebAPI.Model
{
        public class BookingFLight
    {
        public string? BookingCode { get; set; }
        public string? BookingCount { get; set; }
        public string? CabinClass { get; set; }
        public string? FareInfoRef { get; set; }
        public string? SegmentRef { get; set; }
        public string? Option_Id { get; set; }
        public string? PaxType { get; set; }
        public string? AdultTotalPrice { get; set; }
        public string? AdultBasePrice { get; set; }
        public string? AdultTaxes { get; set; }
        public string? ChildTotalPrice { get; set; }
        public string? ChildBasePrice { get; set; }
        public string? ChildTaxes { get; set; }
        public string? InfantTotalPrice { get; set; }
        public string? InfantBasePrice { get; set; }
        public string? InfantTaxes { get; set; }
        public string? FareBasis { get; set; }
        public string? SegmentIndex { get; set; }
        public string? MaxWeightValue { get; set; }
        public string? MaxWeightUnit { get; set; }
        public string? SortBy { get; set; }
        public string? Stops { get; set; }
        public string? Direct { get; set; }
        public string? Flight { get; set; }
        public string? SegmentKey { get; set; }
        public string? Group { get; set; }
        public string? Carrier { get; set; }
        public string? FlightNumber { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public string? FlightTime { get; set; }
        public string? Distance { get; set; }
        public string? ETicketability { get; set; }
        public string? Equipment { get; set; }
        public string? ChangeOfPlane { get; set; }
        public string? ParticipantLevel { get; set; }
        public string? LinkAvailability { get; set; }
        public string? PolledAvailabilityOption { get; set; }
        public string? OptionalServicesIndicator { get; set; }
        public string? AvailabilitySource { get; set; }
        public string? AvailabilityDisplayType { get; set; }
        public string? ProviderCode { get; set; }
        public DateTime? ArrTime { get; set; }
        public string? FinalDestination { get; set; }
        public string? CancelPenalty { get; set; }
        public string? ChangePenalty { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }

        public int? Infants { get; set;}
    }

}

