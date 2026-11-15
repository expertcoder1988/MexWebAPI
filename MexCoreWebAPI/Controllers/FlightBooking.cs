using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using MexCoreWebAPI.Model;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

using System.Security.Cryptography;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using iTextSharp.text.pdf.parser.clipper;
using iTextSharp.text.pdf.qrcode;
using MalikExpressMVC.Models.XML;
using FlightSegment = MexCoreWebAPI.Model.FlightSegment;
using iTextSharp.tool.xml.html.head;
using System.Data.SqlTypes;

using MalikExpressMVC.Models.XML.PNR;

namespace MexCoreWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]

    public class FlightBooking : ControllerBase
    {
        private readonly MexDbContext sql;
        private readonly MexOldDBContext oldSQL;

        public FlightBooking(MexDbContext context, MexOldDBContext OldSQL)
        {
            sql = context;
            oldSQL = OldSQL;
        }

        [HttpPost("Booking")]

        public string Booking([FromBody] BookingFLight[] arr)
        {
            Task<Rootobject> Json = GalBooking(arr);
            // Rootobject res = (JObject) Json.Result; // Json.Result;
            Rootobject rb = Json.Result; // res.ToObject<Rootobject>();
            return rb.AirPriceRsp[0].TransactionId; //.AirPriceRsp[0].TransactionId.ToString();
        }

        [NonAction]
        public async Task<Rootobject> GalBooking(BookingFLight[] arr)
        {
            string Adults = arr[0].Adults.ToString(); string Children = arr[0].Children.ToString(); string Infants = arr[0].Infants.ToString(); string Supplier = arr[0].Carrier;
            string BookingID = arr[0].SegmentKey;
            string type = "OneWay"; string ReturnBookingID = "";

            string from = arr[0].Origin; string to = arr[0].Destination;
            string start = arr[0].DepartureTime.ToString(); string end = arr[0].ArrivalTime.ToString();
            string FlightNumber = arr[0].FlightNumber;
            string FlightNumber1 = "";
            string totaltime0 = arr[0].FlightTime;
            string stops0 = arr[0].Stops;

            string Childamount0 = arr[0].ChildBasePrice; string Childtaxes0 = arr[0].ChildTaxes; string Childtotal0 = arr[0].ChildTotalPrice;
            string Adultamount0 = arr[0].AdultBasePrice; string Adulttaxes0 = arr[0].AdultTaxes; string Adulttotal0 = arr[0].AdultTotalPrice;
            string Infantamount0 = arr[0].InfantBasePrice; string Infanttaxes0 = arr[0].InfantTaxes; string Infanttotal0 = arr[0].InfantTotalPrice;

            string ReturnAdultTotalPrice1 = ""; string serviceclass0 = "";
            string ReturnAdultBasePrice1 = ""; string ReturnAdultTaxes1 = ""; string ReturnChildTotalPrice1 = ""; string ReturnChildBasePrice1 = "";
            string ReturnChildTaxes1 = ""; string ReturnInfantTotalPrice1 = ""; string ReturnInfantBasePrice1 = ""; string ReturnInfantTaxes1 = "";

            string BookingID2 = "-1"; string BookingID3 = "-1"; string BookingID4 = "-1"; string BookingID5 = "-1"; string BookingID6 = "-1";
            string SegmentRef1 = ""; string SegmentRef2 = ""; string SegmentRef3 = ""; string SegmentRef4 = ""; string SegmentRef5 = "";
            string SegmentRef6 = "";

            try
            {
                bool isLetter = char.IsLetter(Adultamount0, 0);

                if (isLetter == true)
                {
                    Adultamount0 = Adultamount0.Remove(0, 3).Trim();
                }

                isLetter = char.IsLetter(Childamount0, 0);
                if (isLetter == true)
                {
                    Childamount0 = Childamount0.Remove(0, 3).Trim();
                }

                isLetter = char.IsLetter(Infantamount0, 0);
                if (isLetter == true)
                {
                    Infantamount0 = Infantamount0.Remove(0, 3).Trim();
                }

                isLetter = char.IsLetter(Adulttotal0, 0);
                if (isLetter == true)
                {
                    Adulttotal0 = Adulttotal0.Remove(0, 3).Trim();
                }

                isLetter = char.IsLetter(Childtotal0, 0);
                if (isLetter == true)
                {
                    Childtotal0 = Childtotal0.Remove(0, 3).Trim();
                }

                isLetter = char.IsLetter(Infanttotal0, 0);
                if (isLetter == true)
                {
                    Infanttotal0 = Infanttotal0.Remove(0, 3).Trim();
                }

                isLetter = char.IsLetter(Adulttaxes0, 0);
                if (isLetter == true)
                {
                    Adulttaxes0 = Adulttaxes0.Remove(0, 3).Trim();
                }

                isLetter = char.IsLetter(Childtaxes0, 0);
                if (isLetter == true)
                {
                    Childtaxes0 = Childtaxes0.Remove(0, 3).Trim();
                }

                isLetter = char.IsLetter(Infanttaxes0, 0);
                if (isLetter == true)
                {
                    Infanttaxes0 = Infanttaxes0.Remove(0, 3).Trim();
                }

                if (Adulttotal0.IndexOf(' ') > -1)
                {
                    Adulttotal0 = Adulttotal0.Remove(0, 4);
                }
                if (Adultamount0.IndexOf(' ') > -1)
                {
                    Adultamount0 = Adultamount0.Remove(0, 4);
                }
                if (Adulttaxes0.IndexOf(' ') > -1)
                {
                    Adulttaxes0 = Adulttaxes0.Remove(0, 4);
                }
                if (Childtotal0.IndexOf(' ') > -1)
                {
                    Childtotal0 = Childtotal0.Remove(0, 4);
                }
                if (Childamount0.IndexOf(' ') > -1)
                {
                    Childamount0 = Childamount0.Remove(0, 4);
                }
                if (Childtaxes0.IndexOf(' ') > -1)
                {
                    Childtaxes0 = Childtaxes0.Remove(0, 4);
                }
                if (Infanttotal0.IndexOf(' ') > -1)
                {
                    Infanttotal0 = Infanttotal0.Remove(0, 4);
                }
                if (Infantamount0.IndexOf(' ') > -1)
                {
                    Infantamount0 = Infantamount0.Remove(0, 4);
                }
                if (Infanttaxes0.IndexOf(' ') > -1)
                {
                    Infanttaxes0 = Infanttaxes0.Remove(0, 4);
                }

                int _Adulttotal0 = Convert.ToInt32(Adulttotal0);
                int _Adultamount0 = Convert.ToInt32(Adultamount0);
                int _Adulttaxes0 = Convert.ToInt32(Adulttaxes0);
                int _Childtotal0 = Convert.ToInt32(Childtotal0);
                int _Childamount0 = Convert.ToInt32(Childamount0);
                int _Childtaxes0 = Convert.ToInt32(Childtaxes0);
                int _Infanttotal0 = Convert.ToInt32(Infanttotal0);
                int _Infantamount0 = Convert.ToInt32(Infantamount0);
                int _Infanttaxes0 = Convert.ToInt32(Infanttaxes0);

                ArrayList Details = new ArrayList();
                int total0 = 0;

                //if (Session["amount"] != null)
                //{
                //    total0 = Convert.ToInt32(Session["amount"].ToString());
                //}

                int idx = 0;

                int Fare = 0;

                Fare = _Adultamount0 + _Childamount0 + _Infantamount0; //Convert.ToInt32(total0.Remove(idx, 3));


                int taxes = _Adulttaxes0 + _Childtaxes0 + _Infanttaxes0;

                int taxes0 = 0;

                //if (Session["taxes0"] != null)
                //{
                //    taxes0 = Convert.ToInt32(Session["taxes0"].ToString());
                //}

                //string amount0 = "0";

                //if (Session["amount0"] != null)
                //{
                //    amount0 = Session["amount0"].ToString();
                //}


                //amount0 = Fare.ToString();

                //string[] tx;

                //if (taxes0.IndexOf(",") > -1)
                //{
                //    tx = taxes0.Split(',');
                //    taxes0 = tx[0].ToString();
                //}

                Fare = Fare - Convert.ToInt32(taxes0);

                if (type == "OneWay")
                {
                    start = end;
                }

                //  //ViewBag.Details = Details;

                // end = Session["end"].ToString();
                // start = Session["start"].ToString();

                string[] StartDate = start.Split('-');
                string[] EndDate = end.Split('-');

                //to = Session["to"].ToString();
                //from = Session["from"].ToString();

                //idx = from.IndexOf("(");
                //from = from.Substring(idx + 1, 3);

                //idx = to.IndexOf("(");
                //to = to.Substring(idx + 1, 3);

                string[] ad;

                if (Adults.IndexOf(",") > -1)
                {
                    ad = Adults.Split(',');
                    Adults = ad[0].ToString();
                }

                if (Children.IndexOf(",") > -1)
                {
                    ad = Children.Split(',');
                    Children = ad[0].ToString();
                }

                if (Infants.IndexOf(",") > -1)
                {
                    ad = Infants.Split(',');
                    Infants = ad[0].ToString();
                }

                if (totaltime0.IndexOf(",") > -1)
                {
                    ad = totaltime0.Split(',');
                    totaltime0 = ad[0].ToString();
                }

                Details.Add(Supplier);
                Details.Add(Adults);
                Details.Add(Children);
                Details.Add(Infants);
                Details.Add(from);
                Details.Add(to);
                Details.Add(start);
                Details.Add(end);
                Details.Add(serviceclass0);
                Details.Add(stops0);
                Details.Add(Fare);
                // Details.Add(amount0);
                Details.Add(taxes);
                Details.Add(FlightNumber);
                Details.Add(totaltime0);

                {

                    var JSONString = AirPriceReq(arr, type, Convert.ToInt32(Adults), Convert.ToInt32(Children), Convert.ToInt32(Infants), BookingID, ReturnBookingID, BookingID2, BookingID3, BookingID4, BookingID5, BookingID6, SegmentRef1,
                        SegmentRef2, SegmentRef3, SegmentRef4, SegmentRef5, SegmentRef6);

                    // Rootobject rb = (Rootobject)JsonConvert.DeserializeObject(JSONString);

                    JObject keyValuePairs = JObject.Parse(JSONString);
                    //JObject res = (JObject)Json.Result; // Json.Result;
                    Rootobject rb = keyValuePairs.ToObject<Rootobject>();

                    string airPricingSolution = HttpContext.Session.GetString("AirPricingSolution");

                    airPricingSolution = airPricingSolution.Remove(0, 1);
                    airPricingSolution = airPricingSolution.Remove(airPricingSolution.Length - 1, 1);
                    airPricingSolution = airPricingSolution.Replace("\\", "");
                    //airPricingSolution = airPricingSolution.Replace("common_v42_0:", "");

                    //airPricingSolution = airPricingSolution.Insert(0, "<root>");
                    //airPricingSolution = airPricingSolution.Insert(airPricingSolution.Length, "</root>");

                    //XmlDocument xmlDoc = new XmlDocument();
                    //xmlDoc.LoadXml(airPricingSolution);

                    //DataSet dataSet = new DataSet();

                    //using (StringReader stringReader = new StringReader(xmlDoc.OuterXml))
                    //{
                    //    // Read the XML data into the DataSet
                    //    dataSet.ReadXml(stringReader);
                    //}

                    if (airPricingSolution != null)
                    {


                        //XElement xml = XElement.Parse(airPricingSolution);
                        //string str = xml.ToString();

                        byte[] APS = Encoding.UTF8.GetBytes(airPricingSolution);

                        byte[] APR = ObjectToByteArray(rb); //Encoding.UTF8.GetBytes(rb);


                        using (sql)
                        {

                            AirPriceSol aps = new AirPriceSol();
                            aps.TransactionId = rb.AirPriceRsp[0].TransactionId;
                            aps.Response = APS;
                            sql.AirPriceSols.Add(aps);

                            sql.SaveChanges();

                            AirPriceRsp apr = new AirPriceRsp();
                            apr.TransactionId = rb.AirPriceRsp[0].TransactionId;
                            apr.Response = APR;
                            sql.AirPriceRsps.Add(apr);

                            sql.SaveChanges();
                        }


                    }

                    return rb;

                    if (null == null)
                    {
                        if (type == "OneWay")
                        {

                        }
                        else if (type == "Return")
                        {

                        }

                        return null;
                    }
                    //else
                    //{
                    //    if (Session["warning"] != null || Session["warning"].ToString() != "")
                    //    {
                    //        //ViewBag.Message = Session["warning"].ToString();
                    //        return View();
                    //    }
                    //}

                }
                //else if (Session["warning"] != null || Session["warning"].ToString() != "")
                //{
                //    //ViewBag.Message = Session["warning"].ToString();
                //    return View();
                //}

                return null; // "something went wrong";
            }
            catch (Exception ex)
            {
                //  //ViewBag.Error = "Something went wrong. Please try again.";
                return null;
            }
        }

        [NonAction]
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            string json = JsonConvert.SerializeObject(obj);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        [NonAction]
        private string AirPriceReq(BookingFLight[] dt, string tripType, int adults, int children, int infants, string BookingID, string ReturnBookingID,
     string BookingID2 = "-1", string BookingID3 = "-1", string BookingID4 = "-1", string BookingID5 = "-1", string BookingID6 = "-1",
     string SegmentRef1 = "", string SegmentRef2 = "", string SegmentRef3 = "", string SegmentRef4 = "", string SegmentRef5 = "",
      string SegmentRef6 = "")
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                // string url = "https://apac.universal-api.pp.travelport.com/B2BGateway/connect/uAPI/AirService";
                string url = "https://emea.universal-api.travelport.com/B2BGateway/connect/uAPI/AirService";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(AirPriceReqXML(dt, tripType, adults, children, infants, BookingID, ReturnBookingID, BookingID2, BookingID3, BookingID4, BookingID5, BookingID6, SegmentRef1,
                    SegmentRef2, SegmentRef3, SegmentRef4, SegmentRef5, SegmentRef6));
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz")));
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                request.ContentType = "text/xml; encoding='utf-8'";
                request.Method = "Post";
                request.Headers.Add("SOAPAction", "");
                request.Host = "twsprofiler.travelport.com";
                request.KeepAlive = true;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream;
                Stream responseStream1;
                TextReader responseStr;
                DataTable FlightSegments = null;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    responseStream = response.GetResponseStream();

                    responseStr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                    var responseReader = new StreamReader(responseStream);
                    var result = responseReader.ReadToEnd();

                    //TextReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                    //XmlSerializer xmlSer = new XmlSerializer(typeof(MalikExpressMVC.Models.Gal.Envelope));
                    //MalikExpressMVC.Models.Gal.Envelope obj = (MalikExpressMVC.Models.Gal.Envelope)xmlSer.Deserialize(respStream);
                    //MalikExpressMVC.Models.Gal.EnvelopeBody rsp = obj.Body;

                    result = result.Replace("air:", "");


                    var doc = XDocument.Parse(result);

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(result);

                    //Log

                    XmlDocument xml1 = new XmlDocument();


                    xml1.LoadXml(AirPriceReqXML(dt, tripType, adults, children, infants, BookingID, ReturnBookingID, BookingID2, BookingID3, BookingID4, BookingID5, BookingID6, SegmentRef1,
                      SegmentRef2, SegmentRef3, SegmentRef4, SegmentRef5, SegmentRef6));

                    XmlReader xmlReader1 = new XmlNodeReader(xml1);

                    string formattedDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    XmlTextWriter writer = new XmlTextWriter(@"C:\TravelportLog\AirPriceReq_" + formattedDate + ".xml", null);
                    writer.Formatting = System.Xml.Formatting.Indented;
                    xml1.Save(writer);

                    XmlTextWriter writer1 = new XmlTextWriter(@"C:\TravelportLog\AirPriceRsp_" + formattedDate + ".xml", null);
                    writer1.Formatting = System.Xml.Formatting.Indented;
                    xml.Save(writer1);

                    // Commnent above 9 lines before publishing

                    int f = 0;
                    foreach (var element in doc.Descendants())
                    {
                        if (element.Name.LocalName.Equals("Text"))
                        {
                            element.Name = "Text" + f.ToString();
                        }
                        f++;
                    }

                    //XmlNode y = xml.SelectSingleNode("AirSegmentRef");
                    //xml.DocumentElement.RemoveChild(y);

                    string AirPriceResult = "";

                    if (xml.GetElementsByTagName("AirPricingSolution")[0] != null)
                    {
                        AirPriceResult = xml.GetElementsByTagName("AirPricingSolution")[0].InnerXml;

                        int idx4 = AirPriceResult.IndexOf("<AirPricingInfo");

                        AirPriceResult = AirPriceResult.Remove(0, idx4);
                    }

                    // int idx = result.IndexOf("<AirItinerary>");
                    //int idx1 = result.IndexOf("</AirItinerary>");

                    string input = result;

                    Regex regex1 = new Regex("<AirItinerary>(.*)</AirItinerary>");
                    var result2 = regex1.Match(input);
                    string AirSegments = result2.Groups[1].ToString();

                    int idx = result.IndexOf("<AirSegmentRef ");

                    if (idx > -1)
                    {
                        input = result.Insert(idx, AirSegments);
                    }
                    Regex regex = new Regex("<AirPriceResult>(.*)</AirPriceResult>");
                    var result1 = regex.Match(input);
                    String AirPricingSolution = result1.Groups[1].ToString();

                    Regex regex11 = new Regex("<OptionalServices>(.*)</OptionalServices>");
                    var result11 = regex.Match(input);
                    String AirPricingSolution1 = result1.Groups[1].ToString();

                    Regex regex21 = new Regex("<FareInfo(.*)</FareInfo>");
                    var result21 = regex21.Match(input);
                    String FareRuleKey = result21.Groups[1].ToString();

                    AirPricingSolution = AirPriceResult; // AirPricingSolution.Replace("common_v42_0:", "");
                    AirPricingSolution = AirPricingSolution.Replace("<AirPricingSolution", "<AirPricingSolution xmlns='http://www.travelport.com/schema/air_v42_0' ");
                    AirPricingSolution = AirPricingSolution.Replace("<HostToken", "<HostToken xmlns='http://www.travelport.com/schema/common_v42_0' ");
                    if (AirPricingSolution1.Length > 0)
                    {
                        AirPricingSolution = AirPricingSolution.Replace(AirPricingSolution1, "");
                    }

                    AirPricingSolution = AirPricingSolution.Replace(":common_v42_0", "");
                    //Session["AirPricingSolution"] = AirPricingSolution.Replace("common_v42_0:", "");



                    // AirBookReqXML();

                    XmlReader xmlReader = new XmlNodeReader(xml);
                    XmlReader reader = doc.CreateReader();

                    DataSet ds = new DataSet();

                    ds.ReadXml(reader);

                    HttpContext.Session.SetString("AirPricingSolution", JsonConvert.SerializeObject(AirPricingSolution));

                    var JSONString = JsonConvert.SerializeObject(ds);
                    return JSONString;

                    //Session["AirPriceRsp"] = ds;

                    //if (ds.Tables.Contains("Fault"))
                    //{
                    //    Session["warning"] = ds.Tables["Fault"].Rows[0]["faultstring"].ToString();
                    //}
                    //else
                    //{
                    //    Session["warning"] = null;
                    //}
                }
                return "something went wrong";
            }
            catch (Exception ex)
            {
                //  //ViewBag.Message = ex.Message;
                return "something went wrong";
            }
        }

        [NonAction]
        private string AirPriceReqXML(BookingFLight[] dt, string tripType, int adults, int children, int infants, string BookingID, string ReturnBookingID,
              string BookingID2 = "-1", string BookingID3 = "-1", string BookingID4 = "-1", string BookingID5 = "-1", string BookingID6 = "-1",
              string SegmentRef1 = "", string SegmentRef2 = "", string SegmentRef3 = "", string SegmentRef4 = "", string SegmentRef5 = "",
               string SegmentRef6 = "")
        {
            StringBuilder strReq = new StringBuilder();
            Guid TraceId = Guid.NewGuid();
            string PlatingCarrier = "";
            string PlatingCarrier1 = "";

            string Origin = dt[0].Origin;
            string Destination = dt[0].Destination;


            DataTable dt1 = new DataTable();

            try
            {
                PlatingCarrier = dt[0].Carrier; // dt.AsEnumerable().Where(p => p.Field<int>("Option_Id") == Convert.ToInt32(BookingID)).Select(p => p.Field<string>("Carrier")).FirstOrDefault();

                strReq.Append(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>");
                strReq.Append("<soapenv:Header/>");
                strReq.Append("<soapenv:Body>");
                //strReq.Append("<AirPriceReq xmlns='http://www.travelport.com/schema/air_v42_0' FareRuleType='long' TraceId='" + TraceId + "' TargetBranch='P7113657' CheckFlightDetails='true' >");
                strReq.Append("<AirPriceReq xmlns='http://www.travelport.com/schema/air_v42_0' FareRuleType='long' TraceId='" + TraceId + "' TargetBranch='P3393195' CheckFlightDetails='true' RetainReservation='Both'>");
                strReq.Append("<BillingPointOfSaleInfo xmlns='http://www.travelport.com/schema/common_v42_0' OriginApplication='uAPI'/>");
                //strReq.Append("<FareRuleType xmlns='http://www.travelport.com/schema/common_v42_0' FareRuleType='long' Provider='1G' />");
                strReq.Append("<AirItinerary>");
                foreach (var itm in dt)
                {
                    strReq.Append("<AirSegment Key='" + itm.SegmentKey + "' AvailabilitySource='" + itm.AvailabilitySource + "' Equipment='" + itm.Equipment + "' AvailabilityDisplayType='" + itm.AvailabilityDisplayType + "' Group='" + itm.Group + "' Carrier='" + itm.Carrier + "' FlightNumber='" + itm.FlightNumber + "' Origin='" + itm.Origin + "' Destination='" + itm.Destination + "' DepartureTime='" + Convert.ToDateTime(itm.DepartureTime).ToUniversalTime().ToString("u").Replace(" ", "T") + "' ArrivalTime='" + Convert.ToDateTime(itm.ArrivalTime).ToUniversalTime().ToString("u").Replace(" ", "T") + "' FlightTime='" + itm.FlightTime + "' Distance='" + itm.Distance + "' ProviderCode='" + itm.ProviderCode + "' ClassOfService='" + itm.CabinClass + "' >");

                    //if (null == null)
                    //{
                    //    if (itm.Destination != Destination)
                    //    {
                    //        strReq.Append("<Connection/>");
                    //    }
                    //}
                    strReq.Append("</AirSegment>");
                }
                //strReq.Append("</AirItinerary>");


                if (tripType == "Return")
                {


                    PlatingCarrier1 = dt[0].Carrier; // dt.AsEnumerable().Where(p => p.Field<int>("Option_Id") == Convert.ToInt32(ReturnBookingID)).Select(p => p.Field<string>("Carrier")).FirstOrDefault();

                    //strReq.Append("<AirItinerary>");
                    foreach (var itm in dt)
                    {
                        strReq.Append("<AirSegment Key='" + itm.SegmentKey + "' AvailabilitySource='" + itm.AvailabilitySource + "' Equipment='" + itm.Equipment + "' AvailabilityDisplayType='" + itm.AvailabilityDisplayType + "' Group='" + itm.Group + "' Carrier='" + itm.Carrier + "' FlightNumber='" + itm.FlightNumber + "' Origin='" + itm.Origin + "' Destination='" + itm.Destination + "' DepartureTime='" + Convert.ToDateTime(itm.DepartureTime).ToUniversalTime().ToString("u").Replace(" ", "T") + "' ArrivalTime='" + Convert.ToDateTime(itm.ArrivalTime).ToUniversalTime().ToString("u").Replace(" ", "T") + "' FlightTime='" + itm.FlightTime + "' Distance='" + itm.Distance + "' ProviderCode='" + itm.ProviderCode + "' ClassOfService='" + itm.CabinClass + "' >");


                        if (null == null)
                        {
                            if (itm.Destination != Destination)
                            {
                                strReq.Append("<Connection/>");
                            }
                        }
                        strReq.Append("</AirSegment>");
                    }


                }


                strReq.Append("</AirItinerary>");

                strReq.Append("<AirPricingModifiers FaresIndicator='PublicFaresOnly'>");
                //strReq.Append("<AirPricingModifiers FaresIndicator='PublicFaresOnly' InventoryRequestType='DirectAccess'>");
                // strReq.Append("<BrandModifiers ModifierType='BasicDetailsOnly'/>");
                strReq.Append("</AirPricingModifiers>");

                if (adults > 0)
                {
                    for (int c = 1; c <= adults; c++)
                    {
                        strReq.Append("<SearchPassenger xmlns='http://www.travelport.com/schema/common_v42_0'  BookingTravelerRef='abcf234" + c + "' Code='ADT'/>");
                    }
                }


                if (children > 0)
                {
                    for (int c = 1; c <= children; c++)
                    {
                        strReq.Append("<SearchPassenger xmlns='http://www.travelport.com/schema/common_v42_0' BookingTravelerRef='cbcf234" + c + "' Code='CNN' Age='10' DOB='2009-01-24'/>");
                    }
                }

                if (infants > 0)
                {
                    for (int c = 1; c <= infants; c++)
                    {
                        strReq.Append("<SearchPassenger xmlns='http://www.travelport.com/schema/common_v42_0' BookingTravelerRef='ibcf234" + c + "' Code='INF' Age='1' DOB='2009-01-24'/>");
                    }
                }

                strReq.Append("<AirPricingCommand>");

                foreach (var itm in dt)
                {
                    strReq.Append("<AirSegmentPricingModifiers AirSegmentRef='" + itm.SegmentKey + "' FareBasisCode='" + itm.FareBasis + "'>");
                    strReq.Append("<PermittedBookingCodes>");
                    strReq.Append("<BookingCode Code = '" + itm.BookingCode + "'/>");
                    strReq.Append("</PermittedBookingCodes>");
                    strReq.Append(" </AirSegmentPricingModifiers>");
                }

                if (tripType == "Return")
                {
                    //var FlightInfo1 = from d in dt.AsEnumerable()
                    //                  where d.Field<int>("option_id") == Convert.ToInt32(ReturnBookingID)
                    //                  select new
                    //                  {
                    //                      key = d.Field<string>("segmentKey"),
                    //                      ClassOfService = d.Field<string>("BookingCode"),
                    //                      FareBasisCode = d.Field<string>("FareBasis")
                    //                  };

                    //strReq.Append("<AirItinerary>");
                    foreach (var itm in dt)
                    {
                        strReq.Append("<AirSegmentPricingModifiers AirSegmentRef='" + itm.SegmentKey + "' FareBasisCode='" + itm.FareBasis + "'>");
                        strReq.Append("<PermittedBookingCodes>");
                        strReq.Append("<BookingCode Code = '" + itm.BookingCode + "'/>");
                        strReq.Append("</PermittedBookingCodes>");
                        strReq.Append(" </AirSegmentPricingModifiers>");
                    }
                }

                strReq.Append("</AirPricingCommand>");
                //strReq.Append("<AirPricingCommand/>");
                strReq.Append("</AirPriceReq>");
                strReq.Append("</soapenv:Body>");
                strReq.Append("</soapenv:Envelope>");



                return strReq.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        [NonAction]
        static DataSet ConvertXmlStringToDataSet(string xmlString)
        {
            DataSet dataSet = new DataSet();

            using (StringReader stringReader = new StringReader(xmlString))
            {
                dataSet.ReadXml(stringReader);
            }

            return dataSet;
        }

        [NonAction]
        public static T ByteArrayToObject<T>(byte[] byteArray)
        {
            if (byteArray == null)
                return default(T);

            string json = System.Text.Encoding.UTF8.GetString(byteArray);
            return JsonConvert.DeserializeObject<T>(json);
        }

        [NonAction]
        public static string ObjectToXml(object obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }

        [NonAction]
        public DataSet XmlToDataSet(string xmlString)
        {
            DataSet dataSet = new DataSet();

            try
            {
                using (System.IO.StringReader stringReader = new System.IO.StringReader(xmlString))
                {
                    dataSet.ReadXml(stringReader);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during XML parsing
                Console.WriteLine("Error: " + ex.Message);
            }

            return dataSet;
        }

        [HttpPost("SaveBooking")]
        public string SaveBooking([FromBody] dynamic info)
        {
            // string res = Convert.ToString(airPriceRes);
            //res = res.Remove(0, 21);
            // XmlDocument doc = JsonConvert.DeserializeXmlNode(res,"body"); ;
            //  return null;


            string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
            var data = JsonConvert.DeserializeObject<dynamic>(json.ToString());


            try
            {
                string firstName = string.IsNullOrEmpty(data?.firstName_0?.Value) ? "" : data?.firstName_0?.Value;
                string lastName = string.IsNullOrEmpty(data?.lastName_0?.Value) ? "" : data?.lastName_0?.Value;
                string fullname = firstName + " " + lastName;
                string Doc = string.IsNullOrEmpty(data?.document_0?.Value) ? "" : data?.document_0?.Value;
                string email = string.IsNullOrEmpty(data?.email_0?.Value) ? "" : data?.email_0?.Value;
                string contact = string.IsNullOrEmpty(data?.MobileNO_0?.Value) ? "" : data?.MobileNO_0?.Value;
                string bookingTitle = string.IsNullOrEmpty(data?.bookingTitle?.Value) ? "" : data?.bookingTitle?.Value;
                string Supplier = string.IsNullOrEmpty(data?.Supplier?.Value) ? "" : data?.Supplier_0?.Value;
                int Adults = data?.Adults != null ? Convert.ToInt32(data?.Adults?.Value) : 1;
                int Children = data?.Children != null ? Convert.ToInt32(data?.Children?.Value) : 0;
                int Infants = data?.Infants != null ? Convert.ToInt32(data?.Infants?.Value) : 0;
                int? BookingID = 0; // string.IsNullOrEmpty(data?.BookingID) ? "" : data?.BookingID;
                string type = string.IsNullOrEmpty(data?.type?.Value) ? "" : data?.type?.Value;
                string count = string.IsNullOrEmpty(data?.count?.Value) ? "" : data?.count?.Value;
                string from = string.IsNullOrEmpty(data?.from?.Value) ? "" : data?.from?.Value;
                string to = string.IsNullOrEmpty(data?.to?.Value) ? "" : data?.to?.Value;
                string start = string.IsNullOrEmpty(data?.start?.ToString()) ? "" : data?.start?.ToString();
                string end = string.IsNullOrEmpty(data?.end?.ToString()) ? "" : data?.end?.ToString();
                string FlightNumber = string.IsNullOrEmpty(data?.FlightNumber?.Value) ? "" : data?.FlightNumber?.Value;
                string serviceclass0 = string.IsNullOrEmpty(data?.serviceclass0?.Value) ? "" : data?.serviceclass0?.Value;
                string stops0 = string.IsNullOrEmpty(data?.stops0?.Value) ? "" : data?.stops0?.Value;
                string total0 = string.IsNullOrEmpty(data?.total0?.Value) ? "" : data?.total0?.Value;
                string amount0 = string.IsNullOrEmpty(data?.amount0?.Value) ? "" : data?.amount0?.Value;
                string taxes0 = string.IsNullOrEmpty(data?.taxes0?.Value) ? "" : data?.taxes0?.Value;
                string currency0 = string.IsNullOrEmpty(data?.currency0?.Value) ? "" : data?.currency0?.Value;
                string totaltime0 = string.IsNullOrEmpty(data?.totaltime0?.Value) ? "" : data?.totaltime0?.Value;
                string totaltime1 = string.IsNullOrEmpty(data?.totaltime0?.Value) ? "" : data?.totaltime1?.Value;
                string stops1 = string.IsNullOrEmpty(data?.totaltime0?.Value) ? "" : data?.stops1?.Value;
                string primaryPhone = string.IsNullOrEmpty(data?.MobileNO_0?.Value) ? "" : data?.MobileNO_0?.Value;
                string secondaryPhone = string.IsNullOrWhiteSpace(data?.LandlineNo_0?.Value) ? "" : data?.LandlineNo_0?.Value;
                string transactionId = string.IsNullOrWhiteSpace(data?.TransactionId?.Value) ? "" : data?.TransactionId?.Value;  //info.transactionId;

                decimal AdultTotalPrice = 0;
                decimal AdultBasePrice = data?.AdultBasePrice != null ? Convert.ToDecimal(data?.AdultBasePrice?.Value) : 0;
                decimal AdultTaxes = data?.AdultTaxes != null ? Convert.ToDecimal(data?.AdultTaxes?.Value) : 0;
                decimal ChildTotalPrice = 0;
                decimal ChildBasePrice = data?.ChildBasePrice != null ? Convert.ToDecimal(data?.ChildBasePrice?.Value) : 0;
                decimal ChildTaxes = data?.ChildTaxes != null ? Convert.ToDecimal(data?.ChildTaxes?.Value) : 0;
                decimal InfantTotalPrice = 0;
                decimal InfantBasePrice = 0;
                decimal InfantTaxes = data?.InfantTaxes != null ? Convert.ToDecimal(data?.InfantTaxes?.Value) : 0;
                int UserID = data?.userID?.Value != null ? Convert.ToInt32(data?.userID?.Value) : 1; // Convert.ToInt32(Session["UserID"]);
                decimal OriginalCost = data?.OriginalCost?.Value != null ? Convert.ToDecimal(data?.OriginalCost?.Value) : 0;
                string CancelPenalty = data?.CancelPenalty?.Value != null ? Convert.ToString(data?.CancelPenalty?.Value) : "0";
                string ChangePenalty = data?.ChangePenalty?.Value != null ? Convert.ToString(data?.ChangePenalty?.Value) : "0";


                #region Booking Info

                decimal Adultsfare = AdultTotalPrice + AdultBasePrice + AdultTaxes;
                decimal Childfare = ChildTotalPrice + ChildBasePrice + ChildTaxes;
                decimal Infatfare = InfantTotalPrice + InfantBasePrice + InfantTaxes;
                string AirlineCode = data?.AirlineCode;
                decimal grandtotal = 0;

                DateTime dttrans = DateTime.Now.ToUniversalTime();
                string leadfirstname = !string.IsNullOrEmpty(firstName) ? firstName.ToString().ToUpper() : "";
                string leadlastname = !string.IsNullOrEmpty(lastName) ? lastName.ToString().ToUpper() : "";

                decimal[] TotalAmt = _AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Children, Infants, AirlineCode);

                decimal totaltaxes = Convert.ToDecimal(data?.TotalTaxes); //AdultTaxes + ChildTaxes + InfantTaxes;
                decimal totalfare = Convert.ToDecimal(data?.TotalPrice); // TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                grandtotal = Convert.ToDecimal(data?.GrandTotal); // totalfare + totaltaxes + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                //if (Adults > 0) { Adultsfare = TotalAmt[2] / Adults; }
                //if (Children > 0) { Childfare = TotalAmt[3] / Children; }
                //if (Infants > 0) { Infatfare = TotalAmt[4] / Infants; } 

                int BookingTypeID = 1; //odSegments.Count();

                string Scale = "";
                decimal Incentives = 0;

                var anyReservation = sql.Reservations.Any();
                int lastBookingId = 0;

                if (anyReservation)
                {
                    lastBookingId = sql.Reservations.Max(p => p.Id);
                }

                Reservation flight = new Reservation()
                {
                    Adults = Adults,
                    Children = Children,
                    Infants = Infants,
                    PNR = "",
                    AdultBasicFare = AdultBasePrice,
                    ChildBasicFare = ChildBasePrice,
                    InfantBasicFare = InfantBasePrice,
                    AdultTax = AdultTaxes,
                    ChildTax = ChildTaxes,
                    InfantTax = InfantTaxes,
                    TotalFare = totalfare,
                    TotalTaxes = totaltaxes,
                    GrandTotal = grandtotal,
                    CustomerComission = 1000,
                    CustomerOComission = 1000,
                    AirlineComission = 1000,
                    Incentive = Incentives,
                    IncentiveScale = Scale,
                    BookingDate = DateTime.Now.ToUniversalTime(),
                    BookingStatus = "Confirmed",
                    BookingTypeID = BookingTypeID,
                    LeadFirstName = leadfirstname,
                    LeadLastName = leadlastname,
                    UserID = UserID,
                    Email = email,
                    PrimaryPhone = primaryPhone,
                    SecondaryPhone = secondaryPhone,
                    Origin = from,
                    Destination = to,
                    DepartureDate = Convert.ToDateTime(start),
                    ArrivalDate = Convert.ToDateTime(end),
                    FlightBookingID = lastBookingId + 1,
                    Vendor = "GAL",
                    OriginalCost = OriginalCost,
                    CancelPenalty = CancelPenalty,
                    ChangePenalty = ChangePenalty
                };
                sql.Reservations.Add(flight);
                sql.SaveChanges();
                BookingID = flight.Id;



                byte[] AirPriceRsp = sql.AirPriceRsps.Where(f => f.TransactionId == transactionId).Select(f => f.Response).FirstOrDefault();

                DataSet flightSegment = new DataSet(); //JsonConvert.DeserializeObject<DataSet>(AirPriceRsp);
                if (AirPriceRsp != null && AirPriceRsp.Length > 0)
                {
                    Rootobject myObject = ByteArrayToObject<Rootobject>(AirPriceRsp); // Replace 'SomeClass' with the actual type of your object
                    string xmlString = ObjectToXml(myObject);

                    flightSegment = XmlToDataSet(xmlString);
                }


                //if (Session["AirPriceRsp"] != null)
                //{
                //    flightSegment = (DataSet)Session["AirPriceRsp"];
                //}

                if (flightSegment.Tables.Contains("AirSegment"))
                {
                    foreach (DataRow dr in flightSegment.Tables["Airsegment"].Rows)
                    {
                        string[] ArrDateTime;

                        if (Convert.ToString(dr["ArrivalTime"]).IndexOf('T') > 0)
                        {
                            ArrDateTime = dr["ArrivalTime"].ToString().Split('T');
                        }
                        else
                        {
                            ArrDateTime = dr["ArrivalTime"].ToString().Split(' ');
                        }

                        string ArrivalDate = ArrDateTime[0].ToString();
                        //string[] ArrTime = ArrDateTime[1].ToString().Split(' ');
                        string ArrivalTime = ArrDateTime[1].ToString();
                        string ArrivalGMTOffset = dr["ArrivalTime"].ToString().Substring(dr["ArrivalTime"].ToString().Length - 6);

                        string[] DepDateTime;

                        if (Convert.ToString(dr["DepartureTime"]).IndexOf('T') > 0)
                        {
                            DepDateTime = dr["DepartureTime"].ToString().Split('T');
                        }
                        else
                        {
                            DepDateTime = dr["DepartureTime"].ToString().Split(' ');
                        }



                        string DepartureDate = DepDateTime[0].ToString();
                        // string[] DepTime = DepDateTime[1].ToString().Split('+');
                        string DepartureTime = DepDateTime[1].ToString();
                        string DepartureGMTOffset = dr["DepartureTime"].ToString().Substring(dr["DepartureTime"].ToString().Length - 6);
                        byte segmenttype = 1;
                        string MarketingAirlineCode = dr != null && dr["Carrier"].ToString() != null ? dr["Carrier"].ToString() : "";
                        string OperatingAirlineCode = dr != null && dr["Carrier"].ToString() != null ? dr["Carrier"].ToString() : "";
                        string CompanyShortName = dr != null && dr["Carrier"].ToString() != null && dr["Carrier"].ToString() != null ? dr["Carrier"].ToString() : "";
                        string OriginCode = dr["Origin"].ToString();
                        string DestinationCode = dr["Destination"].ToString();
                        string serviceclass = dr["ClassOfService"].ToString();
                        int ServiceCls = sql.ServiceClasses.Where(x => x.ClassName == serviceclass).Select(x => x.ServiceClassID).FirstOrDefault();
                        int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                        int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
                        int OperatingAirlineID = sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
                        int MarketingAirlineID = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

                        FlightSegment flightseg = new FlightSegment()
                        {
                            ArrivalDate = Convert.ToDateTime(dr["ArrivalTime"].ToString()),
                            ArrivalTime = ArrivalTime,
                            ArrivalGMTOffset = ArrivalGMTOffset,
                            DepartureDate = Convert.ToDateTime(dr["DepartureTime"].ToString()),
                            DepartureTime = DepartureTime,
                            DepartureGMTOffset = DepartureGMTOffset,
                            ServiceClassID = 4,
                            OriginID = Origin,
                            DestinationID = Dest,
                            FlightNumber = dr["FlightNumber"].ToString(),
                            MarketingAirlineCode = MarketingAirlineCode,
                            OperatingAirlineCode = OperatingAirlineCode,
                            MarketingAirlineID = MarketingAirlineID,
                            OperatingAirlineID = OperatingAirlineID,
                            OperatingAirlineName = CompanyShortName,
                            FlightBookingID = lastBookingId + 1,
                            //AirEquipType = dr["Equipment"].ToString(),
                            ETicket = true,
                            ResBookDesigCode = dr["Key"].ToString(),
                            Way = segmenttype
                        };
                        sql.FlightSegments.Add(flightseg);
                        sql.SaveChanges();
                        BookingID = flightseg.FlightBookingID;
                    }
                }


                string TicketNum = "";
                string sex = "";
                string title = "";
                string dob = "";
                string nationality = "";
                string documenttype = "";
                string document = "";
                DateTime expiry = new DateTime();
                //  string mobile = "";
                #region Pax Info

                int TotalPax = Adults + Children + Infants;
                int Age = 0;
                string paxType = "";


                for (int i = 0; i < TotalPax; i++)
                {
                    bool sex1 = false;
                    DateTime _age = Convert.ToDateTime(data?["dob_" + i.ToString()]?.ToString());

                    sex = data?["Gender_" + i.ToString()]?.ToString(); // data?.GetType().GetProperty("Gender_"+i)??.Value;
                    //document = data?.GetType().GetProperty(_document)??.Value; //document_1

                    document = data?["document_" + i.ToString()]?.ToString().ToUpper();
                    documenttype = data?["documenttype_" + i.ToString()]?.ToString().ToUpper();

                    if (data?["expiry_" + i.ToString()] != null && data?["expiry_" + i.ToString()] != "")
                    {
                        expiry = Convert.ToDateTime(data?["expiry_" + i.ToString()]);
                    }

                    Age = CalculatePaxAge(_age);

                    nationality = data?["nationality_" + i.ToString()]?.ToString(); // data?.nationality_i??.Value.ToString();

                    if (Age < 2)
                    {
                        paxType = "INF";
                    }
                    else if (Age < 18)
                    {
                        paxType = "CNN";
                    }
                    else
                    {
                        paxType = "ADT";
                    }

                    dob = data?["dob_" + i.ToString()]?.ToString();
                    title = data?["title_" + i.ToString()]?.ToString(); // data?.title?.ToUpper();
                    firstName = data?["firstName_" + i.ToString()]?.ToString(); // data?.firstName_i??.Value.ToString().ToUpper();
                    lastName = data?["lastName_" + i.ToString()]?.ToString();  // data?.lastName_i??.Value.ToString().ToUpper();
                    TicketNum = "";

                    if (sex == "M") { sex1 = true; }
                    PassengerDetail passengerDetail = new PassengerDetail()
                    {
                        Title = title,
                        FirstName = firstName,
                        LastName = lastName,
                        DOB = Convert.ToDateTime(dob),
                        Gender = sex1,
                        Document = document,
                        TicketNo = TicketNum,
                        DocumentType = documenttype,
                        DOE = expiry,
                        PassengerStatus = 3,
                        PassegerType = paxType,
                        FlightBookingID = lastBookingId + 1,
                    };

                    sql.PassengerDetails.Add(passengerDetail);
                    sql.SaveChanges();

                }



                #endregion

                #endregion


                string bID = MakeGalFlightBooking(BookingID, transactionId);
                //string bID = Payment(BookingID.ToString(), grandtotal);
                return bID;
            }
            catch (Exception ex)
            {
                //  //TempData["Error"] = ex.Message + " <br /> " + ex.InnerException.InnerException.Message + " . Please try again.";
                return "something went wrong";
            }

        }

        [NonAction]
        public string Payment(string billReference, decimal GrandTotal)
        {
            string MerchantID = "33303563"; //"MC37526"; //Your Merchant from transaction Credentials 
            string Password = "udfu0v40u2"; // "e1z615h0xx"; //Your Password from transaction Credentials 
            string ReturnURL = "https://mountfeasthotel.com/searchresult/thankyou"; //Your Return URL 
            string HashKey = "2h10003su2";// "66v5u0c368";//Your HashKey from transaction Credentials 
                                          // string PostURL = "https://sandbox.jazzcash.com.pk/CustomerPortal/transactionmanagement/merchantform"; 
                                          //Live https://payments.jazzcash.com.pk/CustomerPortal/transactionmanagement/merchantform

            // date_default_timezone_set("Asia/karachi"); 

            decimal gTotal = GrandTotal * 100;
            double Amount = Convert.ToDouble(gTotal); //Last two digits will be considered as Decimal 
            string BillReference = billReference;
            string Description = "Thank you for using Jazz Cash";
            string Language = "EN";
            string TxnCurrency = "PKR";
            string TxnDateTime = DateTime.Now.ToString(DateTimeConstants.Formats.DatabaseDateTimeFormat); // date('YmdHis');
            string TxnExpiryDateTime = DateTime.Now.AddDays(1).ToString(DateTimeConstants.Formats.DatabaseDateTimeFormat);  // date('YmdHis', strtotime('+8 Days'));
            string TxnRefNumber = "T" + TxnDateTime; // date('YmdHis');
            string TxnType = "";
            string Version = "1.1";
            string SubMerchantID = "";
            string DiscountedAmount = "";
            string DiscountedBank = "";
            string ppmpf_1 = "";
            string ppmpf_2 = "";
            string ppmpf_3 = "";
            string ppmpf_4 = "";
            string ppmpf_5 = "";

            string[] HashArray = { Amount.ToString(), BillReference, Description, DiscountedAmount, DiscountedBank, Language, MerchantID, Password, ReturnURL, TxnCurrency, TxnDateTime, TxnExpiryDateTime, TxnRefNumber, TxnType, Version, ppmpf_1, ppmpf_2, ppmpf_3, ppmpf_4, ppmpf_5 };

            string SortedArray = HashKey;
            for (int i = 0; i < HashArray.Count(); i++)
            {
                if (HashArray[i] != "undefined" && HashArray[i] != null && HashArray[i] != "")
                {
                    SortedArray = SortedArray + "&" + HashArray[i];
                }
            }

            string Securehash; // hash_hmac('sha256', SortedArray, HashKey);
            Encoding encoding = Encoding.UTF8;
            var keyByte = encoding.GetBytes(HashKey);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                hmacsha256.ComputeHash(encoding.GetBytes(SortedArray));
                Securehash = ByteToString(hmacsha256.Hash);
            }
            //////////////////////////////////////////////////////



            NameValueCollection data = new NameValueCollection();


            data.Add("pp_Version", Version);
            data.Add("pp_TxnType", TxnType);
            data.Add("pp_Language", Language);
            data.Add("pp_MerchantID", MerchantID);
            data.Add("pp_SubMerchantID", SubMerchantID);
            data.Add("pp_Password", Password);
            data.Add("pp_TxnExpiryDateTime", TxnExpiryDateTime);
            data.Add("pp_TxnRefNo", TxnRefNumber);
            data.Add("pp_Amount", Amount.ToString());
            data.Add("pp_DiscountedAmount", DiscountedAmount);
            data.Add("pp_DiscountBank", DiscountedBank);
            data.Add("pp_TxnCurrency", TxnCurrency);
            data.Add("pp_TxnDateTime", TxnDateTime);
            data.Add("pp_BillReference", BillReference);
            data.Add("pp_Description", Description);
            data.Add("pp_ReturnURL", ReturnURL);
            data.Add("pp_SecureHash", Securehash);
            data.Add("ppmpf_1", ppmpf_1);
            data.Add("ppmpf_2", ppmpf_2);
            data.Add("ppmpf_3", ppmpf_3);
            data.Add("ppmpf_4", ppmpf_4);
            data.Add("ppmpf_5", ppmpf_5);

            IDictionary<string, string> parameters = new Dictionary<string, string>();

            foreach (string key in data)
            {
                parameters.Add(key, data[key]);
            }

            //TempData["FormData"] = data;

            string json = JsonConvert.SerializeObject(data.AllKeys.ToDictionary(k => k, k => data[k]));

            return json;
        }

        [NonAction]
        static string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
                sbinary += buff[i].ToString("X2"); /* hex format */
            return sbinary;
        }

        [NonAction]

        public IActionResult CancelPayment()
        {

            return null;
        }

        [NonAction]
        public string MakeGalFlightBooking(int? BookingID, string TransactionId)
        {
            string result = string.Empty;

            if (BookingID != 0)
            {
                DateTime dt = DateTime.Now;
                bool? sex;
                string? title = "";
                string firstName = "";
                string lastName = "";
                string? dob = "";
                string? nationality = "";
                string? documenttype = "";
                string? document = "";
                string? expiry = "";
                string? mobile = "";
                int bookingID = Convert.ToInt32(BookingID);


                //return bookingID.ToString();

                var AirPriceRsp = sql.AirPriceRsps.Where(f => f.TransactionId == TransactionId).Select(f => f.Response).FirstOrDefault();

                DataSet ds1 = new DataSet(); // JsonConvert.DeserializeObject<DataSet>(AirPriceRsp);

                if (AirPriceRsp != null && AirPriceRsp.Length > 0)
                {
                    Rootobject myObject = ByteArrayToObject<Rootobject>(AirPriceRsp); // Replace 'SomeClass' with the actual type of your object
                    string xmlString = ObjectToXml(myObject);

                    ds1 = XmlToDataSet(xmlString);
                }

                byte[] APS = sql.AirPriceSols.Where(f => f.TransactionId == TransactionId).Select(f => f.Response).FirstOrDefault();
                string AirPriceSol = Encoding.UTF8.GetString(APS);

                //int ind = AirPriceSol.IndexOf(@"<PassengerType Code='ADT' />");

                //if (ind > 0)
                //{
                //    AirPriceSol = AirPriceSol.Replace(@"<PassengerType Code='ADT' />", "");
                //}

                //AirPriceSol = AirPriceSol.Insert(ind, @"<PassengerType Code='ADT' />");

                //string substring = @"<PassengerType Code=\""ADT\"" />";
                //int count = 0;
                //int s = 0;
                //while ((s = AirPriceSol.IndexOf(substring, s)) != -1)
                //{
                //    count++;
                //    s++;
                //}

                //Random random = new Random();
                //string num = random.Next(9).ToString();
                try
                {
                    var Reservation = (from x in sql.Reservations.AsEnumerable()
                                       where x.Id == bookingID
                                       select new
                                       {
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
                                           IncentiveScale = x.IncentiveScale,
                                           BookingDate = x.BookingDate,
                                           BookingStatus = x.BookingStatus,
                                           BookingTypeID = x.BookingTypeID,
                                           LeadFirstName = x.LeadFirstName,
                                           LeadLastName = x.LeadLastName,
                                           Email = x.Email,
                                           PrimaryPhone = x.PrimaryPhone,
                                           SecondaryPhone = x.SecondaryPhone
                                       }).ToList();

                    //var fs = sql.FlightSegments.Where(f => f.FlightBookingID == bookingID).Select(f => f.ServiceClassID).FirstOrDefault();


                    var flightSegments = (from x in sql.FlightSegments.AsEnumerable()
                                          where x.FlightBookingID == bookingID
                                          select new
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
                                              AirEquipType = x.AirEquipType,
                                              ETicket = x.ETicket,
                                              ResBookDesigCode = x.ResBookDesigCode,
                                              Way = x.Way
                                          }).ToList();

                    var PaxList = (from x in sql.PassengerDetails.AsEnumerable()
                                   where x.FlightBookingID == bookingID
                                   select new
                                   {
                                       Title = x.Title,
                                       FirstName = x.FirstName,
                                       LastName = x.LastName,
                                       DOB = x.DOB,
                                       Gender = x.Gender,
                                       Document = x.Document,
                                       TicketNo = x.TicketNo,
                                       DocumentType = x.DocumentType,
                                       DOE = x.DOE,
                                       FlightBookingID = x.FlightBookingID,
                                       PassengerStatus = x.PassengerStatus,
                                       PassegerType = x.PassegerType
                                   }).ToList();

                    var ContactDetails = (from x in sql.Reservations.AsEnumerable()
                                          where x.Id == bookingID
                                          select new
                                          {
                                              Email = x.Email,
                                              PhoneNo = x.PrimaryPhone
                                          }).FirstOrDefault();


                    //string securitytoken = Session["securitytoken"].ToString();
                    //string convid = Session["convid"].ToString();


                    DateTime datet = DateTime.Now.Date;
                    string Dat = datet.ToString("yyyy-MM-dd");

                    DateTime dt1 = DateTime.UtcNow;
                    string tstamp = dt1.ToString("s") + "Z";
                    string AirlineCode = flightSegments[0].OperatingAirlineCode;
                    bool error = false;
                    string email = ContactDetails.Email.ToString();
                    string contact = ContactDetails.PhoneNo.ToString();

                    int Adults = (from x in PaxList where x.PassegerType == "ADT" select x).Count();
                    int Children = (from x in PaxList where x.PassegerType == "CNN" select x).Count();
                    int Infants = (from x in PaxList where x.PassegerType == "INFT" select x).Count();

                    string address = "";
                    string cityname = "";
                    string agencyname = "";
                    string postalcode = "";
                    string phonenumber = "";
                    string companyemail = "";
                    string AdtPaxType = "";
                    string CnnPaxType = "";
                    string InfPaxType = "";
                    string _AdtPaxType = "";
                    string _CnnPaxType = "";
                    string _InfPaxType = "";
                    int CustomerID = 0; // Convert.ToInt32(Session["CustomerID"]);
                    var customers = sql.Customers.Where(x => x.CustomerID == CustomerID).FirstOrDefault();
                    if (customers != null)
                    {
                        companyemail = customers.Email;
                        postalcode = customers.PostalCode;
                        agencyname = customers.CompanyName.Replace("&", "and");
                        if (customers.AddressLine1.Length > 25)
                        {
                            address = !string.IsNullOrEmpty(customers.StreetAddress) ? customers.StreetAddress.Replace("&", "and") : !string.IsNullOrEmpty(customers.AddressLine1) ? customers.AddressLine1.Substring(0, 25).Replace("&", "and") : "";

                        }
                        else
                        {
                            address = !string.IsNullOrEmpty(customers.StreetAddress) ? customers.StreetAddress.Replace("&", "and") : !string.IsNullOrEmpty(customers.AddressLine1) ? customers.AddressLine1.Replace("&", "and") : "";
                        }
                        phonenumber = !string.IsNullOrEmpty(customers.Telephone) ? customers.Telephone : "";


                        int cityid = customers.CityID ?? 1;
                        cityname = sql.Cities.Where(x => x.cityID == cityid).Count() > 0 ? sql.Cities.Where(x => x.cityID == cityid).Select(x => x.cityName).FirstOrDefault() : "";
                    }

                    #region XML Request
                    //DataSet ds = (DataSet)Session["AirPriceRsp"];
                    //DataTable dt = new DataTable();
                    StringBuilder strReq = new StringBuilder();
                    Guid TraceId = Guid.NewGuid();

                    strReq.Append(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>");
                    strReq.Append("<soapenv:Header/>");
                    strReq.Append("<soapenv:Body>");
                    //strReq.Append("<AirCreateReservationReq xmlns='http://www.travelport.com/schema/universal_v42_0' TraceId='" + TraceId + "' TargetBranch='P7113657' Authorizesqly='Travelport' ProviderCode='1G' RetainReservation='None'>");
                    strReq.Append("<AirCreateReservationReq xmlns='http://www.travelport.com/schema/universal_v42_0' TraceId='" + TraceId + "' TargetBranch='P3393195' Authorizesqly='Travelport' ProviderCode='1G' RetainReservation='Both'>");
                    strReq.Append("<BillingPointOfSaleInfo xmlns='http://www.travelport.com/schema/common_v42_0' OriginApplication='uAPI' />");

                    int a = 1;
                    int c = 1;
                    int i = 1;

                    foreach (var itm in PaxList)
                    {
                        if (itm.PassegerType == "ADT")
                        {
                            dob = Convert.ToDateTime(itm.DOB).ToString("yyyy-MM-dd");
                            sex = itm.Gender;
                            document = string.IsNullOrWhiteSpace(itm.Document) ? "" : itm.Document.ToString().ToUpper();
                            documenttype = string.IsNullOrWhiteSpace(itm.DocumentType) ? "" : itm.DocumentType.ToString().ToUpper();
                            expiry = Convert.ToDateTime(itm.DOE).ToString("yyyy-MM-dd");
                            nationality = "PK";
                            title = string.IsNullOrWhiteSpace(itm.Title) ? "" : itm.Title.ToUpper();
                            firstName = string.IsNullOrWhiteSpace(itm.FirstName) ? "" : itm.FirstName.ToString().ToUpper();
                            lastName = string.IsNullOrWhiteSpace(itm.LastName) ? "" : itm.LastName.ToString().ToUpper();
                            mobile = string.IsNullOrWhiteSpace(ContactDetails.PhoneNo) ? "" : ContactDetails.PhoneNo.ToString();

                            strReq.Append("<BookingTraveler xmlns='http://www.travelport.com/schema/common_v42_0' Key='Adt-" + a + "' TravelerType='ADT' Age='" + CalculatePaxAge(Convert.ToDateTime(dob)) + "' DOB='2010-02-07' Gender='M' Nationality='PK' >");
                            //AdtPaxType = AdtPaxType + "<PassengerType Code='ADT' BookingTravelerRef='Adt-" + a + "' />";
                            if (a == 1)
                            {
                                _AdtPaxType = _AdtPaxType + "<PassengerType Code='ADT'/>";
                            }

                            strReq.Append("<BookingTravelerName First='" + firstName + "' Last='" + lastName + "' Prefix='" + title + "'/>");

                            if (a == 1)
                            {
                                strReq.Append("<DeliveryInfo>");
                                strReq.Append("<ShippingAddress  Key='DeliveryInfo-" + c + "'>");
                                strReq.Append("<AddressName>Malik Express</AddressName>");
                                strReq.Append("<Street>Office # 1-2, Javed Plaza, Block 31,</Street>");
                                strReq.Append("<City>Islamabad</City>");
                                strReq.Append("<State>ICT</State>");
                                strReq.Append("<PostalCode>44000</PostalCode>");
                                strReq.Append("<Country>PK</Country>");
                                strReq.Append("</ShippingAddress>");
                                strReq.Append("</DeliveryInfo>");
                            }
                            strReq.Append("<PhoneNumber AreaCode='051' CountryCode='092' Location='PAK' Number='" + mobile + "'/>");
                            strReq.Append("<Email EmailID='" + email + "' Type='Home'/>");

                            DateTime originalDOBDate = DateTime.ParseExact(dob, "yyyy-MM-dd", null);

                            // Get the month abbreviation (e.g., "JAN")
                            string monthDOBAbbreviation = originalDOBDate.ToString("MMM").ToUpper();

                            // Get the last two digits of the year
                            string yearDOBAbbreviation = originalDOBDate.ToString("yy");

                            // Construct the final formatted date string
                            string formattedDOBDate = $"{originalDOBDate.ToString("dd")}{monthDOBAbbreviation}{yearDOBAbbreviation}";

                            DateTime originalExpiryDate = DateTime.ParseExact(expiry, "yyyy-MM-dd", null);

                            // Get the month abbreviation (e.g., "JAN")
                            string monthExpiryAbbreviation = originalExpiryDate.ToString("MMM").ToUpper();

                            // Get the last two digits of the year
                            string yearExpiryAbbreviation = originalExpiryDate.ToString("yy");

                            // Construct the final formatted date string
                            string formattedExpiryDate = $"{originalExpiryDate.ToString("dd")}{monthExpiryAbbreviation}{yearExpiryAbbreviation}";

                            strReq.Append("<SSR Type='DOCS' FreeText='P/PK/" + document + "/PK/" + formattedDOBDate + "/M/" + formattedExpiryDate + "/" + firstName + "/" + lastName + "' Carrier='" + AirlineCode + "' />");

                            strReq.Append("<Address>");
                            strReq.Append("<AddressName>Malik Express</AddressName>");
                            strReq.Append("<Street>Office # 1-2, Javed Plaza, Block 31,</Street>");
                            strReq.Append("<City>Islamabad</City>");
                            strReq.Append("<State>ICT</State>");
                            strReq.Append("<PostalCode>44000</PostalCode>");
                            strReq.Append("<Country>PK</Country>");
                            strReq.Append("</Address>");
                            strReq.Append("</BookingTraveler>");
                            a = a + 1;
                        }

                        if (itm.PassegerType == "INF")
                        {
                            {
                                dob = Convert.ToDateTime(itm.DOB).ToString("yyyy-MM-dd");
                                sex = itm.Gender;
                                document = string.IsNullOrWhiteSpace(itm.Document) ? "" : itm.Document.ToString().ToUpper();
                                documenttype = string.IsNullOrWhiteSpace(itm.DocumentType) ? "" : itm.DocumentType.ToString().ToUpper();
                                expiry = Convert.ToDateTime(itm.DOE).ToString("yyyy-MM-dd");
                                nationality = "PK";
                                title = string.IsNullOrWhiteSpace(itm.Title) ? "" : itm.Title.ToUpper();
                                firstName = string.IsNullOrWhiteSpace(itm.FirstName) ? "" : itm.FirstName.ToString().ToUpper();
                                lastName = string.IsNullOrWhiteSpace(itm.LastName) ? "" : itm.LastName.ToString().ToUpper();
                                mobile = string.IsNullOrWhiteSpace(ContactDetails.PhoneNo) ? "" : ContactDetails.PhoneNo.ToString();

                                strReq.Append("<BookingTraveler xmlns='http://www.travelport.com/schema/common_v42_0' Key='INF-" + i + "' TravelerType='INF' Age='" + CalculatePaxAge(Convert.ToDateTime(dob)) + "' DOB='" + dob + "' Gender='M' Nationality='PK' >");
                                // InfPaxType = _InfPaxType + "<PassengerType Code='INF' Age='" + CalculatePaxAge(Convert.ToDateTime(dob)) + "' BookingTravelerRef='INF-" + i + "' />";
                                _InfPaxType = _InfPaxType + "<PassengerType Code='INF' />";

                                //}
                                strReq.Append("<BookingTravelerName First='" + firstName + "' Last='" + lastName + "' Prefix='" + title + "'/>");
                                strReq.Append("<PhoneNumber AreaCode='051' CountryCode='092' Location='PAK' Number='" + mobile + "'/>");
                                strReq.Append("<Email EmailID='" + email + "' Type='Home'/>");


                                DateTime originalDOBDate = DateTime.ParseExact(dob, "yyyy-MM-dd", null);

                                // Get the month abbreviation (e.g., "JAN")
                                string monthDOBAbbreviation = originalDOBDate.ToString("MMM").ToUpper();

                                // Get the last two digits of the year
                                string yearDOBAbbreviation = originalDOBDate.ToString("yy");

                                // Construct the final formatted date string
                                string formattedDOBDate = $"{originalDOBDate.ToString("dd")}{monthDOBAbbreviation}{yearDOBAbbreviation}";

                                DateTime originalExpiryDate = DateTime.ParseExact(expiry, "yyyy-MM-dd", null);

                                // Get the month abbreviation (e.g., "JAN")
                                string monthExpiryAbbreviation = originalExpiryDate.ToString("MMM").ToUpper();

                                // Get the last two digits of the year
                                string yearExpiryAbbreviation = originalExpiryDate.ToString("yy");

                                // Construct the final formatted date string
                                string formattedExpiryDate = $"{originalExpiryDate.ToString("dd")}{monthExpiryAbbreviation}{yearExpiryAbbreviation}";

                                strReq.Append("<SSR Type='DOCS' FreeText='P/PK/" + document + "/PK/" + formattedDOBDate + "/M/" + formattedExpiryDate + "/" + firstName + "/" + lastName + "' Carrier='" + AirlineCode + "' />");

                                string _dob = Convert.ToDateTime(dob).ToString("ddMMMyy", CultureInfo.InvariantCulture);
                                strReq.Append("<NameRemark><RemarkData>" + _dob + "</RemarkData></NameRemark>");
                                strReq.Append("</BookingTraveler>");
                            }
                            i = i + 1;
                        }

                        if (itm.PassegerType == "CNN")
                        {
                            c = 1;
                            {
                                dob = Convert.ToDateTime(itm.DOB).ToString("yyyy-MM-dd");
                                sex = itm.Gender;
                                document = string.IsNullOrWhiteSpace(itm.Document) ? "" : itm.Document.ToString().ToUpper();
                                documenttype = string.IsNullOrWhiteSpace(itm.DocumentType) ? "" : itm.DocumentType.ToString().ToUpper();
                                expiry = Convert.ToDateTime(itm.DOE).ToString("yyyy-MM-dd");
                                nationality = "PK";
                                title = string.IsNullOrWhiteSpace(itm.Title) ? "" : itm.Title.ToUpper();
                                firstName = string.IsNullOrWhiteSpace(itm.FirstName) ? "" : itm.FirstName.ToString().ToUpper();
                                lastName = string.IsNullOrWhiteSpace(itm.LastName) ? "" : itm.LastName.ToString().ToUpper();
                                mobile = string.IsNullOrWhiteSpace(ContactDetails.PhoneNo) ? "" : ContactDetails.PhoneNo.ToString();

                                strReq.Append("<BookingTraveler xmlns='http://www.travelport.com/schema/common_v42_0' Key='CNN-" + c + "' TravelerType='CNN' Age='" + CalculatePaxAge(Convert.ToDateTime(dob)) + "' DOB='" + dob + "' Gender='M' Nationality='PK' >");
                                // CnnPaxType = CnnPaxType + "<PassengerType Code='CNN' Age='" + CalculatePaxAge(Convert.ToDateTime(dob)) + "' BookingTravelerRef='CNN-" + c + "' />";
                                _CnnPaxType = _CnnPaxType + "<PassengerType Code='CNN' />";
                                strReq.Append("<BookingTravelerName First='" + firstName + "' Last='" + lastName + "' Prefix='" + title + "'/>");
                                strReq.Append("<PhoneNumber AreaCode='051' CountryCode='092' Location='PAK' Number='" + mobile + "'/>");
                                strReq.Append("<Email EmailID='" + email + "' Type='Home'/>");

                                DateTime originalDOBDate = DateTime.ParseExact(dob, "yyyy-MM-dd", null);

                                // Get the month abbreviation (e.g., "JAN")
                                string monthDOBAbbreviation = originalDOBDate.ToString("MMM").ToUpper();

                                // Get the last two digits of the year
                                string yearDOBAbbreviation = originalDOBDate.ToString("yy");

                                // Construct the final formatted date string
                                string formattedDOBDate = $"{originalDOBDate.ToString("dd")}{monthDOBAbbreviation}{yearDOBAbbreviation}";

                                DateTime originalExpiryDate = DateTime.ParseExact(expiry, "yyyy-MM-dd", null);

                                // Get the month abbreviation (e.g., "JAN")
                                string monthExpiryAbbreviation = originalExpiryDate.ToString("MMM").ToUpper();

                                // Get the last two digits of the year
                                string yearExpiryAbbreviation = originalExpiryDate.ToString("yy");

                                // Construct the final formatted date string
                                string formattedExpiryDate = $"{originalExpiryDate.ToString("dd")}{monthExpiryAbbreviation}{yearExpiryAbbreviation}";

                                strReq.Append("<SSR Type='DOCS' FreeText='P/PK/" + document + "/PK/" + formattedDOBDate + "/M/" + formattedExpiryDate + "/" + firstName + "/" + lastName + "' Carrier='" + AirlineCode + "' />");

                                strReq.Append("<NameRemark><RemarkData>P-C" + CalculatePaxAge(Convert.ToDateTime(dob)) + "</RemarkData></NameRemark>");
                                strReq.Append("</BookingTraveler>");
                            }
                            c = c + 1;
                        }
                    }


                    //BookingTravelerRef
                    string AirPricingSolution = AirPriceSol; ;

                    if (string.IsNullOrEmpty(_AdtPaxType) == false)
                    {
                        _AdtPaxType = _AdtPaxType.Replace("'", "\"");
                        AirPricingSolution = AirPricingSolution.Replace(_AdtPaxType, AdtPaxType); // Adults BookingTravelerRef
                    }

                    if (string.IsNullOrEmpty(_CnnPaxType) == false)
                    {
                        _CnnPaxType = _CnnPaxType.Replace("'", "\"");
                        AirPricingSolution = AirPricingSolution.Replace(_CnnPaxType, CnnPaxType); // Children BookingTravelerRef
                    }

                    if (string.IsNullOrEmpty(_InfPaxType) == false)
                    {
                        _InfPaxType = _InfPaxType.Replace("'", "\"");
                        AirPricingSolution = AirPricingSolution.Replace(_InfPaxType, InfPaxType); // Infants BookingTravelerRef
                    }

                    if (AirPricingSolution.IndexOf("PlatingCarrier") > -1)
                    {
                        int idx = AirPricingSolution.IndexOf("PlatingCarrier=");
                        AirPricingSolution = AirPricingSolution.Remove(idx, 19);
                    }

                    if (AirPricingSolution.IndexOf("PLATING CARRIER ") > -1)
                    {
                        AirPricingSolution = AirPricingSolution.Replace("PLATING CARRIER ", "");
                    }

                    strReq.Append("<FormOfPayment xmlns='http://www.travelport.com/schema/common_v42_0' Key='1werwesd33' Type='Cash' ></FormOfPayment>");

                    //DataSet ds1 = (DataSet)Session["AirPriceRsp"];
                    string TotalPrice = "";

                    if (ds1.Tables.Contains("Airpricingsolution"))
                    {
                        strReq.Append("<AirPricingSolution xmlns='http://www.travelport.com/schema/air_v42_0' Key='" + ds1.Tables["Airpricingsolution"].Rows[0]["Key"].ToString() +
                            "' TotalPrice='" + ds1.Tables["Airpricingsolution"].Rows[0]["TotalPrice"].ToString() + "'" +
                            " BasePrice='" + ds1.Tables["Airpricingsolution"].Rows[0]["BasePrice"].ToString() + "'" +
                            " ApproximateTotalPrice='" + ds1.Tables["Airpricingsolution"].Rows[0]["ApproximateTotalPrice"].ToString() + "'" +
                            " ApproximateBasePrice='" + ds1.Tables["Airpricingsolution"].Rows[0]["ApproximateBasePrice"].ToString() + "'" +
                            //" EquivalentBasePrice='" + ds1.Tables["AirPricingSolution"].Rows[0]["EquivalentBasePrice"].ToString() + "'" +
                            " Taxes='" + ds1.Tables["Airpricingsolution"].Rows[0]["Taxes"].ToString() + "'" +
                            " Fees='" + ds1.Tables["Airpricingsolution"].Rows[0]["Fees"].ToString() + "'>");

                        TotalPrice = ds1.Tables["Airpricingsolution"].Rows[0]["TotalPrice"].ToString();
                    }

                    StringBuilder airsegment = new StringBuilder();
                    DataTable dtConnections = new DataTable();
                    if (ds1.Tables.Contains("Airsegment"))
                    {
                        foreach (DataRow dr in ds1.Tables["Airsegment"].Rows)
                        {
                            airsegment.Append("<AirSegment Key='" + dr["Key"].ToString() + "'" +
                                 " OptionalServicesIndicator='" + dr["OptionalServicesIndicator"].ToString() + "'" +
                                 " AvailabilityDisplayType='" + dr["AvailabilityDisplayType"].ToString() + "'" +
                                 " Group ='" + dr["Group"].ToString() + "' Carrier='" + dr["Carrier"].ToString() + "'" +
                                 " FlightNumber='" + dr["FlightNumber"].ToString() + "'" +
                                 " Origin='" + dr["Origin"].ToString() + "' Destination='" + dr["Destination"].ToString() + "'" +
                                 " DepartureTime='" + Convert.ToDateTime(dr["DepartureTime"]).ToString("yyyy-MM-ddTHH:mm:ssK") + "'" +
                                 " ArrivalTime='" + Convert.ToDateTime(dr["ArrivalTime"]).ToString("yyyy-MM-ddTHH:mm:ssK") + "'" +
                                 " FlightTime='" + dr["FlightTime"].ToString() + "' TravelTime='" + dr["TravelTime"].ToString() + "'" +
                                 " Distance='" + dr["Distance"].ToString() + "' ProviderCode='" + dr["ProviderCode"].ToString() + "'" +
                                 " ClassOfService='" + dr["ClassOfService"].ToString() + "'>");
                            airsegment.Append("<CodeshareInfo OperatingCarrier='" + dr["Carrier"].ToString() + "'/>");


                            //if (dr["Destination"].ToString() != Session["Dest"].ToString() && dr["Origin"].ToString() == Session["Depart"].ToString())
                            //{
                            //    airsegment.Append("<Connection/>");
                            //}
                            //else if (dr["Destination"].ToString() != Session["Depart"].ToString() && dr["Origin"].ToString() == Session["Dest"].ToString())
                            //{
                            //    airsegment.Append("<Connection/>");
                            //}


                            airsegment.Append("</AirSegment>");
                        }
                    }

                    int idx1 = AirPricingSolution.IndexOf("<AirPricingInfo");
                    AirPricingSolution = AirPricingSolution.Insert(idx1, airsegment.ToString());


                    StringBuilder strAdults = new StringBuilder();

                    if (Adults > 0)
                    {
                        for (int b = 1; b <= Adults; b++)
                        {
                            strAdults.Append("<PassengerType Code='ADT' BookingTravelerRef='Adt-" + b + "'/>");
                        }
                    }

                    StringBuilder strChildren = new StringBuilder();

                    if (Children > 0)
                    {
                        for (int b = 1; b <= Children; b++)
                        {
                            strChildren.Append("<PassengerType Code='CNN'  Age='10' BookingTravelerRef='CNN-" + b + "'/>");
                        }
                    }

                    StringBuilder strInfant = new StringBuilder();

                    if (Infants > 0)
                    {
                        for (int b = 1; b <= Infants; b++)
                        {
                            strInfant.Append("<PassengerType Code='INF'  Age='1' BookingTravelerRef='INF-" + b + "'/>");
                        }
                    }

                    //List<int> indexes = new List<int>();
                    string strAirPricingInfo = "</AirPricingInfo";

                    //var allIndexOf = AirPricingSolution.AllIndexOf(strAirPricingInfo, StringComparison.OrdinalIgnoreCase);

                    var indexes = Regex.Matches(AirPricingSolution, strAirPricingInfo).Cast<Match>().Select(m => m.Index).ToList();

                    AirPricingSolution.Replace(@"\", "");

                    AirPricingSolution.Replace(@"common_v42_0:", "");

                    AirPricingSolution.Replace(" \" ", " ");

                    //<PassengerType Code=\"ADT\" />

                    if (AirPricingSolution.IndexOf(@"<PassengerType Code=""ADT"" />") > -1)
                    {
                        AirPricingSolution = AirPricingSolution.Replace(@"<PassengerType Code=""ADT"" />", strAdults.ToString());
                    }
                    else
                    {
                        AirPricingSolution = AirPricingSolution.Replace(@"<PassengerType Code=\""ADT\"" />", strAdults.ToString());
                    }

                    if (AirPricingSolution.IndexOf(@"<PassengerType Code=""CNN"" Age=""10"" />") > -1)
                    {
                        AirPricingSolution = AirPricingSolution.Replace(@"<PassengerType Code=""CNN"" Age=""10"" />", strChildren.ToString());
                    }
                    else
                    {
                        AirPricingSolution = AirPricingSolution.Replace(@"<PassengerType Code=\""CNN\"" Age=\""10\"" />", strChildren.ToString());
                    }
                    if (AirPricingSolution.IndexOf(@"<PassengerType Code=""INF"" Age=""1"" />") > -1)
                    {
                        AirPricingSolution = AirPricingSolution.Replace(@"<PassengerType Code=\""INF\"" Age=\""1\"" />", strInfant.ToString());
                    }
                    else
                    {
                        AirPricingSolution = AirPricingSolution.Replace(@"<PassengerType Code=""INF"" Age=""1"" />", strInfant.ToString());
                    }

                    strReq.Append(AirPricingSolution);




                    strReq.Append("</AirPricingSolution>");
                    strReq.Append("<ActionStatus xmlns='http://www.travelport.com/schema/common_v42_0' Type='ACTIVE' TicketDate='T*' ProviderCode='1G' />");
                    //strReq.Append("<Payment xmlns='http://www.travelport.com/schema/common_v42_0' Key='2' Type='Itinerary' FormOfPaymentRef='1werwesd33' Amount='" + TotalPrice + "' />");
                    strReq.Append("</AirCreateReservationReq>");
                    strReq.Append("</soapenv:Body>");
                    strReq.Append("</soapenv:Envelope>");


                    strReq.Replace(@"\", "");

                    strReq.Replace(@"common_v42_0:", "");

                    strReq.Replace(" \" ", " ");


                    var doc = XDocument.Parse(strReq.ToString());
                    //  doc.Root.Descendants().Where(e => e.Name == "Endorsement" || e.Name == "ServiceData").Remove();
                    //doc.Root.Descendants().Where(e => e.Name == "Endorsement").Remove();
                    // return strReq.ToString();

                    while (true)
                    {
                        bool removed = false;
                        foreach (var elem in doc.Descendants())
                        {
                            if (elem.Name.LocalName == "Endorsement" || elem.Name.LocalName == "AirSegmentRef" || elem.Name.LocalName == "ServiceData" || elem.Name.LocalName == "ServiceInfo"
                               || elem.Name.LocalName == "Remark" || elem.Name.LocalName == "BrandingInfo" || elem.Name.LocalName == "ApplicationRules" || elem.Name.LocalName == "OptionalServices"
                               )
                            {
                                elem.Remove();
                                removed = true;
                                break;
                            }
                        }
                        if (!removed)
                            break;
                    }


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

                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(result1);
                        XmlReader xmlReader = new XmlNodeReader(xml);
                        XmlReader reader = doc.CreateReader();

                        //Log

                        //XmlDocument xml1 = new XmlDocument();
                        //xml1.LoadXml(doc.ToString());
                        //XmlReader xmlReader1 = new XmlNodeReader(xml1);

                        //XmlTextWriter writer = new XmlTextWriter(@"D:\TravelportLog\AirCreateReservationReq.xml", null);
                        //writer.Formatting = System.Xml.Formatting.Indented;
                        //xml1.Save(writer);

                        //XmlTextWriter writer1 = new XmlTextWriter(@"D:\TravelportLog\AirCreateReservationRsp.xml", null);
                        //writer1.Formatting = System.Xml.Formatting.Indented;
                        //xml.Save(writer1);

                        // Commnent above 9 lines before publishing



                        //Booking Dataset
                        ds.ReadXml(xmlReader);



                        #endregion
                        //string str = strReq.ToString();
                        //XDocument req = XDocument.Parse(strReq.ToString());                   

                        if (ds.Tables.Count > 0)
                        {
                            if (!ds.Tables.Contains("AirPricingInfo"))
                            {
                                if (ds.Tables.Contains("ResponseMessage"))
                                {
                                    if (ds.Tables["ResponseMessage"].Rows.Count > 0)
                                    {
                                        ////ViewBag.Message = ds.Tables["ResponseMessage"].Rows[0]["ResponseMessage_Text"].ToString();
                                        return ds.Tables["ResponseMessage"].Rows[0]["ResponseMessage_Text"].ToString();
                                    }
                                }
                                if (ds.Tables.Contains("AirSegmentError"))
                                {
                                    if (ds.Tables["AirSegmentError"].Rows.Count > 0)
                                    {
                                        ////ViewBag.Message = ds.Tables["AirSegmentError"].Rows[0]["ErrorMessage"].ToString();
                                        return ds.Tables["AirSegmentError"].Rows[0][1].ToString();
                                    }
                                }
                                if (ds.Tables.Contains("Fault"))
                                {
                                    if (ds.Tables["Fault"].Rows.Count > 0)
                                    {
                                        ////ViewBag.Message = ds.Tables["Fault"].Rows[0]["faultstring"].ToString();
                                        return ds.Tables["Fault"].Rows[0]["faultstring"].ToString();
                                    }
                                }
                            }


                            //DataSet ds2 = (DataSet)Session["AirPriceRsp"];
                            DataTable dtAirPricingInfo;

                            string AirPricingInfoKey = "1";

                            // ds2.ReadXml(AirPricingSolution);
                            List<string> AirPricingInfoKeys = new List<string>();

                            if (ds.Tables.Contains("AirPricingInfo"))
                            {
                                foreach (DataRow ro in ds.Tables["AirPricingInfo"].Rows)
                                {
                                    AirPricingInfoKeys.Add(ro["Key"].ToString());
                                }
                                dtAirPricingInfo = (DataTable)ds.Tables["AirPricingInfo"];
                            }
                            else
                            {
                                AirPricingInfoKeys.Add(ds1.Tables["AirPricingInfo"].Rows[0]["Key"].ToString());
                                dtAirPricingInfo = (DataTable)ds1.Tables["AirPricingInfo"];
                            }

                            string ID = ds.Tables["ProviderReservationInfo"].Rows[0]["LocatorCode"].ToString();
                            string PnrForTicket = ds.Tables["AirReservation"].Rows[0]["LocatorCode"].ToString();
                            string AirlinePNR = ds.Tables["SupplierLocator"].Rows[0]["SupplierLocatorCode"].ToString();

                            if (AirPricingInfoKeys.Count > 0)
                            {
                                //Save pnr for tickets
                                foreach (var itm in AirPricingInfoKeys)
                                {
                                    AirPricingInfoKey api = new AirPricingInfoKey()
                                    {
                                        PNR = ID,
                                        PnrForTicket = PnrForTicket,
                                        airPricingInfoKey = itm,
                                        BookingId = BookingID,
                                        AirlinePNR = AirlinePNR
                                    };
                                    sql.AirPricingInfoKeys.Add(api);
                                    sql.SaveChanges();
                                }

                                //Update PNR in FlightBooking
                                Reservation fb = (from p in sql.Reservations
                                                  where p.Id == bookingID
                                                  select p).SingleOrDefault();
                                fb.PNR = ID;
                                sql.SaveChanges();
                                //////////////////////////
                                // Session["PNR"] = ID;

                                return bookingID.ToString();
                            }

                            //  Session.Remove("AirPriceRsp");
                            //  Session.Remove("AirPricingSolution");
                            // //TempData["Data"] = 0;

                            //  return RedirectToAction("GalInvoice", "FlightInvoice");
                            return bookingID.ToString();
                        }

                        else
                        {
                            string warrning = ""; ;
                            foreach (var msg in ds.Tables[""].Rows[0]["fault"].ToString())
                            {
                                warrning += msg;
                            }
                            if (warrning != "")
                            {
                                //    //ViewBag.Message = warrning;
                            }
                            //ViewBag.Adults = Adults;
                            //ViewBag.Childs = Children;
                            //ViewBag.Infants = Infants;
                            //TempData["Data"] = 0;
                            // List<MalikExpressMVC.Models.XML.OriginDestinationOption> odSegments = (List<MalikExpressMVC.Models.XML.OriginDestinationOption>)Session["odSegments"];
                            return warrning;
                        }
                    }
                    else
                    {
                        //TempData["Data"] = 0;
                        // List<MalikExpressMVC.Models.XML.OriginDestinationOption> odSegments = (List<MalikExpressMVC.Models.XML.OriginDestinationOption>)Session["odSegments"];
                        //ViewBag.Message = "Something went wrong. Please try again.";
                        return "something went wrong";
                    }
                }
                catch (Exception ex)
                {
                    //TempData["Data"] = 0;
                    //ViewBag.Message = ex.Message + " " + ex.StackTrace + " . Please try again.";
                    //List<MalikExpressMVC.Models.XML.OriginDestinationOption> odSegments = (List<MalikExpressMVC.Models.XML.OriginDestinationOption>)Session["odSegments"];

                    return "something went wrong";
                }
            }
            else
            {
                // List<MalikExpressMVC.Models.XML.OriginDestinationOption> odSegments = (List<MalikExpressMVC.Models.XML.OriginDestinationOption>)Session["odSegments"];
                return "something went wrong"; // View(odSegments);
            }

            return "";
        }

        [HttpGet("thankyou")]
        public IActionResult Thankyou(string BookingID)
        {
            try
            {
                int bookingID = Convert.ToInt32(BookingID);
                var Reservation = (from x in sql.Reservations.AsEnumerable()
                                   where x.Id == bookingID
                                   select new Reservation2
                                   {
                                       BookingID = x.Id,
                                       Adults = x.Adults.ToString(),
                                       Children = x.Children.ToString(),
                                       Infants = x.Infants.ToString(),
                                       AdultBasicFare = x.AdultBasicFare.ToString(),
                                       ChildBasicFare = x.ChildBasicFare.ToString(),
                                       InfantBasicFare = x.InfantBasicFare.ToString(),
                                       AdultTax = x.AdultTax.ToString(),
                                       ChildTax = x.ChildTax.ToString(),
                                       InfantTax = x.InfantTax.ToString(),
                                       TotalFare = x.TotalFare.ToString(),
                                       TotalTaxes = x.TotalTaxes.ToString(),
                                       GrandTotal = x.GrandTotal.ToString(),
                                       CustomerComission = x.CustomerComission.ToString(),
                                       CustomerOComission = x.CustomerOComission.ToString(),
                                       AirlineComission = x.AirlineComission.ToString(),
                                       Incentive = x.Incentive.ToString(),
                                       IncentiveScale = x.IncentiveScale.ToString(),
                                       BookingDate = x.BookingDate.ToString(),
                                       BookingStatus = x.BookingStatus.ToString(),
                                       BookingTypeID = x.BookingTypeID.ToString(),
                                       LeadFirstName = x.LeadFirstName.ToString(),
                                       LeadLastName = x.LeadLastName.ToString(),
                                       Email = x.Email.ToString(),
                                       PrimaryPhone = x.PrimaryPhone.ToString(),
                                       SecondaryPhone = x.SecondaryPhone.ToString()
                                   }).ToList();

                //var fs = sql.FlightSegments.Where(f => f.FlightBookingID == bookingID).Select(f => f.ServiceClassID).FirstOrDefault();


                var flightSegments = (from x in sql.FlightSegments.AsEnumerable()
                                      where x.FlightBookingID == bookingID
                                      select new FlightSegments2
                                      {
                                          ArrivalDate = x.ArrivalDate.ToString(),
                                          ArrivalTime = x.ArrivalTime.ToString(),
                                          ArrivalGMTOffset = x.ArrivalGMTOffset.ToString(),
                                          DepartureDate = x.DepartureDate.ToString(),
                                          DepartureTime = x.DepartureTime.ToString(),
                                          DepartureGMTOffset = x.DepartureGMTOffset.ToString(),
                                          ServiceClassID = x.ServiceClassID.ToString(),
                                          OriginID = x.OriginID.ToString(),
                                          DestinationID = x.DestinationID.ToString(),
                                          FlightNumber = x.FlightNumber.ToString(),
                                          MarketingAirlineCode = x.MarketingAirlineCode.ToString(),
                                          OperatingAirlineCode = x.OperatingAirlineCode.ToString(),
                                          MarketingAirlineID = x.MarketingAirlineID.ToString(),
                                          OperatingAirlineID = x.OperatingAirlineID.ToString(),
                                          OperatingAirlineName = x.OperatingAirlineName.ToString(),
                                          FlightBookingID = x.FlightBookingID.ToString(),
                                          ETicket = x.ETicket.ToString(),
                                          ResBookDesigCode = x.ResBookDesigCode.ToString(),
                                          Way = x.Way.ToString()
                                      }).ToList();

                var PaxList = (from x in sql.PassengerDetails.AsEnumerable()
                               where x.FlightBookingID == bookingID
                               select new PaxList
                               {
                                   Title = string.IsNullOrWhiteSpace(x.Title) ? "" : x.Title.ToString(),
                                   FirstName = string.IsNullOrWhiteSpace(x.FirstName) ? "" : x.FirstName.ToString(),
                                   LastName = string.IsNullOrWhiteSpace(x.LastName) ? "" : x.LastName.ToString(),
                                   DOB = string.IsNullOrWhiteSpace(x.DOB.ToString()) ? "" : x.DOB.ToString(),
                                   Gender = x.Gender.ToString(),
                                   Document = string.IsNullOrWhiteSpace(x.Document) ? "" : x.Document.ToString(),
                                   TicketNo = string.IsNullOrWhiteSpace(x.TicketNo) ? "" : x.TicketNo.ToString(),
                                   DocumentType = string.IsNullOrWhiteSpace(x.DocumentType) ? "" : x.DocumentType.ToString(),
                                   DOE = string.IsNullOrWhiteSpace(x.DOE.ToString()) ? "" : x.DOE.ToString(),
                                   FlightBookingID = string.IsNullOrWhiteSpace(x.FlightBookingID.ToString()) ? "" : x.FlightBookingID.ToString(),
                                   PassengerStatus = string.IsNullOrWhiteSpace(x.PassengerStatus.ToString()) ? "" : x.PassengerStatus.ToString(),
                                   PassegerType = string.IsNullOrWhiteSpace(x.PassegerType) ? "" : x.PassegerType.ToString()
                               }).ToList();

                var PNR = (from x in sql.AirPricingInfoKeys.AsEnumerable()
                           where x.BookingId == bookingID
                           select new AirPricingInfoKey2
                           {
                               Id = x.Id,
                               PNR = x.PNR,
                               PnrForTicket = x.PnrForTicket,
                               airPricingInfoKey = x.airPricingInfoKey,
                               BookingId = x.BookingId
                           }).ToList();


                //var container = new FinalBooking
                //{
                //    Id = bookingID,
                //    Reservation = Reservation,
                //    flightSegments = flightSegments,
                //    PaxList = PaxList,
                //    PNR = PNR,
                //};

                //return Ok(container);

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        [NonAction]
        private string CanelBooking(string orderID)
        {
            try
            {
                MexCoreWebAPI.Model.EncryptionManager enc = new MexCoreWebAPI.Model.EncryptionManager();
                string de = enc.Decrypt("tm/Qojgg8CqtjTJfUGJpLfvuFPW87JNQqw7gkDHqHmk=");
                string TA = enc.Encrypt("sw86n9&U");

                StringBuilder strReq = new StringBuilder();
                Guid TraceId = Guid.NewGuid();

                strReq.Append(@"<UniversalRecordCancelReq xmlns='http://www.travelport.com/schema/air_v42_0'");
                strReq.Append(@" TraceId='" + TraceId + "' AuthorizedBy = 'Travelport' TargetBranch = 'P7087680'");
                strReq.Append(@" UniversalRecordLocatorCode='" + orderID + "' Version='1' />");
                strReq.Append(@"<BillingPointOfSaleInfo xmlns='http://www.travelport.com/schema/air_v42_0' OriginApplication='uAPI'/>");
                strReq.Append(@"</UniversalRecordCancelReq>");


                return strReq.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        [NonAction]
        private string OrderTicketVoid(string orderID)
        {
            try
            {
                MexCoreWebAPI.Model.EncryptionManager enc = new MexCoreWebAPI.Model.EncryptionManager();
                string de = enc.Decrypt("tm/Qojgg8CqtjTJfUGJpLfvuFPW87JNQqw7gkDHqHmk=");
                string TA = enc.Encrypt("sw86n9&U");

                StringBuilder strReq = new StringBuilder();
                Guid TraceId = Guid.NewGuid();

                strReq.Append(@"<soapenv:Envelope xmlns:soapenv = 'http://schemas.xmlsoap.org/soap/envelope/' xmlns = 'http://www.iata.org/IATA/2015/00/2020.1/IATA_OrderChangeRQ'>");
                strReq.Append(@"<soapenv:Header/>");
                strReq.Append(@"<soapenv:Body>");
                strReq.Append(@"<IATA_OrderChangeRQ>");
                strReq.Append(@"<Party>");
                strReq.Append(@"<Sender>");
                strReq.Append(@"<TravelAgency>");
                strReq.Append(@"<AgencyID>PSA2746366</AgencyID>");
                strReq.Append(@"<Name>NDC GATEWAY</Name>");
                strReq.Append(@"</TravelAgency>");
                strReq.Append(@"</Sender>");
                strReq.Append(@"</Party>");
                strReq.Append(@"<PayloadAttributes>");
                strReq.Append(@"<PrimaryLangID>EN</PrimaryLangID>");
                strReq.Append(@"</PayloadAttributes>");
                strReq.Append(@"<Request>");
                strReq.Append(@"<Order>");
                strReq.Append(@"<OrderID>" + orderID + "</OrderID>");
                strReq.Append(@"<OwnerCode>PK</OwnerCode>");
                strReq.Append(@"</Order>");
                strReq.Append(@"<OrderChangeParameters>");
                strReq.Append(@"<CurParameter>");
                strReq.Append(@"<CurCode>PKR</CurCode>");
                strReq.Append(@"</CurParameter>");
                strReq.Append(@"</OrderChangeParameters>");
                strReq.Append(@"</Request>");
                strReq.Append(@"</IATA_OrderChangeRQ>");
                strReq.Append(@"</soapenv:Body >");
                strReq.Append(@"</soapenv:Envelope>");

                return strReq.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [NonAction]
        static int CalculatePaxAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year;

            return Years;// String.Format("Age: {0} Year(s) {1} Month(s) {2} Day(s) {3} Hour(s) {4} Second(s)", Years, Months, Days, Hours, Seconds);
        }

        //[HttpPost("MakeFlightBooking")]
        [NonAction]

        private string MakeFlightBooking()
        {
            string result = string.Empty;
            //int count = 0;
            var frm = JsonConvert.DeserializeObject<BookingInfo>(HttpContext.Session.GetString("info"));
            // BookingInfo frm = (BookingInfo)(value);
            //   FormCollection frm = new FormCollection(System.Collections.Specialized.NameValueCollection collection);
            //var sessionValue = HttpContext.Session.GetString("info");

            if (frm != null)
            {
                DateTime dt = DateTime.Now;
                DateTime dtOut;
                string sex = "";
                string title = "";
                string firstName = "";
                string lastName = "";
                string dob = "";
                string nationality = "";
                string documenttype = "";
                string document = "";
                string expiry = "";


                Random random = new Random();
                string num = random.Next(9).ToString();



                //  int UserID = user.intUserID;
                int Adults = frm.Adults != 0 ? Convert.ToInt32(frm.Adults) : 0;
                int Childs = frm.Children != 0 ? Convert.ToInt32(frm.Children) : 0;
                int Infants = frm.Infants != 0 ? Convert.ToInt32(frm.Infants) : 0;

                string from = frm.from;
                string to = frm.to;
                string totaltime0 = frm.totaltime0;
                string totaltime1 = frm.totaltime1;
                string stops0 = frm.stops0;
                string stops1 = frm.stops1;
                string amount = frm.GrandPrice;
                string taxes = frm.TotalTaxes;
                string total = frm.TotalPrice;
                string currency = frm.currency0;
                string serviceclass = frm.serviceclass0;

                try
                {
                    string securitytoken = ""; // Session["securitytoken"].ToString();
                    string convid = ""; // Session["convid"].ToString();
                    string ipcc = "";

                    DateTime datet = DateTime.Now.Date;
                    string Dat = datet.ToString("yyyy-MM-dd");

                    DateTime dt1 = DateTime.UtcNow;
                    string tstamp = dt1.ToString("s") + "Z";
                    string AirlineCode = frm.AirlineCode;
                    bool error = false;
                    string email = !string.IsNullOrEmpty(frm.email) ? frm.email.ToString() : "";
                    string contact = !string.IsNullOrEmpty(frm.MobileNO) ? frm.MobileNO.ToString() : !string.IsNullOrEmpty(frm.LandlineNo) ? frm.LandlineNo.ToString() : "";

                    string address = "";
                    string cityname = "";
                    string agencyname = "";
                    string postalcode = "";
                    string phonenumber = "";
                    string companyemail = "";
                    //int CustomerID = 0; // Convert.ToInt32(Session["CustomerID"]);
                    //var customers = sql.Customers.Where(x => x.CustomerID == CustomerID).FirstOrDefault();
                    //if (customers != null)
                    //{
                    //    companyemail = customers.Email;
                    //    postalcode = customers.PostalCode;
                    //    agencyname = customers.CompanyName.Replace("&", "and");
                    //    if (customers.AddressLine1.Length > 25)
                    //    {
                    //        address = !string.IsNullOrEmpty(customers.StreetAddress) ? customers.StreetAddress.Replace("&", "and") : !string.IsNullOrEmpty(customers.AddressLine1) ? customers.AddressLine1.Substring(0, 25).Replace("&", "and") : "";

                    //    }
                    //    else
                    //    {
                    //        address = !string.IsNullOrEmpty(customers.StreetAddress) ? customers.StreetAddress.Replace("&", "and") : !string.IsNullOrEmpty(customers.AddressLine1) ? customers.AddressLine1.Replace("&", "and") : "";
                    //    }
                    //    phonenumber = !string.IsNullOrEmpty(customers.Telephone) ? customers.Telephone : "";


                    //    int cityid = customers.CityID ?? 1;
                    //    cityname = sql.Cities.Where(x => x.CityID == cityid).Count() > 0 ? sql.Cities.Where(x => x.CityID == cityid).Select(x => x.CityName).FirstOrDefault() : "";
                    //}

                    #region XML Request

                    StringBuilder strReq = new StringBuilder();
                    StringBuilder strAdultPerson = new StringBuilder();
                    StringBuilder strChildPerson = new StringBuilder();
                    StringBuilder strInfantPerson = new StringBuilder();
                    StringBuilder strInfantPerson0 = new StringBuilder();
                    StringBuilder strInfantSSR = new StringBuilder();
                    strReq.Append(@"<SOAP-ENV:Envelope ");
                    strReq.Append("xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' ");
                    strReq.Append("xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding/' ");
                    strReq.Append("xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' ");
                    strReq.Append("xmlns:xsd='http://www.w3.org/2001/XMLSchema'> ");
                    strReq.Append("<SOAP-ENV:Header>");
                    strReq.Append("<m:MessageHeader xmlns:m='http://www.ebxml.org/namespaces/messageHeader'>");
                    strReq.Append("<m:From>");
                    strReq.Append("<m:PartyId type='urn:x12.org:IO5:01'>malikexpress.com</m:PartyId>");
                    strReq.Append("</m:From>");
                    strReq.Append("<m:To>");
                    strReq.Append("<m:PartyId type='urn:x12.org:IO5:01'>http://webservices.sabre.com/sabreXML/2011/10</m:PartyId>");
                    strReq.Append("</m:To>");
                    strReq.Append("<m:CPAId>" + ipcc + "</m:CPAId>");
                    strReq.Append("<m:ConversationId>" + convid + "</m:ConversationId>");
                    strReq.Append("<m:Service m:type='OTA'>Air Shopping Service</m:Service>");
                    strReq.Append("<m:Action>PassengerDetailsRQ</m:Action>");
                    strReq.Append("<m:MessageData>");
                    strReq.Append("<m:MessageId>mid:20001209-136453-2333@clientofsabre.com</m:MessageId>");
                    strReq.Append("<m:Timestamp>" + tstamp + "</m:Timestamp>");
                    strReq.Append("<m:TimeToLive>" + tstamp + "</m:TimeToLive>");
                    strReq.Append("</m:MessageData>");
                    strReq.Append("<m:DuplicateElimination/>");
                    strReq.Append("<m:Description>Passenger Details</m:Description>");
                    strReq.Append("</m:MessageHeader>");
                    strReq.Append("<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext'>");
                    strReq.Append(@"<wsse:BinarySecurityToken valueType='String' EncodingType='wsse:Base64Binary'>" + securitytoken + "</wsse:BinarySecurityToken>");
                    strReq.Append("</wsse:Security>");
                    strReq.Append("</SOAP-ENV:Header>");
                    strReq.Append("<SOAP-ENV:Body>");
                    //strReq.Append(xmlBody);
                    strReq.Append("<PassengerDetailsRQ xmlns='http://services.sabre.com/sp/pd/v3_2' version='3.2.0' IgnoreOnError='false' HaltOnError='false'>");

                    strReq.Append("<PostProcessing IgnoreAfter='false' RedisplayReservation='false' UnmaskCreditCard='true'>");

                    strReq.Append("<EndTransactionRQ>");
                    strReq.Append("<EndTransaction Ind='true'>");
                    strReq.Append("<Email Ind='true'>");
                    // strReq.Append("<Invoice Ind='true'/>");
                    strReq.Append("</Email>");
                    strReq.Append("</EndTransaction>");
                    strReq.Append("<Source ReceivedFrom='SWS'/>");
                    strReq.Append("</EndTransactionRQ>");

                    strReq.Append("</PostProcessing>");
                    strReq.Append("<PriceQuoteInfo>");
                    for (int i = 1; i <= Adults; i++)
                    {

                        strReq.Append("<Link HostedCarrier='false' NameNumber='" + i + ".1' Record='1'/>");

                    }

                    for (int i = 1; i <= Childs; i++)
                    {

                        strReq.Append("<Link HostedCarrier='false' NameNumber='" + (i + Adults) + ".1' Record='2'/>");

                    }
                    for (int i = 1; i <= Infants; i++)
                    {

                        strReq.Append("<Link HostedCarrier='false' NameNumber='" + (i + Adults + Childs) + ".1' Record='3'/>");

                    }
                    strReq.Append("</PriceQuoteInfo>");

                    strReq.Append("<SpecialReqDetails>");

                    strReq.Append("<SpecialServiceRQ>");
                    strReq.Append("<SpecialServiceInfo>");
                    int adultnamenumber = 1;
                    int childnamenumber = 1;
                    int infantnamenumber = 1;
                    for (int i = 1; i <= Adults; i++)
                    {
                        sex = frm.Gender;
                        document = frm.dob.ToString().ToUpper();
                        documenttype = frm.documenttype.ToString().ToUpper();
                        expiry = Convert.ToDateTime(frm.expiry).ToString("yyyy-MM-dd");
                        nationality = frm.nationality.ToString();
                        dob = Convert.ToDateTime(frm.dob).ToString("yyyy-MM-dd");
                        title = frm.title.ToString().ToUpper();
                        firstName = frm.firstName.ToString().ToUpper();
                        lastName = frm.lastName.ToString().ToUpper();

                        strReq.Append("<AdvancePassenger SegmentNumber='A'>");
                        strReq.Append("<Document ExpirationDate='" + expiry + "' Number='" + document + "' Type='" + documenttype + "'>");
                        strReq.Append("<IssueCountry>" + nationality + "</IssueCountry>");
                        strReq.Append("<NationalityCountry>" + nationality + "</NationalityCountry>");
                        strReq.Append("</Document>");
                        strReq.Append("<PersonName DateOfBirth='" + dob + "' Gender='" + sex + "' NameNumber='" + adultnamenumber + ".1' DocumentHolder='true'>");
                        strReq.Append("<GivenName>" + firstName + " " + title + "</GivenName>");
                        strReq.Append("<Surname>" + lastName + "</Surname>");
                        strReq.Append("</PersonName>");
                        strReq.Append("<VendorPrefs>");
                        strReq.Append("<Airline Hosted='false'/>");
                        strReq.Append("</VendorPrefs>");
                        strReq.Append("</AdvancePassenger>");

                        strAdultPerson.Append("<PersonName Infant='false' NameNumber='" + adultnamenumber + ".1' PassengerType='ADT'>");
                        strAdultPerson.Append("<GivenName>" + firstName + " " + title + "</GivenName>");
                        strAdultPerson.Append("<Surname>" + lastName + "</Surname>");
                        strAdultPerson.Append("</PersonName>");

                        if (Infants >= adultnamenumber)
                        {
                            sex = frm.Gender;
                            document = frm.dob.ToString().ToUpper();
                            documenttype = frm.documenttype.ToString().ToUpper();
                            expiry = Convert.ToDateTime(frm.expiry).ToString("yyyy-MM-dd");
                            nationality = frm.nationality.ToString();
                            dob = Convert.ToDateTime(frm.dob).ToString("yyyy-MM-dd");
                            title = frm.title.ToString().ToUpper();
                            firstName = frm.firstName.ToString().ToUpper();
                            lastName = frm.lastName.ToString().ToUpper();
                            DateTime dtDOB = Convert.ToDateTime(frm.dob);
                            TimeSpan t = DateTime.Now - dtDOB;
                            int totalDays = (int)t.TotalDays;
                            int months = totalDays / 30;
                            if (DateTime.Now.AddMonths(-24) <= dtDOB)
                            {
                                string dtDy = dtDOB.ToString("dd");
                                string dtMon = dtDOB.ToString("MMM").ToUpper();
                                string dtyear = dtDOB.ToString("yy");
                                string childage = months.ToString();
                                if (months < 10)
                                {
                                    childage = "0" + months.ToString();
                                }
                                strInfantPerson0.Append("<AdvancePassenger SegmentNumber='A'>");
                                strInfantPerson0.Append("<Document ExpirationDate='" + expiry + "' Number='" + document + "' Type='" + documenttype + "'>");
                                strInfantPerson0.Append("<IssueCountry>" + nationality + "</IssueCountry>");
                                strInfantPerson0.Append("<NationalityCountry>" + nationality + "</NationalityCountry>");
                                strInfantPerson0.Append("</Document>");
                                strInfantPerson0.Append("<PersonName DateOfBirth='" + dob + "' Gender='" + sex + "I' NameNumber='" + infantnamenumber + ".1' DocumentHolder='true'>");
                                strInfantPerson0.Append("<GivenName>" + firstName + " " + title + "</GivenName>");
                                strInfantPerson0.Append("<Surname>" + lastName + "</Surname>");
                                strInfantPerson0.Append("</PersonName>");
                                strInfantPerson0.Append("<VendorPrefs>");
                                strInfantPerson0.Append("<Airline Hosted='false'/>");
                                strInfantPerson0.Append("</VendorPrefs>");
                                strInfantPerson0.Append("</AdvancePassenger>");

                                strInfantSSR.Append("<Service SSR_Code='INFT'> ");
                                strInfantSSR.Append("<PersonName NameNumber='" + adultnamenumber + ".1'/> ");
                                strInfantSSR.Append("<Text>" + lastName + "/" + firstName + " " + title + "/" + dtDy + dtMon + dtyear + "</Text>");
                                strInfantSSR.Append("<VendorPrefs> <Airline Hosted='false'/>");
                                strInfantSSR.Append("</VendorPrefs>");
                                strInfantSSR.Append("</Service>");

                                strInfantPerson.Append("<PersonName Infant='true' NameNumber='" + (Adults + Childs + adultnamenumber) + ".1'  NameReference='I" + childage + "' PassengerType='INF'>");
                                strInfantPerson.Append("<GivenName>" + firstName + " " + title + "</GivenName>");
                                strInfantPerson.Append("<Surname>" + lastName + "</Surname>");
                                strInfantPerson.Append("</PersonName>");


                            }
                            else
                            {
                                error = true;
                                //////ViewBag.Error += "Infant Age should be less than 24 months.";
                            }
                        }

                        adultnamenumber++;

                    }
                    childnamenumber = adultnamenumber;
                    for (int i = 1; i <= Childs; i++)
                    {
                        sex = frm.Gender;
                        document = frm.dob.ToString().ToUpper();
                        documenttype = frm.documenttype.ToString().ToUpper();
                        expiry = Convert.ToDateTime(frm.expiry).ToString("yyyy-MM-dd");
                        nationality = frm.nationality.ToString();
                        dob = Convert.ToDateTime(frm.dob).ToString("yyyy-MM-dd");
                        title = frm.title.ToString().ToUpper();
                        firstName = frm.firstName.ToString().ToUpper();
                        lastName = frm.lastName.ToString().ToUpper();
                        DateTime dtDOB = Convert.ToDateTime(frm.dob);
                        TimeSpan t = DateTime.Now - dtDOB;
                        int totalDays = (int)t.TotalDays;
                        int years = totalDays / 365;
                        if (DateTime.Now.AddYears(-11) <= dtDOB && DateTime.Now.AddYears(-2) >= dtDOB)
                        {
                            string childage = years.ToString();
                            if (years < 10)
                            {
                                childage = "0" + years.ToString();
                            }

                            strReq.Append("<AdvancePassenger SegmentNumber='A'>");
                            strReq.Append("<Document ExpirationDate='" + expiry + "' Number='" + document + "' Type='" + documenttype + "'>");
                            strReq.Append("<IssueCountry>" + nationality + "</IssueCountry>");
                            strReq.Append("<NationalityCountry>" + nationality + "</NationalityCountry>");
                            strReq.Append("</Document>");
                            strReq.Append("<PersonName DateOfBirth='" + dob + "' Gender='" + sex + "' NameNumber='" + childnamenumber + ".1' DocumentHolder='true'>");
                            strReq.Append("<GivenName>" + firstName + "</GivenName>");
                            strReq.Append("<Surname>" + lastName + "</Surname>");
                            strReq.Append("</PersonName>");
                            strReq.Append("<VendorPrefs>");
                            strReq.Append("<Airline Hosted='false'/>");
                            strReq.Append("</VendorPrefs>");
                            strReq.Append("</AdvancePassenger>");

                            strChildPerson.Append("<PersonName Infant='false' NameNumber='" + adultnamenumber + ".1' NameReference='C" + childage + "' PassengerType='CNN'>");
                            strChildPerson.Append("<GivenName>" + firstName + " " + title + "</GivenName>");
                            strChildPerson.Append("<Surname>" + lastName + "</Surname>");
                            strChildPerson.Append("</PersonName>");
                            childnamenumber++;
                        }
                        else
                        {
                            error = true;
                            ////ViewBag.Error += "Child Age should be less than 11 years and should be greater than 2 years.";

                        }
                    }
                    if (Infants > 0 && strInfantPerson0 != null)
                    {

                        strReq.Append(strInfantPerson0.ToString());
                        strReq.Append(strInfantSSR.ToString());

                    }



                    strReq.Append("</SpecialServiceInfo>");
                    strReq.Append("</SpecialServiceRQ>");

                    strReq.Append("</SpecialReqDetails>");

                    strReq.Append("<TravelItineraryAddInfoRQ>");

                    strReq.Append("<AgencyInfo>");
                    strReq.Append("<Address>");
                    if (agencyname != "")
                    {
                        strReq.Append("<AddressLine>" + agencyname + "</AddressLine>");
                    }
                    else
                    {
                        strReq.Append("<AddressLine>MALIK EXPRESS</AddressLine>");
                    }
                    if (!string.IsNullOrEmpty(cityname))
                    {
                        strReq.Append("<CityName>" + cityname + "</CityName>");
                    }
                    else
                    {
                        strReq.Append("<CityName>ISLAMABAD</CityName>");
                    }

                    strReq.Append("<CountryCode>PK</CountryCode>");
                    if (postalcode != "")
                    {
                        strReq.Append("<PostalCode>" + postalcode + "</PostalCode>");
                    }
                    else
                    {
                        strReq.Append("<PostalCode>44000</PostalCode>");
                    }

                    strReq.Append("<StateCountyProv StateCode='PK'/>");
                    if (address != "")
                    {

                        strReq.Append("<StreetNmbr>" + address + "</StreetNmbr>");
                    }
                    else
                    {
                        strReq.Append("<StreetNmbr>I-2 JAVED</StreetNmbr>");
                    }

                    strReq.Append("<VendorPrefs>");
                    strReq.Append("<Airline Hosted='false'/>");
                    strReq.Append("</VendorPrefs>");
                    strReq.Append("</Address>");
                    strReq.Append("<Ticketing PseudoCityCode='" + ipcc + "' TicketType='7TAW' />");
                    strReq.Append("</AgencyInfo>");

                    strReq.Append("<CustomerInfo>");
                    strReq.Append("<ContactNumbers>");
                    //if (phonenumber != "")
                    //{
                    //    strReq.Append("<ContactNumber LocationCode='ISB' NameNumber='1.1' Phone='" + phonenumber + "' PhoneUseType='A'/>");
                    //}

                    strReq.Append("<ContactNumber LocationCode='ISB' NameNumber='1.1' Phone='111-778-800' PhoneUseType='B'/>");
                    strReq.Append("</ContactNumbers>");
                    strReq.Append("<Email Address='" + companyemail + "' NameNumber='1.1' Type='TO'/>");
                    strReq.Append("<Email Address='" + email + "' NameNumber='1.1' Type='CC'/>");

                    strReq.Append(strAdultPerson.ToString());
                    if (Childs > 0 && strChildPerson != null)
                    {
                        strReq.Append(strChildPerson.ToString());
                    }
                    if (Infants > 0 && strInfantPerson != null)
                    {
                        strReq.Append(strInfantPerson.ToString());
                    }


                    strReq.Append("</CustomerInfo>");

                    strReq.Append("</TravelItineraryAddInfoRQ>");

                    strReq.Append("</PassengerDetailsRQ>");
                    strReq.Append("</SOAP-ENV:Body>");
                    strReq.Append("</SOAP-ENV:Envelope>");

                    #endregion
                    //string str = strReq.ToString();
                    //XDocument req = XDocument.Parse(strReq.ToString());


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://webservices.sabre.com");
                    byte[] requestInFormOfBytes = System.Text.Encoding.ASCII.GetBytes(strReq.ToString());
                    request.Method = "POST";
                    request.ContentType = "text/xml;charset=utf-8";
                    request.ContentLength = requestInFormOfBytes.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(requestInFormOfBytes, 0, requestInFormOfBytes.Length);
                    requestStream.Close();
                    if (error == false)
                    {

                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        // string sp=response.
                        TextReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                        //string Resp = respStream.ReadToEnd();
                        //XDocument res = XDocument.Parse(Resp);



                        XmlSerializer xmlSer = new XmlSerializer(typeof(MalikExpressMVC.Models.XML.PNR.Envelope));
                        MalikExpressMVC.Models.XML.PNR.Envelope obj = (MalikExpressMVC.Models.XML.PNR.Envelope)xmlSer.Deserialize(respStream);
                        MalikExpressMVC.Models.XML.PNR.PassengerDetailsRS enObj = obj.Body.PassengerDetailsRS;


                        #region End-Transaction
                        //StringBuilder strReq_end = new StringBuilder();
                        //strReq_end.Append(@"<SOAP-ENV:Envelope ");
                        //strReq_end.Append("xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' ");
                        //strReq_end.Append("xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding/' ");
                        //strReq_end.Append("xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' ");
                        //strReq_end.Append("xmlns:xsd='http://www.w3.org/2001/XMLSchema'> ");
                        //strReq_end.Append("<SOAP-ENV:Header>");
                        //strReq_end.Append("<m:MessageHeader xmlns:m='http://www.ebxml.org/namespaces/messageHeader'>");
                        //strReq_end.Append("<m:From>");
                        //strReq_end.Append("<m:PartyId type='urn:x12.org:IO5:01'>malikexpress.com</m:PartyId>");
                        //strReq_end.Append("</m:From>");
                        //strReq_end.Append("<m:To>");
                        //strReq_end.Append("<m:PartyId type='urn:x12.org:IO5:01'>http://webservices.sabre.com/sabreXML/2011/10</m:PartyId>");
                        //strReq_end.Append("</m:To>");
                        //strReq_end.Append("<m:CPAId>" + ipcc + "</m:CPAId>");
                        //strReq_end.Append("<m:ConversationId>" + convid + "</m:ConversationId>");
                        //strReq_end.Append("<m:Service m:type='OTA'>Air Shopping Service</m:Service>");
                        //strReq_end.Append("<m:Action>PassengerDetailsRQ</m:Action>");
                        //strReq_end.Append("<m:MessageData>");
                        //strReq_end.Append("<m:MessageId>mid:20001209-136453-2333@clientofsabre.com</m:MessageId>");
                        //strReq_end.Append("<m:Timestamp>" + tstamp + "</m:Timestamp>");
                        //strReq_end.Append("<m:TimeToLive>" + tstamp + "</m:TimeToLive>");
                        //strReq_end.Append("</m:MessageData>");
                        //strReq_end.Append("<m:DuplicateElimination/>");
                        //strReq_end.Append("<m:Description>Passenger Details</m:Description>");
                        //strReq_end.Append("</m:MessageHeader>");
                        //strReq_end.Append("<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext'>");
                        //strReq_end.Append(@"<wsse:BinarySecurityToken valueType='String' EncodingType='wsse:Base64Binary'>" + securitytoken + "</wsse:BinarySecurityToken>");
                        //strReq_end.Append("</wsse:Security>");
                        //strReq_end.Append("</SOAP-ENV:Header>");
                        //strReq_end.Append("<SOAP-ENV:Body>");
                        ////strReq.Append(xmlBody);
                        //strReq_end.Append("<PassengerDetailsRQ xmlns='http://services.sabre.com/sp/pd/v3_2' version='3.2.0' IgnoreOnError='false' HaltOnError='false'>");

                        //strReq_end.Append("<PostProcessing IgnoreAfter='false' RedisplayReservation='false' UnmaskCreditCard='true'>");
                        //strReq_end.Append("<EndTransactionRQ>");
                        //strReq_end.Append("<EndTransaction Ind='true'/>");
                        //strReq_end.Append("<Source ReceivedFrom='SWS'/>");
                        //strReq_end.Append("</EndTransactionRQ>");
                        //strReq_end.Append("</PostProcessing>");
                        //strReq_end.Append("</PassengerDetailsRQ>");
                        //strReq_end.Append("</SOAP-ENV:Body>");
                        //strReq_end.Append("</SOAP-ENV:Envelope>");
                        #endregion

                        if (enObj != null && enObj.ItineraryRef != null && enObj.ItineraryRef.ID != null)
                        {
                            string ID = enObj.ItineraryRef.ID;
                            //Session["PNR"] = ID;
                            int UserID = 0; // Convert.ToInt32(Session["UserID"]);

                            #region Booking Info

                            MalikExpressMVC.Models.XML.AirItineraryPricingInfo price = new MalikExpressMVC.Models.XML.AirItineraryPricingInfo(); // Session["price"];

                            int Adts = 0;
                            int Chlds = 0;
                            int inft = 0;
                            decimal Adultsfare = 0;
                            decimal Childfare = 0;
                            decimal Infatfare = 0;
                            decimal AdultTaxes = 0;
                            decimal ChildTaxes = 0;
                            decimal InfnatTaxes = 0;
                            decimal totalfare = Convert.ToDecimal(price.ItinTotalFare.EquivFare.Amount);
                            decimal totaltaxes = 0;

                            foreach (var tax in price.ItinTotalFare.Taxes.Tax)
                            {
                                totaltaxes += Convert.ToDecimal(tax.Amount);
                            }

                            decimal grandtotal = Convert.ToDecimal(price.ItinTotalFare.TotalFare.Amount);
                            foreach (var fare in price.PTC_FareBreakdowns.PTC_FareBreakdown)
                            {
                                if (fare.PassengerTypeQuantity.Code == "ADT")
                                {
                                    Adts = Convert.ToInt32(fare.PassengerTypeQuantity.Quantity);
                                    Adultsfare = Convert.ToDecimal(fare.PassengerFare.EquivFare.Amount);
                                    AdultTaxes = Convert.ToDecimal(fare.PassengerFare.Taxes.TotalTax.Amount);
                                    //foreach (var ta in fare.PassengerFare.Taxes.Tax)
                                    //{
                                    //    AdultTaxes += Convert.ToDecimal(ta.Amount);
                                    //}
                                }
                                if (fare.PassengerTypeQuantity.Code == "CNN")
                                {
                                    Chlds = Convert.ToInt32(fare.PassengerTypeQuantity.Quantity);
                                    Childfare = Convert.ToDecimal(fare.PassengerFare.EquivFare.Amount);
                                    ChildTaxes = Convert.ToDecimal(fare.PassengerFare.Taxes.TotalTax.Amount);
                                    //foreach (var ta in fare.PassengerFare.Taxes.Tax)
                                    //{
                                    //    ChildTaxes += Convert.ToDecimal(ta.Amount);
                                    //}
                                }
                                if (fare.PassengerTypeQuantity.Code == "INF")
                                {
                                    inft = Convert.ToInt32(fare.PassengerTypeQuantity.Quantity);
                                    Infatfare = Convert.ToDecimal(fare.PassengerFare.EquivFare.Amount);
                                    InfnatTaxes = fare.PassengerFare != null && fare.PassengerFare.Taxes.TotalTax != null ? Convert.ToDecimal(fare.PassengerFare.Taxes.TotalTax.Amount) : 0;
                                    //foreach (var ta in fare.PassengerFare.Taxes.Tax)
                                    //{
                                    //    InfnatTaxes += Convert.ToDecimal(ta.Amount);
                                    //}
                                }
                            }
                            DateTime dttrans = DateTime.Now.ToUniversalTime();
                            string leadfirstname = !string.IsNullOrEmpty(frm.firstName) ? frm.firstName.ToString().ToUpper() : "";
                            string leadlastname = !string.IsNullOrEmpty(frm.lastName) ? frm.lastName.ToString().ToUpper() : "";

                            decimal[] TotalAmt = _AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Childs, Infants, AirlineCode);

                            totalfare = TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                            grandtotal = totalfare + totaltaxes + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                            if (Adults > 0) { Adultsfare = TotalAmt[2] / Adults; }
                            if (Childs > 0) { Childfare = TotalAmt[3] / Childs; }
                            if (Infants > 0) { Infatfare = TotalAmt[4] / Infants; }


                            //List<OriginDestinationOption> odSegments = (List<OriginDestinationOption>)Session["odSegments"];

                            //int BookingTypeID = odSegments.Count();

                            //string Scale = "";
                            //decimal Incentives = 0;
                            //if (Session["Incentives"] != null)
                            //{
                            //    HashSet<Incentive> inc = (HashSet<Incentive>)Session["Incentives"];
                            //    var FlightIncentives = inc.Where(x => x.ServiceID == 2).FirstOrDefault();

                            //    Incentives = FlightIncentives.Incentives ?? 0;
                            //    Scale = FlightIncentives.Scale;
                            //}


                            //Reservation flight = new Reservation()
                            //{
                            //    Adults = Adults,
                            //    Children = Childs,
                            //    Infants = Infants,
                            //    PNR = ID,
                            //    AdultBasicFare = Adultsfare,
                            //    ChildBasicFare = Childfare,
                            //    InfantBasicFare = Infatfare,
                            //    AdultTax = AdultTaxes,
                            //    ChildTax = ChildTaxes,
                            //    InfantTax = InfnatTaxes,
                            //    TotalFare = totalfare,
                            //    TotalTaxes = totaltaxes,
                            //    GrandTotal = grandtotal,
                            //    CustomerComission = TotalAmt[0],
                            //    CustomerOComission = TotalAmt[1],
                            //    AirlineComission = TotalAmt[5],
                            //    Incentive = 0,
                            //    IncentiveScale = "",
                            //    BookingDate = DateTime.Now.ToUniversalTime(),
                            //    BookingStatus = "OK",
                            //    BookingTypeID = 0,
                            //    LeadFirstName = leadfirstname,
                            //    LeadLastName = leadlastname,
                            //    UserID =1,
                            //    Email = email,
                            //    PrimaryPhone = !string.IsNullOrEmpty(frm.mobile"]) ? frm.mobile"].ToString() : "",
                            //    SecondaryPhone = !string.IsNullOrEmpty(frm.phone"]) ? frm.phone"].ToString() : ""
                            //};
                            //sql.Reservations.Add(flight);
                            //sql.SaveChanges();
                            //int BookingID = flight.Id;

                            //#region Pax Info

                            //for (int i = 1; i <= Adults; i++)
                            //{
                            //    bool sex1 = false;
                            //    DateTime dtExpiry = Convert.ToDateTime(frm.expiry_" + i]);
                            //    sex = frm.sex_" + i].ToString();
                            //    document = frm.document_" + i].ToString().ToUpper();
                            //    documenttype = frm.documenttype_" + i].ToString().ToUpper();
                            //    expiry = Convert.ToDateTime(frm.expiry_" + i]).ToString("yyyy-MM-dd");
                            //    nationality = frm.nationality_" + i].ToString();
                            //    dob = Convert.ToDateTime(frm.dob_" + i]).ToString("yyyy-MM-dd");
                            //    title = frm.title_" + i].ToString().ToUpper();
                            //    firstName = frm.firstName_" + i].ToString().ToUpper();
                            //    lastName = frm.lastName_" + i].ToString().ToUpper();
                            //    if (sex == "M") { sex1 = true; }
                            //    PassengerDetail passengerDetail = new PassengerDetail()
                            //    {
                            //        Title = title,
                            //        FirstName = firstName,
                            //        LastName = lastName,
                            //        DOB = Convert.ToDateTime(dob),
                            //        Gender = sex1,
                            //        Document = document,
                            //        DocumentType = documenttype,
                            //        DOE = dtExpiry,
                            //        FlightBookingID = BookingID,
                            //        PassengerStatus = 3,
                            //        PassegerType = "ADT"
                            //    };

                            //    sql.PassengerDetails.Add(passengerDetail);
                            //    sql.SaveChanges();

                            //}

                            //for (int i = 1; i <= Childs; i++)
                            //{
                            //    bool sex1 = false;
                            //    DateTime dtExpiry = Convert.ToDateTime(frm.expiryC_" + i]);
                            //    sex = frm.sexC_" + i].ToString();
                            //    document = frm.documentC_" + i].ToString().ToUpper();
                            //    documenttype = frm.documenttypeC_" + i].ToString().ToUpper();
                            //    expiry = Convert.ToDateTime(frm.expiryC_" + i]).ToString("yyyy-MM-dd");
                            //    nationality = frm.nationalityC_" + i].ToString();
                            //    dob = Convert.ToDateTime(frm.dobC_" + i]).ToString("yyyy-MM-dd");
                            //    title = frm.titleC_" + i].ToString().ToUpper();
                            //    firstName = frm.firstNameC_" + i].ToString().ToUpper();
                            //    lastName = frm.lastNameC_" + i].ToString().ToUpper();
                            //    if (sex == "M") { sex1 = true; }
                            //    PassengerDetail passengerDetail = new PassengerDetail()
                            //    {
                            //        Title = title,
                            //        FirstName = firstName,
                            //        LastName = lastName,
                            //        DOB = Convert.ToDateTime(dob),
                            //        Gender = sex1,
                            //        Document = document,
                            //        DocumentType = documenttype,
                            //        DOE = dtExpiry,
                            //        FlightBookingID = BookingID,
                            //        PassengerStatus = 3,
                            //        PassegerType = "CNN"



                            //    };

                            //    sql.PassengerDetails.Add(passengerDetail);
                            //    sql.SaveChanges();

                            //}

                            //for (int i = 1; i <= Infants; i++)
                            //{
                            //    bool sex1 = false;
                            //    DateTime dtExpiry = Convert.ToDateTime(frm.expiryI_" + i]);
                            //    sex = frm.sexI_" + i].ToString();
                            //    document = frm.documentI_" + i].ToString().ToUpper();
                            //    documenttype = frm.documenttypeI_" + i].ToString().ToUpper();
                            //    expiry = Convert.ToDateTime(frm.expiryI_" + i]).ToString("yyyy-MM-dd");
                            //    nationality = frm.nationalityI_" + i].ToString();
                            //    dob = Convert.ToDateTime(frm.dobI_" + i]).ToString("yyyy-MM-dd");
                            //    title = frm.titleI_" + i].ToString().ToUpper();
                            //    firstName = frm.firstNameI_" + i].ToString().ToUpper();
                            //    lastName = frm.lastNameI_" + i].ToString().ToUpper();
                            //    if (sex == "M") { sex1 = true; }
                            //    PassengerDetail passengerDetail = new PassengerDetail()
                            //    {
                            //        Title = title,
                            //        FirstName = firstName,
                            //        LastName = lastName,
                            //        DOB = Convert.ToDateTime(dob),
                            //        Gender = sex1,
                            //        Document = document,
                            //        DocumentType = documenttype,
                            //        DOE = dtExpiry,
                            //        FlightBookingID = BookingID,
                            //        PassengerStatus = 3,
                            //        PassegerType = "INF"

                            //    };

                            //    sql.PassengerDetails.Add(passengerDetail);
                            //    sql.SaveChanges();

                            //}

                            //#endregion

                            string DeptDate = "", DeptTime = "", DeptFrom = "", ArrvlTo = "";
                            bool IsDepart = false;
                            int itrCodes = 0;
                            byte segmenttype = 1;
                            string MrktingAirline = "";

                            List<MalikExpressMVC.Models.XML.OriginDestinationOption> odSegments = new List<MalikExpressMVC.Models.XML.OriginDestinationOption>(); ;// Session["odSegments"];

                            foreach (var item in odSegments)
                            {

                                foreach (var segment in item.FlightSegment)
                                {

                                    string BookingCode = segment != null ? segment.ResBookDesigCode : "";
                                    string AirEquipType = segment != null && segment.Equipment != null && segment.Equipment.AirEquipType != null ? segment.Equipment.AirEquipType : "";
                                    bool ETicket = segment != null && segment.TPA_Extensions != null && segment.TPA_Extensions.ETicket != null ? segment.TPA_Extensions.ETicket.Ind : false;
                                    DateTime DepDate = segment != null && segment.DepartureDateTime != null ? Convert.ToDateTime(segment.DepartureDateTime) : new DateTime();
                                    string DepTime = segment != null && segment.DepartureDateTime != null ? Convert.ToDateTime(segment.DepartureDateTime).ToString("hh:mm tt") : "";
                                    DateTime ArrDate = segment != null && segment.ArrivalDateTime != null ? Convert.ToDateTime(segment.ArrivalDateTime) : new DateTime();
                                    string ArrTime = segment != null && segment.ArrivalDateTime != null ? Convert.ToDateTime(segment.ArrivalDateTime).ToString("hh:mm tt") : "";

                                    string MarketingAirlineCode = segment != null && segment.MarketingAirline != null ? segment.MarketingAirline.Code : "";
                                    string OperatingAirlineCode = segment != null && segment.OperatingAirline != null ? segment.OperatingAirline.Code : "";
                                    string FlightNumber = segment.OperatingAirline.FlightNumber;
                                    string CompanyShortName = segment != null && segment.OperatingAirline != null && segment.OperatingAirline.CompanyShortName != null ? segment.OperatingAirline.CompanyShortName : "";

                                    int ServiceCls = sql.ServiceClasses.Where(x => x.ClassName == serviceclass).Select(x => x.ServiceClassID).FirstOrDefault();
                                    int Origin = sql.Cities.Where(x => x.IATA == segment.DepartureAirport.LocationCode).Select(x => x.cityID).FirstOrDefault();
                                    int Dest = sql.Cities.Where(x => x.IATA == segment.ArrivalAirport.LocationCode).Select(x => x.cityID).FirstOrDefault();
                                    int OperatingAirlineID = sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
                                    int MarketingAirlineID = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

                                    string ArrGMTOffset = segment != null && segment.ArrivalTimeZone != null && segment.ArrivalTimeZone.GMTOffset != null ? segment.ArrivalTimeZone.GMTOffset : "";
                                    string DptGMTOffset = segment != null && segment.DepartureTimeZone != null && segment.DepartureTimeZone.GMTOffset != null ? segment.DepartureTimeZone.GMTOffset : "";

                                    if (!IsDepart)
                                    {
                                        IsDepart = true;
                                        MrktingAirline = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineName).FirstOrDefault();
                                        DeptFrom = sql.Cities.Where(x => x.IATA == segment.DepartureAirport.LocationCode).Select(x => x.cityName).FirstOrDefault();
                                        ArrvlTo = sql.Cities.Where(x => x.IATA == segment.ArrivalAirport.LocationCode).Select(x => x.cityName).FirstOrDefault();

                                        DeptDate = DepDate.ToString("MMM dd, ddd");
                                        DeptTime = DepTime;

                                    }


                                    FlightSegment flightseg = new FlightSegment()
                                    {
                                        ArrivalDate = ArrDate,
                                        ArrivalTime = ArrTime,
                                        ArrivalGMTOffset = ArrGMTOffset,
                                        DepartureDate = DepDate,
                                        DepartureTime = DepTime,
                                        DepartureGMTOffset = DptGMTOffset,
                                        ServiceClassID = ServiceCls,
                                        OriginID = Origin,
                                        DestinationID = Dest,
                                        FlightNumber = FlightNumber,
                                        MarketingAirlineCode = MarketingAirlineCode,
                                        OperatingAirlineCode = OperatingAirlineCode,
                                        MarketingAirlineID = MarketingAirlineID,
                                        OperatingAirlineID = OperatingAirlineID,
                                        OperatingAirlineName = CompanyShortName,
                                        FlightBookingID = 1,
                                        AirEquipType = AirEquipType,
                                        ETicket = ETicket,
                                        ResBookDesigCode = BookingCode,
                                        Way = segmenttype
                                    };
                                    sql.FlightSegments.Add(flightseg);
                                    sql.SaveChanges();
                                    int FlightSegmentID = flightseg.FlightSegmentID;

                                    foreach (var fare in price.PTC_FareBreakdowns.PTC_FareBreakdown)
                                    {
                                        string bookingCode = fare.FareBasisCodes.FareBasisCode[itrCodes] != null ? fare.FareBasisCodes.FareBasisCode[itrCodes].BookingCode : "";
                                        string fareBasisCode = fare.FareBasisCodes.FareBasisCode[itrCodes] != null ? fare.FareBasisCodes.FareBasisCode[itrCodes].Text : "";
                                        string typeCode = fare.PassengerTypeQuantity.Code;
                                        if (bookingCode != "" && fareBasisCode != "")
                                        {
                                            MexCoreWebAPI.Model.FareBasisCode f = new MexCoreWebAPI.Model.FareBasisCode()
                                            {
                                                Code = bookingCode,
                                                Text = fareBasisCode,
                                                PaxType = typeCode,
                                                FlightSegmentID = FlightSegmentID
                                            };
                                            sql.FareBasisCodes.Add(f);
                                            sql.SaveChanges();
                                        }
                                    }
                                    itrCodes++;
                                }
                                segmenttype++;
                            }


                            //    string customermask = customers.CustomerMask;

                            //if (!string.IsNullOrEmpty(customermask) && !string.IsNullOrEmpty(frm.mobile"]) && frm.mobile"].Substring(0, 1) == "9" && frm.mobile"].Substring(1, 1) == "2" && frm.mobile"].Length == 12)
                            //{
                            //    try
                            //    {
                            //        SMSCenter sms = new SMSCenter();
                            //        if (Adults + Childs + Infants > 1)
                            //        {
                            //            string strSMSResult = sms.SendSMS(frm.mobile"].ToString(), "Hello " + leadfirstname + " " + leadlastname + "! Your group travel reservation for " + ArrvlTo + " from " + DeptFrom + " on " + DeptDate + " " + DeptTime + " has been confirmed. Please verify flight times prior to departure. Thank you for choosing Travcont Solution.", customermask);

                            //        }
                            //        else
                            //        {
                            //            string strSMSResult = sms.SendSMS(frm.mobile"].ToString(), "Hello " + leadfirstname + " " + leadlastname + "! Your travel reservation for " + ArrvlTo + " from " + DeptFrom + " on " + DeptDate + " " + DeptTime + " has been confirmed. Please verify flight times prior to departure. Thank you for choosing Travcont Solution.", customermask);
                            //        }
                            //    }
                            //    catch (Exception exe)
                            //    {


                            //    }
                            //}



                            #endregion

                            //  Session.Remove("odSegments");
                            //  Session.Remove("price");

                            return ""; // RedirectToAction("Invoice", "BookSecure");
                        }

                        else
                        {
                            string warrning = ""; ;
                            foreach (var msg in enObj.ApplicationResults.Warning.SystemSpecificResults.Message)
                            {
                                warrning += msg.Text;
                            }
                            if (warrning != "")
                            {
                                //  ////ViewBag.Error = warrning;
                            }
                            //////ViewBag.Adults = Adults;
                            //////ViewBag.Childs = Childs;
                            //////ViewBag.Infants = Infants;

                            //List<OriginDestinationOption> odSegments = (List<OriginDestinationOption>)Session["odSegments"];
                            return null; // View(odSegments);
                        }
                    }
                    else
                    {
                        //  List<OriginDestinationOption> odSegments = (List<OriginDestinationOption>)Session["odSegments"];
                        return null; // View(odSegments);
                    }
                }
                catch (Exception ex)
                {
                    // ////ViewBag.Error = ex.Message + " " + ex.StackTrace + " . Please try again.";
                    //List<OriginDestinationOption> odSegments = (List<OriginDestinationOption>)Session["odSegments"];

                    return null; // View(odSegments);
                }
            }
            else
            {
                // List<OriginDestinationOption> odSegments = (List<OriginDestinationOption>)Session["odSegments"];
                return null; // View(odSegments);
            }
            // return View();
        }


        [NonAction]
        private decimal[] _AddMarkup(decimal adultamount, decimal childamount, decimal infantamount, int adults, int children, int infants, string airlinecode,
            string userid = "-1")
        {
            decimal AdultsAmount = adultamount;
            decimal ChildsAmount = childamount;
            decimal InfantsAmount = infantamount;
            decimal[] arrInc = new decimal[6];
            decimal AirlineMarkup = 0;
            try
            {

                List<AirlineDefaultServiceFee> FlightMarkups = oldSQL.AirlineDefaultServiceFees.ToList();
                if (FlightMarkups != null)
                {
                    if (adults > 0)
                    {
                        var AdultMarkup = FlightMarkups.Where(x => x.PassengerType == "ADT").FirstOrDefault();
                        if (AdultMarkup != null)
                        {
                            if (AdultMarkup.ServiceFeeType == "Percent")
                            {
                                AirlineMarkup = ((AirlineMarkup) + (adultamount * (decimal)AdultMarkup.Amount / 100)) * adults;
                            }
                            else
                            {
                                AirlineMarkup = (AirlineMarkup + (decimal)AdultMarkup.Amount) * adults;
                            }
                        }
                    }
                    if (children > 0)
                    {
                        var ChildMarkup = FlightMarkups.Where(x => x.PassengerType == "CNN").FirstOrDefault();
                        if (ChildMarkup != null)
                        {
                            if (ChildMarkup.ServiceFeeType == "Percent")
                            {
                                AirlineMarkup = ((AirlineMarkup) + (childamount * (decimal)ChildMarkup.Amount / 100)) * children;
                            }
                            else
                            {
                                AirlineMarkup = (AirlineMarkup + (decimal)ChildMarkup.Amount) * children;
                            }
                        }

                    }
                    if (infants > 0)
                    {
                        var InfantMarkup = FlightMarkups.Where(x => x.PassengerType == "INF").FirstOrDefault();
                        if (InfantMarkup != null)
                        {
                            if (InfantMarkup.ServiceFeeType == "Percent")
                            {
                                AirlineMarkup = ((AirlineMarkup) + (infantamount * (decimal)InfantMarkup.Amount / 100)) * infants;
                            }
                            else
                            {
                                AirlineMarkup += (AirlineMarkup + (decimal)InfantMarkup.Amount) * infants;
                            }
                        }

                    }


                }

                var AirlineComission = oldSQL.AirlineSpecificServiceFees.Where(x => x.AirlineCode == airlinecode).AsEnumerable();
                if (AirlineComission != null)
                {
                    if (adults > 0)
                    {
                        var AdultComission = AirlineComission.Where(x => x.PassengerType == "ADT").FirstOrDefault();
                        if (AdultComission != null)
                        {
                            if (AdultComission.FeeType == "Percent")
                            {
                                AirlineMarkup = AirlineMarkup + ((decimal)AdultComission.Amount * adultamount / 100 * adults);
                            }
                            else
                            {
                                AirlineMarkup = AirlineMarkup + ((decimal)AdultComission.Amount * adults);
                            }
                        }
                    }
                    if (children > 0)
                    {
                        var ChildComission = AirlineComission.Where(x => x.PassengerType == "CNN").FirstOrDefault();
                        if (ChildComission != null)
                        {
                            if (ChildComission.FeeType == "Percent")
                            {
                                AirlineMarkup = AirlineMarkup + ((decimal)ChildComission.Amount * childamount / 100 * children);
                            }
                            else
                            {
                                AirlineMarkup = AirlineMarkup + ((decimal)ChildComission.Amount * children);
                            }
                        }
                    }
                    if (infants > 0)
                    {
                        var InfantComission = AirlineComission.Where(x => x.PassengerType == "INF").FirstOrDefault();
                        if (InfantComission != null)
                        {
                            if (InfantComission.FeeType == "Percent")
                            {
                                AirlineMarkup = AirlineMarkup + ((decimal)InfantComission.Amount * infantamount / 100 * infants);
                            }
                            else
                            {
                                AirlineMarkup = AirlineMarkup + ((decimal)InfantComission.Amount * infants);
                            }
                        }
                    }
                }

                if (userid != "-1")
                {
                    int uID = Convert.ToInt32(userid);

                    var inc = (from u in oldSQL.Users
                               where u.UserID == uID
                               join c in oldSQL.Customers on u.CustomerID equals c.CustomerID
                               from i in oldSQL.Incentives // Navigate to Incentives from Customer
                               select new
                               {
                                   // Assuming you want to select specific fields from Incentives
                                   i.IncentiveID,
                                   i.Incentives,
                                   i.IncentiveType,
                                   i.RefIncentives,
                                   i.Scale,
                                   i.ServiceID,
                                   i.RefScale
                               }).ToList();

                    // List<Incentive> inc = user.Incentives.ToList();
                    var FlightIncentives = inc.Where(x => x.ServiceID == 2).FirstOrDefault();



                    string IncentiveType = FlightIncentives.IncentiveType;
                    decimal Incentives = FlightIncentives.Incentives ?? 0;
                    string Scale = FlightIncentives.Scale;
                    decimal RefIncentives = FlightIncentives.RefIncentives ?? 0;
                    string RefScale = FlightIncentives.RefScale;

                    if (IncentiveType == "Markup")
                    {
                        if (adults > 0)
                        {
                            if (Scale == "Percent")
                            {
                                AdultsAmount += (Incentives * AdultsAmount / 100) * adults;
                            }
                            else
                            {
                                AdultsAmount += Incentives * adults;
                            }
                            if (RefScale == "Percent")
                            {
                                AdultsAmount += (RefIncentives * AdultsAmount / 100) * adults;
                            }
                            else
                            {
                                AdultsAmount += RefIncentives * adults;
                            }
                        }
                        if (children > 0)
                        {
                            if (Scale == "Percent")
                            {
                                ChildsAmount += (Incentives * ChildsAmount / 100) * children;
                            }
                            else
                            {
                                ChildsAmount += Incentives * children;
                            }
                            if (RefScale == "Percent")
                            {
                                ChildsAmount += (RefIncentives * ChildsAmount / 100) * children;
                            }
                            else
                            {
                                ChildsAmount += RefIncentives * children;
                            }
                        }
                        if (infants > 0)
                        {
                            if (Scale == "Percent")
                            {
                                InfantsAmount += (Incentives * InfantsAmount / 100) * infants;
                            }
                            else
                            {
                                InfantsAmount += Incentives * infants;
                            }
                            if (RefScale == "Percent")
                            {
                                InfantsAmount += (RefIncentives * InfantsAmount / 100) * infants;
                            }
                            else
                            {
                                InfantsAmount += RefIncentives * infants;
                            }
                        }
                    }
                    decimal TotalAmount = (AdultsAmount * adults) + (ChildsAmount * children) + (InfantsAmount * infants);
                    int TotalPax = (adults + children + infants);

                    if (Scale == "Percent")
                    {
                        Incentives = (Incentives * TotalAmount / 100);
                    }
                    else
                    {
                        Incentives = Incentives * TotalPax;
                    }

                    if (RefScale == "Percent")
                    {
                        RefIncentives = (RefIncentives * TotalAmount / 100);
                    }
                    else
                    {
                        RefIncentives = RefIncentives * TotalPax;
                    }



                    arrInc[0] = Incentives;
                    arrInc[1] = RefIncentives;


                }


                arrInc[2] = AdultsAmount * adults;
                arrInc[3] = ChildsAmount * children;
                arrInc[4] = InfantsAmount * infants;
                arrInc[5] = AirlineMarkup;
                return arrInc;


            }
            catch (Exception ex)
            {

                // //ViewBag.Message = ex.Message;
                return null;
            }
        }
        [HttpGet("cancelbooking")]
        public string CancelBooking(string PNR)
        {
            Task<string> Json = CancelGalileoFlights(PNR);
            // Rootobject res = (JObject) Json.Result; // Json.Result;
            string rb = Json.Result; // res.ToObject<Rootobject>();
            return rb; //.AirPriceRsp[0].TransactionId.ToString();
        }

        [NonAction]
        public async Task<string> CancelGalileoFlights(string PNR)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //   string url = "https://apac.universal-api.pp.travelport.com/B2BGateway/connect/uAPI/AirService";
                string url = "https://emea.universal-api.travelport.com/B2BGateway/connect/uAPI/AirService";
                //  HttpClient request = (HttpWebRequest)HttpClient.Create(url);
                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                //  byte[] bytes;
                // bytes = System.Text.Encoding.ASCII.GetBytes(XMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline));
                //  request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz"))); //Test env
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                request.Headers.Add("ContentType", "text /xml; encoding='utf-8'");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "");
                request.Headers.Add("Host", "twsprofiler.travelport.com");
                request.Headers.Add("KeepAlive", "true");

                var xmlReq = XMLCancelReq(PNR);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

                var str1 = response.RequestMessage;
                var str2 = response.Content.Headers;
                // Get the response
                var customerJsonString = await response.Content.ReadAsStringAsync();

                DataSet ds = new DataSet();

                DataTable FlightSegments = null;
                DataTable dt;

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(custome‌​rJsonString);
                XmlReader xmlReader = new XmlNodeReader(xml);
                ds.ReadXml(xmlReader);

                if (ds.Tables.Count < 7)
                {
                    return "";
                }
                return "";
            }            
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [NonAction]
        private string XMLCancelReq(string PNR)
        {
            StringBuilder strReq = new StringBuilder();
            Guid TraceId = Guid.NewGuid();

            strReq.Append(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>");
            strReq.Append("<soapenv:Header/>");
            strReq.Append("<soapenv:Body>");
            strReq.Append("<UniversalRecordCancelReq xmlns='http://www.travelport.com/schema/air_v52_0' TraceId='3406e5e7-ce50-4443-ae2c-30d77a16e13d' AuthorizedBy = 'Travelport' TargetBranch = 'P3393195' UniversalRecordLocatorCode = '" + PNR + "' Version = '0'>");
            strReq.Append("<BillingPointOfSaleInfo xmlns = 'http://www.travelport.com/schema/common_v52_0' OriginApplication = 'uAPI' />");
            strReq.Append("</UniversalRecordCancelReq>");
            strReq.Append("</soapenv:Body>");
            strReq.Append("</soapenv:Envelope>");


            //strReq.Append("<UniversalRecordCancelReq xmlns='http://www.travelport.com/schema/universal_v52_0' TraceId='bae47b48-1466-40a5-9ace-ee2c6b35c8d3' AuthorizedBy='Travelport' TargetBranch=\'P7087680\' UniversalRecordLocatorCode='1THO2E' Version='0'><BillingPointOfSaleInfo xmlns='http://www.travelport.com/schema/common_v52_0' OriginApplication='uAPI' /></UniversalRecordCancelReq>");

            return strReq.ToString();
        }

        [NonAction]
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
    }

    public class TempFlight
    {
        public int option_id { get; set; }
        public string key { get; set; }
        public string AvailabilitySource { get; set; }
        public string Equipment { get; set; }
        public string AvailabilityDisplayType { get; set; }
        public string Group { get; set; }
        public string Carrier { get; set; }
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string FlightTime { get; set; }
        public string Distance { get; set; }
        public string ProviderCode { get; set; }
        public string ClassOfService { get; set; }
        public string FareBasisCode { get; set; }
    }

    public class BookingInfo
    {
        public int id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? transactionId { get; set; }
        public string? country { get; set; }
        public string? email { get; set; }
        public string? address { get; set; }
        public string? MobileNO { get; set; }
        public string? LandlineNo { get; set; }
        public string? bookingTitle { get; set; }
        public string? Supplier { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Infants { get; set; }
        public int? BookingID { get; set; }
        public string? type { get; set; }
        public string? count { get; set; }
        public string? from { get; set; }
        public string? to { get; set; }
        public string? start { get; set; }
        public string? end { get; set; }
        public string? FlightNumber { get; set; }
        public string? serviceclass0 { get; set; }
        public string? stops0 { get; set; }
        public string? total0 { get; set; }
        public string? amount0 { get; set; }
        public string? taxes0 { get; set; }
        public string? currency0 { get; set; }
        public string? totaltime0 { get; set; }
        public string? totaltime1 { get; set; }
        public string? stops1 { get; set; }
        public string? AdultTotalPrice { get; set; }
        public string? AdultBasePrice { get; set; }
        public string? AdultTaxes { get; set; }
        public string? ChildTotalPrice { get; set; }
        public string? ChildBasePrice { get; set; }
        public string? ChildTaxes { get; set; }
        public string? InfantTotalPrice { get; set; }
        public string? InfantTaxes { get; set; }
        public string? AirlineCode { get; set; }
        public string? Gender { get; set; }
        public string? document { get; set; }
        public string? documenttype { get; set; }
        public string? expiry { get; set; }
        public string? nationality { get; set; }
        public string? dob { get; set; }
        public string? title { get; set; }
        public string? city { get; set; }
        public string? TotalPrice { get; set; }
        public string? TotalTaxes { get; set; }
        public string? GrandPrice { get; set; }

    }

    public class FinalBooking
    {
        public int Id { get; set; }
        public List<Reservation>? Reservation { get; set; }
        public List<FlightSegment>? flightSegments { get; set; }
        public List<PassengerDetail>? PaxList { get; set; }
        public List<AirPricingInfoKey2>? PNR { get; set; }

    }

    public class Reservation2
    {
        public int? BookingID { get; set; }
        public string? Adults { get; set; }
        public string? Children { get; set; }
        public string? Infants { get; set; }
        public string? AdultBasicFare { get; set; }
        public string? ChildBasicFare { get; set; }
        public string? InfantBasicFare { get; set; }
        public string? AdultTax { get; set; }
        public string? ChildTax { get; set; }
        public string? InfantTax { get; set; }
        public string? TotalFare { get; set; }
        public string? TotalTaxes { get; set; }
        public string? GrandTotal { get; set; }
        public string? CustomerComission { get; set; }
        public string? CustomerOComission { get; set; }
        public string? AirlineComission { get; set; }
        public string? Incentive { get; set; }
        public string? IncentiveScale { get; set; }
        public string? BookingDate { get; set; }
        public string? BookingStatus { get; set; }
        public string? BookingTypeID { get; set; }
        public string? LeadFirstName { get; set; }
        public string? LeadLastName { get; set; }
        public string? Email { get; set; }
        public string? PrimaryPhone { get; set; }
        public string? SecondaryPhone { get; set; }
    }

    public class FlightSegments2
    {
        public string? ArrivalDate { get; set; }
        public string? ArrivalTime { get; set; }
        public string? ArrivalGMTOffset { get; set; }
        public string? DepartureDate { get; set; }
        public string? DepartureTime { get; set; }
        public string? DepartureGMTOffset { get; set; }
        public string? ServiceClassID { get; set; }
        public string? OriginID { get; set; }
        public string? DestinationID { get; set; }
        public string? FlightNumber { get; set; }
        public string? MarketingAirlineCode { get; set; }
        public string? OperatingAirlineCode { get; set; }
        public string? MarketingAirlineID { get; set; }
        public string? OperatingAirlineID { get; set; }
        public string? OperatingAirlineName { get; set; }
        public string? FlightBookingID { get; set; }
        public string? AirEquipType { get; set; }
        public string? ETicket { get; set; }
        public string? ResBookDesigCode { get; set; }
        public string? Way { get; set; }
    }

    public class PaxList
    {
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? Document { get; set; }
        public string? TicketNo { get; set; }
        public string? DocumentType { get; set; }
        public string? DOE { get; set; }
        public string? FlightBookingID { get; set; }
        public string? PassengerStatus { get; set; }
        public string? PassegerType { get; set; }
    }

    public class AirPricingInfoKey2
    {
        public int Id { get; set; }
        public string PNR { get; set; }
        public string PnrForTicket { get; set; }
        public string airPricingInfoKey { get; set; }
        public int? BookingId { get; set; }
    }
}