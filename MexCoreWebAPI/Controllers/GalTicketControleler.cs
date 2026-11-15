using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using System.Xml;
using System.Xml.Linq;

namespace MexCoreWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]
    public class GalTicket : ControllerBase
    {
        private readonly MexDbContext obj;
       

        public GalTicket(MexDbContext context)
        {
            obj = context;
        }

        [HttpGet("issuegalticket")]
        public string RetrievePNR(string PNR)
        {
            try
            {
               
                if (string.IsNullOrEmpty(PNR))
                {
                    return "Invalid PNR";
                }
              
                // string temp = Environment.GetEnvironmentVariable("tmp");	// Get temp directory
                //  string PropsFileName = temp + "/session.properties";		// Define dir and file name
                DateTime dt = DateTime.UtcNow;
                string tstamp = dt.ToString("s") + "Z";

                StringBuilder strReq = new StringBuilder();
                Guid TraceId = Guid.NewGuid();

                string APIKey = obj.AirPricingInfoKeys.Where(a => a.PNR == PNR).Select(a => a.airPricingInfoKey).FirstOrDefault();
                string PNRForTicket = obj.AirPricingInfoKeys.Where(a => a.PNR == PNR).Select(a => a.PnrForTicket).FirstOrDefault();
                string AirlinePNR = obj.AirPricingInfoKeys.Where(a => a.PNR == PNR).Select(a => a.AirlinePNR).FirstOrDefault();

                //APIKey = "hKZMqxN3nDKAAGwZxIBAAA==";

                if (APIKey != "" || APIKey != null)
                {
                    strReq.Append(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>");
                    strReq.Append("<soapenv:Header/>");
                    strReq.Append("<soapenv:Body>");
                    strReq.Append("<AirTicketingReq xmlns='http://www.travelport.com/schema/air_v52_0' AuthorizedBy='Travelport' ProviderCode='1G' TargetBranch='P3393195'>");
                    strReq.Append("<BillingPointOfSaleInfo xmlns='http://www.travelport.com/schema/common_v52_0' OriginApplication='uAPI' />");
                    strReq.Append("<AirReservationLocatorCode>" + PNRForTicket + "</AirReservationLocatorCode>");
                    //strReq.Append("<AirPricingInfoRef Key='" + APIKey + "' />");
                    strReq.Append("</AirTicketingReq>");
                    strReq.Append("</soapenv:Body>");
                    strReq.Append("</soapenv:Envelope>");
                    var doc = XDocument.Parse(strReq.ToString());

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    //string url = "https://apac.universal-api.pp.travelport.com/B2BGateway/connect/uAPI/AirService";
                    string url = "https://emea.universal-api.travelport.com/B2BGateway/connect/uAPI/AirService";
                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
                    byte[] bytes;
                    bytes = System.Text.Encoding.ASCII.GetBytes(doc.ToString());
                    //request1.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz")));
                    request1.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                    request1.ContentType = "text/xml; encoding='utf-8'";
                    request1.Method = "Post";
                    request1.Headers.Add("SOAPAction", "");
                    request1.Host = "twsprofiler.travelport.com";
                    request1.KeepAlive = true;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    Stream requestStream1 = request1.GetRequestStream();
                    requestStream1.Write(bytes, 0, bytes.Length);
                    requestStream1.Close();
                    HttpWebResponse response1;
                    response1 = (HttpWebResponse)request1.GetResponse();
                    Stream responseStream;

                    TextReader responseStr;
                    // DataTable FlightSegments = null;
                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();

                    if (response1.StatusCode == HttpStatusCode.OK)
                    {
                        responseStream = response1.GetResponseStream();
                        responseStr = new StreamReader(response1.GetResponseStream(), System.Text.Encoding.Default);

                        var responseReader = new StreamReader(responseStream);
                        var result1 = responseReader.ReadToEnd();
                        //return result;

                        result1 = result1.Replace("universal:", "");
                        result1 = result1.Replace("common_v42_0:", "");
                        result1 = result1.Replace("air:", "");

                        XDocument xdoc = XDocument.Parse(result1);
                        int textCounter = 1;
                        foreach (var textElement in xdoc.Descendants("Text").ToList())
                        {
                            textElement.Name = "Text" + textCounter;
                            textCounter++;
                        }

                        string modifiedXmlString = xdoc.ToString();

                        XmlDocument xml1 = new XmlDocument();
                        xml1.LoadXml(modifiedXmlString);

                        XmlDocument xml = new XmlDocument();
                        XmlReader xmlReader = new XmlNodeReader(xml1);
                        XmlReader reader = doc.CreateReader();

                        ds.ReadXml(xmlReader);



                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables.Contains("Fault"))
                            {
                                string Msg = ds.Tables["Fault"].Rows[0]["Message"].ToString();
                                var jsonError = JsonConvert.SerializeObject(Msg);
                                return jsonError;
                            }
                            else if (ds.Tables.Contains("TicketFailureInfo"))
                            {
                                //string Msg1 = ds.Tables["TicketFailureInfo"].Rows[0]["Message"].ToString();
                                ds1 = AirRetrieveDocumentReq(PNRForTicket);
                            }
                            else
                            {
                                ds1 = ds;
                            }
                        }

                        if (ds1 != null)
                        {
                            if (ds1.Tables.Contains("Ticket"))
                            {
                                DataTable dtTicket = ds1.Tables["Ticket"].Copy();
                                var FlightBookingID = obj.Reservations.Where(a => a.PNR == PNR).Select(a => a.FlightBookingID).FirstOrDefault();
                                var PassengerIDs = obj.PassengerDetails.Where(b => b.FlightBookingID == FlightBookingID).Select(b => b.PassengerDetailID).ToList();

                                for (int f = 0; f < PassengerIDs.Count; f++)
                                {
                                    int? pdID = PassengerIDs[f];
                                    // using (var db = new MexDbContext())
                                    {
                                        var PassengerDetails = obj.PassengerDetails.SingleOrDefault(b => b.FlightBookingID == FlightBookingID && b.PassengerDetailID == pdID);
                                        if (PassengerDetails != null)
                                        {

                                            try
                                            {
                                                if (dtTicket.Rows[f] != null)
                                                {
                                                    PassengerDetails.TicketNo = dtTicket.Rows[f]["TicketNumber"].ToString();
                                                    obj.PassengerDetails.Attach(PassengerDetails);
                                                    obj.Entry(PassengerDetails).State = EntityState.Modified;
                                                    obj.SaveChanges();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                throw;
                                            }
                                        }
                                    }
                                }

                                var Pessengers = obj.PassengerDetails.Where(b => b.FlightBookingID == FlightBookingID).ToList();

                                var bd = from flightBooking in obj.Reservations
                                         where flightBooking.FlightBookingID == FlightBookingID
                                         select new BookingDetailsModel
                                         {
                                             Adults = flightBooking.Adults,
                                             Children = flightBooking.Children,
                                             Infants = flightBooking.Infants,
                                             TotalFare = flightBooking.TotalFare,
                                             TotalTaxes = flightBooking.TotalTaxes,
                                             GrandTotal = flightBooking.GrandTotal
                                         };



                                var result = from flightSegment in obj.FlightSegments
                                             join serviceClass in obj.ServiceClasses on flightSegment.ServiceClassID equals serviceClass.ServiceClassID
                                             join airline in obj.Airlines on flightSegment.OperatingAirlineID equals airline.AirlineID into airlineGroup
                                             from airline in airlineGroup.DefaultIfEmpty()
                                             join destinationCity in obj.Cities on flightSegment.DestinationID equals destinationCity.cityID into destinationCityGroup
                                             from destinationCity in destinationCityGroup.DefaultIfEmpty()
                                             join originCity in obj.Cities on flightSegment.OriginID equals originCity.cityID
                                             where flightSegment.FlightBookingID == FlightBookingID
                                             select new FlightSegmentModel
                                             {
                                                 OriginID = flightSegment.OriginID,
                                                 DestinationID = flightSegment.DestinationID,
                                                 FlightSegmentID = flightSegment.FlightSegmentID,
                                                 Origin = originCity.Airport,
                                                 Destination = destinationCity.Airport,
                                                 ClassName = serviceClass.ClassName,
                                                 ResBookDesigCode = flightSegment.ResBookDesigCode,
                                                 DepartureDate = flightSegment.DepartureDate,
                                                 DepartureTime = flightSegment.DepartureTime,
                                                 ArrivalDate = flightSegment.ArrivalDate,
                                                 ArrivalTime = flightSegment.ArrivalTime,
                                                 FlightBookingID = flightSegment.FlightBookingID,
                                                 OperatingAirlineCode = flightSegment.OperatingAirlineCode,
                                                 FlightNumber = flightSegment.FlightNumber,
                                                 AirlineName = airline.AirlineName
                                             };

                                var FlightSegments = result.ToList();
                                var BookingDetails = bd.ToList();

                                var combinedObject = new
                                {
                                    BookingDetails,
                                    FlightSegments,
                                    Pessengers,
                                    AirlinePNR
                                };

                                var jsonString = JsonConvert.SerializeObject(combinedObject);
                                return jsonString;
                            }
                            else 
                            {
                                string Msg1 = ds.Tables["TicketFailureInfo"].Rows[0]["Message"].ToString();
                                var jsonError12 = JsonConvert.SerializeObject(Msg1);
                                return jsonError12;
                            }
                        }
                        else
                        {
                            string Msg1 = ds.Tables["TicketFailureInfo"].Rows[0]["Message"].ToString();
                            var jsonError12 = JsonConvert.SerializeObject(Msg1);
                            return jsonError12;
                        }


                            //Log

                            //XmlDocument xml1 = new XmlDocument();
                            //xml1.LoadXml(doc.ToString());
                            //XmlReader xmlReader1 = new XmlNodeReader(xml1);

                            //XmlTextWriter writer = new XmlTextWriter(@"C:\TravelportLog\AirTicketingReq.xml", null);
                            //writer.Formatting = Formatting.Indented;
                            //xml1.Save(writer);

                            //XmlTextWriter writer1 = new XmlTextWriter(@"C:\TravelportLog\AirTicketingRsp.xml", null);
                            //writer1.Formatting = Formatting.Indented;
                            //xml.Save(writer1);

                            // Commnent above 9 lines before publishing



                            //Booking Dataset

                        }


                        //ViewBag.TotalTax = totalTaxes;

                        //ViewBag.TotalAmount = baseFare + totalTaxes;

                        //ViewBag.Agent = content.ReceivedFrom.Name;

                        //ViewBag.PNR = PNR.ToUpper();

                        //ViewBag.PSF = PSF;



                        //Fetch(PNR);
                    }
                else
                {
                   return JsonConvert.SerializeObject("Invalid PNR");
                }

                return JsonConvert.SerializeObject("Invalid PNR");
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex.Message.ToString());
            }
        }

        [NonAction]
        public DataSet AirRetrieveDocumentReq(string PNR)
        {
            StringBuilder strReq = new StringBuilder();
            Guid TraceId = Guid.NewGuid();

            //string APIKey = obj.AirPricingInfoKeys.Where(a => a.PNR == PNR).Select(a => a.airPricingInfoKey).FirstOrDefault();
            //string PNRForTicket = obj.AirPricingInfoKeys.Where(a => a.PNR == PNR).Select(a => a.PnrForTicket).FirstOrDefault();

            try
            {
                if (PNR != "" || PNR != null)
                {
                    strReq.Append(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>");
                    strReq.Append("<soapenv:Header/>");
                    strReq.Append("<soapenv:Body>");
                    strReq.Append("<AirRetrieveDocumentReq xmlns='http://www.travelport.com/schema/air_v51_0' xmlns:com='http://www.travelport.com/schema/common_v51_0' ProviderCode='1G' ReturnPricing='true' TargetBranch ='P3393195' AuthorizedBy='Travelport' >");
                    strReq.Append("<com:BillingPointOfSaleInfo OriginApplication = 'UAPI' />");
                    strReq.Append("<AirReservationLocatorCode>" + PNR + "</AirReservationLocatorCode>");
                    strReq.Append("</AirRetrieveDocumentReq>");
                    strReq.Append("</soapenv:Body>");
                    strReq.Append("</soapenv:Envelope>");
                    var doc = XDocument.Parse(strReq.ToString());

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    //string url = "https://apac.universal-api.pp.travelport.com/B2BGateway/connect/uAPI/AirService";
                    string url = "https://emea.universal-api.travelport.com/B2BGateway/connect/uAPI/AirService";
                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
                    byte[] bytes;
                    bytes = System.Text.Encoding.ASCII.GetBytes(doc.ToString());
                    //request1.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz")));
                    request1.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                    request1.ContentType = "text/xml; encoding='utf-8'";
                    request1.Method = "Post";
                    request1.Headers.Add("SOAPAction", "");
                    request1.Host = "twsprofiler.travelport.com";
                    request1.KeepAlive = true;

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    Stream requestStream1 = request1.GetRequestStream();
                    requestStream1.Write(bytes, 0, bytes.Length);
                    requestStream1.Close();
                    HttpWebResponse response1;
                    response1 = (HttpWebResponse)request1.GetResponse();
                    Stream responseStream;

                    TextReader responseStr;
                    // DataTable FlightSegments = null;
                    DataSet ds1 = new DataSet();

                    if (response1.StatusCode == HttpStatusCode.OK)
                    {
                        responseStream = response1.GetResponseStream();
                        responseStr = new StreamReader(response1.GetResponseStream(), System.Text.Encoding.Default);

                        var responseReader = new StreamReader(responseStream);
                        var result1 = responseReader.ReadToEnd();
                        //return result;

                        result1 = result1.Replace("universal:", "");
                        result1 = result1.Replace("common_v51_0:", "");
                        result1 = result1.Replace("air:", "");



                        XDocument xdoc = XDocument.Parse(result1);
                        int textCounter = 1;
                        foreach (var textElement in xdoc.Descendants("Text").ToList())
                        {
                            textElement.Name = "Text" + textCounter;
                            textCounter++;
                        }

                        string modifiedXmlString = xdoc.ToString();

                        XmlDocument xml1 = new XmlDocument();
                        xml1.LoadXml(modifiedXmlString);
                        XmlReader xmlReader1 = new XmlNodeReader(xml1);
                        XmlReader reader = doc.CreateReader();


                        ds1.ReadXml(xmlReader1);



                        if (ds1.Tables.Count > 0)
                        {
                            return ds1;
                        }


                    }
                    return null;
                }
                else
                {
                    return null;
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [NonAction]
        public void FetchIssuedTicketDetails(string PNR)
        {
            //try
            //{
            //    var chkDetails = from s in obj.Reservations where s.PNR == PNR select s;

            //    if (chkDetails.Count() != 0)
            //    {
            //        var Details = (from s in obj.Reservations
            //                       where s.PNR == PNR
            //                       select new
            //                       {
            //                           s.PayableAmount,
            //                           s.PSFAmount,
            //                           s.ReceivableAmount,
            //                           s.ReceivableDate,
            //                           s.Supplier,
            //                           s.SupplierPayableDate,
            //                           s.TicketIssueDate,
            //                           s.TransactionType,
            //                           s.Customer
            //                       }).FirstOrDefault();

            //        //ViewBag.gSupplier = Details.Supplier;
            //        //ViewBag.gCustomer = Details.Customer;
            //        //ViewBag.gPayableAmount = Details.PayableAmount;
            //        //ViewBag.gPSFAmount = Details.PSFAmount;
            //        //ViewBag.gReceivableAmount = Details.ReceivableAmount;
            //        //ViewBag.gReceivableDate = Details.ReceivableDate;
            //        //ViewBag.gSupplierPayableDate = Details.SupplierPayableDate;
            //        //ViewBag.gTicketIssueDate = Details.TicketIssueDate;
            //        //ViewBag.gTransactionType = Details.TransactionType;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //ViewBag.Msg = ex.Message;
            //}
        }

    }

    public class Pessengers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string passengerType { get; set; }
        public string Gender { get; set; }
        public string TicketNo { get; set; }

    }

    public class BookingDetailsModel
    {
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Infants { get; set; }
        public decimal? TotalFare { get; set; }
        public decimal? TotalTaxes { get; set; }
        public decimal? GrandTotal { get; set; }
    }

    public class FlightSegmentModel
    {
        public int? OriginID { get; set; }
        public int? DestinationID { get; set; }
        public int? FlightSegmentID { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string ClassName { get; set; }
        public string ResBookDesigCode { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public int? FlightBookingID { get; set; }
        public string OperatingAirlineCode { get; set; }
        public string FlightNumber { get; set; }
        public string AirlineName { get; set; }
    }

}
