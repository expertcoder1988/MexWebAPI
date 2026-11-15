namespace MexCoreWebAPI.Model
{
    public class UmrahInquiry
    {
        public int Id { get; set; }
        public string? DepartureAirport { get; set; }
        public string? DepartureDate { get; set; }
        public int? Adults { get; set; }
        public int? Child { get; set; }
        public int? Infant { get; set; }
        public string? LeadPassengerName { get; set; }
        public string? Distance { get; set; }
        public string? ContactNo { get; set; }
        public string? HotelType { get; set; }
        public int? NightsInMakkah { get; set; }
        public int? NightsInMadinah { get; set; }
    }

}
