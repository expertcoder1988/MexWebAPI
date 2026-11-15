using Azure;
using Azure.Core;
using iTextSharp.text.pdf.qrcode;

//using MalikExpressMVC.Models.XML;
using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using System.Xml;
using System.Xml.Linq;

namespace MexCoreWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]
    public class HitItFlightSearch : ControllerBase
    {
        private readonly MexDbContext sql;
        private readonly VisaDbContext db;

        public HitItFlightSearch(MexDbContext context, VisaDbContext visadb)
        {
            sql = context;
            db = visadb;
        }

        [HttpPost("lowpricealert")]
        public string LowPriceAlert([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                var frm = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                if (frm != null)
                {

                    string FullName = !string.IsNullOrEmpty(frm.fullName.Value) ? frm.fullName.Value.ToString().ToUpper() : "";
                    string Email = !string.IsNullOrEmpty(frm.email.Value) ? frm.email.Value.ToString() : "";
                    string MobileNum = !string.IsNullOrEmpty(frm.mobileNum.Value) ? frm.mobileNum.Value.ToString() : "";
                    string URL = !string.IsNullOrEmpty(frm.URL.Value) ? frm.URL.Value.ToString() : "";
                    string LowFare = !string.IsNullOrEmpty(frm.LowFare.Value) ? frm.LowFare.Value.ToString() : "0";
                    string MobileRequest = !string.IsNullOrEmpty(frm.MobileRequest.Value) ? frm.MobileRequest.Value.ToString() : "false";
                    string TokenId = !string.IsNullOrEmpty(frm.TokenId.Value) ? frm.TokenId.Value.ToString() : "";


                    bool flag = false;

                    if (MobileRequest == "true")
                    {
                        flag = true;
                    }

                    if (FullName != "" && Email != "")
                    {
                        LowFareRequest lfr = new LowFareRequest();
                        lfr.FullName = FullName;
                        lfr.Email = Email;
                        lfr.MobileNum = MobileNum;
                        lfr.URL = URL;
                        lfr.LowFare = Convert.ToDecimal(LowFare);
                        lfr.Status = true;
                        lfr.EntryTime = DateTime.Now;
                        lfr.MobileRequest = flag;
                        lfr.TokenId = TokenId;

                        sql.LowFareRequests.Add(lfr);
                        sql.SaveChanges();
                    }
                }

                return "alert saved";
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

        [NonAction]
        private string HitItXMLReq(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr, string fromM_1, string fromM_2,
           string fromM_3, string fromM_4, string fromM_5, string fromM_6,
           string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string airline = "")
        {
            try
            {
                MexCoreWebAPI.Model.EncryptionManager enc = new MexCoreWebAPI.Model.EncryptionManager();
                string de = enc.Decrypt("tm/Qojgg8CqtjTJfUGJpLfvuFPW87JNQqw7gkDHqHmk=");
                string TA = enc.Encrypt("sw86n9&U");

                StringBuilder strReq = new StringBuilder();
                Guid TraceId = Guid.NewGuid();

                strReq.Append(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'  xmlns:impl='http://impl.soap.ws.crane.hititcs.com/'>");
                strReq.Append("<soapenv:Header/>");
                strReq.Append("<soapenv:Body>");
                strReq.Append("<impl:GetAvailability>");
                strReq.Append("<AirAvailabilityRequest>");

                strReq.Append("<clientInformation>");
                strReq.Append("<clientIP>127.0.0.1</clientIP>");
                strReq.Append("<member>false</member>");
                //strReq.Append("<password>MALIK2050</password>");
                // strReq.Append("<userName>BSP2730099</userName>");
                strReq.Append("<password>Ahmad2090@</password>");
                strReq.Append("<userName>PSA2746366</userName>");
                //strReq.Append("<password>JUL2023</password>");
                //strReq.Append("<userName>A775SO88</userName>");
                strReq.Append("<preferredCurrency>PKR</preferredCurrency>");
                strReq.Append("</clientInformation>");


                if (tripType == "OneWay")
                {
                    strReq.Append("<originDestinationInformationList>");
                    strReq.Append("<dateOffset>0</dateOffset>");
                    strReq.Append("<departureDateTime>" + Dept + "</departureDateTime>");
                    strReq.Append("<destinationLocation>");
                    strReq.Append("<locationCode>" + to + "</locationCode>");
                    strReq.Append("</destinationLocation>");
                    strReq.Append("<flexibleFaresOnly>false</flexibleFaresOnly>");
                    strReq.Append("<includeInterlineFlights>true</includeInterlineFlights>");
                    strReq.Append("<originLocation>");
                    strReq.Append("<locationCode>" + from + "</locationCode>");
                    strReq.Append("</originLocation>");
                    strReq.Append("</originDestinationInformationList>");

                }
                else if (tripType == "Return")
                {
                    strReq.Append("<originDestinationInformationList>");
                    strReq.Append("<dateOffset>0</dateOffset>");
                    strReq.Append("<departureDateTime>" + Dept + "</departureDateTime>");
                    strReq.Append("<destinationLocation>");
                    strReq.Append("<locationCode>" + to + "</locationCode>");
                    strReq.Append("</destinationLocation>");
                    strReq.Append("<flexibleFaresOnly>false</flexibleFaresOnly>");
                    strReq.Append("<includeInterlineFlights>true</includeInterlineFlights>");
                    strReq.Append("<openFlight>false</openFlight>");
                    strReq.Append("<originLocation>");
                    strReq.Append("<locationCode>" + from + "</locationCode>");
                    strReq.Append("</originLocation>");
                    strReq.Append("</originDestinationInformationList>");
                    strReq.Append("<originDestinationInformationList>");
                    strReq.Append("<dateOffset>0</dateOffset>");
                    strReq.Append("<departureDateTime>" + Arr + "</departureDateTime>");
                    strReq.Append("<destinationLocation>");
                    strReq.Append("<locationCode>" + from + "</locationCode>");
                    strReq.Append("</destinationLocation>");
                    strReq.Append("<flexibleFaresOnly>false</flexibleFaresOnly>");
                    strReq.Append("<includeInterlineFlights>true</includeInterlineFlights>");
                    strReq.Append("<openFlight>false</openFlight>");
                    strReq.Append("<originLocation>");
                    strReq.Append("<locationCode>" + to + "</locationCode>");
                    strReq.Append("</originLocation>");
                    strReq.Append("</originDestinationInformationList>");
                }
                else
                {
                    string[] startDate;
                    startDate = start.Split(',');
                    string IatacodeDep = "KHI";
                    string IatacodeArr = "ISB";



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


                        string[] dte = startDate[1].Split('-');
                        string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00"; //2019-12-29T00:00:00

                        strReq.Append("<originDestinationInformationList>");
                        strReq.Append("<dateOffset>0</dateOffset>");
                        strReq.Append("<departureDateTime>" + _date + "</departureDateTime>");
                        strReq.Append("<destinationLocation>");
                        strReq.Append("<locationCode>" + IatacodeArr + "</locationCode>");
                        strReq.Append("</destinationLocation>");
                        strReq.Append("<flexibleFaresOnly>false</flexibleFaresOnly>");
                        strReq.Append("<includeInterlineFlights>false</includeInterlineFlights>");
                        strReq.Append("<openFlight>false</openFlight>");
                        strReq.Append("<originLocation>");
                        strReq.Append("<locationCode>" + IatacodeDep + "</locationCode>");
                        strReq.Append("</originLocation>");
                        strReq.Append("</originDestinationInformationList>");
                    }

                    if (fromM_2 != "" && toM_2 != null)
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

                        string[] dte = startDate[2].Split('-');
                        string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00";

                        strReq.Append("<originDestinationInformationList>");
                        strReq.Append("<dateOffset>0</dateOffset>");
                        strReq.Append("<departureDateTime>" + _date + "</departureDateTime>");
                        strReq.Append("<destinationLocation>");
                        strReq.Append("<locationCode>" + IatacodeArr + "</locationCode>");
                        strReq.Append("</destinationLocation>");
                        strReq.Append("<flexibleFaresOnly>false</flexibleFaresOnly>");
                        strReq.Append("<includeInterlineFlights>false</includeInterlineFlights>");
                        strReq.Append("<openFlight>false</openFlight>");
                        strReq.Append("<originLocation>");
                        strReq.Append("<locationCode>" + IatacodeDep + "</locationCode>");
                        strReq.Append("</originLocation>");
                        strReq.Append("</originDestinationInformationList>");
                    }

                    if (fromM_3 != "" && toM_3 != null)
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

                        string[] dte = startDate[3].Split('-');
                        string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00";

                        strReq.Append("<originDestinationInformationList>");
                        strReq.Append("<dateOffset>0</dateOffset>");
                        strReq.Append("<departureDateTime>" + _date + "</departureDateTime>");
                        strReq.Append("<destinationLocation>");
                        strReq.Append("<locationCode>" + IatacodeArr + "</locationCode>");
                        strReq.Append("</destinationLocation>");
                        strReq.Append("<flexibleFaresOnly>false</flexibleFaresOnly>");
                        strReq.Append("<includeInterlineFlights>false</includeInterlineFlights>");
                        strReq.Append("<openFlight>false</openFlight>");
                        strReq.Append("<originLocation>");
                        strReq.Append("<locationCode>" + IatacodeDep + "</locationCode>");
                        strReq.Append("</originLocation>");
                        strReq.Append("</originDestinationInformationList>");
                    }

                    if (fromM_4 != "" && toM_4 != null)
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

                        string[] dte = startDate[4].Split('-');
                        string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00";

                        strReq.Append("<originDestinationInformationList>");
                        strReq.Append("<dateOffset>0</dateOffset>");
                        strReq.Append("<departureDateTime>" + _date + "</departureDateTime>");
                        strReq.Append("<destinationLocation>");
                        strReq.Append("<locationCode>" + IatacodeArr + "</locationCode>");
                        strReq.Append("</destinationLocation>");
                        strReq.Append("<flexibleFaresOnly>false</flexibleFaresOnly>");
                        strReq.Append("<includeInterlineFlights>false</includeInterlineFlights>");
                        strReq.Append("<openFlight>false</openFlight>");
                        strReq.Append("<originLocation>");
                        strReq.Append("<locationCode>" + IatacodeDep + "</locationCode>");
                        strReq.Append("</originLocation>");
                        strReq.Append("</originDestinationInformationList>");

                    }

                    if (fromM_5 != "" && toM_5 != null)
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

                        string[] dte = startDate[5].Split('-');
                        string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00";

                        strReq.Append("<originDestinationInformationList>");
                        strReq.Append("<dateOffset>0</dateOffset>");
                        strReq.Append("<departureDateTime>" + _date + "</departureDateTime>");
                        strReq.Append("<destinationLocation>");
                        strReq.Append("<locationCode>" + IatacodeArr + "</locationCode>");
                        strReq.Append("</destinationLocation>");
                        strReq.Append("<flexibleFaresOnly>false</flexibleFaresOnly>");
                        strReq.Append("<includeInterlineFlights>false</includeInterlineFlights>");
                        strReq.Append("<openFlight>false</openFlight>");
                        strReq.Append("<originLocation>");
                        strReq.Append("<locationCode>" + IatacodeDep + "</locationCode>");
                        strReq.Append("</originLocation>");
                        strReq.Append("</originDestinationInformationList>");
                    }

                    if (fromM_6 != "" && toM_6 != null)
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

                        string[] dte = startDate[6].Split('-');
                        string _date = dte[2].ToString() + "-" + dte[1].ToString() + "-" + dte[0].ToString() + "T00:00:00";

                        strReq.Append("<originDestinationInformationList>");
                        strReq.Append("<dateOffset>0</dateOffset>");
                        strReq.Append("<departureDateTime>" + _date + "</departureDateTime>");
                        strReq.Append("<destinationLocation>");
                        strReq.Append("<locationCode>" + IatacodeArr + "</locationCode>");
                        strReq.Append("</destinationLocation>");
                        strReq.Append("<flexibleFaresOnly>false</flexibleFaresOnly>");
                        strReq.Append("<includeInterlineFlights>false</includeInterlineFlights>");
                        strReq.Append("<openFlight>false</openFlight>");
                        strReq.Append("<originLocation>");
                        strReq.Append("<locationCode>" + IatacodeDep + "</locationCode>");
                        strReq.Append("</originLocation>");
                        strReq.Append("</originDestinationInformationList>");
                    }
                    //    strReq.Append("<tripType>MULTI_DIRECTIONAL</tripType>");
                }

                strReq.Append("<travelerInformation>");

                if (adults > 0)
                {
                    strReq.Append("<passengerTypeQuantityList>");
                    strReq.Append("<hasStrecher/>");
                    strReq.Append("<passengerType>");
                    strReq.Append("<code>ADLT</code>");
                    strReq.Append("</passengerType>");
                    strReq.Append("<quantity>" + adults + "</quantity>");
                    strReq.Append("</passengerTypeQuantityList>");
                }

                if (children > 0)
                {
                    strReq.Append("<passengerTypeQuantityList>");
                    strReq.Append("<hasStrecher/>");
                    strReq.Append("<passengerType>");
                    strReq.Append("<code>CHLD</code>");
                    strReq.Append("</passengerType>");
                    strReq.Append("<quantity>" + children + "</quantity>");
                    strReq.Append("</passengerTypeQuantityList>");

                }

                if (infants > 0)
                {
                    strReq.Append("<passengerTypeQuantityList>");
                    strReq.Append("<hasStrecher/>");
                    strReq.Append("<passengerType>");
                    strReq.Append("<code>INFT</code>");
                    strReq.Append("</passengerType>");
                    strReq.Append("<quantity>" + infants + "</quantity>");
                    strReq.Append("</passengerTypeQuantityList>");
                    //strReq.Append("</travelerInformation>");
                }

                strReq.Append("</travelerInformation>");

                if (tripType == "OneWay")
                {
                    strReq.Append("<tripType>ONE_WAY</tripType>");

                }
                else if (tripType == "Return")
                {
                    strReq.Append("<tripType>ROUND_TRIP</tripType>");
                }
                else
                {
                    strReq.Append("<tripType>MULTI_DIRECTIONAL</tripType>");
                }


                strReq.Append("</AirAvailabilityRequest>");

                strReq.Append("</impl:GetAvailability>");
                strReq.Append("<impl:GetAirAvailability/>");
                strReq.Append("</soapenv:Body>");
                strReq.Append("</soapenv:Envelope>");

                //Session["GetAirAvailability_Req"] = strReq.ToString();

                //Log

                //string fileName = @"C:\hallmark\GetAvailability_OW_Req.txt"; //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Req.txt";
                //FileInfo fi = new FileInfo(fileName);

                //// Check if file already exists. If yes, delete it.
                //if (fi.Exists)
                //{
                //    fi.Delete();
                //}

                //// Create a new file     
                //using (FileStream fs = fi.Create())
                //{
                //    Byte[] txt = new UTF8Encoding(true).GetBytes(strReq.ToString());
                //    fs.Write(txt, 0, txt.Length);
                //}

                //// Write file contents on console.     
                //using (StreamReader sr = System.IO.File.OpenText(fileName))
                //{
                //    string s = "";
                //    while ((s = sr.ReadLine()) != null)
                //    {
                //        Console.WriteLine(s);
                //    }
                //}

                return strReq.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [NonAction]
        protected DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                               == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                           (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        [HttpGet("hititflights")]
        public async Task<string> HitItFlights(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr,
         string fromM_1, string fromM_2, string fromM_3, string fromM_4, string fromM_5, string fromM_6,
         string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string airline = "")
        {
            try
            {
                //7H13D7 booking
                //Session["HitITAirSegments"] = null;
                //Session["dtFareList"] = null;
                //Session["dtCityInfo"] = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string url = "https://app.crane.aero/craneota/CraneOTAService";
                //string url = "https://app-stage.crane.aero/craneota/CraneOTAService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                // byte[] bytes;

                //   bytes = System.Text.Encoding.ASCII.GetBytes(HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline));
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz"))); //Test env
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");


                ////Stream requestStream = request.GetRequestStream();
                ////requestStream.Write(bytes, 0, bytes.Length);
                ////requestStream.Close();
                ///
                var xmlReq = HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

                //HttpWebResponse response;
                //response = (HttpWebResponse)request.GetResponse();
                //Stream responseStream;
                //TextReader responseStr;
                //DataTable FlightSegments = null;
                //DataTable dt;
                DataSet ds = new DataSet();

                //if (Session["AirSegments"] != null)
                //{
                //    Session["AirSegments"] = null;
                //}
                //  MalikExpressMVC.Models.Galileo.Envelope obj;
                // MalikExpressMVC.Models.Galileo.LowFareSearchRspObj rsp;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // responseStream = response.GetResponseStream();
                    //  responseStr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                    // var responseReader = new StreamReader(responseStream);
                    // var responsestr1 = new StreamReader(responseStream).ReadToEnd();

                    var str1 = response.RequestMessage;
                    var str2 = response.Content.Headers;
                    // Get the response
                    var customerJsonString = await response.Content.ReadAsStringAsync();

                    // DataSet ds = new DataSet();

                    DataTable FlightSegments = null;
                    DataTable dt;

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(custome‌​rJsonString);
                    XmlReader xmlReader = new XmlNodeReader(xml);
                    ds.ReadXml(xmlReader);

                    //TextReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);

                    // XmlSerializer xmlSer = new XmlSerializer(typeof(MalikExpressMVC.Models.HitIt.Envelope));
                    // MalikExpressMVC.Models.HitIt.Envelope obj = (MalikExpressMVC.Models.HitIt.Envelope)xmlSer.Deserialize(respStream);
                    // MalikExpressMVC.Models.HitIt.GetAvailabilityResponse enObj = obj.Body.GetAvailabilityResponse;

                    //Session["HitItFlights"] = enObj;
                    //return;

                    //Log

                    string fileName = @"C:\hallmark\GetAvailability_RT_Req.txt";  //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Res.txt";
                    FileInfo fi = new FileInfo(fileName);

                    //////Check if file already exists. If yes, delete it.
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }

                    ////Create a new file
                    using (FileStream fs = fi.Create())
                    {
                        Byte[] txt = new UTF8Encoding(true).GetBytes(xmlReq);
                        fs.Write(txt, 0, txt.Length);
                    }

                    //////Write file contents on console.
                    ////using (StreamReader sr = System.IO.File.OpenText(fileName))
                    ////{
                    ////    string s = "";
                    ////    while ((s = sr.ReadLine()) != null)
                    ////    {
                    ////        Console.WriteLine(s);
                    ////    }
                    ////}

                    fileName = @"C:\hallmark\GetAvailability_RT_Res.txt";  //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Res.txt";
                    FileInfo fi1 = new FileInfo(fileName);

                    //////Check if file already exists. If yes, delete it.
                    if (fi1.Exists)
                    {
                        fi1.Delete();
                    }

                    ////Create a new file
                    using (FileStream fs = fi1.Create())
                    {
                        Byte[] txt = new UTF8Encoding(true).GetBytes(customerJsonString);
                        fs.Write(txt, 0, txt.Length);
                    }

                    //////Write file contents on console.
                    ////using (StreamReader sr = System.IO.File.OpenText(fileName))
                    ////{
                    ////    string s = "";
                    ////    while ((s = sr.ReadLine()) != null)
                    ////    {
                    ////        Console.WriteLine(s);
                    ////    }
                    ////}


                    // Commnent above 9 lines before publishing


                    StringBuilder sb = new StringBuilder();

                    ////get dataset table names and columns names
                    //foreach(DataTable dt11 in ds.Tables)
                    //{
                    //    sb.Append(dt11.TableName);
                    //    sb.Append(Environment.NewLine);
                    //    foreach (DataColumn col in dt11.Columns)
                    //    {
                    //        sb.Append(col.ColumnName);
                    //        sb.Append(Environment.NewLine);
                    //    }
                    //    sb.Append(Environment.NewLine);
                    //}

                    var CityInfo = (from p in ds.Tables["cityInfo"].AsEnumerable()
                                    join t in ds.Tables["city"].AsEnumerable()
                                    on p.Field<int?>("cityInfo_Id") equals t.Field<int?>("cityInfo_Id")
                                    join t1 in ds.Tables["country"].AsEnumerable()
                                   on p.Field<int?>("cityInfo_Id") equals t1.Field<int?>("cityInfo_Id")
                                    select new
                                    {
                                        cityInfo_Id = p.Field<int?>("cityInfo_Id"),
                                        arrivalAirport_Id = p.Field<int?>("arrivalAirport_Id") == null ? 0 : p.Field<int?>("arrivalAirport_Id"),
                                        departureAirport_Id = p.Field<int?>("departureAirport_Id"),
                                        locationCode = t.Field<string>("locationCode"),
                                        locationName = t.Field<string>("locationName"),
                                        locationNameLanguage = t.Field<string>("locationNameLanguage"),
                                        country_locationCode = t1.Field<string>("locationCode"),
                                        country_locationName = t1.Field<string>("locationName"),
                                        countrylocationNameLanguage = t1.Field<string>("locationNameLanguage"),
                                        country_Id = t1.Field<int?>("country_Id"),
                                    }).ToList();

                    DataTable dtCityInfo = LINQResultToDataTable(CityInfo);

                    var JoinResult = (from p in ds.Tables["Availability"].AsEnumerable()
                                      join t in ds.Tables["availabilityResultList"].AsEnumerable()
                                      on p.Field<int?>("Availability_Id") equals t.Field<int?>("Availability_Id")
                                      join t1 in ds.Tables["availabilityRouteList"].AsEnumerable()
                                      on t.Field<int?>("availabilityResultList_Id") equals t1.Field<int?>("availabilityResultList_Id")
                                      join t2 in ds.Tables["availabilityByDateList"].AsEnumerable()
                                     on t1.Field<int?>("availabilityRouteList_Id") equals t2.Field<int?>("availabilityRouteList_Id")
                                      join t3 in ds.Tables["originDestinationOptionList"].AsEnumerable()
                                      on t2.Field<int?>("availabilityByDateList_id") equals t3.Field<int?>("availabilityByDateList_id")
                                      join t4 in ds.Tables["fareComponentGroupList"].AsEnumerable()
                                      on t3.Field<int?>("originDestinationOptionList_Id") equals t4.Field<int?>("originDestinationOptionList_Id")
                                      join t5 in ds.Tables["boundList"].AsEnumerable()
                                      on t4.Field<int?>("fareComponentGroupList_Id") equals t5.Field<int?>("fareComponentGroupList_Id")
                                      join t6 in ds.Tables["availFlightSegmentList"].AsEnumerable()
                                      on t5.Field<int?>("boundList_Id") equals t6.Field<int?>("boundList_Id")
                                      join t7 in ds.Tables["bookingClassList"].AsEnumerable()
                                      on t6.Field<int?>("availFlightSegmentList_Id") equals t7.Field<int?>("availFlightSegmentList_Id")
                                      join t8 in ds.Tables["flightSegment"].AsEnumerable()
                                      on t7.Field<int?>("availFlightSegmentList_Id") equals t8.Field<int?>("availFlightSegmentList_Id")
                                      join t9 in ds.Tables["airline"].AsEnumerable()
                                      on t8.Field<int?>("flightSegment_Id") equals t9.Field<int?>("flightSegment_Id")
                                      join t10 in ds.Tables["arrivalAirport"].AsEnumerable()
                                      on t9.Field<int?>("flightSegment_Id") equals t10.Field<int?>("flightSegment_Id")
                                      join t11 in ds.Tables["departureAirport"].AsEnumerable()
                                      on t10.Field<int?>("flightSegment_Id") equals t11.Field<int?>("flightSegment_Id")
                                      join t12 in ds.Tables["equipment"].AsEnumerable()
                                      on t10.Field<int?>("flightSegment_Id") equals t12.Field<int?>("flightSegment_Id")
                                      //join t13 in ds.Tables["flightNotes"].AsEnumerable()
                                      //on t10.Field<int?>("flightSegment_Id") equals t13.Field<int?>("flightSegment_Id")
                                      select new
                                      {
                                          Availability_Id = p.Field<int?>("Availability_Id"),
                                          availabilityResultList_Id = t.Field<int?>("availabilityResultList_Id"),
                                          availabilityRouteList_Id = t1.Field<int?>("availabilityRouteList_Id"),
                                          flexibleFaresOnly = t1.Field<string>("flexibleFaresOnly"),
                                          dateList = t2.Field<string>("dateList"),
                                          availabilityByDateList_Id = t2.Field<int?>("availabilityByDateList_Id"),
                                          originDestinationOptionList_Id = t3.Field<int?>("originDestinationOptionList_Id"),
                                          fareComponentGroupList_Id = t4.Field<int?>("fareComponentGroupList_Id"),
                                          boundList_Id = t5.Field<int?>("boundList_Id"),
                                          boundCode = t5.Field<string>("boundCode"),
                                          availFlightSegmentList_Id = t6.Field<int?>("availFlightSegmentList_Id"),
                                          addOnSegment = t6.Field<string>("addOnSegment"),
                                          cabin = t7.Field<string>("cabin"),
                                          resBookDesigCode = t7.Field<string>("resBookDesigCode"),
                                          resBookDesigQuantity = t7.Field<string>("resBookDesigQuantity"),
                                          flightSegment_Id = t8.Field<int?>("flightSegment_Id"),
                                          departureDateTime = t8.Field<string>("departureDateTime"),
                                          departureDateTimeUTC = t8.Field<string>("departureDateTimeUTC"),
                                          arrivalDateTime = t8.Field<string>("arrivalDateTime"),
                                          arrivalDateTimeUTC = t8.Field<string>("arrivalDateTimeUTC"),
                                          flightNumber = t8.Field<string>("flightNumber"),
                                          ondControlled = t8.Field<string>("ondControlled"),
                                          sector = t8.Field<string>("sector"),
                                          codeshare = t8.Field<string>("codeshare"),
                                          distance = t8.Field<string>("distance"),
                                          flownMileageQty = t8.Field<string>("flownMileageQty"),
                                          journeyDuration = t8.Field<string>("journeyDuration"),
                                          onTimeRate = t8.Field<string>("onTimeRate"),
                                          secureFlightDataRequired = t8.Field<string>("secureFlightDataRequired"),
                                          stopQuantity = t8.Field<string>("stopQuantity"),
                                          code = t9.Field<string>("code"),
                                          companyFullName = t9.Field<string>("companyFullName"),
                                          companyShortName = t9.Field<string>("companyShortName"),
                                          departureAirport_Id = t11.Field<int?>("departureAirport_Id"),
                                          dep_codeContext = t11.Field<string>("codeContext"),
                                          dep_language = t11.Field<string>("language"),
                                          dep_locationCode = t11.Field<string>("locationCode"),
                                          dep_locationName = t11.Field<string>("locationName"),
                                          dep_terminal = t11.Field<string>("terminal"),
                                          arrivalAirport_Id = t10.Field<int?>("arrivalAirport_Id"),
                                          codeContext = t10.Field<string>("codeContext"),
                                          language = t10.Field<string>("language"),
                                          arr_locationCode = t10.Field<string>("locationCode"),
                                          locationName = t10.Field<string>("locationName"),
                                          terminal = t10.Field<string>("terminal"),
                                          airEquipType = t12.Field<string>("airEquipType"),
                                          airEquipTypeModel = t12.Field<string>("airEquipTypeModel"),
                                          changeofGauge = t12.Field<string>("changeofGauge"),
                                          //deiCode = t13.Field<string>("deiCode"),
                                          //explanation = t13.Field<string>("explanation"),
                                          //note = t13.Field<string>("note"),


                                      }).ToList();

                    DataTable flights = LINQResultToDataTable(JoinResult);

                    var FareList = (from p in ds.Tables["fareComponentList"].AsEnumerable()
                                    join t in ds.Tables["passengerFareInfoList"].AsEnumerable()
                                    on p.Field<int?>("fareComponentList_Id") equals t.Field<int?>("fareComponentList_Id")
                                    join t1 in ds.Tables["fareInfoList"].AsEnumerable()
                                    on t.Field<int?>("passengerFareInfoList_Id") equals t1.Field<int?>("passengerFareInfoList_Id")
                                    join t2 in ds.Tables["fareBaggageAllowance"].AsEnumerable()
                                    on t1.Field<int?>("fareInfoList_Id") equals t2.Field<int?>("fareInfoList_Id")
                                    join t4 in ds.Tables["passengerTypeQuantity"].AsEnumerable()
                                    on t.Field<int?>("passengerFareInfoList_Id") equals t4.Field<int?>("passengerFareInfoList_Id")
                                    join t5 in ds.Tables["passengerType"].AsEnumerable()
                                    on t4.Field<int?>("passengerTypeQuantity_Id") equals t5.Field<int?>("passengerTypeQuantity_Id")
                                    join t3 in ds.Tables["maxAllowedWeight"].AsEnumerable()
                                    on t2.Field<int?>("fareBaggageAllowance_Id") equals t3.Field<int?>("fareBaggageAllowance_Id")
                                    join t6 in ds.Tables["pricingInfo"].AsEnumerable()
                                    on t.Field<int?>("passengerFareInfoList_Id") equals t6.Field<int?>("passengerFareInfoList_Id")
                                    join t7 in ds.Tables["baseFare"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t7.Field<int?>("pricingInfo_Id")
                                    join t8 in ds.Tables["amount"].AsEnumerable()
                                    on t7.Field<int?>("baseFare_Id") equals t8.Field<int?>("baseFare_Id")
                                    join t9 in ds.Tables["equivBaseFare"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t9.Field<int?>("pricingInfo_Id")
                                    join t10 in ds.Tables["surcharges"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t10.Field<int?>("pricingInfo_Id")
                                    join t11 in ds.Tables["pricingOverview"].AsEnumerable()
                                    on p.Field<int?>("fareComponentList_Id") equals t11.Field<int?>("fareComponentList_Id")
                                    join t12 in ds.Tables["totalAmount"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t12.Field<int?>("pricingOverview_Id")
                                    join t13 in ds.Tables["taxes"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t13.Field<int?>("pricingInfo_Id")
                                    join t14 in ds.Tables["taxList"].AsEnumerable()
                                    on t13.Field<int?>("taxes_Id") equals t14.Field<int?>("taxes_Id")
                                    join t15 in ds.Tables["taxAmount"].AsEnumerable()
                                    on t14.Field<int?>("taxList_Id") equals t15.Field<int?>("taxList_Id")
                                    join t16 in ds.Tables["totalFare"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t16.Field<int?>("pricingInfo_Id")
                                    join t17 in ds.Tables["totalBaseFare"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t17.Field<int?>("pricingOverview_Id")
                                    join t18 in ds.Tables["totalSurcharge"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t18.Field<int?>("pricingOverview_Id")
                                    join t19 in ds.Tables["totalTax"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t19.Field<int?>("pricingOverview_Id")
                                    select new
                                    {
                                        fareComponentGroupList_Id = p.Field<int?>("fareComponentGroupList_Id"),
                                        fareComponentList_Id = p.Field<int?>("fareComponentList_Id"),
                                        fareKind = p.Field<string>("fareKind"),
                                        internalID = p.Field<string>("internalID"),
                                        maxUsageDate = p.Field<string>("maxUsageDate"),
                                        passengerFareInfoList_Id = t1.Field<int?>("passengerFareInfoList_Id"),
                                        // endorsementList = t.Field<string>("endorsementList"),
                                        fareInfoList_Id = t1.Field<int?>("fareInfoList_Id"),
                                        cabinClassCode = t1.Field<string>("cabinClassCode"),
                                        fareGroupName = t1.Field<string>("fareGroupName"),
                                        fareReferenceCode = t1.Field<string>("fareReferenceCode"),
                                        fareReferenceID = t1.Field<string>("fareReferenceID"),
                                        fareReferenceName = t1.Field<string>("fareReferenceName"),
                                        flightSegmentSequence = t1.Field<string>("flightSegmentSequence"),
                                        notValidAfter = t1.Field<string>("notValidAfter"),
                                        notValidBefore = t1.Field<string>("notValidBefore"),
                                        resBookDesigCode = t1.Field<string>("resBookDesigCode"),
                                        fareBaggageAllowance_Id = t2.Field<int?>("fareBaggageAllowance_Id"),
                                        allowanceType = t2.Field<string>("allowanceType"),
                                        maxAllowedPieces = t2.Field<string>("maxAllowedPieces"),
                                        unitOfMeasureCode = t3.Field<string>("unitOfMeasureCode"),
                                        weight = t3.Field<string>("weight"),
                                        passengerTypeQuantity_Id = t4.Field<int?>("passengerTypeQuantity_Id"),
                                        hasStrecher = t4.Field<string>("hasStrecher"),
                                        quantity = t4.Field<string>("quantity"),
                                        passengerType_code = t5.Field<string>("code"),
                                        pricingInfo_Id = t6.Field<int?>("pricingInfo_Id"),
                                        fareBaggageAllowance = t6.Field<string>("fareBaggageAllowance"),
                                        fareConstruction = t6.Field<string>("fareConstruction"),
                                        discountApplied = t6.Field<string>("discountApplied"),
                                        passengerTypeCode = t6.Field<string>("passengerTypeCode"),
                                        baseFare_id = t7.Field<int?>("baseFare_id"),
                                        amount_Id = t8.Field<int?>("amount_Id"),
                                        amount_totalFare_Id = t8.Field<int?>("totalFare_Id"),
                                        accountingSign = t8.Field<string>("accountingSign"),
                                        mileAmount = t8.Field<string>("mileAmount"),
                                        value = t8.Field<string>("value"),
                                        equivBaseFare_Id = t9.Field<int?>("equivBaseFare_Id"),
                                        equivBaseFare_accountingSign = t9.Field<string>("accountingSign"),
                                        equivBaseFare_mileAmount = t9.Field<string>("mileAmount"),
                                        equivBaseFare_value = t9.Field<string>("value"),
                                        surcharges_Id = t10.Field<int?>("surcharges_Id"),
                                        pricingOverview_Id = t11.Field<int?>("pricingOverview_Id"),
                                        totalAmount_accountingSign = t12.Field<string>("accountingSign"),
                                        totalAmount_totalAmount_Id = t12.Field<int?>("totalAmount_Id"),
                                        totalAmount_mileAmount = t12.Field<string>("mileAmount"),
                                        totalAmount_value = t12.Field<string>("value"),
                                        totalAmount_surcharges_Id = t12.Field<int?>("surcharges_Id"),
                                        totalAmount_taxes_Id = t12.Field<int?>("taxes_Id"),
                                        taxes_taxes_Id = t13.Field<int?>("taxes_Id"),
                                        taxList_Id = t14.Field<int?>("taxList_Id"),
                                        paid = t14.Field<string>("paid"),
                                        taxCode = t14.Field<string>("taxCode"),
                                        taxAmount_taxAmount_Id = t15.Field<int?>("taxAmount_Id"),
                                        taxAmount_accountingSign = t15.Field<string>("accountingSign"),
                                        taxAmount_mileAmount = t15.Field<string>("mileAmount"),
                                        taxAmount_value = t15.Field<string>("value"),
                                        totalFare_Id = t16.Field<int?>("totalFare_Id"),
                                        totalBaseFare_Id = t17.Field<int?>("totalBaseFare_Id"),
                                        totalBaseFare_accountingSign = t17.Field<string>("accountingSign"),
                                        totalBaseFare_mileAmount = t17.Field<string>("mileAmount"),
                                        totalBaseFare_value = t17.Field<string>("value"),
                                        totalSurcharge_Id = t18.Field<int?>("totalSurcharge_Id"),
                                        totalSurcharge_accountingSign = t18.Field<string>("accountingSign"),
                                        totalSurcharge_mileAmount = t18.Field<string>("mileAmount"),
                                        totalSurcharge_value = t18.Field<string>("value"),
                                        totalTax_Id = t19.Field<int?>("totalTax_Id"),
                                        totalTax_accountingSign = t19.Field<string>("accountingSign"),
                                        totalTax_mileAmount = t19.Field<string>("mileAmount"),
                                        totalTax_value = t19.Field<string>("value"),
                                    }).ToList();

                    DataTable dtFareList = LINQResultToDataTable(FareList);

                    DataColumn column = new DataColumn("AdultTotalPrice");
                    column.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column1 = new DataColumn("AdultBasePrice");
                    column1.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column2 = new DataColumn("AdultTaxes");
                    column2.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column3 = new DataColumn("ChildTotalPrice");
                    column3.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column4 = new DataColumn("ChildBasePrice");
                    column4.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column5 = new DataColumn("InfantTotalPrice");
                    column5.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column6 = new DataColumn("InfantBasePrice");
                    column6.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column7 = new DataColumn("InfantTaxes");
                    column7.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column8 = new DataColumn("ChildTaxes");
                    column8.DataType = System.Type.GetType("System.Decimal");

                    DataColumn GrandTotal = new DataColumn("GrandTotal");
                    GrandTotal.DataType = System.Type.GetType("System.Decimal");

                    //DataColumn dep_locationCode = new DataColumn("dep_locationCode");
                    //dep_locationCode.DataType = System.Type.GetType("System.String");

                    //DataColumn column13 = new DataColumn("departureAirport_Id");
                    //column13.DataType = System.Type.GetType("System.Int");

                    //DataColumn column14 = new DataColumn("locationCode");
                    //column14.DataType = System.Type.GetType("System.String");

                    //DataColumn column15 = new DataColumn("locationName");
                    //column15.DataType = System.Type.GetType("System.String");

                    //DataColumn column16 = new DataColumn("locationNameLanguage");
                    //column16.DataType = System.Type.GetType("System.String");

                    //DataColumn column17 = new DataColumn("country_locationCode");
                    //column17.DataType = System.Type.GetType("System.String");

                    //DataColumn column18 = new DataColumn("country_locationName");
                    //column18.DataType = System.Type.GetType("System.String");

                    //DataColumn column19 = new DataColumn("countrylocationNameLanguage");
                    //column19.DataType = System.Type.GetType("System.String");

                    //DataColumn column20 = new DataColumn("country_Id");
                    //column20.DataType = System.Type.GetType("System.Int");

                    flights.Columns.Add(column);
                    flights.Columns.Add(column1);
                    flights.Columns.Add(column2);
                    flights.Columns.Add(column3);
                    flights.Columns.Add(column4);
                    flights.Columns.Add(column5);
                    flights.Columns.Add(column6);
                    flights.Columns.Add(column7);
                    flights.Columns.Add(column8);
                    flights.Columns.Add(GrandTotal);
                    flights.Columns.Add("allowanceType");
                    flights.Columns.Add("maxAllowedPieces");
                    flights.Columns.Add("unitOfMeasureCode");
                    flights.Columns.Add("weight");

                    //flights.Columns.Add(column13);
                    //flights.Columns.Add(column14);
                    //flights.Columns.Add(column15);
                    //flights.Columns.Add(column16);
                    //flights.Columns.Add(column17);
                    //flights.Columns.Add(column19);
                    //flights.Columns.Add(column20);

                    foreach (DataRow ro in flights.Rows)
                    {
                        foreach (DataRow ro1 in dtFareList.Rows)
                        {
                            if (Convert.ToString(ro["fareComponentGroupList_Id"]) == Convert.ToString(ro1["fareComponentGroupList_Id"]) &&
                                Convert.ToString(ro["resBookDesigCode"]) == Convert.ToString(ro1["resBookDesigCode"]))
                            {
                                ro["allowanceType"] = ro1["allowanceType"];
                                ro["maxAllowedPieces"] = ro1["maxAllowedPieces"];
                                ro["unitOfMeasureCode"] = ro1["unitOfMeasureCode"];
                                ro["weight"] = ro1["weight"];

                                if (ro1["passengerType_code"].ToString() == "ADLT" && ro["AdultBasePrice"] == DBNull.Value)
                                {
                                    ro["AdultTotalPrice"] = ro1["totalAmount_value"];
                                    ro["AdultBasePrice"] = ro1["equivBaseFare_value"];
                                    ro["AdultTaxes"] = ro1["totalTax_value"];
                                    ro["ChildTotalPrice"] = 0;
                                    ro["ChildBasePrice"] = 0;
                                    ro["ChildTaxes"] = 0;
                                    ro["InfantTotalPrice"] = 0;
                                    ro["InfantBasePrice"] = 0;
                                    ro["InfantTaxes"] = 0;
                                }
                                else if (ro1["passengerType_code"].ToString() == "CHLD" && ro["ChildBasePrice"] == DBNull.Value)
                                {
                                    ro["ChildTotalPrice"] = ro1["totalAmount_value"];
                                    ro["ChildBasePrice"] = ro1["equivBaseFare_value"];
                                    ro["ChildTaxes"] = ro1["totalTax_value"];
                                    ro["InfantTotalPrice"] = 0;
                                    ro["InfantBasePrice"] = 0;
                                    ro["InfantTaxes"] = 0;
                                }
                                else if (ro1["passengerType_code"].ToString() == "INFT" && ro["InfantBasePrice"] == DBNull.Value)
                                {
                                    ro["InfantTotalPrice"] = ro1["totalAmount_value"];
                                    ro["InfantBasePrice"] = ro1["equivBaseFare_value"];
                                    ro["InfantTaxes"] = ro1["totalTax_value"];
                                }
                            }
                        }
                    }

                    foreach (DataRow ro in flights.Rows)
                    {

                        ro["GrandTotal"] = Convert.ToDecimal(ro["AdultTotalPrice"]) + Convert.ToDecimal(ro["ChildTotalPrice"]) +
                                        Convert.ToDecimal(ro["InfantTotalPrice"]) + 1000;
                    }

                    //foreach (DataRow ro in flights.Rows)
                    //{
                    //    foreach (DataRow ro1 in ds.Tables["arrivalAirport"].Rows)
                    //    {
                    //        if (Convert.ToString(ro["flightSegment_Id"]) == Convert.ToString(ro1["flightSegment_Id"]))
                    //        {
                    //            ro["arr_locationCode"] = ro1["locationCode"];
                    //        }
                    //    }
                    //}


                    int optionID = 0;
                    flights.Columns.Add("Duration");
                    flights.Columns.Add("TotalDuration");
                    flights.Columns.Add("Option_Id");
                    flights.Columns.Add("Direct");
                    flights.Columns.Add("Flight");


                    foreach (DataRow dr in flights.Rows)
                    {
                        string dur = dr["journeyDuration"].ToString();
                        string[] d;
                        int h = 0;
                        int m = 0;

                        if (dur.IndexOf("PT") > -1)
                        {
                            dur = dur.Replace("PT", "");
                        }

                        if (dur.IndexOf('H') > -1 && dur.IndexOf('M') > -1)
                        {
                            if (dur.IndexOf('M') > -1)
                            {
                                dur = dur.Replace("M", "");
                            }

                            dur = dur.Replace("H", ":");
                            d = dur.Split(":");
                            h = Convert.ToInt32(d[0]);
                            m = Convert.ToInt32(d[1]);
                            h = h * 60;
                            h = h + m;
                        }
                        else
                        {
                            dur = dur.Replace("H", "");
                            h = Convert.ToInt32(dur);
                            h = h * 60;
                        }

                        dr["Duration"] = h;
                        dr["TotalDuration"] = h;
                    }

                    foreach (DataRow dr in flights.Rows)
                    {
                        if (dr["dep_locationCode"].ToString() == from && dr["arr_locationCode"].ToString() == to)
                        {
                            dr["Direct"] = 1;
                        }
                        else
                        {
                            dr["Direct"] = 0;
                        }

                        if (dr["dep_locationCode"].ToString() != from)
                        {
                            dr["Flight"] = "DC"; //Connecting Flight
                        }
                        else
                        {
                            dr["Flight"] = "DM"; //Main Flight
                        }
                    }


                    //&& dr["Flight"] = "M"

                    foreach (DataRow dr in flights.Rows)
                    {
                        string cabin = "";
                        cabin = dr["Cabin"].ToString();

                        if (dr["Option_Id"] == null || dr["Option_Id"].ToString() == "")
                        {
                            foreach (DataRow dr1 in flights.Rows)
                            {
                                if (dr1["Flight"].ToString() == "DM" && dr1["Cabin"].ToString() == cabin && dr1["Direct"].ToString() == "1")
                                {
                                    dr1["Option_Id"] = optionID;
                                    optionID = optionID + 1;
                                }
                                else if (dr1["Flight"].ToString() == "DM" && dr1["Cabin"].ToString() == cabin && dr1["Direct"].ToString() == "0")
                                {
                                    dr1["Option_Id"] = optionID;

                                }
                                else if (dr1["Flight"].ToString() == "DC" && dr1["Cabin"].ToString() == cabin)
                                {
                                    dr1["Option_Id"] = optionID;
                                    optionID = optionID + 1;
                                }
                            }

                        }
                    }

                    foreach (DataRow dr in flights.Rows)
                    {
                        foreach (DataRow dr1 in flights.Rows)
                        {
                            if (dr["Option_Id"].ToString() == dr1["Option_Id"].ToString())
                            {
                                if ((dr["Flight"] == "DM" && dr1["Flight"] == "DM") && (dr["Direct"] == "0" && dr1["Direct"] == "0"))
                                {
                                    dr["TotalDuration"] = Convert.ToInt32(dr["Duration"]) + Convert.ToInt32(dr1["Duration"]);
                                }
                            }
                            //else
                            //{

                            //}
                        }
                    }


                    Guid TransactionId = Guid.NewGuid();

                    flights.Columns.Add("TransactionId");
                    foreach (DataRow dr in flights.Rows)
                    {
                        dr["TransactionId"] = TransactionId;
                    }


                    DataColumn column0 = new DataColumn("SegmentID");
                    column0.DataType = System.Type.GetType("System.Int32");

                    List<DataRow> rowsToRemove = new List<DataRow>();

                    foreach (DataRow ro in flights.Rows)
                    {
                        if (ro["Direct"] == "0")
                        {
                            rowsToRemove.Add(ro);
                        }
                    }

                    foreach (DataRow rowToRemove in rowsToRemove)
                    {
                        flights.Rows.Remove(rowToRemove);
                    }

                    flights.AcceptChanges();


                    flights.Columns.Add(column0);
                    column0.SetOrdinal(0);

                    for (int a = 0; a < flights.Rows.Count; a++)
                    {
                        flights.Rows[a]["SegmentID"] = a;
                    }

                    DataView dv = flights.DefaultView;
                    dv.Sort = "GrandTotal";
                    flights = dv.ToTable();

                    MemoryStream stream = new MemoryStream();
                    ds.WriteXml(stream);
                    byte[] hititResponse = stream.ToArray();
                    stream = null;

                    MemoryStream streamFL = new MemoryStream();
                    dtFareList.TableName = "dtFareList";
                    dtFareList.WriteXml(streamFL);
                    byte[] fareList = streamFL.ToArray();
                    streamFL = null;

                    MemoryStream streamCI = new MemoryStream();
                    dtCityInfo.TableName = "dtCityInfo";
                    dtCityInfo.WriteXml(streamCI);
                    byte[] cityInfo = streamCI.ToArray();
                    streamCI = null;

                    //   ds.Tables["flightNotes"].WriteXml(stream);
                    //   byte[] flightNotes = stream.ToArray();

                    MemoryStream streamFligts = new MemoryStream();
                    flights.TableName = "flights";
                    flights.WriteXml(streamFligts);
                    byte[] finalResult = streamFligts.ToArray();
                    streamFligts = null;


                    HitItResult hr = new HitItResult();
                    hr.TransactionId = TransactionId;
                    hr.Origin = from;
                    hr.Destination = to;
                    hr.TotalPax = adults + children + infants;
                    hr.StartDate = Dept;
                    hr.HititResponse = hititResponse;
                    hr.FareList = fareList;
                    hr.CityInfo = cityInfo;
                    // hr.FlightNotes = flightNotes;
                    hr.FinalResult = finalResult;
                    hr.Active = true;
                    sql.HitItResults.Add(hr);
                    sql.SaveChanges();

                    var JSONString = DataTableToJSONWithStringBuilder(flights);
                    return JSONString;


                }
                return "something went wrong";
            }
            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message;
                return "something went wrong"; // ex.Message.ToString();// "something went wrong";
            }
        }

        [HttpGet("hititflightsreturn")]
        public async Task<string> HitItFlights_Returns(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr,
         string fromM_1, string fromM_2, string fromM_3, string fromM_4, string fromM_5, string fromM_6,
         string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string airline = "")
        {
            try
            {
                //Session["HitITAirSegments"] = null;
                //Session["dtFareList"] = null;
                //Session["dtCityInfo"] = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string url = "https://app.crane.aero/craneota/CraneOTAService";
                //string url = "https://app-stage.crane.aero/craneota/CraneOTAService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                // byte[] bytes;

                //   bytes = System.Text.Encoding.ASCII.GetBytes(HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline));
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz"))); //Test env
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");


                ////Stream requestStream = request.GetRequestStream();
                ////requestStream.Write(bytes, 0, bytes.Length);
                ////requestStream.Close();
                ///
                var xmlReq = HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

                DataSet ds = new DataSet();

                //if (Session["AirSegments"] != null)
                //{
                //    Session["AirSegments"] = null;
                //}
                //  MalikExpressMVC.Models.Galileo.Envelope obj;
                // MalikExpressMVC.Models.Galileo.LowFareSearchRspObj rsp;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var str1 = response.RequestMessage;
                    var str2 = response.Content.Headers;
                    // Get the response
                    var customerJsonString = await response.Content.ReadAsStringAsync();

                    // DataSet ds = new DataSet();

                    DataTable FlightSegments = null;
                    DataTable dt;

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(custome‌​rJsonString);
                    XmlReader xmlReader = new XmlNodeReader(xml);
                    ds.ReadXml(xmlReader);

                    //TextReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);

                    // XmlSerializer xmlSer = new XmlSerializer(typeof(MalikExpressMVC.Models.HitIt.Envelope));
                    // MalikExpressMVC.Models.HitIt.Envelope obj = (MalikExpressMVC.Models.HitIt.Envelope)xmlSer.Deserialize(respStream);
                    // MalikExpressMVC.Models.HitIt.GetAvailabilityResponse enObj = obj.Body.GetAvailabilityResponse;

                    //Session["HitItFlights"] = enObj;
                    //return;

                    //Log

                    string fileName = @"C:\hallmark\GetAvailability_RT_Req.txt";  //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Res.txt";
                    FileInfo fi = new FileInfo(fileName);

                    //////Check if file already exists. If yes, delete it.
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }

                    ////Create a new file
                    using (FileStream fs = fi.Create())
                    {
                        Byte[] txt = new UTF8Encoding(true).GetBytes(xmlReq);
                        fs.Write(txt, 0, txt.Length);
                    }

                    //////Write file contents on console.
                    ////using (StreamReader sr = System.IO.File.OpenText(fileName))
                    ////{
                    ////    string s = "";
                    ////    while ((s = sr.ReadLine()) != null)
                    ////    {
                    ////        Console.WriteLine(s);
                    ////    }
                    ////}

                    fileName = @"C:\hallmark\GetAvailability_RT_Res.txt";  //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Res.txt";
                    FileInfo fi1 = new FileInfo(fileName);

                    //////Check if file already exists. If yes, delete it.
                    if (fi1.Exists)
                    {
                        fi1.Delete();
                    }

                    ////Create a new file
                    using (FileStream fs = fi1.Create())
                    {
                        Byte[] txt = new UTF8Encoding(true).GetBytes(customerJsonString);
                        fs.Write(txt, 0, txt.Length);
                    }

                    //////Write file contents on console.
                    ////using (StreamReader sr = System.IO.File.OpenText(fileName))
                    ////{
                    ////    string s = "";
                    ////    while ((s = sr.ReadLine()) != null)
                    ////    {
                    ////        Console.WriteLine(s);
                    ////    }
                    ////}


                    // Commnent above 9 lines before publishing

                    // Commnent above 9 lines before publishing

                    ds.ReadXml(xmlReader);

                    StringBuilder sb = new StringBuilder();

                    // DataTable flightSegments = ds.Tables["flightSegment"];

                    ds.Tables["flightSegment"].Columns.Add("FlightType");


                    DataColumn SegmentGroupID = new DataColumn("SegmentGroupID");
                    SegmentGroupID.DataType = System.Type.GetType("System.Int32");

                    ds.Tables["flightSegment"].Columns.Add(SegmentGroupID);

                    //ds.Tables["flightSegment"].Columns[SegmentGroupID].SetOrdinal(1);
                    SegmentGroupID.SetOrdinal(1);
                    ds.Tables["flightSegment"].Columns["FlightType"].SetOrdinal(2);

                    foreach (DataRow dr in ds.Tables["flightSegment"].Rows)
                    {
                        if (dr["FlightType"] == DBNull.Value || dr["FlightType"] == "")
                        {
                            if (Convert.ToDateTime(dr["departureDateTime"]).ToShortDateString() == Convert.ToDateTime(Dept).ToShortDateString())
                            {
                                dr["FlightType"] = "Dep Main";
                            }
                            //else if (Convert.ToDateTime(dr["departureDateTime"]).ToShortDateString() == Convert.ToDateTime(Dept).ToShortDateString() && dr["sector"].ToString().ToLower() == "domestic")
                            //{
                            //    dr["FlightType"] = "Dep Connecting";
                            //}
                            //if (Convert.ToDateTime(dr["arrivalDateTime"]).ToShortDateString() == Convert.ToDateTime(Arr).ToShortDateString() && dr["sector"].ToString().ToLower() == "international")
                            //{
                            //    dr["FlightType"] = "Ret Main";
                            //}
                            //else if (Convert.ToDateTime(dr["arrivalDateTime"]).ToShortDateString() == Convert.ToDateTime(Arr).ToShortDateString() && dr["sector"].ToString().ToLower() == "domestic")
                            //{
                            //    dr["FlightType"] = "Ret Connecting";
                            //}
                        }
                    }

                    int currentGroupId = -1;

                    foreach (DataRow row in ds.Tables["flightSegment"].Rows)
                    {
                        string flightType = row["FlightType"].ToString();

                        if (flightType == "Dep Main")
                        {
                            currentGroupId++;
                            row[SegmentGroupID] = currentGroupId;
                        }
                        else
                        {
                            row[SegmentGroupID] = currentGroupId;
                        }
                    }

                    DataView dvData = new DataView(ds.Tables["bookingClassList"]);
                    dvData.Sort = "cabin";
                    DataTable bookingClassList = dvData.ToTable(true, "availFlightSegmentList_id", "cabin", "resBookDesigCode", "resBookDesigQuantity");

                    DataView dvflightSegment = new DataView(ds.Tables["flightSegment"]);
                    dvflightSegment.Sort = "flightSegment_Id";
                    DataTable dtflightSegment = dvflightSegment.ToTable(true, "availFlightSegmentList_id", "SegmentGroupID", "flightSegment_Id", "departureDateTime", "departureDateTimeUTC", "arrivalDateTime",
                        "arrivalDateTimeUTC", "flightNumber", "ondControlled", "sector", "codeshare", "distance", "flownMileageQty", "journeyDuration",
                        "onTimeRate", "secureFlightDataRequired", "stopQuantity");


                    //DataColumn SegmentGroupID = new DataColumn("SegmentGroupID");
                    //SegmentGroupID.DataType = System.Type.GetType("System.Int32");
                    DataColumn Flight = new DataColumn("Flight");
                    Flight.DataType = System.Type.GetType("System.String");

                    //int segID = 0;

                    //dtflightSegment.Columns.Add(SegmentGroupID);
                    //SegmentGroupID.SetOrdinal(0);

                    dtflightSegment.Columns.Add(Flight);

                    //foreach (DataRow dr in dtflightSegment.Rows)
                    //{
                    //    if (Convert.ToDateTime(dr["departureDateTime"]).ToShortDateString() == Convert.ToDateTime(Dept).ToShortDateString())
                    //    {
                    //        dr["SegmentGroupID"] = segID;
                    //       // dr["Flight"] = "M";
                    //    }
                    //    else if (Convert.ToDateTime(dr["departureDateTime"]).ToShortDateString() == Convert.ToDateTime(Arr).ToShortDateString())
                    //    {
                    //        dr["SegmentGroupID"] = segID;
                    //        segID = segID + 1;
                    //        //dr["Flight"] = "R";
                    //    }
                    //    else
                    //    {
                    //        dr["SegmentGroupID"] = segID;
                    //       // dr["Flight"] = "C";
                    //    }

                    //}


                    var CityInfo = (from p in ds.Tables["cityInfo"].AsEnumerable()
                                    join t in ds.Tables["city"].AsEnumerable()
                                    on p.Field<int?>("cityInfo_Id") equals t.Field<int?>("cityInfo_Id")
                                    join t1 in ds.Tables["country"].AsEnumerable()
                                   on p.Field<int?>("cityInfo_Id") equals t1.Field<int?>("cityInfo_Id")
                                    select new
                                    {
                                        cityInfo_Id = p.Field<int?>("cityInfo_Id"),
                                        arrivalAirport_Id = p.Field<int?>("arrivalAirport_Id") == null ? 0 : p.Field<int?>("arrivalAirport_Id"),
                                        departureAirport_Id = p.Field<int?>("departureAirport_Id"),
                                        locationCode = t.Field<string>("locationCode"),
                                        locationName = t.Field<string>("locationName"),
                                        locationNameLanguage = t.Field<string>("locationNameLanguage"),
                                        country_locationCode = t1.Field<string>("locationCode"),
                                        country_locationName = t1.Field<string>("locationName"),
                                        countrylocationNameLanguage = t1.Field<string>("locationNameLanguage"),
                                        country_Id = t1.Field<int?>("country_Id"),
                                    }).ToList();

                    DataTable dtCityInfo = LINQResultToDataTable(CityInfo);

                    #region Flights

                    var JoinResult = (from p in ds.Tables["Availability"].AsEnumerable()
                                      join t in ds.Tables["availabilityResultList"].AsEnumerable()
                                      on p.Field<int?>("Availability_Id") equals t.Field<int?>("Availability_Id")
                                      join t1 in ds.Tables["availabilityRouteList"].AsEnumerable()
                                      on t.Field<int?>("availabilityResultList_Id") equals t1.Field<int?>("availabilityResultList_Id")
                                      join t2 in ds.Tables["availabilityByDateList"].AsEnumerable()
                                     on t1.Field<int?>("availabilityRouteList_Id") equals t2.Field<int?>("availabilityRouteList_Id")
                                      join t3 in ds.Tables["originDestinationOptionList"].AsEnumerable()
                                      on t2.Field<int?>("availabilityByDateList_id") equals t3.Field<int?>("availabilityByDateList_id")
                                      join t4 in ds.Tables["fareComponentGroupList"].AsEnumerable()
                                      on t3.Field<int?>("originDestinationOptionList_Id") equals t4.Field<int?>("originDestinationOptionList_Id")
                                      join t5 in ds.Tables["boundList"].AsEnumerable()
                                      on t4.Field<int?>("fareComponentGroupList_Id") equals t5.Field<int?>("fareComponentGroupList_Id")
                                      join t6 in ds.Tables["availFlightSegmentList"].AsEnumerable()
                                      on t5.Field<int?>("boundList_Id") equals t6.Field<int?>("boundList_Id")
                                      join t7 in bookingClassList.AsEnumerable()
                                      on t6.Field<int?>("availFlightSegmentList_Id") equals t7.Field<int?>("availFlightSegmentList_Id")
                                      join t8 in dtflightSegment.AsEnumerable()
                                      on t7.Field<int?>("availFlightSegmentList_Id") equals t8.Field<int?>("availFlightSegmentList_Id")
                                      join t9 in ds.Tables["airline"].AsEnumerable()
                                      on t8.Field<int?>("flightSegment_Id") equals t9.Field<int?>("flightSegment_Id")
                                      join t10 in ds.Tables["arrivalAirport"].AsEnumerable()
                                      on t9.Field<int?>("flightSegment_Id") equals t10.Field<int?>("flightSegment_Id")
                                      join t11 in ds.Tables["departureAirport"].AsEnumerable()
                                      on t10.Field<int?>("flightSegment_Id") equals t11.Field<int?>("flightSegment_Id")
                                      join t12 in ds.Tables["equipment"].AsEnumerable()
                                      on t8.Field<int?>("flightSegment_Id") equals t12.Field<int?>("flightSegment_Id")
                                      //join t13 in ds.Tables["datelist"].AsEnumerable()
                                      //on t2.Field<int?>("availabilityByDateList_id") equals t13.Field<int?>("availabilityByDateList_id")
                                      select new
                                      {
                                          Availability_Id = p.Field<int?>("Availability_Id"),
                                          availabilityResultList_Id = t.Field<int?>("availabilityResultList_Id"),
                                          availabilityRouteList_Id = t1.Field<int?>("availabilityRouteList_Id"),
                                          flexibleFaresOnly = t1.Field<string>("flexibleFaresOnly"),
                                          //dateList = t13.Field<string>("dateList_text"),
                                          //availabilityByDateList_Id = t2.Field<int?>("availabilityByDateList_Id"),
                                          originDestinationOptionList_Id = t3.Field<int?>("originDestinationOptionList_Id"),
                                          fareComponentGroupList_Id = t4.Field<int?>("fareComponentGroupList_Id"),
                                          boundList_Id = t5.Field<int?>("boundList_Id"),
                                          boundCode = t5.Field<string>("boundCode"),
                                          availFlightSegmentList_Id = t6.Field<int?>("availFlightSegmentList_Id"),
                                          addOnSegment = t6.Field<string>("addOnSegment"),
                                          cabin = t7.Field<string>("cabin"),
                                          resBookDesigCode = t7.Field<string>("resBookDesigCode"),
                                          resBookDesigQuantity = t7.Field<string>("resBookDesigQuantity"),
                                          SegmentGroupID = t8.Field<int?>("SegmentGroupID"),
                                          Flight = t8.Field<string>("Flight"),
                                          flightSegment_Id = t8.Field<int?>("flightSegment_Id"),
                                          departureDateTime = t8.Field<string>("departureDateTime"),
                                          departureDateTimeUTC = t8.Field<string>("departureDateTimeUTC"),
                                          arrivalDateTime = t8.Field<string>("arrivalDateTime"),
                                          arrivalDateTimeUTC = t8.Field<string>("arrivalDateTimeUTC"),
                                          flightNumber = t8.Field<string>("flightNumber"),
                                          ondControlled = t8.Field<string>("ondControlled"),
                                          sector = t8.Field<string>("sector"),
                                          codeshare = t8.Field<string>("codeshare"),
                                          distance = t8.Field<string>("distance"),
                                          flownMileageQty = t8.Field<string>("flownMileageQty"),
                                          journeyDuration = t8.Field<string>("journeyDuration"),
                                          onTimeRate = t8.Field<string>("onTimeRate"),
                                          secureFlightDataRequired = t8.Field<string>("secureFlightDataRequired"),
                                          stopQuantity = t8.Field<string>("stopQuantity"),
                                          code = t9.Field<string>("code"),
                                          companyFullName = t9.Field<string>("companyFullName"),
                                          companyShortName = t9.Field<string>("companyShortName"),
                                          departureAirport_Id = t11.Field<int?>("departureAirport_Id"),
                                          dep_codeContext = t11.Field<string>("codeContext"),
                                          dep_language = t11.Field<string>("language"),
                                          dep_locationCode = t11.Field<string>("locationCode"),
                                          dep_locationName = t11.Field<string>("locationName"),
                                          dep_terminal = t11.Field<string>("terminal"),
                                          arrivalAirport_Id = t10.Field<int?>("arrivalAirport_Id"),
                                          codeContext = t10.Field<string>("codeContext"),
                                          language = t10.Field<string>("language"),
                                          locationCode = t10.Field<string>("locationCode"),
                                          locationName = t10.Field<string>("locationName"),
                                          terminal = t10.Field<string>("terminal"),
                                          airEquipType = t12.Field<string>("airEquipType"),
                                          airEquipTypeModel = t12.Field<string>("airEquipTypeModel"),
                                          changeofGauge = t12.Field<string>("changeofGauge"),
                                          //deiCode = t13.Field<string>("deiCode"),
                                          //explanation = t13.Field<string>("explanation"),
                                          //note = t13.Field<string>("note"),


                                      }).ToList();

                    DataTable flights = LINQResultToDataTable(JoinResult);

                    #endregion

                    #region farelist
                    var FareList = (from p in ds.Tables["fareComponentList"].AsEnumerable()
                                    join t in ds.Tables["passengerFareInfoList"].AsEnumerable()
                                    on p.Field<int?>("fareComponentList_Id") equals t.Field<int?>("fareComponentList_Id")
                                    join t1 in ds.Tables["fareInfoList"].AsEnumerable()
                                    on t.Field<int?>("passengerFareInfoList_Id") equals t1.Field<int?>("passengerFareInfoList_Id")
                                    join t2 in ds.Tables["fareBaggageAllowance"].AsEnumerable()
                                    on t1.Field<int?>("fareInfoList_Id") equals t2.Field<int?>("fareInfoList_Id")
                                    join t4 in ds.Tables["passengerTypeQuantity"].AsEnumerable()
                                    on t.Field<int?>("passengerFareInfoList_Id") equals t4.Field<int?>("passengerFareInfoList_Id")
                                    join t5 in ds.Tables["passengerType"].AsEnumerable()
                                    on t4.Field<int?>("passengerTypeQuantity_Id") equals t5.Field<int?>("passengerTypeQuantity_Id")
                                    join t3 in ds.Tables["maxAllowedWeight"].AsEnumerable()
                                    on t2.Field<int?>("fareBaggageAllowance_Id") equals t3.Field<int?>("fareBaggageAllowance_Id")
                                    join t6 in ds.Tables["pricingInfo"].AsEnumerable()
                                    on t.Field<int?>("passengerFareInfoList_Id") equals t6.Field<int?>("passengerFareInfoList_Id")
                                    join t7 in ds.Tables["baseFare"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t7.Field<int?>("pricingInfo_Id")
                                    join t8 in ds.Tables["amount"].AsEnumerable()
                                    on t7.Field<int?>("baseFare_Id") equals t8.Field<int?>("baseFare_Id")
                                    join t9 in ds.Tables["equivBaseFare"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t9.Field<int?>("pricingInfo_Id")
                                    join t10 in ds.Tables["surcharges"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t10.Field<int?>("pricingInfo_Id")
                                    join t11 in ds.Tables["pricingOverview"].AsEnumerable()
                                    on p.Field<int?>("fareComponentList_Id") equals t11.Field<int?>("fareComponentList_Id")
                                    join t12 in ds.Tables["totalAmount"].AsEnumerable()
                                     on t11.Field<int?>("pricingOverview_Id") equals t12.Field<int?>("pricingOverview_Id")
                                    join t13 in ds.Tables["taxes"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t13.Field<int?>("pricingInfo_Id")
                                    join t14 in ds.Tables["taxList"].AsEnumerable()
                                    on t13.Field<int?>("taxes_Id") equals t14.Field<int?>("taxes_Id")
                                    join t15 in ds.Tables["taxAmount"].AsEnumerable()
                                    on t14.Field<int?>("taxList_Id") equals t15.Field<int?>("taxList_Id")
                                    join t16 in ds.Tables["totalFare"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t16.Field<int?>("pricingInfo_Id")
                                    join t17 in ds.Tables["totalBaseFare"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t17.Field<int?>("pricingOverview_Id")
                                    join t18 in ds.Tables["totalSurcharge"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t18.Field<int?>("pricingOverview_Id")
                                    join t19 in ds.Tables["totalTax"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t19.Field<int?>("pricingOverview_Id")
                                    select new
                                    {
                                        fareComponentGroupList_Id = p.Field<int?>("fareComponentGroupList_Id"),
                                        fareComponentList_Id = p.Field<int?>("fareComponentList_Id"),
                                        fareKind = p.Field<string>("fareKind"),
                                        internalID = p.Field<string>("internalID"),
                                        maxUsageDate = p.Field<string>("maxUsageDate"),
                                        passengerFareInfoList_Id = t1.Field<int?>("passengerFareInfoList_Id"),
                                        //    endorsementList = t.Field<string>("endorsementList"),
                                        fareInfoList_Id = t1.Field<int?>("fareInfoList_Id"),
                                        cabinClassCode = t1.Field<string>("cabinClassCode"),
                                        fareGroupName = t1.Field<string>("fareGroupName"),
                                        fareReferenceCode = t1.Field<string>("fareReferenceCode"),
                                        fareReferenceID = t1.Field<string>("fareReferenceID"),
                                        fareReferenceName = t1.Field<string>("fareReferenceName"),
                                        flightSegmentSequence = t1.Field<string>("flightSegmentSequence"),
                                        notValidAfter = t1.Field<string>("notValidAfter"),
                                        notValidBefore = t1.Field<string>("notValidBefore"),
                                        resBookDesigCode = t1.Field<string>("resBookDesigCode"),
                                        fareBaggageAllowance_Id = t2.Field<int?>("fareBaggageAllowance_Id"),
                                        allowanceType = t2.Field<string>("allowanceType"),
                                        maxAllowedPieces = t2.Field<string>("maxAllowedPieces"),
                                        unitOfMeasureCode = t3.Field<string>("unitOfMeasureCode"),
                                        weight = t3.Field<string>("weight"),
                                        passengerTypeQuantity_Id = t4.Field<int?>("passengerTypeQuantity_Id"),
                                        hasStrecher = t4.Field<string>("hasStrecher"),
                                        quantity = t4.Field<string>("quantity"),
                                        passengerType_code = t5.Field<string>("code"),
                                        pricingInfo_Id = t6.Field<int?>("pricingInfo_Id"),
                                        fareBaggageAllowance = t6.Field<string>("fareBaggageAllowance"),
                                        fareConstruction = t6.Field<string>("fareConstruction"),
                                        discountApplied = t6.Field<string>("discountApplied"),
                                        passengerTypeCode = t6.Field<string>("passengerTypeCode"),
                                        baseFare_id = t7.Field<int?>("baseFare_id"),
                                        amount_Id = t8.Field<int?>("amount_Id"),
                                        amount_totalFare_Id = t8.Field<int?>("totalFare_Id"),
                                        accountingSign = t8.Field<string>("accountingSign"),
                                        mileAmount = t8.Field<string>("mileAmount"),
                                        value = t8.Field<string>("value"),
                                        equivBaseFare_Id = t9.Field<int?>("equivBaseFare_Id"),
                                        equivBaseFare_accountingSign = t9.Field<string>("accountingSign"),
                                        equivBaseFare_mileAmount = t9.Field<string>("mileAmount"),
                                        equivBaseFare_value = t9.Field<string>("value"),
                                        surcharges_Id = t10.Field<int?>("surcharges_Id"),
                                        pricingOverview_Id = t11.Field<int?>("pricingOverview_Id"),
                                        totalAmount_accountingSign = t12.Field<string>("accountingSign"),
                                        totalAmount_totalAmount_Id = t12.Field<int?>("totalAmount_Id"),
                                        totalAmount_mileAmount = t12.Field<string>("mileAmount"),
                                        totalAmount_value = t12.Field<string>("value"),
                                        totalAmount_surcharges_Id = t12.Field<int?>("surcharges_Id"),
                                        totalAmount_taxes_Id = t12.Field<int?>("taxes_Id"),
                                        taxes_taxes_Id = t13.Field<int?>("taxes_Id"),
                                        taxList_Id = t14.Field<int?>("taxList_Id"),
                                        paid = t14.Field<string>("paid"),
                                        taxCode = t14.Field<string>("taxCode"),
                                        taxAmount_taxAmount_Id = t15.Field<int?>("taxAmount_Id"),
                                        taxAmount_accountingSign = t15.Field<string>("accountingSign"),
                                        taxAmount_mileAmount = t15.Field<string>("mileAmount"),
                                        taxAmount_value = t15.Field<string>("value"),
                                        totalFare_Id = t16.Field<int?>("totalFare_Id"),
                                        totalBaseFare_Id = t17.Field<int?>("totalBaseFare_Id"),
                                        totalBaseFare_accountingSign = t17.Field<string>("accountingSign"),
                                        totalBaseFare_mileAmount = t17.Field<string>("mileAmount"),
                                        totalBaseFare_value = t17.Field<string>("value"),
                                        totalSurcharge_Id = t18.Field<int?>("totalSurcharge_Id"),
                                        totalSurcharge_accountingSign = t18.Field<string>("accountingSign"),
                                        totalSurcharge_mileAmount = t18.Field<string>("mileAmount"),
                                        totalSurcharge_value = t18.Field<string>("value"),
                                        totalTax_Id = t19.Field<int?>("totalTax_Id"),
                                        totalTax_accountingSign = t19.Field<string>("accountingSign"),
                                        totalTax_mileAmount = t19.Field<string>("mileAmount"),
                                        totalTax_value = t19.Field<string>("value"),
                                    }).ToList();

                    DataTable dtFareList = LINQResultToDataTable(FareList);

                    #endregion

                    var FareInfoList = (from p in ds.Tables["fareComponentList"].AsEnumerable()
                                        join t in ds.Tables["passengerFareInfoList"].AsEnumerable()
                                        on p.Field<int?>("fareComponentList_Id") equals t.Field<int?>("fareComponentList_Id")
                                        join t1 in ds.Tables["fareInfoList"].AsEnumerable()
                                        on t.Field<int?>("passengerFareInfoList_Id") equals t1.Field<int?>("passengerFareInfoList_Id")
                                        select new
                                        {
                                            fareComponentGroupList_Id = p.Field<int?>("fareComponentGroupList_Id"),
                                            fareComponentList_Id = p.Field<int?>("fareComponentList_Id"),
                                            fareKind = p.Field<string>("fareKind"),
                                            internalID = p.Field<string>("internalID"),
                                            maxUsageDate = p.Field<string>("maxUsageDate"),
                                            passengerFareInfoList_Id = t1.Field<int?>("passengerFareInfoList_Id"),
                                            //  endorsementList = t.Field<string>("endorsementList"),
                                            fareInfoList_Id = t1.Field<int?>("fareInfoList_Id"),
                                            cabinClassCode = t1.Field<string>("cabinClassCode"),
                                            fareGroupName = t1.Field<string>("fareGroupName"),
                                            fareReferenceCode = t1.Field<string>("fareReferenceCode"),
                                            fareReferenceID = t1.Field<string>("fareReferenceID"),
                                            fareReferenceName = t1.Field<string>("fareReferenceName"),
                                            flightSegmentSequence = t1.Field<string>("flightSegmentSequence"),
                                            notValidAfter = t1.Field<string>("notValidAfter"),
                                            notValidBefore = t1.Field<string>("notValidBefore"),
                                            resBookDesigCode = t1.Field<string>("resBookDesigCode")
                                        }).ToList();

                    DataTable dtFareInfoList = LINQResultToDataTable(FareInfoList);


                    dtFareInfoList.Columns.Add("pricingInfo_Id");
                    dtFareInfoList.Columns.Add("pricingOverview_id");
                    dtFareInfoList.Columns.Add("AdultBasePrice");
                    dtFareInfoList.Columns.Add("AdultTaxes");
                    //flights.Columns.Add("fareComponentList_Id");
                    //flights.Columns.Add("fareReferenceName");
                    //flights.Columns.Add("fareReferenceCode");

                    foreach (DataRow dro in ds.Tables["pricingOverview"].Rows)
                    {
                        foreach (DataRow dro1 in dtFareInfoList.Rows)
                        {
                            if (dro["fareComponentList_Id"].ToString() == dro1["fareComponentList_Id"].ToString())
                            {
                                dro1["pricingOverview_id"] = dro["pricingOverview_id"];
                            }
                        }
                    }

                    foreach (DataRow dro in ds.Tables["pricingInfo"].Rows)
                    {
                        foreach (DataRow dro1 in dtFareInfoList.Rows)
                        {
                            if (dro["passengerFareInfoList_Id"].ToString() == dro1["passengerFareInfoList_Id"].ToString())
                            {
                                dro1["pricingInfo_Id"] = dro["pricingInfo_Id"];
                            }
                        }
                    }

                    foreach (DataRow dro in ds.Tables["totaltax"].Rows)
                    {
                        foreach (DataRow dro1 in dtFareInfoList.Rows)
                        {
                            if (dro["pricingOverview_id"].ToString() == dro1["pricingOverview_id"].ToString())
                            {
                                dro1["AdultTaxes"] = dro["value"];
                            }
                        }
                    }

                    foreach (DataRow dro in ds.Tables["totalamount"].Rows)
                    {
                        foreach (DataRow dro1 in dtFareInfoList.Rows)
                        {
                            if (dro["pricingOverview_id"].ToString() == dro1["pricingOverview_id"].ToString())
                            {
                                dro1["AdultBasePrice"] = dro["value"];
                            }
                        }
                    }

                    DataColumn column = new DataColumn("AdultTotalPrice");
                    column.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column1 = new DataColumn("AdultBasePrice");
                    column1.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column2 = new DataColumn("AdultTaxes");
                    column2.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column3 = new DataColumn("ChildTotalPrice");
                    column3.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column4 = new DataColumn("ChildBasePrice");
                    column4.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column5 = new DataColumn("InfantTotalPrice");
                    column5.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column6 = new DataColumn("InfantBasePrice");
                    column6.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column7 = new DataColumn("InfantTaxes");
                    column7.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column8 = new DataColumn("ChildTaxes");
                    column8.DataType = System.Type.GetType("System.Decimal");

                    DataColumn fareReferenceCode = new DataColumn("fareReferenceCode");
                    fareReferenceCode.DataType = System.Type.GetType("System.String");

                    DataColumn fareReferenceName = new DataColumn("fareReferenceName");
                    fareReferenceName.DataType = System.Type.GetType("System.String");

                    DataColumn totalAmount_value = new DataColumn("totalAmount_value");
                    totalAmount_value.DataType = System.Type.GetType("System.Double");
                    DataColumn GrandTotal = new DataColumn("GrandTotal");
                    GrandTotal.DataType = System.Type.GetType("System.Decimal");


                    DataColumn fareComponentList_Id = new DataColumn("fareComponentList_Id");
                    fareComponentList_Id.DataType = System.Type.GetType("System.String");

                    //DataColumn column15 = new DataColumn("locationName");
                    //column15.DataType = System.Type.GetType("System.String");

                    //DataColumn column16 = new DataColumn("locationNameLanguage");
                    //column16.DataType = System.Type.GetType("System.String");

                    //DataColumn column17 = new DataColumn("country_locationCode");
                    //column17.DataType = System.Type.GetType("System.String");

                    //DataColumn column18 = new DataColumn("country_locationName");
                    //column18.DataType = System.Type.GetType("System.String");

                    //DataColumn column19 = new DataColumn("countrylocationNameLanguage");
                    //column19.DataType = System.Type.GetType("System.String");

                    //DataColumn column20 = new DataColumn("country_Id");
                    //column20.DataType = System.Type.GetType("System.Int");

                    flights.Columns.Add(totalAmount_value);
                    flights.Columns.Add(column);
                    flights.Columns.Add(column1);
                    flights.Columns.Add(column2);
                    flights.Columns.Add(column3);
                    flights.Columns.Add(column4);
                    flights.Columns.Add(column5);
                    flights.Columns.Add(column6);
                    flights.Columns.Add(column7);
                    flights.Columns.Add(column8);
                    flights.Columns.Add(GrandTotal);
                    flights.Columns.Add("allowanceType");
                    flights.Columns.Add("maxAllowedPieces");
                    flights.Columns.Add("unitOfMeasureCode");
                    flights.Columns.Add("weight");
                    flights.Columns.Add("fareComponentList_Id");
                    flights.Columns.Add("fareReferenceName");
                    flights.Columns.Add("fareReferenceCode");
                    flights.Columns.Add("fareReferenceID");

                    flights.Columns["SegmentGroupID"].SetOrdinal(1);
                    flights.Columns["fareComponentGroupList_Id"].SetOrdinal(2);
                    flights.Columns["fareComponentList_Id"].SetOrdinal(3);

                    //flights.Columns.Add(dep_locationCode);
                    flights.Columns.Add("arr_locationCode");

                    foreach (DataRow ro in flights.Rows)
                    {
                        foreach (DataRow ro1 in dtFareInfoList.Rows)
                        {
                            if (Convert.ToString(ro["fareComponentGroupList_Id"]) == Convert.ToString(ro1["fareComponentGroupList_Id"]) &&
                                Convert.ToString(ro["resBookDesigCode"]) == Convert.ToString(ro1["resBookDesigCode"]))
                            {
                                ro["fareComponentList_Id"] = ro1["fareComponentList_Id"];
                                ro["fareReferenceName"] = ro1["fareReferenceName"];
                                ro["fareReferenceCode"] = ro1["fareReferenceCode"];
                                ro["fareReferenceID"] = ro1["fareReferenceID"];
                            }
                        }
                    }

                    foreach (DataRow ro1 in dtFareInfoList.Rows)
                    {
                        foreach (DataRow ro in flights.Rows)
                        {
                            if (Convert.ToString(ro["fareComponentGroupList_Id"]) == Convert.ToString(ro1["fareComponentGroupList_Id"]) &&
                            Convert.ToString(ro["fareComponentList_Id"]) == Convert.ToString(ro1["fareComponentList_Id"]))
                            {

                                if (ro["AdultBasePrice"] == DBNull.Value)
                                {
                                    ro["AdultTotalPrice"] = Convert.ToDecimal(ro1["AdultBasePrice"]) + Convert.ToDecimal(ro1["AdultTaxes"]);
                                    ro["AdultBasePrice"] = ro1["AdultBasePrice"];
                                    ro["AdultTaxes"] = ro1["AdultTaxes"];
                                    ro["ChildTotalPrice"] = 0;
                                    ro["ChildBasePrice"] = 0;
                                    ro["ChildTaxes"] = 0;
                                    ro["InfantTotalPrice"] = 0;
                                    ro["InfantBasePrice"] = 0;
                                    ro["InfantTaxes"] = 0;
                                }
                                //else if (ro1["passengerType_code"].ToString() == "CHLD" && ro["ChildBasePrice"] == DBNull.Value)
                                //{
                                //    ro["totalAmount_value"] = ro1["totalAmount_value"];
                                //    ro["ChildTotalPrice"] = ro1["totalAmount_value"];
                                //    ro["ChildBasePrice"] = ro1["equivBaseFare_value"];
                                //    ro["ChildTaxes"] = ro1["totalTax_value"];
                                //    ro["InfantTotalPrice"] = 0;
                                //    ro["InfantBasePrice"] = 0;
                                //    ro["InfantTaxes"] = 0;
                                //}
                                //else if (ro1["passengerType_code"].ToString() == "INFT" && ro["InfantBasePrice"] == DBNull.Value)
                                //{
                                //    ro["totalAmount_value"] = ro1["totalAmount_value"];
                                //    ro["InfantTotalPrice"] = ro1["totalAmount_value"];
                                //    ro["InfantBasePrice"] = ro1["equivBaseFare_value"];
                                //    ro["InfantTaxes"] = ro1["totalTax_value"];
                                //}
                            }
                        }
                    }




                    foreach (DataRow ro in flights.Rows)
                    {
                        foreach (DataRow ro1 in dtFareList.Rows)
                        {
                            if (Convert.ToString(ro["fareComponentGroupList_Id"]) == Convert.ToString(ro1["fareComponentGroupList_Id"]) &&
                                Convert.ToString(ro["resBookDesigCode"]) == Convert.ToString(ro1["resBookDesigCode"]))
                            {
                                ro["allowanceType"] = ro1["allowanceType"];
                                ro["maxAllowedPieces"] = ro1["maxAllowedPieces"];
                                ro["unitOfMeasureCode"] = ro1["unitOfMeasureCode"];
                                ro["weight"] = ro1["weight"];

                                //if (ro1["passengerType_code"].ToString() == "ADLT" && ro["AdultBasePrice"] == DBNull.Value)
                                //{
                                //    ro["totalAmount_value"] = ro1["totalAmount_value"];
                                //    ro["AdultTotalPrice"] = ro1["totalAmount_value"];
                                //    ro["AdultBasePrice"] = ro1["equivBaseFare_value"];
                                //    ro["AdultTaxes"] = ro1["totalTax_value"];
                                //    ro["ChildTotalPrice"] = 0;
                                //    ro["ChildBasePrice"] = 0;
                                //    ro["ChildTaxes"] = 0;
                                //    ro["InfantTotalPrice"] = 0;
                                //    ro["InfantBasePrice"] = 0;
                                //    ro["InfantTaxes"] = 0;
                                //}
                                //else if (ro1["passengerType_code"].ToString() == "CHLD" && ro["ChildBasePrice"] == DBNull.Value)
                                //{
                                //    ro["totalAmount_value"] = ro1["totalAmount_value"];
                                //    ro["ChildTotalPrice"] = ro1["totalAmount_value"];
                                //    ro["ChildBasePrice"] = ro1["equivBaseFare_value"];
                                //    ro["ChildTaxes"] = ro1["totalTax_value"];
                                //    ro["InfantTotalPrice"] = 0;
                                //    ro["InfantBasePrice"] = 0;
                                //    ro["InfantTaxes"] = 0;
                                //}
                                //else if (ro1["passengerType_code"].ToString() == "INFT" && ro["InfantBasePrice"] == DBNull.Value)
                                //{
                                //    ro["totalAmount_value"] = ro1["totalAmount_value"];
                                //    ro["InfantTotalPrice"] = ro1["totalAmount_value"];
                                //    ro["InfantBasePrice"] = ro1["equivBaseFare_value"];
                                //    ro["InfantTaxes"] = ro1["totalTax_value"];
                                //}
                            }
                        }
                    }


                    foreach (DataRow ro in flights.Rows)
                    {
                        foreach (DataRow ro1 in ds.Tables["departureAirport"].Rows)
                        {
                            if (Convert.ToString(ro["flightSegment_Id"]) == Convert.ToString(ro1["flightSegment_Id"]))
                            {
                                ro["dep_locationCode"] = ro1["locationCode"];
                            }
                        }
                    }

                    foreach (DataRow ro in flights.Rows)
                    {
                        foreach (DataRow ro1 in ds.Tables["arrivalAirport"].Rows)
                        {
                            if (Convert.ToString(ro["flightSegment_Id"]) == Convert.ToString(ro1["flightSegment_Id"]))
                            {
                                ro["arr_locationCode"] = ro1["locationCode"];
                            }
                        }
                    }

                    DataColumn column0 = new DataColumn("SegmentID");
                    column0.DataType = System.Type.GetType("System.Int32");


                    int optionID = 0;
                    flights.Columns.Add("Duration");
                    flights.Columns.Add("TotalDuration");
                    flights.Columns.Add("Option_Id");
                    flights.Columns["Option_Id"].SetOrdinal(0);
                    flights.Columns.Add("Direct");
                    //flights.Columns.Add("Flight");



                    foreach (DataRow dr in flights.Rows)
                    {
                        string dur = dr["journeyDuration"].ToString();
                        string[] d;
                        int h = 0;
                        int m = 0;

                        if (dur.IndexOf("PT") > -1)
                        {
                            dur = dur.Replace("PT", "");
                        }

                        if (dur.IndexOf('H') > -1 && dur.IndexOf('M') > -1)
                        {
                            if (dur.IndexOf('M') > -1)
                            {
                                dur = dur.Replace("M", "");
                            }

                            dur = dur.Replace("H", ":");
                            d = dur.Split(":");
                            h = Convert.ToInt32(d[0]);
                            m = Convert.ToInt32(d[1]);
                            h = h * 60;
                            h = h + m;
                        }
                        else
                        {
                            dur = dur.Replace("H", "");
                            h = Convert.ToInt32(dur);
                            h = h * 60;
                        }

                        dr["Duration"] = h;
                        dr["TotalDuration"] = h;
                    }

                    foreach (DataRow dr in flights.Rows)
                    {
                        if (dr["dep_locationCode"].ToString() == from && dr["arr_locationCode"].ToString() == to)
                        {
                            dr["Direct"] = 1;
                            dr["Flight"] = "DM"; //Main Flight
                        }
                        else if (dr["dep_locationCode"].ToString() == to && dr["arr_locationCode"].ToString() == from)
                        {
                            dr["Direct"] = 1;
                            dr["Flight"] = "RM"; //Main Flight
                        }
                        else if (dr["dep_locationCode"].ToString() == from && dr["dep_locationCode"].ToString() != to && dr["arr_locationCode"].ToString() != to)
                        {
                            dr["Direct"] = 0;
                            dr["Flight"] = "DM"; //Connecting Flight
                        }
                        else if (dr["dep_locationCode"].ToString() != from && dr["dep_locationCode"].ToString() != to && dr["arr_locationCode"].ToString() == to)
                        {
                            dr["Direct"] = 0;
                            dr["Flight"] = "DC"; //Connecting Flight
                        }
                        else if (dr["dep_locationCode"].ToString() == to && dr["dep_locationCode"].ToString() != from && dr["arr_locationCode"].ToString() != from)
                        {
                            dr["Direct"] = 0;
                            dr["Flight"] = "RM";
                        }
                        else if (dr["dep_locationCode"].ToString() != to && dr["dep_locationCode"].ToString() != from && dr["arr_locationCode"].ToString() == from)
                        {
                            dr["Direct"] = 0;
                            dr["Flight"] = "RC"; //Connecting Flight
                        }
                        else
                        {
                            dr["Direct"] = 0;
                            dr["Flight"] = "DRC";
                        }
                    }

                    foreach (DataRow ro in flights.Rows)
                    {

                        ro["GrandTotal"] = Convert.ToDecimal(ro["AdultTotalPrice"]) + Convert.ToDecimal(ro["ChildTotalPrice"]) +
                                        Convert.ToDecimal(ro["InfantTotalPrice"]) + 1000;
                    }

                    //foreach (DataRow dr in flights.Rows)
                    //{
                    //    if (dr["dep_locationCode"].ToString() != from)
                    //    {
                    //        dr["Flight"] = "C"; //Connecting Flight
                    //    }
                    //    else
                    //    {
                    //        dr["Flight"] = "M"; //Main Flight
                    //    }
                    //}

                    //&& dr["Flight"] = "M"

                    //foreach (DataRow dr in flights.Rows)
                    //{
                    //    dr["Option_Id"] = optionID;
                    //    optionID++;
                    //}

                    //string cabin = "";
                    //string fareComponentGroupList = "";
                    //string boundList_Id = "";
                    //string flightSegmentId = "";



                    //foreach (DataRow row in dataTable.Rows)
                    //{
                    //    Console.WriteLine($"Option ID: {row["option_id"]}, Unique Combination: {row["fareComponentGroupList_Id"]}\t{row["boundList_Id"]}\t{row["boundCode"]}\t{row["cabin"]}\t{row["Flight"]}\t{row["flightSegment_Id"]}");
                    //}


                    //foreach (DataRow dr in flights.Rows)
                    //{
                    //    flightSegmentId = dr["flightSegment_Id"].ToString();
                    //    fareComponentGroupList = dr["fareComponentGroupList_Id"].ToString();                        
                    //    boundList_Id = dr["boundList_Id"].ToString();
                    //    cabin = dr["Cabin"].ToString();


                    //    if (dr["Option_Id"] == null || dr["Option_Id"].ToString() == "")
                    //    {
                    //        foreach (DataRow dr1 in flights.Rows)
                    //        {
                    //          //  if (dr["flightSegment_Id"] == dr1["flightSegment_Id"])
                    //            {
                    //                if (dr1["Flight"].ToString() == "DM" && dr1["Cabin"].ToString() == cabin && dr1["flightSegment_Id"].ToString() == flightSegmentId && dr1["fareComponentGroupList_Id"].ToString() == fareComponentGroupList && dr1["boundList_Id"].ToString() == boundList_Id)
                    //                {
                    //                    dr1["Option_Id"] = optionID;

                    //                }
                    //                else if (dr1["Flight"].ToString() == "DC" && dr1["Cabin"].ToString() == cabin && dr1["flightSegment_Id"].ToString() == flightSegmentId && dr1["fareComponentGroupList_Id"].ToString() == fareComponentGroupList && dr1["boundList_Id"].ToString() == boundList_Id)
                    //                {
                    //                    dr1["Option_Id"] = optionID;

                    //                }
                    //                else
                    //                {
                    //                    optionID++;
                    //                }
                    //                }
                    //        }
                    //    }
                    //}

                    //foreach (DataRow dr in flights.Rows)
                    //{
                    //    flightSegmentId = dr["flightSegment_Id"].ToString();
                    //    fareComponentGroupList = dr["fareComponentGroupList_Id"].ToString();
                    //    boundList_Id = dr["boundList_Id"].ToString();
                    //    cabin = dr["Cabin"].ToString();
                    //   // optionID++;
                    //    if (dr["Option_Id"] == null || dr["Option_Id"].ToString() == "")
                    //    {
                    //        foreach (DataRow dr1 in flights.Rows)
                    //        {
                    //           // if (dr["flightSegment_Id"] != dr1["flightSegment_Id"])
                    //            {
                    //                if (dr1["Flight"].ToString() == "RM" && dr1["Cabin"].ToString() == cabin && dr1["flightSegment_Id"].ToString() == flightSegmentId && dr1["fareComponentGroupList_Id"].ToString() == fareComponentGroupList && dr1["boundList_Id"].ToString() == boundList_Id)
                    //                {
                    //                    dr1["Option_Id"] = optionID;

                    //                }
                    //                else if (dr1["Flight"].ToString() == "RC" && dr1["Cabin"].ToString() == cabin && dr1["flightSegment_Id"].ToString() == flightSegmentId && dr1["fareComponentGroupList_Id"].ToString() == fareComponentGroupList && dr1["boundList_Id"].ToString() == boundList_Id)
                    //                {
                    //                    dr1["Option_Id"] = optionID;

                    //                }
                    //                else
                    //                {
                    //                    optionID++;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    Dictionary<string, int> uniqueCombinations = new Dictionary<string, int>();
                    int optionIdCounter = 0;

                    foreach (DataRow row in flights.Rows)
                    {
                        string key = $"{row["fareComponentGroupList_Id"]}\t{row["boundList_Id"]}\t{row["boundCode"]}\t{row["cabin"]}\t{row["Flight"]}\t{row["flightSegment_Id"]}";

                        if (!uniqueCombinations.ContainsKey(key))
                        {
                            uniqueCombinations[key] = optionIdCounter++;
                        }

                        row["option_id"] = uniqueCombinations[key]; // Assign the option_id value to the row
                    }

                    flights.Columns.Add(column0);
                    column0.SetOrdinal(0);

                    for (int a = 0; a < flights.Rows.Count; a++)
                    {
                        flights.Rows[a]["SegmentID"] = a;
                    }

                    Guid TransactionId = Guid.NewGuid();

                    flights.Columns.Add("TransactionId");
                    foreach (DataRow dr in flights.Rows)
                    {
                        dr["TransactionId"] = TransactionId;
                    }

                    DataView dv = flights.DefaultView;
                    dv.Sort = "totalAmount_value desc";
                    flights = dv.ToTable();

                    MemoryStream stream = new MemoryStream();
                    ds.WriteXml(stream);
                    byte[] hititResponse = stream.ToArray();
                    stream = null;

                    MemoryStream streamFL = new MemoryStream();
                    dtFareList.TableName = "dtFareList";
                    dtFareList.WriteXml(streamFL);
                    byte[] fareList = streamFL.ToArray();
                    streamFL = null;

                    MemoryStream streamCI = new MemoryStream();
                    dtCityInfo.TableName = "dtCityInfo";
                    dtCityInfo.WriteXml(streamCI);
                    byte[] cityInfo = streamCI.ToArray();
                    streamCI = null;

                    //   ds.Tables["flightNotes"].WriteXml(stream);
                    //   byte[] flightNotes = stream.ToArray();

                    MemoryStream streamFligts = new MemoryStream();
                    flights.TableName = "flights";
                    flights.WriteXml(streamFligts);
                    byte[] finalResult = streamFligts.ToArray();
                    streamFligts = null;

                    HitItResult hr = new HitItResult();
                    hr.TransactionId = TransactionId;
                    hr.Origin = from;
                    hr.Destination = to;
                    hr.TotalPax = adults + children + infants;
                    hr.StartDate = Dept;
                    hr.HititResponse = hititResponse;
                    hr.FareList = fareList;
                    hr.CityInfo = cityInfo;
                    // hr.FlightNotes = flightNotes;
                    hr.FinalResult = finalResult;
                    hr.Active = true;
                    sql.HitItResults.Add(hr);
                    sql.SaveChanges();

                    var JSONString = DataTableToJSONWithStringBuilder(flights);
                    return JSONString;

                    //TempData["AirSegments"] = dtHitItFlights;
                    //if (Session["HitITAirSegments"] == null)
                    //{
                    //    Session["HitITAirSegments"] = flights;
                    //    Session["dtFareList"] = dtFareList;
                    //    Session["dtCityInfo"] = dtCityInfo;
                    //    Session["flightNotes"] = ds.Tables["flightNotes"];
                    //}
                    //TempData["Origin"] = from;
                    //TempData["Destination"] = to;
                    //TempData["HitItFType"] = tripType;

                    //TempData["HitItFlightsCounts"] = flights.Rows.Count; // ds.Tables["flightSegment"];
                    //                                                     // TempData["HitItFlightsOptions"] = flights.Rows.Count;
                    //TempData["adults"] = adults; TempData["children"] = children; TempData["infants"] = infants;
                    //TempData["deiCode"] = ds.Tables["flightNotes"].Rows[0]["deiCode"].ToString();
                    //TempData["explanation"] = ds.Tables["flightNotes"].Rows[0]["explanation"].ToString();
                    //TempData["note"] = ds.Tables["flightNotes"].Rows[0]["note"].ToString();
                }
                return "semothing went wrong";
            }
            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message;
                return "semething went wrong";
            }
        }

        [HttpGet("hititflightsmulti")]
        public string HitItFlights_Multi(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr,
         string fromM_1, string fromM_2, string fromM_3, string fromM_4, string fromM_5, string fromM_6,
         string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string end, string airline = "")
        {
            try
            {
                //Session["HitITAirSegments"] = null;
                //Session["dtFareList"] = null;
                //Session["dtCityInfo"] = null;

                //string url = "https://app.crane.aero/craneota/CraneOTAService";
                string url = "https://app-stage.crane.aero/craneota/CraneOTAService";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(HitItXMLReq(tripType, "ISB", "KHI", adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline));
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz"))); //Test env
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                request.ContentType = "text/xml; encoding='utf-8'";
                request.Method = "Post";
                request.Headers.Add("SOAPAction", "");
                request.Host = "app-stage.crane.aero";
                request.KeepAlive = true;

                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                ServicePointManager.Expect100Continue = false;
                // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream;
                TextReader responseStr;
                //DataTable FlightSegments = null;
                //DataTable dt;
                DataSet ds = new DataSet();

                //if (Session["AirSegments"] != null)
                //{
                //    Session["AirSegments"] = null;
                //}
                //  MalikExpressMVC.Models.Galileo.Envelope obj;
                // MalikExpressMVC.Models.Galileo.LowFareSearchRspObj rsp;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    responseStream = response.GetResponseStream();
                    responseStr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                    // var responseReader = new StreamReader(responseStream);
                    // var responsestr1 = new StreamReader(responseStream).ReadToEnd();

                    //Session["Depart"] = from;
                    //Session["Dest"] = to;

                    var responseReader = new StreamReader(responseStream);
                    var result1 = responseReader.ReadToEnd();

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(result1);
                    XmlReader xmlReader = new XmlNodeReader(xml);

                    //TextReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);

                    // XmlSerializer xmlSer = new XmlSerializer(typeof(MalikExpressMVC.Models.HitIt.Envelope));
                    // MalikExpressMVC.Models.HitIt.Envelope obj = (MalikExpressMVC.Models.HitIt.Envelope)xmlSer.Deserialize(respStream);
                    // MalikExpressMVC.Models.HitIt.GetAvailabilityResponse enObj = obj.Body.GetAvailabilityResponse;

                    //Session["HitItFlights"] = enObj;
                    //return;

                    //Log

                    //Log

                    string fileName = @"c:\HallMark\GetAvailability_Multi_Res.txt";
                    FileInfo fi = new FileInfo(fileName);

                    //////Check if file already exists. If yes, delete it.
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }

                    ////Create a new file
                    using (FileStream fs = fi.Create())
                    {
                        Byte[] txt = new UTF8Encoding(true).GetBytes(result1);
                        fs.Write(txt, 0, txt.Length);
                    }

                    //////Write file contents on console.
                    using (StreamReader sr = System.IO.File.OpenText(fileName))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(s);
                        }
                    }


                    // Commnent above 9 lines before publishing

                    // Commnent above 9 lines before publishing

                    ds.ReadXml(xmlReader);

                    StringBuilder sb = new StringBuilder();

                    string[] _sDate = start.Split(',');
                    _sDate = _sDate.Distinct().ToArray();
                    _sDate = _sDate.Skip(1).ToArray();

                    _sDate = _sDate.Where(val => val != "").ToArray();

                    string eDate = _sDate[_sDate.Length - 1];

                    string sDate = _sDate[0];

                    if (sDate.IndexOf("-") > -1)
                    {
                        _sDate = sDate.Split('-');
                        start = _sDate[2] + "/" + _sDate[1] + "/" + _sDate[0];
                    }
                    else
                    {
                        start = sDate;
                    }

                    DateTime dtStart = Convert.ToDateTime(start);
                    Dept = string.Format("{0:s}", dtStart);//"2016-04-01T11:00:00"

                    string[] _endDate;
                    //_endDate = eDate.Split(',');

                    if (eDate.IndexOf("-") > -1)
                    {
                        _endDate = eDate.Split('-');
                        end = _endDate[2] + "/" + _endDate[1] + "/" + _endDate[0];
                    }
                    //else
                    //{

                    //}

                    DateTime dtEnd = Convert.ToDateTime(end);

                    Arr = string.Format("{0:s}", dtEnd);//"2016-04-15T11:00:00"

                    DataView dvData = new DataView(ds.Tables["bookingClassList"]);
                    dvData.Sort = "cabin";
                    DataTable bookingClassList = dvData.ToTable(true, "availFlightSegmentList_id", "cabin", "resBookDesigCode", "resBookDesigQuantity");

                    DataView dvflightSegment = new DataView(ds.Tables["flightSegment"]);
                    dvflightSegment.Sort = "flightSegment_Id";
                    DataTable dtflightSegment = dvflightSegment.ToTable(true, "availFlightSegmentList_id", "flightSegment_Id", "departureDateTime", "departureDateTimeUTC", "arrivalDateTime",
                        "arrivalDateTimeUTC", "flightNumber", "ondControlled", "sector", "codeshare", "distance", "flownMileageQty", "journeyDuration",
                        "onTimeRate", "secureFlightDataRequired", "stopQuantity");


                    DataColumn SegmentGroupID = new DataColumn("SegmentGroupID");
                    SegmentGroupID.DataType = System.Type.GetType("System.Int32");
                    DataColumn Flight = new DataColumn("Flight");
                    Flight.DataType = System.Type.GetType("System.String");

                    int segID = 0;

                    dtflightSegment.Columns.Add(SegmentGroupID);
                    SegmentGroupID.SetOrdinal(0);

                    dtflightSegment.Columns.Add(Flight);

                    foreach (DataRow dr in dtflightSegment.Rows)
                    {
                        if (Convert.ToDateTime(dr["departureDateTime"]).ToShortDateString() == Convert.ToDateTime(Dept).ToShortDateString())
                        {
                            dr["SegmentGroupID"] = segID;
                            dr["Flight"] = "M";
                        }
                        else if (Convert.ToDateTime(dr["departureDateTime"]).ToShortDateString() == Convert.ToDateTime(Arr).ToShortDateString())
                        {
                            dr["SegmentGroupID"] = segID;
                            segID = segID + 1;
                            dr["Flight"] = "R";
                        }
                        else
                        {
                            dr["SegmentGroupID"] = segID;
                            dr["Flight"] = "C";
                        }

                    }


                    var CityInfo = (from p in ds.Tables["cityInfo"].AsEnumerable()
                                    join t in ds.Tables["city"].AsEnumerable()
                                    on p.Field<int?>("cityInfo_Id") equals t.Field<int?>("cityInfo_Id")
                                    join t1 in ds.Tables["country"].AsEnumerable()
                                   on p.Field<int?>("cityInfo_Id") equals t1.Field<int?>("cityInfo_Id")
                                    select new
                                    {
                                        cityInfo_Id = p.Field<int?>("cityInfo_Id"),
                                        arrivalAirport_Id = p.Field<int?>("arrivalAirport_Id") == null ? 0 : p.Field<int?>("arrivalAirport_Id"),
                                        departureAirport_Id = p.Field<int?>("departureAirport_Id"),
                                        locationCode = t.Field<string>("locationCode"),
                                        locationName = t.Field<string>("locationName"),
                                        locationNameLanguage = t.Field<string>("locationNameLanguage"),
                                        country_locationCode = t1.Field<string>("locationCode"),
                                        country_locationName = t1.Field<string>("locationName"),
                                        countrylocationNameLanguage = t1.Field<string>("locationNameLanguage"),
                                        country_Id = t1.Field<int?>("country_Id"),
                                    }).ToList();

                    DataTable dtCityInfo = LINQResultToDataTable(CityInfo);

                    #region Flights

                    var JoinResult = (from p in ds.Tables["Availability"].AsEnumerable()
                                      join t in ds.Tables["availabilityResultList"].AsEnumerable()
                                      on p.Field<int?>("Availability_Id") equals t.Field<int?>("Availability_Id")
                                      join t1 in ds.Tables["availabilityRouteList"].AsEnumerable()
                                      on t.Field<int?>("availabilityResultList_Id") equals t1.Field<int?>("availabilityResultList_Id")
                                      join t2 in ds.Tables["availabilityByDateList"].AsEnumerable()
                                     on t1.Field<int?>("availabilityRouteList_Id") equals t2.Field<int?>("availabilityRouteList_Id")
                                      join t3 in ds.Tables["originDestinationOptionList"].AsEnumerable()
                                      on t2.Field<int?>("availabilityByDateList_id") equals t3.Field<int?>("availabilityByDateList_id")
                                      join t4 in ds.Tables["fareComponentGroupList"].AsEnumerable()
                                      on t3.Field<int?>("originDestinationOptionList_Id") equals t4.Field<int?>("originDestinationOptionList_Id")
                                      join t5 in ds.Tables["boundList"].AsEnumerable()
                                      on t4.Field<int?>("fareComponentGroupList_Id") equals t5.Field<int?>("fareComponentGroupList_Id")
                                      join t6 in ds.Tables["availFlightSegmentList"].AsEnumerable()
                                      on t5.Field<int?>("boundList_Id") equals t6.Field<int?>("boundList_Id")
                                      join t7 in bookingClassList.AsEnumerable()
                                      on t6.Field<int?>("availFlightSegmentList_Id") equals t7.Field<int?>("availFlightSegmentList_Id")
                                      join t8 in dtflightSegment.AsEnumerable()
                                      on t7.Field<int?>("availFlightSegmentList_Id") equals t8.Field<int?>("availFlightSegmentList_Id")
                                      join t9 in ds.Tables["airline"].AsEnumerable()
                                      on t8.Field<int?>("flightSegment_Id") equals t9.Field<int?>("flightSegment_Id")
                                      //join t10 in ds.Tables["arrivalAirport"].AsEnumerable()
                                      //on t9.Field<int?>("flightSegment_Id") equals t10.Field<int?>("flightSegment_Id")
                                      //join t11 in ds.Tables["departureAirport"].AsEnumerable()
                                      //on t10.Field<int?>("flightSegment_Id") equals t11.Field<int?>("flightSegment_Id")
                                      join t12 in ds.Tables["equipment"].AsEnumerable()
                                      on t8.Field<int?>("flightSegment_Id") equals t12.Field<int?>("flightSegment_Id")
                                      //join t13 in ds.Tables["datelist"].AsEnumerable()
                                      //on t2.Field<int?>("availabilityByDateList_id") equals t13.Field<int?>("availabilityByDateList_id")
                                      select new
                                      {
                                          Availability_Id = p.Field<int?>("Availability_Id"),
                                          availabilityResultList_Id = t.Field<int?>("availabilityResultList_Id"),
                                          availabilityRouteList_Id = t1.Field<int?>("availabilityRouteList_Id"),
                                          flexibleFaresOnly = t1.Field<string>("flexibleFaresOnly"),
                                          //dateList = t13.Field<string>("dateList_text"),
                                          //availabilityByDateList_Id = t2.Field<int?>("availabilityByDateList_Id"),
                                          originDestinationOptionList_Id = t3.Field<int?>("originDestinationOptionList_Id"),
                                          fareComponentGroupList_Id = t4.Field<int?>("fareComponentGroupList_Id"),
                                          boundList_Id = t5.Field<int?>("boundList_Id"),
                                          boundCode = t5.Field<string>("boundCode"),
                                          availFlightSegmentList_Id = t6.Field<int?>("availFlightSegmentList_Id"),
                                          addOnSegment = t6.Field<string>("addOnSegment"),
                                          cabin = t7.Field<string>("cabin"),
                                          resBookDesigCode = t7.Field<string>("resBookDesigCode"),
                                          resBookDesigQuantity = t7.Field<string>("resBookDesigQuantity"),
                                          SegmentGroupID = t8.Field<int?>("SegmentGroupID"),
                                          Flight = t8.Field<string>("Flight"),
                                          flightSegment_Id = t8.Field<int?>("flightSegment_Id"),
                                          departureDateTime = t8.Field<string>("departureDateTime"),
                                          departureDateTimeUTC = t8.Field<string>("departureDateTimeUTC"),
                                          arrivalDateTime = t8.Field<string>("arrivalDateTime"),
                                          arrivalDateTimeUTC = t8.Field<string>("arrivalDateTimeUTC"),
                                          flightNumber = t8.Field<string>("flightNumber"),
                                          ondControlled = t8.Field<string>("ondControlled"),
                                          sector = t8.Field<string>("sector"),
                                          codeshare = t8.Field<string>("codeshare"),
                                          distance = t8.Field<string>("distance"),
                                          flownMileageQty = t8.Field<string>("flownMileageQty"),
                                          journeyDuration = t8.Field<string>("journeyDuration"),
                                          onTimeRate = t8.Field<string>("onTimeRate"),
                                          secureFlightDataRequired = t8.Field<string>("secureFlightDataRequired"),
                                          stopQuantity = t8.Field<string>("stopQuantity"),
                                          code = t9.Field<string>("code"),
                                          companyFullName = t9.Field<string>("companyFullName"),
                                          companyShortName = t9.Field<string>("companyShortName"),
                                          //departureAirport_Id = t11.Field<int?>("departureAirport_Id"),
                                          //dep_codeContext = t11.Field<string>("codeContext"),
                                          //dep_language = t11.Field<string>("language"),
                                          //dep_locationCode = t11.Field<string>("locationCode"),
                                          //dep_locationName = t11.Field<string>("locationName"),
                                          //dep_terminal = t11.Field<string>("terminal"),
                                          //arrivalAirport_Id = t10.Field<int?>("arrivalAirport_Id"),
                                          //codeContext = t10.Field<string>("codeContext"),
                                          //language = t10.Field<string>("language"),
                                          //locationCode = t10.Field<string>("locationCode"),
                                          //locationName = t10.Field<string>("locationName"),
                                          //terminal = t10.Field<string>("terminal"),
                                          airEquipType = t12.Field<string>("airEquipType"),
                                          airEquipTypeModel = t12.Field<string>("airEquipTypeModel"),
                                          changeofGauge = t12.Field<string>("changeofGauge"),
                                          //deiCode = t13.Field<string>("deiCode"),
                                          //explanation = t13.Field<string>("explanation"),
                                          //note = t13.Field<string>("note"),


                                      }).ToList();

                    DataTable flights = LINQResultToDataTable(JoinResult);

                    #endregion

                    #region farelist
                    var FareList = (from p in ds.Tables["fareComponentList"].AsEnumerable()
                                    join t in ds.Tables["passengerFareInfoList"].AsEnumerable()
                                    on p.Field<int?>("fareComponentList_Id") equals t.Field<int?>("fareComponentList_Id")
                                    join t1 in ds.Tables["fareInfoList"].AsEnumerable()
                                    on t.Field<int?>("passengerFareInfoList_Id") equals t1.Field<int?>("passengerFareInfoList_Id")
                                    join t2 in ds.Tables["fareBaggageAllowance"].AsEnumerable()
                                    on t1.Field<int?>("fareInfoList_Id") equals t2.Field<int?>("fareInfoList_Id")
                                    join t4 in ds.Tables["passengerTypeQuantity"].AsEnumerable()
                                    on t.Field<int?>("passengerFareInfoList_Id") equals t4.Field<int?>("passengerFareInfoList_Id")
                                    join t5 in ds.Tables["passengerType"].AsEnumerable()
                                    on t4.Field<int?>("passengerTypeQuantity_Id") equals t5.Field<int?>("passengerTypeQuantity_Id")
                                    join t3 in ds.Tables["maxAllowedWeight"].AsEnumerable()
                                    on t2.Field<int?>("fareBaggageAllowance_Id") equals t3.Field<int?>("fareBaggageAllowance_Id")
                                    join t6 in ds.Tables["pricingInfo"].AsEnumerable()
                                    on t.Field<int?>("passengerFareInfoList_Id") equals t6.Field<int?>("passengerFareInfoList_Id")
                                    join t7 in ds.Tables["baseFare"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t7.Field<int?>("pricingInfo_Id")
                                    join t8 in ds.Tables["amount"].AsEnumerable()
                                    on t7.Field<int?>("baseFare_Id") equals t8.Field<int?>("baseFare_Id")
                                    join t9 in ds.Tables["equivBaseFare"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t9.Field<int?>("pricingInfo_Id")
                                    join t10 in ds.Tables["surcharges"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t10.Field<int?>("pricingInfo_Id")
                                    join t11 in ds.Tables["pricingOverview"].AsEnumerable()
                                    on p.Field<int?>("fareComponentList_Id") equals t11.Field<int?>("fareComponentList_Id")
                                    join t12 in ds.Tables["totalAmount"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t12.Field<int?>("pricingOverview_Id")
                                    join t13 in ds.Tables["taxes"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t13.Field<int?>("pricingInfo_Id")
                                    join t14 in ds.Tables["taxList"].AsEnumerable()
                                    on t13.Field<int?>("taxes_Id") equals t14.Field<int?>("taxes_Id")
                                    join t15 in ds.Tables["taxAmount"].AsEnumerable()
                                    on t14.Field<int?>("taxList_Id") equals t15.Field<int?>("taxList_Id")
                                    join t16 in ds.Tables["totalFare"].AsEnumerable()
                                    on t6.Field<int?>("pricingInfo_Id") equals t16.Field<int?>("pricingInfo_Id")
                                    join t17 in ds.Tables["totalBaseFare"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t17.Field<int?>("pricingOverview_Id")
                                    join t18 in ds.Tables["totalSurcharge"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t18.Field<int?>("pricingOverview_Id")
                                    join t19 in ds.Tables["totalTax"].AsEnumerable()
                                    on t11.Field<int?>("pricingOverview_Id") equals t19.Field<int?>("pricingOverview_Id")
                                    select new
                                    {
                                        fareComponentGroupList_Id = p.Field<int?>("fareComponentGroupList_Id"),
                                        fareComponentList_Id = p.Field<int?>("fareComponentList_Id"),
                                        fareKind = p.Field<string>("fareKind"),
                                        internalID = p.Field<string>("internalID"),
                                        maxUsageDate = p.Field<string>("maxUsageDate"),
                                        passengerFareInfoList_Id = t1.Field<int?>("passengerFareInfoList_Id"),
                                        //  endorsementList = t.Field<string>("endorsementList"),
                                        fareInfoList_Id = t1.Field<int?>("fareInfoList_Id"),
                                        cabinClassCode = t1.Field<string>("cabinClassCode"),
                                        fareGroupName = t1.Field<string>("fareGroupName"),
                                        fareReferenceCode = t1.Field<string>("fareReferenceCode"),
                                        fareReferenceID = t1.Field<string>("fareReferenceID"),
                                        fareReferenceName = t1.Field<string>("fareReferenceName"),
                                        flightSegmentSequence = t1.Field<string>("flightSegmentSequence"),
                                        notValidAfter = t1.Field<string>("notValidAfter"),
                                        notValidBefore = t1.Field<string>("notValidBefore"),
                                        resBookDesigCode = t1.Field<string>("resBookDesigCode"),
                                        fareBaggageAllowance_Id = t2.Field<int?>("fareBaggageAllowance_Id"),
                                        allowanceType = t2.Field<string>("allowanceType"),
                                        maxAllowedPieces = t2.Field<string>("maxAllowedPieces"),
                                        unitOfMeasureCode = t3.Field<string>("unitOfMeasureCode"),
                                        weight = t3.Field<string>("weight"),
                                        passengerTypeQuantity_Id = t4.Field<int?>("passengerTypeQuantity_Id"),
                                        hasStrecher = t4.Field<string>("hasStrecher"),
                                        quantity = t4.Field<string>("quantity"),
                                        passengerType_code = t5.Field<string>("code"),
                                        pricingInfo_Id = t6.Field<int?>("pricingInfo_Id"),
                                        fareBaggageAllowance = t6.Field<string>("fareBaggageAllowance"),
                                        fareConstruction = t6.Field<string>("fareConstruction"),
                                        discountApplied = t6.Field<string>("discountApplied"),
                                        passengerTypeCode = t6.Field<string>("passengerTypeCode"),
                                        baseFare_id = t7.Field<int?>("baseFare_id"),
                                        amount_Id = t8.Field<int?>("amount_Id"),
                                        amount_totalFare_Id = t8.Field<int?>("totalFare_Id"),
                                        accountingSign = t8.Field<string>("accountingSign"),
                                        mileAmount = t8.Field<string>("mileAmount"),
                                        value = t8.Field<string>("value"),
                                        equivBaseFare_Id = t9.Field<int?>("equivBaseFare_Id"),
                                        equivBaseFare_accountingSign = t9.Field<string>("accountingSign"),
                                        equivBaseFare_mileAmount = t9.Field<string>("mileAmount"),
                                        equivBaseFare_value = t9.Field<string>("value"),
                                        surcharges_Id = t10.Field<int?>("surcharges_Id"),
                                        pricingOverview_Id = t11.Field<int?>("pricingOverview_Id"),
                                        totalAmount_accountingSign = t12.Field<string>("accountingSign"),
                                        totalAmount_totalAmount_Id = t12.Field<int?>("totalAmount_Id"),
                                        totalAmount_mileAmount = t12.Field<string>("mileAmount"),
                                        totalAmount_value = t12.Field<string>("value"),
                                        totalAmount_surcharges_Id = t12.Field<int?>("surcharges_Id"),
                                        totalAmount_taxes_Id = t12.Field<int?>("taxes_Id"),
                                        taxes_taxes_Id = t13.Field<int?>("taxes_Id"),
                                        taxList_Id = t14.Field<int?>("taxList_Id"),
                                        paid = t14.Field<string>("paid"),
                                        taxCode = t14.Field<string>("taxCode"),
                                        taxAmount_taxAmount_Id = t15.Field<int?>("taxAmount_Id"),
                                        taxAmount_accountingSign = t15.Field<string>("accountingSign"),
                                        taxAmount_mileAmount = t15.Field<string>("mileAmount"),
                                        taxAmount_value = t15.Field<string>("value"),
                                        totalFare_Id = t16.Field<int?>("totalFare_Id"),
                                        totalBaseFare_Id = t17.Field<int?>("totalBaseFare_Id"),
                                        totalBaseFare_accountingSign = t17.Field<string>("accountingSign"),
                                        totalBaseFare_mileAmount = t17.Field<string>("mileAmount"),
                                        totalBaseFare_value = t17.Field<string>("value"),
                                        totalSurcharge_Id = t18.Field<int?>("totalSurcharge_Id"),
                                        totalSurcharge_accountingSign = t18.Field<string>("accountingSign"),
                                        totalSurcharge_mileAmount = t18.Field<string>("mileAmount"),
                                        totalSurcharge_value = t18.Field<string>("value"),
                                        totalTax_Id = t19.Field<int?>("totalTax_Id"),
                                        totalTax_accountingSign = t19.Field<string>("accountingSign"),
                                        totalTax_mileAmount = t19.Field<string>("mileAmount"),
                                        totalTax_value = t19.Field<string>("value"),
                                    }).ToList();

                    DataTable dtFareList = LINQResultToDataTable(FareList);

                    #endregion


                    DataColumn column = new DataColumn("AdultTotalPrice");
                    column.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column1 = new DataColumn("AdultBasePrice");
                    column1.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column2 = new DataColumn("AdultTaxes");
                    column2.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column3 = new DataColumn("ChildTotalPrice");
                    column3.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column4 = new DataColumn("ChildBasePrice");
                    column4.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column5 = new DataColumn("InfantTotalPrice");
                    column5.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column6 = new DataColumn("InfantBasePrice");
                    column6.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column7 = new DataColumn("InfantTaxes");
                    column7.DataType = System.Type.GetType("System.Decimal");

                    DataColumn column8 = new DataColumn("ChildTaxes");
                    column8.DataType = System.Type.GetType("System.Decimal");

                    DataColumn arr_locationCode = new DataColumn("arr_locationCode");
                    arr_locationCode.DataType = System.Type.GetType("System.String");

                    DataColumn dep_locationCode = new DataColumn("dep_locationCode");
                    dep_locationCode.DataType = System.Type.GetType("System.String");

                    //DataColumn column13 = new DataColumn("departureAirport_Id");
                    //column13.DataType = System.Type.GetType("System.Int");

                    //DataColumn column14 = new DataColumn("locationCode");
                    //column14.DataType = System.Type.GetType("System.String");

                    //DataColumn column15 = new DataColumn("locationName");
                    //column15.DataType = System.Type.GetType("System.String");

                    //DataColumn column16 = new DataColumn("locationNameLanguage");
                    //column16.DataType = System.Type.GetType("System.String");

                    //DataColumn column17 = new DataColumn("country_locationCode");
                    //column17.DataType = System.Type.GetType("System.String");

                    //DataColumn column18 = new DataColumn("country_locationName");
                    //column18.DataType = System.Type.GetType("System.String");

                    //DataColumn column19 = new DataColumn("countrylocationNameLanguage");
                    //column19.DataType = System.Type.GetType("System.String");

                    //DataColumn column20 = new DataColumn("country_Id");
                    //column20.DataType = System.Type.GetType("System.Int");

                    flights.Columns.Add(column);
                    flights.Columns.Add(column1);
                    flights.Columns.Add(column2);
                    flights.Columns.Add(column3);
                    flights.Columns.Add(column4);
                    flights.Columns.Add(column5);
                    flights.Columns.Add(column6);
                    flights.Columns.Add(column7);
                    flights.Columns.Add(column8);

                    flights.Columns.Add(dep_locationCode);
                    flights.Columns.Add(arr_locationCode);
                    //flights.Columns.Add(column13);
                    //flights.Columns.Add(column14);
                    //flights.Columns.Add(column15);
                    //flights.Columns.Add(column16);
                    //flights.Columns.Add(column17);
                    //flights.Columns.Add(column19);
                    //flights.Columns.Add(column20);

                    foreach (DataRow ro in flights.Rows)
                    {
                        foreach (DataRow ro1 in dtFareList.Rows)
                        {
                            if (Convert.ToString(ro["fareComponentGroupList_Id"]) == Convert.ToString(ro1["fareComponentGroupList_Id"]) &&
                                Convert.ToString(ro["resBookDesigCode"]) == Convert.ToString(ro1["resBookDesigCode"]))
                            {
                                if (ro1["passengerType_code"].ToString() == "ADLT")
                                {
                                    ro["AdultTotalPrice"] = ro1["totalAmount_value"];
                                    ro["AdultBasePrice"] = ro1["equivBaseFare_value"];
                                    ro["AdultTaxes"] = ro1["totalTax_value"];
                                    ro["ChildTotalPrice"] = 0;
                                    ro["ChildBasePrice"] = 0;
                                    ro["ChildTaxes"] = 0;
                                    ro["InfantTotalPrice"] = 0;
                                    ro["InfantBasePrice"] = 0;
                                    ro["InfantTaxes"] = 0;
                                }
                                else if (ro1["passengerType_code"].ToString() == "CHLD")
                                {
                                    ro["ChildTotalPrice"] = ro1["totalAmount_value"];
                                    ro["ChildBasePrice"] = ro1["equivBaseFare_value"];
                                    ro["ChildTaxes"] = ro1["totalTax_value"];
                                    ro["InfantTotalPrice"] = 0;
                                    ro["InfantBasePrice"] = 0;
                                    ro["InfantTaxes"] = 0;
                                }
                                else if (ro1["passengerType_code"].ToString() == "INFT")
                                {
                                    ro["InfantTotalPrice"] = ro1["totalAmount_value"];
                                    ro["InfantBasePrice"] = ro1["equivBaseFare_value"];
                                    ro["InfantTaxes"] = ro1["totalTax_value"];
                                }
                            }
                        }
                    }

                    foreach (DataRow ro in flights.Rows)
                    {
                        foreach (DataRow ro1 in ds.Tables["departureAirport"].Rows)
                        {
                            if (Convert.ToString(ro["flightSegment_Id"]) == Convert.ToString(ro1["flightSegment_Id"]))
                            {
                                ro["dep_locationCode"] = ro1["locationCode"];
                            }
                        }
                    }

                    foreach (DataRow ro in flights.Rows)
                    {
                        foreach (DataRow ro1 in ds.Tables["arrivalAirport"].Rows)
                        {
                            if (Convert.ToString(ro["flightSegment_Id"]) == Convert.ToString(ro1["flightSegment_Id"]))
                            {
                                ro["arr_locationCode"] = ro1["locationCode"];
                            }
                        }
                    }

                    DataColumn column0 = new DataColumn("SegmentID");
                    column0.DataType = System.Type.GetType("System.Int32");


                    flights.Columns.Add(column0);
                    column0.SetOrdinal(0);

                    for (int a = 0; a < flights.Rows.Count; a++)
                    {
                        flights.Rows[a]["SegmentID"] = a;
                    }

                    DataView dv = flights.DefaultView;
                    dv.Sort = "AdultTotalPrice";
                    flights = dv.ToTable();

                    var JSONString = DataTableToJSONWithStringBuilder(flights);
                    return JSONString;

                    ////TempData["AirSegments"] = dtHitItFlights;
                    //if (Session["HitITAirSegments"] == null)
                    //{
                    //    Session["HitITAirSegments"] = flights;
                    //    Session["dtFareList"] = dtFareList;
                    //    Session["dtCityInfo"] = dtCityInfo;
                    //    Session["flightNotes"] = ds.Tables["flightNotes"];
                    //}
                    //TempData["Origin"] = from;
                    //TempData["Destination"] = to;
                    //TempData["HitItFType"] = tripType;

                    //TempData["HitItFlightsCounts"] = flights.Rows.Count; // ds.Tables["flightSegment"];
                    //                                                     // TempData["HitItFlightsOptions"] = flights.Rows.Count;
                    //TempData["adults"] = adults; TempData["children"] = children; TempData["infants"] = infants;
                    //TempData["deiCode"] = ds.Tables["flightNotes"].Rows[0]["deiCode"].ToString();
                    //TempData["explanation"] = ds.Tables["flightNotes"].Rows[0]["explanation"].ToString();
                    //TempData["note"] = ds.Tables["flightNotes"].Rows[0]["note"].ToString();
                }

                return "something went wrong";
            }
            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message;
                return "something went wrong";
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

        [NonAction]
        private DataTable ConvertJArrayToDataTable(JArray jArray)
        {
            DataTable dataTable = new DataTable();

            if (jArray.Count > 0)
            {
                foreach (JProperty property in jArray[0])
                {
                    dataTable.Columns.Add(property.Name, typeof(string));
                }

                foreach (JObject jObject in jArray)
                {
                    DataRow dataRow = dataTable.NewRow();
                    foreach (JProperty property in jObject.Properties())
                    {
                        dataRow[property.Name] = property.Value.ToString();
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        [NonAction]
        static string ConvertXmlToJson(string xmlString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            // Convert XmlDocument to JObject
            JObject jsonObject = JObject.Parse(JsonConvert.SerializeXmlNode(xmlDoc,
                Newtonsoft.Json.Formatting.None, true));

            // Convert JObject to JSON string
            string jsonString = jsonObject.ToString();

            return jsonString;
        }

        [HttpPost("hititbooking")]
        public string HitItBooking([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                var frm = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                //string flightsList = frm.TotalFlights; // JsonConvert.DeserializeObject<dynamic>(frm.TotalFlights);

                int flightSegmentIDsForFlightDM = -1;
                int flightSegmentIDsForFlightRM = -1;
                int flightSegmentIDsForFlightDC = -1;
                int flightSegmentIDsForFlightRC = -1;

                DataTable dtFlights = null;

                if (frm != null)
                {
                    JArray totalFlightsArray = frm["TotalFlights"] as JArray;

                    if (totalFlightsArray != null)
                    {
                        dtFlights = ConvertJArrayToDataTable(totalFlightsArray);

                        // Convert the JArray to a List<JToken> or another suitable type
                        List<JToken> totalFlightsList = totalFlightsArray.ToList();

                        //List<string> segmentIDsForFlightDM = new List<string>();

                        foreach (JToken flightToken in totalFlightsList)
                        {
                            string flightCode = flightToken["Flight"]?.ToString();
                            string segmentID = flightToken["flightSegment_Id"]?.ToString();

                            if (flightCode == "DM")
                            {
                                // segmentIDsForFlightDM.Add(segmentID);
                                flightSegmentIDsForFlightDM = Convert.ToInt32(segmentID);
                            }
                            else if (flightCode == "RM")
                            {
                                // segmentIDsForFlightDM.Add(segmentID);
                                flightSegmentIDsForFlightRM = Convert.ToInt32(segmentID);
                            }
                            else if (flightCode == "DC")
                            {
                                // segmentIDsForFlightDM.Add(segmentID);
                                flightSegmentIDsForFlightDC = Convert.ToInt32(segmentID);
                            }
                            else if (flightCode == "RC")
                            {
                                // segmentIDsForFlightDM.Add(segmentID);
                                flightSegmentIDsForFlightRC = Convert.ToInt32(segmentID);
                            }
                        }

                    }

                    DateTime dt = DateTime.Now;
                    //DateTime dtOut;
                    string sex = "M";
                    string title = "";
                    string firstName = "";
                    string lastName = "";
                    string dob = "";
                    string nationality = "";
                    string documenttype = "";
                    string document = "";
                    string expiry = "";
                    string mobile = "";

                    int UserID = 127; // Convert.ToInt32(Session["UserID"]);
                    Random random = new Random();
                    string num = random.Next(9).ToString();


                    //  int UserID = user.intUserID;
                    //data.firstName_0.Value
                    int Adults = frm.Adults.Value != null ? Convert.ToInt32(frm.Adults.Value) : 1;
                    int Children = frm.Children.Value != null ? Convert.ToInt32(frm.Children.Value) : 0;
                    int Infants = frm.Infants.Value != null ? Convert.ToInt32(frm.Infants.Value) : 0;

                    string from = frm.from.Value;
                    string to = frm.to.Value;
                    string totaltime0 = frm.totaltime0.Value;
                    string totaltime1 = frm.totaltime1.Value;
                    string stops0 = frm.stops0.Value;
                    string stops1 = frm.stops1.Value;
                    string amount = frm.amount0.Value;
                    string taxes = frm.taxes0.Value;
                    string total = frm.total0.Value;
                    string currency = frm.currency0.Value;
                    //string serviceclass = ""; //frm.cabin.Value;
                    string optionID = frm.Option_Id.Value;
                    Guid TransactionId = Guid.Parse(frm?.TransactionId?.Value);
                    DateTime ArrivalDateTime = frm?.ArrivalDateTime?.Value; //frm?["ArrivalDateTime"].ToString();
                    string Departure = frm.Dep_locationCode.Value;
                    DateTime DepartureDateTime = frm.DepartureDateTime.Value;
                    string Arrival = frm.Arr_locationCode.Value;
                    string resBookDesigCode = frm.ResBookDesigCode.Value;

                    string Cabin = frm.Cabin.Value;
                    string fareComponentGroupList_Id = frm.FareComponentGroupList_Id.Value;

                    string leadfirstname = !string.IsNullOrEmpty(frm.firstName_0.Value) ? frm.firstName_0.Value.ToString().ToUpper() : "";
                    string leadlastname = !string.IsNullOrEmpty(frm.lastName_0.Value) ? frm.lastName_0.Value.ToString().ToUpper() : "";
                    string fromM_1 = "";
                    string fromM_2 = "";
                    string fromM_3 = "";
                    string fromM_4 = "";
                    string fromM_5 = "";
                    string fromM_6 = "";
                    string toM_1 = "";
                    string toM_2 = "";
                    string toM_3 = "";
                    string toM_4 = "";
                    string toM_5 = "";
                    string toM_6 = "";
                    string start = "";
                    string airline = "";

                    string flightType = !string.IsNullOrEmpty(frm.flightType.Value) ? frm.flightType.Value.ToString().ToUpper() : "";

                    string fareReferenceID = !string.IsNullOrEmpty(frm.fareReferenceID.Value) ? frm.fareReferenceID.Value.ToString() : "";

                    string resBookDesigCode1 = "";
                    string fareComponentGroupList_Id1 = "";
                    string optionID_r = "";
                    //Guid TransactionId = Guid.Parse(frm.TransactionId_r.Value);
                    DateTime ArrivalDateTime_r = DateTime.Now;
                    string Departure_r = "";
                    DateTime DepartureDateTime_r = DateTime.Now;
                    string Arrival_r = "";
                    string resBookDesigCode_r = "";
                    string Cabin_r = "";
                    string fareComponentGroupList_Id_r = "0"; // frm.FareComponentGroupList_Id_r.Value;

                    if (flightType == "RETURN")
                    {
                        fareComponentGroupList_Id_r = frm.FareComponentGroupList_Id_r.Value;
                        resBookDesigCode1 = frm.ResBookDesigCode_r.Value;
                        fareComponentGroupList_Id1 = frm.FareComponentGroupList_Id_r.Value;
                        optionID_r = frm.Option_Id_r.Value;
                        ArrivalDateTime_r = frm.ArrivalDateTime_r.Value;
                        Departure_r = frm.Dep_locationCode_r.Value;
                        DepartureDateTime_r = frm.DepartureDateTime_r.Value;
                        Arrival_r = frm.Arr_locationCode_r.Value;
                        resBookDesigCode_r = frm.ResBookDesigCode_r.Value;
                        Cabin_r = frm.Cabin_r.Value;
                    }

                    string depDT = DepartureDateTime.ToString("MM/dd/yyyy HH:mm:ss");

                    DataSet ds = new DataSet();
                    //  DataTable dtFlights = dataTable.copy; //new DataTable();
                    DataSet dtFareList = new DataSet();
                    DataSet dtCityInfo = new DataSet();
                    DataTable dtflightNotes = new DataTable();

                    byte[] hr = sql.HitItResults.Where(x => x.TransactionId == TransactionId).Select(x => x.HititResponse).FirstOrDefault();
                    byte[] fl = sql.HitItResults.Where(x => x.TransactionId == TransactionId).Select(x => x.FareList).FirstOrDefault();
                    byte[] ci = sql.HitItResults.Where(x => x.TransactionId == TransactionId).Select(x => x.CityInfo).FirstOrDefault();
                    byte[] fn = sql.HitItResults.Where(x => x.TransactionId == TransactionId).Select(x => x.FlightNotes).FirstOrDefault();
                    byte[] finalResult = sql.HitItResults.Where(x => x.TransactionId == TransactionId).Select(x => x.FinalResult).FirstOrDefault();

                    DataSet dsHititResponse = new DataSet();

                    if (hr != null && hr.Length > 0)
                    {
                        MemoryStream stream = new MemoryStream(hr);
                        dsHititResponse.ReadXml(stream);
                    }
                    else
                    {
                        return "stream error. something went wrong.";
                    }
                    //UTF8Encoding encoding = new UTF8Encoding();
                    //string str =  encoding.GetString(fl, 0, fl.Length);

                    MemoryStream streamFL = new MemoryStream(fl);
                    dtFareList.ReadXml(streamFL);


                    MemoryStream streamCI = new MemoryStream(ci);
                    dtCityInfo.ReadXml(streamCI);

                    //dsHititResponse  = JsonConvert.DeserializeObject<DataSet>(hr);


                    foreach (DataRow row in dtFlights.Rows)
                    {
                        // Convert the values to DateTime and then format to ISO 8601
                        string departureDateTime = row["departureDateTime"].ToString();
                        string departureDateTimeUTC = row["departureDateTimeUTC"].ToString();
                        string arrivalDateTime = row["arrivalDateTime"].ToString();
                        string arrivalDateTimeUTC = row["arrivalDateTimeUTC"].ToString();

                        row["departureDateTime"] = Convert.ToDateTime(departureDateTime).ToString("s", CultureInfo.InvariantCulture);
                        row["departureDateTimeUTC"] = Convert.ToDateTime(departureDateTimeUTC).ToString("s", CultureInfo.InvariantCulture);
                        row["arrivalDateTime"] = Convert.ToDateTime(arrivalDateTime).ToString("s", CultureInfo.InvariantCulture);
                        row["arrivalDateTimeUTC"] = Convert.ToDateTime(arrivalDateTimeUTC).ToString("s", CultureInfo.InvariantCulture);
                    }

                    dtFlights.AcceptChanges();

                    //if (flightType == "ONEWAY")
                    //{
                    //    dsHititResponse = HitItFlightsForBooking("OneWay", Departure, Arrival, Adults, Children, Infants, DepartureDateTime.ToString("yyyy-MM-dd"),
                    //                                    ArrivalDateTime.ToString("yyyy-MM-dd"),
                    //                                  fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6,
                    //                                  toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline).Result;
                    //    dtCityInfo = CityInfo(dsHititResponse);
                    //    dtFareList = FareList(dsHititResponse);
                    //   // dtFlights = Flights(dsHititResponse, dtFareList, Departure, Arrival, flightType);
                    //}
                    //else
                    //{
                    //    dsHititResponse = HitItFlightsReturnsForBooking("Return", Departure, Arrival, Adults, Children, Infants, DepartureDateTime.ToString("yyyy-MM-dd"),
                    //                                                            ArrivalDateTime_r.ToString("yyyy-MM-dd"),
                    //                                                          fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6,
                    //                                                          toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline).Result;
                    //    dtCityInfo = ReturnCityInfo(dsHititResponse);
                    //    dtFareList = ReturnFareList(dsHititResponse);
                    //  //  dtFlights = ReturnFlights(dsHititResponse, dtFareList, Departure, Arrival, flightType, DepartureDateTime.ToString("yyyy-MM-dd"),
                    //                                                         //   ArrivalDateTime_r.ToString("yyyy-MM-dd"));
                    //}


                    var FareInfoList = (from p in dsHititResponse.Tables["fareComponentList"].AsEnumerable()
                                        join t in dsHititResponse.Tables["passengerFareInfoList"].AsEnumerable()
                                        on p.Field<int?>("fareComponentList_Id") equals t.Field<int?>("fareComponentList_Id")
                                        join t1 in dsHititResponse.Tables["fareInfoList"].AsEnumerable()
                                        on t.Field<int?>("passengerFareInfoList_Id") equals t1.Field<int?>("passengerFareInfoList_Id")
                                        select new
                                        {
                                            fareComponentGroupList_Id = p.Field<int?>("fareComponentGroupList_Id"),
                                            fareComponentList_Id = p.Field<int?>("fareComponentList_Id"),
                                            fareKind = p.Field<string>("fareKind"),
                                            internalID = p.Field<string>("internalID"),
                                            maxUsageDate = p.Field<string>("maxUsageDate"),
                                            passengerFareInfoList_Id = t1.Field<int?>("passengerFareInfoList_Id"),
                                            //   endorsementList = t.Field<string>("endorsementList"),
                                            fareInfoList_Id = t1.Field<int?>("fareInfoList_Id"),
                                            cabinClassCode = t1.Field<string>("cabinClassCode"),
                                            fareGroupName = t1.Field<string>("fareGroupName"),
                                            fareReferenceCode = t1.Field<string>("fareReferenceCode"),
                                            fareReferenceID = t1.Field<string>("fareReferenceID"),
                                            fareReferenceName = t1.Field<string>("fareReferenceName"),
                                            flightSegmentSequence = t1.Field<string>("flightSegmentSequence"),
                                            notValidAfter = t1.Field<string>("notValidAfter"),
                                            notValidBefore = t1.Field<string>("notValidBefore"),
                                            resBookDesigCode = t1.Field<string>("resBookDesigCode")
                                        }).ToList();

                    DataTable dtFareInfoList = LINQResultToDataTable(FareInfoList);

                    string deiCode = "7";
                    string explanation = "Meal Service";
                    string note = "S";

                    if (dsHititResponse.Tables.Contains("flightNotes"))
                    {
                        var FlightNotes = (from n in dsHititResponse.Tables["flightNotes"].AsEnumerable().Where(n => n.Field<int?>("flightSegment_ID") == flightSegmentIDsForFlightDM)
                                           select new
                                           {
                                               deiCode = n.Field<string?>("deiCode"),
                                               explanation = n.Field<string?>("explanation"),
                                               note = n.Field<string?>("note")
                                           }).FirstOrDefault();

                        if (FlightNotes != null)
                        {
                            deiCode = FlightNotes.deiCode;
                            explanation = FlightNotes.explanation;
                            note = FlightNotes.note;
                        }
                    }

                    string deiCode1 = "7";
                    string explanation1 = "Meal Service";
                    string note1 = "S";

                    if (flightType != "ONEWAY")
                    {
                        if (dsHititResponse.Tables.Contains("flightNotes"))
                        {
                            var FlightNotes = (from n in dsHititResponse.Tables["flightNotes"].AsEnumerable().Where(n => n.Field<int?>("flightSegment_ID") == flightSegmentIDsForFlightRM)
                                               select new
                                               {
                                                   deiCode = n.Field<string?>("deiCode"),
                                                   explanation = n.Field<string?>("explanation"),
                                                   note = n.Field<string?>("note")
                                               }).FirstOrDefault();
                            if (FlightNotes != null)
                            {
                                deiCode1 = FlightNotes.deiCode;
                                explanation1 = FlightNotes.explanation;
                                note1 = FlightNotes.note;
                            }
                        }
                    }

                    string fType = "OneWay";

                    if (flightType == "ONEWAY")
                    {
                        fType = "OneWay";
                    }
                    else
                    {
                        fType = "Return";
                    }

                    //  string fareComponentGroupList_Id1 = frm.fareComponentGroupList_Id1.Value;
                    //   string resBookDesigCode1 = frm.resBookDesigCode1.Value;
                    //    string deiCode1 = frm.deiCode1.Value;
                    //   string explanation1 = frm.explanation1.Value;
                    //   string note1 = frm.note1.Value;
                    string _ticket = ""; // frm.chkTicket.Value;

                    DateTime datet = DateTime.Now.Date;
                    string Dat = datet.ToString("yyyy-MM-dd");

                    DateTime dt1 = DateTime.UtcNow;
                    string tstamp = dt1.ToString("s") + "Z";
                    //string AirlineCode = "PK";
                    //bool error = false;
                    string email = !string.IsNullOrEmpty(frm.email_0.Value) ? frm.email_0.Value.ToString() : "";
                    string contact = !string.IsNullOrEmpty(frm.MobileNO_0.Value) ? frm.MobileNO_0.Value.ToString() : "";

                    //string address = "";
                    //string cityname = "";
                    //string agencyname = "";
                    //string postalcode = "";
                    //string phonenumber = "";
                    //string companyemail = "";
                    //int CustomerID = 0; // Convert.ToInt32(Session["CustomerID"]);

                    #region XML Request

                    string xmlRequest = "";
                    StringBuilder strReq = new StringBuilder();

                    if (Adults > 0)
                    {
                        for (int i = 0; i < Adults; i++)
                        {
                            DateTime _age = Convert.ToDateTime(frm?["dob_" + i.ToString()]?.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(frm?["expiry_" + i.ToString()]);
                            sex = frm?["Gender_" + i.ToString()];
                            document = frm?["document_" + i.ToString()]?.ToString().ToUpper();
                            documenttype = frm?["documenttype_" + i.ToString()]?.ToString().ToUpper();

                            if (frm?["expiry_" + i.ToString()] != null && frm?["expiry_" + i.ToString()] != "")
                            {
                                expiry = frm?["expiry_" + i.ToString()];
                            }


                            nationality = frm?["nationality_" + i.ToString()]?.ToString(); // data?.nationality_i?.Value.ToString();

                            dob = frm?["dob_" + i.ToString()]?.ToString();
                            title = frm?["title_" + i.ToString()]?.ToString(); // data?.title?.ToUpper();
                            firstName = frm?["firstName_" + i.ToString()]?.ToString().ToUpper(); // data?.firstName_i?.Value.ToString().ToUpper();
                            lastName = frm?["lastName_" + i.ToString()]?.ToString().ToUpper();  // data?.lastName_i?.Value.ToString().ToUpper();                            
                            title = frm?["title_" + i.ToString()]?.ToString().ToUpper();
                            mobile = contact;

                            if (sex == "Female")
                            {
                                sex = "F";
                            }
                            else
                            {
                                sex = "M";
                            }

                            strReq.Append("<airTravelerList>");
                            strReq.Append("<gender>" + sex + "</gender>");
                            strReq.Append("<hasStrecher/><parentSequence/><passengerTypeCode>ADLT</passengerTypeCode>");
                            strReq.Append("<personName>");
                            strReq.Append("<givenName>" + firstName + "</givenName>");
                            strReq.Append("<shareMarketInd/><surname>" + lastName + "</surname>");
                            strReq.Append("</personName>");
                            strReq.Append("<accompaniesqlyInfant/><contactPerson>");
                            strReq.Append("<email>");
                            strReq.Append("<email>" + email + "</email>");
                            strReq.Append("<markedForSendingRezInfo/><preferred/><shareMarketInd>true</shareMarketInd>");
                            strReq.Append("</email>");
                            strReq.Append("<markedForSendingRezInfo>true</markedForSendingRezInfo>");
                            strReq.Append("<phoneNumber>");
                            strReq.Append("<areaCode>051</areaCode>");
                            strReq.Append("<countryCode>+92</countryCode>");
                            strReq.Append("<markedForSendingRezInfo>true</markedForSendingRezInfo>");
                            strReq.Append("<preferred/><shareMarketInd/><subscriberNumber>" + mobile + "</subscriberNumber>");
                            strReq.Append("</phoneNumber>");
                            strReq.Append("<shareMarketInd>true</shareMarketInd>");
                            strReq.Append("<socialSecurityNumber>" + document + "</socialSecurityNumber><useForInvoicing/>");
                            strReq.Append("<shareContactInfo>true</shareContactInfo>");
                            strReq.Append("</contactPerson>");
                            strReq.Append("<requestedSeatCount>1</requestedSeatCount><shareMarketInd/><unaccompaniedMinor/>");
                            strReq.Append("</airTravelerList>");
                        }
                    }

                    if (Children > 0)
                    {
                        for (int c = 1; c <= Children; c++)
                        {
                            sex = frm?["sexC_" + c.ToString()]?.ToString();
                            document = frm?["documentC_" + c.ToString()]?.ToString();
                            documenttype = frm?["documenttypeC_" + c.ToString()]?.ToString();
                            expiry = Convert.ToDateTime(frm?["expiryC_" + c.ToString()]?.ToString("yyyy -MM-dd"));
                            nationality = frm?["nationalityC_" + c.ToString()]?.ToString();
                            firstName = frm?["firstNameC_" + c.ToString()]?.ToString();
                            lastName = frm?["lastNameC_" + c.ToString()]?.ToString();
                            DateTime dtDOB = Convert.ToDateTime(frm?["dobC_" + c.ToString()]?.ToString());
                            //TimeSpan t = DateTime.Now - dtDOB;
                            dob = Convert.ToDateTime(frm?["dobC_" + c.ToString()]?.ToString().ToString("yyyy -MM-dd"));

                            if (sex == "Female")
                            {
                                sex = "F";
                            }
                            else
                            {
                                sex = "M";
                            }
                            strReq.Append("<airTravelerList>");
                            strReq.Append("<gender>" + sex + "</gender>");
                            strReq.Append("<birthDate>" + dob + "</birthDate>");
                            strReq.Append("<hasStrecher/><parentSequence/><passengerTypeCode>CHLD</passengerTypeCode>");
                            strReq.Append("<personName>");
                            strReq.Append("<givenName>" + firstName + "</givenName>");
                            strReq.Append("<shareMarketInd/><surname>" + lastName + "</surname>");
                            strReq.Append("</personName>");
                            strReq.Append("<accompaniesqlyInfant/><contactPerson>");
                            strReq.Append("<email>");
                            strReq.Append("<email>" + email + "</email>");
                            strReq.Append("<markedForSendingRezInfo/><preferred/><shareMarketInd>true</shareMarketInd>");
                            strReq.Append("</email>");
                            strReq.Append("<markedForSendingRezInfo>true</markedForSendingRezInfo>");
                            strReq.Append("<phoneNumber>");
                            strReq.Append("<areaCode>051</areaCode>");
                            strReq.Append("<countryCode>+92</countryCode>");
                            strReq.Append("<markedForSendingRezInfo>true</markedForSendingRezInfo>");
                            strReq.Append("<preferred/><shareMarketInd/><subscriberNumber>" + mobile + "</subscriberNumber>");
                            strReq.Append("</phoneNumber>");
                            strReq.Append("<shareMarketInd>true</shareMarketInd>");
                            strReq.Append("<socialSecurityNumber>" + document + "</socialSecurityNumber><useForInvoicing/>");
                            strReq.Append("<shareContactInfo>true</shareContactInfo>");
                            strReq.Append("</contactPerson>");
                            strReq.Append("<requestedSeatCount>1</requestedSeatCount><shareMarketInd/><unaccompaniedMinor/>");
                            strReq.Append("</airTravelerList>");
                        }
                    }

                    strReq.Append("<requestPurpose>MODIFY_PERMANENTLY_AND_CALC</requestPurpose>");

                    strReq.Append("<specialRequestDetails>");

                    if (Children > 0)
                    {
                        for (int c = 1; c <= Children; c++)
                        {
                            DateTime dtDOB = Convert.ToDateTime(frm?["dobC_" + c.ToString()]?.ToString().ToString("yyyy-MM-dd"));

                            strReq.Append("<specialServiceRequestList>");
                            strReq.Append("<airTravelerSequence>3</airTravelerSequence>");
                            strReq.Append("<flightSegmentSequence>0</flightSegmentSequence>");
                            strReq.Append("<SSR>");
                            strReq.Append("<allowedQuantityPerPassenger/>");
                            strReq.Append("<bundleRelatedSsr/>");
                            strReq.Append("<code>CHLD</code>");
                            strReq.Append("<explanation>" + dtDOB.ToString("yyyy-MM-dd") + "</explanation>");
                            strReq.Append("<extraBaggage/>");
                            strReq.Append("<free/>");
                            strReq.Append("<showOnItinerary/>");
                            strReq.Append("<unitOfMeasureExist/>");
                            strReq.Append("</SSR>");
                            strReq.Append("<serviceQuantity>1</serviceQuantity>");
                            strReq.Append("<status>NN</status>");
                            strReq.Append("<ticketed/>");
                            strReq.Append("</specialServiceRequestList>");
                        }
                    }


                    if (Infants > 0)
                    {
                        for (int i = 1; i <= Infants; i++)
                        {
                            DateTime dtDOB = Convert.ToDateTime(frm?["dobI_" + i.ToString()]?.ToString("yyyy-MM-dd"));
                            sex = frm?["sexI_" + i.ToString()]?.ToString();
                            document = frm?["documentI_" + i.ToString()]?.ToString();
                            documenttype = frm?["documenttypeI_" + i.ToString()]?.ToString();
                            expiry = Convert.ToDateTime(frm?["expiryI_" + i.ToString()]?.ToString("yyyy-MM-dd"));
                            nationality = frm?["nationalityI_" + i.ToString()]?.ToString();
                            firstName = frm?["firstNameI_" + i.ToString()]?.ToString();
                            lastName = frm?["lastNameI_" + i.ToString()]?.ToString();

                            if (fType == "OneWay")
                            {
                                strReq.Append("<specialServiceRequestList>");
                                strReq.Append("<airTravelerSequence>1</airTravelerSequence>");
                                strReq.Append("<flightSegmentSequence>1</flightSegmentSequence>");
                                strReq.Append("<SSR>");
                                strReq.Append("<allowedQuantityPerPassenger/>");
                                strReq.Append("<bundleRelatedSsr/>");
                                strReq.Append("<code>INFT</code>");
                                strReq.Append("<explanation>" + firstName + "/" + lastName + " Inft " + dtDOB.ToString("ddMMMyy") + "</explanation>");
                                strReq.Append("<extraBaggage/>");
                                strReq.Append("<free/>");
                                strReq.Append("<showOnItinerary/>");
                                strReq.Append("<unitOfMeasureExist/>");
                                strReq.Append("</SSR>");
                                strReq.Append("<serviceQuantity>1</serviceQuantity>");
                                strReq.Append("<status>NN</status>");
                                strReq.Append("<ticketed/>");
                                strReq.Append("</specialServiceRequestList>");
                            }
                            else if (fType == "Return")
                            {
                                strReq.Append("<specialServiceRequestList>");
                                strReq.Append("<airTravelerSequence>1</airTravelerSequence>");
                                strReq.Append("<flightSegmentSequence>1</flightSegmentSequence>");
                                strReq.Append("<SSR>");
                                strReq.Append("<allowedQuantityPerPassenger/>");
                                strReq.Append("<bundleRelatedSsr/>");
                                strReq.Append("<code>INFT</code>");
                                strReq.Append("<explanation>" + firstName + "/" + lastName + " Inft " + dtDOB.ToString("ddMMMyy") + "</explanation>");
                                strReq.Append("<extraBaggage/>");
                                strReq.Append("<free/>");
                                strReq.Append("<showOnItinerary/>");
                                strReq.Append("<unitOfMeasureExist/>");
                                strReq.Append("</SSR>");
                                strReq.Append("<serviceQuantity>1</serviceQuantity>");
                                strReq.Append("<status>NN</status>");
                                strReq.Append("<ticketed/>");
                                strReq.Append("</specialServiceRequestList>");
                                strReq.Append("<specialServiceRequestList>");
                                strReq.Append("<airTravelerSequence>1</airTravelerSequence>");
                                strReq.Append("<flightSegmentSequence>2</flightSegmentSequence>");
                                strReq.Append("<SSR>");
                                strReq.Append("<allowedQuantityPerPassenger/>");
                                strReq.Append("<bundleRelatedSsr/>");
                                strReq.Append("<code>INFT</code>");
                                strReq.Append("<explanation>" + firstName + "/" + lastName + " Inft " + dtDOB.ToString("ddMMMyy") + "</explanation>");
                                strReq.Append("<extraBaggage/>");
                                strReq.Append("<free/>");
                                strReq.Append("<showOnItinerary/>");
                                strReq.Append("<unitOfMeasureExist/>");
                                strReq.Append("</SSR>");
                                strReq.Append("<serviceQuantity>1</serviceQuantity>");
                                strReq.Append("<status>NN</status>");
                                strReq.Append("<ticketed/>");
                                strReq.Append("</specialServiceRequestList>");
                            }
                        }
                    }

                    strReq.Append("</specialRequestDetails>");

                    if (fType == "OneWay")
                    {
                        //DataRow item = (from DataRow drow1 in dtFlights.Rows
                        //                where drow1["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id.ToString() &&
                        //                drow1["resBookDesigCode"].ToString() == resBookDesigCode.ToString()
                        //                select drow1).FirstOrDefault();

                        //DataRow fareDetails = (from DataRow drow2 in dtFareList.Tables[0].Rows
                        //                       where drow2["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id.ToString() &&
                        //                       drow2["resBookDesigCode"].ToString() == resBookDesigCode.ToString()
                        //                       select drow2).FirstOrDefault();

                        //if (fareDetails == null)
                        //{
                        //    return "no seat available. please check another date.";
                        //}

                        //DataRow DepartureCity = (from DataRow drow3 in dtCityInfo.Tables[0].Rows
                        //                         where drow3["locationCode"].ToString() == Departure
                        //                         select drow3).FirstOrDefault();

                        //DataRow ArrivalCity = (from DataRow drow4 in dtCityInfo.Tables[0].Rows
                        //                       where drow4["locationCode"].ToString() == Arrival
                        //                       select drow4).FirstOrDefault();

                        xmlRequest = OneWayBookingXML(strReq.ToString(), dtFlights, dtFareList.Tables[0], resBookDesigCode, deiCode, explanation, note, dtCityInfo.Tables[0]);
                    }
                    else if (fType == "Return")
                    {
                        DataRow item = (from DataRow drow1 in dtFlights.Rows
                                        where drow1["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id.ToString() &&
                                        drow1["resBookDesigCode"].ToString() == resBookDesigCode.ToString() && drow1["Flight"].ToString() == "DM"
                                        select drow1).FirstOrDefault();

                        DataRow fareDetails = (from DataRow drow2 in dtFareList.Tables[0].Rows
                                               where drow2["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id.ToString() &&
                                               drow2["resBookDesigCode"].ToString() == resBookDesigCode.ToString()
                                               select drow2).FirstOrDefault();

                        //DataRow fareDetails = (from DataRow drow2 in dtFareList.Tables[0].Rows
                        //                       where drow2["fareReferenceID"].ToString() == fareReferenceID.ToString() 
                        //                       select drow2).FirstOrDefault();

                        ////////DataRow fareDetails = (from DataRow drow2 in dtFareList.Rows
                        ////////                       where drow2["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id.ToString() &&
                        ////////                       drow2["resBookDesigCode"].ToString() == resBookDesigCode.ToString() &&
                        ////////                       Convert.ToDateTime(drow2["notValidBefore"]).ToShortDateString() == Convert.ToDateTime(item["departureDateTime"]).ToShortDateString()
                        ////////                       select drow2).FirstOrDefault();

                        DataRow Returnitem = (from DataRow drow1 in dtFlights.Rows
                                              where drow1["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id1.ToString() &&
                                              drow1["resBookDesigCode"].ToString() == resBookDesigCode1.ToString() && drow1["Flight"].ToString() == "RM"
                                              select drow1).FirstOrDefault();


                        //DataRow ReturnfareDetails = (from DataRow drow2 in dtFareList.Tables[0].Rows
                        //                             where drow2["fareReferenceID"].ToString() == fareReferenceID.ToString()
                        //                             select drow2).FirstOrDefault();

                        DataRow ReturnfareDetails = (from DataRow drow2 in dtFareList.Tables[0].Rows
                                                     where drow2["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id1.ToString() &&
                                                     drow2["resBookDesigCode"].ToString() == resBookDesigCode1.ToString()
                                                     select drow2).FirstOrDefault();

                        if (ReturnfareDetails == null)
                        {
                            return "no seat available. please check another date.";
                        }


                        DataRow DepartureCity = (from DataRow drow3 in dtCityInfo.Tables[0].Rows
                                                 where drow3["locationCode"].ToString() == Departure
                                                 select drow3).FirstOrDefault();

                        DataRow ArrivalCity = (from DataRow drow4 in dtCityInfo.Tables[0].Rows
                                               where drow4["locationCode"].ToString() == Arrival
                                               select drow4).FirstOrDefault();

                        DataRow FlightNotes = (from DataRow drow4 in dtflightNotes.Rows
                                               where drow4["flightSegment_Id"].ToString() == Arrival
                                               select drow4).FirstOrDefault();

                        xmlRequest = ReturnBookingXML(strReq.ToString(), item, fareDetails, Returnitem, ReturnfareDetails, DepartureCity, ArrivalCity, resBookDesigCode, deiCode, explanation, note, resBookDesigCode1, deiCode1, explanation1, note1);
                    }

                    #endregion


                    string result1 = HitItDepartureBooking(xmlRequest).Result;

                    //XmlDocument doc = new XmlDocument();
                    //doc.Load("C:\\Users\\Hp\\Downloads\\ret-reposne.xml");
                    //XmlReader xmlReader = new XmlNodeReader(doc);

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(result1);
                    XmlReader xmlReader = new XmlNodeReader(xml);

                    ds.ReadXml(xmlReader);

                    if (ds.Tables.Count < 1)
                    {
                        return "something went wrong. 1";
                    }

                    DataTable dtAirPricingInfo;

                    //  string AirPricingInfoKey = "1";

                    // ds2.ReadXml(AirPricingSolution);
                    List<string> bookingReferenceIDList = new List<string>();

                    if (ds.Tables.Contains("bookingReferenceIDList"))
                    {
                        foreach (DataRow ro in ds.Tables["bookingReferenceIDList"].Rows)
                        {
                            bookingReferenceIDList.Add(ro["ID"].ToString());
                        }
                        dtAirPricingInfo = (DataTable)ds.Tables["bookingReferenceIDList"];
                    }
                    else
                    {
                        bookingReferenceIDList.Add(ds.Tables["bookingReferenceIDList"].Rows[0]["ID"].ToString());
                        dtAirPricingInfo = (DataTable)ds.Tables["bookingReferenceIDList"];
                    }

                    string ID1 = ds.Tables["bookingReferenceIDList"].Rows[0]["ID"].ToString();
                    string PnrForTicket = ds.Tables["bookingReferenceIDList"].Rows[0]["referenceID"].ToString();

                    if (bookingReferenceIDList.Count > 0)
                    {
                        #region Booking Info

                        //int Adts = 0;
                        //int Chlds = 0;
                        //int inft = 0; flightSegmentIDsForFlightDM

                        //var Fares = (from f in dtFlights.AsEnumerable().Where(f => f.Field<string?>("fareComponentGroupList_Id") == fc &&
                        //                    f.Field<string?>("boundCode") == "Outbound" && f.Field<string?>("resBookDesigCode") == resBookDesigCode && (f.Field<string?>("Flight") == "DM" || f.Field<string?>("Flight") == "M"))
                        //             select new
                        //             {
                        //                 AdultTotalPrice = f.Field<decimal?>("AdultTotalPrice"),
                        //                 AdultBasePrice = f.Field<decimal?>("AdultBasePrice"),
                        //                 AdultTaxes = f.Field<decimal?>("AdultTaxes"),
                        //                 ChildTotalPrice = f.Field<decimal?>("ChildTotalPrice"),
                        //                 ChildBasePrice = f.Field<decimal?>("ChildBasePrice"),
                        //                 ChildTaxes = f.Field<decimal?>("ChildTaxes"),
                        //                 InfantTotalPrice = f.Field<decimal?>("InfantTotalPrice"),
                        //                 InfantBasePrice = f.Field<decimal?>("InfantBasePrice"),
                        //                 InfantTaxes = f.Field<decimal?>("InfantTaxes"),
                        //             }).FirstOrDefault();

                        string fsID = flightSegmentIDsForFlightDM.ToString();

                        var Fares = (from f in dtFlights.AsEnumerable().Where(f => f.Field<string?>("flightSegment_Id") == fsID)
                                     select new
                                     {
                                         AdultTotalPrice = f.Field<string?>("AdultTotalPrice"),
                                         AdultBasePrice = f.Field<string?>("AdultBasePrice"),
                                         AdultTaxes = f.Field<string?>("AdultTaxes"),
                                         ChildTotalPrice = f.Field<string?>("ChildTotalPrice"),
                                         ChildBasePrice = f.Field<string?>("ChildBasePrice"),
                                         ChildTaxes = f.Field<string?>("ChildTaxes"),
                                         InfantTotalPrice = f.Field<string?>("InfantTotalPrice"),
                                         InfantBasePrice = f.Field<string?>("InfantBasePrice"),
                                         InfantTaxes = f.Field<string?>("InfantTaxes"),
                                     }).FirstOrDefault();



                        decimal? Adultsfare = Convert.ToDecimal(Fares.AdultBasePrice);
                        decimal? Childfare = Convert.ToDecimal(Fares.ChildBasePrice);
                        decimal? Infantfare = Convert.ToDecimal(Fares.InfantBasePrice);
                        decimal? AdultTaxes = Convert.ToDecimal(Fares.AdultTaxes);
                        decimal? ChildTaxes = Convert.ToDecimal(Fares.ChildTaxes);
                        decimal? InfnatTaxes = Convert.ToDecimal(Fares.InfantTaxes);
                        decimal? totalfare = 0;
                        decimal? totaltaxes = 0;

                        decimal? grandtotal = 0;


                        DateTime dttrans = DateTime.Now.ToUniversalTime();

                        // decimal[] TotalAmt = 1000; //_AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Children, Infants, AirlineCode);

                        totaltaxes = AdultTaxes + ChildTaxes + InfnatTaxes;
                        totalfare = Adultsfare + Childfare + Infantfare; //TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                        grandtotal = totalfare + totaltaxes + 1000;// + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                                                                   //if (Adults > 0) { Adultsfare = TotalAmt[2] / Adults; }
                                                                   //if (Children > 0) { Childfare = TotalAmt[3] / Children; }
                                                                   //if (Infants > 0) { Infatfare = TotalAmt[4] / Infants; }


                        // List<MalikExpressMVC.Models.XML.OriginDestinationOption> odSegments = (List<MalikExpressMVC.Models.XML.OriginDestinationOption>)Session["odSegments"];

                        int BookingTypeID = 1; //odSegments.Count();

                        string Scale = "";
                        decimal Incentives = 0;
                        //if (Session["Incentives"] != null)
                        //{
                        //    HashSet<Incentive> inc = (HashSet<Incentive>)Session["Incentives"];
                        //    var FlightIncentives = inc.Where(x => x.ServiceID == 2).FirstOrDefault();

                        //    Incentives = FlightIncentives.Incentives ?? 0;
                        //    Scale = FlightIncentives.Scale;
                        //}

                        var anyReservation = sql.Reservations.Any();
                        int lastBookingId = 0;

                        if (anyReservation)
                        {
                            lastBookingId = sql.Reservations.Max(p => p.Id);
                        }



                        string ID = ds.Tables["bookingReferenceIDList"].Rows[0][1].ToString();
                        Reservation flight = new Reservation()
                        {
                            Adults = Adults,
                            Children = Children,
                            Infants = Infants,
                            PNR = ID,
                            AdultBasicFare = Adultsfare,
                            ChildBasicFare = Childfare,
                            InfantBasicFare = Infantfare,
                            AdultTax = AdultTaxes,
                            ChildTax = ChildTaxes,
                            InfantTax = InfnatTaxes,
                            TotalFare = totalfare,
                            TotalTaxes = totaltaxes,
                            GrandTotal = grandtotal,
                            CustomerComission = 0,
                            CustomerOComission = 0,
                            AirlineComission = 0,
                            Incentive = Incentives,
                            IncentiveScale = Scale,
                            BookingDate = DateTime.Now.ToUniversalTime(),
                            BookingStatus = "Confirmed",
                            BookingTypeID = BookingTypeID,
                            LeadFirstName = leadfirstname,
                            LeadLastName = leadlastname,
                            UserID = UserID,
                            Email = email,
                            PrimaryPhone = contact,
                            SecondaryPhone = "",
                            FlightBookingID = lastBookingId + 1
                        };

                        sql.Reservations.Add(flight);
                        sql.SaveChanges();

                        int BookingID = flight.Id;

                        string TicketNum = "";
                        #endregion

                        #region Pax Info

                        int Age = 0;
                        string paxType = "";
                        for (int i = 0; i < Adults; i++)
                        {
                            bool sex1 = false;
                            DateTime _age = Convert.ToDateTime(frm?["dob_" + i.ToString()]?.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(frm?["expiry_" + i.ToString()]);
                            sex = frm?["Gender_" + i.ToString()];
                            document = frm?["document_" + i.ToString()]?.ToString().ToUpper();
                            documenttype = frm?["documenttype_" + i.ToString()]?.ToString().ToUpper();

                            if (frm?["expiry_" + i.ToString()] != null && frm?["expiry_" + i.ToString()] != "")
                            {
                                expiry = frm?["expiry_" + i.ToString()];
                            }

                            Age = CalculatePaxAge(_age);

                            nationality = frm?["nationality_" + i.ToString()]?.ToString(); // data?.nationality_i?.Value.ToString();

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

                            dob = frm?["dob_" + i.ToString()]?.ToString();
                            title = frm?["title_" + i.ToString()]?.ToString(); // data?.title?.ToUpper();
                            firstName = frm?["firstName_" + i.ToString()]?.ToString(); // data?.firstName_i?.Value.ToString().ToUpper();
                            lastName = frm?["lastName_" + i.ToString()]?.ToString();  // data?.lastName_i?.Value.ToString().ToUpper();
                            TicketNum = "";



                            if (ds.Tables.Contains("Ticket"))
                            {
                                TicketNum = ds.Tables["Ticket"].Rows[i - 1]["TicketNumber"].ToString();
                            }

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
                                DOE = dtExpiry,
                                FlightBookingID = lastBookingId + 1,
                                PassengerStatus = 3,
                                PassegerType = "ADT"
                            };

                            sql.PassengerDetails.Add(passengerDetail);
                            sql.SaveChanges();

                        }

                        for (int i = 1; i <= Infants; i++)
                        {
                            bool sex1 = false;
                            DateTime _age = Convert.ToDateTime(frm?["dob_I" + i.ToString()]?.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(frm?["expiry_I" + i.ToString()]);
                            sex = frm?["Gender_" + i.ToString()];
                            document = frm?["document_I" + i.ToString()]?.ToString().ToUpper();
                            documenttype = frm?["documenttype_I" + i.ToString()]?.ToString().ToUpper();

                            if (frm?["expiry_I" + i.ToString()] != null && frm?["expiry_I" + i.ToString()] != "")
                            {
                                expiry = Convert.ToDateTime(frm?["expiry_I" + i.ToString()]);
                            }

                            Age = CalculatePaxAge(_age);

                            nationality = frm?["nationality_I" + i.ToString()]?.ToString(); // data?.nationality_i?.Value.ToString();

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

                            dob = frm?["dob_I" + i.ToString()]?.ToString();
                            title = frm?["title_I" + i.ToString()]?.ToString(); // data?.title?.ToUpper();
                            firstName = frm?["firstName_I" + i.ToString()]?.ToString(); // data?.firstName_i?.Value.ToString().ToUpper();
                            lastName = frm?["lastName_I" + i.ToString()]?.ToString();  // data?.lastName_i?.Value.ToString().ToUpper();
                            TicketNum = "";



                            if (ds.Tables.Contains("Ticket"))
                            {
                                TicketNum = ds.Tables["Ticket"].Rows[i - 1]["TicketNumber"].ToString();
                            }
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
                                DOE = dtExpiry,
                                FlightBookingID = lastBookingId + 1,
                                PassengerStatus = 3,
                                PassegerType = "INF"

                            };

                            sql.PassengerDetails.Add(passengerDetail);
                            sql.SaveChanges();

                        }

                        for (int i = 1; i <= Children; i++)
                        {
                            bool sex1 = false;
                            DateTime _age = Convert.ToDateTime(frm?["dob_C" + i.ToString()]?.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(frm?["expiry_C" + i.ToString()]);
                            sex = frm?["Gender_C" + i.ToString()];
                            document = frm?["document_C" + i.ToString()]?.ToString().ToUpper();
                            documenttype = frm?["documenttype_C" + i.ToString()]?.ToString().ToUpper();

                            if (frm?["expiry_C" + i.ToString()] != null && frm?["expiry_C" + i.ToString()] != "")
                            {
                                expiry = Convert.ToDateTime(frm?["expiry_" + i.ToString()]);
                            }

                            Age = CalculatePaxAge(_age);

                            nationality = frm?["nationality_C" + i.ToString()]?.ToString(); // data?.nationality_i?.Value.ToString();

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

                            dob = frm?["dob_C" + i.ToString()]?.ToString();
                            title = frm?["title_C" + i.ToString()]?.ToString(); // data?.title?.ToUpper();
                            firstName = frm?["firstName_C" + i.ToString()]?.ToString(); // data?.firstName_i?.Value.ToString().ToUpper();
                            lastName = frm?["lastName_C" + i.ToString()]?.ToString();  // data?.lastName_i?.Value.ToString().ToUpper();
                            TicketNum = "";


                            if (ds.Tables.Contains("Ticket"))
                            {
                                TicketNum = ds.Tables["Ticket"].Rows[i - 1]["TicketNumber"].ToString();
                            }
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
                                DOE = dtExpiry,
                                FlightBookingID = lastBookingId + 1,
                                PassengerStatus = 3,
                                PassegerType = "CNN"
                            };

                            sql.PassengerDetails.Add(passengerDetail);
                            sql.SaveChanges();

                        }

                        string DeptDate = "", DeptTime = "", DeptFrom = "", ArrvlTo = "";
                        bool IsDepart = false;
                        // int itrCodes = 0;
                        byte segmenttype = 1;
                        string MrktingAirline = "";
                        //  DataTable dtAirSegment = dsHititResponse.Tables["AirSegment"];

                        bool dmFlag = false;
                        bool dcFlag = false;
                        bool rmFlag = false;
                        bool rcFlag = false;

                        if (fType == "OneWay")
                        {
                            segmenttype = 1;
                        }
                        else
                        {
                            segmenttype = 2;
                        }

                        foreach (DataRow segment in dtFlights.Rows)
                        {

                            if (flightSegmentIDsForFlightDM != -1 && dmFlag == false)
                            {
                                if (Convert.ToInt32(segment["flightSegment_ID"]) == flightSegmentIDsForFlightDM)
                                {
                                    string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

                                    string AirEquipType = "";
                                    if (dtFlights.Columns.Contains("airEquipTypeModel"))
                                    {
                                        AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
                                    }
                                    bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
                                    DateTime DepDate = Convert.ToDateTime(segment["departureDateTime"]); // segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"].ToString()) : new DateTime();
                                    string DepTime = Convert.ToDateTime(segment["departureDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"]).ToString("hh:mm tt") : "";
                                    DateTime ArrDate = Convert.ToDateTime(segment["arrivalDateTime"]); // segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"].ToString()) : new DateTime();
                                    string ArrTime = Convert.ToDateTime(segment["arrivalDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"]).ToString("hh:mm tt") : "";

                                    string MarketingAirlineCode = "PK";// segment != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string OperatingAirlineCode = "PK"; // segment != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string FlightNumber = segment["flightNumber"].ToString(); //FlightNumber.ToString();
                                    string CompanyShortName = segment != null && segment["code"].ToString() != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string OriginCode = segment["dep_LocationCode"].ToString();  //segment["Origin"].ToString();
                                    string DestinationCode = segment["arr_LocationCode"].ToString(); // segment["Destination"].ToString();
                                                                                                     // string serviceclass = resBookDesigCode.ToString();
                                    int cabin = sql.ServiceClasses.Where(x => x.ClassName == segment["cabin"]).Select(x => x.ServiceClassID).FirstOrDefault(); int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                                    //int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                                    int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
                                    int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
                                    int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

                                    string ArrGMTOffset = segment != null && segment["arrivalDateTimeUTC"].ToString() != null && segment["arrivalDateTimeUTC"].ToString() != null ? segment["arrivalDateTimeUTC"].ToString().Substring(segment["arrivalDateTimeUTC"].ToString().Length - 6) : "";
                                    string DptGMTOffset = segment != null && segment["departureDateTimeUTC"].ToString() != null && segment["departureDateTimeUTC"].ToString() != null ? segment["departureDateTimeUTC"].ToString().Substring(segment["departureDateTimeUTC"].ToString().Length - 6) : "";

                                    if (!IsDepart)
                                    {
                                        IsDepart = true;
                                        MrktingAirline = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineName).FirstOrDefault();
                                        DeptFrom = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityName).FirstOrDefault();
                                        ArrvlTo = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityName).FirstOrDefault();

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
                                        ServiceClassID = cabin,
                                        OriginID = Origin,
                                        DestinationID = Dest,
                                        FlightNumber = FlightNumber,
                                        MarketingAirlineCode = MarketingAirlineCode,
                                        OperatingAirlineCode = OperatingAirlineCode,
                                        MarketingAirlineID = MarketingAirlineID,
                                        OperatingAirlineID = OperatingAirlineID,
                                        OperatingAirlineName = CompanyShortName,
                                        FlightBookingID = lastBookingId + 1,
                                        AirEquipType = AirEquipType,
                                        ETicket = ETicket,
                                        ResBookDesigCode = segment["resBookDesigCode"].ToString(),
                                        Baggage = segment["weight"] + " " + segment["unitOfMeasureCode"],
                                        Way = segmenttype
                                    };
                                    sql.FlightSegments.Add(flightseg);
                                    sql.SaveChanges();
                                    int FlightSegmentID = flightseg.FlightSegmentID;
                                    dmFlag = true;

                                    //  itrCodes++;
                                }
                            }
                            else if (flightSegmentIDsForFlightDC != -1 && dcFlag == false)
                            {
                                if (Convert.ToInt32(segment["flightSegment_ID"]) == flightSegmentIDsForFlightDC)
                                {
                                    string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

                                    string AirEquipType = "";
                                    if (dtFlights.Columns.Contains("airEquipTypeModel"))
                                    {
                                        AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
                                    }
                                    bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
                                    DateTime DepDate = Convert.ToDateTime(segment["departureDateTime"]); // segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"].ToString()) : new DateTime();
                                    string DepTime = Convert.ToDateTime(segment["departureDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"]).ToString("hh:mm tt") : "";
                                    DateTime ArrDate = Convert.ToDateTime(segment["arrivalDateTime"]); // segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"].ToString()) : new DateTime();
                                    string ArrTime = Convert.ToDateTime(segment["arrivalDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"]).ToString("hh:mm tt") : "";

                                    string MarketingAirlineCode = "PK";// segment != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string OperatingAirlineCode = "PK"; // segment != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string FlightNumber = segment["flightNumber"].ToString(); //FlightNumber.ToString();
                                    string CompanyShortName = segment != null && segment["code"].ToString() != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string OriginCode = segment["dep_LocationCode"].ToString();  //segment["Origin"].ToString();
                                    string DestinationCode = segment["arr_LocationCode"].ToString(); // segment["Destination"].ToString();
                                                                                                     // string serviceclass = resBookDesigCode.ToString();
                                    int cabin = sql.ServiceClasses.Where(x => x.ClassName == segment["cabin"]).Select(x => x.ServiceClassID).FirstOrDefault(); int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                                    // int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                                    int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
                                    int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
                                    int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

                                    string ArrGMTOffset = segment != null && segment["arrivalDateTimeUTC"].ToString() != null && segment["arrivalDateTimeUTC"].ToString() != null ? segment["arrivalDateTimeUTC"].ToString().Substring(segment["arrivalDateTimeUTC"].ToString().Length - 6) : "";
                                    string DptGMTOffset = segment != null && segment["departureDateTimeUTC"].ToString() != null && segment["departureDateTimeUTC"].ToString() != null ? segment["departureDateTimeUTC"].ToString().Substring(segment["departureDateTimeUTC"].ToString().Length - 6) : "";

                                    if (!IsDepart)
                                    {
                                        IsDepart = true;
                                        MrktingAirline = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineName).FirstOrDefault();
                                        DeptFrom = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityName).FirstOrDefault();
                                        ArrvlTo = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityName).FirstOrDefault();

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
                                        ServiceClassID = cabin,
                                        OriginID = Origin,
                                        DestinationID = Dest,
                                        FlightNumber = FlightNumber,
                                        MarketingAirlineCode = MarketingAirlineCode,
                                        OperatingAirlineCode = OperatingAirlineCode,
                                        MarketingAirlineID = MarketingAirlineID,
                                        OperatingAirlineID = OperatingAirlineID,
                                        OperatingAirlineName = CompanyShortName,
                                        FlightBookingID = lastBookingId + 1,
                                        AirEquipType = AirEquipType,
                                        ETicket = ETicket,
                                        ResBookDesigCode = segment["resBookDesigCode"].ToString(),
                                        Baggage = segment["weight"].ToString() + " " + segment["unitOfMeasureCode"].ToString(),
                                        Way = segmenttype
                                    };
                                    sql.FlightSegments.Add(flightseg);
                                    sql.SaveChanges();
                                    int FlightSegmentID = flightseg.FlightSegmentID;

                                    dcFlag = true;
                                    //  itrCodes++;
                                }
                            }
                            else if (flightSegmentIDsForFlightRM != -1 && rmFlag == false)
                            {
                                if (Convert.ToInt32(segment["flightSegment_ID"]) == flightSegmentIDsForFlightRM)
                                {
                                    string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

                                    string AirEquipType = "";
                                    if (dtFlights.Columns.Contains("airEquipTypeModel"))
                                    {
                                        AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
                                    }
                                    bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
                                    DateTime DepDate = Convert.ToDateTime(segment["departureDateTime"]); // segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"].ToString()) : new DateTime();
                                    string DepTime = Convert.ToDateTime(segment["departureDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"]).ToString("hh:mm tt") : "";
                                    DateTime ArrDate = Convert.ToDateTime(segment["arrivalDateTime"]); // segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"].ToString()) : new DateTime();
                                    string ArrTime = Convert.ToDateTime(segment["arrivalDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"]).ToString("hh:mm tt") : "";

                                    string MarketingAirlineCode = "PK";// segment != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string OperatingAirlineCode = "PK"; // segment != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string FlightNumber = segment["flightNumber"].ToString(); //FlightNumber.ToString();
                                    string CompanyShortName = segment != null && segment["code"].ToString() != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string OriginCode = segment["dep_LocationCode"].ToString();  //segment["Origin"].ToString();
                                    string DestinationCode = segment["arr_LocationCode"].ToString(); // segment["Destination"].ToString();
                                                                                                     // string serviceclass = resBookDesigCode.ToString();
                                    int cabin = sql.ServiceClasses.Where(x => x.ClassName == segment["cabin"]).Select(x => x.ServiceClassID).FirstOrDefault(); int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                                    int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
                                    int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
                                    int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

                                    string ArrGMTOffset = segment != null && segment["arrivalDateTimeUTC"].ToString() != null && segment["arrivalDateTimeUTC"].ToString() != null ? segment["arrivalDateTimeUTC"].ToString().Substring(segment["arrivalDateTimeUTC"].ToString().Length - 6) : "";
                                    string DptGMTOffset = segment != null && segment["departureDateTimeUTC"].ToString() != null && segment["departureDateTimeUTC"].ToString() != null ? segment["departureDateTimeUTC"].ToString().Substring(segment["departureDateTimeUTC"].ToString().Length - 6) : "";

                                    if (!IsDepart)
                                    {
                                        IsDepart = true;
                                        MrktingAirline = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineName).FirstOrDefault();
                                        DeptFrom = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityName).FirstOrDefault();
                                        ArrvlTo = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityName).FirstOrDefault();

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
                                        ServiceClassID = cabin,
                                        OriginID = Origin,
                                        DestinationID = Dest,
                                        FlightNumber = FlightNumber,
                                        MarketingAirlineCode = MarketingAirlineCode,
                                        OperatingAirlineCode = OperatingAirlineCode,
                                        MarketingAirlineID = MarketingAirlineID,
                                        OperatingAirlineID = OperatingAirlineID,
                                        OperatingAirlineName = CompanyShortName,
                                        FlightBookingID = lastBookingId + 1,
                                        AirEquipType = AirEquipType,
                                        ETicket = ETicket,
                                        ResBookDesigCode = segment["resBookDesigCode"].ToString(),
                                        Baggage = segment["weight"].ToString() + " " + segment["unitOfMeasureCode"].ToString(),
                                        Way = segmenttype
                                    };
                                    sql.FlightSegments.Add(flightseg);
                                    sql.SaveChanges();
                                    int FlightSegmentID = flightseg.FlightSegmentID;
                                    rmFlag = true;
                                    //  itrCodes++;
                                }
                            }
                            else if (flightSegmentIDsForFlightRC != -1)
                            {
                                if (Convert.ToInt32(segment["flightSegment_ID"]) == flightSegmentIDsForFlightRC)
                                {
                                    // string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

                                    string AirEquipType = "";
                                    if (dtFlights.Columns.Contains("airEquipTypeModel"))
                                    {
                                        AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
                                    }
                                    bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
                                    DateTime DepDate = Convert.ToDateTime(segment["departureDateTime"]); // segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"].ToString()) : new DateTime();
                                    string DepTime = Convert.ToDateTime(segment["departureDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"]).ToString("hh:mm tt") : "";
                                    DateTime ArrDate = Convert.ToDateTime(segment["arrivalDateTime"]); // segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"].ToString()) : new DateTime();
                                    string ArrTime = Convert.ToDateTime(segment["arrivalDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"]).ToString("hh:mm tt") : "";

                                    string MarketingAirlineCode = "PK";// segment != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string OperatingAirlineCode = "PK"; // segment != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string FlightNumber = segment["flightNumber"].ToString(); //FlightNumber.ToString();
                                    string CompanyShortName = segment != null && segment["code"].ToString() != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
                                    string OriginCode = segment["dep_LocationCode"].ToString();  //segment["Origin"].ToString();
                                    string DestinationCode = segment["arr_LocationCode"].ToString(); // segment["Destination"].ToString();
                                                                                                     // string serviceclass = resBookDesigCode.ToString();
                                    int cabin = sql.ServiceClasses.Where(x => x.ClassName == segment["cabin"]).Select(x => x.ServiceClassID).FirstOrDefault();

                                    int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                                    int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
                                    int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
                                    int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

                                    string ArrGMTOffset = segment != null && segment["arrivalDateTimeUTC"].ToString() != null && segment["arrivalDateTimeUTC"].ToString() != null ? segment["arrivalDateTimeUTC"].ToString().Substring(segment["arrivalDateTimeUTC"].ToString().Length - 6) : "";
                                    string DptGMTOffset = segment != null && segment["departureDateTimeUTC"].ToString() != null && segment["departureDateTimeUTC"].ToString() != null ? segment["departureDateTimeUTC"].ToString().Substring(segment["departureDateTimeUTC"].ToString().Length - 6) : "";

                                    if (!IsDepart)
                                    {
                                        IsDepart = true;
                                        MrktingAirline = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineName).FirstOrDefault();
                                        DeptFrom = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityName).FirstOrDefault();
                                        ArrvlTo = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityName).FirstOrDefault();

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
                                        ServiceClassID = cabin,
                                        OriginID = Origin,
                                        DestinationID = Dest,
                                        FlightNumber = FlightNumber,
                                        MarketingAirlineCode = MarketingAirlineCode,
                                        OperatingAirlineCode = OperatingAirlineCode,
                                        MarketingAirlineID = MarketingAirlineID,
                                        OperatingAirlineID = OperatingAirlineID,
                                        OperatingAirlineName = CompanyShortName,
                                        FlightBookingID = lastBookingId + 1,
                                        AirEquipType = AirEquipType,
                                        ETicket = ETicket,
                                        ResBookDesigCode = segment["resBookDesigCode"].ToString(),
                                        Baggage = segment["weight"].ToString() + " " + segment["unitOfMeasureCode"].ToString(),
                                        Way = segmenttype
                                    };
                                    sql.FlightSegments.Add(flightseg);
                                    sql.SaveChanges();
                                    int FlightSegmentID = flightseg.FlightSegmentID;

                                    rcFlag = true;

                                    //itrCodes++;
                                }
                            }
                            //segmenttype++;
                        }

                        if (ds.Tables.Contains("bookingReferenceIDList"))
                        {
                            if (ds.Tables["bookingReferenceIDList"].Rows.Count > 0)
                            {
                                string jsonData = "";


                                //if (frm.Option_Id_r != null && frm.Option_Id_r != "")
                                //{
                                //    string ReturnPNR = HitItReturnBooking(info, lastBookingId + 1);
                                //    jsonData = (lastBookingId + 1).ToString(); //@"{'FlightBookingId':" + (lastBookingId+1) + "}"; //@"{'DepPNR':" + ID + ",'RtnPNR':" + ReturnPNR + "}";
                                //    return jsonData;
                                //}
                                //else
                                //{
                                jsonData = (lastBookingId + 1).ToString(); //@"{'FlightBookingId':" + (lastBookingId + 1) + "}"; //@"{'DepPNR':" + ID + ",'RtnPNR':" + 0 + "}";
                                return jsonData;
                                //}
                                // return "Booking done. PNR: " + ID.ToString() + ds.Tables["ResponseMessage"].Rows[0]["ResponseMessage_Text"].ToString();
                            }
                        }
                        return "something went wrong";
                        #endregion
                    }

                }
                else
                {
                    return "something went wrong";
                }
                return "something went wrong";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [NonAction]
        public async Task<string> HitItDepartureBooking(string req)
        {
            try
            {
                var doc = XDocument.Parse(req);
                DataSet ds = new DataSet();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string url = "https://app.crane.aero/craneota/CraneOTAService";
                //string url = "https://app-stage.crane.aero/craneota/CraneOTAService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                // byte[] bytes;

                //   bytes = System.Text.Encoding.ASCII.GetBytes(HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline));
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz"))); //Test env
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");


                ////Stream requestStream = request.GetRequestStream();
                ////requestStream.Write(bytes, 0, bytes.Length);
                ////requestStream.Close();
                ///
                var xmlReq = doc.ToString(); //HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

                var result1 = await response.Content.ReadAsStringAsync();

                //var result1 = responseReader.ReadToEnd();
                //return result;

                result1 = result1.Replace("universal:", "");
                result1 = result1.Replace("common_v42_0:", "");
                result1 = result1.Replace("air:", "");

                ////Log

                string fileName = @"C:\hallmark\CreateBooking_RT_Req.txt";
                FileInfo fi = new FileInfo(fileName);

                // Check if file already exists. If yes, delete it.
                if (fi.Exists)
                {
                    fi.Delete();
                }

                // Create a new file     
                using (FileStream fs = fi.Create())
                {
                    Byte[] txt = new UTF8Encoding(true).GetBytes(xmlReq);
                    fs.Write(txt, 0, txt.Length);
                }


                fi = null;

                fileName = @"C:\hallmark\CreateBooking_RT_Res.txt";
                fi = new FileInfo(fileName);

                // Check if file already exists. If yes, delete it.
                if (fi.Exists)
                {
                    fi.Delete();
                }

                // Create a new file     
                using (FileStream fs = fi.Create())
                {
                    Byte[] txt = new UTF8Encoding(true).GetBytes(result1);
                    fs.Write(txt, 0, txt.Length);
                }




                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var str1 = response.RequestMessage;
                    var str2 = response.Content.Headers;
                    // Get the response


                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(result1);
                    XmlReader xmlReader = new XmlNodeReader(xml);
                    XmlReader reader = doc.CreateReader();


                    // Commnent above 9 lines before publishing

                    //Booking Dataset
                    ds.ReadXml(xmlReader);

                    if (ds.Tables.Count > 0)
                    {
                        if (!ds.Tables.Contains("AirBookingResponse"))
                        {
                            // ViewBag.Message = "Something went wrong."; //ds.Tables["Fault"].Rows[0]["faultstring"].ToString();
                            return "Something went wrong. 2";
                        }
                        else
                        {
                            //  string xmlString = ConvertDataSetToXml(ds);
                            return result1.ToString();
                        }

                        return null;
                    }
                    else
                    {

                        return "Something went wrong. 3";
                    }

                }
                return "Something went wrong. 4";
            }
            catch (Exception ex)
            {
                // throw new Exception;
                return ex.Message.ToString();
            }
        }

        [NonAction]
        private string ConvertDataSetToXml(DataSet dataSet)
        {
            using (StringWriter writer = new StringWriter())
            {
                dataSet.WriteXml(writer, XmlWriteMode.WriteSchema);
                return writer.ToString();
            }
        }



        [NonAction]
        public async Task<DataSet> HitItReturnResponse(string req)
        {
            try
            {
                var doc = XDocument.Parse(req);
                DataSet ds = new DataSet();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                string url = "https://app.crane.aero/craneota/CraneOTAService";
                // string url = "https://app-stage.crane.aero/craneota/CraneOTAService"; //Test:

                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(doc.ToString());
                //request1.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz")));
                //request1.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                request1.Headers.Add("ContentType", "text/xml");
                request1.Headers.Add("Method", "Post");
                request1.Headers.Add("SOAPAction", "");
                request1.Headers.Add("Host", "app.crane.aero");
                request1.Headers.Add("KeepAlive", "true");

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

                    ////Log

                    //string fileName = @"F:\HITITCS\HallMark\Test_Cases\CreateBooking_OW_Req.txt";
                    //FileInfo fi = new FileInfo(fileName);

                    //// Check if file already exists. If yes, delete it.
                    //if (fi.Exists)
                    //{
                    //    fi.Delete();
                    //}

                    //// Create a new file     
                    //using (FileStream fs = fi.Create())
                    //{
                    //    Byte[] txt = new UTF8Encoding(true).GetBytes(req);
                    //    fs.Write(txt, 0, txt.Length);
                    //}

                    //// Write file contents on console.     
                    //using (StreamReader sr = System.IO.File.OpenText(fileName))
                    //{
                    //    string s = "";
                    //    while ((s = sr.ReadLine()) != null)
                    //    {
                    //        Console.WriteLine(s);
                    //    }
                    //}

                    //fi = null;

                    //fileName = @"F:\HITITCS\HallMark\Test_Cases\CreateBooking_OW_Res.txt";
                    //fi = new FileInfo(fileName);

                    //// Check if file already exists. If yes, delete it.
                    //if (fi.Exists)
                    //{
                    //    fi.Delete();
                    //}

                    //// Create a new file     
                    //using (FileStream fs = fi.Create())
                    //{
                    //    Byte[] txt = new UTF8Encoding(true).GetBytes(result1);
                    //    fs.Write(txt, 0, txt.Length);
                    //}

                    //// Write file contents on console.     
                    //using (StreamReader sr = System.IO.File.OpenText(fileName))
                    //{
                    //    string s = "";
                    //    while ((s = sr.ReadLine()) != null)
                    //    {
                    //        Console.WriteLine(s);
                    //    }
                    //}

                    // Commnent above 9 lines before publishing

                    //Booking Dataset
                    ds.ReadXml(xmlReader);

                    if (ds.Tables.Count > 0)
                    {
                        if (!ds.Tables.Contains("AirBookingResponse"))
                        {
                            // ViewBag.Message = "Something went wrong."; //ds.Tables["Fault"].Rows[0]["faultstring"].ToString();
                            return null;
                        }
                        else
                        {
                            return ds;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("HitItFlightsForBooking")]
        public async Task<DataSet> HitItFlightsForBooking(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr,
        string fromM_1, string fromM_2, string fromM_3, string fromM_4, string fromM_5, string fromM_6,
        string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string airline = "")
        {
            try
            {
                //7H13D7 booking
                //Session["HitITAirSegments"] = null;
                //Session["dtFareList"] = null;
                //Session["dtCityInfo"] = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string url = "https://app.crane.aero/craneota/CraneOTAService";
                //string url = "https://app-stage.crane.aero/craneota/CraneOTAService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                // byte[] bytes;

                //   bytes = System.Text.Encoding.ASCII.GetBytes(HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline));
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz"))); //Test env
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");


                ////Stream requestStream = request.GetRequestStream();
                ////requestStream.Write(bytes, 0, bytes.Length);
                ////requestStream.Close();
                ///
                var xmlReq = HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

                //HttpWebResponse response;
                //response = (HttpWebResponse)request.GetResponse();
                //Stream responseStream;
                //TextReader responseStr;
                //DataTable FlightSegments = null;
                //DataTable dt;
                DataSet ds = new DataSet();

                //if (Session["AirSegments"] != null)
                //{
                //    Session["AirSegments"] = null;
                //}
                //  MalikExpressMVC.Models.Galileo.Envelope obj;
                // MalikExpressMVC.Models.Galileo.LowFareSearchRspObj rsp;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // responseStream = response.GetResponseStream();
                    //  responseStr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                    // var responseReader = new StreamReader(responseStream);
                    // var responsestr1 = new StreamReader(responseStream).ReadToEnd();

                    var str1 = response.RequestMessage;
                    var str2 = response.Content.Headers;
                    // Get the response
                    var customerJsonString = await response.Content.ReadAsStringAsync();

                    // DataSet ds = new DataSet();

                    DataTable FlightSegments = null;
                    DataTable dt;

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(custome‌​rJsonString);
                    XmlReader xmlReader = new XmlNodeReader(xml);
                    ds.ReadXml(xmlReader);

                    return ds;

                }
                return null;
            }
            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message;
                return null; // ex.Message.ToString();// "something went wrong";
            }
        }

        [HttpGet("HitItflightsreturnsforbooking")]
        public async Task<DataSet> HitItFlightsReturnsForBooking(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr,
            string fromM_1, string fromM_2, string fromM_3, string fromM_4, string fromM_5, string fromM_6,
            string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string airline = "")
        {
            try
            {
                //Session["HitITAirSegments"] = null;
                //Session["dtFareList"] = null;
                //Session["dtCityInfo"] = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string url = "https://app.crane.aero/craneota/CraneOTAService";
                //string url = "https://app-stage.crane.aero/craneota/CraneOTAService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                // byte[] bytes;

                //   bytes = System.Text.Encoding.ASCII.GetBytes(HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline));
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2331053302-8bac8a57" + ":" + "eK&6*5LkGz"))); //Test env
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("Universal API/uAPI2340884402-180f8110" + ":" + "9s}Yc*S6?4"))); //Live Env
                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");


                ////Stream requestStream = request.GetRequestStream();
                ////requestStream.Write(bytes, 0, bytes.Length);
                ////requestStream.Close();
                ///
                var xmlReq = HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

                DataSet ds = new DataSet();

                //if (Session["AirSegments"] != null)
                //{
                //    Session["AirSegments"] = null;
                //}
                //  MalikExpressMVC.Models.Galileo.Envelope obj;
                // MalikExpressMVC.Models.Galileo.LowFareSearchRspObj rsp;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var str1 = response.RequestMessage;
                    var str2 = response.Content.Headers;
                    // Get the response
                    var customerJsonString = await response.Content.ReadAsStringAsync();

                    // DataSet ds = new DataSet();

                    DataTable FlightSegments = null;
                    DataTable dt;

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(custome‌​rJsonString);
                    XmlReader xmlReader = new XmlNodeReader(xml);
                    ds.ReadXml(xmlReader);

                    return ds;
                }
                return null;
            }
            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message;
                return null;
            }
        }

        [HttpGet("userbookings")]
        public async Task<string> UserBookings(string userID)
        {
            try
            {
                //from p in dsHititResponse.Tables["fareComponentList"].AsEnumerable()
                //join t in dsHititResponse.Tables["passengerFareInfoList"].AsEnumerable()
                //on p.Field<int?>("fareComponentList_Id") equals t.Field<int?>("fareComponentList_Id")

                int uID = Convert.ToInt32(userID);
                var reservations = (from x in sql.Reservations
                                    where x.UserID == uID
                                    select new
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
                                        PNR = x.PNR,
                                        FlightBookingID = x.FlightBookingID,
                                        UserID = x.UserID,
                                        DepartureDateTime = x.DepartureDate.ToString(),
                                        ArrivalDateTime = x.ArrivalDate.ToString(),
                                        DepartureCity = x.Origin,
                                        ArrivalCity = x.Destination,
                                        Segments = (from t in sql.FlightSegments
                                                    where t.FlightBookingID == x.FlightBookingID
                                                    select new
                                                    {
                                                        ArrivalDate = t.ArrivalDate,
                                                        ArrivalTime = t.ArrivalTime,
                                                        ArrivalGMTOffset = t.ArrivalGMTOffset,
                                                        DepartureDate = t.DepartureDate,
                                                        DepartureTime = t.DepartureTime,
                                                        DepartureGMTOffset = t.DepartureGMTOffset,
                                                        ServiceClassID = t.ServiceClassID ?? 0,
                                                        OriginID = t.OriginID ?? 0,
                                                        DestinationID = t.DestinationID ?? 0,
                                                        FlightNumber = t.FlightNumber,
                                                        MarketingAirlineCode = t.MarketingAirlineCode,
                                                        OperatingAirlineCode = t.OperatingAirlineCode,
                                                        MarketingAirlineID = t.MarketingAirlineID ?? 0,
                                                        OperatingAirlineID = t.OperatingAirlineID ?? 0,
                                                        OperatingAirlineName = t.OperatingAirlineName ?? "",
                                                        ETicket = t.ETicket ?? false,
                                                        ResBookDesigCode = t.ResBookDesigCode ?? "",
                                                        Way = t.Way,
                                                        Baggage = t.Baggage ?? "",

                                                    }).ToList()
                                    }).ToList();

                //var fs = sql.FlightSegments.Where(f => f.FlightBookingID == bookingID).Select(f => f.ServiceClassID).FirstOrDefault();


                //var PaxList = (from x in sql.PassengerDetails.AsEnumerable()
                //               where x.FlightBookingID == bookingID
                //               select new PassengerDetail
                //               {
                //                   Title = string.IsNullOrWhiteSpace(x.Title) ? "" : x.Title,
                //                   FirstName = string.IsNullOrWhiteSpace(x.FirstName) ? "" : x.FirstName,
                //                   LastName = string.IsNullOrWhiteSpace(x.LastName) ? "" : x.LastName,
                //                   DOB = x.DOB,
                //                   Gender = x.Gender,
                //                   Document = string.IsNullOrWhiteSpace(x.Document) ? "" : x.Document,
                //                   TicketNo = string.IsNullOrWhiteSpace(x.TicketNo) ? "" : x.TicketNo,
                //                   DocumentType = string.IsNullOrWhiteSpace(x.DocumentType) ? "" : x.DocumentType,
                //                   DOE = x.DOE,
                //                   FlightBookingID = x.FlightBookingID,
                //                   PassengerStatus = x.PassengerStatus,
                //                   PassegerType = x.PassegerType
                //               }).ToList();

                //var PNR = (from x in sql.AirPricingInfoKeys.AsEnumerable()
                //           where x.BookingId == bookingID
                //           select new AirPricingInfoKey2
                //           {
                //               Id = x.Id,
                //               PNR = x.PNR,
                //               PnrForTicket = x.PnrForTicket,
                //               airPricingInfoKey = x.airPricingInfoKey,
                //               BookingId = x.BookingId
                //           }).ToList();


                //var container = new FinalBooking
                //{
                //    Id = uID,
                //    Reservation = reservation,
                //    flightSegments = flightSegments,
                //    PaxList = PaxList,
                //    //PNR = PNR,
                //};

                string jsonString = JsonConvert.SerializeObject(reservations, Newtonsoft.Json.Formatting.Indented);


                return jsonString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("flightbookingdetails")]
        public async Task<string> FlightBookingDetails(string BookingID)
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
                                       PNR = x.PNR,
                                       DepartureDate = x.DepartureDate,
                                       ArrivalDate = x.ArrivalDate,
                                       Origin = x.Origin,
                                       Destination = x.Destination
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
                                          ServiceClassID = x.ServiceClassID ?? 0, // Replace with a default value
                                          OriginID = x.OriginID ?? 0, // Replace with a default value                                          
                                          DestinationID = x.DestinationID ?? 0, // Replace with a default value
                                          FlightNumber = x.FlightNumber,
                                          MarketingAirlineCode = x.MarketingAirlineCode,
                                          OperatingAirlineCode = x.OperatingAirlineCode,
                                          MarketingAirlineID = x.MarketingAirlineID ?? 0, // Replace with a default value
                                          OperatingAirlineID = x.OperatingAirlineID ?? 0, // Replace with a default value
                                          OperatingAirlineName = x.OperatingAirlineName ?? "", // Replace with a default value
                                          FlightBookingID = x.FlightBookingID,
                                          ETicket = x.ETicket ?? false, // Replace with a default value
                                          ResBookDesigCode = x.ResBookDesigCode ?? "", // Replace with a default value
                                          Way = x.Way,
                                          Baggage = x.Baggage ?? "", // Replace with a default value
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

                //var PNR = (from x in sql.AirPricingInfoKeys.AsEnumerable()
                //           where x.BookingId == bookingID
                //           select new AirPricingInfoKey2
                //           {
                //               Id = x.Id,
                //               PNR = x.PNR,
                //               PnrForTicket = x.PnrForTicket,
                //               airPricingInfoKey = x.airPricingInfoKey,
                //               BookingId = x.BookingId
                //           }).ToList();


                var container = new FinalBooking
                {
                    Id = bookingID,
                    Reservation = reservation,
                    flightSegments = flightSegments,
                    PaxList = PaxList,
                    //PNR = PNR,
                };

                string jsonString = JsonConvert.SerializeObject(container, Newtonsoft.Json.Formatting.Indented);


                return jsonString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [NonAction]
        private DataTable CityInfo(DataSet ds)
        {
            var CityInfo = (from p in ds.Tables["cityInfo"].AsEnumerable()
                            join t in ds.Tables["city"].AsEnumerable()
                            on p.Field<int?>("cityInfo_Id") equals t.Field<int?>("cityInfo_Id")
                            join t1 in ds.Tables["country"].AsEnumerable()
                           on p.Field<int?>("cityInfo_Id") equals t1.Field<int?>("cityInfo_Id")
                            select new
                            {
                                cityInfo_Id = p.Field<int?>("cityInfo_Id"),
                                arrivalAirport_Id = p.Field<int?>("arrivalAirport_Id") == null ? 0 : p.Field<int?>("arrivalAirport_Id"),
                                departureAirport_Id = p.Field<int?>("departureAirport_Id"),
                                locationCode = t.Field<string>("locationCode"),
                                locationName = t.Field<string>("locationName"),
                                locationNameLanguage = t.Field<string>("locationNameLanguage"),
                                country_locationCode = t1.Field<string>("locationCode"),
                                country_locationName = t1.Field<string>("locationName"),
                                countrylocationNameLanguage = t1.Field<string>("locationNameLanguage"),
                                country_Id = t1.Field<int?>("country_Id"),
                            }).ToList();

            DataTable dtCityInfo = LINQResultToDataTable(CityInfo);
            return dtCityInfo;
        }

        [NonAction]
        private DataTable ReturnCityInfo(DataSet ds)
        {
            var CityInfo = (from p in ds.Tables["cityInfo"].AsEnumerable()
                            join t in ds.Tables["city"].AsEnumerable()
                            on p.Field<int?>("cityInfo_Id") equals t.Field<int?>("cityInfo_Id")
                            join t1 in ds.Tables["country"].AsEnumerable()
                           on p.Field<int?>("cityInfo_Id") equals t1.Field<int?>("cityInfo_Id")
                            select new
                            {
                                cityInfo_Id = p.Field<int?>("cityInfo_Id"),
                                arrivalAirport_Id = p.Field<int?>("arrivalAirport_Id") == null ? 0 : p.Field<int?>("arrivalAirport_Id"),
                                departureAirport_Id = p.Field<int?>("departureAirport_Id"),
                                locationCode = t.Field<string>("locationCode"),
                                locationName = t.Field<string>("locationName"),
                                locationNameLanguage = t.Field<string>("locationNameLanguage"),
                                country_locationCode = t1.Field<string>("locationCode"),
                                country_locationName = t1.Field<string>("locationName"),
                                countrylocationNameLanguage = t1.Field<string>("locationNameLanguage"),
                                country_Id = t1.Field<int?>("country_Id"),
                            }).ToList();

            DataTable dtCityInfo = LINQResultToDataTable(CityInfo);
            return dtCityInfo;
        }
        [NonAction]
        private DataTable Flights(DataSet ds, DataTable dtFareList, string from, string to, string flightType)
        {
            var bookingClassList = ds.Tables["bookingClassList"].AsEnumerable().Distinct(DataRowComparer.Default);
            DataTable dtBookingClassList = ds.Tables["bookingClassList"].Clone(); //new DataTable();

            // Iterate through the distinct rows and add them to the unique DataTable
            foreach (var row in bookingClassList)
            {
                dtBookingClassList.Rows.Add(row.ItemArray);
            }

            var JoinResult = (from p in ds.Tables["Availability"].AsEnumerable()
                              join t in ds.Tables["availabilityResultList"].AsEnumerable()
                              on p.Field<int?>("Availability_Id") equals t.Field<int?>("Availability_Id")
                              join t1 in ds.Tables["availabilityRouteList"].AsEnumerable()
                              on t.Field<int?>("availabilityResultList_Id") equals t1.Field<int?>("availabilityResultList_Id")
                              join t2 in ds.Tables["availabilityByDateList"].AsEnumerable()
                             on t1.Field<int?>("availabilityRouteList_Id") equals t2.Field<int?>("availabilityRouteList_Id")
                              join t3 in ds.Tables["originDestinationOptionList"].AsEnumerable()
                              on t2.Field<int?>("availabilityByDateList_id") equals t3.Field<int?>("availabilityByDateList_id")
                              join t4 in ds.Tables["fareComponentGroupList"].AsEnumerable()
                              on t3.Field<int?>("originDestinationOptionList_Id") equals t4.Field<int?>("originDestinationOptionList_Id")
                              join t5 in ds.Tables["boundList"].AsEnumerable()
                              on t4.Field<int?>("fareComponentGroupList_Id") equals t5.Field<int?>("fareComponentGroupList_Id")
                              join t6 in ds.Tables["availFlightSegmentList"].AsEnumerable()
                              on t5.Field<int?>("boundList_Id") equals t6.Field<int?>("boundList_Id")
                              join t7 in ds.Tables["bookingClassList"].AsEnumerable()
                              on t6.Field<int?>("availFlightSegmentList_Id") equals t7.Field<int?>("availFlightSegmentList_Id")
                              join t8 in ds.Tables["flightSegment"].AsEnumerable()
                              on t7.Field<int?>("availFlightSegmentList_Id") equals t8.Field<int?>("availFlightSegmentList_Id")
                              join t9 in ds.Tables["airline"].AsEnumerable()
                              on t8.Field<int?>("flightSegment_Id") equals t9.Field<int?>("flightSegment_Id")
                              join t10 in ds.Tables["arrivalAirport"].AsEnumerable()
                              on t9.Field<int?>("flightSegment_Id") equals t10.Field<int?>("flightSegment_Id")
                              join t11 in ds.Tables["departureAirport"].AsEnumerable()
                              on t10.Field<int?>("flightSegment_Id") equals t11.Field<int?>("flightSegment_Id")
                              join t12 in ds.Tables["equipment"].AsEnumerable()
                              on t10.Field<int?>("flightSegment_Id") equals t12.Field<int?>("flightSegment_Id")
                              //join t13 in ds.Tables["flightNotes"].AsEnumerable()
                              //on t10.Field<int?>("flightSegment_Id") equals t13.Field<int?>("flightSegment_Id")
                              select new
                              {
                                  Availability_Id = p.Field<int?>("Availability_Id"),
                                  availabilityResultList_Id = t.Field<int?>("availabilityResultList_Id"),
                                  availabilityRouteList_Id = t1.Field<int?>("availabilityRouteList_Id"),
                                  flexibleFaresOnly = t1.Field<string>("flexibleFaresOnly"),
                                  //dateList = t2.Field<string>("dateList"),
                                  availabilityByDateList_Id = t2.Field<int?>("availabilityByDateList_Id"),
                                  originDestinationOptionList_Id = t3.Field<int?>("originDestinationOptionList_Id"),
                                  fareComponentGroupList_Id = t4.Field<int?>("fareComponentGroupList_Id"),
                                  boundList_Id = t5.Field<int?>("boundList_Id"),
                                  boundCode = t5.Field<string>("boundCode"),
                                  availFlightSegmentList_Id = t6.Field<int?>("availFlightSegmentList_Id"),
                                  addOnSegment = t6.Field<string>("addOnSegment"),
                                  cabin = t7.Field<string>("cabin"),
                                  resBookDesigCode = t7.Field<string>("resBookDesigCode"),
                                  resBookDesigQuantity = t7.Field<string>("resBookDesigQuantity"),
                                  flightSegment_Id = t8.Field<int?>("flightSegment_Id"),
                                  departureDateTime = t8.Field<string>("departureDateTime"),
                                  departureDateTimeUTC = t8.Field<string>("departureDateTimeUTC"),
                                  arrivalDateTime = t8.Field<string>("arrivalDateTime"),
                                  arrivalDateTimeUTC = t8.Field<string>("arrivalDateTimeUTC"),
                                  flightNumber = t8.Field<string>("flightNumber"),
                                  ondControlled = t8.Field<string>("ondControlled"),
                                  sector = t8.Field<string>("sector"),
                                  codeshare = t8.Field<string>("codeshare"),
                                  distance = t8.Field<string>("distance"),
                                  flownMileageQty = t8.Field<string>("flownMileageQty"),
                                  journeyDuration = t8.Field<string>("journeyDuration"),
                                  onTimeRate = t8.Field<string>("onTimeRate"),
                                  secureFlightDataRequired = t8.Field<string>("secureFlightDataRequired"),
                                  stopQuantity = t8.Field<string>("stopQuantity"),
                                  code = t9.Field<string>("code"),
                                  companyFullName = t9.Field<string>("companyFullName"),
                                  companyShortName = t9.Field<string>("companyShortName"),
                                  departureAirport_Id = t11.Field<int?>("departureAirport_Id"),
                                  dep_codeContext = t11.Field<string>("codeContext"),
                                  dep_language = t11.Field<string>("language"),
                                  dep_locationCode = t11.Field<string>("locationCode"),
                                  dep_locationName = t11.Field<string>("locationName"),
                                  dep_terminal = t11.Field<string>("terminal"),
                                  arrivalAirport_Id = t10.Field<int?>("arrivalAirport_Id"),
                                  codeContext = t10.Field<string>("codeContext"),
                                  language = t10.Field<string>("language"),
                                  arr_locationCode = t10.Field<string>("locationCode"),
                                  locationName = t10.Field<string>("locationName"),
                                  terminal = t10.Field<string>("terminal"),
                                  airEquipType = t12.Field<string>("airEquipType"),
                                  airEquipTypeModel = t12.Field<string>("airEquipTypeModel"),
                                  changeofGauge = t12.Field<string>("changeofGauge"),
                                  //deiCode = t13.Field<string>("deiCode"),
                                  //explanation = t13.Field<string>("explanation"),
                                  //note = t13.Field<string>("note"),


                              }).ToList();

            DataTable flights = LINQResultToDataTable(JoinResult);

            DataColumn column = new DataColumn("AdultTotalPrice");
            column.DataType = System.Type.GetType("System.Decimal");

            DataColumn column1 = new DataColumn("AdultBasePrice");
            column1.DataType = System.Type.GetType("System.Decimal");

            DataColumn column2 = new DataColumn("AdultTaxes");
            column2.DataType = System.Type.GetType("System.Decimal");

            DataColumn column3 = new DataColumn("ChildTotalPrice");
            column3.DataType = System.Type.GetType("System.Decimal");

            DataColumn column4 = new DataColumn("ChildBasePrice");
            column4.DataType = System.Type.GetType("System.Decimal");

            DataColumn column5 = new DataColumn("InfantTotalPrice");
            column5.DataType = System.Type.GetType("System.Decimal");

            DataColumn column6 = new DataColumn("InfantBasePrice");
            column6.DataType = System.Type.GetType("System.Decimal");

            DataColumn column7 = new DataColumn("InfantTaxes");
            column7.DataType = System.Type.GetType("System.Decimal");

            DataColumn column8 = new DataColumn("ChildTaxes");
            column8.DataType = System.Type.GetType("System.Decimal");

            //DataColumn arr_locationCode = new DataColumn("arr_locationCode");
            //arr_locationCode.DataType = System.Type.GetType("System.String");

            //DataColumn dep_locationCode = new DataColumn("dep_locationCode");
            //dep_locationCode.DataType = System.Type.GetType("System.String");

            //DataColumn column13 = new DataColumn("departureAirport_Id");
            //column13.DataType = System.Type.GetType("System.Int");

            //DataColumn column14 = new DataColumn("locationCode");
            //column14.DataType = System.Type.GetType("System.String");

            //DataColumn column15 = new DataColumn("locationName");
            //column15.DataType = System.Type.GetType("System.String");

            //DataColumn column16 = new DataColumn("locationNameLanguage");
            //column16.DataType = System.Type.GetType("System.String");

            //DataColumn column17 = new DataColumn("country_locationCode");
            //column17.DataType = System.Type.GetType("System.String");

            //DataColumn column18 = new DataColumn("country_locationName");
            //column18.DataType = System.Type.GetType("System.String");

            //DataColumn column19 = new DataColumn("countrylocationNameLanguage");
            //column19.DataType = System.Type.GetType("System.String");

            //DataColumn column20 = new DataColumn("country_Id");
            //column20.DataType = System.Type.GetType("System.Int");

            flights.Columns.Add(column);
            flights.Columns.Add(column1);
            flights.Columns.Add(column2);
            flights.Columns.Add(column3);
            flights.Columns.Add(column4);
            flights.Columns.Add(column5);
            flights.Columns.Add(column6);
            flights.Columns.Add(column7);
            flights.Columns.Add(column8);
            //flights.Columns.Add(dep_locationCode);
            //                   flights.Columns.Add(arr_locationCode);

            //flights.Columns.Add(column13);
            //flights.Columns.Add(column14);
            //flights.Columns.Add(column15);
            //flights.Columns.Add(column16);
            //flights.Columns.Add(column17);
            //flights.Columns.Add(column19);
            //flights.Columns.Add(column20);

            foreach (DataRow ro in flights.Rows)
            {
                foreach (DataRow ro1 in dtFareList.Rows)
                {
                    if (Convert.ToString(ro["fareComponentGroupList_Id"]) == Convert.ToString(ro1["fareComponentGroupList_Id"]) &&
                        Convert.ToString(ro["resBookDesigCode"]) == Convert.ToString(ro1["resBookDesigCode"]))
                    {
                        if (ro1["passengerType_code"].ToString() == "ADLT")
                        {
                            ro["AdultTotalPrice"] = ro1["totalAmount_value"];
                            ro["AdultBasePrice"] = ro1["equivBaseFare_value"];
                            ro["AdultTaxes"] = ro1["totalTax_value"];
                            ro["ChildTotalPrice"] = 0;
                            ro["ChildBasePrice"] = 0;
                            ro["ChildTaxes"] = 0;
                            ro["InfantTotalPrice"] = 0;
                            ro["InfantBasePrice"] = 0;
                            ro["InfantTaxes"] = 0;
                        }
                        else if (ro1["passengerType_code"].ToString() == "CHLD")
                        {
                            ro["ChildTotalPrice"] = ro1["totalAmount_value"];
                            ro["ChildBasePrice"] = ro1["equivBaseFare_value"];
                            ro["ChildTaxes"] = ro1["totalTax_value"];
                            ro["InfantTotalPrice"] = 0;
                            ro["InfantBasePrice"] = 0;
                            ro["InfantTaxes"] = 0;
                        }
                        else if (ro1["passengerType_code"].ToString() == "INFT")
                        {
                            ro["InfantTotalPrice"] = ro1["totalAmount_value"];
                            ro["InfantBasePrice"] = ro1["equivBaseFare_value"];
                            ro["InfantTaxes"] = ro1["totalTax_value"];
                        }
                    }
                }
            }

            //foreach (DataRow ro in flights.Rows)
            //{
            //    foreach (DataRow ro1 in ds.Tables["departureAirport"].Rows)
            //    {
            //        if (Convert.ToString(ro["flightSegment_Id"]) == Convert.ToString(ro1["flightSegment_Id"]))
            //        {
            //            ro["dep_locationCode"] = ro1["locationCode"];
            //        }
            //    }
            //}

            //foreach (DataRow ro in flights.Rows)
            //{
            //    foreach (DataRow ro1 in ds.Tables["arrivalAirport"].Rows)
            //    {
            //        if (Convert.ToString(ro["flightSegment_Id"]) == Convert.ToString(ro1["flightSegment_Id"]))
            //        {
            //            ro["arr_locationCode"] = ro1["locationCode"];
            //        }
            //    }
            //}


            int optionID = 0;
            flights.Columns.Add("Option_Id");
            flights.Columns.Add("Direct");
            flights.Columns.Add("Flight");


            if (flightType == "ONEWAY")
            {
                foreach (DataRow dr in flights.Rows)
                {
                    if (dr["dep_locationCode"].ToString() == from && dr["arr_locationCode"].ToString() == to)
                    {
                        dr["Direct"] = 1;
                    }
                    else
                    {
                        dr["Direct"] = 0;
                    }
                }

                foreach (DataRow dr in flights.Rows)
                {
                    if (dr["dep_locationCode"].ToString() != from)
                    {
                        dr["Flight"] = "C"; //Connecting Flight
                    }
                    else
                    {
                        dr["Flight"] = "M"; //Main Flight
                    }
                }
            }
            else
            {
                foreach (DataRow dr in flights.Rows)
                {
                    if (dr["dep_locationCode"].ToString() == from && dr["arr_locationCode"].ToString() == to)
                    {
                        dr["Direct"] = 1;
                        dr["Flight"] = "DM"; //Main Flight
                    }
                    else if (dr["dep_locationCode"].ToString() == to && dr["arr_locationCode"].ToString() == from)
                    {
                        dr["Direct"] = 1;
                        dr["Flight"] = "RM"; //Main Flight
                    }
                    else if (dr["dep_locationCode"].ToString() == from && dr["dep_locationCode"].ToString() != to && dr["arr_locationCode"].ToString() != to)
                    {
                        dr["Direct"] = 0;
                        dr["Flight"] = "DM"; //Connecting Flight
                    }
                    else if (dr["dep_locationCode"].ToString() != from && dr["dep_locationCode"].ToString() != to && dr["arr_locationCode"].ToString() == to)
                    {
                        dr["Direct"] = 0;
                        dr["Flight"] = "DC"; //Connecting Flight
                    }
                    else if (dr["dep_locationCode"].ToString() == to && dr["dep_locationCode"].ToString() != from && dr["arr_locationCode"].ToString() != from)
                    {
                        dr["Direct"] = 0;
                        dr["Flight"] = "RM";
                    }
                    else if (dr["dep_locationCode"].ToString() != to && dr["dep_locationCode"].ToString() != from && dr["arr_locationCode"].ToString() == from)
                    {
                        dr["Direct"] = 0;
                        dr["Flight"] = "RC"; //Connecting Flight
                    }
                    else
                    {
                        dr["Direct"] = 0;
                        dr["Flight"] = "DRC";
                    }
                }
            }
            //&& dr["Flight"] = "M"

            foreach (DataRow dr in flights.Rows)
            {
                string cabin = "";
                cabin = dr["Cabin"].ToString();

                if (dr["Option_Id"] == null || dr["Option_Id"].ToString() == "")
                {
                    foreach (DataRow dr1 in flights.Rows)
                    {
                        if (dr1["Flight"].ToString() == "M" && dr1["Cabin"].ToString() == cabin && dr1["Direct"].ToString() == "1")
                        {
                            dr1["Option_Id"] = optionID;
                            optionID = optionID + 1;
                        }
                        else if (dr1["Flight"].ToString() == "M" && dr1["Cabin"].ToString() == cabin && dr1["Direct"].ToString() == "0")
                        {
                            dr1["Option_Id"] = optionID;

                        }
                        else if (dr1["Flight"].ToString() == "C" && dr1["Cabin"].ToString() == cabin)
                        {
                            dr1["Option_Id"] = optionID;
                            optionID = optionID + 1;
                        }
                    }

                }
            }

            Guid TransactionId = Guid.NewGuid();

            flights.Columns.Add("TransactionId");
            foreach (DataRow dr in flights.Rows)
            {
                dr["TransactionId"] = TransactionId;
            }


            DataColumn column0 = new DataColumn("SegmentID");
            column0.DataType = System.Type.GetType("System.Int32");


            flights.Columns.Add(column0);
            column0.SetOrdinal(0);

            for (int a = 0; a < flights.Rows.Count; a++)
            {
                flights.Rows[a]["SegmentID"] = a;
            }

            DataView dv = flights.DefaultView;
            dv.Sort = "AdultTotalPrice";
            flights = dv.ToTable();

            return flights;
        }

        private DataTable ReturnFlights(DataSet ds, DataTable dtFareList, string from, string to, string flightType, string Dept, string Arr)
        {
            DataView dvData = new DataView(ds.Tables["bookingClassList"]);
            dvData.Sort = "cabin";
            DataTable bookingClassList = dvData.ToTable(true, "availFlightSegmentList_id", "cabin", "resBookDesigCode", "resBookDesigQuantity");

            DataView dvflightSegment = new DataView(ds.Tables["flightSegment"]);
            dvflightSegment.Sort = "flightSegment_Id";
            DataTable dtflightSegment = dvflightSegment.ToTable(true, "availFlightSegmentList_id", "flightSegment_Id", "departureDateTime", "departureDateTimeUTC", "arrivalDateTime",
                "arrivalDateTimeUTC", "flightNumber", "ondControlled", "sector", "codeshare", "distance", "flownMileageQty", "journeyDuration",
                "onTimeRate", "secureFlightDataRequired", "stopQuantity");


            DataColumn SegmentGroupID = new DataColumn("SegmentGroupID");
            SegmentGroupID.DataType = System.Type.GetType("System.Int32");
            DataColumn Flight = new DataColumn("Flight");
            Flight.DataType = System.Type.GetType("System.String");

            int segID = 0;

            dtflightSegment.Columns.Add(SegmentGroupID);
            SegmentGroupID.SetOrdinal(0);

            dtflightSegment.Columns.Add(Flight);

            foreach (DataRow dr in dtflightSegment.Rows)
            {
                if (Convert.ToDateTime(dr["departureDateTime"]).ToShortDateString() == Convert.ToDateTime(Dept).ToShortDateString())
                {
                    dr["SegmentGroupID"] = segID;
                    // dr["Flight"] = "M";
                }
                else if (Convert.ToDateTime(dr["departureDateTime"]).ToShortDateString() == Convert.ToDateTime(Arr).ToShortDateString())
                {
                    dr["SegmentGroupID"] = segID;
                    segID = segID + 1;
                    //dr["Flight"] = "R";
                }
                else
                {
                    dr["SegmentGroupID"] = segID;
                    // dr["Flight"] = "C";
                }

            }

            var JoinResult = (from p in ds.Tables["Availability"].AsEnumerable()
                              join t in ds.Tables["availabilityResultList"].AsEnumerable()
                              on p.Field<int?>("Availability_Id") equals t.Field<int?>("Availability_Id")
                              join t1 in ds.Tables["availabilityRouteList"].AsEnumerable()
                              on t.Field<int?>("availabilityResultList_Id") equals t1.Field<int?>("availabilityResultList_Id")
                              join t2 in ds.Tables["availabilityByDateList"].AsEnumerable()
                             on t1.Field<int?>("availabilityRouteList_Id") equals t2.Field<int?>("availabilityRouteList_Id")
                              join t3 in ds.Tables["originDestinationOptionList"].AsEnumerable()
                              on t2.Field<int?>("availabilityByDateList_id") equals t3.Field<int?>("availabilityByDateList_id")
                              join t4 in ds.Tables["fareComponentGroupList"].AsEnumerable()
                              on t3.Field<int?>("originDestinationOptionList_Id") equals t4.Field<int?>("originDestinationOptionList_Id")
                              join t5 in ds.Tables["boundList"].AsEnumerable()
                              on t4.Field<int?>("fareComponentGroupList_Id") equals t5.Field<int?>("fareComponentGroupList_Id")
                              join t6 in ds.Tables["availFlightSegmentList"].AsEnumerable()
                              on t5.Field<int?>("boundList_Id") equals t6.Field<int?>("boundList_Id")
                              join t7 in bookingClassList.AsEnumerable()
                              on t6.Field<int?>("availFlightSegmentList_Id") equals t7.Field<int?>("availFlightSegmentList_Id")
                              join t8 in dtflightSegment.AsEnumerable()
                              on t7.Field<int?>("availFlightSegmentList_Id") equals t8.Field<int?>("availFlightSegmentList_Id")
                              join t9 in ds.Tables["airline"].AsEnumerable()
                              on t8.Field<int?>("flightSegment_Id") equals t9.Field<int?>("flightSegment_Id")
                              join t10 in ds.Tables["arrivalAirport"].AsEnumerable()
                              on t9.Field<int?>("flightSegment_Id") equals t10.Field<int?>("flightSegment_Id")
                              join t11 in ds.Tables["departureAirport"].AsEnumerable()
                              on t10.Field<int?>("flightSegment_Id") equals t11.Field<int?>("flightSegment_Id")
                              join t12 in ds.Tables["equipment"].AsEnumerable()
                              on t8.Field<int?>("flightSegment_Id") equals t12.Field<int?>("flightSegment_Id")
                              //join t13 in ds.Tables["datelist"].AsEnumerable()
                              //on t2.Field<int?>("availabilityByDateList_id") equals t13.Field<int?>("availabilityByDateList_id")
                              select new
                              {
                                  Availability_Id = p.Field<int?>("Availability_Id"),
                                  availabilityResultList_Id = t.Field<int?>("availabilityResultList_Id"),
                                  availabilityRouteList_Id = t1.Field<int?>("availabilityRouteList_Id"),
                                  flexibleFaresOnly = t1.Field<string>("flexibleFaresOnly"),
                                  //dateList = t13.Field<string>("dateList_text"),
                                  //availabilityByDateList_Id = t2.Field<int?>("availabilityByDateList_Id"),
                                  originDestinationOptionList_Id = t3.Field<int?>("originDestinationOptionList_Id"),
                                  fareComponentGroupList_Id = t4.Field<int?>("fareComponentGroupList_Id"),
                                  boundList_Id = t5.Field<int?>("boundList_Id"),
                                  boundCode = t5.Field<string>("boundCode"),
                                  availFlightSegmentList_Id = t6.Field<int?>("availFlightSegmentList_Id"),
                                  addOnSegment = t6.Field<string>("addOnSegment"),
                                  cabin = t7.Field<string>("cabin"),
                                  resBookDesigCode = t7.Field<string>("resBookDesigCode"),
                                  resBookDesigQuantity = t7.Field<string>("resBookDesigQuantity"),
                                  //  SegmentGroupID = t8.Field<int?>("SegmentGroupID"),
                                  Flight = t8.Field<string>("Flight"),
                                  flightSegment_Id = t8.Field<int?>("flightSegment_Id"),
                                  departureDateTime = t8.Field<string>("departureDateTime"),
                                  departureDateTimeUTC = t8.Field<string>("departureDateTimeUTC"),
                                  arrivalDateTime = t8.Field<string>("arrivalDateTime"),
                                  arrivalDateTimeUTC = t8.Field<string>("arrivalDateTimeUTC"),
                                  flightNumber = t8.Field<string>("flightNumber"),
                                  ondControlled = t8.Field<string>("ondControlled"),
                                  sector = t8.Field<string>("sector"),
                                  codeshare = t8.Field<string>("codeshare"),
                                  distance = t8.Field<string>("distance"),
                                  flownMileageQty = t8.Field<string>("flownMileageQty"),
                                  journeyDuration = t8.Field<string>("journeyDuration"),
                                  onTimeRate = t8.Field<string>("onTimeRate"),
                                  secureFlightDataRequired = t8.Field<string>("secureFlightDataRequired"),
                                  stopQuantity = t8.Field<string>("stopQuantity"),
                                  code = t9.Field<string>("code"),
                                  companyFullName = t9.Field<string>("companyFullName"),
                                  companyShortName = t9.Field<string>("companyShortName"),
                                  departureAirport_Id = t11.Field<int?>("departureAirport_Id"),
                                  dep_codeContext = t11.Field<string>("codeContext"),
                                  dep_language = t11.Field<string>("language"),
                                  dep_locationCode = t11.Field<string>("locationCode"),
                                  dep_locationName = t11.Field<string>("locationName"),
                                  dep_terminal = t11.Field<string>("terminal"),
                                  arrivalAirport_Id = t10.Field<int?>("arrivalAirport_Id"),
                                  codeContext = t10.Field<string>("codeContext"),
                                  language = t10.Field<string>("language"),
                                  locationCode = t10.Field<string>("locationCode"),
                                  locationName = t10.Field<string>("locationName"),
                                  terminal = t10.Field<string>("terminal"),
                                  airEquipType = t12.Field<string>("airEquipType"),
                                  airEquipTypeModel = t12.Field<string>("airEquipTypeModel"),
                                  changeofGauge = t12.Field<string>("changeofGauge"),
                                  //deiCode = t13.Field<string>("deiCode"),
                                  //explanation = t13.Field<string>("explanation"),
                                  //note = t13.Field<string>("note"),


                              }).ToList();

            DataTable flights = LINQResultToDataTable(JoinResult);

            DataColumn column = new DataColumn("AdultTotalPrice");
            column.DataType = System.Type.GetType("System.Decimal");

            DataColumn column1 = new DataColumn("AdultBasePrice");
            column1.DataType = System.Type.GetType("System.Decimal");

            DataColumn column2 = new DataColumn("AdultTaxes");
            column2.DataType = System.Type.GetType("System.Decimal");

            DataColumn column3 = new DataColumn("ChildTotalPrice");
            column3.DataType = System.Type.GetType("System.Decimal");

            DataColumn column4 = new DataColumn("ChildBasePrice");
            column4.DataType = System.Type.GetType("System.Decimal");

            DataColumn column5 = new DataColumn("InfantTotalPrice");
            column5.DataType = System.Type.GetType("System.Decimal");

            DataColumn column6 = new DataColumn("InfantBasePrice");
            column6.DataType = System.Type.GetType("System.Decimal");

            DataColumn column7 = new DataColumn("InfantTaxes");
            column7.DataType = System.Type.GetType("System.Decimal");

            DataColumn column8 = new DataColumn("ChildTaxes");
            column8.DataType = System.Type.GetType("System.Decimal");

            //DataColumn arr_locationCode = new DataColumn("arr_locationCode");
            //arr_locationCode.DataType = System.Type.GetType("System.String");

            //DataColumn dep_locationCode = new DataColumn("dep_locationCode");
            //dep_locationCode.DataType = System.Type.GetType("System.String");

            DataColumn totalAmount_value = new DataColumn("totalAmount_value");
            totalAmount_value.DataType = System.Type.GetType("System.Double");
            DataColumn GrandTotal = new DataColumn("GrandTotal");
            GrandTotal.DataType = System.Type.GetType("System.Decimal");

            flights.Columns.Add(totalAmount_value);
            flights.Columns.Add(column);
            flights.Columns.Add(column1);
            flights.Columns.Add(column2);
            flights.Columns.Add(column3);
            flights.Columns.Add(column4);
            flights.Columns.Add(column5);
            flights.Columns.Add(column6);
            flights.Columns.Add(column7);
            flights.Columns.Add(column8);
            flights.Columns.Add(GrandTotal);
            flights.Columns.Add("allowanceType");
            flights.Columns.Add("maxAllowedPieces");
            flights.Columns.Add("unitOfMeasureCode");
            flights.Columns.Add("weight");


            //flights.Columns.Add(dep_locationCode);
            flights.Columns.Add("arr_locationCode");

            foreach (DataRow ro in flights.Rows)
            {
                foreach (DataRow ro1 in dtFareList.Rows)
                {
                    if (Convert.ToString(ro["fareComponentGroupList_Id"]) == Convert.ToString(ro1["fareComponentGroupList_Id"]) &&
                        Convert.ToString(ro["resBookDesigCode"]) == Convert.ToString(ro1["resBookDesigCode"]))
                    {
                        ro["allowanceType"] = ro1["allowanceType"];
                        ro["maxAllowedPieces"] = ro1["maxAllowedPieces"];
                        ro["unitOfMeasureCode"] = ro1["unitOfMeasureCode"];
                        ro["weight"] = ro1["weight"];

                        if (ro1["passengerType_code"].ToString() == "ADLT" && ro["AdultBasePrice"] == DBNull.Value)
                        {
                            ro["totalAmount_value"] = ro1["totalAmount_value"];
                            ro["AdultTotalPrice"] = ro1["totalAmount_value"];
                            ro["AdultBasePrice"] = ro1["equivBaseFare_value"];
                            ro["AdultTaxes"] = ro1["totalTax_value"];
                            ro["ChildTotalPrice"] = 0;
                            ro["ChildBasePrice"] = 0;
                            ro["ChildTaxes"] = 0;
                            ro["InfantTotalPrice"] = 0;
                            ro["InfantBasePrice"] = 0;
                            ro["InfantTaxes"] = 0;
                        }
                        else if (ro1["passengerType_code"].ToString() == "CHLD" && ro["ChildBasePrice"] == DBNull.Value)
                        {
                            ro["totalAmount_value"] = ro1["totalAmount_value"];
                            ro["ChildTotalPrice"] = ro1["totalAmount_value"];
                            ro["ChildBasePrice"] = ro1["equivBaseFare_value"];
                            ro["ChildTaxes"] = ro1["totalTax_value"];
                            ro["InfantTotalPrice"] = 0;
                            ro["InfantBasePrice"] = 0;
                            ro["InfantTaxes"] = 0;
                        }
                        else if (ro1["passengerType_code"].ToString() == "INFT" && ro["InfantBasePrice"] == DBNull.Value)
                        {
                            ro["totalAmount_value"] = ro1["totalAmount_value"];
                            ro["InfantTotalPrice"] = ro1["totalAmount_value"];
                            ro["InfantBasePrice"] = ro1["equivBaseFare_value"];
                            ro["InfantTaxes"] = ro1["totalTax_value"];
                        }
                    }
                }
            }


            foreach (DataRow ro in flights.Rows)
            {
                foreach (DataRow ro1 in ds.Tables["departureAirport"].Rows)
                {
                    if (Convert.ToString(ro["flightSegment_Id"]) == Convert.ToString(ro1["flightSegment_Id"]))
                    {
                        ro["dep_locationCode"] = ro1["locationCode"];
                    }
                }
            }

            foreach (DataRow ro in flights.Rows)
            {
                foreach (DataRow ro1 in ds.Tables["arrivalAirport"].Rows)
                {
                    if (Convert.ToString(ro["flightSegment_Id"]) == Convert.ToString(ro1["flightSegment_Id"]))
                    {
                        ro["arr_locationCode"] = ro1["locationCode"];
                    }
                }
            }

            DataColumn column0 = new DataColumn("SegmentID");
            column0.DataType = System.Type.GetType("System.Int32");


            int optionID = 0;
            flights.Columns.Add("Duration");
            flights.Columns.Add("TotalDuration");
            flights.Columns.Add("Option_Id");
            flights.Columns["Option_Id"].SetOrdinal(0);
            flights.Columns.Add("Direct");
            //flights.Columns.Add("Flight");



            foreach (DataRow dr in flights.Rows)
            {
                string dur = dr["journeyDuration"].ToString();
                string[] d;
                int h = 0;
                int m = 0;

                if (dur.IndexOf("PT") > -1)
                {
                    dur = dur.Replace("PT", "");
                }

                if (dur.IndexOf('H') > -1 && dur.IndexOf('M') > -1)
                {
                    if (dur.IndexOf('M') > -1)
                    {
                        dur = dur.Replace("M", "");
                    }

                    dur = dur.Replace("H", ":");
                    d = dur.Split(":");
                    h = Convert.ToInt32(d[0]);
                    m = Convert.ToInt32(d[1]);
                    h = h * 60;
                    h = h + m;
                }
                else
                {
                    dur = dur.Replace("H", "");
                    h = Convert.ToInt32(dur);
                    h = h * 60;
                }

                dr["Duration"] = h;
                dr["TotalDuration"] = h;
            }

            foreach (DataRow dr in flights.Rows)
            {
                if (dr["dep_locationCode"].ToString() == from && dr["arr_locationCode"].ToString() == to)
                {
                    dr["Direct"] = 1;
                    dr["Flight"] = "DM"; //Main Flight
                }
                else if (dr["dep_locationCode"].ToString() == to && dr["arr_locationCode"].ToString() == from)
                {
                    dr["Direct"] = 1;
                    dr["Flight"] = "RM"; //Main Flight
                }
                else if (dr["dep_locationCode"].ToString() == from && dr["dep_locationCode"].ToString() != to && dr["arr_locationCode"].ToString() != to)
                {
                    dr["Direct"] = 0;
                    dr["Flight"] = "DM"; //Connecting Flight
                }
                else if (dr["dep_locationCode"].ToString() != from && dr["dep_locationCode"].ToString() != to && dr["arr_locationCode"].ToString() == to)
                {
                    dr["Direct"] = 0;
                    dr["Flight"] = "DC"; //Connecting Flight
                }
                else if (dr["dep_locationCode"].ToString() == to && dr["dep_locationCode"].ToString() != from && dr["arr_locationCode"].ToString() != from)
                {
                    dr["Direct"] = 0;
                    dr["Flight"] = "RM";
                }
                else if (dr["dep_locationCode"].ToString() != to && dr["dep_locationCode"].ToString() != from && dr["arr_locationCode"].ToString() == from)
                {
                    dr["Direct"] = 0;
                    dr["Flight"] = "RC"; //Connecting Flight
                }
                else
                {
                    dr["Direct"] = 0;
                    dr["Flight"] = "DRC";
                }
            }

            foreach (DataRow ro in flights.Rows)
            {

                ro["GrandTotal"] = Convert.ToDecimal(ro["AdultTotalPrice"]) + Convert.ToDecimal(ro["ChildTotalPrice"]) +
                                Convert.ToDecimal(ro["InfantTotalPrice"]) + 1000;
            }


            Dictionary<string, int> uniqueCombinations = new Dictionary<string, int>();
            int optionIdCounter = 0;

            foreach (DataRow row in flights.Rows)
            {
                string key = $"{row["fareComponentGroupList_Id"]}\t{row["boundList_Id"]}\t{row["boundCode"]}\t{row["cabin"]}\t{row["Flight"]}\t{row["flightSegment_Id"]}";

                if (!uniqueCombinations.ContainsKey(key))
                {
                    uniqueCombinations[key] = optionIdCounter++;
                }

                row["option_id"] = uniqueCombinations[key]; // Assign the option_id value to the row
            }

            flights.Columns.Add(column0);
            column0.SetOrdinal(0);

            for (int a = 0; a < flights.Rows.Count; a++)
            {
                flights.Rows[a]["SegmentID"] = a;
            }

            DataView dv = flights.DefaultView;
            dv.Sort = "totalAmount_value desc";
            flights = dv.ToTable();

            return flights;
        }
        [NonAction]
        private DataTable FareList(DataSet ds)
        {
            var FareList = (from p in ds.Tables["fareComponentList"].AsEnumerable()
                            join t in ds.Tables["passengerFareInfoList"].AsEnumerable()
                            on p.Field<int?>("fareComponentList_Id") equals t.Field<int?>("fareComponentList_Id")
                            join t1 in ds.Tables["fareInfoList"].AsEnumerable()
                            on t.Field<int?>("passengerFareInfoList_Id") equals t1.Field<int?>("passengerFareInfoList_Id")
                            join t2 in ds.Tables["fareBaggageAllowance"].AsEnumerable()
                            on t1.Field<int?>("fareInfoList_Id") equals t2.Field<int?>("fareInfoList_Id")
                            join t4 in ds.Tables["passengerTypeQuantity"].AsEnumerable()
                            on t.Field<int?>("passengerFareInfoList_Id") equals t4.Field<int?>("passengerFareInfoList_Id")
                            join t5 in ds.Tables["passengerType"].AsEnumerable()
                            on t4.Field<int?>("passengerTypeQuantity_Id") equals t5.Field<int?>("passengerTypeQuantity_Id")
                            join t3 in ds.Tables["maxAllowedWeight"].AsEnumerable()
                            on t2.Field<int?>("fareBaggageAllowance_Id") equals t3.Field<int?>("fareBaggageAllowance_Id")
                            join t6 in ds.Tables["pricingInfo"].AsEnumerable()
                            on t.Field<int?>("passengerFareInfoList_Id") equals t6.Field<int?>("passengerFareInfoList_Id")
                            join t7 in ds.Tables["baseFare"].AsEnumerable()
                            on t6.Field<int?>("pricingInfo_Id") equals t7.Field<int?>("pricingInfo_Id")
                            join t8 in ds.Tables["amount"].AsEnumerable()
                            on t7.Field<int?>("baseFare_Id") equals t8.Field<int?>("baseFare_Id")
                            join t9 in ds.Tables["equivBaseFare"].AsEnumerable()
                            on t6.Field<int?>("pricingInfo_Id") equals t9.Field<int?>("pricingInfo_Id")
                            join t10 in ds.Tables["surcharges"].AsEnumerable()
                            on t6.Field<int?>("pricingInfo_Id") equals t10.Field<int?>("pricingInfo_Id")
                            join t11 in ds.Tables["pricingOverview"].AsEnumerable()
                            on p.Field<int?>("fareComponentList_Id") equals t11.Field<int?>("fareComponentList_Id")
                            join t12 in ds.Tables["totalAmount"].AsEnumerable()
                            on t11.Field<int?>("pricingOverview_Id") equals t12.Field<int?>("pricingOverview_Id")
                            join t13 in ds.Tables["taxes"].AsEnumerable()
                            on t6.Field<int?>("pricingInfo_Id") equals t13.Field<int?>("pricingInfo_Id")
                            join t14 in ds.Tables["taxList"].AsEnumerable()
                            on t13.Field<int?>("taxes_Id") equals t14.Field<int?>("taxes_Id")
                            join t15 in ds.Tables["taxAmount"].AsEnumerable()
                            on t14.Field<int?>("taxList_Id") equals t15.Field<int?>("taxList_Id")
                            join t16 in ds.Tables["totalFare"].AsEnumerable()
                            on t6.Field<int?>("pricingInfo_Id") equals t16.Field<int?>("pricingInfo_Id")
                            join t17 in ds.Tables["totalBaseFare"].AsEnumerable()
                            on t11.Field<int?>("pricingOverview_Id") equals t17.Field<int?>("pricingOverview_Id")
                            join t18 in ds.Tables["totalSurcharge"].AsEnumerable()
                            on t11.Field<int?>("pricingOverview_Id") equals t18.Field<int?>("pricingOverview_Id")
                            join t19 in ds.Tables["totalTax"].AsEnumerable()
                            on t11.Field<int?>("pricingOverview_Id") equals t19.Field<int?>("pricingOverview_Id")
                            select new
                            {
                                fareComponentGroupList_Id = p.Field<int?>("fareComponentGroupList_Id"),
                                fareComponentList_Id = p.Field<int?>("fareComponentList_Id"),
                                fareKind = p.Field<string>("fareKind"),
                                internalID = p.Field<string>("internalID"),
                                maxUsageDate = p.Field<string>("maxUsageDate"),
                                passengerFareInfoList_Id = t1.Field<int?>("passengerFareInfoList_Id"),
                                //    endorsementList = t.Field<string>("endorsementList"),
                                fareInfoList_Id = t1.Field<int?>("fareInfoList_Id"),
                                cabinClassCode = t1.Field<string>("cabinClassCode"),
                                fareGroupName = t1.Field<string>("fareGroupName"),
                                fareReferenceCode = t1.Field<string>("fareReferenceCode"),
                                fareReferenceID = t1.Field<string>("fareReferenceID"),
                                fareReferenceName = t1.Field<string>("fareReferenceName"),
                                flightSegmentSequence = t1.Field<string>("flightSegmentSequence"),
                                notValidAfter = t1.Field<string>("notValidAfter"),
                                notValidBefore = t1.Field<string>("notValidBefore"),
                                resBookDesigCode = t1.Field<string>("resBookDesigCode"),
                                fareBaggageAllowance_Id = t2.Field<int?>("fareBaggageAllowance_Id"),
                                allowanceType = t2.Field<string>("allowanceType"),
                                maxAllowedPieces = t2.Field<string>("maxAllowedPieces"),
                                unitOfMeasureCode = t3.Field<string>("unitOfMeasureCode"),
                                weight = t3.Field<string>("weight"),
                                passengerTypeQuantity_Id = t4.Field<int?>("passengerTypeQuantity_Id"),
                                hasStrecher = t4.Field<string>("hasStrecher"),
                                quantity = t4.Field<string>("quantity"),
                                passengerType_code = t5.Field<string>("code"),
                                pricingInfo_Id = t6.Field<int?>("pricingInfo_Id"),
                                fareBaggageAllowance = t6.Field<string>("fareBaggageAllowance"),
                                fareConstruction = t6.Field<string>("fareConstruction"),
                                discountApplied = t6.Field<string>("discountApplied"),
                                passengerTypeCode = t6.Field<string>("passengerTypeCode"),
                                baseFare_id = t7.Field<int?>("baseFare_id"),
                                amount_Id = t8.Field<int?>("amount_Id"),
                                amount_totalFare_Id = t8.Field<int?>("totalFare_Id"),
                                accountingSign = t8.Field<string>("accountingSign"),
                                mileAmount = t8.Field<string>("mileAmount"),
                                value = t8.Field<string>("value"),
                                equivBaseFare_Id = t9.Field<int?>("equivBaseFare_Id"),
                                equivBaseFare_accountingSign = t9.Field<string>("accountingSign"),
                                equivBaseFare_mileAmount = t9.Field<string>("mileAmount"),
                                equivBaseFare_value = t9.Field<string>("value"),
                                surcharges_Id = t10.Field<int?>("surcharges_Id"),
                                pricingOverview_Id = t11.Field<int?>("pricingOverview_Id"),
                                totalAmount_accountingSign = t12.Field<string>("accountingSign"),
                                totalAmount_totalAmount_Id = t12.Field<int?>("totalAmount_Id"),
                                totalAmount_mileAmount = t12.Field<string>("mileAmount"),
                                totalAmount_value = t12.Field<string>("value"),
                                totalAmount_surcharges_Id = t12.Field<int?>("surcharges_Id"),
                                totalAmount_taxes_Id = t12.Field<int?>("taxes_Id"),
                                taxes_taxes_Id = t13.Field<int?>("taxes_Id"),
                                taxList_Id = t14.Field<int?>("taxList_Id"),
                                paid = t14.Field<string>("paid"),
                                taxCode = t14.Field<string>("taxCode"),
                                taxAmount_taxAmount_Id = t15.Field<int?>("taxAmount_Id"),
                                taxAmount_accountingSign = t15.Field<string>("accountingSign"),
                                taxAmount_mileAmount = t15.Field<string>("mileAmount"),
                                taxAmount_value = t15.Field<string>("value"),
                                totalFare_Id = t16.Field<int?>("totalFare_Id"),
                                totalBaseFare_Id = t17.Field<int?>("totalBaseFare_Id"),
                                totalBaseFare_accountingSign = t17.Field<string>("accountingSign"),
                                totalBaseFare_mileAmount = t17.Field<string>("mileAmount"),
                                totalBaseFare_value = t17.Field<string>("value"),
                                totalSurcharge_Id = t18.Field<int?>("totalSurcharge_Id"),
                                totalSurcharge_accountingSign = t18.Field<string>("accountingSign"),
                                totalSurcharge_mileAmount = t18.Field<string>("mileAmount"),
                                totalSurcharge_value = t18.Field<string>("value"),
                                totalTax_Id = t19.Field<int?>("totalTax_Id"),
                                totalTax_accountingSign = t19.Field<string>("accountingSign"),
                                totalTax_mileAmount = t19.Field<string>("mileAmount"),
                                totalTax_value = t19.Field<string>("value"),
                            }).ToList();

            DataTable dtFareList = LINQResultToDataTable(FareList);
            return dtFareList;
        }

        private DataTable ReturnFareList(DataSet ds)
        {
            var FareList = (from p in ds.Tables["fareComponentList"].AsEnumerable()
                            join t in ds.Tables["passengerFareInfoList"].AsEnumerable()
                            on p.Field<int?>("fareComponentList_Id") equals t.Field<int?>("fareComponentList_Id")
                            join t1 in ds.Tables["fareInfoList"].AsEnumerable()
                            on t.Field<int?>("passengerFareInfoList_Id") equals t1.Field<int?>("passengerFareInfoList_Id")
                            join t2 in ds.Tables["fareBaggageAllowance"].AsEnumerable()
                            on t1.Field<int?>("fareInfoList_Id") equals t2.Field<int?>("fareInfoList_Id")
                            join t4 in ds.Tables["passengerTypeQuantity"].AsEnumerable()
                            on t.Field<int?>("passengerFareInfoList_Id") equals t4.Field<int?>("passengerFareInfoList_Id")
                            join t5 in ds.Tables["passengerType"].AsEnumerable()
                            on t4.Field<int?>("passengerTypeQuantity_Id") equals t5.Field<int?>("passengerTypeQuantity_Id")
                            join t3 in ds.Tables["maxAllowedWeight"].AsEnumerable()
                            on t2.Field<int?>("fareBaggageAllowance_Id") equals t3.Field<int?>("fareBaggageAllowance_Id")
                            join t6 in ds.Tables["pricingInfo"].AsEnumerable()
                            on t.Field<int?>("passengerFareInfoList_Id") equals t6.Field<int?>("passengerFareInfoList_Id")
                            join t7 in ds.Tables["baseFare"].AsEnumerable()
                            on t6.Field<int?>("pricingInfo_Id") equals t7.Field<int?>("pricingInfo_Id")
                            join t8 in ds.Tables["amount"].AsEnumerable()
                            on t7.Field<int?>("baseFare_Id") equals t8.Field<int?>("baseFare_Id")
                            join t9 in ds.Tables["equivBaseFare"].AsEnumerable()
                            on t6.Field<int?>("pricingInfo_Id") equals t9.Field<int?>("pricingInfo_Id")
                            join t10 in ds.Tables["surcharges"].AsEnumerable()
                            on t6.Field<int?>("pricingInfo_Id") equals t10.Field<int?>("pricingInfo_Id")
                            join t11 in ds.Tables["pricingOverview"].AsEnumerable()
                            on p.Field<int?>("fareComponentList_Id") equals t11.Field<int?>("fareComponentList_Id")
                            join t12 in ds.Tables["totalAmount"].AsEnumerable()
                            on t11.Field<int?>("pricingOverview_Id") equals t12.Field<int?>("pricingOverview_Id")
                            join t13 in ds.Tables["taxes"].AsEnumerable()
                            on t6.Field<int?>("pricingInfo_Id") equals t13.Field<int?>("pricingInfo_Id")
                            join t14 in ds.Tables["taxList"].AsEnumerable()
                            on t13.Field<int?>("taxes_Id") equals t14.Field<int?>("taxes_Id")
                            join t15 in ds.Tables["taxAmount"].AsEnumerable()
                            on t14.Field<int?>("taxList_Id") equals t15.Field<int?>("taxList_Id")
                            join t16 in ds.Tables["totalFare"].AsEnumerable()
                            on t6.Field<int?>("pricingInfo_Id") equals t16.Field<int?>("pricingInfo_Id")
                            join t17 in ds.Tables["totalBaseFare"].AsEnumerable()
                            on t11.Field<int?>("pricingOverview_Id") equals t17.Field<int?>("pricingOverview_Id")
                            join t18 in ds.Tables["totalSurcharge"].AsEnumerable()
                            on t11.Field<int?>("pricingOverview_Id") equals t18.Field<int?>("pricingOverview_Id")
                            join t19 in ds.Tables["totalTax"].AsEnumerable()
                            on t11.Field<int?>("pricingOverview_Id") equals t19.Field<int?>("pricingOverview_Id")
                            select new
                            {
                                fareComponentGroupList_Id = p.Field<int?>("fareComponentGroupList_Id"),
                                fareComponentList_Id = p.Field<int?>("fareComponentList_Id"),
                                fareKind = p.Field<string>("fareKind"),
                                internalID = p.Field<string>("internalID"),
                                maxUsageDate = p.Field<string>("maxUsageDate"),
                                passengerFareInfoList_Id = t1.Field<int?>("passengerFareInfoList_Id"),
                                //  endorsementList = t.Field<string>("endorsementList"),
                                fareInfoList_Id = t1.Field<int?>("fareInfoList_Id"),
                                cabinClassCode = t1.Field<string>("cabinClassCode"),
                                fareGroupName = t1.Field<string>("fareGroupName"),
                                fareReferenceCode = t1.Field<string>("fareReferenceCode"),
                                fareReferenceID = t1.Field<string>("fareReferenceID"),
                                fareReferenceName = t1.Field<string>("fareReferenceName"),
                                flightSegmentSequence = t1.Field<string>("flightSegmentSequence"),
                                notValidAfter = t1.Field<string>("notValidAfter"),
                                notValidBefore = t1.Field<string>("notValidBefore"),
                                resBookDesigCode = t1.Field<string>("resBookDesigCode"),
                                fareBaggageAllowance_Id = t2.Field<int?>("fareBaggageAllowance_Id"),
                                allowanceType = t2.Field<string>("allowanceType"),
                                maxAllowedPieces = t2.Field<string>("maxAllowedPieces"),
                                unitOfMeasureCode = t3.Field<string>("unitOfMeasureCode"),
                                weight = t3.Field<string>("weight"),
                                passengerTypeQuantity_Id = t4.Field<int?>("passengerTypeQuantity_Id"),
                                hasStrecher = t4.Field<string>("hasStrecher"),
                                quantity = t4.Field<string>("quantity"),
                                passengerType_code = t5.Field<string>("code"),
                                pricingInfo_Id = t6.Field<int?>("pricingInfo_Id"),
                                fareBaggageAllowance = t6.Field<string>("fareBaggageAllowance"),
                                fareConstruction = t6.Field<string>("fareConstruction"),
                                discountApplied = t6.Field<string>("discountApplied"),
                                passengerTypeCode = t6.Field<string>("passengerTypeCode"),
                                baseFare_id = t7.Field<int?>("baseFare_id"),
                                amount_Id = t8.Field<int?>("amount_Id"),
                                amount_totalFare_Id = t8.Field<int?>("totalFare_Id"),
                                accountingSign = t8.Field<string>("accountingSign"),
                                mileAmount = t8.Field<string>("mileAmount"),
                                value = t8.Field<string>("value"),
                                equivBaseFare_Id = t9.Field<int?>("equivBaseFare_Id"),
                                equivBaseFare_accountingSign = t9.Field<string>("accountingSign"),
                                equivBaseFare_mileAmount = t9.Field<string>("mileAmount"),
                                equivBaseFare_value = t9.Field<string>("value"),
                                surcharges_Id = t10.Field<int?>("surcharges_Id"),
                                pricingOverview_Id = t11.Field<int?>("pricingOverview_Id"),
                                totalAmount_accountingSign = t12.Field<string>("accountingSign"),
                                totalAmount_totalAmount_Id = t12.Field<int?>("totalAmount_Id"),
                                totalAmount_mileAmount = t12.Field<string>("mileAmount"),
                                totalAmount_value = t12.Field<string>("value"),
                                totalAmount_surcharges_Id = t12.Field<int?>("surcharges_Id"),
                                totalAmount_taxes_Id = t12.Field<int?>("taxes_Id"),
                                taxes_taxes_Id = t13.Field<int?>("taxes_Id"),
                                taxList_Id = t14.Field<int?>("taxList_Id"),
                                paid = t14.Field<string>("paid"),
                                taxCode = t14.Field<string>("taxCode"),
                                taxAmount_taxAmount_Id = t15.Field<int?>("taxAmount_Id"),
                                taxAmount_accountingSign = t15.Field<string>("accountingSign"),
                                taxAmount_mileAmount = t15.Field<string>("mileAmount"),
                                taxAmount_value = t15.Field<string>("value"),
                                totalFare_Id = t16.Field<int?>("totalFare_Id"),
                                totalBaseFare_Id = t17.Field<int?>("totalBaseFare_Id"),
                                totalBaseFare_accountingSign = t17.Field<string>("accountingSign"),
                                totalBaseFare_mileAmount = t17.Field<string>("mileAmount"),
                                totalBaseFare_value = t17.Field<string>("value"),
                                totalSurcharge_Id = t18.Field<int?>("totalSurcharge_Id"),
                                totalSurcharge_accountingSign = t18.Field<string>("accountingSign"),
                                totalSurcharge_mileAmount = t18.Field<string>("mileAmount"),
                                totalSurcharge_value = t18.Field<string>("value"),
                                totalTax_Id = t19.Field<int?>("totalTax_Id"),
                                totalTax_accountingSign = t19.Field<string>("accountingSign"),
                                totalTax_mileAmount = t19.Field<string>("mileAmount"),
                                totalTax_value = t19.Field<string>("value"),
                            }).ToList();

            DataTable dtFareList = LINQResultToDataTable(FareList);
            return dtFareList;
        }
        protected string OneWayBookingXML(string strReq1, DataTable Flights, DataTable dtFareList, string resBookDesigCode,
                string deiCode, string explanation, string note, DataTable cityInfo)
        {
            try
            {
                StringBuilder strReq = new StringBuilder();
                //  Guid TraceId = Guid.NewGuid();

                strReq.Append(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:impl='http://impl.soap.ws.crane.hititcs.com/'>");
                strReq.Append("<soapenv:Header/>");
                strReq.Append("<soapenv:Body>");
                strReq.Append("<impl:CreateBooking>");
                strReq.Append("<AirBookingRequest>");

                strReq.Append("<clientInformation>");
                strReq.Append("<clientIP>39.40.40.15</clientIP>");
                strReq.Append("<member>false</member>");
                //strReq.Append("<password>MALIK2050</password>");
                //strReq.Append("<userName>BSP2730099</userName>");
                strReq.Append("<password>Ahmad2090@</password>");
                strReq.Append("<userName>PSA2746366</userName>");
                strReq.Append("<preferredCurrency>PKR</preferredCurrency>");
                strReq.Append("</clientInformation>");


                strReq.Append("<airItinerary>");
                strReq.Append("<adviceCodeSegmentExist/>");
                strReq.Append("<bookOriginDestinationOptions>");
                strReq.Append("<bookOriginDestinationOptionList>");

                //DataRow DepartureCity = null;
                //DataRow ArrivalCity = null;
                

                foreach (DataRow item in Flights.Rows)
                {
                    DataRow fareDetails = (from DataRow drow2 in dtFareList.Rows
                                           where drow2["fareComponentGroupList_Id"].ToString() == item["fareComponentGroupList_Id"].ToString() &&
                                           drow2["resBookDesigCode"].ToString() == item["resBookDesigCode"].ToString()
                                           select drow2).FirstOrDefault();
                    DataRow DepartureCity = (from DataRow drow3 in cityInfo.Rows
                                     where drow3["locationCode"].ToString() == item["dep_locationCode"].ToString()
                                     select drow3).FirstOrDefault();

                    DataRow ArrivalCity = (from DataRow drow4 in cityInfo.Rows
                                   where drow4["locationCode"].ToString() == item["arr_locationCode"].ToString()
                                   select drow4).FirstOrDefault();

                    strReq.Append("<bookFlightSegmentList>");
                    strReq.Append("<actionCode>NN</actionCode>");
                    strReq.Append("<addOnSegment/>");
                    strReq.Append("<bookingClass>");
                    strReq.Append("<cabin>" + item["cabin"].ToString() + "</cabin>");
                    strReq.Append("<resBookDesigCode>" + resBookDesigCode + "</resBookDesigCode>");
                    strReq.Append("<resBookDesigQuantity>" + item["resBookDesigQuantity"].ToString() + "</resBookDesigQuantity>");
                    strReq.Append("</bookingClass>");
                    strReq.Append("<fareInfo>");
                    strReq.Append("<cabinClassCode>" + fareDetails["cabinClassCode"].ToString() + "</cabinClassCode>");
                    strReq.Append("<fareBaggageAllowance>");
                    strReq.Append("<allowanceType>" + fareDetails["allowanceType"].ToString() + "</allowanceType>");
                    strReq.Append("<maxAllowedPieces>0</maxAllowedPieces>");
                    strReq.Append("<maxAllowedWeight>");
                    strReq.Append("<unitOfMeasureCode>" + fareDetails["unitOfMeasureCode"].ToString() + "</unitOfMeasureCode>");
                    strReq.Append("<weight>" + fareDetails["weight"].ToString() + "</weight>");
                    strReq.Append("</maxAllowedWeight>");
                    strReq.Append("</fareBaggageAllowance>");
                    strReq.Append("<fareGroupName>" + fareDetails["fareGroupName"].ToString() + "</fareGroupName>");
                    strReq.Append("<fareReferenceCode>" + fareDetails["fareReferenceCode"].ToString() + "</fareReferenceCode>");
                    strReq.Append("<fareReferenceID>" + fareDetails["fareReferenceID"].ToString() + "</fareReferenceID>");
                    strReq.Append("<fareReferenceName>" + fareDetails["fareReferenceName"].ToString() + "</fareReferenceName>");
                    strReq.Append("<flightSegmentSequence>1</flightSegmentSequence>");
                    strReq.Append("<notValidAfter>" + fareDetails["notValidAfter"].ToString() + "</notValidAfter>");
                    strReq.Append("<notValidBefore>" + fareDetails["notValidBefore"].ToString() + "</notValidBefore>");
                    strReq.Append("<resBookDesigCode>" + resBookDesigCode + "</resBookDesigCode>");
                    strReq.Append("</fareInfo>");
                    strReq.Append("<flightSegment>");
                    strReq.Append("<airline>");
                    strReq.Append("<code>" + item["code"].ToString() + "</code>");
                    strReq.Append("<companyFullName>" + item["companyFullName"].ToString() + "</companyFullName>");
                    strReq.Append("<companyShortName>" + item["companyShortName"].ToString() + "</companyShortName>");
                    strReq.Append("</airline>");
                    strReq.Append("<arrivalAirport>");
                    strReq.Append("<cityInfo>");
                    strReq.Append("<city>");
                    strReq.Append("<locationCode>" + ArrivalCity["locationCode"].ToString() + "</locationCode>");
                    strReq.Append("<locationName>" + ArrivalCity["locationName"].ToString() + "</locationName>");
                    strReq.Append("<locationNameLanguage>" + ArrivalCity["locationNameLanguage"].ToString() + "</locationNameLanguage>");
                    strReq.Append("</city>");
                    strReq.Append("<country>");
                    strReq.Append("<locationCode>" + ArrivalCity["country_locationCode"].ToString() + "</locationCode>");
                    strReq.Append("<locationName>" + ArrivalCity["country_locationName"].ToString() + "</locationName>");
                    strReq.Append("<locationNameLanguage>" + ArrivalCity["countrylocationNameLanguage"].ToString() + "</locationNameLanguage>");
                    strReq.Append("<currency>");
                    strReq.Append("<code>PKR</code>");
                    strReq.Append("</currency>");
                    strReq.Append("</country>");
                    strReq.Append("</cityInfo>");
                    strReq.Append("<codeContext>IATA</codeContext>");
                    strReq.Append("<language>" + ArrivalCity["countrylocationNameLanguage"].ToString() + "</language>");
                    strReq.Append("<locationCode>" + ArrivalCity["locationCode"].ToString() + "</locationCode>");
                    strReq.Append("<locationName>" + ArrivalCity["locationName"].ToString() + "</locationName>");
                    strReq.Append("</arrivalAirport>");
                    strReq.Append("<arrivalDateTime>" + item["arrivalDateTime"].ToString() + "</arrivalDateTime>");
                    strReq.Append("<arrivalDateTimeUTC>" + item["arrivalDateTimeUTC"].ToString() + "</arrivalDateTimeUTC>");                   
                    strReq.Append("<departureAirport>");
                    strReq.Append("<cityInfo>");
                    strReq.Append("<city>");
                    strReq.Append("<locationCode>" + DepartureCity["locationCode"].ToString() + "</locationCode>");
                    strReq.Append("<locationName>" + DepartureCity["locationName"].ToString() + "</locationName>");
                    strReq.Append("<locationNameLanguage>" + DepartureCity["locationNameLanguage"].ToString() + "</locationNameLanguage>");
                    strReq.Append("</city>");
                    strReq.Append("<country>");
                    strReq.Append("<locationCode>" + DepartureCity["country_locationCode"].ToString() + "</locationCode>");
                    strReq.Append("<locationName>" + DepartureCity["country_locationName"].ToString() + "</locationName>");
                    strReq.Append("<locationNameLanguage>" + DepartureCity["countrylocationNameLanguage"].ToString() + "</locationNameLanguage>");
                    strReq.Append("<currency>");
                    strReq.Append("<code>PKR</code>");
                    strReq.Append("</currency>");
                    strReq.Append("</country>");
                    strReq.Append("</cityInfo>");
                    strReq.Append("<codeContext>IATA</codeContext>");
                    strReq.Append("<language>" + DepartureCity["countrylocationNameLanguage"].ToString() + "</language>");
                    strReq.Append("<locationCode>" + DepartureCity["locationCode"].ToString() + "</locationCode>");
                    strReq.Append("<locationName>" + DepartureCity["locationName"].ToString() + "</locationName>");
                    strReq.Append("</departureAirport>");
                    strReq.Append("<departureDateTime>" + item["departureDateTime"].ToString() + "</departureDateTime>");
                    strReq.Append("<departureDateTimeUTC>" + item["departureDateTimeUTC"].ToString() + "</departureDateTimeUTC>");
                    strReq.Append("<flightNumber>" + item["flightNumber"].ToString() + "</flightNumber>");
                    strReq.Append("<ondControlled>false</ondControlled>");
                    strReq.Append("<sector>" + item["sector"].ToString() + "</sector>");
                    strReq.Append("<codeshare>false</codeshare>");
                    strReq.Append("<distance>" + item["distance"].ToString() + "</distance>");
                    strReq.Append("<equipment>");
                    strReq.Append("<airEquipType>" + item["airEquipType"].ToString() + "</airEquipType>");
                    strReq.Append("<airEquipTypeModel>" + item["airEquipTypeModel"].ToString() + "</airEquipTypeModel>");
                    strReq.Append("<changeofGauge>false</changeofGauge>");
                    strReq.Append("</equipment>");
                    strReq.Append("<flightNotes>");
                    strReq.Append("<deiCode>" + deiCode + "</deiCode>");
                    strReq.Append("<explanation>" + explanation + "</explanation>");
                    strReq.Append("<note>" + note + "</note>");
                    strReq.Append("</flightNotes>");
                    strReq.Append("<flownMileageQty>" + item["flownMileageQty"].ToString() + "</flownMileageQty>");
                    strReq.Append("<journeyDuration>" + item["journeyDuration"].ToString() + "</journeyDuration>");
                    strReq.Append("<onTimeRate>" + item["onTimeRate"].ToString() + "</onTimeRate>");
                    strReq.Append("<secureFlightDataRequired>" + item["secureFlightDataRequired"].ToString() + "</secureFlightDataRequired>");
                    strReq.Append("<stopQuantity>" + item["stopQuantity"].ToString() + "</stopQuantity>");
                    strReq.Append("</flightSegment><involuntaryPermissionGiven/><sequenceNumber/>");
                    strReq.Append("</bookFlightSegmentList>");
                }                          
              
                strReq.Append("</bookOriginDestinationOptionList>");


                strReq.Append("</bookOriginDestinationOptions>");

                strReq.Append("</airItinerary>");

                strReq.Append(strReq1);


                strReq.Append("</AirBookingRequest>");
                strReq.Append("</impl:CreateBooking>");
                strReq.Append("</soapenv:Body>");
                strReq.Append("</soapenv:Envelope>");

                return strReq.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        protected string ReturnBookingXML(string strReq1, DataRow item, DataRow fareDetails, DataRow Returnitem, DataRow ReturnfareDetails, DataRow DepartureCity, DataRow ArrivalCity,
                        string resBookDesigCode, string deiCode, string explanation, string note,
                        string resBookDesigCode1, string deiCode1, string explanation1, string note1)
        {
            StringBuilder strReq = new StringBuilder();
            //Guid TraceId = Guid.NewGuid();

            strReq.Append(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:impl='http://impl.soap.ws.crane.hititcs.com/'>");
            strReq.Append("<soapenv:Header/>");
            strReq.Append("<soapenv:Body>");
            strReq.Append("<impl:CreateBooking>");
            strReq.Append("<AirBookingRequest>");

            strReq.Append("<clientInformation>");
            strReq.Append("<clientIP>39.40.40.15</clientIP>");
            strReq.Append("<member>false</member>");
            // strReq.Append("<password>Piac1234</password>");
            //strReq.Append("<password>MALIK2050</password>");
            //strReq.Append("<userName>BSP2730099</userName>");
            strReq.Append("<password>Ahmad2090@</password>");
            strReq.Append("<userName>PSA2746366</userName>");
            strReq.Append("<preferredCurrency>PKR</preferredCurrency>");
            strReq.Append("</clientInformation>");


            strReq.Append("<airItinerary>");
            strReq.Append("<adviceCodeSegmentExist/>");
            strReq.Append("<bookOriginDestinationOptions>");

            strReq.Append("<bookOriginDestinationOptionList>");
            strReq.Append("<bookFlightSegmentList>");
            strReq.Append("<actionCode>NN</actionCode>");
            strReq.Append("<addOnSegment/>");
            strReq.Append("<bookingClass>");
            strReq.Append("<cabin>" + item["cabin"].ToString() + "</cabin>");
            strReq.Append("<resBookDesigCode>" + resBookDesigCode + "</resBookDesigCode>");
            strReq.Append("<resBookDesigQuantity>" + item["resBookDesigQuantity"].ToString() + "</resBookDesigQuantity>");
            strReq.Append("</bookingClass>");
            strReq.Append("<fareInfo>");
            strReq.Append("<cabinClassCode>" + fareDetails["cabinClassCode"].ToString() + "</cabinClassCode>");
            strReq.Append("<fareBaggageAllowance>");
            strReq.Append("<allowanceType>" + fareDetails["allowanceType"].ToString() + "</allowanceType>");
            strReq.Append("<maxAllowedPieces>0</maxAllowedPieces>");
            strReq.Append("<maxAllowedWeight>");
            strReq.Append("<unitOfMeasureCode>" + fareDetails["unitOfMeasureCode"].ToString() + "</unitOfMeasureCode>");
            strReq.Append("<weight>" + fareDetails["weight"].ToString() + "</weight>");
            strReq.Append("</maxAllowedWeight>");
            strReq.Append("</fareBaggageAllowance>");
            strReq.Append("<fareGroupName>" + fareDetails["fareGroupName"].ToString() + "</fareGroupName>");
            strReq.Append("<fareReferenceCode>" + fareDetails["fareReferenceCode"].ToString() + "</fareReferenceCode>");
            strReq.Append("<fareReferenceID>" + fareDetails["fareReferenceID"].ToString() + "</fareReferenceID>");
            strReq.Append("<fareReferenceName>" + fareDetails["fareReferenceName"].ToString() + "</fareReferenceName>");
            strReq.Append("<flightSegmentSequence>1</flightSegmentSequence>");
            strReq.Append("<notValidAfter>" + fareDetails["notValidAfter"].ToString() + "</notValidAfter>");
            strReq.Append("<notValidBefore>" + fareDetails["notValidBefore"].ToString() + "</notValidBefore>");
            strReq.Append("<resBookDesigCode>" + resBookDesigCode + "</resBookDesigCode>");
            strReq.Append("</fareInfo>");
            strReq.Append("<flightSegment>");
            strReq.Append("<airline>");
            strReq.Append("<code>" + item["code"].ToString() + "</code>");
            strReq.Append("<companyFullName>" + item["companyFullName"].ToString() + "</companyFullName>");
            strReq.Append("<companyShortName>" + item["companyShortName"].ToString() + "</companyShortName>");
            strReq.Append("</airline>");
            strReq.Append("<arrivalAirport>");
            strReq.Append("<cityInfo>");
            strReq.Append("<city>");
            strReq.Append("<locationCode>" + ArrivalCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + ArrivalCity["locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + ArrivalCity["locationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("</city>");
            strReq.Append("<country>");
            strReq.Append("<locationCode>" + ArrivalCity["country_locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + ArrivalCity["country_locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + ArrivalCity["countrylocationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("<currency>");
            strReq.Append("<code>PKR</code>");
            strReq.Append("</currency>");
            strReq.Append("</country>");
            strReq.Append("</cityInfo>");
            strReq.Append("<codeContext>IATA</codeContext>");
            strReq.Append("<language>" + ArrivalCity["countrylocationNameLanguage"].ToString() + "</language>");
            strReq.Append("<locationCode>" + ArrivalCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + ArrivalCity["locationName"].ToString() + "</locationName>");
            strReq.Append("</arrivalAirport>");
            strReq.Append("<arrivalDateTime>" + item["arrivalDateTime"].ToString() + "</arrivalDateTime>");
            strReq.Append("<arrivalDateTimeUTC>" + item["arrivalDateTimeUTC"].ToString() + "</arrivalDateTimeUTC>");
            strReq.Append("<departureAirport>");
            strReq.Append("<cityInfo>");
            strReq.Append("<city>");
            strReq.Append("<locationCode>" + DepartureCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + DepartureCity["locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + DepartureCity["locationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("</city>");
            strReq.Append("<country>");
            strReq.Append("<locationCode>" + DepartureCity["country_locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + DepartureCity["country_locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + DepartureCity["countrylocationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("<currency>");
            strReq.Append("<code>PKR</code>");
            strReq.Append("</currency>");
            strReq.Append("</country>");
            strReq.Append("</cityInfo>");
            strReq.Append("<codeContext>IATA</codeContext>");
            strReq.Append("<language>" + DepartureCity["countrylocationNameLanguage"].ToString() + "</language>");
            strReq.Append("<locationCode>" + DepartureCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + DepartureCity["locationName"].ToString() + "</locationName>");
            strReq.Append("</departureAirport>");
            strReq.Append("<departureDateTime>" + item["departureDateTime"].ToString() + "</departureDateTime>");
            strReq.Append("<departureDateTimeUTC>" + item["departureDateTimeUTC"].ToString() + "</departureDateTimeUTC>");
            strReq.Append("<flightNumber>" + item["flightNumber"].ToString() + "</flightNumber>");
            strReq.Append("<ondControlled>false</ondControlled>");
            strReq.Append("<sector>" + item["sector"].ToString() + "</sector>");
            strReq.Append("<codeshare>false</codeshare>");
            strReq.Append("<distance>" + item["distance"].ToString() + "</distance>");
            strReq.Append("<equipment>");
            strReq.Append("<airEquipType>" + item["airEquipType"].ToString() + "</airEquipType>");
            strReq.Append("<airEquipTypeModel>" + item["airEquipTypeModel"].ToString() + "</airEquipTypeModel>");
            strReq.Append("<changeofGauge>false</changeofGauge>");
            strReq.Append("</equipment>");
            strReq.Append("<flightNotes>");
            strReq.Append("<deiCode>" + deiCode + "</deiCode>");
            strReq.Append("<explanation>" + explanation + "</explanation>");
            strReq.Append("<note>" + note + "</note>");
            strReq.Append("</flightNotes>");
            strReq.Append("<flownMileageQty>" + item["flownMileageQty"].ToString() + "</flownMileageQty>");
            strReq.Append("<journeyDuration>" + item["journeyDuration"].ToString() + "</journeyDuration>");
            strReq.Append("<onTimeRate>" + item["onTimeRate"].ToString() + "</onTimeRate>");
            strReq.Append("<secureFlightDataRequired>" + item["secureFlightDataRequired"].ToString() + "</secureFlightDataRequired>");
            strReq.Append("<stopQuantity>" + item["stopQuantity"].ToString() + "</stopQuantity>");
            strReq.Append("</flightSegment><involuntaryPermissionGiven/><sequenceNumber/>");
            strReq.Append("</bookFlightSegmentList>");
            strReq.Append("</bookOriginDestinationOptionList>");
            //Return
            strReq.Append("<bookOriginDestinationOptionList>");
            strReq.Append("<bookFlightSegmentList>");
            strReq.Append("<actionCode>NN</actionCode>");
            strReq.Append("<addOnSegment/>");
            strReq.Append("<bookingClass>");
            strReq.Append("<cabin>" + Returnitem["cabin"].ToString() + "</cabin>");
            strReq.Append("<resBookDesigCode>" + resBookDesigCode1 + "</resBookDesigCode>");
            strReq.Append("<resBookDesigQuantity>" + Returnitem["resBookDesigQuantity"].ToString() + "</resBookDesigQuantity>");
            strReq.Append("</bookingClass>");
            strReq.Append("<fareInfo>");
            strReq.Append("<cabinClassCode>" + ReturnfareDetails["cabinClassCode"].ToString() + "</cabinClassCode>");
            strReq.Append("<fareBaggageAllowance>");
            strReq.Append("<allowanceType>" + ReturnfareDetails["allowanceType"].ToString() + "</allowanceType>");
            strReq.Append("<maxAllowedPieces>0</maxAllowedPieces>");
            strReq.Append("<maxAllowedWeight>");
            strReq.Append("<unitOfMeasureCode>" + ReturnfareDetails["unitOfMeasureCode"].ToString() + "</unitOfMeasureCode>");
            strReq.Append("<weight>" + ReturnfareDetails["weight"].ToString() + "</weight>");
            strReq.Append("</maxAllowedWeight>");
            strReq.Append("</fareBaggageAllowance>");
            strReq.Append("<fareGroupName>" + ReturnfareDetails["fareGroupName"].ToString() + "</fareGroupName>");
            strReq.Append("<fareReferenceCode>" + ReturnfareDetails["fareReferenceCode"].ToString() + "</fareReferenceCode>");
            strReq.Append("<fareReferenceID>" + ReturnfareDetails["fareReferenceID"].ToString() + "</fareReferenceID>");
            strReq.Append("<fareReferenceName>" + ReturnfareDetails["fareReferenceName"].ToString() + "</fareReferenceName>");
            strReq.Append("<flightSegmentSequence>2</flightSegmentSequence>");
            strReq.Append("<notValidAfter>" + ReturnfareDetails["notValidAfter"].ToString() + "</notValidAfter>");
            strReq.Append("<notValidBefore>" + ReturnfareDetails["notValidBefore"].ToString() + "</notValidBefore>");
            strReq.Append("<resBookDesigCode>" + resBookDesigCode1 + "</resBookDesigCode>");
            strReq.Append("</fareInfo>");
            strReq.Append("<flightSegment>");
            strReq.Append("<airline>");
            strReq.Append("<code>" + Returnitem["code"].ToString() + "</code>");
            strReq.Append("<companyFullName>" + Returnitem["companyFullName"].ToString() + "</companyFullName>");
            strReq.Append("<companyShortName>" + Returnitem["companyShortName"].ToString() + "</companyShortName>");
            strReq.Append("</airline>");
            strReq.Append("<arrivalAirport>");
            strReq.Append("<cityInfo>");
            strReq.Append("<city>");
            strReq.Append("<locationCode>" + DepartureCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + DepartureCity["locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + DepartureCity["locationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("</city>");
            strReq.Append("<country>");
            strReq.Append("<locationCode>" + DepartureCity["country_locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + DepartureCity["country_locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + DepartureCity["countrylocationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("<currency>");
            strReq.Append("<code>PKR</code>");
            strReq.Append("</currency>");
            strReq.Append("</country>");
            strReq.Append("</cityInfo>");
            strReq.Append("<codeContext>IATA</codeContext>");
            strReq.Append("<language>" + DepartureCity["countrylocationNameLanguage"].ToString() + "</language>");
            strReq.Append("<locationCode>" + DepartureCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + DepartureCity["locationName"].ToString() + "</locationName>");
            strReq.Append("</arrivalAirport>");
            strReq.Append("<arrivalDateTime>" + Returnitem["arrivalDateTime"].ToString() + "</arrivalDateTime>");
            strReq.Append("<arrivalDateTimeUTC>" + Returnitem["arrivalDateTimeUTC"].ToString() + "</arrivalDateTimeUTC>");
            strReq.Append("<departureAirport>");
            strReq.Append("<cityInfo>");
            strReq.Append("<city>");
            strReq.Append("<locationCode>" + ArrivalCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + ArrivalCity["locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + ArrivalCity["locationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("</city>");
            strReq.Append("<country>");
            strReq.Append("<locationCode>" + ArrivalCity["country_locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + ArrivalCity["country_locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + ArrivalCity["countrylocationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("<currency>");
            strReq.Append("<code>" + Returnitem["code"].ToString() + "</code>");
            strReq.Append("</currency>");
            strReq.Append("</country>");
            strReq.Append("</cityInfo>");
            strReq.Append("<codeContext>IATA</codeContext>");
            strReq.Append("<language>" + ArrivalCity["countrylocationNameLanguage"].ToString() + "</language>");
            strReq.Append("<locationCode>" + ArrivalCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + ArrivalCity["locationName"].ToString() + "</locationName>");
            strReq.Append("</departureAirport>");
            strReq.Append("<departureDateTime>" + Returnitem["departureDateTime"].ToString() + "</departureDateTime>");
            strReq.Append("<departureDateTimeUTC>" + Returnitem["departureDateTimeUTC"].ToString() + "</departureDateTimeUTC>");
            strReq.Append("<flightNumber>" + Returnitem["flightNumber"].ToString() + "</flightNumber>");
            strReq.Append("<ondControlled>false</ondControlled>");
            strReq.Append("<sector>" + Returnitem["sector"].ToString() + "</sector>");
            strReq.Append("<codeshare>false</codeshare>");
            strReq.Append("<distance>" + Returnitem["distance"].ToString() + "</distance>");
            strReq.Append("<equipment>");
            strReq.Append("<airEquipType>" + Returnitem["airEquipType"].ToString() + "</airEquipType>");
            strReq.Append("<airEquipTypeModel>" + Returnitem["airEquipTypeModel"].ToString() + "</airEquipTypeModel>");
            strReq.Append("<changeofGauge>false</changeofGauge>");
            strReq.Append("</equipment>");
            strReq.Append("<flightNotes>");
            strReq.Append("<deiCode>" + deiCode1 + "</deiCode>");
            strReq.Append("<explanation>" + explanation1 + "</explanation>");
            strReq.Append("<note>" + note1 + "</note>");
            strReq.Append("</flightNotes>");
            strReq.Append("<flownMileageQty>" + Returnitem["flownMileageQty"].ToString() + "</flownMileageQty>");
            strReq.Append("<journeyDuration>" + Returnitem["journeyDuration"].ToString() + "</journeyDuration>");
            strReq.Append("<onTimeRate>" + Returnitem["onTimeRate"].ToString() + "</onTimeRate>");
            strReq.Append("<secureFlightDataRequired>" + Returnitem["secureFlightDataRequired"].ToString() + "</secureFlightDataRequired>");
            strReq.Append("<stopQuantity>" + Returnitem["stopQuantity"].ToString() + "</stopQuantity>");
            strReq.Append("</flightSegment><involuntaryPermissionGiven/><sequenceNumber/>");
            strReq.Append("</bookFlightSegmentList>");
            strReq.Append("</bookOriginDestinationOptionList>");

            strReq.Append("</bookOriginDestinationOptions>");

            strReq.Append("</airItinerary>");

            strReq.Append(strReq1);

            strReq.Append("</AirBookingRequest>");
            strReq.Append("</impl:CreateBooking>");
            strReq.Append("</soapenv:Body>");
            strReq.Append("</soapenv:Envelope>");

            return strReq.ToString();
        }

        protected string MultiBookingXML(string strReq1, DataRow item, DataRow fareDetails, string fareComponentGroupList_Id1, DataTable dtHitIt, DataTable dtFareList, DataTable dtCityInfo,
                    DataRow DepartureCity, DataRow ArrivalCity,
                   string resBookDesigCode, string deiCode, string explanation, string note,
                   string resBookDesigCode1, string deiCode1, string explanation1, string note1)
        {
            StringBuilder strReq = new StringBuilder();
            //Guid TraceId = Guid.NewGuid();

            strReq.Append(@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:impl='http://impl.soap.ws.crane.hititcs.com/'>");
            strReq.Append("<soapenv:Header/>");
            strReq.Append("<soapenv:Body>");
            strReq.Append("<impl:CreateBooking>");
            strReq.Append("<AirBookingRequest>");

            strReq.Append("<clientInformation>");
            strReq.Append("<clientIP>39.40.40.15</clientIP>");
            strReq.Append("<member>false</member>");
            //strReq.Append("<password>MALIK2050</password>");
            //strReq.Append("<userName>BSP2730099</userName>");
            strReq.Append("<password>Test12345</password>");
            strReq.Append("<userName>PSA2746366</userName>");
            strReq.Append("<preferredCurrency>PKR</preferredCurrency>");
            strReq.Append("</clientInformation>");


            strReq.Append("<airItinerary>");
            strReq.Append("<adviceCodeSegmentExist/>");
            strReq.Append("<bookOriginDestinationOptions>");
            strReq.Append("<bookOriginDestinationOptionList>");
            strReq.Append("<bookFlightSegmentList>");
            strReq.Append("<actionCode>NN</actionCode>");
            strReq.Append("<addOnSegment/>");
            strReq.Append("<bookingClass>");
            strReq.Append("<cabin>" + item["cabin"].ToString() + "</cabin>");
            strReq.Append("<resBookDesigCode>" + resBookDesigCode + "</resBookDesigCode>");
            strReq.Append("<resBookDesigQuantity>" + item["resBookDesigQuantity"].ToString() + "</resBookDesigQuantity>");
            strReq.Append("</bookingClass>");
            strReq.Append("<fareInfo>");
            strReq.Append("<cabinClassCode>" + fareDetails["cabinClassCode"].ToString() + "</cabinClassCode>");
            strReq.Append("<fareBaggageAllowance>");
            strReq.Append("<allowanceType>" + fareDetails["allowanceType"].ToString() + "</allowanceType>");
            strReq.Append("<maxAllowedPieces>0</maxAllowedPieces>");
            strReq.Append("<maxAllowedWeight>");
            strReq.Append("<unitOfMeasureCode>" + fareDetails["unitOfMeasureCode"].ToString() + "</unitOfMeasureCode>");
            strReq.Append("<weight>" + fareDetails["weight"].ToString() + "</weight>");
            strReq.Append("</maxAllowedWeight>");
            strReq.Append("</fareBaggageAllowance>");
            strReq.Append("<fareGroupName>" + fareDetails["fareGroupName"].ToString() + "</fareGroupName>");
            strReq.Append("<fareReferenceCode>" + fareDetails["fareReferenceCode"].ToString() + "</fareReferenceCode>");
            strReq.Append("<fareReferenceID>" + fareDetails["fareReferenceID"].ToString() + "</fareReferenceID>");
            strReq.Append("<fareReferenceName>" + fareDetails["fareReferenceName"].ToString() + "</fareReferenceName>");
            strReq.Append("<flightSegmentSequence>1</flightSegmentSequence>");
            strReq.Append("<notValidAfter>" + fareDetails["notValidAfter"].ToString() + "</notValidAfter>");
            strReq.Append("<notValidBefore>" + fareDetails["notValidBefore"].ToString() + "</notValidBefore>");
            strReq.Append("<resBookDesigCode>" + resBookDesigCode + "</resBookDesigCode>");
            strReq.Append("</fareInfo>");
            strReq.Append("<flightSegment>");
            strReq.Append("<airline>");
            strReq.Append("<code>" + item["code"].ToString() + "</code>");
            strReq.Append("<companyFullName>" + item["companyFullName"].ToString() + "</companyFullName>");
            strReq.Append("<companyShortName>" + item["companyShortName"].ToString() + "</companyShortName>");
            strReq.Append("</airline>");
            strReq.Append("<arrivalAirport>");
            strReq.Append("<cityInfo>");
            strReq.Append("<city>");
            strReq.Append("<locationCode>" + ArrivalCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + ArrivalCity["locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + ArrivalCity["locationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("</city>");
            strReq.Append("<country>");
            strReq.Append("<locationCode>" + ArrivalCity["country_locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + ArrivalCity["country_locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + ArrivalCity["countrylocationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("<currency>");
            strReq.Append("<code>PKR</code>");
            strReq.Append("</currency>");
            strReq.Append("</country>");
            strReq.Append("</cityInfo>");
            strReq.Append("<codeContext>IATA</codeContext>");
            strReq.Append("<language>" + ArrivalCity["countrylocationNameLanguage"].ToString() + "</language>");
            strReq.Append("<locationCode>" + ArrivalCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + ArrivalCity["locationName"].ToString() + "</locationName>");
            strReq.Append("</arrivalAirport>");
            strReq.Append("<arrivalDateTime>" + item["arrivalDateTime"].ToString() + "</arrivalDateTime>");
            strReq.Append("<arrivalDateTimeUTC>" + item["arrivalDateTimeUTC"].ToString() + "</arrivalDateTimeUTC>");
            strReq.Append("<departureAirport>");
            strReq.Append("<cityInfo>");
            strReq.Append("<city>");
            strReq.Append("<locationCode>" + DepartureCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + DepartureCity["locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + DepartureCity["locationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("</city>");
            strReq.Append("<country>");
            strReq.Append("<locationCode>" + DepartureCity["country_locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + DepartureCity["country_locationName"].ToString() + "</locationName>");
            strReq.Append("<locationNameLanguage>" + DepartureCity["countrylocationNameLanguage"].ToString() + "</locationNameLanguage>");
            strReq.Append("<currency>");
            strReq.Append("<code>PKR</code>");
            strReq.Append("</currency>");
            strReq.Append("</country>");
            strReq.Append("</cityInfo>");
            strReq.Append("<codeContext>IATA</codeContext>");
            strReq.Append("<language>" + DepartureCity["countrylocationNameLanguage"].ToString() + "</language>");
            strReq.Append("<locationCode>" + DepartureCity["locationCode"].ToString() + "</locationCode>");
            strReq.Append("<locationName>" + DepartureCity["locationName"].ToString() + "</locationName>");
            strReq.Append("</departureAirport>");
            strReq.Append("<departureDateTime>" + item["departureDateTime"].ToString() + "</departureDateTime>");
            strReq.Append("<departureDateTimeUTC>" + item["departureDateTimeUTC"].ToString() + "</departureDateTimeUTC>");
            strReq.Append("<flightNumber>" + item["flightNumber"].ToString() + "</flightNumber>");
            strReq.Append("<ondControlled>false</ondControlled>");
            strReq.Append("<sector>" + item["sector"].ToString() + "</sector>");
            strReq.Append("<codeshare>false</codeshare>");
            strReq.Append("<distance>" + item["distance"].ToString() + "</distance>");
            strReq.Append("<equipment>");
            strReq.Append("<airEquipType>" + item["airEquipType"].ToString() + "</airEquipType>");
            strReq.Append("<airEquipTypeModel>" + item["airEquipTypeModel"].ToString() + "</airEquipTypeModel>");
            strReq.Append("<changeofGauge>false</changeofGauge>");
            strReq.Append("</equipment>");
            strReq.Append("<flightNotes>");
            strReq.Append("<deiCode>" + deiCode + "</deiCode>");
            strReq.Append("<explanation>" + explanation + "</explanation>");
            strReq.Append("<note>" + note + "</note>");
            strReq.Append("</flightNotes>");
            strReq.Append("<flownMileageQty>" + item["flownMileageQty"].ToString() + "</flownMileageQty>");
            strReq.Append("<journeyDuration>" + item["journeyDuration"].ToString() + "</journeyDuration>");
            strReq.Append("<onTimeRate>" + item["onTimeRate"].ToString() + "</onTimeRate>");
            strReq.Append("<secureFlightDataRequired>" + item["secureFlightDataRequired"].ToString() + "</secureFlightDataRequired>");
            strReq.Append("<stopQuantity>" + item["stopQuantity"].ToString() + "</stopQuantity>");
            strReq.Append("</flightSegment><involuntaryPermissionGiven/><sequenceNumber/>");
            strReq.Append("</bookFlightSegmentList>");
            strReq.Append("</bookOriginDestinationOptionList>");
            //Others

            var Returnitem = (from DataRow drow1 in dtHitIt.Rows
                              where drow1["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id1.ToString() &&
                              drow1["resBookDesigCode"].ToString() == resBookDesigCode.ToString() && drow1["Flight"].ToString() != "M"
                              select drow1).ToList();

            foreach (var itm in Returnitem)
            {


                DataRow mfareDetails = (from DataRow drow2 in dtFareList.Rows
                                        where drow2["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id1.ToString() &&
                                        drow2["resBookDesigCode"].ToString() == resBookDesigCode.ToString() &&
                                        Convert.ToDateTime(drow2["notValidBefore"]).ToShortDateString() == Convert.ToDateTime(itm["arrivalDateTime"]).ToShortDateString()
                                        select drow2).FirstOrDefault();

                DataRow mDepartureCity = (from DataRow drow3 in dtCityInfo.Rows
                                          where drow3["locationCode"].ToString() == itm["dep_locationcode"].ToString()
                                          select drow3).FirstOrDefault();

                DataRow mArrivalCity = (from DataRow drow4 in dtCityInfo.Rows
                                        where drow4["locationCode"].ToString() == itm["arr_locationcode"].ToString()
                                        select drow4).FirstOrDefault();

                DataRow FlightNotes = (from DataRow drow4 in dtHitIt.Rows
                                       where drow4["flightSegment_Id"].ToString() == itm["arr_locationcode"].ToString()
                                       select drow4).FirstOrDefault();

                strReq.Append("<bookOriginDestinationOptionList>");
                strReq.Append("<bookFlightSegmentList>");
                strReq.Append("<actionCode>NN</actionCode>");
                strReq.Append("<addOnSegment/>");
                strReq.Append("<bookingClass>");
                strReq.Append("<cabin>" + itm["cabin"].ToString() + "</cabin>");
                strReq.Append("<resBookDesigCode>" + resBookDesigCode1 + "</resBookDesigCode>");
                strReq.Append("<resBookDesigQuantity>" + itm["resBookDesigQuantity"].ToString() + "</resBookDesigQuantity>");
                strReq.Append("</bookingClass>");
                strReq.Append("<fareInfo>");
                strReq.Append("<cabinClassCode>" + mfareDetails["cabinClassCode"].ToString() + "</cabinClassCode>");
                strReq.Append("<fareBaggageAllowance>");
                strReq.Append("<allowanceType>" + mfareDetails["allowanceType"].ToString() + "</allowanceType>");
                strReq.Append("<maxAllowedPieces>0</maxAllowedPieces>");
                strReq.Append("<maxAllowedWeight>");
                strReq.Append("<unitOfMeasureCode>" + mfareDetails["unitOfMeasureCode"].ToString() + "</unitOfMeasureCode>");
                strReq.Append("<weight>" + mfareDetails["weight"].ToString() + "</weight>");
                strReq.Append("</maxAllowedWeight>");
                strReq.Append("</fareBaggageAllowance>");
                strReq.Append("<fareGroupName>" + mfareDetails["fareGroupName"].ToString() + "</fareGroupName>");
                strReq.Append("<fareReferenceCode>" + mfareDetails["fareReferenceCode"].ToString() + "</fareReferenceCode>");
                strReq.Append("<fareReferenceID>" + mfareDetails["fareReferenceID"].ToString() + "</fareReferenceID>");
                strReq.Append("<fareReferenceName>" + mfareDetails["fareReferenceName"].ToString() + "</fareReferenceName>");
                strReq.Append("<flightSegmentSequence>2</flightSegmentSequence>");
                strReq.Append("<notValidAfter>" + mfareDetails["notValidAfter"].ToString() + "</notValidAfter>");
                strReq.Append("<notValidBefore>" + mfareDetails["notValidBefore"].ToString() + "</notValidBefore>");
                strReq.Append("<resBookDesigCode>" + resBookDesigCode1 + "</resBookDesigCode>");
                strReq.Append("</fareInfo>");
                strReq.Append("<flightSegment>");
                strReq.Append("<airline>");
                strReq.Append("<code>" + itm["code"].ToString() + "</code>");
                strReq.Append("<companyFullName>" + itm["companyFullName"].ToString() + "</companyFullName>");
                strReq.Append("<companyShortName>" + itm["companyShortName"].ToString() + "</companyShortName>");
                strReq.Append("</airline>");
                strReq.Append("<arrivalAirport>");
                strReq.Append("<cityInfo>");
                strReq.Append("<city>");
                strReq.Append("<locationCode>" + mArrivalCity["locationCode"].ToString() + "</locationCode>");
                strReq.Append("<locationName>" + mArrivalCity["locationName"].ToString() + "</locationName>");
                strReq.Append("<locationNameLanguage>" + mArrivalCity["locationNameLanguage"].ToString() + "</locationNameLanguage>");
                strReq.Append("</city>");
                strReq.Append("<country>");
                strReq.Append("<locationCode>" + mArrivalCity["country_locationCode"].ToString() + "</locationCode>");
                strReq.Append("<locationName>" + mArrivalCity["country_locationName"].ToString() + "</locationName>");
                strReq.Append("<locationNameLanguage>" + mArrivalCity["countrylocationNameLanguage"].ToString() + "</locationNameLanguage>");
                strReq.Append("<currency>");
                strReq.Append("<code>PKR</code>");
                strReq.Append("</currency>");
                strReq.Append("</country>");
                strReq.Append("</cityInfo>");
                strReq.Append("<codeContext>IATA</codeContext>");
                strReq.Append("<language>" + mArrivalCity["countrylocationNameLanguage"].ToString() + "</language>");
                strReq.Append("<locationCode>" + mArrivalCity["locationCode"].ToString() + "</locationCode>");
                strReq.Append("<locationName>" + mArrivalCity["locationName"].ToString() + "</locationName>");
                strReq.Append("</arrivalAirport>");
                strReq.Append("<arrivalDateTime>" + itm["arrivalDateTime"].ToString() + "</arrivalDateTime>");
                strReq.Append("<arrivalDateTimeUTC>" + itm["arrivalDateTimeUTC"].ToString() + "</arrivalDateTimeUTC>");
                strReq.Append("<departureAirport>");
                strReq.Append("<cityInfo>");
                strReq.Append("<city>");
                strReq.Append("<locationCode>" + mDepartureCity["locationCode"].ToString() + "</locationCode>");
                strReq.Append("<locationName>" + mDepartureCity["locationName"].ToString() + "</locationName>");
                strReq.Append("<locationNameLanguage>" + mDepartureCity["locationNameLanguage"].ToString() + "</locationNameLanguage>");
                strReq.Append("</city>");
                strReq.Append("<country>");
                strReq.Append("<locationCode>" + mDepartureCity["country_locationCode"].ToString() + "</locationCode>");
                strReq.Append("<locationName>" + mDepartureCity["country_locationName"].ToString() + "</locationName>");
                strReq.Append("<locationNameLanguage>" + mDepartureCity["countrylocationNameLanguage"].ToString() + "</locationNameLanguage>");
                strReq.Append("<currency>");
                strReq.Append("<code>" + itm["code"].ToString() + "</code>");
                strReq.Append("</currency>");
                strReq.Append("</country>");
                strReq.Append("</cityInfo>");
                strReq.Append("<codeContext>IATA</codeContext>");
                strReq.Append("<language>" + mDepartureCity["countrylocationNameLanguage"].ToString() + "</language>");
                strReq.Append("<locationCode>" + mDepartureCity["locationCode"].ToString() + "</locationCode>");
                strReq.Append("<locationName>" + mDepartureCity["locationName"].ToString() + "</locationName>");
                strReq.Append("</departureAirport>");
                strReq.Append("<departureDateTime>" + itm["departureDateTime"].ToString() + "</departureDateTime>");
                strReq.Append("<departureDateTimeUTC>" + itm["departureDateTimeUTC"].ToString() + "</departureDateTimeUTC>");
                strReq.Append("<flightNumber>" + itm["flightNumber"].ToString() + "</flightNumber>");
                strReq.Append("<ondControlled>false</ondControlled>");
                strReq.Append("<sector>" + itm["sector"].ToString() + "</sector>");
                strReq.Append("<codeshare>false</codeshare>");
                strReq.Append("<distance>" + itm["distance"].ToString() + "</distance>");
                strReq.Append("<equipment>");
                strReq.Append("<airEquipType>" + itm["airEquipType"].ToString() + "</airEquipType>");
                strReq.Append("<airEquipTypeModel>" + itm["airEquipTypeModel"].ToString() + "</airEquipTypeModel>");
                strReq.Append("<changeofGauge>false</changeofGauge>");
                strReq.Append("</equipment>");
                strReq.Append("<flightNotes>");
                strReq.Append("<deiCode>" + deiCode1 + "</deiCode>");
                strReq.Append("<explanation>" + explanation1 + "</explanation>");
                strReq.Append("<note>" + note1 + "</note>");
                strReq.Append("</flightNotes>");
                strReq.Append("<flownMileageQty>" + itm["flownMileageQty"].ToString() + "</flownMileageQty>");
                strReq.Append("<journeyDuration>" + itm["journeyDuration"].ToString() + "</journeyDuration>");
                strReq.Append("<onTimeRate>" + itm["onTimeRate"].ToString() + "</onTimeRate>");
                strReq.Append("<secureFlightDataRequired>" + itm["secureFlightDataRequired"].ToString() + "</secureFlightDataRequired>");
                strReq.Append("<stopQuantity>" + itm["stopQuantity"].ToString() + "</stopQuantity>");
                strReq.Append("</flightSegment><involuntaryPermissionGiven/><sequenceNumber/>");
                strReq.Append("</bookFlightSegmentList>");
                strReq.Append("</bookOriginDestinationOptionList>");
            }

            strReq.Append("</bookOriginDestinationOptions>");

            strReq.Append("</airItinerary>");

            strReq.Append(strReq1);

            strReq.Append("</AirBookingRequest>");
            strReq.Append("</impl:CreateBooking>");
            strReq.Append("</soapenv:Body>");
            strReq.Append("</soapenv:Envelope>");

            return strReq.ToString();
        }

    }
}


//[NonAction]
//public string HitItReturnBooking([FromBody] dynamic info, int FlightBookingID)
//{
//    try
//    {
//        string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
//        var frm = JsonConvert.DeserializeObject<dynamic>(json.ToString());

//        if (frm != null)
//        {
//            DateTime dt = DateTime.Now;
//            //DateTime dtOut;
//            string sex = "M";
//            string title = "";
//            string firstName = "";
//            string lastName = "";
//            string dob = "";
//            string nationality = "";
//            string documenttype = "";
//            string document = "";
//            string expiry = "";
//            string mobile = "";

//            int UserID = 1; // Convert.ToInt32(Session["UserID"]);
//            Random random = new Random();
//            string num = random.Next(9).ToString();


//            //  int UserID = user.intUserID;
//            //data.firstName_0.Value
//            int Adults = frm.Adults.Value != null ? Convert.ToInt32(frm.Adults.Value) : 1;
//            int Children = frm.Children.Value != null ? Convert.ToInt32(frm.Children.Value) : 0;
//            int Infants = frm.Infants.Value != null ? Convert.ToInt32(frm.Infants.Value) : 0;

//            string from = frm.from.Value;
//            string to = frm.to.Value;
//            string totaltime0 = frm.totaltime0.Value;
//            string totaltime1 = frm.totaltime1.Value;
//            string stops0 = frm.stops0.Value;
//            string stops1 = frm.stops1.Value;
//            string amount = frm.amount0.Value;
//            string taxes = frm.taxes0.Value;
//            string total = frm.total0.Value;
//            string currency = frm.currency0.Value;
//            //string serviceclass = ""; //frm.cabin.Value;
//            string optionID = frm.Option_Id_r.Value;
//            //Guid TransactionId = Guid.Parse(frm.TransactionId_r.Value);
//            DateTime ArrivalDateTime = frm.ArrivalDateTime_r.Value;
//            string Departure = frm.Dep_locationCode_r.Value;
//            DateTime DepartureDateTime = frm.DepartureDateTime_r.Value;
//            string Arrival = frm.Arr_locationCode_r.Value;
//            string resBookDesigCode = frm.ResBookDesigCode.Value;
//            string resBookDesigCode1 = frm.ResBookDesigCode_r.Value;
//            string Cabin = frm.Cabin_r.Value;
//            string fareComponentGroupList_Id = frm.FareComponentGroupList_Id_r.Value;
//            string leadfirstname = !string.IsNullOrEmpty(frm.firstName_0.Value) ? frm.firstName_0.Value.ToString().ToUpper() : "";
//            string leadlastname = !string.IsNullOrEmpty(frm.lastName_0.Value) ? frm.lastName_0.Value.ToString().ToUpper() : "";
//            string fromM_1 = "";
//            string fromM_2 = "";
//            string fromM_3 = "";
//            string fromM_4 = "";
//            string fromM_5 = "";
//            string fromM_6 = "";
//            string toM_1 = "";
//            string toM_2 = "";
//            string toM_3 = "";
//            string toM_4 = "";
//            string toM_5 = "";
//            string toM_6 = "";
//            string start = "";
//            string airline = "";


//            string depDT = DepartureDateTime.ToString("MM/dd/yyyy HH:mm:ss");

//            DataSet ds = new DataSet();
//            DataTable dtFlights = new DataTable();
//            DataTable dtFareList = new DataTable();
//            DataTable dtCityInfo = new DataTable();
//            DataTable dtflightNotes = new DataTable();

//            // byte[] hr = sql.HitItResults.Where(x => x.TransactionId == TransactionId).Select(x => x.HititResponse).FirstOrDefault();
//            //var fl = sql.HitItResults.Where(x => x.TransactionId == TransactionId).Select(x => x.FareList).FirstOrDefault();
//            //var ci = sql.HitItResults.Where(x => x.TransactionId == TransactionId).Select(x => x.CityInfo).FirstOrDefault();
//            //var fn = sql.HitItResults.Where(x => x.TransactionId == TransactionId).Select(x => x.FlightNotes).FirstOrDefault();
//            //byte[] finalResult = sql.HitItResults.Where(x => x.TransactionId == TransactionId).Select(x => x.FinalResult).FirstOrDefault();

//            DataSet dsHititResponse = new DataSet();
//            dsHititResponse = HitItFlightsForBooking("OneWay", Departure, Arrival, Adults, Children, Infants, DepartureDateTime.ToString("yyyy-MM-dd"),
//                                            ArrivalDateTime.ToString("yyyy-MM-dd"),
//                                          fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6,
//                                          toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline).Result;

//            // MemoryStream stream = new MemoryStream(hr);

//            // dsHititResponse.read(stream);
//            //   dsHititResponse  = JsonConvert.DeserializeObject<DataSet>(Result);


//            dtCityInfo = CityInfo(dsHititResponse);
//            dtFareList = FareList(dsHititResponse);
//            dtFlights = Flights(dsHititResponse, dtFareList, Departure, Arrival, "OnewAy");

//            var flightSegment_ID = (from f in dtFlights.AsEnumerable().Where(f => f.Field<string?>("Option_Id") == optionID)
//                                    select new { flightSegment_ID = f.Field<int?>("flightSegment_ID") }).FirstOrDefault();

//            string deiCode = "7";
//            string explanation = "Meal Service";
//            string note = "S";

//            if (dsHititResponse.Tables.Contains("flightNotes"))
//            {
//                var FlightNotes = (from n in dsHititResponse.Tables["flightNotes"].AsEnumerable().Where(n => n.Field<int?>("flightSegment_ID") == flightSegment_ID.flightSegment_ID)
//                                   select new
//                                   {
//                                       deiCode = n.Field<string?>("deiCode"),
//                                       explanation = n.Field<string?>("explanation"),
//                                       note = n.Field<string?>("note")
//                                   }).FirstOrDefault();

//                deiCode = FlightNotes.deiCode;
//                explanation = FlightNotes.explanation;
//                note = FlightNotes.note;
//            }

//            string fType = "OneWay";

//            //  string fareComponentGroupList_Id1 = frm.fareComponentGroupList_Id1.Value;
//            //string resBookDesigCode1 = frm.resBookDesigCode_r.Value;

//            string deiCode1 = "7";
//            string explanation1 = "Meal Service";
//            string note1 = "S";

//            if (dsHititResponse.Tables.Contains("flightNotes"))
//            {
//                var FlightNotes = (from n in dsHititResponse.Tables["flightNotes"].AsEnumerable().Where(n => n.Field<int?>("flightSegment_ID") == flightSegment_ID.flightSegment_ID)
//                                   select new
//                                   {
//                                       deiCode = n.Field<string?>("deiCode"),
//                                       explanation = n.Field<string?>("explanation"),
//                                       note = n.Field<string?>("note")
//                                   }).FirstOrDefault();

//                deiCode = FlightNotes.deiCode;
//                explanation = FlightNotes.explanation;
//                note = FlightNotes.note;
//            }


//            string _ticket = ""; // frm.chkTicket.Value;

//            DateTime datet = DateTime.Now.Date;
//            string Dat = datet.ToString("yyyy-MM-dd");

//            DateTime dt1 = DateTime.UtcNow;
//            string tstamp = dt1.ToString("s") + "Z";
//            //string AirlineCode = "PK";
//            //bool error = false;
//            string email = !string.IsNullOrEmpty(frm.email_0.Value) ? frm.email_0.Value.ToString() : "";
//            string contact = !string.IsNullOrEmpty(frm.MobileNO_0.Value) ? frm.MobileNO_0.Value.ToString() : "";

//            //string address = "";
//            //string cityname = "";
//            //string agencyname = "";
//            //string postalcode = "";
//            //string phonenumber = "";
//            //string companyemail = "";
//            //int CustomerID = 0; // Convert.ToInt32(Session["CustomerID"]);

//            #region XML Request

//            string xmlRequest = "";
//            StringBuilder strReq = new StringBuilder();

//            if (Adults > 0)
//            {
//                for (int i = 0; i < Adults; i++)
//                {
//                    DateTime _age = Convert.ToDateTime(frm?["dob_" + i.ToString()]?.ToString());
//                    DateTime dtExpiry = Convert.ToDateTime(frm?["expiry_" + i.ToString()]);
//                    sex = frm?["Gender_" + i.ToString()];
//                    document = frm?["document_" + i.ToString()]?.ToString().ToUpper();
//                    documenttype = frm?["documenttype_" + i.ToString()]?.ToString().ToUpper();

//                    if (frm?["expiry_" + i.ToString()] != null && frm?["expiry_" + i.ToString()] != "")
//                    {
//                        expiry = frm?["expiry_" + i.ToString()];
//                    }


//                    nationality = frm?["nationality_" + i.ToString()]?.ToString(); // data?.nationality_i?.Value.ToString();

//                    dob = frm?["dob_" + i.ToString()]?.ToString();
//                    title = frm?["title_" + i.ToString()]?.ToString(); // data?.title?.ToUpper();
//                    firstName = frm?["firstName_" + i.ToString()]?.ToString().ToUpper(); // data?.firstName_i?.Value.ToString().ToUpper();
//                    lastName = frm?["lastName_" + i.ToString()]?.ToString().ToUpper();  // data?.lastName_i?.Value.ToString().ToUpper();                            
//                    title = frm?["title_" + i.ToString()]?.ToString().ToUpper();
//                    mobile = contact;



//                    strReq.Append("<airTravelerList>");
//                    strReq.Append("<gender>" + sex + "</gender>");
//                    strReq.Append("<hasStrecher/><parentSequence/><passengerTypeCode>ADLT</passengerTypeCode>");
//                    strReq.Append("<personName>");
//                    strReq.Append("<givenName>" + firstName + "</givenName>");
//                    strReq.Append("<shareMarketInd/><surname>" + lastName + "</surname>");
//                    strReq.Append("</personName>");
//                    strReq.Append("<accompaniesqlyInfant/><contactPerson>");
//                    strReq.Append("<email>");
//                    strReq.Append("<email>" + email + "</email>");
//                    strReq.Append("<markedForSendingRezInfo/><preferred/><shareMarketInd>true</shareMarketInd>");
//                    strReq.Append("</email>");
//                    strReq.Append("<markedForSendingRezInfo>true</markedForSendingRezInfo>");
//                    strReq.Append("<phoneNumber>");
//                    strReq.Append("<areaCode>051</areaCode>");
//                    strReq.Append("<countryCode>+92</countryCode>");
//                    strReq.Append("<markedForSendingRezInfo>true</markedForSendingRezInfo>");
//                    strReq.Append("<preferred/><shareMarketInd/><subscriberNumber>" + mobile + "</subscriberNumber>");
//                    strReq.Append("</phoneNumber>");
//                    strReq.Append("<shareMarketInd>true</shareMarketInd>");
//                    strReq.Append("<socialSecurityNumber>" + document + "</socialSecurityNumber><useForInvoicing/>");
//                    strReq.Append("<shareContactInfo>true</shareContactInfo>");
//                    strReq.Append("</contactPerson>");
//                    strReq.Append("<requestedSeatCount>1</requestedSeatCount><shareMarketInd/><unaccompaniedMinor/>");
//                    strReq.Append("</airTravelerList>");
//                }
//            }

//            if (Children > 0)
//            {
//                for (int c = 1; c <= Children; c++)
//                {
//                    sex = frm?["sexC_" + c.ToString()]?.ToString();
//                    document = frm?["documentC_" + c.ToString()]?.ToString();
//                    documenttype = frm?["documenttypeC_" + c.ToString()]?.ToString();
//                    expiry = Convert.ToDateTime(frm?["expiryC_" + c.ToString()]?.ToString("yyyy -MM-dd"));
//                    nationality = frm?["nationalityC_" + c.ToString()]?.ToString();
//                    firstName = frm?["firstNameC_" + c.ToString()]?.ToString();
//                    lastName = frm?["lastNameC_" + c.ToString()]?.ToString();
//                    DateTime dtDOB = Convert.ToDateTime(frm?["dobC_" + c.ToString()]?.ToString());
//                    //TimeSpan t = DateTime.Now - dtDOB;
//                    dob = Convert.ToDateTime(frm?["dobC_" + c.ToString()]?.ToString().ToString("yyyy -MM-dd"));
//                    strReq.Append("<airTravelerList>");
//                    strReq.Append("<gender>" + sex + "</gender>");
//                    strReq.Append("<birthDate>" + dob + "</birthDate>");
//                    strReq.Append("<hasStrecher/><parentSequence/><passengerTypeCode>CHLD</passengerTypeCode>");
//                    strReq.Append("<personName>");
//                    strReq.Append("<givenName>" + firstName + "</givenName>");
//                    strReq.Append("<shareMarketInd/><surname>" + lastName + "</surname>");
//                    strReq.Append("</personName>");
//                    strReq.Append("<accompaniesqlyInfant/><contactPerson>");
//                    strReq.Append("<email>");
//                    strReq.Append("<email>" + email + "</email>");
//                    strReq.Append("<markedForSendingRezInfo/><preferred/><shareMarketInd>true</shareMarketInd>");
//                    strReq.Append("</email>");
//                    strReq.Append("<markedForSendingRezInfo>true</markedForSendingRezInfo>");
//                    strReq.Append("<phoneNumber>");
//                    strReq.Append("<areaCode>051</areaCode>");
//                    strReq.Append("<countryCode>+92</countryCode>");
//                    strReq.Append("<markedForSendingRezInfo>true</markedForSendingRezInfo>");
//                    strReq.Append("<preferred/><shareMarketInd/><subscriberNumber>" + mobile + "</subscriberNumber>");
//                    strReq.Append("</phoneNumber>");
//                    strReq.Append("<shareMarketInd>true</shareMarketInd>");
//                    strReq.Append("<socialSecurityNumber>" + document + "</socialSecurityNumber><useForInvoicing/>");
//                    strReq.Append("<shareContactInfo>true</shareContactInfo>");
//                    strReq.Append("</contactPerson>");
//                    strReq.Append("<requestedSeatCount>1</requestedSeatCount><shareMarketInd/><unaccompaniedMinor/>");
//                    strReq.Append("</airTravelerList>");
//                }
//            }

//            strReq.Append("<requestPurpose>MODIFY_PERMANENTLY_AND_CALC</requestPurpose>");

//            strReq.Append("<specialRequestDetails>");

//            if (Children > 0)
//            {
//                for (int c = 1; c <= Children; c++)
//                {
//                    DateTime dtDOB = Convert.ToDateTime(frm?["dobC_" + c.ToString()]?.ToString().ToString("yyyy-MM-dd"));

//                    strReq.Append("<specialServiceRequestList>");
//                    strReq.Append("<airTravelerSequence>3</airTravelerSequence>");
//                    strReq.Append("<flightSegmentSequence>0</flightSegmentSequence>");
//                    strReq.Append("<SSR>");
//                    strReq.Append("<allowedQuantityPerPassenger/>");
//                    strReq.Append("<bundleRelatedSsr/>");
//                    strReq.Append("<code>CHLD</code>");
//                    strReq.Append("<explanation>" + dtDOB.ToString("yyyy-MM-dd") + "</explanation>");
//                    strReq.Append("<extraBaggage/>");
//                    strReq.Append("<free/>");
//                    strReq.Append("<showOnItinerary/>");
//                    strReq.Append("<unitOfMeasureExist/>");
//                    strReq.Append("</SSR>");
//                    strReq.Append("<serviceQuantity>1</serviceQuantity>");
//                    strReq.Append("<status>NN</status>");
//                    strReq.Append("<ticketed/>");
//                    strReq.Append("</specialServiceRequestList>");
//                }
//            }


//            if (Infants > 0)
//            {
//                for (int i = 1; i <= Infants; i++)
//                {
//                    DateTime dtDOB = Convert.ToDateTime(frm?["dobI_" + i.ToString()]?.ToString("yyyy-MM-dd"));
//                    sex = frm?["sexI_" + i.ToString()]?.ToString();
//                    document = frm?["documentI_" + i.ToString()]?.ToString();
//                    documenttype = frm?["documenttypeI_" + i.ToString()]?.ToString();
//                    expiry = Convert.ToDateTime(frm?["expiryI_" + i.ToString()]?.ToString("yyyy-MM-dd"));
//                    nationality = frm?["nationalityI_" + i.ToString()]?.ToString();
//                    firstName = frm?["firstNameI_" + i.ToString()]?.ToString();
//                    lastName = frm?["lastNameI_" + i.ToString()]?.ToString();

//                    if (fType == "OneWay")
//                    {
//                        strReq.Append("<specialServiceRequestList>");
//                        strReq.Append("<airTravelerSequence>1</airTravelerSequence>");
//                        strReq.Append("<flightSegmentSequence>1</flightSegmentSequence>");
//                        strReq.Append("<SSR>");
//                        strReq.Append("<allowedQuantityPerPassenger/>");
//                        strReq.Append("<bundleRelatedSsr/>");
//                        strReq.Append("<code>INFT</code>");
//                        strReq.Append("<explanation>" + firstName + "/" + lastName + " Inft " + dtDOB.ToString("ddMMMyy") + "</explanation>");
//                        strReq.Append("<extraBaggage/>");
//                        strReq.Append("<free/>");
//                        strReq.Append("<showOnItinerary/>");
//                        strReq.Append("<unitOfMeasureExist/>");
//                        strReq.Append("</SSR>");
//                        strReq.Append("<serviceQuantity>1</serviceQuantity>");
//                        strReq.Append("<status>NN</status>");
//                        strReq.Append("<ticketed/>");
//                        strReq.Append("</specialServiceRequestList>");
//                    }
//                    else if (fType == "Return")
//                    {
//                        strReq.Append("<specialServiceRequestList>");
//                        strReq.Append("<airTravelerSequence>1</airTravelerSequence>");
//                        strReq.Append("<flightSegmentSequence>1</flightSegmentSequence>");
//                        strReq.Append("<SSR>");
//                        strReq.Append("<allowedQuantityPerPassenger/>");
//                        strReq.Append("<bundleRelatedSsr/>");
//                        strReq.Append("<code>INFT</code>");
//                        strReq.Append("<explanation>" + firstName + "/" + lastName + " Inft " + dtDOB.ToString("ddMMMyy") + "</explanation>");
//                        strReq.Append("<extraBaggage/>");
//                        strReq.Append("<free/>");
//                        strReq.Append("<showOnItinerary/>");
//                        strReq.Append("<unitOfMeasureExist/>");
//                        strReq.Append("</SSR>");
//                        strReq.Append("<serviceQuantity>1</serviceQuantity>");
//                        strReq.Append("<status>NN</status>");
//                        strReq.Append("<ticketed/>");
//                        strReq.Append("</specialServiceRequestList>");
//                        strReq.Append("<specialServiceRequestList>");
//                        strReq.Append("<airTravelerSequence>1</airTravelerSequence>");
//                        strReq.Append("<flightSegmentSequence>2</flightSegmentSequence>");
//                        strReq.Append("<SSR>");
//                        strReq.Append("<allowedQuantityPerPassenger/>");
//                        strReq.Append("<bundleRelatedSsr/>");
//                        strReq.Append("<code>INFT</code>");
//                        strReq.Append("<explanation>" + firstName + "/" + lastName + " Inft " + dtDOB.ToString("ddMMMyy") + "</explanation>");
//                        strReq.Append("<extraBaggage/>");
//                        strReq.Append("<free/>");
//                        strReq.Append("<showOnItinerary/>");
//                        strReq.Append("<unitOfMeasureExist/>");
//                        strReq.Append("</SSR>");
//                        strReq.Append("<serviceQuantity>1</serviceQuantity>");
//                        strReq.Append("<status>NN</status>");
//                        strReq.Append("<ticketed/>");
//                        strReq.Append("</specialServiceRequestList>");
//                    }
//                }
//            }

//            strReq.Append("</specialRequestDetails>");

//            if (fType == "OneWay")
//            {
//                DataRow item = (from DataRow drow1 in dtFlights.Rows
//                                where drow1["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id.ToString() &&
//                                drow1["resBookDesigCode"].ToString() == resBookDesigCode.ToString()
//                                select drow1).FirstOrDefault();

//                DataRow fareDetails = (from DataRow drow2 in dtFareList.Rows
//                                       where drow2["fareComponentGroupList_Id"].ToString() == fareComponentGroupList_Id.ToString() &&
//                                       drow2["resBookDesigCode"].ToString() == resBookDesigCode.ToString()
//                                       select drow2).FirstOrDefault();

//                DataRow DepartureCity = (from DataRow drow3 in dtCityInfo.Rows
//                                         where drow3["locationCode"].ToString() == Departure
//                                         select drow3).FirstOrDefault();

//                DataRow ArrivalCity = (from DataRow drow4 in dtCityInfo.Rows
//                                       where drow4["locationCode"].ToString() == Arrival
//                                       select drow4).FirstOrDefault();

//                xmlRequest = OneWayBookingXML(strReq.ToString(), dtFlights, fareDetails, DepartureCity, ArrivalCity, resBookDesigCode, deiCode, explanation, note);
//            }

//            #endregion

//            ds = HitItReturnResponse(xmlRequest).Result;

//            //XmlDocument doc = new XmlDocument();
//            //doc.Load("C:\\Users\\Hp\\Downloads\\response.xml");
//            //XmlReader xmlReader = new XmlNodeReader(doc);

//            //ds.ReadXml(xmlReader);

//            //DataSet ds1 = null; // (DataSet)Session["AirPriceRsp"];
//            DataTable dtAirPricingInfo;

//            //  string AirPricingInfoKey = "1";

//            // ds2.ReadXml(AirPricingSolution);
//            List<string> bookingReferenceIDList = new List<string>();

//            if (ds.Tables.Contains("bookingReferenceIDList"))
//            {
//                foreach (DataRow ro in ds.Tables["bookingReferenceIDList"].Rows)
//                {
//                    bookingReferenceIDList.Add(ro["ID"].ToString());
//                }
//                dtAirPricingInfo = (DataTable)ds.Tables["bookingReferenceIDList"];
//            }
//            else
//            {
//                bookingReferenceIDList.Add(ds.Tables["bookingReferenceIDList"].Rows[0]["ID"].ToString());
//                dtAirPricingInfo = (DataTable)ds.Tables["bookingReferenceIDList"];
//            }

//            string ID1 = ds.Tables["bookingReferenceIDList"].Rows[0]["ID"].ToString();
//            string PnrForTicket = ds.Tables["bookingReferenceIDList"].Rows[0]["referenceID"].ToString();

//            if (bookingReferenceIDList.Count > 0)
//            {

//                #region Booking Info


//                int Adts = 0;
//                int Chlds = 0;
//                int inft = 0;

//                var Fares = (from f in dtFlights.AsEnumerable().Where(f => f.Field<string?>("Option_Id") == optionID)
//                             select new
//                             {
//                                 AdultTotalPrice = f.Field<decimal?>("AdultTotalPrice"),
//                                 AdultBasePrice = f.Field<decimal?>("AdultBasePrice"),
//                                 AdultTaxes = f.Field<decimal?>("AdultTaxes"),
//                                 ChildTotalPrice = f.Field<decimal?>("ChildTotalPrice"),
//                                 ChildBasePrice = f.Field<decimal?>("ChildBasePrice"),
//                                 ChildTaxes = f.Field<decimal?>("ChildTaxes"),
//                                 InfantTotalPrice = f.Field<decimal?>("InfantTotalPrice"),
//                                 InfantBasePrice = f.Field<decimal?>("InfantBasePrice"),
//                                 InfantTaxes = f.Field<decimal?>("InfantTaxes"),
//                             }).FirstOrDefault();




//                decimal? Adultsfare = Fares.AdultBasePrice;
//                decimal? Childfare = Fares.ChildBasePrice;
//                decimal? Infantfare = Fares.InfantBasePrice;
//                decimal? AdultTaxes = Fares.AdultTaxes;
//                decimal? ChildTaxes = Fares.ChildTaxes;
//                decimal? InfnatTaxes = Fares.InfantTaxes;
//                decimal? totalfare = 0;
//                decimal? totaltaxes = 0;

//                decimal? grandtotal = 0;


//                DateTime dttrans = DateTime.Now.ToUniversalTime();

//                // decimal[] TotalAmt = 1000; //_AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Children, Infants, AirlineCode);

//                totaltaxes = AdultTaxes + ChildTaxes + InfnatTaxes;
//                totalfare = Adultsfare + Childfare + Infantfare; //TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
//                grandtotal = totalfare + totaltaxes + 1000;// + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
//                                                           //if (Adults > 0) { Adultsfare = TotalAmt[2] / Adults; }
//                                                           //if (Children > 0) { Childfare = TotalAmt[3] / Children; }
//                                                           //if (Infants > 0) { Infatfare = TotalAmt[4] / Infants; }


//                // List<MalikExpressMVC.Models.XML.OriginDestinationOption> odSegments = (List<MalikExpressMVC.Models.XML.OriginDestinationOption>)Session["odSegments"];

//                int BookingTypeID = 1; //odSegments.Count();

//                string Scale = "";
//                decimal Incentives = 0;
//                //if (Session["Incentives"] != null)
//                //{
//                //    HashSet<Incentive> inc = (HashSet<Incentive>)Session["Incentives"];
//                //    var FlightIncentives = inc.Where(x => x.ServiceID == 2).FirstOrDefault();

//                //    Incentives = FlightIncentives.Incentives ?? 0;
//                //    Scale = FlightIncentives.Scale;
//                //}
//                string ID = ds.Tables["bookingReferenceIDList"].Rows[0][1].ToString();
//                Reservation flight = new Reservation()
//                {
//                    Adults = Adults,
//                    Children = Children,
//                    Infants = Infants,
//                    PNR = ID,
//                    AdultBasicFare = Adultsfare,
//                    ChildBasicFare = Childfare,
//                    InfantBasicFare = Infantfare,
//                    AdultTax = AdultTaxes,
//                    ChildTax = ChildTaxes,
//                    InfantTax = InfnatTaxes,
//                    TotalFare = totalfare,
//                    TotalTaxes = totaltaxes,
//                    GrandTotal = grandtotal,
//                    CustomerComission = 0,
//                    CustomerOComission = 0,
//                    AirlineComission = 0,
//                    Incentive = Incentives,
//                    IncentiveScale = Scale,
//                    BookingDate = DateTime.Now.ToUniversalTime(),
//                    BookingStatus = "OK",
//                    BookingTypeID = BookingTypeID,
//                    LeadFirstName = leadfirstname,
//                    LeadLastName = leadlastname,
//                    UserID = UserID,
//                    Email = email,
//                    PrimaryPhone = contact,
//                    SecondaryPhone = "",
//                    FlightBookingID = FlightBookingID
//                };

//                sql.Reservations.Add(flight);
//                sql.SaveChanges();

//                int BookingID = flight.Id;

//                string TicketNum = "";
//                #endregion

//                #region Pax Info                        

//                string DeptDate = "", DeptTime = "", DeptFrom = "", ArrvlTo = "";
//                bool IsDepart = false;
//                int itrCodes = 0;
//                byte segmenttype = 1;
//                string MrktingAirline = "";
//                //  DataTable dtAirSegment = dsHititResponse.Tables["AirSegment"];


//                foreach (DataRow segment in dtFlights.Rows)
//                {

//                    if (segment["option_ID"].ToString() == optionID)
//                    {

//                        string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

//                        string AirEquipType = "";
//                        if (dtFlights.Columns.Contains("airEquipTypeModel"))
//                        {
//                            AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
//                        }
//                        bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
//                        DateTime DepDate = DepartureDateTime; // segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"].ToString()) : new DateTime();
//                        string DepTime = DepartureDateTime.ToString("HH:mm:ss"); //segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"]).ToString("hh:mm tt") : "";
//                        DateTime ArrDate = ArrivalDateTime; // segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"].ToString()) : new DateTime();
//                        string ArrTime = ArrivalDateTime.ToString("HH:mm:ss"); //segment != null && segment["departureDateTime"].ToString() != null ? Convert.ToDateTime(segment["departureDateTime"]).ToString("hh:mm tt") : "";

//                        string MarketingAirlineCode = "PK";// segment != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
//                        string OperatingAirlineCode = "PK"; // segment != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
//                        string FlightNumber = segment["flightNumber"].ToString(); //FlightNumber.ToString();
//                        string CompanyShortName = segment != null && segment["code"].ToString() != null && segment["code"].ToString() != null ? segment["code"].ToString() : "";
//                        string OriginCode = segment["dep_LocationCode"].ToString();  //segment["Origin"].ToString();
//                        string DestinationCode = segment["arr_LocationCode"].ToString(); // segment["Destination"].ToString();
//                                                                                         // string serviceclass = resBookDesigCode.ToString();
//                        int ServiceCls = 4; // sql.ServiceClasses.Where(x => x.ClassName == resBookDesigCode).Select(x => x.ServiceClassID).FirstOrDefault();
//                        int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
//                        int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
//                        int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
//                        int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

//                        string ArrGMTOffset = segment != null && segment["arrivalDateTimeUTC"].ToString() != null && segment["arrivalDateTimeUTC"].ToString() != null ? segment["arrivalDateTimeUTC"].ToString().Substring(segment["arrivalDateTimeUTC"].ToString().Length - 6) : "";
//                        string DptGMTOffset = segment != null && segment["departureDateTimeUTC"].ToString() != null && segment["departureDateTimeUTC"].ToString() != null ? segment["departureDateTimeUTC"].ToString().Substring(segment["departureDateTimeUTC"].ToString().Length - 6) : "";

//                        if (!IsDepart)
//                        {
//                            IsDepart = true;
//                            MrktingAirline = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineName).FirstOrDefault();
//                            DeptFrom = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityName).FirstOrDefault();
//                            ArrvlTo = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityName).FirstOrDefault();

//                            DeptDate = DepDate.ToString("MMM dd, ddd");
//                            DeptTime = DepTime;

//                        }

//                        if (ServiceCls != 1 && ServiceCls != 2 && ServiceCls != 3)
//                        {
//                            ServiceCls = 4;
//                        }

//                        FlightSegment flightseg = new FlightSegment()
//                        {
//                            ArrivalDate = ArrDate,
//                            ArrivalTime = ArrTime,
//                            ArrivalGMTOffset = ArrGMTOffset,
//                            DepartureDate = DepDate,
//                            DepartureTime = DepTime,
//                            DepartureGMTOffset = DptGMTOffset,
//                            ServiceClassID = ServiceCls,
//                            OriginID = Origin,
//                            DestinationID = Dest,
//                            FlightNumber = FlightNumber,
//                            MarketingAirlineCode = MarketingAirlineCode,
//                            OperatingAirlineCode = OperatingAirlineCode,
//                            MarketingAirlineID = MarketingAirlineID,
//                            OperatingAirlineID = OperatingAirlineID,
//                            OperatingAirlineName = CompanyShortName,
//                            FlightBookingID = FlightBookingID,
//                            AirEquipType = AirEquipType,
//                            ETicket = ETicket,
//                            ResBookDesigCode = resBookDesigCode,
//                            Way = segmenttype
//                        };
//                        sql.FlightSegments.Add(flightseg);
//                        sql.SaveChanges();
//                        int FlightSegmentID = flightseg.FlightSegmentID;

//                        //DataTable dtFareInfo;

//                        //if (ds.Tables.Contains("FareInfo"))
//                        //{
//                        //    dtFareInfo = (DataTable)ds.Tables["FareInfo"];
//                        //}
//                        //else
//                        //{
//                        //    dtFareInfo = (DataTable)ds1.Tables["FareInfo"];
//                        //}


//                        //foreach (DataRow fare in dtFareInfo.Rows)
//                        //{
//                        //    string bookingCode = fare["key"] != null ? fare["key"].ToString() : "";
//                        //    string fareBasisCode = fare["FareBasis"] != null ? fare["FareBasis"].ToString() : "";
//                        //    string typeCode = fare["PassengerTypeCode"].ToString();
//                        //    if (bookingCode != "" && fareBasisCode != "")
//                        //    {
//                        //        FareBasisCode f = new FareBasisCode()
//                        //        {
//                        //            Code = bookingCode,
//                        //            Text = fareBasisCode,
//                        //            PaxType = typeCode,
//                        //            FlightSegmentID = FlightSegmentID
//                        //        };
//                        //        sql.FareBasisCodes.Add(f);
//                        //        sql.SaveChanges();
//                        //    }
//                        //}
//                        itrCodes++;
//                    }
//                    segmenttype++;
//                }


//                //  Session.Remove("AirPriceRsp");
//                //  Session.Remove("AirPricingSolution");

//                //  TempData["Data"] = 1;


//                if (ds.Tables.Contains("bookingReferenceIDList"))
//                {
//                    if (ds.Tables["bookingReferenceIDList"].Rows.Count > 0)
//                    {
//                        ID = ds.Tables["bookingReferenceIDList"].Rows[0][1].ToString();
//                        return ID.ToString();
//                    }
//                }


//                return "something went wrong";
//                #endregion


//            }
//            return "something went wrong";
//        }
//        else
//        {
//            return "something went wrong";
//        }
//        return "something went wrong";
//    }
//    catch (Exception ex)
//    {
//        return "something went wrong";
//    }
//}