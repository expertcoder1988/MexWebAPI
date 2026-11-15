
namespace MexCoreWebAPI.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Airline
    {
       
        public int AirlineID { get; set; }
        public string? AirlineName { get; set; }
        public string? AirlineShortName { get; set; }
        public string? IataCode { get; set; }
        public string? ICAOCode { get; set; }
        public string? AirlineCapital { get; set; }
        public string? Country { get; set; }
        public string? BaseClass { get; set; }
    
     
    }
}
