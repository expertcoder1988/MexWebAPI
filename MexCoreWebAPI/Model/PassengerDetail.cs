using MexCoreWebAPI.Controllers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using MalikExpressMVC.Models.XML;

namespace MexCoreWebAPI.Model
{
    public class PassengerDetail
    {
        public PassengerDetail()
        {
            BookingRefunds = new HashSet<BookingRefund>();
            FlightSeats = new HashSet<FlightSeat>();
        }

        public int? PassengerDetailID { get; set; }

        public int? FlightBookingID { get; set; }

        [StringLength(10)]
        public string? Title { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        public bool? Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }

        [StringLength(10)]
        public string? DocumentType { get; set; }

        [StringLength(20)]
        public string? Document { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOE { get; set; }

        [StringLength(20)]
        public string? TicketNo { get; set; }

        public DateTime? TicketedDate { get; set; }

        public byte? PassengerStatus { get; set; }

        [StringLength(250)]
        public string? SSR { get; set; }

        [StringLength(250)]
        public string? Note { get; set; }

        [StringLength(5)]
        public string? PassegerType { get; set; }

     
        public virtual ICollection<BookingRefund> BookingRefunds { get; set; }

        public virtual Reservation FlightBooking { get; set; }

     
        public virtual ICollection<FlightSeat> FlightSeats { get; set; }
    }

    public class AirPricingInfoKey
    {
        public int Id { get; set; }
        public string PNR { get; set; }
        public string PnrForTicket { get; set; }
        public string airPricingInfoKey { get; set; }
        public int? BookingId { get; set; }
        public string AirlinePNR {get; set; }
    }
    public partial class ServiceClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServiceClass()
        {
            FlightSegments = new HashSet<FlightSegment>();
        }

        public int ServiceClassID { get; set; }

        [StringLength(20)]
        public string ClassName { get; set; }

        public bool? Status { get; set; }

        public bool? DefaultSelected { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FlightSegment> FlightSegments { get; set; }
    }

    public partial class BookingRefund
    {
        public int BookingRefundID { get; set; }

        public int? PassengerDetailID { get; set; }

        public int? FlightSegmentID { get; set; }

        public DateTime? RefundDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? RefundCharges { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        public virtual FlightSegment FlightSegment { get; set; }

        public virtual PassengerDetail PassengerDetail { get; set; }
    }

    public partial class FlightSeat
    {
        [Key]
        public int SeatID { get; set; }

        public int? PassengerDetailID { get; set; }

        public int? FlightSegmentID { get; set; }

        public DateTime? TicketedDate { get; set; }

        [StringLength(20)]
        public string TicketNumber { get; set; }

        [StringLength(4)]
        public string SeatNumber { get; set; }

        public byte? PaxSegmentStatus { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        public virtual FlightSegment FlightSegment { get; set; }

        public virtual PassengerDetail PassengerDetail { get; set; }
    }

    public partial class FlightSegment
    {
       public int FlightSegmentID { get; set; }

        public int? OriginID { get; set; }

        public int? DestinationID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DepartureDate { get; set; }


        public string? DepartureTime { get; set; }


        public string? DepartureGMTOffset { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ArrivalDate { get; set; }


        public string? ArrivalTime { get; set; }


        public string? ArrivalGMTOffset { get; set; }

        public string? SegmentType { get; set; }

        public string? AirEquipType { get; set; }

        public bool? ETicket { get; set; }

        public int? OperatingAirlineID { get; set; }

        public int? MarketingAirlineID { get; set; }

    

        [StringLength(10)]
        public string? OperatingAirlineCode { get; set; }

        [StringLength(10)]
        public string? MarketingAirlineCode { get; set; }

        [StringLength(50)]
        public string? OperatingAirlineName { get; set; }

        [StringLength(10)]
        public string? FlightNumber { get; set; }

        public int? FlightBookingID { get; set; }
        public string? Baggage { get; set; }
        public int? ServiceClassID { get; set; }

        public string? ResBookDesigCode { get; set; }
        public byte? Way { get; set; }
        public string? OriginCityName { get; set; }
        public string? DestinationCityName { get; set; }

    }

    public class FareBasisCodes
    {
        public List<FareBasisCode> FareBasisCode { get; set; }
    }

    public class FareBasisCode
    {
        public int FareBasisCodeID { get; set; }

        [StringLength(60)]
        public string Code { get; set; }

        [StringLength(20)]
        public string Text { get; set; }

        [StringLength(3)]
        public string PaxType { get; set; }

        public int? FlightSegmentID { get; set; }

        public FlightSegment FlightSegment { get; set; }
    }

    public class OriginDestinationOption
    {
        [XmlElement(ElementName = "FlightSegment", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<FlightSegment> FlightSegment { get; set; }
        [XmlAttribute(AttributeName = "ElapsedTime")]
        public int ElapsedTime { get; set; }
    }

    [Table("AirlineSpecificServiceFee")]
    public partial class AirlineSpecificServiceFee
    {
        [Key]
        public int AirlineSpecificID { get; set; }

        [StringLength(2)]
        public string AirlineCode { get; set; }

        [StringLength(10)]
        public string FeeType { get; set; }

        public double? Amount { get; set; }

        [StringLength(10)]
        public string ServiceType { get; set; }

        [StringLength(10)]
        public string PassengerType { get; set; }
    }

    public partial class Incentive
    {
        public int IncentiveID { get; set; }

        [StringLength(50)]
        public string? IncentiveType { get; set; }

        [Column(TypeName = "money")]
        public decimal? Incentives { get; set; }

        [StringLength(10)]
        public string? Scale { get; set; }

        [Column(TypeName = "money")]
        public decimal? RefIncentives { get; set; }

        [StringLength(10)]
        public string? RefScale { get; set; }

        public int? ServiceID { get; set; }

        public int? CustomerID { get; set; }

        public virtual Customer Customer { get; set; }
    }

 

 

    public class Envelope1
    {
        [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
        [XmlAttribute(AttributeName = "soap-env", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soapenv { get; set; }
    }
}
