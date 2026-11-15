using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;

namespace MexCoreWebAPI.Model
{
    public class HitItResult
    {
        [Key]
        public int Id { get; set; }
        public Guid TransactionId { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public string? StartDate { get; set; }
        public int TotalPax { get; set; }   
        public byte[]? HititResponse { get; set; }
        public byte[]? FareList { get; set; }
        public byte[]? CityInfo { get; set; }
        public byte[]? FlightNotes { get; set; }
        public byte[]? FinalResult { get; set; }
        public bool Active { get; set; }    
    }
}
