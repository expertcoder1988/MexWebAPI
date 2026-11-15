
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace MalikExpressMVC.Models.XML
{
    [XmlRoot(ElementName = "From", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
    public class From
    {
        [XmlElement(ElementName = "PartyId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string PartyId { get; set; }
    }

    [XmlRoot(ElementName = "To", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
    public class To
    {
        [XmlElement(ElementName = "PartyId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string PartyId { get; set; }
    }

    [XmlRoot(ElementName = "MessageData", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
    public class MessageData
    {
        [XmlElement(ElementName = "MessageId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string MessageId { get; set; }
        [XmlElement(ElementName = "Timestamp", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string Timestamp { get; set; }
    }

    [XmlRoot(ElementName = "MessageHeader", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
    public class MessageHeader
    {
        [XmlElement(ElementName = "From", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public From From { get; set; }
        [XmlElement(ElementName = "To", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public To To { get; set; }
        [XmlElement(ElementName = "CPAId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string CPAId { get; set; }
        [XmlElement(ElementName = "ConversationId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string ConversationId { get; set; }
        [XmlElement(ElementName = "Service", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string Service { get; set; }
        [XmlElement(ElementName = "Action", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string Action { get; set; }
        [XmlElement(ElementName = "MessageData", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public MessageData MessageData { get; set; }
        [XmlElement(ElementName = "RefToMessageId", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string RefToMessageId { get; set; }
        [XmlAttribute(AttributeName = "eb", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Eb { get; set; }
        [XmlAttribute(AttributeName = "mustUnderstand", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public string MustUnderstand { get; set; }
        [XmlAttribute(AttributeName = "version", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public string Version { get; set; }
    }

    [XmlRoot(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Header
    {
        [XmlElement(ElementName = "MessageHeader", Namespace = "http://www.ebxml.org/namespaces/messageHeader")]
        public MessageHeader MessageHeader { get; set; }
    }

    [XmlRoot(ElementName = "Warning", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Warning
    {
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "MessageClass")]
        public string MessageClass { get; set; }
        [XmlAttribute(AttributeName = "ShortText")]
        public string ShortText { get; set; }
    }

    [XmlRoot(ElementName = "Warnings", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Warnings
    {
        [XmlElement(ElementName = "Warning", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Warning> Warning { get; set; }
    }

    [XmlRoot(ElementName = "DepartureAirport", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class DepartureAirport
    {
        [XmlAttribute(AttributeName = "LocationCode")]
        public string LocationCode { get; set; }
        [XmlAttribute(AttributeName = "TerminalID")]
        public string TerminalID { get; set; }
    }

    [XmlRoot(ElementName = "ArrivalAirport", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ArrivalAirport
    {
        [XmlAttribute(AttributeName = "LocationCode")]
        public string LocationCode { get; set; }
        [XmlAttribute(AttributeName = "TerminalID")]
        public string TerminalID { get; set; }
    }

    [XmlRoot(ElementName = "OperatingAirline", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OperatingAirline
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "FlightNumber")]
        public string FlightNumber { get; set; }
        [XmlAttribute(AttributeName = "CompanyShortName")]
        public string CompanyShortName { get; set; }
    }

    [XmlRoot(ElementName = "Equipment", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Equipment
    {
        [XmlAttribute(AttributeName = "AirEquipType")]
        public string AirEquipType { get; set; }
        [XmlAttribute(AttributeName = "ChangeofGauge")]
        public string ChangeofGauge { get; set; }
    }

    [XmlRoot(ElementName = "MarketingAirline", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class MarketingAirline
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "DepartureTimeZone", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class DepartureTimeZone
    {
        [XmlAttribute(AttributeName = "GMTOffset")]
        public string GMTOffset { get; set; }
    }

    [XmlRoot(ElementName = "ArrivalTimeZone", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ArrivalTimeZone
    {
        [XmlAttribute(AttributeName = "GMTOffset")]
        public string GMTOffset { get; set; }
    }

    [XmlRoot(ElementName = "eTicket", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ETicket
    {
        [XmlAttribute(AttributeName = "Ind")]
        public bool Ind { get; set; }
    }

    [XmlRoot(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TPA_Extensions
    {
        [XmlElement(ElementName = "eTicket", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ETicket ETicket { get; set; }
        [XmlElement(ElementName = "Messages", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Messages Messages { get; set; }
        [XmlElement(ElementName = "BaggageInformationList", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public BaggageInformationList BaggageInformationList { get; set; }
        [XmlElement(ElementName = "FareCalcLine", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public FareCalcLine FareCalcLine { get; set; }
        [XmlElement(ElementName = "SeatsRemaining", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public SeatsRemaining SeatsRemaining { get; set; }
        [XmlElement(ElementName = "Cabin", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Cabin Cabin { get; set; }
        [XmlElement(ElementName = "Meal", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Meal Meal { get; set; }
        [XmlElement(ElementName = "DivideInParty", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public DivideInParty DivideInParty { get; set; }
        [XmlElement(ElementName = "ValidatingCarrier", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ValidatingCarrier ValidatingCarrier { get; set; }
        [XmlElement(ElementName = "Surcharges", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Surcharges> Surcharges { get; set; }
    }

    [XmlRoot(ElementName = "FlightSegment", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class FlightSegment
    {
        [XmlElement(ElementName = "DepartureAirport", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public DepartureAirport DepartureAirport { get; set; }
        [XmlElement(ElementName = "ArrivalAirport", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ArrivalAirport ArrivalAirport { get; set; }
        [XmlElement(ElementName = "OperatingAirline", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public OperatingAirline OperatingAirline { get; set; }
        [XmlElement(ElementName = "Equipment", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Equipment Equipment { get; set; }
        [XmlElement(ElementName = "MarketingAirline", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public MarketingAirline MarketingAirline { get; set; }
        [XmlElement(ElementName = "MarriageGrp", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string MarriageGrp { get; set; }
        [XmlElement(ElementName = "DepartureTimeZone", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public DepartureTimeZone DepartureTimeZone { get; set; }
        [XmlElement(ElementName = "ArrivalTimeZone", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ArrivalTimeZone ArrivalTimeZone { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions TPA_Extensions { get; set; }
        [XmlAttribute(AttributeName = "DepartureDateTime")]
        public string DepartureDateTime { get; set; }
        [XmlAttribute(AttributeName = "ArrivalDateTime")]
        public string ArrivalDateTime { get; set; }
        [XmlAttribute(AttributeName = "StopQuantity")]
        public int StopQuantity { get; set; }
        [XmlAttribute(AttributeName = "FlightNumber")]
        public string FlightNumber { get; set; }
        [XmlAttribute(AttributeName = "ResBookDesigCode")]
        public string ResBookDesigCode { get; set; }
        [XmlAttribute(AttributeName = "ElapsedTime")]
        public int ElapsedTime { get; set; }
        [XmlElement(ElementName = "DisclosureAirline", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public DisclosureAirline DisclosureAirline { get; set; }
        [XmlElement(ElementName = "OnTimePerformance", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public OnTimePerformance OnTimePerformance { get; set; }
        [XmlElement(ElementName = "StopAirports", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public StopAirports StopAirports { get; set; }
    }

    [XmlRoot(ElementName = "OriginDestinationOption", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OriginDestinationOption
    {
        [XmlElement(ElementName = "FlightSegment", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<FlightSegment> FlightSegment { get; set; }
        [XmlAttribute(AttributeName = "ElapsedTime")]
        public int ElapsedTime { get; set; }
    }

    [XmlRoot(ElementName = "OriginDestinationOptions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OriginDestinationOptions
    {
        [XmlElement(ElementName = "OriginDestinationOption", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<OriginDestinationOption> OriginDestinationOption { get; set; }
    }

    [XmlRoot(ElementName = "AirItinerary", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AirItinerary
    {
        [XmlElement(ElementName = "OriginDestinationOptions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public OriginDestinationOptions OriginDestinationOptions { get; set; }
        [XmlAttribute(AttributeName = "DirectionInd")]
        public string DirectionInd { get; set; }
    }

    [XmlRoot(ElementName = "BaseFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class BaseFare
    {
        [XmlAttribute(AttributeName = "Amount")]
        public double Amount { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "DecimalPlaces")]
        public int DecimalPlaces { get; set; }
    }

    [XmlRoot(ElementName = "FareConstruction", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class FareConstruction
    {
        [XmlAttribute(AttributeName = "Amount")]
        public double Amount { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "DecimalPlaces")]
        public int DecimalPlaces { get; set; }
    }

    [XmlRoot(ElementName = "EquivFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class EquivFare
    {
        [XmlAttribute(AttributeName = "Amount")]
        public double Amount { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "DecimalPlaces")]
        public int DecimalPlaces { get; set; }
    }

    [XmlRoot(ElementName = "Tax", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Tax
    {
        [XmlAttribute(AttributeName = "TaxCode")]
        public string TaxCode { get; set; }
        [XmlAttribute(AttributeName = "Amount")]
        public double Amount { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "DecimalPlaces")]
        public int DecimalPlaces { get; set; }
    }

    [XmlRoot(ElementName = "Taxes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Taxes
    {
        [XmlElement(ElementName = "Tax", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Tax> Tax { get; set; }
        [XmlElement(ElementName = "TotalTax", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TotalTax TotalTax { get; set; }
    }

    [XmlRoot(ElementName = "TotalFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TotalFare
    {
        [XmlAttribute(AttributeName = "Amount")]
        public double Amount { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "DecimalPlaces")]
        public int DecimalPlaces { get; set; }
    }

    [XmlRoot(ElementName = "ItinTotalFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ItinTotalFare
    {
        [XmlElement(ElementName = "BaseFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public BaseFare BaseFare { get; set; }
        [XmlElement(ElementName = "FareConstruction", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public FareConstruction FareConstruction { get; set; }
        [XmlElement(ElementName = "EquivFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public EquivFare EquivFare { get; set; }
        [XmlElement(ElementName = "Taxes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Taxes Taxes { get; set; }
        [XmlElement(ElementName = "TotalFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TotalFare TotalFare { get; set; }
    }

    [XmlRoot(ElementName = "PassengerTypeQuantity", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PassengerTypeQuantity
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "Quantity")]
        public string Quantity { get; set; }
    }

    [XmlRoot(ElementName = "FareBasisCode", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class FareBasisCode
    {
        [XmlAttribute(AttributeName = "BookingCode")]
        public string BookingCode { get; set; }
        [XmlAttribute(AttributeName = "DepartureAirportCode")]
        public string DepartureAirportCode { get; set; }
        [XmlAttribute(AttributeName = "ArrivalAirportCode")]
        public string ArrivalAirportCode { get; set; }
        [XmlText]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "AvailabilityBreak")]
        public string AvailabilityBreak { get; set; }
    }

    [XmlRoot(ElementName = "FareBasisCodes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class FareBasisCodes
    {
        [XmlElement(ElementName = "FareBasisCode", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<FareBasisCode> FareBasisCode { get; set; }
    }

    [XmlRoot(ElementName = "TotalTax", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TotalTax
    {
        [XmlAttribute(AttributeName = "Amount")]
        public double Amount { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "DecimalPlaces")]
        public int DecimalPlaces { get; set; }
    }

    [XmlRoot(ElementName = "Message", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Message
    {
        [XmlAttribute(AttributeName = "AirlineCode")]
        public string AirlineCode { get; set; }
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "FailCode")]
        public string FailCode { get; set; }
        [XmlAttribute(AttributeName = "Info")]
        public string Info { get; set; }
    }

    [XmlRoot(ElementName = "Messages", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Messages
    {
        [XmlElement(ElementName = "Message", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Message> Message { get; set; }
    }

    [XmlRoot(ElementName = "Segment", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Segment
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "Allowance", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Allowance
    {
        [XmlAttribute(AttributeName = "Pieces")]
        public string Pieces { get; set; }
    }

    [XmlRoot(ElementName = "BaggageInformation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class BaggageInformation
    {
        [XmlElement(ElementName = "Segment", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<Segment> Segment { get; set; }
        [XmlElement(ElementName = "Allowance", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Allowance Allowance { get; set; }
    }

    [XmlRoot(ElementName = "BaggageInformationList", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class BaggageInformationList
    {
        [XmlElement(ElementName = "BaggageInformation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<BaggageInformation> BaggageInformation { get; set; }
    }

    [XmlRoot(ElementName = "PassengerFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PassengerFare
    {
        [XmlElement(ElementName = "BaseFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public BaseFare BaseFare { get; set; }
        [XmlElement(ElementName = "FareConstruction", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public FareConstruction FareConstruction { get; set; }
        [XmlElement(ElementName = "EquivFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public EquivFare EquivFare { get; set; }
        [XmlElement(ElementName = "Taxes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Taxes Taxes { get; set; }
        [XmlElement(ElementName = "TotalFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TotalFare TotalFare { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions TPA_Extensions { get; set; }
    }

    [XmlRoot(ElementName = "Endorsements", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Endorsements
    {
        [XmlAttribute(AttributeName = "NonRefundableIndicator")]
        public string NonRefundableIndicator { get; set; }
    }

    [XmlRoot(ElementName = "FareCalcLine", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class FareCalcLine
    {
        [XmlAttribute(AttributeName = "Info")]
        public string Info { get; set; }
    }

    [XmlRoot(ElementName = "SeatsRemaining", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class SeatsRemaining
    {
        [XmlAttribute(AttributeName = "Number")]
        public int Number { get; set; }
        [XmlAttribute(AttributeName = "BelowMin")]
        public bool BelowMin { get; set; }
    }

    [XmlRoot(ElementName = "Cabin", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Cabin
    {
        [XmlAttribute(AttributeName = "Cabin")]
        public string _Cabin { get; set; }
    }

    [XmlRoot(ElementName = "Meal", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Meal
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "FareInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class FareInfo
    {
        [XmlElement(ElementName = "FareReference", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string FareReference { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions TPA_Extensions { get; set; }
    }

    [XmlRoot(ElementName = "FareInfos", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class FareInfos
    {
        [XmlElement(ElementName = "FareInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<FareInfo> FareInfo { get; set; }
    }

    [XmlRoot(ElementName = "PTC_FareBreakdown", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PTC_FareBreakdown
    {
        [XmlElement(ElementName = "PassengerTypeQuantity", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public PassengerTypeQuantity PassengerTypeQuantity { get; set; }
        [XmlElement(ElementName = "FareBasisCodes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public FareBasisCodes FareBasisCodes { get; set; }
        [XmlElement(ElementName = "PassengerFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public PassengerFare PassengerFare { get; set; }
        [XmlElement(ElementName = "Endorsements", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Endorsements Endorsements { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions TPA_Extensions { get; set; }
        [XmlElement(ElementName = "FareInfos", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public FareInfos FareInfos { get; set; }
    }

    [XmlRoot(ElementName = "PTC_FareBreakdowns", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PTC_FareBreakdowns
    {
        [XmlElement(ElementName = "PTC_FareBreakdown", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<PTC_FareBreakdown> PTC_FareBreakdown { get; set; }
    }

    [XmlRoot(ElementName = "DivideInParty", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class DivideInParty
    {
        [XmlAttribute(AttributeName = "Indicator")]
        public string Indicator { get; set; }
    }

    [XmlRoot(ElementName = "AirItineraryPricingInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class AirItineraryPricingInfo
    {
        [XmlElement(ElementName = "ItinTotalFare", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ItinTotalFare ItinTotalFare { get; set; }
        [XmlElement(ElementName = "PTC_FareBreakdowns", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public PTC_FareBreakdowns PTC_FareBreakdowns { get; set; }
        [XmlElement(ElementName = "FareInfos", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public FareInfos FareInfos { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions TPA_Extensions { get; set; }
        [XmlAttribute(AttributeName = "PricingSource")]
        public string PricingSource { get; set; }
        [XmlAttribute(AttributeName = "PricingSubSource")]
        public string PricingSubSource { get; set; }
        [XmlAttribute(AttributeName = "LastTicketDate")]
        public string LastTicketDate { get; set; }
    }

    [XmlRoot(ElementName = "TicketingInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TicketingInfo
    {
        [XmlAttribute(AttributeName = "TicketType")]
        public string TicketType { get; set; }
        [XmlAttribute(AttributeName = "ValidInterline")]
        public string ValidInterline { get; set; }
    }

    [XmlRoot(ElementName = "ValidatingCarrier", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ValidatingCarrier
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "PricedItinerary", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PricedItinerary
    {
        [XmlElement(ElementName = "AirItinerary", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AirItinerary AirItinerary { get; set; }
        [XmlElement(ElementName = "AirItineraryPricingInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public AirItineraryPricingInfo AirItineraryPricingInfo { get; set; }
        [XmlElement(ElementName = "TicketingInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TicketingInfo TicketingInfo { get; set; }
        [XmlElement(ElementName = "TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TPA_Extensions TPA_Extensions { get; set; }
        [XmlAttribute(AttributeName = "SequenceNumber")]
        public int SequenceNumber { get; set; }
    }

    [XmlRoot(ElementName = "DisclosureAirline", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class DisclosureAirline
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "OnTimePerformance", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OnTimePerformance
    {
        [XmlAttribute(AttributeName = "Percentage")]
        public string Percentage { get; set; }
        [XmlAttribute(AttributeName = "Level")]
        public string Level { get; set; }
    }

    [XmlRoot(ElementName = "Surcharges", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Surcharges
    {
        [XmlAttribute(AttributeName = "Ind")]
        public string Ind { get; set; }
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "StopAirport", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class StopAirport
    {
        [XmlAttribute(AttributeName = "LocationCode")]
        public string LocationCode { get; set; }
        [XmlAttribute(AttributeName = "ArrivalDateTime")]
        public string ArrivalDateTime { get; set; }
        [XmlAttribute(AttributeName = "DepartureDateTime")]
        public string DepartureDateTime { get; set; }
        [XmlAttribute(AttributeName = "ElapsedTime")]
        public int ElapsedTime { get; set; }
        [XmlAttribute(AttributeName = "Duration")]
        public int Duration { get; set; }
        [XmlAttribute(AttributeName = "GMTOffset")]
        public double GMTOffset { get; set; }
        [XmlAttribute(AttributeName = "Equipment")]
        public string Equipment { get; set; }
    }

    [XmlRoot(ElementName = "StopAirports", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class StopAirports
    {
        [XmlElement(ElementName = "StopAirport", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public StopAirport StopAirport { get; set; }
    }

    [XmlRoot(ElementName = "PricedItineraries", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PricedItineraries
    {
        [XmlElement(ElementName = "PricedItinerary", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<PricedItinerary> PricedItinerary { get; set; }
    }

    [XmlRoot(ElementName = "OTA_AirLowFareSearchRS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OTA_AirLowFareSearchRS
    {
        [XmlElement(ElementName = "Success", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Success { get; set; }
        [XmlElement(ElementName = "Warnings", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Warnings Warnings { get; set; }
        [XmlElement(ElementName = "PricedItineraries", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public PricedItineraries PricedItineraries { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "PricedItinCount")]
        public int PricedItinCount { get; set; }
        [XmlAttribute(AttributeName = "BrandedOneWayItinCount")]
        public int BrandedOneWayItinCount { get; set; }
        [XmlAttribute(AttributeName = "SimpleOneWayItinCount")]
        public int SimpleOneWayItinCount { get; set; }
        [XmlAttribute(AttributeName = "DepartedItinCount")]
        public int DepartedItinCount { get; set; }
        [XmlAttribute(AttributeName = "SoldOutItinCount")]
        public int SoldOutItinCount { get; set; }
        [XmlAttribute(AttributeName = "AvailableItinCount")]
        public int AvailableItinCount { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class FlightBargainObject
    {
        [XmlElement(ElementName = "OTA_AirLowFareSearchRS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public OTA_AirLowFareSearchRS OTA_AirLowFareSearchRS { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public FlightBargainObject FlightBargainObject { get; set; }
        [XmlAttribute(AttributeName = "SOAP-ENV", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string SOAPENV { get; set; }
    }

}


