using Microsoft.AspNetCore.Mvc;

using System.Text;

//using System.Reflection.Metadata;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;
using System.Runtime.Serialization;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using Microsoft.Extensions.Primitives;
using MexCoreWebAPI.Model;
using Newtonsoft.Json;

namespace MexCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly MexDbContext sql;

        //string htmlContent = "<html><body><h1>Hello, world!</h1></body></html>";
        string outputPath = @"D:/Asim/MEX_API_GitHub/MexCoreWebAPI/MexCoreWebAPI/MexCoreWebAPI/Tickets/output.pdf";
        //string htmlFile = @"D:/Asim/MEX_API_GitHub/MexCoreWebAPI/MexCoreWebAPI/MexCoreWebAPI/Tickets/ticket.html";

        public PdfController(MexDbContext context)
        {
            sql = context;
        }

        [HttpGet("generate")]
        public IActionResult GeneratePdf()
        {
            try
            {
               string htmlContent = HTMLContect();
               // string encodedHtml = Encoding.UTF8.GetString(Encoding.Default.GetBytes(htmlContent));
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(outputPath, FileMode.Create));

                doc.Open();

                using (TextReader reader = new StringReader(htmlContent))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, reader);
                }

                doc.Close();

                //string jsonData = System.IO.File.ReadAllText(@"D:/Asim/MEX_API_GitHub/MexCoreWebAPI/MexCoreWebAPI/MexCoreWebAPI/Tickets/my.json");
                //using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData)))
                //{
                //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(MyData));
                //    MyData data = (MyData)serializer.ReadObject(stream);

                //    // Create PDF document
                //    Document doc = new Document();
                //    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(@"D:/Asim/MEX_API_GitHub/MexCoreWebAPI/MexCoreWebAPI/MexCoreWebAPI/Tickets/output.pdf", FileMode.Create));
                //    doc.Open();

                //    // Add content to PDF
                //    doc.Add(new Paragraph("Name: " + data.Name));
                //    doc.Add(new Paragraph("Age: " + data.Age));

                //    // Close document
                //    doc.Close();
                //}


            }
            catch (Exception ex)
            {
                return BadRequest($"Conversion failed: {ex.Message}");
            }

            return null;
        }

        [NonAction]
        public string HTMLContect()
        {
            FinalBooking fb = FlightBookingDetails("21");

            StringBuilder strReq = new StringBuilder();

            strReq.Append("<html>");
            strReq.Append("<head>");
            //strReq.Append("<link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@4.4.1/dist/css/bootstrap.min.css' crossorigin='anonymous'>");
           
            //strReq.Append("<style> .center {" +
            //"border: 5px solid; " +
            //"position: absolute; " +
            //"top: 50 %; " +
            //"left: 50 %; " +
            //"transform: translate(-50 %, -50 %); " +
            //"padding: 10px; " +
            //"} </style>");

            strReq.Append("</head>");
            strReq.Append("<body>");
            strReq.Append("<h1>Booking Details</h1>");
            strReq.Append("<h2>PNR: <span style='color:#E5B815'>IKRTGB</span></h2>");
            strReq.Append("<hr/>");
          strReq.Append("<br/>");
            strReq.Append("<h3>Passenger Details</h3>");
            strReq.Append("<Table style='font-size:10px;width:100%' border='0'><tr><th>Last Name</th><th>First Name</th><th>DOB</th><th>Doc. Type</th><th>Doc. No.</th><th>Expiry</th></tr>");
            foreach (var itm in fb.PaxList)
            {
                strReq.Append("<tr><td>" + itm.FirstName + "</td><td>" + itm.LastName + "</td><td>" + Convert.ToDateTime(itm.DOB).ToShortDateString() + "</td><td>" + itm.DocumentType + "</td><td>" + itm.Document + "</td><td>" + Convert.ToDateTime(itm.DOE).ToShortDateString() + "</td></tr>");
            }
            strReq.Append("</table>");
            strReq.Append("<br/>");
            strReq.Append("<hr/>");
            strReq.Append("<h4>Flight Details</h4>");
           strReq.Append("<Table style='font-size:10px;width:100%' border='0'><tr><th></th><th>Origin</th><th>Destination</th><th>Departure Date</th><th>Arrival Date</th><th>Cabin</th></tr>");
            foreach (var itm in fb.flightSegments)
            {
                strReq.Append("<tr><td></td><td>" + itm.OriginID + "</td><td>" + itm.DestinationID + "</td><td>" + Convert.ToDateTime(itm.DepartureDate).ToShortDateString() + "<br/>" + itm.DepartureTime + "</td><td>" + Convert.ToDateTime(itm.ArrivalDate).ToShortDateString() + "<br/>" + itm.ArrivalTime + "</td><td>" + Cabin(itm.ServiceClassID) + "(" + itm.ResBookDesigCode + ")" + "</td></tr>");
            }
            strReq.Append("</table>");
            strReq.Append("<hr/>");
            strReq.Append("</body>");
            strReq.Append("</html>");

            return strReq.ToString();

        }

        [NonAction]
        public FinalBooking FlightBookingDetails(string BookingID)
        {
            try
            {
                int bookingID = Convert.ToInt32(BookingID);
                var reservation = (from x in sql.Reservations.AsEnumerable()
                                   where x.FlightBookingID == bookingID
                                   select new Reservation
                                   {
                                       Id = x.Id,
                                       Adults = x.Adults,
                                       Children = x.Children,
                                       Infants = x.Infants,
                                       AdultBasicFare = x.AdultBasicFare,
                                       ChildBasicFare = x.ChildBasicFare,
                                       InfantBasicFare = x.InfantBasicFare,
                                       AdultTax = x.AdultTax,
                                       ChildTax = x.ChildTax,
                                       InfantTax = x.InfantTax,
                                       TotalFare = x.TotalFare,
                                       TotalTaxes = x.TotalTaxes,
                                       GrandTotal = x.GrandTotal,
                                       CustomerComission = x.CustomerComission,
                                       CustomerOComission = x.CustomerOComission,
                                       AirlineComission = x.AirlineComission,
                                       Incentive = x.Incentive,
                                       BookingDate = x.BookingDate,
                                       BookingStatus = x.BookingStatus,
                                       BookingTypeID = x.BookingTypeID,
                                       LeadFirstName = x.LeadFirstName,
                                       LeadLastName = x.LeadLastName,
                                       Email = x.Email,
                                       PrimaryPhone = x.PrimaryPhone,
                                       SecondaryPhone = x.SecondaryPhone,
                                       PNR = x.PNR
                                   }).ToList();

                //var fs = sql.FlightSegments.Where(f => f.FlightBookingID == bookingID).Select(f => f.ServiceClassID).FirstOrDefault();


                var flightSegments = (from x in sql.FlightSegments.AsEnumerable()
                                      where x.FlightBookingID == bookingID
                                      select new FlightSegment
                                      {
                                          ArrivalDate = x.ArrivalDate,
                                          ArrivalTime = x.ArrivalTime,
                                          ArrivalGMTOffset = x.ArrivalGMTOffset,
                                          DepartureDate = x.DepartureDate,
                                          DepartureTime = x.DepartureTime,
                                          DepartureGMTOffset = x.DepartureGMTOffset,
                                          ServiceClassID = x.ServiceClassID,
                                          OriginID = x.OriginID,
                                          DestinationID = x.DestinationID,
                                          FlightNumber = x.FlightNumber,
                                          MarketingAirlineCode = x.MarketingAirlineCode,
                                          OperatingAirlineCode = x.OperatingAirlineCode,
                                          MarketingAirlineID = x.MarketingAirlineID,
                                          OperatingAirlineID = x.OperatingAirlineID,
                                          OperatingAirlineName = x.OperatingAirlineName,
                                          FlightBookingID = x.FlightBookingID,
                                          ETicket = x.ETicket,
                                          ResBookDesigCode = x.ResBookDesigCode,
                                          Way = x.Way
                                      }).ToList();

                var PaxList = (from x in sql.PassengerDetails.AsEnumerable()
                               where x.FlightBookingID == bookingID
                               select new PassengerDetail
                               {
                                   Title = string.IsNullOrWhiteSpace(x.Title) ? "" : x.Title,
                                   FirstName = string.IsNullOrWhiteSpace(x.FirstName) ? "" : x.FirstName,
                                   LastName = string.IsNullOrWhiteSpace(x.LastName) ? "" : x.LastName,
                                   DOB = x.DOB,
                                   Gender = x.Gender,
                                   Document = string.IsNullOrWhiteSpace(x.Document) ? "" : x.Document,
                                   TicketNo = string.IsNullOrWhiteSpace(x.TicketNo) ? "" : x.TicketNo,
                                   DocumentType = string.IsNullOrWhiteSpace(x.DocumentType) ? "" : x.DocumentType,
                                   DOE = x.DOE,
                                   FlightBookingID = x.FlightBookingID,
                                   PassengerStatus = x.PassengerStatus,
                                   PassegerType = x.PassegerType
                               }).ToList();


                var container = new FinalBooking
                {
                    Id = bookingID,
                    Reservation = reservation,
                    flightSegments = flightSegments,
                    PaxList = PaxList,
                    //PNR = PNR,
                };

                //string jsonString = JsonConvert.SerializeObject(container, Newtonsoft.Json.Formatting.Indented);


                return container;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [NonAction]
        private string Cabin(int? code)
        {
            //var cabin = (from c in sql.ServiceClasses.AsEnumerable().Where(c => c.ServiceClassID.Equals(code)).Select new { c.ClassName }).FirstOrDefault();

            var cabin = sql.ServiceClasses.Where(p => p.ServiceClassID == code).FirstOrDefault();

            return cabin.ClassName.ToString();
        }

        [NonAction]
        private string Class(string code)
        {
            //var cabin = (from c in sql.ServiceClasses.AsEnumerable().Where(c => c.ServiceClassID.Equals(code)).Select new { c.ClassName }).FirstOrDefault();

            var cabin = sql.ServiceClasses.Where(p => p.ServiceClassID.Equals(code)).FirstOrDefault();

            return cabin.ClassName.ToString();
        }

    }

    [DataContract]
    public class MyData
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "age")]
        public int Age { get; set; }

        // Add more properties as needed
    }
}


//public string HTMLContect()
//{
//    StringBuilder strReq = new StringBuilder();

//    strReq.Append("<Html>");
//    strReq.Append("<Head>");
//    //strReq.Append("<link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@4.4.1/dist/css/bootstrap.min.css' integrity='sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh' crossorigin='anonymous'>");
//    strReq.Append("</Head>");
//    strReq.Append("<Body>");
//    strReq.Append("<section className = 'innerpage-wrapper>");
//    strReq.Append("<div className='innerpage-section-padding'>");
//    strReq.Append("<div className='container SlipBackground p-0' style='maxWidth:'960px'>");
//    strReq.Append("<div className='w-100 m-0 pt-2 pl-2 MainSlip' style='fontWeight:600;color:'white'> <h3> BOOKING DETAILS </h3></div>");
//    strReq.Append("<div className='w-100 m-0 pt-2 pl-2' style='color:#0867d2;backgroundColor:#d0dadf':fontWeight:600><span> Islamabad - London </span></div>");
//    strReq.Append("<div>");
//    strReq.Append("<div className='d-flex justify-content-between col m-5 w-100'>");
//    strReq.Append("<div>");
//    strReq.Append("<label> PNR ID:</label>");
//    strReq.Append("<h2>");
//    strReq.Append("8ZAUI0M");
//    strReq.Append("</h2>");
//    strReq.Append("</div>");
//    strReq.Append("<div>");
//    strReq.Append("</div>");
//    strReq.Append("<span className='float-right mr-5'>");
//    strReq.Append("<label> Total Amount:</label>");
//    strReq.Append("<h3>");
//    strReq.Append("40000");
//    strReq.Append("Grand Total");
//    strReq.Append("</h3>");
//    strReq.Append("</span>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='row m-5'>");
//    strReq.Append("<div className='col-lg-4 col-sm-12'>");
//    strReq.Append("<span><span> Passenger Name:</span> <span style='fontWeight:600;fontSize:18px'>");
//    strReq.Append("Asif Kiani");
//    strReq.Append("</span></span>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='col-lg-4 col-sm-12'>");
//    strReq.Append("<span><span> Gender:</span> <span style='fontWeight:600;fontSize:18px'>");
//    strReq.Append("Male");
//    strReq.Append("</span></span>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='col-lg-4 col-sm-12'>");
//    strReq.Append("<span><span> DOB:</span> <span style='fontWeight:600;fontSize:18px'>");
//    strReq.Append("15 August 2023");
//    strReq.Append("</span></span>");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='row m-5'>");
//    strReq.Append("<div className='col-lg-4 col-sm-12'>");
//    strReq.Append("<span><span> Document #:</span> <span style='fontWeight:600;fontSize:'18px'>");
//    strReq.Append("6110478789878");
//    strReq.Append("</span></span>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='col-lg-4 col-sm-12'>");
//    strReq.Append("<span><span> DOE:</span> <span style='fontWeight:600; fontSize:18px'>");
//    strReq.Append("15 August 2024");
//    strReq.Append("</span></span>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='col-lg-4 col-sm-12'>");
//    strReq.Append("<span><span> Booking Date:</span> <span style='fontWeight:600; fontSize:18px'>");
//    strReq.Append("15 August 2024");
//    strReq.Append("</span></span>");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='row col-12 p-0 m-0' style='backgroundColor:#eeeeee'>");
//    strReq.Append("<div className='col-lg-1 col-sm-12 p-2 ml-5'>");
//    strReq.Append("<div>");
//    strReq.Append("PK-2398");
//    strReq.Append("</div>");
//    strReq.Append("<div>");
//    strReq.Append("<img  src ='images/airlines/PK.png'  style='width:47px;height:30px'  alt='flight-img' />");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='col-5 p-2'>");
//    strReq.Append("<div style='fontWeight:bolder'> DEPART:</div>");
//    strReq.Append("<div style='lineHeight:initial'> Islamabad International(ISB) </div>");
//    strReq.Append("Thursday 24 August 2023 05:00AM");
//    strReq.Append("</div>");
//    strReq.Append("<div className='col-5 p-2>");
//    strReq.Append("<div style='fontWeight:bolder'> ARRIV:</div>");
//    strReq.Append("<div style='lineHeight:initial'> Istanbul International(IST) </div>");
//    strReq.Append("Saturday 26 August 2023 05:00AM");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='row'>");
//    strReq.Append("<div className='ml-5'>");
//    strReq.Append("<span> <span className='font-weight-bold' style='color:#6e6464'> Duration:</span> <span style='color:#6e6464'} }> 5h 55m </span></span>");
//    strReq.Append("<span className='ml-2'> <span className='font-weight-bold' style='color:6e6464'> Cabin Class:</span> <span style='color:#6e6464'> Economy(Y) </span></span>");
//    strReq.Append("<span className='ml-2'> <span className='font-weight-bold' style='color:6e6464'> Airline:</span> <span style='color:#6e6464'> Pakistan Airlines </span></span>");
//    strReq.Append("<span className='ml-2'> <span className='font-weight-bold' style='color:6e6464'> Stop:</span> <span style='color:#6e6464'> 0 </span></span>");
//    strReq.Append("<span className='ml-2'> <span className='font-weight-bold' style='color:6e6464'> Baggage Allowance:</span> <span style='color:#6e6464'> 40 Kilograms </span></span>");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='row col-12 p-0 m-0' style='backgroundColor:#eeeeee'>");
//    strReq.Append("<div className='col-lg-1 col-sm-12 p-2 ml-5'>");
//    strReq.Append("<div>");
//    strReq.Append("PK - 2312");
//    strReq.Append("</div>");
//    strReq.Append("<div>");
//    strReq.Append("<img  src ='images/airlines/PK.png' style='width:47px; height:30px' alt='flight-img' />");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='col-5 p-2'>");
//    strReq.Append("<div style='fontWeight:bolder'> DEPART:</div>");
//    strReq.Append("<div style='lineHeight:initial'> Istanbul International(IST) </div>");
//    strReq.Append("<div style='lineHeight:initial'> Thursday 29 August 2023 05:05AM </div>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='col-5 p-2'>");
//    strReq.Append("<div style='fontWeight:bolder'> ARRIV:</div>");
//    strReq.Append("<div style='lineHeight:initial'> Islamabad International(ISB) </div>");
//    strReq.Append("<div style='lineHeight:initial'> Thursday 30 August 2023 05:05AM </div>");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("<div className='row'>");
//    strReq.Append("<div className='ml-5'>");
//    strReq.Append("<span> <span className='font-weight-bold' style='color:#6e6464'> Duration:</span> <span style='color:#6e6464'> 5h 55m </span></span>");
//    strReq.Append("<span className='ml-2'> <span className='font-weight-bold' style=color:#6e6464'> Cabin Class:</span> <span style='color:#6e6464> Economy(Y) </span></span>");
//    strReq.Append("<span className='ml-2'> <span className='font-weight-bold' style=color:#6e6464'> Airline:</span> <span style='color:#6e6464'> Pakistan Airlines </span></span>");
//    strReq.Append("<span className='ml-2'> <span className='font-weight-bold' style=color:#6e6464'> Stop:</span> <span style='color:#6e6464'> 0 </span></span>");
//    strReq.Append("<span className='ml-2'> <span className='font-weight-bold' style=color:#6e6464'> Baggage Allowance:</span> <span style='color:#6e6464'> 40 Kilograms </span></span>");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("</div>");
//    strReq.Append("</section>");
//    strReq.Append("</Body>");
//    strReq.Append("</Html>");

//    return strReq.ToString();

//}