
namespace MexCoreWebAPI.Model
{


    public class Rootobject
    {
        public Body[] Body { get; set; }
        public Airpricersp[] AirPriceRsp { get; set; }
        public Responsemessage[] ResponseMessage { get; set; }
        public Airitinerary[] AirItinerary { get; set; }
        public Airsegment[] AirSegment { get; set; }
        public Codeshareinfo[] CodeshareInfo { get; set; }
        public Flightdetail[] FlightDetails { get; set; }
        public Airpriceresult[] AirPriceResult { get; set; }
        public Airpricingsolution[] AirPricingSolution { get; set; }
        public Airsegmentref[] AirSegmentRef { get; set; }
        public Airpricinginfo[] AirPricingInfo { get; set; }
        public Fareinfo[] FareInfo { get; set; }
        public Endorsement[] Endorsement { get; set; }
        public Farerulekey[] FareRuleKey { get; set; }
        public Brand[] Brand { get; set; }
        public Bookinginfo[] BookingInfo { get; set; }
        public Taxinfo[] TaxInfo { get; set; }
        public Passengertype[] PassengerType { get; set; }
        public Changepenalty[] ChangePenalty { get; set; }
        public Cancelpenalty[] CancelPenalty { get; set; }
        public Baggageallowance[] BaggageAllowances { get; set; }
        public Baggageallowanceinfo[] BaggageAllowanceInfo { get; set; }
        public Urlinfo[] URLInfo { get; set; }
        public Textinfo[] TextInfo { get; set; }
        public Bagdetail[] BagDetails { get; set; }
        public Baggagerestriction[] BaggageRestriction { get; set; }
        public Carryonallowanceinfo[] CarryOnAllowanceInfo { get; set; }
        public Carryondetail[] CarryOnDetails { get; set; }
        public Farenote[] FareNote { get; set; }
        public Hosttoken[] HostToken { get; set; }
        public Farerule[] FareRule { get; set; }
        public Farerulelong[] FareRuleLong { get; set; }
    }

    public class Body
    {
        public int Body_Id { get; set; }
    }

    public class Airpricersp
    {
        public int AirPriceRsp_Id { get; set; }
        public string TraceId { get; set; }
        public string TransactionId { get; set; }
        public string ResponseTime { get; set; }
        public int Body_Id { get; set; }
    }

    public class Responsemessage
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string ProviderCode { get; set; }
        public string ResponseMessage_Text { get; set; }
        public int AirPriceRsp_Id { get; set; }
    }

    public class Airitinerary
    {
        public int AirItinerary_Id { get; set; }
        public int AirPriceRsp_Id { get; set; }
    }

    public class Airsegment
    {
        public int AirSegment_Id { get; set; }
        public string Key { get; set; }
        public string Group { get; set; }
        public string Carrier { get; set; }
        public string FlightNumber { get; set; }
        public string ProviderCode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string FlightTime { get; set; }
        public string TravelTime { get; set; }
        public string Distance { get; set; }
        public string ClassOfService { get; set; }
        public string ChangeOfPlane { get; set; }
        public string OptionalServicesIndicator { get; set; }
        public string AvailabilitySource { get; set; }
        public string PolledAvailabilityOption { get; set; }
        public string AvailabilityDisplayType { get; set; }
        public int AirItinerary_Id { get; set; }
    }

    public class Codeshareinfo
    {
        public string OperatingCarrier { get; set; }
        public string CodeshareInfo_Text { get; set; }
        public int AirSegment_Id { get; set; }
    }

    public class Flightdetail
    {
        public string InFlightServices { get; set; }
        public string Key { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string FlightTime { get; set; }
        public string TravelTime { get; set; }
        public string Equipment { get; set; }
        public string DestinationTerminal { get; set; }
        public string AutomatedCheckin { get; set; }
        public int AirSegment_Id { get; set; }
    }

    public class Airpriceresult
    {
        public int AirPriceResult_Id { get; set; }
        public int AirPriceRsp_Id { get; set; }
    }

    public class Airpricingsolution
    {
        public int AirPricingSolution_Id { get; set; }
        public string Key { get; set; }
        public string TotalPrice { get; set; }
        public string BasePrice { get; set; }
        public string ApproximateTotalPrice { get; set; }
        public string ApproximateBasePrice { get; set; }
        public string EquivalentBasePrice { get; set; }
        public string Taxes { get; set; }
        public string Fees { get; set; }
        public string ApproximateTaxes { get; set; }
        public string QuoteDate { get; set; }
        public int AirPriceResult_Id { get; set; }
    }

    public class Airsegmentref
    {
        public string Key { get; set; }
        public int AirPricingSolution_Id { get; set; }
    }

    public class Airpricinginfo
    {
        public int AirPricingInfo_Id { get; set; }
        public string FareCalc { get; set; }
        public string Key { get; set; }
        public string TotalPrice { get; set; }
        public string BasePrice { get; set; }
        public string ApproximateTotalPrice { get; set; }
        public string ApproximateBasePrice { get; set; }
        public string EquivalentBasePrice { get; set; }
        public string ApproximateTaxes { get; set; }
        public string Taxes { get; set; }
        public DateTime LatestTicketingTime { get; set; }
        public string PricingMethod { get; set; }
        public string Refundable { get; set; }
        public string IncludesVAT { get; set; }
        public string ETicketability { get; set; }
        public string PlatingCarrier { get; set; }
        public string ProviderCode { get; set; }
        public int AirPricingSolution_Id { get; set; }
    }

    public class Fareinfo
    {
        public int FareInfo_Id { get; set; }
        public string Key { get; set; }
        public string FareBasis { get; set; }
        public string PassengerTypeCode { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string DepartureDate { get; set; }
        public string Amount { get; set; }
        public string NegotiatedFare { get; set; }
        public string NotValidBefore { get; set; }
        public string NotValidAfter { get; set; }
        public string TaxAmount { get; set; }
        public int AirPricingInfo_Id { get; set; }
    }

    public class Endorsement
    {
        public string Value { get; set; }
        public int FareInfo_Id { get; set; }
    }

    public class Farerulekey
    {
        public string FareInfoRef { get; set; }
        public string ProviderCode { get; set; }
        public string FareRuleKey_Text { get; set; }
        public int FareInfo_Id { get; set; }
    }

    public class Brand
    {
        public string Key { get; set; }
        public string BrandFound { get; set; }
        public string UpSellBrandFound { get; set; }
        public int FareInfo_Id { get; set; }
    }

    public class Bookinginfo
    {
        public string BookingCode { get; set; }
        public string CabinClass { get; set; }
        public string FareInfoRef { get; set; }
        public string SegmentRef { get; set; }
        public string HostTokenRef { get; set; }
        public int AirPricingInfo_Id { get; set; }
    }

    public class Taxinfo
    {
        public string Category { get; set; }
        public string Amount { get; set; }
        public string Key { get; set; }
        public int AirPricingInfo_Id { get; set; }
    }

    public class Passengertype
    {
        public string Code { get; set; }
        public int AirPricingInfo_Id { get; set; }
    }

    public class Changepenalty
    {
        public string Amount { get; set; }
        public int AirPricingInfo_Id { get; set; }
    }

    public class Cancelpenalty
    {
        public string Amount { get; set; }
        public int AirPricingInfo_Id { get; set; }
    }

    public class Baggageallowance
    {
        public int BaggageAllowances_Id { get; set; }
        public int AirPricingInfo_Id { get; set; }
    }

    public class Baggageallowanceinfo
    {
        public int BaggageAllowanceInfo_Id { get; set; }
        public string TravelerType { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Carrier { get; set; }
        public int BaggageAllowances_Id { get; set; }
    }

    public class Urlinfo
    {
        public string URL { get; set; }
        public int BaggageAllowanceInfo_Id { get; set; }
    }

    public class Textinfo
    {
        public string Text34 { get; set; }
        public string Text35 { get; set; }
        public string Text43 { get; set; }
        public string Text39 { get; set; }
        public int? BaggageRestriction_Id { get; set; }
        public int? BaggageAllowanceInfo_Id { get; set; }
        public string Text46 { get; set; }
        public string Text54 { get; set; }
        public string Text50 { get; set; }
        public int? CarryOnAllowanceInfo_Id { get; set; }
    }

    public class Bagdetail
    {
        public int BagDetails_Id { get; set; }
        public string ApplicableBags { get; set; }
        public int BaggageAllowanceInfo_Id { get; set; }
    }

    public class Baggagerestriction
    {
        public int BaggageRestriction_Id { get; set; }
        public int? BagDetails_Id { get; set; }
        public int? CarryOnDetails_Id { get; set; }
    }

    public class Carryonallowanceinfo
    {
        public int CarryOnAllowanceInfo_Id { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Carrier { get; set; }
        public int BaggageAllowances_Id { get; set; }
    }

    public class Carryondetail
    {
        public int CarryOnDetails_Id { get; set; }
        public string ApplicableCarryOnBags { get; set; }
        public int CarryOnAllowanceInfo_Id { get; set; }
    }

    public class Farenote
    {
        public string Key { get; set; }
        public string FareNote_Text { get; set; }
        public int AirPricingSolution_Id { get; set; }
    }

    public class Hosttoken
    {
        public string Key { get; set; }
        public string HostToken_Text { get; set; }
        public int AirPricingSolution_Id { get; set; }
    }

    public class Farerule
    {
        public int FareRule_Id { get; set; }
        public string FareInfoRef { get; set; }
        public string RuleNumber { get; set; }
        public string Source { get; set; }
        public string TariffNumber { get; set; }
        public int AirPriceResult_Id { get; set; }
    }

    public class Farerulelong
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string FareRuleLong_Text { get; set; }
        public int FareRule_Id { get; set; }
    }


}