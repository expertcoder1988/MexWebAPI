using LowFareAlerts;
using MalikExpressMVC.Models.XML;
using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace MexCoreWebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]
    public class FlightSearch : ControllerBase
    {
        //MexDbContext sql;
        private readonly MexDbContext sql;
        private readonly VisaDbContext db;
        private readonly MexOldDBContext oldSQL;

        public FlightSearch(MexDbContext context, VisaDbContext visadb, MexOldDBContext OldSQL)
        {
            sql = context;
            db = visadb;
            oldSQL = OldSQL;
        }

        [HttpGet("AirportName")]

        //where c.IATA.Equals(Iata)
        public string AirportName(string term)
        {
            // Response.Headers.Add("Access-Control-Allow-Origin", "https://malikexpress.com");

            var val = (from c in sql.Cities
                       join co in sql.Countries on c.countryID equals co.CountryID
                       where ((c.cityName.StartsWith(term) || c.IATA.StartsWith(term) || c.Airport.StartsWith(term)) &&
            !string.IsNullOrEmpty(c.IATA))
                       select c.cityName + " " + co.CountryName + " (" + c.IATA + ")").ToList();

            var json = System.Text.Json.JsonSerializer.Serialize(val);
            return json.ToString();

            // ViewBag.CityName = val != null ? val.cityname : "Not Available";
            // return PartialView("Destination");
        }

        [HttpGet("airportlist")]

        //where c.IATA.Equals(Iata)
        public string AirportList()
        {
            // Response.Headers.Add("Access-Control-Allow-Origin", "https://malikexpress.com");

            var val = (from c in sql.Cities
                       join co in sql.Countries on c.countryID equals co.CountryID
                       where !string.IsNullOrEmpty(c.IATA)
                       select c.cityName + " " + co.CountryName + " (" + c.IATA + ")").ToList();

            var json = System.Text.Json.JsonSerializer.Serialize(val);
            return json.ToString();

            // ViewBag.CityName = val != null ? val.cityname : "Not Available";
            // return PartialView("Destination");
        }

        [HttpGet("cityname")]
        public string CityName(string term)
        {
            var cityName = (from x in sql.Cities.AsEnumerable()
                            where x.cityCode == term
                            select new { Name = x.cityName }).FirstOrDefault();


            var json = System.Text.Json.JsonSerializer.Serialize(cityName);
            return json.ToString();
        }

        [HttpGet("countries")]
        public string Countries()
        {
            var countryName = (from x in sql.Countries.AsEnumerable()
                               orderby x.CountryName
                               select new { Id = x.CountryID, Value = x.CountryName }).ToList();


            var json = System.Text.Json.JsonSerializer.Serialize(countryName);
            return json.ToString();
        }

        [HttpGet("CountryAll")]
        public string CountryList()
        {
            var Countries = db.VisaCountries.ToList();
            var CountryList = new List<string>();
            foreach (var item in Countries)
            {
                var visa = db.Visas.Where(x => x.CountryID == item.Id).FirstOrDefault();
                if (visa == null)
                {
                    continue;
                }
                else
                {
                    CountryList.Add(item.Name);
                }
            }
            var json = System.Text.Json.JsonSerializer.Serialize(CountryList);
            return json.ToString();
        }

        [HttpGet("cities")]
        public string Cities(int term)
        {
            var countryName = (from x in sql.Cities.AsEnumerable()
                               where x.countryID == term
                               orderby x.cityName
                               select new { Id = x.cityID, Value = x.cityName }).ToList();

            var json = System.Text.Json.JsonSerializer.Serialize(countryName);
            return json.ToString();
        }

        [HttpGet("pakcities")]
        public string PakCities()
        {
            var countryName = (from x in sql.Cities.AsEnumerable()
                               where x.countryID == 144
                               orderby x.cityName
                               select new { Id = x.cityID, Value = x.cityName }).ToList();

            var json = System.Text.Json.JsonSerializer.Serialize(countryName);
            return json.ToString();
        }

        [HttpGet("citynamebyid")]
        public string CityNameByID(int term)
        {
            var cityName = (from x in sql.Cities.AsEnumerable()
                            where x.cityID == term
                            select new { Name = x.cityName, Code = x.cityCode }).FirstOrDefault();


            var json = System.Text.Json.JsonSerializer.Serialize(cityName);
            return json.ToString();
        }

        [HttpGet("Results")]

        public string Results(int adults, int children, int infants, string fromM_1="", string fromM_2 = "", string fromM_3 = "", string fromM_4 = "", string fromM_5 = "", 
            string fromM_6 = "", string toM_1 = "", string toM_2 = "", string toM_3 = "", string toM_4 = "",
            string toM_5 = "", string toM_6 = "", string from = "", string to = "", string start = "", string end = "",
            string type = "", string airline = "", string service = "",  int? flexdate=0, int? directconecting=0, int? multipleshow=0,
            int stops = -1)
        {

            ////Session["convid"] = null;
            if (stops == -1)
            {
                //Session.Remove("Results");
                if ((string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || string.IsNullOrEmpty(start) || adults == null || children == null || infants == null) && (type == "OneWay" || type == "Return"))
                {
                    return "Bad Request";
                }
                else if ((string.IsNullOrEmpty(fromM_1) || string.IsNullOrEmpty(fromM_2) || string.IsNullOrEmpty(toM_1) || string.IsNullOrEmpty(toM_2) || string.IsNullOrEmpty(start) || adults == null || children == null || infants == null) && (type == "Other"))
                {
                    return "Bad Request";
                }
                else
                {
                    string Dept = "", Arr = "";
                    string IatacodeDep = "DFW";
                    string IatacodeArr = "CDG";
                    string[] sdate;
                    string[] edate;


                    if (type != "Other")
                    {
                        //sdate = start.Split('-');
                        string[] _sDate = start.Split(',');
                        string sDate = _sDate[0];

                        if (sDate.IndexOf("-") > -1)
                        {
                            _sDate = sDate.Split('-');
                            start = _sDate[0] + "/" + _sDate[1] + "/" + _sDate[2];
                        }
                        else
                        {
                            start = sDate;
                        }

                        DateTime dtStart = Convert.ToDateTime(start);
                        Dept = string.Format("{0:s}", dtStart);//"2016-04-01T11:00:00"

                        string[] eDate;

                        if (end.IndexOf("-") > -1)
                        {
                            edate = end.Split('-');
                            end = edate[0] + "/" + edate[1] + "/" + edate[2];
                        }
                        else
                        {

                        }

                        DateTime dtEnd = Convert.ToDateTime(end);

                        Arr = string.Format("{0:s}", dtEnd);//"2016-04-15T11:00:00"

                        if (from.IndexOf('(') < 0)
                        {
                            var _IatacodeDep = sql.Cities.Where(x => x.cityCode == from || x.IATA == from).Select(x => x.IATA).FirstOrDefault();
                            if (_IatacodeDep != null)
                            {
                                IatacodeDep = _IatacodeDep;
                                IatacodeDep = IatacodeDep.ToUpper();
                            }
                            else
                            {
                                return "";
                            }
                        }
                        else
                        {
                            string[] fromIata = from.Split('(');

                            if (fromIata.Length > 1)
                            {
                                IatacodeDep = fromIata[1].Split(')')[0].Trim();
                            }
                            else if (fromIata.Length > 0 && fromIata[0].Length == 3)
                            {
                                IatacodeDep = fromIata[0];
                            }
                            else if (fromIata.Length > 0 && fromIata[0].Length > 3)
                            {
                                IatacodeDep = sql.Cities.Where(x => x.cityCode == fromIata[0]).Select(x => x.cityCode).FirstOrDefault();
                            }
                            IatacodeDep = IatacodeDep.ToUpper();
                        }

                        //var _strFrom = sql.Cities.Where(a => a.IATA == IatacodeDep).Select(a =>
                        //                 new User
                        //                 {

                        //                 }).FirstOrDefault();

                        if (to.IndexOf('(') < 0)
                        {
                            var _IatacodeArr = sql.Cities.Where(x => x.cityCode == to || x.IATA == to).Select(x => x.IATA).FirstOrDefault();
                            if (_IatacodeArr != null)
                            {
                                IatacodeArr = _IatacodeArr;
                                IatacodeArr = IatacodeArr.ToUpper();
                            }
                            else
                            {
                                return "";// RedirectToAction("Index", "Flight");
                            }
                        }
                        else
                        {
                            string[] toIata = to.Split('(');

                            if (toIata.Length > 1)
                            {
                                IatacodeArr = toIata[1].Split(')')[0].Trim();
                            }
                            else if (toIata.Length > 0 && toIata[0].Length == 3)
                            {
                                IatacodeArr = toIata[0];
                            }
                            else if (toIata.Length > 0 && toIata[0].Length > 3)
                            {
                                IatacodeArr = sql.Cities.Where(x => x.cityCode == toIata[0]).Select(x => x.cityCode).FirstOrDefault();
                            }
                            IatacodeArr = IatacodeArr.ToUpper();
                        }
                    }
                    else
                    {
                        Dept = start;
                    }

                    //Functions
                    Task<string> Json = GalileoFlights(type, IatacodeDep, IatacodeArr, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                    string res = Json.Result;

                    //if (type == "OneWay")
                    //{
                    //HitItFlights(type, IatacodeDep, IatacodeArr, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                    //}
                    //else if (type == "Return")
                    //{
                    //    HitItFlights_Returns(type, IatacodeDep, IatacodeArr, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                    //}
                    //else
                    //{
                    //    HitItFlights_Multi(type, IatacodeDep, IatacodeArr, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, end, airline);
                    //}
                    // SereneFlights(type, IatacodeDep, IatacodeArr, adults, children, infants, Dept, Arr);
                    //AirblueFLights(type, IatacodeDep, IatacodeArr, adults, children, infants, Dept, Arr);
                    return res;



                }
            }
            else
            {

                return "";

            }
        }

        public static string ExtractCode(string input)
        {
            // Use regular expression to find the content inside parentheses
            Match match = Regex.Match(input, @"\(([A-Z]{3})\)");

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null; // Return null if no match is found
        }

        //[HttpGet("GalileoFlights")]
        [NonAction]
        public async Task<string> GalileoFlights(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr,
string fromM_1, string fromM_2, string fromM_3, string fromM_4, string fromM_5, string fromM_6,
string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string airline = "", string userid = "-1")
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

                var xmlReq = XMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);



                {
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

                    //Log

                    string formattedDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    XmlDocument xml1 = new XmlDocument();
                    xml1.LoadXml(xmlReq.ToString());
                    XmlReader xmlReader1 = new XmlNodeReader(xml1);

                    XmlTextWriter writer = new XmlTextWriter(@"C:\TravelportLog\AirLowFareSearch_Request_" + formattedDate + ".xml", null);
                    writer.Formatting = System.Xml.Formatting.Indented;
                    xml1.Save(writer);

                    XmlTextWriter writer1 = new XmlTextWriter(@"C:\TravelportLog\AirLowFareSearch_Response_" + formattedDate + ".xml", null);
                    writer1.Formatting = System.Xml.Formatting.Indented;
                    xml.Save(writer1);

                    // Commnent above 9 lines before publishing


                    if (ds.Tables.Count < 7)
                    {
                        if (ds.Tables.Contains("ErrorInfo"))
                        {
                            ////TempData["Message"] = ds.Tables["ErrorInfo"].Rows[0]["Description"].ToString();
                            return "";
                        }

                    }
                    else if (tripType == "OneWay")
                    {
                        #region oneway

                        var distinctCarriers = ds.Tables["AirSegment"].AsEnumerable()
                        .Select(row => new
                        {
                            carriers = row.Field<string>("carrier"),
                        })
                        .Distinct()
                        .ToList();

                        string commaSeparatedValues = string.Join(",", distinctCarriers);
                        commaSeparatedValues = commaSeparatedValues.Replace("{ carriers = ", "");
                        commaSeparatedValues = commaSeparatedValues.Replace("}", "");

                        var CarrierName = (from x in sql.Airlines
                                           .Where(x => commaSeparatedValues.Contains(x.IataCode))
                                           select new
                                           {
                                               x.IataCode,
                                               x.AirlineName
                                           }).ToList();


                        ds.Tables["AirSegment"].Columns.Add("Direct");


                        foreach (DataRow dr in ds.Tables["AirSegment"].Rows)
                        {
                            if (dr["Origin"].ToString() == from && dr["Destination"].ToString() == to)
                            {
                                dr["Direct"] = 1;
                            }
                            else
                            {
                                dr["Direct"] = 0;
                            }
                        }
                        ds.Tables["AirSegment"].Columns.Add("Flight");
                        //int i = 1;

                        foreach (DataRow dr in ds.Tables["AirSegment"].Rows)
                        {
                            if (dr["Origin"].ToString() != from)
                            {
                                dr["Flight"] = "DC"; //Connecting Flight
                            }
                            else
                            {
                                dr["Flight"] = "DM"; //Main Flight
                            }
                        }
                        ds.Tables["BookingInfo"].Columns.Add("PaxType");

                        foreach (DataRow dr in ds.Tables["BookingInfo"].Rows)
                        {
                            string id = dr["option_id"].ToString();
                            DataTable BookingInfo = ds.Tables["BookingInfo"];
                            DataTable FareInfoRef = ds.Tables["FareInfoRef"];
                            DataTable AirPricingInfo = ds.Tables["AirPricingInfo"];
                            DataTable PassengerType = ds.Tables["PassengerType"];

                            string ff = (BookingInfo.AsEnumerable().Where(p => p["option_id"].ToString() == id).Select(p => p["FareInfoRef"].ToString())).FirstOrDefault();

                            var AirPricingInfo_id = (FareInfoRef.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["AirPricingInfo_id"].ToString())).FirstOrDefault();

                            string PaxType = (PassengerType.AsEnumerable().Where(p => Convert.ToInt32(p["AirPricingInfo_id"].ToString()) == Convert.ToInt32(AirPricingInfo_id)).Select(p => p["Code"].ToString())).FirstOrDefault();

                            if (PaxType == "ADT")
                            {
                                dr["PaxType"] = "P";
                            }
                            else if (PaxType == "CNN")
                            {
                                dr["PaxType"] = "C";
                            }
                            else if (PaxType == "INF")
                            {
                                dr["PaxType"] = "I";
                            }
                        }

                        DataTable dt5 = new DataTable();
                        dt5 = ds.Tables["BookingInfo"].Copy();
                        dt5.Columns.Add("AdultTotalPrice");
                        dt5.Columns.Add("AdultBasePrice");
                        dt5.Columns.Add("AdultTaxes");
                        dt5.Columns.Add("ChildTotalPrice");
                        dt5.Columns.Add("ChildBasePrice");
                        dt5.Columns.Add("ChildTaxes");
                        dt5.Columns.Add("InfantTotalPrice");
                        dt5.Columns.Add("InfantBasePrice");
                        dt5.Columns.Add("InfantTaxes");
                        dt5.Columns.Add("FareBasis");
                        dt5.Columns.Add("SegmentIndex");
                        dt5.Columns.Add("MaxWeightValue");
                        dt5.Columns.Add("MaxWeightUnit");


                        foreach (DataRow dr in dt5.Rows)
                        {
                            string id = dr["option_id"].ToString();
                            DataTable BookingInfo = ds.Tables["BookingInfo"];
                            DataTable FareInfoRef = ds.Tables["FareInfoRef"];
                            DataTable FareInfo = ds.Tables["FareInfo"];
                            DataTable AirPricingInfo = ds.Tables["AirPricingInfo"];
                            DataTable AirPricePoint = ds.Tables["AirPricePoint"];
                            DataTable PassengerType = ds.Tables["PassengerType"];
                            DataTable Connection = ds.Tables["Connection"];

                            string ff = (BookingInfo.AsEnumerable().Where(p => p["option_id"].ToString() == id).Select(p => p["FareInfoRef"].ToString())).FirstOrDefault();

                            string SegmentIndex = "0";

                            if (Connection != null)
                            {
                                SegmentIndex = (Connection.AsEnumerable().Where(p => p["option_id"].ToString() == id).Select(p => p["SegmentIndex"].ToString())).FirstOrDefault();
                            }

                            // Check if the DataTable contains the column "baggageallowance_id"
                            if (!ds.Tables["baggageallowance"].Columns.Contains("baggageallowance_id"))
                            {
                                // If the column doesn't exist, add it
                                ds.Tables["baggageallowance"].Columns.Add("baggageallowance_id", typeof(int));

                                int idCounter = 1;

                                // Iterate through the rows of the DataTable and assign unique IDs
                                foreach (DataRow row in ds.Tables["baggageallowance"].Rows)
                                {
                                    // Assign the unique ID to the "baggageallowance_id" column
                                    row["baggageallowance_id"] = idCounter++;

                                    // If you want to break the loop when the counter reaches a certain value,
                                    // you can add a condition here, for example:
                                    // if (idCounter > maxId) break;
                                }
                            }

                            string MaxWeightValue = "";
                            var MaxWeightUnit = "";

                            var AirPricingInfo_id = (FareInfoRef.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["AirPricingInfo_id"].ToString())).FirstOrDefault();

                            string PaxType = (PassengerType.AsEnumerable().Where(p => Convert.ToInt32(p["AirPricingInfo_id"].ToString()) == Convert.ToInt32(AirPricingInfo_id)).Select(p => p["Code"].ToString())).FirstOrDefault();

                            // var FareBasisCode = (FareInfo.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["FareBasis"].ToString())).FirstOrDefault();

                            var FareBasisCode = (FareInfo.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["FareBasis"].ToString())).FirstOrDefault();

                            var FareInfo_id = (FareInfo.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["FareInfo_id"].ToString())).FirstOrDefault();

                            var baggageallowance_id = (ds.Tables["baggageallowance"].AsEnumerable().Where(p => p["FareInfo_id"].ToString() == FareInfo_id).Select(p => p["baggageallowance_id"].ToString())).FirstOrDefault();

                            if (!ds.Tables.Contains("maxweight"))
                            {
                                if (ds.Tables["baggageallowance"].Columns.Contains("maxweight"))
                                {
                                    MaxWeightValue = (ds.Tables["baggageallowance"].AsEnumerable().Where(p => p["baggageallowance_id"].ToString() == baggageallowance_id).Select(p => p["maxweight"].ToString())).FirstOrDefault();
                                }
                                //if (ds.Tables["baggageallowance"].Columns.Contains("NumberofPieces"))
                                //{
                                //    MaxWeightUnit = (ds.Tables["baggageallowance"].AsEnumerable().Where(p => p["baggageallowance_id"].ToString() == baggageallowance_id).Select(p => p["NumberofPieces"].ToString())).FirstOrDefault();
                                //}
                            }
                            else
                            {
                                MaxWeightValue = (ds.Tables["maxweight"].AsEnumerable().Where(p => p["baggageallowance_id"].ToString() == baggageallowance_id).Select(p => p["value"].ToString())).FirstOrDefault();
                                MaxWeightUnit = (ds.Tables["maxweight"].AsEnumerable().Where(p => p["baggageallowance_id"].ToString() == baggageallowance_id).Select(p => p["unit"].ToString())).FirstOrDefault();

                            }
                            var APInfo = (AirPricingInfo.AsEnumerable().Where(p => p["AirPricingInfo_id"].ToString() == AirPricingInfo_id).Select(p => p["AirPricePoint_Id"].ToString())).FirstOrDefault();

                            var APP = from d in AirPricePoint.AsEnumerable()
                                      where d.Field<int>("AirPricePoint_Id") == Convert.ToInt32(APInfo)
                                      select new
                                      {
                                          TotalPrice = d.Field<string>("ApproximateTotalPrice"),
                                          BasePrice = d.Field<string>("ApproximateBasePrice"),
                                          Taxes = d.Field<string>("Taxes")
                                      };

                            dr["MaxWeightValue"] = MaxWeightValue;
                            dr["MaxWeightUnit"] = MaxWeightUnit;
                            dr["FareBasis"] = FareBasisCode;
                            dr["SegmentIndex"] = SegmentIndex;

                            foreach (var itm in APP)
                            {
                                if (PaxType == "ADT")
                                {
                                    dr["AdultBasePrice"] = itm.BasePrice;
                                    dr["AdultTotalPrice"] = itm.TotalPrice;
                                    dr["AdultTaxes"] = itm.Taxes;
                                }
                                else if (PaxType == "CNN")
                                {
                                    dr["ChildBasePrice"] = itm.BasePrice;
                                    dr["ChildTotalPrice"] = itm.TotalPrice;
                                    dr["ChildTaxes"] = itm.Taxes;
                                }
                                else if (PaxType == "INF")
                                {
                                    dr["InfantBasePrice"] = itm.BasePrice;
                                    dr["InfantTotalPrice"] = itm.TotalPrice;
                                    dr["InfantTaxes"] = itm.Taxes;
                                }
                            }
                        }

                        FlightSegments = ds.Tables["BookingInfo"].Copy();
                        FlightSegments.Columns.Add("AdultTotalPrice");
                        FlightSegments.Columns.Add("AdultBasePrice");
                        FlightSegments.Columns.Add("AdultTaxes");
                        FlightSegments.Columns.Add("ChildTotalPrice");
                        FlightSegments.Columns.Add("ChildBasePrice");
                        FlightSegments.Columns.Add("ChildTaxes");
                        FlightSegments.Columns.Add("InfantTotalPrice");
                        FlightSegments.Columns.Add("InfantBasePrice");
                        FlightSegments.Columns.Add("InfantTaxes");
                        FlightSegments.Columns.Add("FareBasis");
                        FlightSegments.Columns.Add("SegmentIndex");
                        FlightSegments.Columns.Add("MaxWeightValue");
                        FlightSegments.Columns.Add("MaxWeightUnit");
                        FlightSegments.Columns.Add("GrandTotal");
                        FlightSegments.Columns.Add("TransactionId");

                        Guid TransactionId = Guid.NewGuid();

                        foreach (DataRow dr in FlightSegments.Rows)
                        {
                            dr["TransactionId"] = TransactionId;
                        }

                        foreach (DataRow dr in FlightSegments.Rows)
                        {
                            //if (dr["PaxType"].ToString() == "P")
                            {
                                foreach (DataRow dr1 in dt5.Rows)
                                {
                                    dr["SegmentIndex"] = dr1["SegmentIndex"];
                                    dr["MaxWeightValue"] = dr1["MaxWeightValue"];
                                    dr["MaxWeightUnit"] = dr1["MaxWeightUnit"];

                                    if (dr["SegmentRef"].ToString() == dr1["SegmentRef"].ToString() && (dr1["PaxType"].ToString() == "P") && dr["BookingCode"].ToString() == dr1["BookingCode"].ToString())
                                    {
                                        dr["AdultBasePrice"] = dr1["AdultBasePrice"];
                                        dr["AdultTotalPrice"] = dr1["AdultTotalPrice"];
                                        dr["AdultTaxes"] = dr1["AdultTaxes"];
                                        break;
                                    }
                                    else if (dr["SegmentRef"].ToString() == dr1["SegmentRef"].ToString() && (dr1["PaxType"].ToString() == "C") && dr["BookingCode"].ToString() == dr1["BookingCode"].ToString())
                                    {
                                        dr["ChildBasePrice"] = dr1["ChildBasePrice"];
                                        dr["ChildTotalPrice"] = dr1["ChildTotalPrice"];
                                        dr["ChildTaxes"] = dr1["ChildTaxes"];
                                        break;
                                    }
                                    else if (dr["SegmentRef"].ToString() == dr1["SegmentRef"].ToString() && (dr1["PaxType"].ToString() == "I") && dr["BookingCode"].ToString() == dr1["BookingCode"].ToString())
                                    {
                                        dr["InfantBasePrice"] = dr1["InfantBasePrice"];
                                        dr["InfantTotalPrice"] = dr1["InfantTotalPrice"];
                                        dr["InfantTaxes"] = dr1["InfantTaxes"];
                                        break;
                                    }
                                }
                            }
                        }

                        FlightSegments.Columns.Add("SortBy", typeof(Int32));

                        foreach (DataRow row in FlightSegments.Rows)
                        {
                            string price = Convert.ToString(row["AdultTotalPrice"]).Replace("PKR", "");
                            row["SortBy"] = price;
                        }

                        DataView dv = FlightSegments.DefaultView;
                        dv.Sort = "SortBy";
                        FlightSegments = dv.ToTable();

                        for (int t = FlightSegments.Rows.Count - 1; t >= 0; t--)
                        {
                            if (FlightSegments.Rows[t]["PaxType"].ToString() == "C" || FlightSegments.Rows[t]["PaxType"].ToString() == "I")
                            {
                                FlightSegments.Rows[t].Delete();
                                FlightSegments.AcceptChanges();
                            }
                        }

                        foreach (DataRow itm in FlightSegments.Rows)
                        {
                            if (string.IsNullOrEmpty(itm["AdultTotalPrice"].ToString()) == true)
                            {
                                itm["AdultTotalPrice"] = "0";
                            }
                            else
                            {
                                itm["AdultTotalPrice"] = Convert.ToString(itm["AdultTotalPrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["AdultBasePrice"].ToString()) == true)
                            {
                                itm["AdultBasePrice"] = "0";
                            }
                            else
                            {
                                itm["AdultBasePrice"] = Convert.ToString(itm["AdultBasePrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["AdultTaxes"].ToString()) == true)
                            {
                                itm["AdultTaxes"] = "0";
                            }
                            else
                            {
                                itm["AdultTaxes"] = Convert.ToString(itm["AdultTaxes"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["ChildTotalPrice"].ToString()) == true)
                            {
                                itm["ChildTotalPrice"] = "0";
                            }
                            else
                            {
                                itm["ChildTotalPrice"] = Convert.ToString(itm["ChildTotalPrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["ChildBasePrice"].ToString()) == true)
                            {
                                itm["ChildBasePrice"] = "0";
                            }
                            else
                            {
                                itm["ChildBasePrice"] = Convert.ToString(itm["ChildBasePrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["ChildTaxes"].ToString()) == true)
                            {
                                itm["ChildTaxes"] = "0";
                            }
                            else
                            {
                                itm["ChildTaxes"] = Convert.ToString(itm["ChildTaxes"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["InfantTotalPrice"].ToString()) == true)
                            {
                                itm["InfantTotalPrice"] = "0";
                            }
                            else
                            {
                                itm["InfantTotalPrice"] = Convert.ToString(itm["InfantTotalPrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["InfantBasePrice"].ToString()) == true)
                            {
                                itm["InfantBasePrice"] = "0";
                            }
                            else
                            {
                                itm["InfantBasePrice"] = Convert.ToString(itm["InfantBasePrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["InfantTaxes"].ToString()) == true)
                            {
                                itm["InfantTaxes"] = "0";
                            }
                            else
                            {
                                itm["InfantTaxes"] = Convert.ToString(itm["InfantTaxes"]).Insert(3, " ");
                            }
                        }




                        FlightSegments.Columns.Add("Stops");
                        FlightSegments.Columns.Add("Direct");
                        FlightSegments.Columns.Add("Flight");
                        FlightSegments.Columns.Add("SegmentKey");
                        FlightSegments.Columns.Add("Group");
                        FlightSegments.Columns.Add("Carrier");
                        FlightSegments.Columns.Add("FlightNumber");
                        FlightSegments.Columns.Add("Origin");
                        FlightSegments.Columns.Add("Destination");
                        FlightSegments.Columns.Add("DepartureTime");
                        FlightSegments.Columns.Add("ArrivalTime");
                        FlightSegments.Columns.Add("FlightTime");
                        FlightSegments.Columns.Add("Distance");
                        FlightSegments.Columns.Add("ETicketability");
                        FlightSegments.Columns.Add("Equipment");
                        FlightSegments.Columns.Add("ChangeOfPlane");
                        FlightSegments.Columns.Add("ParticipantLevel");
                        FlightSegments.Columns.Add("LinkAvailability");
                        FlightSegments.Columns.Add("PolledAvailabilityOption");
                        FlightSegments.Columns.Add("OptionalServicesIndicator");
                        FlightSegments.Columns.Add("AvailabilitySource");
                        FlightSegments.Columns.Add("AvailabilityDisplayType");
                        FlightSegments.Columns.Add("ProviderCode");
                        FlightSegments.Columns.Add("ArrTime"); //Final Arrival Time
                        FlightSegments.Columns.Add("FinalDestination");
                        FlightSegments.Columns.Add("CancelPenalty");
                        FlightSegments.Columns.Add("ChangePenalty");
                        FlightSegments.Columns.Add("CarrierName");
                        FlightSegments.Columns.Add("OriginalCost");

                        //ds.Tables["AirSegment"].Columns.Add("CarrierName");

                        foreach (DataRow row in FlightSegments.Rows)
                        {
                            foreach (DataRow row1 in ds.Tables["AirSegment"].Rows)
                            {
                                if (row["SegmentRef"].ToString() == row1["Key"].ToString())
                                {
                                    row["ProviderCode"] = "1G";
                                    row["Direct"] = row1["Direct"];
                                    row["Flight"] = row1["Flight"];
                                    row["SegmentKey"] = row1["Key"];
                                    row["Group"] = row1["Group"];
                                    row["Carrier"] = row1["Carrier"];
                                    row["FlightNumber"] = row1["FlightNumber"];
                                    row["Origin"] = row1["Origin"];
                                    row["Destination"] = row1["Destination"];
                                    row["DepartureTime"] = row1["DepartureTime"];
                                    row["ArrivalTime"] = row1["ArrivalTime"];
                                    row["FlightTime"] = row1["FlightTime"];
                                    row["Distance"] = row1["Distance"];
                                    row["ETicketability"] = row1["ETicketability"];
                                    row["Equipment"] = row1["Equipment"];
                                    row["ChangeOfPlane"] = row1["ChangeOfPlane"];
                                    row["ChangeOfPlane"] = row1["ChangeOfPlane"];
                                    if (ds.Tables["AirSegment"].Columns.Contains("ParticipantLevel"))
                                    {
                                        row["ParticipantLevel"] = row1["ParticipantLevel"];
                                    }

                                    if (ds.Tables["AirSegment"].Columns.Contains("LinkAvailability"))
                                    {
                                        row["LinkAvailability"] = row1["LinkAvailability"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("PolledAvailabilityOption"))
                                    {
                                        row["PolledAvailabilityOption"] = row1["PolledAvailabilityOption"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("OptionalServicesIndicator"))
                                    {
                                        row["OptionalServicesIndicator"] = row1["OptionalServicesIndicator"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("AvailabilitySource"))
                                    {
                                        row["AvailabilitySource"] = row1["AvailabilitySource"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("AvailabilityDisplayType"))
                                    {
                                        row["AvailabilityDisplayType"] = row1["AvailabilityDisplayType"];
                                    }

                                    row["FinalDestination"] = to;
                                }
                            }
                        }

                        foreach (DataRow ro in FlightSegments.Rows)
                        {
                            string[] adultPrice = null;
                            string[] childPrice = null;
                            string[] infantPrice = null;
                            decimal grandTotal = 0;
                            decimal totalfare = 0;
                            decimal totaltaxes = 0;
                            decimal _adultPrice = 0;
                            decimal _childPrice = 0;
                            decimal _infantPrice = 0;



                            if (ro["AdultTotalPrice"].ToString().IndexOf(' ') > -1)
                            {
                                adultPrice = ro["AdultTotalPrice"].ToString().Split(' ');
                                _adultPrice = Convert.ToDecimal(adultPrice[1]);
                                grandTotal = grandTotal + Convert.ToDecimal(adultPrice[1]);
                            }

                            if (ro["ChildTotalPrice"].ToString().IndexOf(' ') > -1)
                            {
                                childPrice = ro["ChildTotalPrice"].ToString().Split(' ');
                                _childPrice = Convert.ToDecimal(childPrice[1]);
                                grandTotal = grandTotal + Convert.ToDecimal(childPrice[1]);
                            }

                            if (ro["InfantTotalPrice"].ToString().IndexOf(' ') > -1)
                            {
                                infantPrice = ro["InfantTotalPrice"].ToString().Split(' ');
                                _infantPrice = Convert.ToDecimal(infantPrice[1]);
                                grandTotal = grandTotal + Convert.ToDecimal(infantPrice[1]);
                            }

                            ro["OriginalCost"] = grandTotal;

                            grandTotal = 0;

                            decimal[] TotalAmt = _AddMarkup(_adultPrice, _childPrice, _infantPrice, adults, children, infants, ro["carrier"].ToString(), userid);

                            //decimal[] TotalAmt = _AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Childs, Infants, AirlineCode);

                            //totalfare = TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                            grandTotal = TotalAmt[2] + TotalAmt[3] + TotalAmt[4] + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                            ro["GrandTotal"] = grandTotal;
                        }


                        ds.EnforceConstraints = false;

                        foreach (DataRow dr in FlightSegments.Rows)
                        {
                            foreach (var itm in CarrierName)
                            {
                                if (dr["carrier"].ToString() == itm.IataCode.ToString())
                                {
                                    dr["CarrierName"] = itm.AirlineName.ToString();
                                }
                            }
                        }

                        foreach (DataRow dr in FlightSegments.Rows)
                        {
                            string id = dr["SegmentKey"].ToString();
                            DataTable BookingInfo = ds.Tables["BookingInfo"];
                            DataTable FareInfoRef = ds.Tables["FareInfoRef"];
                            DataTable AirPricingInfo = ds.Tables["AirPricingInfo"];
                            DataTable PassengerType = ds.Tables["PassengerType"];

                            string CancelPenalty = "";
                            string ChangePenalty = "";

                            string ff = (BookingInfo.AsEnumerable().Where(p => p["SegmentRef"].ToString() == id && p["BookingCode"].ToString() == dr["BookingCode"].ToString()).Select(p => p["FareInfoRef"].ToString())).FirstOrDefault();

                            var AirPricingInfo_id = (FareInfoRef.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["AirPricingInfo_id"].ToString())).FirstOrDefault();

                            string PaxType = (PassengerType.AsEnumerable().Where(p => Convert.ToInt32(p["AirPricingInfo_id"].ToString()) == Convert.ToInt32(AirPricingInfo_id)).Select(p => p["Code"].ToString())).FirstOrDefault();

                            if (ds.Tables["CancelPenalty"] != null)
                            {
                                CancelPenalty = (ds.Tables["CancelPenalty"].AsEnumerable().Where(p => p["AirPricingInfo_id"].ToString() == AirPricingInfo_id).Select(p => p["Amount"].ToString())).FirstOrDefault();
                            }

                            if (ds.Tables["ChangePenalty"] != null)
                            {
                                ChangePenalty = (ds.Tables["ChangePenalty"].AsEnumerable().Where(p => p["AirPricingInfo_id"].ToString() == AirPricingInfo_id).Select(p => p["Amount"].ToString())).FirstOrDefault();
                            }

                            int count = FlightSegments.AsEnumerable().Count(row => row.Field<int>("Option_Id") == Convert.ToInt32(dr["Option_Id"].ToString()));
                            string arrtime = FlightSegments.AsEnumerable().Where(p => p.Field<int>("Option_Id") == Convert.ToInt32(dr["Option_Id"].ToString()) && p.Field<string>("Destination") == to).Select(p => p["ArrivalTime"].ToString()).FirstOrDefault();

                            dr["CancelPenalty"] = CancelPenalty;
                            dr["ChangePenalty"] = ChangePenalty;

                            if (Arr.IndexOf("T") > -1)
                            {
                                dr["arrtime"] = arrtime;
                            }
                            else
                            {
                                dr["arrtime"] = dr["ArrivalTime"];
                            }

                            dr["stops"] = (count - 1).ToString();

                        }
                        FlightSegments.AcceptChanges();

                        for (int i = FlightSegments.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow row = FlightSegments.Rows[i];

                            // Check if the condition is true for the current row
                            if (FlightSegments.Rows[i]["Carrier"].ToString() == "PK")
                            {
                                // Delete the row
                                FlightSegments.Rows.Remove(row);
                            }
                            else
                            {
                                // Handle the else case, if needed
                            }
                        }

                        FlightSegments.Columns.Add("TotalDuration");

                        List<GalFlights> flights = ConvertDataTableToList(FlightSegments);

                        var flightDurations = from flight in flights
                                              group flight by flight.Option_Id into flightGroup
                                              select new
                                              {
                                                  Option_Id = flightGroup.Key,
                                                  TotalDuration = FormatDuration(CalculateTotalDuration(flightGroup))
                                              };

                        foreach (DataRow fl in FlightSegments.Rows)
                        {
                            foreach (var itm in flightDurations)
                            {
                                if (fl["Option_Id"].ToString() == itm.Option_Id.ToString())
                                {
                                    fl["TotalDuration"] = itm.TotalDuration;
                                }
                            }

                        }

                        var JSONString = DataTableToJSONWithStringBuilder(FlightSegments);
                        return JSONString;
                        #endregion
                    }
                    else if (tripType == "Return")
                    {
                        #region return
                        //Session["Connection"] = ds.Tables["Connection"];
                        //Session["securitytoken"] = ds.Tables["LowFareSearchRsp"].Rows[0]["TransactionId"].ToString();
                        //Session["convid"] = ds.Tables["LowFareSearchRsp"].Rows[0]["TraceId"].ToString();

                        var distinctCarriers = ds.Tables["AirSegment"].AsEnumerable()
                       .Select(row => new
                       {
                           carriers = row.Field<string>("carrier"),
                       })
                       .Distinct()
                       .ToList();

                        string commaSeparatedValues = string.Join(",", distinctCarriers);
                        commaSeparatedValues = commaSeparatedValues.Replace("{ carriers = ", "");
                        commaSeparatedValues = commaSeparatedValues.Replace("}", "");

                        var CarrierName = (from x in sql.Airlines
                                           .Where(x => commaSeparatedValues.Contains(x.IataCode))
                                           select new
                                           {
                                               x.IataCode,
                                               x.AirlineName
                                           }).ToList();

                        ds.Tables["AirSegment"].Columns.Add("Direct");

                        foreach (DataRow dr in ds.Tables["AirSegment"].Rows)
                        {
                            if (dr["Origin"].ToString() == from && dr["Destination"].ToString() == to)
                            {
                                dr["Direct"] = 1;
                            }
                            else
                            {
                                dr["Direct"] = 0;
                            }
                        }
                        // ds.Tables["AirSegment"].Columns.Add("Flight");
                        DataColumn Col = ds.Tables["AirSegment"].Columns.Add("Flight", System.Type.GetType("System.String"));
                        Col.SetOrdinal(7);
                        foreach (DataRow dr in ds.Tables["AirSegment"].Rows)
                        {
                            if (dr["Origin"].ToString() == from)
                            {

                                dr["Flight"] = "DM"; //Departture Flight
                            }
                            else if (dr["Origin"].ToString() == to)
                            {
                                dr["Flight"] = "RM"; //Return Flight
                            }
                            else if ((dr["Origin"].ToString() != from || dr["Origin"].ToString() == to) && dr["Destination"].ToString() == to)
                            {
                                dr["Flight"] = "DC"; //Connecting Flight
                            }
                            else if ((dr["Origin"].ToString() != from || dr["Origin"].ToString() == to) && dr["Destination"].ToString() == from)
                            {
                                dr["Flight"] = "RC"; //Connecting Flight
                            }
                            else
                            {
                                dr["Flight"] = "DC"; //Connecting Flight

                            }
                        }
                        ds.Tables["BookingInfo"].Columns.Add("PaxType");

                        foreach (DataRow dr in ds.Tables["BookingInfo"].Rows)
                        {
                            string id = dr["option_id"].ToString();
                            DataTable BookingInfo = ds.Tables["BookingInfo"];
                            DataTable FareInfoRef = ds.Tables["FareInfoRef"];
                            DataTable AirPricingInfo = ds.Tables["AirPricingInfo"];
                            DataTable PassengerType = ds.Tables["PassengerType"];

                            string ff = (BookingInfo.AsEnumerable().Where(p => p["option_id"].ToString() == id).Select(p => p["FareInfoRef"].ToString())).FirstOrDefault();

                            var AirPricingInfo_id = (FareInfoRef.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["AirPricingInfo_id"].ToString())).FirstOrDefault();

                            string PaxType = (PassengerType.AsEnumerable().Where(p => Convert.ToInt32(p["AirPricingInfo_id"].ToString()) == Convert.ToInt32(AirPricingInfo_id)).Select(p => p["Code"].ToString())).FirstOrDefault();

                            if (PaxType == "ADT")
                            {
                                dr["PaxType"] = "P";
                            }
                            else if (PaxType == "CNN")
                            {
                                dr["PaxType"] = "C";
                            }
                            else if (PaxType == "INF")
                            {
                                dr["PaxType"] = "I";
                            }
                        }

                        DataTable dt5 = new DataTable();
                        dt5 = ds.Tables["BookingInfo"].Copy();
                        dt5.Columns.Add("AdultTotalPrice");
                        dt5.Columns.Add("AdultBasePrice");
                        dt5.Columns.Add("AdultTaxes");
                        dt5.Columns.Add("ChildTotalPrice");
                        dt5.Columns.Add("ChildBasePrice");
                        dt5.Columns.Add("ChildTaxes");
                        dt5.Columns.Add("InfantTotalPrice");
                        dt5.Columns.Add("InfantBasePrice");
                        dt5.Columns.Add("InfantTaxes");
                        dt5.Columns.Add("FareBasis");
                        dt5.Columns.Add("SegmentIndex");
                        dt5.Columns.Add("MaxWeightValue");
                        dt5.Columns.Add("MaxWeightUnit");

                        foreach (DataRow dr in dt5.Rows)
                        {
                            string id = dr["option_id"].ToString();
                            DataTable BookingInfo = ds.Tables["BookingInfo"];
                            DataTable FareInfoRef = ds.Tables["FareInfoRef"];
                            DataTable FareInfo = ds.Tables["FareInfo"];
                            DataTable AirPricingInfo = ds.Tables["AirPricingInfo"];
                            DataTable AirPricePoint = ds.Tables["AirPricePoint"];
                            DataTable PassengerType = ds.Tables["PassengerType"];
                            DataTable Connection = ds.Tables["Connection"];

                            string ff = (BookingInfo.AsEnumerable().Where(p => p["option_id"].ToString() == id).Select(p => p["FareInfoRef"].ToString())).FirstOrDefault();

                            string SegmentIndex = "0";

                            if (Connection != null)
                            {
                                SegmentIndex = (Connection.AsEnumerable().Where(p => p["option_id"].ToString() == id).Select(p => p["SegmentIndex"].ToString())).FirstOrDefault();
                            }

                            // Check if the DataTable contains the column "baggageallowance_id"
                            if (!ds.Tables["baggageallowance"].Columns.Contains("baggageallowance_id"))
                            {
                                // If the column doesn't exist, add it
                                ds.Tables["baggageallowance"].Columns.Add("baggageallowance_id", typeof(int));

                                int idCounter = 1;

                                // Iterate through the rows of the DataTable and assign unique IDs
                                foreach (DataRow row in ds.Tables["baggageallowance"].Rows)
                                {
                                    // Assign the unique ID to the "baggageallowance_id" column
                                    row["baggageallowance_id"] = idCounter++;

                                    // If you want to break the loop when the counter reaches a certain value,
                                    // you can add a condition here, for example:
                                    // if (idCounter > maxId) break;
                                }
                            }

                            string MaxWeightValue = "";
                            string MaxWeightUnit = "";

                            var AirPricingInfo_id = (FareInfoRef.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["AirPricingInfo_id"].ToString())).FirstOrDefault();

                            string PaxType = (PassengerType.AsEnumerable().Where(p => Convert.ToInt32(p["AirPricingInfo_id"].ToString()) == Convert.ToInt32(AirPricingInfo_id)).Select(p => p["Code"].ToString())).FirstOrDefault();

                            var FareBasisCode = (FareInfo.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["FareBasis"].ToString())).FirstOrDefault();

                            var FareInfo_id = (FareInfo.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["FareInfo_id"].ToString())).FirstOrDefault();

                            var baggageallowance_id = (ds.Tables["baggageallowance"].AsEnumerable().Where(p => p["FareInfo_id"].ToString() == FareInfo_id).Select(p => p["baggageallowance_id"].ToString())).FirstOrDefault();

                            if (!ds.Tables.Contains("maxweight"))
                            {
                                if (ds.Tables["baggageallowance"].Columns.Contains("maxweight"))
                                {
                                    MaxWeightValue = (ds.Tables["baggageallowance"].AsEnumerable().Where(p => p["baggageallowance_id"].ToString() == baggageallowance_id).Select(p => p["maxweight"].ToString())).FirstOrDefault();
                                }
                                //if (ds.Tables["baggageallowance"].Columns.Contains("NumberofPieces"))
                                //{
                                //    MaxWeightUnit = (ds.Tables["baggageallowance"].AsEnumerable().Where(p => p["baggageallowance_id"].ToString() == baggageallowance_id).Select(p => p["NumberofPieces"].ToString())).FirstOrDefault();
                                //}
                            }
                            else
                            {
                                MaxWeightValue = (ds.Tables["maxweight"].AsEnumerable().Where(p => p["baggageallowance_id"].ToString() == baggageallowance_id).Select(p => p["value"].ToString())).FirstOrDefault();
                                MaxWeightUnit = (ds.Tables["maxweight"].AsEnumerable().Where(p => p["baggageallowance_id"].ToString() == baggageallowance_id).Select(p => p["unit"].ToString())).FirstOrDefault();

                            }


                            //AirPricePoint
                            var APInfo = (AirPricingInfo.AsEnumerable().Where(p => p["AirPricingInfo_id"].ToString() == AirPricingInfo_id).Select(p => p["AirPricePoint_Id"].ToString())).FirstOrDefault();

                            var APP = from d in AirPricePoint.AsEnumerable()
                                      where d.Field<int>("AirPricePoint_Id") == Convert.ToInt32(APInfo)
                                      select new
                                      {
                                          TotalPrice = d.Field<string>("ApproximateTotalPrice"),
                                          BasePrice = d.Field<string>("ApproximateBasePrice"),
                                          Taxes = d.Field<string>("Taxes")
                                      };

                            dr["MaxWeightValue"] = MaxWeightValue;
                            dr["MaxWeightUnit"] = MaxWeightUnit;
                            dr["SegmentIndex"] = SegmentIndex;
                            dr["FareBasis"] = FareBasisCode;

                            foreach (var itm in APP)
                            {
                                if (PaxType == "ADT")
                                {
                                    dr["AdultBasePrice"] = itm.BasePrice;
                                    dr["AdultTotalPrice"] = itm.TotalPrice;
                                    dr["AdultTaxes"] = itm.Taxes;
                                }
                                else if (PaxType == "CNN")
                                {
                                    dr["ChildBasePrice"] = itm.BasePrice;
                                    dr["ChildTotalPrice"] = itm.TotalPrice;
                                    dr["ChildTaxes"] = itm.Taxes;
                                }
                                else if (PaxType == "INF")
                                {
                                    dr["InfantBasePrice"] = itm.BasePrice;
                                    dr["InfantTotalPrice"] = itm.TotalPrice;
                                    dr["InfantTaxes"] = itm.Taxes;
                                }
                            }
                        }

                        FlightSegments = ds.Tables["BookingInfo"].Copy();
                        FlightSegments.Columns.Add("AdultTotalPrice");
                        FlightSegments.Columns.Add("AdultBasePrice");
                        FlightSegments.Columns.Add("AdultTaxes");
                        FlightSegments.Columns.Add("ChildTotalPrice");
                        FlightSegments.Columns.Add("ChildBasePrice");
                        FlightSegments.Columns.Add("ChildTaxes");
                        FlightSegments.Columns.Add("InfantTotalPrice");
                        FlightSegments.Columns.Add("InfantBasePrice");
                        FlightSegments.Columns.Add("InfantTaxes");
                        FlightSegments.Columns.Add("FareBasis");
                        FlightSegments.Columns.Add("GrandTotal");
                        FlightSegments.Columns.Add("SegmentIndex");
                        FlightSegments.Columns.Add("MaxWeightValue");
                        FlightSegments.Columns.Add("MaxWeightUnit");


                        foreach (DataRow dr in FlightSegments.Rows)
                        {
                            //if (dr["PaxType"].ToString() == "P")
                            {
                                foreach (DataRow dr1 in dt5.Rows)
                                {
                                    dr["SegmentIndex"] = dr1["SegmentIndex"];
                                    dr["MaxWeightValue"] = dr1["MaxWeightValue"];
                                    dr["MaxWeightUnit"] = dr1["MaxWeightUnit"];
                                    // dr["FareBasis"] = dr1["FareBasis"];
                                    if (dr["SegmentRef"].ToString() == dr1["SegmentRef"].ToString() && (dr1["PaxType"].ToString() == "P") && dr["BookingCode"].ToString() == dr1["BookingCode"].ToString())
                                    {
                                        dr["AdultBasePrice"] = dr1["AdultBasePrice"];
                                        dr["AdultTotalPrice"] = dr1["AdultTotalPrice"];
                                        dr["AdultTaxes"] = dr1["AdultTaxes"];
                                        break;
                                    }
                                    else if (dr["SegmentRef"].ToString() == dr1["SegmentRef"].ToString() && (dr1["PaxType"].ToString() == "C") && dr["BookingCode"].ToString() == dr1["BookingCode"].ToString())
                                    {
                                        dr["ChildBasePrice"] = dr1["ChildBasePrice"];
                                        dr["ChildTotalPrice"] = dr1["ChildTotalPrice"];
                                        dr["ChildTaxes"] = dr1["ChildTaxes"];
                                        break;
                                    }
                                    else if (dr["SegmentRef"].ToString() == dr1["SegmentRef"].ToString() && (dr1["PaxType"].ToString() == "I") && dr["BookingCode"].ToString() == dr1["BookingCode"].ToString())
                                    {
                                        dr["InfantBasePrice"] = dr1["InfantBasePrice"];
                                        dr["InfantTotalPrice"] = dr1["InfantTotalPrice"];
                                        dr["InfantTaxes"] = dr1["InfantTaxes"];
                                        break;
                                    }
                                }
                            }
                        }

                        for (int t = FlightSegments.Rows.Count - 1; t >= 0; t--)
                        {
                            if (FlightSegments.Rows[t]["PaxType"].ToString() == "C" || FlightSegments.Rows[t]["PaxType"].ToString() == "I")
                            {
                                FlightSegments.Rows[t].Delete();
                                FlightSegments.AcceptChanges();
                            }
                        }

                        foreach (DataRow itm in FlightSegments.Rows)
                        {
                            if (string.IsNullOrEmpty(itm["AdultTotalPrice"].ToString()) == true)
                            {
                                itm["AdultTotalPrice"] = "0";
                            }
                            else
                            {
                                itm["AdultTotalPrice"] = Convert.ToString(itm["AdultTotalPrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["AdultBasePrice"].ToString()) == true)
                            {
                                itm["AdultBasePrice"] = "0";
                            }
                            else
                            {
                                itm["AdultBasePrice"] = Convert.ToString(itm["AdultBasePrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["AdultTaxes"].ToString()) == true)
                            {
                                itm["AdultTaxes"] = "0";
                            }
                            else
                            {
                                itm["AdultTaxes"] = Convert.ToString(itm["AdultTaxes"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["ChildTotalPrice"].ToString()) == true)
                            {
                                itm["ChildTotalPrice"] = "0";
                            }
                            else
                            {
                                itm["ChildTotalPrice"] = Convert.ToString(itm["ChildTotalPrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["ChildBasePrice"].ToString()) == true)
                            {
                                itm["ChildBasePrice"] = "0";
                            }
                            else
                            {
                                itm["ChildBasePrice"] = Convert.ToString(itm["ChildBasePrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["ChildTaxes"].ToString()) == true)
                            {
                                itm["ChildTaxes"] = "0";
                            }
                            else
                            {
                                itm["ChildTaxes"] = Convert.ToString(itm["ChildTaxes"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["InfantTotalPrice"].ToString()) == true)
                            {
                                itm["InfantTotalPrice"] = "0";
                            }
                            else
                            {
                                itm["InfantTotalPrice"] = Convert.ToString(itm["InfantTotalPrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["InfantBasePrice"].ToString()) == true)
                            {
                                itm["InfantBasePrice"] = "0";
                            }
                            else
                            {
                                itm["InfantBasePrice"] = Convert.ToString(itm["InfantBasePrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["InfantTaxes"].ToString()) == true)
                            {
                                itm["InfantTaxes"] = "0";
                            }
                            else
                            {
                                itm["InfantTaxes"] = Convert.ToString(itm["InfantTaxes"]).Insert(3, " ");
                            }
                        }



                        //sorting 
                        //FlightSegments.Columns.Add("SortBy", typeof(Int32));

                        //foreach (DataRow row in FlightSegments.Rows)
                        //{
                        //    string price = Convert.ToString(row["AdultTotalPrice"]).Replace("PKR", "");
                        //    row["SortBy"] = price;
                        //}

                        //DataView dv = FlightSegments.DefaultView;
                        //dv.Sort = "SortBy";
                        //FlightSegments = dv.ToTable();

                        ds.EnforceConstraints = false;


                        FlightSegments.Columns.Add("Stops");
                        FlightSegments.Columns.Add("Direct");
                        FlightSegments.Columns.Add("Flight");
                        FlightSegments.Columns.Add("SegmentKey");
                        FlightSegments.Columns.Add("Group");
                        FlightSegments.Columns.Add("Carrier");
                        FlightSegments.Columns.Add("FlightNumber");
                        FlightSegments.Columns.Add("Origin");
                        FlightSegments.Columns.Add("Destination");
                        FlightSegments.Columns.Add("DepartureTime");
                        FlightSegments.Columns.Add("ArrivalTime");
                        FlightSegments.Columns.Add("FlightTime");
                        FlightSegments.Columns.Add("Distance");
                        FlightSegments.Columns.Add("ETicketability");
                        FlightSegments.Columns.Add("Equipment");
                        FlightSegments.Columns.Add("ChangeOfPlane");
                        FlightSegments.Columns.Add("ParticipantLevel");
                        FlightSegments.Columns.Add("LinkAvailability");
                        FlightSegments.Columns.Add("PolledAvailabilityOption");
                        FlightSegments.Columns.Add("OptionalServicesIndicator");
                        FlightSegments.Columns.Add("AvailabilitySource");
                        FlightSegments.Columns.Add("AvailabilityDisplayType");
                        FlightSegments.Columns.Add("ProviderCode");
                        FlightSegments.Columns.Add("ArrTime"); //Final Arrival Time
                        FlightSegments.Columns.Add("FinalDestination");


                        foreach (DataRow row in FlightSegments.Rows)
                        {
                            foreach (DataRow row1 in ds.Tables["AirSegment"].Rows)
                            {
                                if (row["SegmentRef"].ToString() == row1["Key"].ToString())
                                {
                                    row["ProviderCode"] = "1G";
                                    row["Direct"] = row1["Direct"];
                                    row["Flight"] = row1["Flight"];
                                    row["SegmentKey"] = row1["Key"];
                                    row["Group"] = row1["Group"];
                                    row["Carrier"] = row1["Carrier"];
                                    row["FlightNumber"] = row1["FlightNumber"];
                                    row["Origin"] = row1["Origin"];
                                    row["Destination"] = row1["Destination"];
                                    row["DepartureTime"] = row1["DepartureTime"];
                                    row["ArrivalTime"] = row1["ArrivalTime"];
                                    row["FlightTime"] = row1["FlightTime"];
                                    row["Distance"] = row1["Distance"];
                                    row["ETicketability"] = row1["ETicketability"];
                                    row["Equipment"] = row1["Equipment"];
                                    row["ChangeOfPlane"] = row1["ChangeOfPlane"];
                                    if (ds.Tables["AirSegment"].Columns.Contains("ParticipantLevel"))
                                    {
                                        row["ParticipantLevel"] = row1["ParticipantLevel"];
                                    }

                                    if (ds.Tables["AirSegment"].Columns.Contains("LinkAvailability"))
                                    {
                                        row["LinkAvailability"] = row1["LinkAvailability"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("PolledAvailabilityOption"))
                                    {
                                        row["PolledAvailabilityOption"] = row1["PolledAvailabilityOption"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("OptionalServicesIndicator"))
                                    {
                                        row["OptionalServicesIndicator"] = row1["OptionalServicesIndicator"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("AvailabilitySource"))
                                    {
                                        row["AvailabilitySource"] = row1["AvailabilitySource"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("AvailabilityDisplayType"))
                                    {
                                        row["AvailabilityDisplayType"] = row1["AvailabilityDisplayType"];
                                    }

                                    row["FinalDestination"] = to;
                                }
                            }
                        }

                        foreach (DataRow ro in FlightSegments.Rows)
                        {
                            string[] adultPrice = null;
                            string[] childPrice = null;
                            string[] infantPrice = null;
                            decimal grandTotal = 0;
                            decimal _adultPrice = 0;
                            decimal _childPrice = 0;
                            decimal _infantPrice = 0;

                            if (ro["AdultTotalPrice"].ToString().IndexOf(' ') > -1)
                            {
                                adultPrice = ro["AdultTotalPrice"].ToString().Split(' ');
                                _adultPrice = Convert.ToDecimal(adultPrice[1]);
                                //grandTotal = grandTotal + Convert.ToDecimal(adultPrice[1]);
                            }

                            if (ro["ChildTotalPrice"].ToString().IndexOf(' ') > -1)
                            {
                                childPrice = ro["ChildTotalPrice"].ToString().Split(' ');
                                _childPrice = Convert.ToDecimal(childPrice[1]);
                                // grandTotal = grandTotal + Convert.ToDecimal(childPrice[1]);
                            }

                            if (ro["InfantTotalPrice"].ToString().IndexOf(' ') > -1)
                            {
                                infantPrice = ro["InfantTotalPrice"].ToString().Split(' ');
                                _infantPrice = Convert.ToDecimal(infantPrice[1]);
                                // grandTotal = grandTotal + Convert.ToDecimal(infantPrice[1]);
                            }

                            decimal[] TotalAmt = _AddMarkup(_adultPrice, _childPrice, _infantPrice, adults, children, infants, ro["carrier"].ToString(), userid);

                            //decimal[] TotalAmt = _AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Childs, Infants, AirlineCode);

                            //totalfare = TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                            grandTotal = TotalAmt[2] + TotalAmt[3] + TotalAmt[4] + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                            ro["GrandTotal"] = grandTotal;
                        }

                        foreach (DataRow dr in FlightSegments.Rows)
                        {
                            string id = dr["SegmentKey"].ToString();
                            DataTable BookingInfo = ds.Tables["BookingInfo"];
                            DataTable FareInfoRef = ds.Tables["FareInfoRef"];
                            DataTable FareInfo = ds.Tables["FareInfo"];
                            DataTable AirPricingInfo = ds.Tables["AirPricingInfo"];
                            DataTable PassengerType = ds.Tables["PassengerType"];

                            string ff = (BookingInfo.AsEnumerable().Where(p => p["SegmentRef"].ToString() == id && p["BookingCode"].ToString() == dr["BookingCode"].ToString()).Select(p => p["FareInfoRef"].ToString())).FirstOrDefault();

                            var AirPricingInfo_id = (FareInfoRef.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["AirPricingInfo_id"].ToString())).FirstOrDefault();

                            string PaxType = (PassengerType.AsEnumerable().Where(p => Convert.ToInt32(p["AirPricingInfo_id"].ToString()) == Convert.ToInt32(AirPricingInfo_id)).Select(p => p["Code"].ToString())).FirstOrDefault();

                            int count = FlightSegments.AsEnumerable().Count(row => row.Field<int>("Option_Id") == Convert.ToInt32(dr["Option_Id"].ToString()));
                            string arrtime = "";

                            if (dr["Flight"].ToString() == "DM")
                            {
                                arrtime = FlightSegments.AsEnumerable().Where(p => p.Field<int>("Option_Id") == Convert.ToInt32(dr["Option_Id"].ToString()) && p.Field<string>("Destination") == to).Select(p => p["ArrivalTime"].ToString()).FirstOrDefault();
                            }
                            else if (dr["Flight"].ToString() == "RM")
                            {
                                arrtime = FlightSegments.AsEnumerable().Where(p => p.Field<int>("Option_Id") == Convert.ToInt32(dr["Option_Id"].ToString()) && p.Field<string>("Destination") == from).Select(p => p["ArrivalTime"].ToString()).FirstOrDefault();
                            }
                            else
                            {
                                arrtime = dr["ArrivalTime"].ToString();

                            }

                            var FareBasisCode = (FareInfo.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["FareBasis"].ToString())).FirstOrDefault();

                            dr["FareBasis"] = FareBasisCode;

                            if (Arr.IndexOf("T") > -1)
                            {
                                dr["arrtime"] = arrtime;
                            }
                            else
                            {
                                dr["arrtime"] = dr["ArrivalTime"];
                            }

                            dr["stops"] = (count - 1).ToString();

                        }

                        DataTable dtReturn = FlightSegments.Copy();

                        for (int b = dtReturn.Rows.Count - 1; b >= 0; b--)
                        {
                            if (dtReturn.Rows[b]["Flight"].ToString() != "RM")
                            {
                                dtReturn.Rows[b].Delete();
                                dtReturn.AcceptChanges();
                            }
                        }

                        FlightSegments.Columns.Add("TransactionId");

                        Guid TransactionId = Guid.NewGuid();

                        foreach (DataRow dr in FlightSegments.Rows)
                        {
                            dr["TransactionId"] = TransactionId;
                        }

                        FlightSegments.Columns.Add("ReturnOption_Id");
                        FlightSegments.Columns["Flight"].SetOrdinal(0);
                        FlightSegments.Columns["Option_Id"].SetOrdinal(1);
                        FlightSegments.Columns["ReturnOption_Id"].SetOrdinal(2);


                        //DataColumn Col1 = FlightSegments.Columns.Add("FlightOption_Id", System.Type.GetType("System.String"));
                        //Col1.SetOrdinal(4);

                        //  int o = 0;
                        //foreach (DataRow row in FlightSegments.Rows)
                        for (int o = 0; o < FlightSegments.Rows.Count - 1; o++)
                        {
                            //if (o >= FlightSegments.Rows.Count)
                            //{
                            //    break;
                            //}

                            if (FlightSegments.Rows[o]["Flight"].ToString() == "DM")
                            {
                                for (int s = o + 1; s < FlightSegments.Rows.Count; s++)
                                    if (FlightSegments.Rows[s]["Flight"].ToString() == "RM")// && (FlightSegments.Rows[s]["ReturnOption_Id"] == "" || FlightSegments.Rows[s]["ReturnOption_Id"] == DBNull.Value))
                                    {
                                        FlightSegments.Rows[o]["ReturnOption_Id"] = FlightSegments.Rows[s]["Option_Id"];
                                        FlightSegments.Rows[s]["ReturnOption_Id"] = FlightSegments.Rows[o]["Option_Id"];
                                        break;
                                    }
                                //o++;
                            }
                            // }
                        }

                        FlightSegments.AcceptChanges();



                        for (int i = FlightSegments.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow row = FlightSegments.Rows[i];

                            // Check if the condition is true for the current row
                            if (FlightSegments.Rows[i]["Carrier"].ToString() == "PK")
                            {
                                // Delete the row
                                FlightSegments.Rows.Remove(row);
                            }
                            else
                            {
                                // Handle the else case, if needed
                            }
                        }

                        FlightSegments.Columns.Add("TotalDuration");

                        List<GalFlights> flights = ConvertDataTableToList(FlightSegments);

                        var flightDurations = from flight in flights
                                              group flight by flight.Option_Id into flightGroup
                                              select new
                                              {
                                                  Option_Id = flightGroup.Key,
                                                  TotalDuration = FormatDuration(CalculateTotalDuration(flightGroup))
                                              };

                        foreach (DataRow fl in FlightSegments.Rows)
                        {
                            foreach (var itm in flightDurations)
                            {
                                if (fl["Option_Id"].ToString() == itm.Option_Id.ToString())
                                {
                                    fl["TotalDuration"] = itm.TotalDuration;
                                }
                            }

                        }

                        var JSONString = DataTableToJSONWithStringBuilder(FlightSegments);
                        return JSONString;

                        #endregion
                    }
                    else if (tripType == "Other")
                    {
                        #region other

                        MultipleDistenations md = new MultipleDistenations();

                        md.From1 = fromM_1;
                        md.From2 = fromM_2;
                        md.From3 = fromM_3;
                        md.From4 = fromM_4;
                        md.From5 = fromM_5;
                        md.From6 = fromM_6;
                        md.To1 = toM_1;
                        md.To2 = toM_2;
                        md.To3 = toM_3;
                        md.To4 = toM_4;
                        md.To5 = toM_5;
                        md.To6 = toM_6;

                        DataTable AirPricingInfo1 = ds.Tables["AirPricingInfo"].Copy();
                        DataTable FlightOptionsList = ds.Tables["FlightOptionsList"].Copy();
                        DataTable FlightOptions = ds.Tables["FlightOption"].Copy();
                        DataTable Option = ds.Tables["Option"].Copy();
                        DataTable BookingInfo1 = ds.Tables["BookingInfo"].Copy();
                        DataTable AirSegment = ds.Tables["AirSegment"].Copy();

                        BookingInfo1.Columns.Add("FlightOption_id");
                        BookingInfo1.Columns.Add("FlightOptionsList_id");
                        BookingInfo1.Columns.Add("AirPricingInfo_id");
                        BookingInfo1.Columns.Add("SegmentKey");
                        BookingInfo1.Columns.Add("Group");
                        BookingInfo1.Columns.Add("Carrier");
                        BookingInfo1.Columns.Add("FlightNumber");
                        BookingInfo1.Columns.Add("Origin");
                        BookingInfo1.Columns.Add("Destination");
                        BookingInfo1.Columns.Add("DepartureTime");
                        BookingInfo1.Columns.Add("ArrivalTime");
                        BookingInfo1.Columns.Add("FlightTime");
                        BookingInfo1.Columns.Add("Distance");
                        BookingInfo1.Columns.Add("ETicketability");
                        BookingInfo1.Columns.Add("Equipment");
                        BookingInfo1.Columns.Add("ChangeOfPlane");
                        BookingInfo1.Columns.Add("ParticipantLevel");
                        BookingInfo1.Columns.Add("LinkAvailability");
                        BookingInfo1.Columns.Add("PolledAvailabilityOption");
                        BookingInfo1.Columns.Add("OptionalServicesIndicator");
                        BookingInfo1.Columns.Add("AvailabilitySource");
                        BookingInfo1.Columns.Add("AvailabilityDisplayType");


                        foreach (DataRow dr in BookingInfo1.Rows)
                        {
                            foreach (DataRow dr1 in Option.Rows)
                            {
                                if (dr["Option_id"].ToString() == dr1["Option_id"].ToString())
                                {
                                    dr["FlightOption_id"] = dr1["FlightOption_id"];
                                }
                            }
                        }

                        foreach (DataRow dr in BookingInfo1.Rows)
                        {
                            foreach (DataRow dr1 in FlightOptions.Rows)
                            {
                                if (dr["FlightOption_id"].ToString() == dr1["FlightOption_id"].ToString())
                                {
                                    dr["FlightOptionsList_id"] = dr1["FlightOptionsList_id"];
                                }
                            }
                        }

                        foreach (DataRow dr in BookingInfo1.Rows)
                        {
                            foreach (DataRow dr1 in FlightOptionsList.Rows)
                            {
                                if (dr["FlightOptionsList_id"].ToString() == dr1["FlightOptionsList_id"].ToString())
                                {
                                    dr["AirPricingInfo_id"] = Convert.ToInt32(dr1["AirPricingInfo_id"]);
                                }
                            }
                        }

                        foreach (DataRow dr in BookingInfo1.Rows)
                        {
                            foreach (DataRow dr1 in AirSegment.Rows)
                            {
                                if (dr["SegmentRef"].ToString() == dr1["key"].ToString())
                                {
                                    dr["SegmentKey"] = dr1["Key"];
                                    dr["Group"] = dr1["Group"];
                                    dr["Carrier"] = dr1["Carrier"];
                                    dr["FlightNumber"] = dr1["FlightNumber"];
                                    dr["Origin"] = dr1["Origin"];
                                    dr["Destination"] = dr1["Destination"];
                                    dr["DepartureTime"] = dr1["DepartureTime"];
                                    dr["ArrivalTime"] = dr1["ArrivalTime"];
                                    dr["FlightTime"] = dr1["FlightTime"];
                                    dr["Distance"] = dr1["Distance"];
                                    dr["ETicketability"] = dr1["ETicketability"];
                                    dr["Equipment"] = dr1["Equipment"];
                                    dr["ChangeOfPlane"] = dr1["ChangeOfPlane"];
                                    dr["ParticipantLevel"] = dr1["ParticipantLevel"];
                                    dr["LinkAvailability"] = dr1["LinkAvailability"];
                                    dr["PolledAvailabilityOption"] = dr1["PolledAvailabilityOption"];
                                    dr["OptionalServicesIndicator"] = dr1["OptionalServicesIndicator"];
                                    dr["AvailabilitySource"] = dr1["AvailabilitySource"];
                                    dr["AvailabilityDisplayType"] = dr1["AvailabilityDisplayType"];
                                }
                            }
                        }

                        ds.Tables["AirSegment"].Columns.Add("Direct");

                        foreach (DataRow dr in ds.Tables["AirSegment"].Rows)
                        {
                            if (dr["group"].ToString() == "0")
                            {
                                if (dr["Origin"].ToString() == fromM_1 && dr["Destination"].ToString() == toM_1)
                                {
                                    dr["Direct"] = 1;
                                }
                                else
                                {
                                    dr["Direct"] = 0;
                                }
                            }
                            if (dr["group"].ToString() == "1")
                            {
                                if (dr["Origin"].ToString() == fromM_2 && dr["Destination"].ToString() == toM_2)
                                {
                                    dr["Direct"] = 1;
                                }
                                else
                                {
                                    dr["Direct"] = 0;
                                }
                            }
                            if (dr["group"].ToString() == "2")
                            {
                                if (dr["Origin"].ToString() == fromM_3 && dr["Destination"].ToString() == toM_3)
                                {
                                    dr["Direct"] = 1;
                                }
                                else
                                {
                                    dr["Direct"] = 0;
                                }
                            }
                            if (dr["group"].ToString() == "3")
                            {
                                if (dr["Origin"].ToString() == fromM_4 && dr["Destination"].ToString() == toM_4)
                                {
                                    dr["Direct"] = 1;
                                }
                                else
                                {
                                    dr["Direct"] = 0;
                                }
                            }
                            if (dr["group"].ToString() == "4")
                            {
                                if (dr["Origin"].ToString() == fromM_5 && dr["Destination"].ToString() == toM_5)
                                {
                                    dr["Direct"] = 1;
                                }
                                else
                                {
                                    dr["Direct"] = 0;
                                }
                            }
                            if (dr["group"].ToString() == "5")
                            {
                                if (dr["Origin"].ToString() == fromM_6 && dr["Destination"].ToString() == toM_6)
                                {
                                    dr["Direct"] = 1;
                                }
                                else
                                {
                                    dr["Direct"] = 0;
                                }
                            }
                        }
                        ds.Tables["AirSegment"].Columns.Add("Flight");
                        //int i = 1;

                        foreach (DataRow dr in ds.Tables["AirSegment"].Rows)
                        {
                            if (dr["group"].ToString() == "0")
                            {
                                if (dr["Origin"].ToString() != fromM_1)
                                {
                                    dr["Flight"] = "C"; //Connecting Flight
                                }
                                else
                                {
                                    dr["Flight"] = "M"; //Main Flight
                                }
                            }
                            if (dr["group"].ToString() == "1")
                            {
                                if (dr["Origin"].ToString() != fromM_2)
                                {
                                    dr["Flight"] = "C"; //Connecting Flight
                                }
                                else
                                {
                                    dr["Flight"] = "M"; //Main Flight
                                }
                            }
                            if (dr["group"].ToString() == "2")
                            {
                                if (dr["Origin"].ToString() != fromM_3)
                                {
                                    dr["Flight"] = "C"; //Connecting Flight
                                }
                                else
                                {
                                    dr["Flight"] = "M"; //Main Flight
                                }
                            }
                            if (dr["group"].ToString() == "3")
                            {
                                if (dr["Origin"].ToString() != fromM_4)
                                {
                                    dr["Flight"] = "C"; //Connecting Flight
                                }
                                else
                                {
                                    dr["Flight"] = "M"; //Main Flight
                                }
                            }
                            if (dr["group"].ToString() == "4")
                            {
                                if (dr["Origin"].ToString() != fromM_5)
                                {
                                    dr["Flight"] = "C"; //Connecting Flight
                                }
                                else
                                {
                                    dr["Flight"] = "M"; //Main Flight
                                }
                            }
                            if (dr["group"].ToString() == "5")
                            {
                                if (dr["Origin"].ToString() != fromM_6)
                                {
                                    dr["Flight"] = "C"; //Connecting Flight
                                }
                                else
                                {
                                    dr["Flight"] = "M"; //Main Flight
                                }
                            }
                        }
                        ds.Tables["BookingInfo"].Columns.Add("PaxType");

                        foreach (DataRow dr in ds.Tables["BookingInfo"].Rows)
                        {
                            string id = dr["option_id"].ToString();
                            DataTable BookingInfo = ds.Tables["BookingInfo"];
                            DataTable FareInfoRef = ds.Tables["FareInfoRef"];
                            DataTable AirPricingInfo = ds.Tables["AirPricingInfo"];
                            DataTable PassengerType = ds.Tables["PassengerType"];

                            string ff = (BookingInfo.AsEnumerable().Where(p => p["option_id"].ToString() == id).Select(p => p["FareInfoRef"].ToString())).FirstOrDefault();

                            var AirPricingInfo_id = (FareInfoRef.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["AirPricingInfo_id"].ToString())).FirstOrDefault();

                            string PaxType = (PassengerType.AsEnumerable().Where(p => Convert.ToInt32(p["AirPricingInfo_id"].ToString()) == Convert.ToInt32(AirPricingInfo_id)).Select(p => p["Code"].ToString())).FirstOrDefault();

                            if (PaxType == "ADT")
                            {
                                dr["PaxType"] = "P";
                            }
                            else if (PaxType == "CNN")
                            {
                                dr["PaxType"] = "C";
                            }
                            else if (PaxType == "INF")
                            {
                                dr["PaxType"] = "I";
                            }
                        }

                        DataTable dt5 = new DataTable();
                        dt5 = ds.Tables["BookingInfo"].Copy();
                        dt5.Columns.Add("AdultTotalPrice");
                        dt5.Columns.Add("AdultBasePrice");
                        dt5.Columns.Add("AdultTaxes");
                        dt5.Columns.Add("ChildTotalPrice");
                        dt5.Columns.Add("ChildBasePrice");
                        dt5.Columns.Add("ChildTaxes");
                        dt5.Columns.Add("InfantTotalPrice");
                        dt5.Columns.Add("InfantBasePrice");
                        dt5.Columns.Add("InfantTaxes");
                        dt5.Columns.Add("FareBasis");
                        dt5.Columns.Add("SegmentIndex");


                        foreach (DataRow dr in dt5.Rows)
                        {
                            string id = dr["option_id"].ToString();
                            DataTable BookingInfo = ds.Tables["BookingInfo"];
                            DataTable FareInfoRef = ds.Tables["FareInfoRef"];
                            DataTable FareInfo = ds.Tables["FareInfo"];
                            DataTable AirPricingInfo = ds.Tables["AirPricingInfo"];
                            DataTable AirPricePoint = ds.Tables["AirPricePoint"];
                            DataTable PassengerType = ds.Tables["PassengerType"];
                            DataTable Connection = ds.Tables["Connection"];

                            string ff = (BookingInfo.AsEnumerable().Where(p => p["option_id"].ToString() == id).Select(p => p["FareInfoRef"].ToString())).FirstOrDefault();
                            string SegmentIndex = "0";

                            if (Connection != null)
                            {
                                SegmentIndex = (Connection.AsEnumerable().Where(p => p["option_id"].ToString() == id).Select(p => p["SegmentIndex"].ToString())).FirstOrDefault();
                            }
                            var AirPricingInfo_id = (FareInfoRef.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["AirPricingInfo_id"].ToString())).FirstOrDefault();

                            string PaxType = (PassengerType.AsEnumerable().Where(p => Convert.ToInt32(p["AirPricingInfo_id"].ToString()) == Convert.ToInt32(AirPricingInfo_id)).Select(p => p["Code"].ToString())).FirstOrDefault();

                            var FareBasisCode = (FareInfo.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["FareBasis"].ToString())).FirstOrDefault();

                            //AirPricePoint
                            var APInfo = (AirPricingInfo.AsEnumerable().Where(p => p["AirPricingInfo_id"].ToString() ==
                            AirPricingInfo_id).Select(p => p["AirPricePoint_Id"].ToString())).FirstOrDefault();


                            var APP = from d in AirPricePoint.AsEnumerable()
                                      where d.Field<int>("AirPricePoint_Id") == Convert.ToInt32(APInfo)
                                      select new
                                      {
                                          TotalPrice = d.Field<string>("ApproximateTotalPrice"),
                                          BasePrice = d.Field<string>("ApproximateBasePrice"),
                                          Taxes = d.Field<string>("Taxes")
                                      };



                            dr["FareBasis"] = FareBasisCode;
                            dr["SegmentIndex"] = SegmentIndex;

                            foreach (var itm in APP)
                            {
                                if (PaxType == "ADT")
                                {
                                    dr["AdultBasePrice"] = itm.BasePrice;
                                    dr["AdultTotalPrice"] = itm.TotalPrice;
                                    dr["AdultTaxes"] = itm.Taxes;
                                }
                                else if (PaxType == "CNN")
                                {
                                    dr["ChildBasePrice"] = itm.BasePrice;
                                    dr["ChildTotalPrice"] = itm.TotalPrice;
                                    dr["ChildTaxes"] = itm.Taxes;
                                }
                                else if (PaxType == "INF")
                                {
                                    dr["InfantBasePrice"] = itm.BasePrice;
                                    dr["InfantTotalPrice"] = itm.TotalPrice;
                                    dr["InfantTaxes"] = itm.Taxes;
                                }
                            }
                        }

                        FlightSegments = ds.Tables["BookingInfo"].Copy();
                        FlightSegments.Columns.Add("AdultTotalPrice");
                        FlightSegments.Columns.Add("AdultBasePrice");
                        FlightSegments.Columns.Add("AdultTaxes");
                        FlightSegments.Columns.Add("ChildTotalPrice");
                        FlightSegments.Columns.Add("ChildBasePrice");
                        FlightSegments.Columns.Add("ChildTaxes");
                        FlightSegments.Columns.Add("InfantTotalPrice");
                        FlightSegments.Columns.Add("InfantBasePrice");
                        FlightSegments.Columns.Add("InfantTaxes");
                        FlightSegments.Columns.Add("FareBasis");
                        FlightSegments.Columns.Add("SegmentIndex");

                        foreach (DataRow dr in FlightSegments.Rows)
                        {
                            //if (dr["PaxType"].ToString() == "P")
                            {
                                foreach (DataRow dr1 in dt5.Rows)
                                {
                                    dr["SegmentIndex"] = dr1["SegmentIndex"];

                                    if (dr["SegmentRef"].ToString() == dr1["SegmentRef"].ToString() && (dr1["PaxType"].ToString() == "P") && dr["BookingCode"].ToString() == dr1["BookingCode"].ToString())
                                    {
                                        dr["AdultBasePrice"] = dr1["AdultBasePrice"];
                                        dr["AdultTotalPrice"] = dr1["AdultTotalPrice"];
                                        dr["AdultTaxes"] = dr1["AdultTaxes"];
                                        dr["FareBasis"] = dr1["FareBasis"];
                                        break;
                                    }
                                    else if (dr["SegmentRef"].ToString() == dr1["SegmentRef"].ToString() && (dr1["PaxType"].ToString() == "C") && dr["BookingCode"].ToString() == dr1["BookingCode"].ToString())
                                    {
                                        dr["ChildBasePrice"] = dr1["ChildBasePrice"];
                                        dr["ChildTotalPrice"] = dr1["ChildTotalPrice"];
                                        dr["ChildTaxes"] = dr1["ChildTaxes"];
                                        break;
                                    }
                                    else if (dr["SegmentRef"].ToString() == dr1["SegmentRef"].ToString() && (dr1["PaxType"].ToString() == "I") && dr["BookingCode"].ToString() == dr1["BookingCode"].ToString())
                                    {
                                        dr["InfantBasePrice"] = dr1["InfantBasePrice"];
                                        dr["InfantTotalPrice"] = dr1["InfantTotalPrice"];
                                        dr["InfantTaxes"] = dr1["InfantTaxes"];
                                        break;
                                    }
                                }
                            }
                        }

                        //FlightSegments.Columns.Add("SortBy", typeof(Int32));

                        //foreach (DataRow row in FlightSegments.Rows)
                        //{
                        //    string price = Convert.ToString(row["AdultTotalPrice"]).Replace("PKR", "");
                        //    row["SortBy"] = Convert.ToInt32(price);
                        //}

                        //DataView dv = FlightSegments.DefaultView;
                        //dv.Sort = "SortBy asc";
                        //FlightSegments = null;
                        //FlightSegments = dv.ToTable();

                        //FlightSegments.DefaultView.Sort = "SortBy";

                        for (int t = FlightSegments.Rows.Count - 1; t >= 0; t--)
                        {
                            if (FlightSegments.Rows[t]["PaxType"].ToString() == "C" || FlightSegments.Rows[t]["PaxType"].ToString() == "I")
                            {
                                FlightSegments.Rows[t].Delete();
                                FlightSegments.AcceptChanges();
                            }
                        }

                        foreach (DataRow itm in FlightSegments.Rows)
                        {
                            if (string.IsNullOrEmpty(itm["AdultTotalPrice"].ToString()) == true)
                            {
                                itm["AdultTotalPrice"] = "0";
                            }
                            else
                            {
                                itm["AdultTotalPrice"] = Convert.ToString(itm["AdultTotalPrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["AdultBasePrice"].ToString()) == true)
                            {
                                itm["AdultBasePrice"] = "0";
                            }
                            else
                            {
                                itm["AdultBasePrice"] = Convert.ToString(itm["AdultBasePrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["AdultTaxes"].ToString()) == true)
                            {
                                itm["AdultTaxes"] = "0";
                            }
                            else
                            {
                                itm["AdultTaxes"] = Convert.ToString(itm["AdultTaxes"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["ChildTotalPrice"].ToString()) == true)
                            {
                                itm["ChildTotalPrice"] = "0";
                            }
                            else
                            {
                                itm["ChildTotalPrice"] = Convert.ToString(itm["ChildTotalPrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["ChildBasePrice"].ToString()) == true)
                            {
                                itm["ChildBasePrice"] = "0";
                            }
                            else
                            {
                                itm["ChildBasePrice"] = Convert.ToString(itm["ChildBasePrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["ChildTaxes"].ToString()) == true)
                            {
                                itm["ChildTaxes"] = "0";
                            }
                            else
                            {
                                itm["ChildTaxes"] = Convert.ToString(itm["ChildTaxes"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["InfantTotalPrice"].ToString()) == true)
                            {
                                itm["InfantTotalPrice"] = "0";
                            }
                            else
                            {
                                itm["InfantTotalPrice"] = Convert.ToString(itm["InfantTotalPrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["InfantBasePrice"].ToString()) == true)
                            {
                                itm["InfantBasePrice"] = "0";
                            }
                            else
                            {
                                itm["InfantBasePrice"] = Convert.ToString(itm["InfantBasePrice"]).Insert(3, " ");
                            }
                            if (string.IsNullOrEmpty(itm["InfantTaxes"].ToString()) == true)
                            {
                                itm["InfantTaxes"] = "0";
                            }
                            else
                            {
                                itm["InfantTaxes"] = Convert.ToString(itm["InfantTaxes"]).Insert(3, " ");
                            }
                        }

                        ds.EnforceConstraints = false;


                        FlightSegments.Columns.Add("Stops");
                        FlightSegments.Columns.Add("Direct");
                        FlightSegments.Columns.Add("Flight");
                        FlightSegments.Columns.Add("SegmentKey");
                        FlightSegments.Columns.Add("Group");
                        FlightSegments.Columns.Add("Carrier");
                        FlightSegments.Columns.Add("FlightNumber");
                        FlightSegments.Columns.Add("Origin");
                        FlightSegments.Columns.Add("Destination");
                        FlightSegments.Columns.Add("DepartureTime");
                        FlightSegments.Columns.Add("ArrivalTime");
                        FlightSegments.Columns.Add("FlightTime");
                        FlightSegments.Columns.Add("Distance");
                        FlightSegments.Columns.Add("ETicketability");
                        FlightSegments.Columns.Add("Equipment");
                        FlightSegments.Columns.Add("ChangeOfPlane");
                        FlightSegments.Columns.Add("ParticipantLevel");
                        FlightSegments.Columns.Add("LinkAvailability");
                        FlightSegments.Columns.Add("PolledAvailabilityOption");
                        FlightSegments.Columns.Add("OptionalServicesIndicator");
                        FlightSegments.Columns.Add("AvailabilitySource");
                        FlightSegments.Columns.Add("AvailabilityDisplayType");
                        FlightSegments.Columns.Add("ProviderCode");
                        FlightSegments.Columns.Add("ArrTime"); //Final Arrival Time
                        FlightSegments.Columns.Add("FinalDestination");
                        FlightSegments.Columns.Add("TransactionId");

                        Guid TransactionId = Guid.NewGuid();

                        foreach (DataRow dr in FlightSegments.Rows)
                        {
                            dr["TransactionId"] = TransactionId;
                        }

                        foreach (DataRow row in FlightSegments.Rows)
                        {
                            foreach (DataRow row1 in ds.Tables["AirSegment"].Rows)
                            {
                                if (row["SegmentRef"].ToString() == row1["Key"].ToString())
                                {
                                    row["ProviderCode"] = "1G";
                                    row["Direct"] = row1["Direct"];
                                    row["Flight"] = row1["Flight"];
                                    row["SegmentKey"] = row1["Key"];
                                    row["Group"] = row1["Group"];
                                    row["Carrier"] = row1["Carrier"];
                                    row["FlightNumber"] = row1["FlightNumber"];
                                    row["Origin"] = row1["Origin"];
                                    row["Destination"] = row1["Destination"];
                                    row["DepartureTime"] = row1["DepartureTime"];
                                    row["ArrivalTime"] = row1["ArrivalTime"];
                                    row["FlightTime"] = row1["FlightTime"];
                                    row["Distance"] = row1["Distance"];
                                    row["ETicketability"] = row1["ETicketability"];
                                    row["Equipment"] = row1["Equipment"];
                                    row["ChangeOfPlane"] = row1["ChangeOfPlane"];
                                    if (ds.Tables["AirSegment"].Columns.Contains("ParticipantLevel"))
                                    {
                                        row["ParticipantLevel"] = row1["ParticipantLevel"];
                                    }

                                    if (ds.Tables["AirSegment"].Columns.Contains("LinkAvailability"))
                                    {
                                        row["LinkAvailability"] = row1["LinkAvailability"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("PolledAvailabilityOption"))
                                    {
                                        row["PolledAvailabilityOption"] = row1["PolledAvailabilityOption"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("OptionalServicesIndicator"))
                                    {
                                        row["OptionalServicesIndicator"] = row1["OptionalServicesIndicator"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("AvailabilitySource"))
                                    {
                                        row["AvailabilitySource"] = row1["AvailabilitySource"];
                                    }
                                    if (ds.Tables["AirSegment"].Columns.Contains("AvailabilityDisplayType"))
                                    {
                                        row["AvailabilityDisplayType"] = row1["AvailabilityDisplayType"];
                                    }

                                    row["FinalDestination"] = to;
                                }
                            }
                        }

                        foreach (DataRow dr in FlightSegments.Rows)
                        {
                            string id = dr["SegmentKey"].ToString();
                            DataTable BookingInfo = ds.Tables["BookingInfo"];
                            DataTable FareInfoRef = ds.Tables["FareInfoRef"];
                            DataTable AirPricingInfo = ds.Tables["AirPricingInfo"];
                            DataTable PassengerType = ds.Tables["PassengerType"];

                            string ff = (BookingInfo.AsEnumerable().Where(p => p["SegmentRef"].ToString() == id && p["BookingCode"].ToString() == dr["BookingCode"].ToString()).Select(p => p["FareInfoRef"].ToString())).FirstOrDefault();

                            var AirPricingInfo_id = (FareInfoRef.AsEnumerable().Where(p => p["Key"].ToString() == ff).Select(p => p["AirPricingInfo_id"].ToString())).FirstOrDefault();

                            string PaxType = (PassengerType.AsEnumerable().Where(p => Convert.ToInt32(p["AirPricingInfo_id"].ToString()) == Convert.ToInt32(AirPricingInfo_id)).Select(p => p["Code"].ToString())).FirstOrDefault();

                            int count = FlightSegments.AsEnumerable().Count(row => row.Field<int>("Option_Id") == Convert.ToInt32(dr["Option_Id"].ToString()));
                            string arrtime = FlightSegments.AsEnumerable().Where(p => p.Field<int>("Option_Id") == Convert.ToInt32(dr["Option_Id"].ToString()) && p.Field<string>("Destination") == to).Select(p => p["ArrivalTime"].ToString()).FirstOrDefault();

                            if (Arr.IndexOf("T") > -1)
                            {
                                dr["arrtime"] = arrtime;
                            }
                            else
                            {
                                dr["arrtime"] = dr["ArrivalTime"];
                            }

                            dr["stops"] = (count - 1).ToString();

                        }
                        FlightSegments.AcceptChanges();

                        DataTable segments = new DataTable();
                        segments.Columns.Add("AirPricingInfo_id");
                        segments.Columns.Add("Option_id1");
                        segments.Columns.Add("SegmentRef1");
                        segments.Columns.Add("Origin1");
                        segments.Columns.Add("Destination1");
                        segments.Columns.Add("TotalPrice");
                        segments.Columns.Add("Option_id2");
                        segments.Columns.Add("SegmentRef2");
                        segments.Columns.Add("Origin2");
                        segments.Columns.Add("Destination2");
                        segments.Columns.Add("Option_id3");
                        segments.Columns.Add("SegmentRef3");
                        segments.Columns.Add("Origin3");
                        segments.Columns.Add("Destination3");
                        segments.Columns.Add("Option_id4");
                        segments.Columns.Add("SegmentRef4");
                        segments.Columns.Add("Origin4");
                        segments.Columns.Add("Destination4");
                        segments.Columns.Add("Option_id5");
                        segments.Columns.Add("SegmentRef5");
                        segments.Columns.Add("Origin5");
                        segments.Columns.Add("Destination5");
                        segments.Columns.Add("Option_id6");
                        segments.Columns.Add("SegmentRef6");
                        segments.Columns.Add("Origin6");
                        segments.Columns.Add("Destination6");

                        fromM_1 = ExtractCode(fromM_1);

                        //  fromM_1 = fromM_1.Substring(fromM_1.Length - 4, 3);
                        //1st segment
                        foreach (DataRow ro in BookingInfo1.Rows)
                        {
                            if (ro["Group"].ToString() == "0" && ro["Origin"].ToString() == fromM_1)
                            {
                                DataRow row = segments.NewRow();
                                row["AirPricingInfo_id"] = ro["AirPricingInfo_id"];
                                row["SegmentRef1"] = ro["SegmentRef"];
                                row["Origin1"] = ro["Origin"];
                                //  row["OriginalOrigin1"] = ro["OriginalOrigin"];
                                row["Destination1"] = ro["Destination"];
                                row["Option_id1"] = ro["Option_id"];
                                segments.Rows.Add(row);
                            }
                        }

                        //List<string> levels = BookingInfo1.AsEnumerable().Select(al => al.Field<string>("AirPricingInfo_id")).Distinct().ToList();
                        //int min = levels.Min();
                        //int max = levels.Max();

                        ////Total Price
                        //for (int i = min; i <= max; i++)
                        //{
                        //    foreach (DataRow ro in BookingInfo1.Rows)
                        //    {
                        //        foreach (DataRow ro1 in segments.Rows)
                        //        {
                        //            if (ro1["AirPricingInfo_id"].ToString() == i.ToString() && ro1["SegmentRef1"] == ro["SegmentRef"] && ro["PassengerType"].ToString() == "ADT")
                        //            {
                        //                ro1["TotalPrice"] = ro["TotalPrice"];
                        //            }
                        //        }
                        //    }
                        //}

                        ////2nd segment
                        //for (int i = min; i <= max; i++)
                        //{
                        foreach (DataRow ro in BookingInfo1.Rows)
                        {
                            foreach (DataRow ro1 in segments.Rows)
                            {
                                if (ro1["AirPricingInfo_id"].ToString() == ro["AirPricingInfo_id"].ToString() && ro["Group"].ToString() == "1")
                                {
                                    ro1["SegmentRef2"] = ro["SegmentRef"];
                                    ro1["Origin2"] = ro["Origin"];
                                    // ro1["OriginalOrigin2"] = ro["OriginalOrigin"];
                                    ro1["Destination2"] = ro["Destination"];
                                    ro1["Option_id2"] = ro["Option_id"];
                                }
                            }
                        }
                        //}

                        ////3rd segment
                        //for (int i = min; i <= max; i++)
                        //{
                        foreach (DataRow ro in BookingInfo1.Rows)
                        {
                            foreach (DataRow ro1 in segments.Rows)
                            {
                                if (ro1["AirPricingInfo_id"].ToString() == ro["AirPricingInfo_id"].ToString() && ro["Group"].ToString() == "2")
                                {
                                    ro1["SegmentRef3"] = ro["SegmentRef"];
                                    ro1["Origin3"] = ro["Origin"];
                                    //  ro1["OriginalOrigin3"] = ro["OriginalOrigin"];
                                    ro1["Destination3"] = ro["Destination"];
                                    ro1["Option_id3"] = ro["Option_id"];
                                }
                            }
                        }
                        //}

                        ////4th segment
                        //for (int i = min; i <= max; i++)
                        //{
                        foreach (DataRow ro in BookingInfo1.Rows)
                        {
                            foreach (DataRow ro1 in segments.Rows)
                            {
                                if (ro1["AirPricingInfo_id"].ToString() == ro["AirPricingInfo_id"].ToString() && ro["Group"].ToString() == "3")
                                {
                                    ro1["SegmentRef4"] = ro["SegmentRef"];
                                    ro1["Origin4"] = ro["Origin"];
                                    //  ro1["OriginalOrigin4"] = ro["OriginalOrigin"];
                                    ro1["Destination4"] = ro["Destination"];
                                    ro1["Option_id4"] = ro["Option_id"];
                                }
                            }
                        }
                        //}

                        //5th segment
                        //for (int i = min; i <= max; i++)
                        //{
                        foreach (DataRow ro in BookingInfo1.Rows)
                        {
                            foreach (DataRow ro1 in segments.Rows)
                            {
                                if (ro1["AirPricingInfo_id"].ToString() == ro["AirPricingInfo_id"].ToString() && ro["Group"].ToString() == "4")
                                {
                                    ro1["SegmentRef5"] = ro["SegmentRef"];
                                    ro1["Origin5"] = ro["Origin"];
                                    //  ro1["OriginalOrigin5"] = ro["OriginalOrigin"];
                                    ro1["Destination5"] = ro["Destination"];
                                    ro1["Option_id5"] = ro["Option_id"];
                                }
                            }
                        }
                        //}

                        ////6th segment
                        //for (int i = min; i <= max; i++)
                        //{
                        foreach (DataRow ro in BookingInfo1.Rows)
                        {
                            foreach (DataRow ro1 in segments.Rows)
                            {
                                if (ro1["AirPricingInfo_id"].ToString() == ro["AirPricingInfo_id"].ToString() && ro["Group"].ToString() == "5")
                                {
                                    ro1["SegmentRef6"] = ro["SegmentRef"];
                                    ro1["Origin6"] = ro["Origin"];
                                    //   ro1["OriginalOrigin6"] = ro["OriginalOrigin"];
                                    ro1["Destination6"] = ro["Destination"];
                                    ro1["Option_id6"] = ro["Option_id"];
                                }
                            }
                        }
                        //}
                        //TempData["FlightType"] = "other";
                        //Session["MultipleSegments"] = segments;

                        // TempData["AirSegments"] = FlightSegments;
                        //  var JSONString = DataTableToJSONWithStringBuilder(segments);

                        string JSONString = CombineAndSerializeDataTables(FlightSegments, segments);

                        return JSONString;

                        #endregion
                    }

                }
                return "";

            }
            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message;
                //return ex.Message.ToString();
                return "something went wrong";
            }
        }

        public static string CombineAndSerializeDataTables(DataTable flightSegments, DataTable segments)
        {
            // Create a combined structure
            var combined = new
            {
                FlightSegments = flightSegments,
                Segments = segments
            };

            // Serialize to JSON
            string jsonString = JsonConvert.SerializeObject(combined, Newtonsoft.Json.Formatting.Indented);
            return jsonString;
        }

        private void AddColumnsToBookingInfo(DataTable bookingInfo)
        {
            bookingInfo.Columns.Add("FlightOption_id", typeof(string));
            bookingInfo.Columns.Add("FlightOptionsList_id", typeof(string));
            bookingInfo.Columns.Add("AirPricingInfo_id", typeof(int));
            bookingInfo.Columns.Add("SegmentKey", typeof(string));
            bookingInfo.Columns.Add("Group", typeof(string));
            bookingInfo.Columns.Add("Carrier", typeof(string));
            bookingInfo.Columns.Add("FlightNumber", typeof(string));
            bookingInfo.Columns.Add("Origin", typeof(string));
            bookingInfo.Columns.Add("Destination", typeof(string));
            bookingInfo.Columns.Add("DepartureTime", typeof(string));
            bookingInfo.Columns.Add("ArrivalTime", typeof(string));
            bookingInfo.Columns.Add("FlightTime", typeof(string));
            bookingInfo.Columns.Add("Distance", typeof(string));
            bookingInfo.Columns.Add("ETicketability", typeof(string));
            bookingInfo.Columns.Add("Equipment", typeof(string));
            bookingInfo.Columns.Add("ChangeOfPlane", typeof(string));
            bookingInfo.Columns.Add("ParticipantLevel", typeof(string));
            bookingInfo.Columns.Add("LinkAvailability", typeof(string));
            bookingInfo.Columns.Add("PolledAvailabilityOption", typeof(string));
            bookingInfo.Columns.Add("OptionalServicesIndicator", typeof(string));
            bookingInfo.Columns.Add("AvailabilitySource", typeof(string));
            bookingInfo.Columns.Add("AvailabilityDisplayType", typeof(string));
        }

        private void AddColumnsToAirSegment(DataTable airSegment)
        {
            airSegment.Columns.Add("Direct", typeof(string));
            airSegment.Columns.Add("Flight", typeof(string));
        }

        private void ProcessAirSegments(DataTable airSegment, MultipleDistenations md)
        {
            foreach (DataRow dr in airSegment.Rows)
            {
                for (int i = 1; i <= 6; i++)
                {
                    if (dr["Origin"].ToString() == md.GetType().GetProperty($"From{i}").GetValue(md, null)?.ToString()
                        && dr["Destination"].ToString() == md.GetType().GetProperty($"To{i}").GetValue(md, null)?.ToString())
                    {
                        dr["Direct"] = "DIRECT";
                        dr["Flight"] = i.ToString();
                    }
                }
            }
        }

        private void AddFareColumns(DataTable dt)
        {
            dt.Columns.Add("AdultBasePrice", typeof(string));
            dt.Columns.Add("AdultTotalPrice", typeof(string));
            dt.Columns.Add("AdultTaxes", typeof(string));
            dt.Columns.Add("ChildBasePrice", typeof(string));
            dt.Columns.Add("ChildTotalPrice", typeof(string));
            dt.Columns.Add("ChildTaxes", typeof(string));
            dt.Columns.Add("InfantBasePrice", typeof(string));
            dt.Columns.Add("InfantTotalPrice", typeof(string));
            dt.Columns.Add("InfantTaxes", typeof(string));
            dt.Columns.Add("FareBasis", typeof(string));
            dt.Columns.Add("SegmentIndex", typeof(string));
        }

        private void FormatPriceColumns(DataTable flightSegments)
        {
            foreach (DataColumn column in flightSegments.Columns)
            {
                if (column.ColumnName.EndsWith("Price") || column.ColumnName.EndsWith("Taxes"))
                {
                    column.ReadOnly = false;
                }
            }
        }

        private void AddSegmentColumns(DataTable flightSegments)
        {
            flightSegments.Columns.Add("Group", typeof(string));
            flightSegments.Columns.Add("Carrier", typeof(string));
            flightSegments.Columns.Add("FlightNumber", typeof(string));
            flightSegments.Columns.Add("Origin", typeof(string));
            flightSegments.Columns.Add("Destination", typeof(string));
            flightSegments.Columns.Add("Direct", typeof(string));
            flightSegments.Columns.Add("Flight", typeof(string));
            flightSegments.Columns.Add("DepartureTime", typeof(string));
            flightSegments.Columns.Add("ArrivalTime", typeof(string));
            flightSegments.Columns.Add("FlightTime", typeof(string));
        }

        private void FormatFlightSegments(DataTable flightSegments)
        {
            foreach (DataRow row in flightSegments.Rows)
            {
                // Example: Format Departure and Arrival Times to a standard format (e.g., "yyyy-MM-dd HH:mm:ss")
                if (DateTime.TryParse(row["DepartureTime"].ToString(), out DateTime departureTime))
                {
                    row["DepartureTime"] = departureTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (DateTime.TryParse(row["ArrivalTime"].ToString(), out DateTime arrivalTime))
                {
                    row["ArrivalTime"] = arrivalTime.ToString("yyyy-MM-dd HH:mm:ss");
                }

                // Example: Ensure all prices are formatted with two decimal places
                FormatPriceColumn(row, "AdultBasePrice");
                FormatPriceColumn(row, "AdultTotalPrice");
                FormatPriceColumn(row, "AdultTaxes");
                FormatPriceColumn(row, "ChildBasePrice");
                FormatPriceColumn(row, "ChildTotalPrice");
                FormatPriceColumn(row, "ChildTaxes");
                FormatPriceColumn(row, "InfantBasePrice");
                FormatPriceColumn(row, "InfantTotalPrice");
                FormatPriceColumn(row, "InfantTaxes");

                // Example: Convert "Direct" column to upper case
                row["Direct"] = row["Direct"].ToString().ToUpper();

                // Example: Ensure "Flight" column is zero-padded to two digits (e.g., "01", "02", etc.)
                if (int.TryParse(row["Flight"].ToString(), out int flightNumber))
                {
                    row["Flight"] = flightNumber.ToString("D2");
                }
            }
        }

        private void FormatPriceColumn(DataRow row, string columnName)
        {
            if (decimal.TryParse(row[columnName].ToString(), out decimal price))
            {
                row[columnName] = price.ToString("F2");
            }
        }

        [NonAction]
        public static List<GalFlights> ConvertDataTableToList(DataTable dataTable)
        {
            var flights = new List<GalFlights>();

            foreach (DataRow row in dataTable.Rows)
            {
                var flight = new GalFlights
                {
                    ArrivalTime = row["ArrivalTime"].ToString(),
                    DepartureTime = row["DepartureTime"].ToString(),
                    Option_Id = Convert.ToInt32(row["Option_Id"].ToString())
                };
                flights.Add(flight);
            }

            return flights;
        }

        private static TimeSpan CalculateTotalDuration(IGrouping<int?, GalFlights> flightGroup)
        {
            var sortedFlights = flightGroup.OrderBy(f => DateTime.Parse(f.DepartureTime, CultureInfo.InvariantCulture)).ToList();
            var departureTime = DateTime.Parse(sortedFlights.First().DepartureTime, CultureInfo.InvariantCulture);
            var arrivalTime = DateTime.Parse(sortedFlights.Last().ArrivalTime, CultureInfo.InvariantCulture);
            return arrivalTime - departureTime;
        }

        private static string FormatDuration(TimeSpan duration)
        {
            int hours = (int)duration.TotalHours;
            int minutes = duration.Minutes;
            return $"{hours}H{minutes}M";
        }

        //Galilio XML Request
        private string XMLReq(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr, string fromM_1, string fromM_2,
            string fromM_3, string fromM_4, string fromM_5, string fromM_6,
            string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string airline = "")
        {
            StringBuilder strReq = new StringBuilder();
            Guid TraceId = Guid.NewGuid();

            strReq.Append(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>");
            strReq.Append("<soapenv:Header/>");
            strReq.Append("<soapenv:Body>");
            //strReq.Append("<LowFareSearchReq xmlns='http://www.travelport.com/schema/air_v42_0' TraceId='" + TraceId + "' TargetBranch='P7113657' ReturnUpsellFare='true'>");
            strReq.Append("<LowFareSearchReq xmlns='http://www.travelport.com/schema/air_v42_0' TraceId='" + TraceId + "' TargetBranch='P3393195' ReturnUpsellFare='true'>");
            strReq.Append("<BillingPointOfSaleInfo xmlns='http://www.travelport.com/schema/common_v42_0' OriginApplication='uAPI'/>");
            if (tripType == "OneWay")
            {
                strReq.Append("<SearchAirLeg>");
                strReq.Append("<SearchOrigin>");
                strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + from + "' PreferCity='true'/>");
                strReq.Append("</SearchOrigin>");
                strReq.Append("<SearchDestination>");
                strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + to + "' PreferCity='true'/>");
                strReq.Append("</SearchDestination>");
                strReq.Append("<SearchDepTime PreferredTime='" + Dept + "' />");
                strReq.Append("</SearchAirLeg>");
            }
            else if (tripType == "Return")
            {
                strReq.Append("<SearchAirLeg>");
                strReq.Append("<SearchOrigin>");
                strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + from + "' PreferCity='true'/>");
                strReq.Append("</SearchOrigin>");
                strReq.Append("<SearchDestination>");
                strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + to + "' PreferCity='true'/>");
                strReq.Append("</SearchDestination>");
                strReq.Append("<SearchDepTime PreferredTime='" + Dept + "' />");
                strReq.Append("</SearchAirLeg>");
                strReq.Append("<SearchAirLeg>");
                strReq.Append("<SearchOrigin>");
                strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + to + "' PreferCity='true'/>");
                strReq.Append("</SearchOrigin>");
                strReq.Append("<SearchDestination>");
                strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + from + "' PreferCity='true'/>");
                strReq.Append("</SearchDestination>");
                strReq.Append("<SearchDepTime PreferredTime='" + Arr + "' />");
                strReq.Append("</SearchAirLeg>");
            }
            else
            {
                string[] startDate;
                startDate = start.Split(',');
                string IatacodeDep = "";
                string IatacodeArr = "";

                if (fromM_1 != "")
                {
                    string[] fromIata = fromM_1.Split('(');

                    if (fromM_1.IndexOf('(') < 0)
                    {
                        var _IatacodeDep = sql.Cities.Where(x => x.cityName == fromM_1).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeDep != null)
                        {
                            IatacodeDep = _IatacodeDep;
                            IatacodeDep = IatacodeDep.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (fromIata.Length > 1)
                        {
                            IatacodeDep = fromIata[1].Split(')')[0].Trim();
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length == 3)
                        {
                            IatacodeDep = fromIata[0];
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length > 3)
                        {
                            IatacodeDep = sql.Cities.Where(x => x.cityName == fromIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeDep = IatacodeDep.ToUpper();
                    }

                    string[] toIata = toM_1.Split('(');

                    if (toM_1.IndexOf('(') < 0)
                    {
                        var _IatacodeArr = sql.Cities.Where(x => x.cityName == toM_1).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeArr != null)
                        {
                            IatacodeArr = _IatacodeArr;
                            IatacodeArr = IatacodeArr.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (toIata.Length > 1)
                        {
                            IatacodeArr = toIata[1].Split(')')[0].Trim();
                        }
                        else if (toIata.Length > 0 && toIata[0].Length == 3)
                        {
                            IatacodeArr = toIata[0];
                        }
                        else if (toIata.Length > 0 && toIata[0].Length > 3)
                        {
                            IatacodeArr = sql.Cities.Where(x => x.cityName == toIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeArr = IatacodeArr.ToUpper();
                    }


                    string[] dte = startDate[0].Split('-');
                    string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00"; //2019-12-29T00:00:00

                    strReq.Append("<SearchAirLeg>");
                    strReq.Append("<SearchOrigin>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeDep + "' PreferCity='true'/>");
                    strReq.Append("</SearchOrigin>");
                    strReq.Append("<SearchDestination>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeArr + "' PreferCity='true'/>");
                    strReq.Append("</SearchDestination>");
                    strReq.Append("<SearchDepTime PreferredTime='" + _date + "' />");
                    strReq.Append("</SearchAirLeg>");
                }

                if (fromM_2 != "" && toM_2 != "")
                {
                    string[] fromIata = fromM_2.Split('(');

                    if (fromM_2.IndexOf('(') < 0)
                    {
                        var _IatacodeDep = sql.Cities.Where(x => x.cityName == fromM_2).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeDep != null)
                        {
                            IatacodeDep = _IatacodeDep;
                            IatacodeDep = IatacodeDep.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (fromIata.Length > 1)
                        {
                            IatacodeDep = fromIata[1].Split(')')[0].Trim();
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length == 3)
                        {
                            IatacodeDep = fromIata[0];
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length > 3)
                        {
                            IatacodeDep = sql.Cities.Where(x => x.cityName == fromIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeDep = IatacodeDep.ToUpper();
                    }

                    string[] toIata = toM_2.Split('(');

                    if (toM_2.IndexOf('(') < 0)
                    {
                        var _IatacodeArr = sql.Cities.Where(x => x.cityName == toM_2).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeArr != null)
                        {
                            IatacodeArr = _IatacodeArr;
                            IatacodeArr = IatacodeArr.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (toIata.Length > 1)
                        {
                            IatacodeArr = toIata[1].Split(')')[0].Trim();
                        }
                        else if (toIata.Length > 0 && toIata[0].Length == 3)
                        {
                            IatacodeArr = toIata[0];
                        }
                        else if (toIata.Length > 0 && toIata[0].Length > 3)
                        {
                            IatacodeArr = sql.Cities.Where(x => x.cityName == toIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeArr = IatacodeArr.ToUpper();
                    }

                    string[] dte = startDate[1].Split('-');
                    string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00";

                    strReq.Append("<SearchAirLeg>");
                    strReq.Append("<SearchOrigin>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeDep + "' PreferCity='true'/>");
                    strReq.Append("</SearchOrigin>");
                    strReq.Append("<SearchDestination>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeArr + "' PreferCity='true'/>");
                    strReq.Append("</SearchDestination>");
                    strReq.Append("<SearchDepTime PreferredTime='" + _date + "' />");
                    strReq.Append("</SearchAirLeg>");
                }

                if ((fromM_3 != "" && fromM_3 != "0") && (toM_3 != "" && toM_3 != "0"))
                {
                    string[] fromIata = fromM_3.Split('(');

                    if (fromM_3.IndexOf('(') < 0)
                    {
                        var _IatacodeDep = sql.Cities.Where(x => x.cityName == fromM_3).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeDep != null)
                        {
                            IatacodeDep = _IatacodeDep;
                            IatacodeDep = IatacodeDep.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (fromIata.Length > 1)
                        {
                            IatacodeDep = fromIata[1].Split(')')[0].Trim();
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length == 3)
                        {
                            IatacodeDep = fromIata[0];
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length > 3)
                        {
                            IatacodeDep = sql.Cities.Where(x => x.cityName == fromIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeDep = IatacodeDep.ToUpper();
                    }

                    string[] toIata = toM_3.Split('(');

                    if (toM_3.IndexOf('(') < 0)
                    {
                        var _IatacodeArr = sql.Cities.Where(x => x.cityName == toM_3).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeArr != null)
                        {
                            IatacodeArr = _IatacodeArr;
                            IatacodeArr = IatacodeArr.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (toIata.Length > 1)
                        {
                            IatacodeArr = toIata[1].Split(')')[0].Trim();
                        }
                        else if (toIata.Length > 0 && toIata[0].Length == 3)
                        {
                            IatacodeArr = toIata[0];
                        }
                        else if (toIata.Length > 0 && toIata[0].Length > 3)
                        {
                            IatacodeArr = sql.Cities.Where(x => x.cityName == toIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeArr = IatacodeArr.ToUpper();
                    }

                    string[] dte = startDate[2].Split('-');
                    string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00";

                    strReq.Append("<SearchAirLeg>");
                    strReq.Append("<SearchOrigin>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeDep + "' PreferCity='true'/>");
                    strReq.Append("</SearchOrigin>");
                    strReq.Append("<SearchDestination>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeArr + "' PreferCity='true'/>");
                    strReq.Append("</SearchDestination>");
                    strReq.Append("<SearchDepTime PreferredTime='" + _date + "' />");
                    strReq.Append("</SearchAirLeg>");
                }

                if ((fromM_4 != "" && fromM_4 != "0") && (toM_4 != "" && toM_4 != "0"))
                {
                    string[] fromIata = fromM_4.Split('(');

                    if (fromM_4.IndexOf('(') < 0)
                    {
                        var _IatacodeDep = sql.Cities.Where(x => x.cityName == fromM_4).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeDep != null)
                        {
                            IatacodeDep = _IatacodeDep;
                            IatacodeDep = IatacodeDep.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (fromIata.Length > 1)
                        {
                            IatacodeDep = fromIata[1].Split(')')[0].Trim();
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length == 3)
                        {
                            IatacodeDep = fromIata[0];
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length > 3)
                        {
                            IatacodeDep = sql.Cities.Where(x => x.cityName == fromIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeDep = IatacodeDep.ToUpper();
                    }

                    string[] toIata = toM_4.Split('(');

                    if (toM_4.IndexOf('(') < 0)
                    {
                        var _IatacodeArr = sql.Cities.Where(x => x.cityName == toM_4).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeArr != null)
                        {
                            IatacodeArr = _IatacodeArr;
                            IatacodeArr = IatacodeArr.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (toIata.Length > 1)
                        {
                            IatacodeArr = toIata[1].Split(')')[0].Trim();
                        }
                        else if (toIata.Length > 0 && toIata[0].Length == 3)
                        {
                            IatacodeArr = toIata[0];
                        }
                        else if (toIata.Length > 0 && toIata[0].Length > 3)
                        {
                            IatacodeArr = sql.Cities.Where(x => x.cityName == toIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeArr = IatacodeArr.ToUpper();
                    }

                    string[] dte = startDate[3].Split('-');
                    string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00";

                    strReq.Append("<SearchAirLeg>");
                    strReq.Append("<SearchOrigin>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeDep + "' PreferCity='true'/>");
                    strReq.Append("</SearchOrigin>");
                    strReq.Append("<SearchDestination>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeArr + "' PreferCity='true'/>");
                    strReq.Append("</SearchDestination>");
                    strReq.Append("<SearchDepTime PreferredTime='" + _date + "' />");
                    strReq.Append("</SearchAirLeg>");

                }

                if ((fromM_5 != "" && fromM_5 != "0") && (toM_5 != "" && toM_5 != "0"))
                {
                    string[] fromIata = fromM_5.Split('(');

                    if (fromM_5.IndexOf('(') < 0)
                    {
                        var _IatacodeDep = sql.Cities.Where(x => x.cityName == fromM_5).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeDep != null)
                        {
                            IatacodeDep = _IatacodeDep;
                            IatacodeDep = IatacodeDep.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (fromIata.Length > 1)
                        {
                            IatacodeDep = fromIata[1].Split(')')[0].Trim();
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length == 3)
                        {
                            IatacodeDep = fromIata[0];
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length > 3)
                        {
                            IatacodeDep = sql.Cities.Where(x => x.cityName == fromIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeDep = IatacodeDep.ToUpper();
                    }

                    string[] toIata = toM_5.Split('(');

                    if (toM_5.IndexOf('(') < 0)
                    {
                        var _IatacodeArr = sql.Cities.Where(x => x.cityName == toM_5).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeArr != null)
                        {
                            IatacodeArr = _IatacodeArr;
                            IatacodeArr = IatacodeArr.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (toIata.Length > 1)
                        {
                            IatacodeArr = toIata[1].Split(')')[0].Trim();
                        }
                        else if (toIata.Length > 0 && toIata[0].Length == 3)
                        {
                            IatacodeArr = toIata[0];
                        }
                        else if (toIata.Length > 0 && toIata[0].Length > 3)
                        {
                            IatacodeArr = sql.Cities.Where(x => x.cityName == toIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeArr = IatacodeArr.ToUpper();
                    }

                    string[] dte = startDate[4].Split('-');
                    string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00";

                    strReq.Append("<SearchAirLeg>");
                    strReq.Append("<SearchOrigin>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeDep + "' PreferCity='true'/>");
                    strReq.Append("</SearchOrigin>");
                    strReq.Append("<SearchDestination>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeArr + "' PreferCity='true'/>");
                    strReq.Append("</SearchDestination>");
                    strReq.Append("<SearchDepTime PreferredTime='" + _date + "' />");
                    strReq.Append("</SearchAirLeg>");
                }

                if ((fromM_6 != "" && fromM_6 != "0") && (toM_6 != "" && toM_6 != "0"))
                {
                    string[] fromIata = fromM_6.Split('(');

                    if (fromM_6.IndexOf('(') < 0)
                    {
                        var _IatacodeDep = sql.Cities.Where(x => x.cityName == fromM_6).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeDep != null)
                        {
                            IatacodeDep = _IatacodeDep;
                            IatacodeDep = IatacodeDep.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (fromIata.Length > 1)
                        {
                            IatacodeDep = fromIata[1].Split(')')[0].Trim();
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length == 3)
                        {
                            IatacodeDep = fromIata[0];
                        }
                        else if (fromIata.Length > 0 && fromIata[0].Length > 3)
                        {
                            IatacodeDep = sql.Cities.Where(x => x.cityName == fromIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeDep = IatacodeDep.ToUpper();
                    }

                    string[] toIata = toM_6.Split('(');

                    if (toM_6.IndexOf('(') < 0)
                    {
                        var _IatacodeArr = sql.Cities.Where(x => x.cityName == toM_6).Select(x => x.IATA).FirstOrDefault();
                        if (_IatacodeArr != null)
                        {
                            IatacodeArr = _IatacodeArr;
                            IatacodeArr = IatacodeArr.ToUpper();
                        }
                        else
                        {
                            return "invalid";
                        }
                    }
                    else
                    {
                        if (toIata.Length > 1)
                        {
                            IatacodeArr = toIata[1].Split(')')[0].Trim();
                        }
                        else if (toIata.Length > 0 && toIata[0].Length == 3)
                        {
                            IatacodeArr = toIata[0];
                        }
                        else if (toIata.Length > 0 && toIata[0].Length > 3)
                        {
                            IatacodeArr = sql.Cities.Where(x => x.cityName == toIata[0]).Select(x => x.cityCode).FirstOrDefault();
                        }
                        IatacodeArr = IatacodeArr.ToUpper();
                    }

                    string[] dte = startDate[5].Split('-');
                    string _date = "2022-08-09T00:00:00"; //dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00";

                    strReq.Append("<SearchAirLeg>");
                    strReq.Append("<SearchOrigin>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeDep + "' PreferCity='true'/>");
                    strReq.Append("</SearchOrigin>");
                    strReq.Append("<SearchDestination>");
                    strReq.Append("<CityOrAirport xmlns='http://www.travelport.com/schema/common_v42_0' Code='" + IatacodeArr + "' PreferCity='true'/>");
                    strReq.Append("</SearchDestination>");
                    strReq.Append("<SearchDepTime PreferredTime='" + _date + "' />");
                    strReq.Append("</SearchAirLeg>");
                }
            }


            strReq.Append("<AirSearchModifiers>");
            strReq.Append("<PreferredProviders>");
            strReq.Append("<Provider xmlns='http://www.travelport.com/schema/common_v42_0' Code='1G'/>");
            strReq.Append("</PreferredProviders>");
            airline = airline.Trim();
            if (airline != null && airline != "" && airline != " " && airline != ",," && airline != "null" && airline != "0")
            {
                airline = airline.Substring(airline.IndexOf("(") + 1, 2);
                strReq.Append("<PreferredCarriers>");
                strReq.Append("<Carrier Code='" + airline + "' xmlns='http://www.travelport.com/schema/common_v42_0'/>");
                strReq.Append("</PreferredCarriers>");
            }

            strReq.Append("</AirSearchModifiers>");

            if (adults > 0)
            {
                for (int c = 0; c < adults; c++)
                {
                    strReq.Append("<SearchPassenger xmlns='http://www.travelport.com/schema/common_v42_0' Code='ADT'/>");
                }
            }


            if (children > 0)
            {
                for (int c = 0; c < children; c++)
                {
                    strReq.Append("<SearchPassenger xmlns='http://www.travelport.com/schema/common_v42_0' Code='CNN' Age='10' DOB='2009-01-24'/>");
                }
            }

            if (infants > 0)
            {
                for (int c = 0; c < infants; c++)
                {
                    strReq.Append("<SearchPassenger xmlns='http://www.travelport.com/schema/common_v42_0' Code='INF' Age='1' DOB='2009-01-24'/>");
                }
            }


            strReq.Append("<AirPricingModifiers>");
            //strReq.Append("<AccountCodes>");
            //strReq.Append("<AccountCode xmlns='http://www.travelport.com/schema/common_v42_0' Code='-'/>");
            //strReq.Append("</AccountCodes>");           
            strReq.Append("</AirPricingModifiers>");
            strReq.Append("</LowFareSearchReq>");
            strReq.Append("</soapenv:Body>");
            strReq.Append("</soapenv:Envelope>");

            return strReq.ToString();
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
        [HttpGet("CountrySearch")]
        public string CountryName(string term)
        {
            var val = db.VisaCountries.Where(x => x.Name.StartsWith(term)).Select(x => x.Name).ToList();

            var json = System.Text.Json.JsonSerializer.Serialize(val);
            return json.ToString();


        }

    }

    public class GalFlights
    {
            public string? BookingCode { get; set; }
            public int? BookingCount { get; set; }
            public string? CabinClass { get; set; }
            public string? FareInfoRef { get; set; }
            public string? SegmentRef { get; set; }
            public int? Option_Id { get; set; }
            public string? PaxType { get; set; }
            public string? AdultTotalPrice { get; set; }
            public string? AdultBasePrice { get; set; }
            public string? AdultTaxes { get; set; }
            public int? ChildTotalPrice { get; set; }
            public int? ChildBasePrice { get; set; }
            public int? ChildTaxes { get; set; }
            public int? InfantTotalPrice { get; set; }
            public int? InfantBasePrice { get; set; }
            public int? InfantTaxes { get; set; }
            public int? MaxWeightValue { get; set; }
            public string? MaxWeightUnit { get; set; }
            public int? GrandTotal { get; set; }
            public string? TransactionId { get; set; }
            public int? SortBy { get; set; }
            public int? Stops { get; set; }
            public int? Direct { get; set; }
            public string? Flight { get; set; }
            public string? SegmentKey { get; set; }
            public int? Group { get; set; }
            public string? Carrier { get; set; }
            public int? FlightNumber { get; set; }
            public string? Origin { get; set; }
            public string? Destination { get; set; }
            public string? DepartureTime { get; set; }
            public string? ArrivalTime { get; set; }
            public int? FlightTime { get; set; }
            public int? Distance { get; set; }
            public string? ETicketability { get; set; }
            public object? Equipment { get; set; }
            public bool? ChangeOfPlane { get; set; }
            public string? ParticipantLevel { get; set; }
            public bool? LinkAvailability { get; set; }
            public string? PolledAvailabilityOption { get; set; }
            public bool? OptionalServicesIndicator { get; set; }
            public string? AvailabilitySource { get; set; }
            public string? AvailabilityDisplayType { get; set; }
            public string? ProviderCode { get; set; }
            public string? ArrTime { get; set; }
            public string? FinalDestination { get; set; }
            public string? CancelPenalty { get; set; }
            public string? ChangePenalty { get; set; }
            public string? CarrierName { get; set; }
            public int? OriginalCost { get; set; }
            public int? SegmentIndex { get; set; }
       

    }
}


//// Save data into session
////HttpContext.Session.Set("Connection", ds.Tables["Connection"]);
////HttpContext.Session.SetString("securitytoken", ds.Tables["LowFareSearchRsp"].Rows[0]["TransactionId"].ToString());
////HttpContext.Session.SetString("convid", ds.Tables["LowFareSearchRsp"].Rows[0]["TraceId"].ToString());

//var md = new MultipleDistenations
//{
//    From1 = fromM_1,
//    From2 = fromM_2,
//    From3 = fromM_3,
//    From4 = fromM_4,
//    From5 = fromM_5,
//    From6 = fromM_6,
//    To1 = toM_1,
//    To2 = toM_2,
//    To3 = toM_3,
//    To4 = toM_4,
//    To5 = toM_5,
//    To6 = toM_6
//};

//var AirPricingInfo1 = ds.Tables["AirPricingInfo"].Copy();
//var FlightOptionsList = ds.Tables["FlightOptionsList"].Copy();
//var FlightOptions = ds.Tables["FlightOption"].Copy();
//var Option = ds.Tables["Option"].Copy();
//var BookingInfo1 = ds.Tables["BookingInfo"].Copy();


//ds.Tables["AirSegment"].Columns.Add("Direct");

//foreach (DataRow dr in ds.Tables["AirSegment"].Rows)
//{
//    if (dr["Origin"].ToString() == from && dr["Destination"].ToString() == to)
//    {
//        dr["Direct"] = 1;
//    }
//    else
//    {
//        dr["Direct"] = 0;
//    }
//}
//// ds.Tables["AirSegment"].Columns.Add("Flight");
//DataColumn Col = ds.Tables["AirSegment"].Columns.Add("Flight", System.Type.GetType("System.String"));
//Col.SetOrdinal(7);
//foreach (DataRow dr in ds.Tables["AirSegment"].Rows)
//{
//    if (dr["Origin"].ToString() == from)
//    {

//        dr["Flight"] = "D"; //Departture Flight
//    }
//    else if (dr["Origin"].ToString() == to)
//    {
//        dr["Flight"] = "R"; //Return Flight
//    }
//    else if ((dr["Origin"].ToString() != from || dr["Origin"].ToString() == to) && dr["Destination"].ToString() == to)
//    {
//        dr["Flight"] = "CD"; //Connecting Flight
//    }
//    else if ((dr["Origin"].ToString() != from || dr["Origin"].ToString() == to) && dr["Destination"].ToString() == from)
//    {
//        dr["Flight"] = "CR"; //Connecting Flight
//    }
//    else
//    {
//        dr["Flight"] = "CD"; //Connecting Flight

//    }
//}

//// Add necessary columns to BookingInfo1
//AddColumnsToBookingInfo(BookingInfo1);

//var AirSegment = ds.Tables["AirSegment"].Copy();

//// Create dictionaries for lookups
//var optionLookup = Option.AsEnumerable().ToDictionary(row => row["Option_id"].ToString());
//var flightOptionsLookup = FlightOptions.AsEnumerable().ToDictionary(row => row["FlightOption_id"].ToString());
//var flightOptionsListLookup = FlightOptionsList.AsEnumerable().ToDictionary(row => row["FlightOptionsList_id"].ToString());
//var airSegmentLookup = AirSegment.AsEnumerable().ToDictionary(row => row["Key"].ToString());

//// Process BookingInfo1 with lookups
//foreach (DataRow dr in BookingInfo1.Rows)
//{
//    dr["FlightOption_id"] = optionLookup[dr["Option_id"].ToString()]["FlightOption_id"];
//    dr["FlightOptionsList_id"] = flightOptionsLookup[dr["FlightOption_id"].ToString()]["FlightOptionsList_id"];
//    dr["AirPricingInfo_id"] = Convert.ToInt32(flightOptionsListLookup[dr["FlightOptionsList_id"].ToString()]["AirPricingInfo_id"]);
//    var segment = airSegmentLookup[dr["SegmentRef"].ToString()];
//    dr["SegmentKey"] = segment["Key"];
//    dr["Group"] = segment["Group"];
//    dr["Carrier"] = segment["Carrier"];
//    dr["FlightNumber"] = segment["FlightNumber"];
//    dr["Origin"] = segment["Origin"];
//    dr["Destination"] = segment["Destination"];
//    dr["DepartureTime"] = segment["DepartureTime"];
//    dr["ArrivalTime"] = segment["ArrivalTime"];
//    dr["FlightTime"] = segment["FlightTime"];
//    dr["Distance"] = segment["Distance"];
//    dr["ETicketability"] = segment["ETicketability"];
//    dr["Equipment"] = segment["Equipment"];
//    dr["ChangeOfPlane"] = segment["ChangeOfPlane"];
//    dr["ParticipantLevel"] = segment["ParticipantLevel"];
//    dr["LinkAvailability"] = segment["LinkAvailability"];
//    dr["PolledAvailabilityOption"] = segment["PolledAvailabilityOption"];
//    dr["OptionalServicesIndicator"] = segment["OptionalServicesIndicator"];
//    dr["AvailabilitySource"] = segment["AvailabilitySource"];
//    dr["AvailabilityDisplayType"] = segment["AvailabilityDisplayType"];
//}

////   AddColumnsToAirSegment(ds.Tables["AirSegment"]);

//// Add "Direct" and "Flight" columns to AirSegment
//ProcessAirSegments(ds.Tables["AirSegment"], md);

//ds.Tables["BookingInfo"].Columns.Add("PaxType");

//var fareInfoRefLookup = ds.Tables["FareInfoRef"]
//    .AsEnumerable()
//    .GroupBy(row => row["Key"].ToString())
//    .ToDictionary(group => group.Key, group => group.First()); var passengerTypeLookup = ds.Tables["PassengerType"].AsEnumerable().ToDictionary(row => Convert.ToInt32(row["AirPricingInfo_id"].ToString()));
//var airPricingInfoLookup = ds.Tables["AirPricingInfo"].AsEnumerable().ToDictionary(row => row["AirPricingInfo_id"].ToString());

//foreach (DataRow dr in ds.Tables["BookingInfo"].Rows)
//{
//    var id = dr["option_id"].ToString();
//    var fareInfoRef = fareInfoRefLookup[(ds.Tables["BookingInfo"].AsEnumerable().First(p => p["option_id"].ToString() == id)["FareInfoRef"].ToString())];
//    var airPricingInfoId = fareInfoRef["AirPricingInfo_id"].ToString();
//    var paxType = passengerTypeLookup[Convert.ToInt32(airPricingInfoId)]["Code"].ToString();

//    dr["PaxType"] = paxType switch
//    {
//        "ADT" => "P",
//        "CNN" => "C",
//        "INF" => "I",
//        _ => dr["PaxType"]
//    };
//}

//var dt5 = ds.Tables["BookingInfo"].Copy();
//AddFareColumns(dt5);

//var airPricePointLookup = ds.Tables["AirPricePoint"].AsEnumerable().ToDictionary(row => Convert.ToInt32(row["AirPricePoint_Id"].ToString()));

//foreach (DataRow dr in dt5.Rows)
//{
//    var id = dr["option_id"].ToString();
//    var fareInfoRef = fareInfoRefLookup[(ds.Tables["BookingInfo"].AsEnumerable().First(p => p["option_id"].ToString() == id)["FareInfoRef"].ToString())];
//    var airPricingInfoId = fareInfoRef["AirPricingInfo_id"].ToString();
//    var paxType = passengerTypeLookup[Convert.ToInt32(airPricingInfoId)]["Code"].ToString();
//    var fareBasisCode = ds.Tables["FareInfo"].AsEnumerable().First(p => p["Key"].ToString() == fareInfoRef["Key"].ToString())["FareBasis"].ToString();
//    var airPricePoint = airPricePointLookup[Convert.ToInt32(airPricingInfoId)];

//    dr["FareBasis"] = fareBasisCode;
//    dr["SegmentIndex"] = ds.Tables["Connection"].AsEnumerable().FirstOrDefault(p => p["option_id"].ToString() == id)?["SegmentIndex"].ToString() ?? "0";

//    dr["AdultBasePrice"] = paxType == "ADT" ? airPricePoint["ApproximateBasePrice"].ToString() : dr["AdultBasePrice"];
//    dr["AdultTotalPrice"] = paxType == "ADT" ? airPricePoint["ApproximateTotalPrice"].ToString() : dr["AdultTotalPrice"];
//    dr["AdultTaxes"] = paxType == "ADT" ? airPricePoint["Taxes"].ToString() : dr["AdultTaxes"];

//    dr["ChildBasePrice"] = paxType == "CNN" ? airPricePoint["ApproximateBasePrice"].ToString() : dr["ChildBasePrice"];
//    dr["ChildTotalPrice"] = paxType == "CNN" ? airPricePoint["ApproximateTotalPrice"].ToString() : dr["ChildTotalPrice"];
//    dr["ChildTaxes"] = paxType == "CNN" ? airPricePoint["Taxes"].ToString() : dr["ChildTaxes"];

//    dr["InfantBasePrice"] = paxType == "INF" ? airPricePoint["ApproximateBasePrice"].ToString() : dr["InfantBasePrice"];
//    dr["InfantTotalPrice"] = paxType == "INF" ? airPricePoint["ApproximateTotalPrice"].ToString() : dr["InfantTotalPrice"];
//    dr["InfantTaxes"] = paxType == "INF" ? airPricePoint["Taxes"].ToString() : dr["InfantTaxes"];
//}

//FlightSegments = ds.Tables["BookingInfo"].Copy();
//AddFareColumns(FlightSegments);

//foreach (DataRow dr in FlightSegments.Rows)
//{
//    foreach (DataRow dr1 in dt5.Rows)
//    {
//        if (dr["SegmentRef"].ToString() == dr1["SegmentRef"].ToString() && dr["BookingCode"].ToString() == dr1["BookingCode"].ToString())
//        {
//            dr["AdultBasePrice"] = dr1["AdultBasePrice"];
//            dr["AdultTotalPrice"] = dr1["AdultTotalPrice"];
//            dr["AdultTaxes"] = dr1["AdultTaxes"];
//            dr["FareBasis"] = dr1["FareBasis"];
//            dr["SegmentIndex"] = dr1["SegmentIndex"];
//            break;
//        }
//    }
//}

//// Remove non-Adult passengers
//for (int i = FlightSegments.Rows.Count - 1; i >= 0; i--)
//{
//    if (FlightSegments.Rows[i]["PaxType"].ToString() is "C" or "I")
//    {
//        FlightSegments.Rows.RemoveAt(i);
//    }
//}

//FormatPriceColumns(FlightSegments);

//ds.EnforceConstraints = false;

//AddSegmentColumns(FlightSegments);

//foreach (DataRow row in FlightSegments.Rows)
//{
//    var segment = airSegmentLookup[row["SegmentRef"].ToString()];
//    row["Direct"] = segment["Direct"];
//    row["Flight"] = segment["Flight"];
//    row["DepartureTime"] = segment["DepartureTime"];
//    row["ArrivalTime"] = segment["ArrivalTime"];
//    row["FlightTime"] = segment["FlightTime"];
//    row["Group"] = segment["Group"];
//    row["Carrier"] = segment["Carrier"];
//    row["FlightNumber"] = segment["FlightNumber"];
//    row["Origin"] = segment["Origin"];
//    row["Destination"] = segment["Destination"];
//}

//FormatFlightSegments(FlightSegments);

//DataView dv = FlightSegments.DefaultView;
//dv.Sort = "option_id ASC, SegmentIndex ASC";
//FlightSegments = dv.ToTable();


//var JSONString = DataTableToJSONWithStringBuilder(FlightSegments);
//return JSONString;