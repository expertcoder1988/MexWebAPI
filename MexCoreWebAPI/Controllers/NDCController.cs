using LowFareAlerts;

using MexCoreWebAPI.Model;
using MexCoreWebAPI.Model.NDC;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
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
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MexCoreWebAPI.Model;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.FileSystemGlobbing;

namespace MexCoreWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]
    public class NDCController : ControllerBase
    {
        private readonly MexDbContext sql;
        private readonly MexOldDBContext oldSQL;

        public NDCController(MexDbContext context, MexOldDBContext OldSQL)
        {
            sql = context;
            oldSQL = OldSQL;
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

                strReq.Append(@"<soapenv:Envelope xmlns:soapenv = 'http://schemas.xmlsoap.org/soap/envelope/' xmlns = 'http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRQ'>");
                strReq.Append(@"<soapenv:Header/>");
                strReq.Append(@"<soapenv:Body>");
                strReq.Append(@"<IATA_AirShoppingRQ>");
                strReq.Append(@"<MessageDoc>");
                strReq.Append(@"<Name>NDC GATEWAY</Name>");
                strReq.Append(@"<RefVersionNumber>20.1</RefVersionNumber>");
                strReq.Append(@"</MessageDoc>");
                strReq.Append(@"<Party>");
                strReq.Append(@"<Sender>");
                strReq.Append(@"<TravelAgency>");
                strReq.Append(@"<AgencyID>PSA2746366</AgencyID>");
                strReq.Append(@"</TravelAgency>");
                strReq.Append(@"</Sender>");
                strReq.Append(@"</Party>");
                strReq.Append(@"<Request>");
                strReq.Append(@"<FlightRequest>");

                if (tripType == "OneWay")
                {
                    strReq.Append(@"<OriginDestCriteria>");
                    strReq.Append(@"<DestArrivalCriteria>");
                    strReq.Append(@"<IATA_LocationCode>" + to + "</IATA_LocationCode>");
                    strReq.Append(@"</DestArrivalCriteria>");
                    strReq.Append(@"<OriginDepCriteria>");
                    strReq.Append(@"<Date>" + Dept + "</Date>");
                    strReq.Append(@"<IATA_LocationCode>" + from + "</IATA_LocationCode>");
                    strReq.Append(@"</OriginDepCriteria>");
                    strReq.Append(@"<PreferredCabinType>");
                    strReq.Append(@"<CabinTypeCode>Y</CabinTypeCode>");
                    strReq.Append(@"</PreferredCabinType>");
                    strReq.Append(@"</OriginDestCriteria>");
                }
                else if (tripType == "Return")
                {
                    strReq.Append(@"<OriginDestCriteria>");
                    strReq.Append(@"<DestArrivalCriteria>");
                    strReq.Append(@"<IATA_LocationCode>" + to + "</IATA_LocationCode>");
                    strReq.Append(@"</DestArrivalCriteria>");
                    strReq.Append(@"<OriginDepCriteria>");
                    strReq.Append(@"<Date>" + Dept + "</Date>");
                    strReq.Append(@"<IATA_LocationCode>" + from + "</IATA_LocationCode>");
                    strReq.Append(@"</OriginDepCriteria>");
                    strReq.Append(@"<PreferredCabinType>");
                    strReq.Append(@"<CabinTypeCode>Y</CabinTypeCode>");
                    strReq.Append(@"</PreferredCabinType>");
                    strReq.Append(@"</OriginDestCriteria>");
                    strReq.Append(@"<OriginDestCriteria>");
                    strReq.Append(@"<DestArrivalCriteria>");
                    strReq.Append(@"<IATA_LocationCode>" + from + "</IATA_LocationCode>");
                    strReq.Append(@"</DestArrivalCriteria>");
                    strReq.Append(@"<OriginDepCriteria>");
                    strReq.Append(@"<Date>" + Arr + "</Date>");
                    strReq.Append(@"<IATA_LocationCode>" + to + "</IATA_LocationCode>");
                    strReq.Append(@"</OriginDepCriteria>");
                    strReq.Append(@"<PreferredCabinType>");
                    strReq.Append(@"<CabinTypeCode>Y</CabinTypeCode>");
                    strReq.Append(@"</PreferredCabinType>");
                    strReq.Append(@"</OriginDestCriteria>");
                }

                strReq.Append(@"</FlightRequest>");
                strReq.Append(@"<Paxs>");

                int id = 1;
                for (int a = 1; a <= adults; a++)
                {
                    strReq.Append(@"<Pax>");
                    strReq.Append(@"<PaxID>SH" + id++ + "</PaxID>");
                    strReq.Append(@"<PTC>ADT</PTC>");
                    strReq.Append(@"</Pax>");
                }
                for (int c = 1; c <= children; c++)
                {
                    strReq.Append(@"<Pax>");
                    strReq.Append(@"<PaxID>SH" + id++ + "</PaxID>");
                    strReq.Append(@"<PTC>CHD</PTC>");
                    strReq.Append(@"</Pax>");
                }
                for (int i = 1; i <= infants; i++)
                {
                    strReq.Append(@"<Pax>");
                    strReq.Append(@"<PaxID>SH" + id++ + "</PaxID>");
                    strReq.Append(@"<PTC>INF</PTC>");
                    strReq.Append(@"</Pax>");
                }


                strReq.Append(@"</Paxs>");
                strReq.Append(@"<ResponseParameters>");
                strReq.Append(@"<CurParameter>");
                strReq.Append(@"<RequestedCurCode>PKR</RequestedCurCode>");
                strReq.Append(@"</CurParameter>");
                strReq.Append(@"<LangUsage>");
                strReq.Append(@"<LangCode>EN</LangCode>");
                strReq.Append(@"</LangUsage>");
                strReq.Append(@"</ResponseParameters>");
                strReq.Append(@"<ShoppingCriteria>");
                strReq.Append(@"<ConnectionCriteria>");
                strReq.Append(@"<InterlineInd>true</InterlineInd>");
                strReq.Append(@"</ConnectionCriteria>");
                strReq.Append(@"</ShoppingCriteria>");
                strReq.Append(@"</Request>");
                strReq.Append(@"</IATA_AirShoppingRQ>");
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
         string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string airline = "", string userid = "-1")
        {
            try
            {
                // MexCoreWebAPI.Model.NDC.Envelope ndcFlights = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string url = "https://app.crane.aero/cranendc/v20.1/CraneNDCService";
                // string url = "https://app-stage.crane.aero/cranendc/v20.1/CraneNDCService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");
                request.Headers.Add("username", "PSA2746366");
                // request.Headers.Add("password", "Test1234"); //Test password
                request.Headers.Add("password", "Sam7890@#_"); // production password

                var xmlReq = HitItXMLReq("OneWay", from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

                DataSet ds = new DataSet();



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

                    //Log

                    //string fileName = @"C:\hallmark\doAirShopping_2A1C1I_req.txt";  //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Res.txt";
                    //FileInfo fi = new FileInfo(fileName);

                    ////////Check if file already exists. If yes, delete it.
                    //if (fi.Exists)
                    //{
                    //    fi.Delete();
                    //}

                    //////Create a new file
                    //using (FileStream fs = fi.Create())
                    //{
                    //    Byte[] txt = new UTF8Encoding(true).GetBytes(xmlReq);
                    //    fs.Write(txt, 0, txt.Length);
                    //}


                    //fileName = @"C:\hallmark\doAirShopping_2A1C1I_res.txt";  //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Res.txt";
                    //FileInfo fi1 = new FileInfo(fileName);

                    ////////Check if file already exists. If yes, delete it.
                    //if (fi1.Exists)
                    //{
                    //    fi1.Delete();
                    //}

                    //////Create a new file
                    //using (FileStream fs = fi1.Create())
                    //{
                    //    Byte[] txt = new UTF8Encoding(true).GetBytes(customerJsonString);
                    //    fs.Write(txt, 0, txt.Length);
                    //}

                    if (!ds.Tables.Contains("Price"))
                    {
                        return "something went wrong";
                    }

                    DataTable dtPrice = ds.Tables["Price"].Copy();
                    dtPrice.Columns.Add("PaxType");

                    int TotalOffers = Convert.ToInt32(ds.Tables["AllOffersSummary"].Rows[0][0].ToString());
                    int TotalPaxTypes = dtPrice.Rows.Count / TotalOffers;

                    for (int s = 0; s < dtPrice.Rows.Count; s++)
                    {
                        if (adults > 0)
                        {
                            dtPrice.Rows[s]["PaxType"] = "ADT";
                            s = s + (TotalPaxTypes - 1);
                        }
                    }

                    if (TotalPaxTypes > 1)
                    {
                        if (children > 0)
                        {
                            for (int s = 1; s < dtPrice.Rows.Count; s++)
                            {
                                if (dtPrice.Rows[s]["PaxType"] != "")
                                {
                                    if (children > 0)
                                    {
                                        dtPrice.Rows[s]["PaxType"] = "CHD";
                                        s = s + (TotalPaxTypes - 1);
                                    }
                                }
                            }
                        }

                        if (infants > 0)
                        {
                            int inf = 0;

                            if (children == 0)
                            {
                                inf = 1;
                            }
                            else
                            {
                                inf = TotalPaxTypes - 1;
                            }

                            for (int s = inf; s < dtPrice.Rows.Count; s++)
                            {
                                if (dtPrice.Rows[s]["PaxType"] != "")
                                {
                                    if (infants > 0)
                                    {
                                        dtPrice.Rows[s]["PaxType"] = "INF";
                                        s = s + (TotalPaxTypes - 1);
                                    }
                                }
                            }
                        }
                    }

                    if (!ds.Tables.Contains("PaxJourneyRefID"))
                    {
                        // Create the table
                        DataTable paxJourneyRefIDTable = new DataTable("PaxJourneyRefID");
                        paxJourneyRefIDTable.Columns.Add("PaxJourneyRefID_Text", typeof(string));
                        paxJourneyRefIDTable.Columns.Add("OriginDest_Id", typeof(int));

                        // Add the table to the DataSet
                        ds.Tables.Add(paxJourneyRefIDTable);
                    }

                    // Retrieve the PaxJourney table
                    DataTable paxJourneyTable = ds.Tables["PaxJourney"];

                    // Populate the PaxJourneyRefID table
                    foreach (DataRow row in paxJourneyTable.Rows)
                    {
                        DataRow newRow = ds.Tables["PaxJourneyRefID"].NewRow();
                        newRow["PaxJourneyRefID_Text"] = row["PaxJourneyID"]; // Assuming 'PaxJourneyID' is a column in 'PaxJourney'
                        newRow["OriginDest_Id"] = "0"; // Insert '0' into the PaxJourneyRefID_Text column
                        ds.Tables["PaxJourneyRefID"].Rows.Add(newRow);
                    }

                    if (!ds.Tables["OriginDest"].Columns.Contains("OriginDest_Id"))
                    {
                        ds.Tables["OriginDest"].Columns.Add("OriginDest_Id", typeof(int));

                        int s = 0;
                        foreach (DataRow row in ds.Tables["OriginDest"].Rows)
                        {
                            row["OriginDest_Id"] = s;
                            s++;
                        }

                    }

                    var OriginDest = (from od in ds.Tables["OriginDest"].AsEnumerable()
                                      join odl in ds.Tables["OriginDestList"].AsEnumerable() on od["OriginDestList_Id"] equals odl["OriginDestList_Id"]
                                      join pjr in ds.Tables["PaxJourneyRefID"].AsEnumerable() on od["OriginDest_Id"] equals pjr["OriginDest_Id"]
                                      join pjl in ds.Tables["PaxJourneyList"].AsEnumerable() on odl["DataLists_Id"] equals pjl["DataLists_Id"]
                                      // join pj in ds.Tables["PaxJourney"].AsEnumerable() on pjl["PaxJourneyList_Id"] equals pj["PaxJourneyList_Id"]

                                      select new
                                      {
                                          OriginDestList_Id = odl["OriginDestList_Id"],
                                          DataLists_Id = odl["DataLists_Id"],
                                          DestCode = od["DestCode"],
                                          OriginCode = od["OriginCode"],
                                          OriginDestID = od["OriginDestID"],
                                          OriginDest_Id = od["OriginDest_Id"],
                                          PaxJourneyRefID_Text = pjr["PaxJourneyRefID_Text"],
                                          PaxJourneyList_Id = pjl["PaxJourneyList_Id"],
                                          // PaxJourneyID = pj["PaxJourneyID"],
                                          // PaxSegmentRefID = pj["PaxSegmentRefID"]
                                      }).ToList();

                    DataTable dtSegments = LINQResultToDataTable(OriginDest);
                    dtSegments.Columns.Add("PaxSegmentRefID");
                    dtSegments.Columns.Add("PaxJourney_ID");

                    foreach (DataRow dr in dtSegments.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["PaxJourney"].Rows)
                        {
                            if (dr["PaxJourneyList_Id"].ToString() == dr1["PaxJourneyList_Id"].ToString())
                            {
                                if (ds.Tables["PaxJourney"].Columns.Contains("PaxSegmentRefID") == true)
                                {
                                    dr["PaxSegmentRefID"] = dr1["PaxSegmentRefID"];
                                }
                                else if (ds.Tables["PaxJourney"].Columns.Contains("PaxJourney_ID") == true)
                                {
                                    dr["PaxJourney_ID"] = dr1["PaxJourney_ID"];
                                }
                            }
                        }
                    }


                    int i = 0;

                    if (ds.Tables.Contains("PaxSegmentRefID"))
                    {
                        foreach (DataRow dr in ds.Tables["PaxSegmentRefID"].Rows)
                        {
                            dr["PaxSegmentRefID_Text"] = dr["PaxSegmentRefID_Text"] + "~" + i++;
                        }
                    }
                    else
                    {
                        DataTable newTable = ds.Tables["PaxJourney"].Copy();
                        newTable.TableName = "PaxSegmentRefID";
                        newTable.Columns.Add("PaxSegmentRefID_Text");
                        newTable.Columns.Add("PaxJourney_Id");
                        ds.Tables.Add(newTable);

                        int s = 0;
                        foreach (DataRow dr in ds.Tables["PaxSegmentRefID"].Rows)
                        {
                            dr["PaxSegmentRefID_Text"] = dr["PaxSegmentRefID"] + "~" + i++;
                            dr["PaxJourney_Id"] = s.ToString();
                            s++;
                        }
                    }

                    i = 0;
                    foreach (DataRow dr in ds.Tables["PaxSegment"].Rows)
                    {
                        dr["PaxSegmentID"] = dr["PaxSegmentID"] + "~" + i++;
                    }


                    var PaxSegment = (from psr in ds.Tables["PaxSegmentRefID"].AsEnumerable()
                                      join ps in ds.Tables["PaxSegment"].AsEnumerable() on psr["PaxSegmentRefID_Text"] equals ps["PaxSegmentID"]
                                      join arr in ds.Tables["Arrival"].AsEnumerable() on ps["PaxSegment_Id"] equals arr["PaxSegment_Id"]
                                      join dep in ds.Tables["Dep"].AsEnumerable() on ps["PaxSegment_Id"] equals dep["PaxSegment_Id"]
                                      select new
                                      {
                                          PaxSegmentRefID_Text = psr["PaxSegmentRefID_Text"],
                                          PaxJourney_Id = psr["PaxJourney_Id"],
                                          PaxSegmentID = ps["PaxSegmentID"],
                                          PaxSegment_Id = ps["PaxSegment_Id"],
                                          Duration = ps["Duration"],
                                          Arr_AircraftScheduledDateTime = arr["AircraftScheduledDateTime"],
                                          Arr_IATA_LocationCode = arr["IATA_LocationCode"],
                                          Arr_StationName = arr["StationName"],
                                          Dep_AircraftScheduledDateTime = dep["AircraftScheduledDateTime"],
                                          Dep_IATA_LocationCode = dep["IATA_LocationCode"],
                                          Dep_StationName = dep["StationName"],
                                      }).ToList();


                    DataTable dtPaxSegment = LINQResultToDataTable(PaxSegment);
                    dtPaxSegment.Columns.Add("PaxJourneyID");

                    dtPaxSegment.Columns["PaxJourneyID"].SetOrdinal(2);


                    foreach (DataRow dr in dtPaxSegment.Rows)
                    {

                        if (!ds.Tables["PaxJourney"].Columns.Contains("PaxJourney_Id"))
                        {

                            foreach (DataRow dr1 in ds.Tables["PaxSegmentRefID"].Rows)
                            {
                                if (dr["PaxJourney_Id"].ToString() == dr1["PaxJourney_Id"].ToString())
                                {
                                    dr["PaxJourneyID"] = dr1["PaxJourneyID"];
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow dr1 in ds.Tables["PaxJourney"].Rows)
                            {
                                if (dr["PaxJourney_Id"].ToString() == dr1["PaxJourney_Id"].ToString())
                                {
                                    dr["PaxJourneyID"] = dr1["PaxJourneyID"];
                                }
                            }
                        }

                    }


                    var offers = (from o in ds.Tables["offer"].AsEnumerable()
                                  join j in ds.Tables["JourneyOverview"].AsEnumerable()
                                  on o.Field<int?>("Offer_Id") equals j.Field<int?>("Offer_Id")
                                  join jp in ds.Tables["JourneyPriceClass"].AsEnumerable()
                                  on j.Field<int?>("JourneyOverview_Id") equals jp.Field<int?>("JourneyOverview_Id")
                                  join OI in ds.Tables["OfferItem"].AsEnumerable()
                                   on o.Field<int?>("Offer_Id") equals OI.Field<int?>("Offer_Id")
                                  join FD in ds.Tables["FareDetail"].AsEnumerable()
                                   on OI.Field<int?>("OfferItem_Id") equals FD.Field<int?>("OfferItem_Id")
                                  join FC in ds.Tables["FareComponent"].AsEnumerable()
                                   on FD.Field<int?>("FareDetail_Id") equals FC.Field<int?>("FareDetail_Id")
                                  join CT in ds.Tables["CabinType"].AsEnumerable()
                                   on FC.Field<int?>("FareComponent_Id") equals CT.Field<int?>("FareComponent_Id")
                                  join RB in ds.Tables["RBD"].AsEnumerable()
                                                       on CT.Field<int?>("FareComponent_Id") equals RB.Field<int?>("FareComponent_Id")
                                  join FP in ds.Tables["FarePriceType"].AsEnumerable()
                                                       on FD.Field<int?>("FareDetail_Id") equals FP.Field<int?>("FareDetail_Id")
                                  join OPT in ds.Tables["OfferItemPaymentTimeLimit"].AsEnumerable()
                                                       on OI.Field<int?>("OfferItem_Id") equals OPT.Field<int?>("OfferItem_Id")
                                  join PTL in ds.Tables["PaymentTimeLimitDate"].AsEnumerable()
                                                       on OPT.Field<int?>("OfferItemPaymentTimeLimit_Id") equals PTL.Field<int?>("OfferItemPaymentTimeLimit_Id")
                                  join Pr in dtPrice.AsEnumerable()
                                                       on OI.Field<int?>("OfferItem_Id") equals Pr.Field<int?>("OfferItem_Id")
                                  join EA in ds.Tables["EquivAmount"].AsEnumerable()
                                                       on Pr.Field<int?>("Price_Id") equals EA.Field<int?>("Price_Id")
                                  select new
                                  {
                                      Offer_Id = o.Field<int?>("Offer_Id"),
                                      PaxJourneyRefID = jp.Field<string>("PaxJourneyRefID"),
                                      OfferID = o.Field<string?>("OfferID"),
                                      OwnerCode = o.Field<string?>("OwnerCode"),
                                      CarrierOffers_Id = o.Field<int?>("CarrierOffers_id"),
                                      JourneyOverview_Id = j.Field<int?>("JourneyOverview_Id"),
                                      OfferItem_Id = OI.Field<int?>("OfferItem_Id"),
                                      MandatoryInd = OI.Field<string?>("MandatoryInd"),
                                      OfferItemID = OI.Field<string>("OfferItemID"),
                                      FareDetail_Id = FD.Field<int?>("FareDetail_Id"),
                                      FareComponent_Id = FC.Field<int?>("FareComponent_Id"),
                                      FareBasisCode = FC.Field<string?>("FareBasisCode"),
                                      FareTypeCode = FC.Field<string?>("FareTypeCode"),
                                      PaxSegmentRefID = FC.Field<string?>("PaxSegmentRefID"),
                                      CabinTypeCode = CT.Field<string?>("CabinTypeCode"),
                                      CabintypeName = CT.Field<string?>("CabinTypeName"),
                                      RBD_Code = RB.Field<string?>("RBD_Code"),
                                      FarePriceTypeCode = FP.Field<string?>("FarePriceTypeCode"),
                                      Price = FP.Field<string?>("Price"),
                                      OfferItemPaymentTimeLimit_Id = OPT.Field<int?>("OfferItemPaymentTimeLimit_Id"),
                                      PaymentTimeLimitDateTime = PTL.Field<string?>("PaymentTimeLimitDateTime"),
                                      Price_Id = Pr.Field<int?>("Price_Id"),
                                      PaxType = Pr.Field<string?>("PaxType"),
                                      CurCode = EA.Field<string?>("CurCode"),
                                      EquivAmount_Text = EA.Field<string?>("EquivAmount_Text"),

                                  }).ToList();

                    DataTable dtOffers = LINQResultToDataTable(offers);
                    dtOffers.Columns.Add("TotalPrice_Id");
                    dtOffers.Columns.Add("Tax");
                    dtOffers.Columns.Add("TotalAmount_Text");


                    foreach (DataRow dr in dtOffers.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["TotalPrice"].Rows)
                        {
                            if (dr["Offer_Id"].ToString() == dr1["Offer_Id"].ToString())
                            {
                                dr["TotalPrice_Id"] = dr1["TotalPrice_Id"];
                            }
                        }
                    }

                    var Taxes = from tp in ds.Tables["TotalPrice"].AsEnumerable()
                                join t in ds.Tables["TaxSummary"].AsEnumerable() on tp.Field<int?>("TotalPrice_ID") equals t.Field<int?>("TotalPrice_Id")
                                join tt in ds.Tables["TotalTaxAmount"].AsEnumerable() on t.Field<int?>("TaxSummary_ID") equals tt.Field<int?>("TaxSummary_Id")
                                select new
                                {
                                    TotalPrice_ID = tp.Field<int?>("TotalPrice_ID"),
                                    Offer_Id = tp.Field<int?>("Offer_Id"),
                                    TaxSummary_ID = t.Field<int?>("TaxSummary_ID"),
                                    CurCode = tt.Field<string?>("CurCode"),
                                    TotalTaxAmount_Text = tt.Field<string?>("TotalTaxAmount_Text")
                                };
                    DataTable dtTaxes = LINQResultToDataTable(Taxes);


                    foreach (DataRow dr in dtOffers.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["TotalAmount"].Rows)
                        {
                            if (dr["TotalPrice_Id"].ToString() == dr1["TotalPrice_Id"].ToString())
                            {
                                dr["TotalAmount_Text"] = dr1["TotalAmount_Text"];
                            }
                        }
                    }

                    foreach (DataRow dr in dtOffers.Rows)
                    {
                        foreach (DataRow dr1 in dtTaxes.Rows)
                        {
                            if (dr["TotalPrice_Id"].ToString() == dr1["TotalPrice_Id"].ToString())
                            {
                                dr["Tax"] = dr1["TotalTaxAmount_Text"];
                            }
                        }
                    }


                    DataTable Flights = new DataTable();
                    Flights = dtPaxSegment.Copy();

                    Flights.Columns.Add("Offer_ID");
                    Flights.Columns.Add("OfferID");
                    Flights.Columns.Add("OwnerCode");
                    Flights.Columns.Add("CarrierOffers_Id");
                    Flights.Columns.Add("JourneyOverview_Id");
                    Flights.Columns.Add("PaxJourneyRefID");
                    Flights.Columns.Add("OfferItem_Id");
                    Flights.Columns.Add("MandatoryInd");
                    Flights.Columns.Add("OfferItemID");
                    Flights.Columns.Add("FareDetail_Id");
                    Flights.Columns.Add("FareComponent_Id");
                    Flights.Columns.Add("FareBasisCode");
                    Flights.Columns.Add("FareTypeCode");
                    Flights.Columns.Add("PaxSegmentRefID");
                    Flights.Columns.Add("CabinTypeCode");
                    Flights.Columns.Add("CabintypeName");
                    Flights.Columns.Add("RBD_Code");
                    Flights.Columns.Add("FarePriceTypeCode");
                    Flights.Columns.Add("Price");
                    Flights.Columns.Add("OfferItemPaymentTimeLimit_Id");
                    Flights.Columns.Add("PaymentTimeLimitDateTime");
                    Flights.Columns.Add("Price_Id");
                    Flights.Columns.Add("CurCode");
                    Flights.Columns.Add("EquivAmount_Text");
                    Flights.Columns.Add("FlightType");
                    Flights.Columns.Add("Direct");
                    Flights.Columns.Add("TotalPrice_Id");
                    Flights.Columns.Add("Tax");
                    Flights.Columns.Add("TotalAmount_Text");
                    Flights.Columns.Add("Child_TotalPrice_Id");
                    Flights.Columns.Add("Inf_TotalPrice_Id");

                    Flights.Columns["PaxJourneyRefID"].SetOrdinal(1);
                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in dtOffers.Rows)
                        {
                            if (dr["PaxJourneyId"].ToString() == dr1["PaxJourneyRefID"].ToString())
                            {
                                if (dr1["PaxType"].ToString() == "ADT")
                                {
                                    dr["Offer_ID"] = dr1["Offer_ID"];
                                    dr["OfferID"] = dr1["OfferID"];
                                    dr["OwnerCode"] = dr1["OwnerCode"];
                                    dr["CarrierOffers_Id"] = dr1["CarrierOffers_Id"];
                                    dr["JourneyOverview_Id"] = dr1["JourneyOverview_Id"];
                                    dr["PaxJourneyRefID"] = dr1["PaxJourneyRefID"];
                                    dr["OfferItem_Id"] = dr1["OfferItem_Id"];
                                    dr["MandatoryInd"] = dr1["MandatoryInd"];
                                    dr["OfferItemID"] = dr1["OfferItemID"];
                                    dr["FareDetail_Id"] = dr1["FareDetail_Id"];
                                    dr["FareComponent_Id"] = dr1["FareComponent_Id"];
                                    dr["FareBasisCode"] = dr1["FareBasisCode"];
                                    dr["FareTypeCode"] = dr1["FareTypeCode"];
                                    dr["PaxSegmentRefID"] = dr1["PaxSegmentRefID"];
                                    dr["CabinTypeCode"] = dr1["CabinTypeCode"];
                                    dr["CabintypeName"] = dr1["CabintypeName"];
                                    dr["RBD_Code"] = dr1["RBD_Code"];
                                    dr["FarePriceTypeCode"] = dr1["FarePriceTypeCode"];
                                    dr["Price"] = dr1["Price"];
                                    dr["OfferItemPaymentTimeLimit_Id"] = dr1["OfferItemPaymentTimeLimit_Id"];
                                    dr["PaymentTimeLimitDateTime"] = dr1["PaymentTimeLimitDateTime"];
                                    dr["Price_Id"] = dr1["Price_Id"];
                                    dr["CurCode"] = dr1["CurCode"];
                                    dr["EquivAmount_Text"] = dr1["EquivAmount_Text"];
                                    dr["TotalPrice_Id"] = dr1["TotalPrice_Id"];
                                    dr["Tax"] = dr1["Tax"];
                                    dr["TotalAmount_Text"] = dr1["TotalAmount_Text"];
                                    dr["Child_TotalPrice_Id"] = 0;
                                    dr["Inf_TotalPrice_Id"] = 0;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in dtOffers.Rows)
                        {
                            if (dr["PaxJourneyId"].ToString() == dr1["PaxJourneyRefID"].ToString())
                            {
                                if (dr1["PaxType"].ToString() == "CHD")
                                {
                                    dr["Child_TotalPrice_Id"] = dr1["EquivAmount_Text"];
                                }
                            }
                        }
                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in dtOffers.Rows)
                        {
                            if (dr["PaxJourneyId"].ToString() == dr1["PaxJourneyRefID"].ToString())
                            {
                                if (dr1["PaxType"].ToString() == "INF")
                                {
                                    dr["Inf_TotalPrice_Id"] = dr1["EquivAmount_Text"];
                                }
                            }
                        }
                    }



                    Flights.Columns.Add("CarrierDesigCode");
                    Flights.Columns.Add("CarrierName");
                    Flights.Columns.Add("OperatingCarrierFlightNumberText");
                    Flights.Columns.Add("OriginalCost");
                    Flights.Columns.Add("TotalDuration");

                    List<NDCFlight> flights = ConvertDataTableToList(Flights);

                    var flightDurations = from flight in flights
                                          group flight by flight.Offer_ID into flightGroup
                                          select new
                                          {
                                              Offer_Id = flightGroup.Key,
                                              TotalDuration = FormatDuration(CalculateTotalDuration(flightGroup))
                                          };

                    foreach (DataRow fl in Flights.Rows)
                    {
                        foreach (var itm in flightDurations)
                        {
                            if (fl["Offer_Id"].ToString() == itm.Offer_Id.ToString())
                            {
                                fl["TotalDuration"] = itm.TotalDuration;
                            }
                        }

                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["OperatingCarrierInfo"].Rows)
                        {
                            if (dr["PaxSegment_Id"].ToString() == dr1["PaxSegment_Id"].ToString())
                            {
                                dr["OperatingCarrierFlightNumberText"] = dr1["OperatingCarrierFlightNumberText"];
                                dr["CarrierName"] = dr1["CarrierName"];
                                dr["CarrierDesigCode"] = dr1["CarrierDesigCode"];
                            }
                        }
                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        if (dr["Dep_IATA_LocationCode"].ToString() == from && dr["Arr_IATA_LocationCode"].ToString() == to)
                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 1;
                        }
                        else if (dr["Dep_IATA_LocationCode"].ToString() == from && dr["Arr_IATA_LocationCode"].ToString() != to)
                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 0;
                        }
                        else
                        {
                            dr["FlightType"] = "DC";
                            dr["Direct"] = 0;
                        }
                    }

                    foreach (DataRow ro in Flights.Rows)
                    {
                        decimal grandTotal = 0;
                        ro["OriginalCost"] = ro["TotalAmount_Text"];

                        decimal[] TotalAmt = _AddMarkup(Convert.ToDecimal(ro["EquivAmount_Text"]), Convert.ToDecimal(ro["Child_TotalPrice_Id"]), Convert.ToDecimal(ro["Inf_TotalPrice_Id"]), adults, children, infants, ro["CarrierDesigCode"].ToString(), userid);

                        //decimal[] TotalAmt = _AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Childs, Infants, AirlineCode);

                        //totalfare = TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                        grandTotal = TotalAmt[2] + TotalAmt[3] + TotalAmt[4] + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                        ro["TotalAmount_Text"] = grandTotal + Convert.ToDecimal(ro["Tax"]);
                    }

                    Flights.Columns["FlightType"].SetOrdinal(0);
                    Flights.Columns["Direct"].SetOrdinal(1);

                    foreach (DataRow dr in Flights.Rows)
                    {
                        string[] str;
                        string[] _str;
                        str = dr["PaxSegmentRefID_Text"].ToString().Split('~');
                        _str = dr["PaxSegmentID"].ToString().Split('~');

                        dr["PaxSegmentRefID_Text"] = str[0];
                        dr["PaxSegmentID"] = _str[0];
                    }

                    Flights.Columns.Add("ShoppingResponseRefID");
                    Flights.Columns.Add("weight");
                    Flights.Columns.Add("unitOfMeasureCode");

                    Flights.Columns["ShoppingResponseRefID"].SetOrdinal(0);

                    string CityName = "";

                    foreach (DataRow dr in Flights.Rows)
                    {
                        dr["ShoppingResponseRefID"] = ds.Tables["ShoppingResponse"].Rows[0][0];
                        CityName = getCityName(dr["Dep_StationName"].ToString());

                        if (CityName != null && CityName != "")
                        {
                            dr["Dep_StationName"] = CityName;
                        }
                    }

                    CityName = null;

                    // Commnent above 9 lines before publishing

                    Guid TransactionId = Guid.NewGuid();

                    Flights.Columns.Add("TransactionId");
                    foreach (DataRow dr in Flights.Rows)
                    {
                        dr["TransactionId"] = TransactionId;
                    }

                    DataView dv = Flights.DefaultView;
                    // dv.Sort = "GrandTotal";
                    Flights = dv.ToTable();

                    //MemoryStream stream = new MemoryStream();
                    //ds.WriteXml(stream);
                    //byte[] hititResponse = stream.ToArray();
                    //stream = null;

                    //MemoryStream streamFL = new MemoryStream();
                    //dtFareList.TableName = "dtFareList";
                    //dtFareList.WriteXml(streamFL);
                    //byte[] fareList = streamFL.ToArray();
                    //streamFL = null;

                    //MemoryStream streamCI = new MemoryStream();
                    //dtCityInfo.TableName = "dtCityInfo";
                    //dtCityInfo.WriteXml(streamCI);
                    //byte[] cityInfo = streamCI.ToArray();
                    //streamCI = null;

                    //   ds.Tables["flightNotes"].WriteXml(stream);
                    //   byte[] flightNotes = stream.ToArray();

                    //MemoryStream streamFligts = new MemoryStream();
                    //Flights.TableName = "flights";
                    //Flights.WriteXml(streamFligts);
                    //byte[] finalResult = streamFligts.ToArray();
                    //streamFligts = null;


                    //HitItResult hr = new HitItResult();
                    //hr.TransactionId = TransactionId;
                    //hr.Origin = from;
                    //hr.Destination = to;
                    //hr.TotalPax = adults + children + infants;
                    //hr.StartDate = Dept;
                    //hr.HititResponse = hititResponse;
                    //// hr.FareList = fareList;
                    ////  hr.CityInfo = cityInfo;
                    //// hr.FlightNotes = flightNotes;
                    //hr.FinalResult = finalResult;
                    //hr.Active = true;
                    //sql.HitItResults.Add(hr);
                    //sql.SaveChanges();

                    string JSONString = "something went wrong";
                    //if (Flights != null)
                    //{
                    //    JSONString = JsonConvert.SerializeObject(Flights);
                    //}

                    // Parse the JSON string into a JObject for further processing
                    //  JObject jsonObject = JObject.Parse(jsonString);

                    JSONString = DataTableToJSONWithStringBuilder(Flights);
                    return JSONString;


                }
                return "something went wrong";
            }
            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message;
                return ex.Message.ToString();// "something went wrong";
            }
        }

        [NonAction]
        public static List<NDCFlight> ConvertDataTableToList(DataTable dataTable)
        {
            var flights = new List<NDCFlight>();

            foreach (DataRow row in dataTable.Rows)
            {
                var flight = new NDCFlight
                {
                    Arr_AircraftScheduledDateTime = row["Arr_AircraftScheduledDateTime"].ToString(),
                    Dep_AircraftScheduledDateTime = row["Dep_AircraftScheduledDateTime"].ToString(),
                    Offer_ID = row["Offer_ID"].ToString()
                };
                flights.Add(flight);
            }

            return flights;
        }

        private static TimeSpan CalculateTotalDuration(IGrouping<string, NDCFlight> flightGroup)
        {
            var sortedFlights = flightGroup.OrderBy(f => DateTime.Parse(f.Dep_AircraftScheduledDateTime, CultureInfo.InvariantCulture)).ToList();
            var departureTime = DateTime.Parse(sortedFlights.First().Dep_AircraftScheduledDateTime, CultureInfo.InvariantCulture);
            var arrivalTime = DateTime.Parse(sortedFlights.Last().Arr_AircraftScheduledDateTime, CultureInfo.InvariantCulture);
            return arrivalTime - departureTime;
        }

        private static string FormatDuration(TimeSpan duration)
        {
            int hours = (int)duration.TotalHours;
            int minutes = duration.Minutes;
            return $"{hours}H{minutes}M";
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
        public string getCityName(string IATA)
        {
            var cityName = (from c in sql.Cities.AsEnumerable()
                            where c.IATA == IATA
                            select new
                            {
                                value = c.cityName
                            }).FirstOrDefault();

            return cityName?.value.ToString();
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

        [HttpGet("hititreturnflights")]
        public async Task<string> HitItReturnFlights(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr,
        string fromM_1, string fromM_2, string fromM_3, string fromM_4, string fromM_5, string fromM_6,
        string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string airline = "", string userid = "-1")
        {
            try
            {
                // MexCoreWebAPI.Model.NDC.Envelope ndcFlights = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string url = "https://app.crane.aero/cranendc/v20.1/CraneNDCService";
                // string url = "https://app-stage.crane.aero/cranendc/v20.1/CraneNDCService";


                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");
                request.Headers.Add("username", "PSA2746366");
                // request.Headers.Add("password", "Test1234"); //Test password
                request.Headers.Add("password", "Sam7890@#_"); // production password

                var xmlReq = HitItXMLReq(tripType, from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

                DataSet ds = new DataSet();



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

                    if (!ds.Tables.Contains("Price"))
                    {
                        return "something went wrong";
                    }

                    DataTable dtPrice = ds.Tables["Price"].Copy();
                    dtPrice.Columns.Add("PaxType");

                    int TotalOffers = Convert.ToInt32(ds.Tables["AllOffersSummary"].Rows[0][0].ToString());
                    int TotalPaxTypes = dtPrice.Rows.Count / TotalOffers;

                    for (int s = 0; s < dtPrice.Rows.Count; s++)
                    {
                        if (adults > 0)
                        {
                            dtPrice.Rows[s]["PaxType"] = "ADT";
                            s = s + (TotalPaxTypes - 1);
                        }
                    }

                    if (TotalPaxTypes > 1)
                    {
                        if (children > 0)
                        {
                            for (int s = 1; s < dtPrice.Rows.Count; s++)
                            {
                                if (dtPrice.Rows[s]["PaxType"] != "")
                                {
                                    if (children > 0)
                                    {
                                        dtPrice.Rows[s]["PaxType"] = "CHD";
                                        s = s + (TotalPaxTypes - 1);
                                    }
                                }
                            }
                        }

                        if (infants > 0)
                        {
                            int inf = 0;

                            if (children == 0)
                            {
                                inf = 1;
                            }
                            else
                            {
                                inf = TotalPaxTypes - 1;
                            }

                            for (int s = inf; s < dtPrice.Rows.Count; s++)
                            {
                                if (dtPrice.Rows[s]["PaxType"] != "")
                                {
                                    if (infants > 0)
                                    {
                                        dtPrice.Rows[s]["PaxType"] = "INF";
                                        s = s + (TotalPaxTypes - 1);
                                    }
                                }
                            }
                        }
                    }

                    if (!ds.Tables.Contains("PaxJourneyRefID"))
                    {
                        // Create the table
                        DataTable paxJourneyRefIDTable = new DataTable("PaxJourneyRefID");
                        paxJourneyRefIDTable.Columns.Add("PaxJourneyRefID_Text", typeof(string));
                        paxJourneyRefIDTable.Columns.Add("OriginDest_Id", typeof(int));

                        // Add the table to the DataSet
                        ds.Tables.Add(paxJourneyRefIDTable);
                    }

                    // Retrieve the PaxJourney table
                    DataTable paxJourneyTable = ds.Tables["PaxJourney"];

                    // Populate the PaxJourneyRefID table
                    foreach (DataRow row in paxJourneyTable.Rows)
                    {
                        DataRow newRow = ds.Tables["PaxJourneyRefID"].NewRow();
                        newRow["PaxJourneyRefID_Text"] = row["PaxJourneyID"]; // Assuming 'PaxJourneyID' is a column in 'PaxJourney'
                        newRow["OriginDest_Id"] = "0"; // Insert '0' into the PaxJourneyRefID_Text column
                        ds.Tables["PaxJourneyRefID"].Rows.Add(newRow);
                    }

                    if (!ds.Tables["OriginDest"].Columns.Contains("OriginDest_Id"))
                    {
                        ds.Tables["OriginDest"].Columns.Add("OriginDest_Id", typeof(int));

                        int s = 0;
                        foreach (DataRow row in ds.Tables["OriginDest"].Rows)
                        {
                            row["OriginDest_Id"] = s;
                            s++;
                        }

                    }

                    var OriginDest = (from od in ds.Tables["OriginDest"].AsEnumerable()
                                      join odl in ds.Tables["OriginDestList"].AsEnumerable() on od["OriginDestList_Id"] equals odl["OriginDestList_Id"]
                                      join pjr in ds.Tables["PaxJourneyRefID"].AsEnumerable() on od["OriginDest_Id"] equals pjr["OriginDest_Id"]
                                      join pjl in ds.Tables["PaxJourneyList"].AsEnumerable() on odl["DataLists_Id"] equals pjl["DataLists_Id"]
                                      // join pj in ds.Tables["PaxJourney"].AsEnumerable() on pjl["PaxJourneyList_Id"] equals pj["PaxJourneyList_Id"]

                                      select new
                                      {
                                          OriginDestList_Id = odl["OriginDestList_Id"],
                                          DataLists_Id = odl["DataLists_Id"],
                                          DestCode = od["DestCode"],
                                          OriginCode = od["OriginCode"],
                                          OriginDestID = od["OriginDestID"],
                                          OriginDest_Id = od["OriginDest_Id"],
                                          PaxJourneyRefID_Text = pjr["PaxJourneyRefID_Text"],
                                          PaxJourneyList_Id = pjl["PaxJourneyList_Id"],
                                          // PaxJourneyID = pj["PaxJourneyID"],
                                          // PaxSegmentRefID = pj["PaxSegmentRefID"]
                                      }).ToList();

                    DataTable dtSegments = LINQResultToDataTable(OriginDest);
                    dtSegments.Columns.Add("PaxSegmentRefID");
                    dtSegments.Columns.Add("PaxJourney_ID");

                    foreach (DataRow dr in dtSegments.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["PaxJourney"].Rows)
                        {
                            if (dr["PaxJourneyList_Id"].ToString() == dr1["PaxJourneyList_Id"].ToString())
                            {
                                if (ds.Tables["PaxJourney"].Columns.Contains("PaxSegmentRefID") == true)
                                {
                                    dr["PaxSegmentRefID"] = dr1["PaxSegmentRefID"];
                                }
                                else if (ds.Tables["PaxJourney"].Columns.Contains("PaxJourney_ID") == true)
                                {
                                    dr["PaxJourney_ID"] = dr1["PaxJourney_ID"];
                                }
                            }
                        }
                    }


                    int i = 0;
                    foreach (DataRow dr in ds.Tables["PaxSegmentRefID"].Rows)
                    {
                        dr["PaxSegmentRefID_Text"] = dr["PaxSegmentRefID_Text"] + "~" + i++;
                    }

                    i = 0;
                    foreach (DataRow dr in ds.Tables["PaxSegment"].Rows)
                    {
                        dr["PaxSegmentID"] = dr["PaxSegmentID"] + "~" + i++;
                    }


                    var PaxSegment = (from psr in ds.Tables["PaxSegmentRefID"].AsEnumerable()
                                      join ps in ds.Tables["PaxSegment"].AsEnumerable() on psr["PaxSegmentRefID_Text"] equals ps["PaxSegmentID"]
                                      join arr in ds.Tables["Arrival"].AsEnumerable() on ps["PaxSegment_Id"] equals arr["PaxSegment_Id"]
                                      join dep in ds.Tables["Dep"].AsEnumerable() on ps["PaxSegment_Id"] equals dep["PaxSegment_Id"]
                                      select new
                                      {
                                          PaxSegmentRefID_Text = psr["PaxSegmentRefID_Text"],
                                          PaxJourney_Id = psr["PaxJourney_Id"],
                                          PaxSegmentID = ps["PaxSegmentID"],
                                          PaxSegment_Id = ps["PaxSegment_Id"],
                                          Duration = ps["Duration"],
                                          Arr_AircraftScheduledDateTime = arr["AircraftScheduledDateTime"],
                                          Arr_IATA_LocationCode = arr["IATA_LocationCode"],
                                          Arr_StationName = arr["StationName"],
                                          Dep_AircraftScheduledDateTime = dep["AircraftScheduledDateTime"],
                                          Dep_IATA_LocationCode = dep["IATA_LocationCode"],
                                          Dep_StationName = dep["StationName"],
                                      }).ToList();


                    DataTable dtPaxSegment = LINQResultToDataTable(PaxSegment);
                    dtPaxSegment.Columns.Add("PaxJourneyID");

                    dtPaxSegment.Columns["PaxJourneyID"].SetOrdinal(2);


                    foreach (DataRow dr in dtPaxSegment.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["PaxJourney"].Rows)
                        {
                            if (dr["PaxJourney_Id"].ToString() == dr1["PaxJourney_Id"].ToString())
                            {
                                dr["PaxJourneyID"] = dr1["PaxJourneyID"];
                            }
                        }
                    }


                    var offers = (from o in ds.Tables["offer"].AsEnumerable()
                                  join j in ds.Tables["JourneyOverview"].AsEnumerable()
                                  on o.Field<int?>("Offer_Id") equals j.Field<int?>("Offer_Id")
                                  join jp in ds.Tables["JourneyPriceClass"].AsEnumerable()
                                  on j.Field<int?>("JourneyOverview_Id") equals jp.Field<int?>("JourneyOverview_Id")
                                  join OI in ds.Tables["OfferItem"].AsEnumerable()
                                   on o.Field<int?>("Offer_Id") equals OI.Field<int?>("Offer_Id")
                                  join FD in ds.Tables["FareDetail"].AsEnumerable()
                                   on OI.Field<int?>("OfferItem_Id") equals FD.Field<int?>("OfferItem_Id")
                                  join FC in ds.Tables["FareComponent"].AsEnumerable()
                                   on FD.Field<int?>("FareDetail_Id") equals FC.Field<int?>("FareDetail_Id")
                                  join CT in ds.Tables["CabinType"].AsEnumerable()
                                   on FC.Field<int?>("FareComponent_Id") equals CT.Field<int?>("FareComponent_Id")
                                  join RB in ds.Tables["RBD"].AsEnumerable()
                                                       on CT.Field<int?>("FareComponent_Id") equals RB.Field<int?>("FareComponent_Id")
                                  join FP in ds.Tables["FarePriceType"].AsEnumerable()
                                                       on FD.Field<int?>("FareDetail_Id") equals FP.Field<int?>("FareDetail_Id")
                                  join OPT in ds.Tables["OfferItemPaymentTimeLimit"].AsEnumerable()
                                                       on OI.Field<int?>("OfferItem_Id") equals OPT.Field<int?>("OfferItem_Id")
                                  join PTL in ds.Tables["PaymentTimeLimitDate"].AsEnumerable()
                                                       on OPT.Field<int?>("OfferItemPaymentTimeLimit_Id") equals PTL.Field<int?>("OfferItemPaymentTimeLimit_Id")
                                  join Pr in dtPrice.AsEnumerable()
                                                       on OI.Field<int?>("OfferItem_Id") equals Pr.Field<int?>("OfferItem_Id")
                                  join EA in ds.Tables["EquivAmount"].AsEnumerable()
                                                       on Pr.Field<int?>("Price_Id") equals EA.Field<int?>("Price_Id")
                                  select new
                                  {
                                      Offer_Id = o.Field<int?>("Offer_Id"),
                                      PaxJourneyRefID = jp.Field<string>("PaxJourneyRefID"),
                                      OfferID = o.Field<string?>("OfferID"),
                                      OwnerCode = o.Field<string?>("OwnerCode"),
                                      CarrierOffers_Id = o.Field<int?>("CarrierOffers_id"),
                                      JourneyOverview_Id = j.Field<int?>("JourneyOverview_Id"),
                                      OfferItem_Id = OI.Field<int?>("OfferItem_Id"),
                                      MandatoryInd = OI.Field<string?>("MandatoryInd"),
                                      OfferItemID = OI.Field<string>("OfferItemID"),
                                      FareDetail_Id = FD.Field<int?>("FareDetail_Id"),
                                      FareComponent_Id = FC.Field<int?>("FareComponent_Id"),
                                      FareBasisCode = FC.Field<string?>("FareBasisCode"),
                                      FareTypeCode = FC.Field<string?>("FareTypeCode"),
                                      PaxSegmentRefID = FC.Field<string?>("PaxSegmentRefID"),
                                      CabinTypeCode = CT.Field<string?>("CabinTypeCode"),
                                      CabintypeName = CT.Field<string?>("CabinTypeName"),
                                      RBD_Code = RB.Field<string?>("RBD_Code"),
                                      FarePriceTypeCode = FP.Field<string?>("FarePriceTypeCode"),
                                      Price = FP.Field<string?>("Price"),
                                      OfferItemPaymentTimeLimit_Id = OPT.Field<int?>("OfferItemPaymentTimeLimit_Id"),
                                      PaymentTimeLimitDateTime = PTL.Field<string?>("PaymentTimeLimitDateTime"),
                                      Price_Id = Pr.Field<int?>("Price_Id"),
                                      PaxType = Pr.Field<string?>("PaxType"),
                                      CurCode = EA.Field<string?>("CurCode"),
                                      EquivAmount_Text = EA.Field<string?>("EquivAmount_Text"),

                                  }).ToList();

                    DataTable dtOffers = LINQResultToDataTable(offers);
                    dtOffers.Columns.Add("TotalPrice_Id");
                    dtOffers.Columns.Add("Tax");
                    dtOffers.Columns.Add("TotalAmount_Text");


                    foreach (DataRow dr in dtOffers.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["TotalPrice"].Rows)
                        {
                            if (dr["Offer_Id"].ToString() == dr1["Offer_Id"].ToString())
                            {
                                dr["TotalPrice_Id"] = dr1["TotalPrice_Id"];
                            }
                        }
                    }


                    foreach (DataRow dr in dtOffers.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["TotalAmount"].Rows)
                        {
                            if (dr["TotalPrice_Id"].ToString() == dr1["TotalPrice_Id"].ToString())
                            {
                                dr["TotalAmount_Text"] = dr1["TotalAmount_Text"];
                            }
                        }
                    }

                    var Taxes = from tp in ds.Tables["TotalPrice"].AsEnumerable()
                                join t in ds.Tables["TaxSummary"].AsEnumerable() on tp.Field<int?>("TotalPrice_ID") equals t.Field<int?>("TotalPrice_Id")
                                join tt in ds.Tables["TotalTaxAmount"].AsEnumerable() on t.Field<int?>("TaxSummary_ID") equals tt.Field<int?>("TaxSummary_Id")
                                select new
                                {
                                    TotalPrice_ID = tp.Field<int?>("TotalPrice_ID"),
                                    Offer_Id = tp.Field<int?>("Offer_Id"),
                                    TaxSummary_ID = t.Field<int?>("TaxSummary_ID"),
                                    CurCode = tt.Field<string?>("CurCode"),
                                    TotalTaxAmount_Text = tt.Field<string?>("TotalTaxAmount_Text")
                                };
                    DataTable dtTaxes = LINQResultToDataTable(Taxes);

                    foreach (DataRow dr in dtOffers.Rows)
                    {
                        foreach (DataRow dr1 in dtTaxes.Rows)
                        {
                            if (dr["TotalPrice_Id"].ToString() == dr1["TotalPrice_Id"].ToString())
                            {
                                dr["Tax"] = dr1["TotalTaxAmount_Text"];
                            }
                        }
                    }


                    DataTable Flights = new DataTable();
                    Flights = dtPaxSegment.Copy();

                    Flights.Columns.Add("Offer_ID");
                    Flights.Columns.Add("OfferID");
                    Flights.Columns.Add("OwnerCode");
                    Flights.Columns.Add("CarrierOffers_Id");
                    Flights.Columns.Add("JourneyOverview_Id");
                    Flights.Columns.Add("PaxJourneyRefID");
                    Flights.Columns.Add("OfferItem_Id");
                    Flights.Columns.Add("MandatoryInd");
                    Flights.Columns.Add("OfferItemID");
                    Flights.Columns.Add("FareDetail_Id");
                    Flights.Columns.Add("FareComponent_Id");
                    Flights.Columns.Add("FareBasisCode");
                    Flights.Columns.Add("FareTypeCode");
                    Flights.Columns.Add("PaxSegmentRefID");
                    Flights.Columns.Add("CabinTypeCode");
                    Flights.Columns.Add("CabintypeName");
                    Flights.Columns.Add("RBD_Code");
                    Flights.Columns.Add("FarePriceTypeCode");
                    Flights.Columns.Add("Price");
                    Flights.Columns.Add("OfferItemPaymentTimeLimit_Id");
                    Flights.Columns.Add("PaymentTimeLimitDateTime");
                    Flights.Columns.Add("Price_Id");
                    Flights.Columns.Add("CurCode");
                    Flights.Columns.Add("EquivAmount_Text");
                    Flights.Columns.Add("FlightType");
                    Flights.Columns.Add("Direct");
                    Flights.Columns.Add("TotalPrice_Id");
                    Flights.Columns.Add("Tax");
                    Flights.Columns.Add("TotalAmount_Text");
                    Flights.Columns.Add("Child_TotalPrice_Id");
                    Flights.Columns.Add("Inf_TotalPrice_Id");

                    Flights.Columns["PaxJourneyRefID"].SetOrdinal(1);
                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in dtOffers.Rows)
                        {
                            if (dr["PaxJourneyId"].ToString() == dr1["PaxJourneyRefID"].ToString())
                            {
                                if (dr1["PaxType"].ToString() == "ADT")
                                {
                                    dr["Offer_ID"] = dr1["Offer_ID"];
                                    dr["OfferID"] = dr1["OfferID"];
                                    dr["OwnerCode"] = dr1["OwnerCode"];
                                    dr["CarrierOffers_Id"] = dr1["CarrierOffers_Id"];
                                    dr["JourneyOverview_Id"] = dr1["JourneyOverview_Id"];
                                    dr["PaxJourneyRefID"] = dr1["PaxJourneyRefID"];
                                    dr["OfferItem_Id"] = dr1["OfferItem_Id"];
                                    dr["MandatoryInd"] = dr1["MandatoryInd"];
                                    dr["OfferItemID"] = dr1["OfferItemID"];
                                    dr["FareDetail_Id"] = dr1["FareDetail_Id"];
                                    dr["FareComponent_Id"] = dr1["FareComponent_Id"];
                                    dr["FareBasisCode"] = dr1["FareBasisCode"];
                                    dr["FareTypeCode"] = dr1["FareTypeCode"];
                                    dr["PaxSegmentRefID"] = dr1["PaxSegmentRefID"];
                                    dr["CabinTypeCode"] = dr1["CabinTypeCode"];
                                    dr["CabintypeName"] = dr1["CabintypeName"];
                                    dr["RBD_Code"] = dr1["RBD_Code"];
                                    dr["FarePriceTypeCode"] = dr1["FarePriceTypeCode"];
                                    dr["Price"] = dr1["Price"];
                                    dr["OfferItemPaymentTimeLimit_Id"] = dr1["OfferItemPaymentTimeLimit_Id"];
                                    dr["PaymentTimeLimitDateTime"] = dr1["PaymentTimeLimitDateTime"];
                                    dr["Price_Id"] = dr1["Price_Id"];
                                    dr["CurCode"] = dr1["CurCode"];
                                    dr["EquivAmount_Text"] = dr1["EquivAmount_Text"];
                                    dr["TotalPrice_Id"] = dr1["TotalPrice_Id"];
                                    dr["Tax"] = dr1["Tax"];
                                    dr["TotalAmount_Text"] = dr1["TotalAmount_Text"];
                                    dr["Child_TotalPrice_Id"] = 0;
                                    dr["Inf_TotalPrice_Id"] = 0;
                                }
                            }
                        }
                    }
                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in dtOffers.Rows)
                        {
                            if (dr["PaxJourneyId"].ToString() == dr1["PaxJourneyRefID"].ToString())
                            {
                                if (dr1["PaxType"].ToString() == "CHD")
                                {
                                    dr["Child_TotalPrice_Id"] = dr1["EquivAmount_Text"];
                                }
                            }
                        }
                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in dtOffers.Rows)
                        {
                            if (dr["PaxJourneyId"].ToString() == dr1["PaxJourneyRefID"].ToString())
                            {
                                if (dr1["PaxType"].ToString() == "INF")
                                {
                                    dr["Inf_TotalPrice_Id"] = dr1["EquivAmount_Text"];
                                }
                            }
                        }
                    }

                    Flights.Columns.Add("CarrierDesigCode");
                    Flights.Columns.Add("CarrierName");
                    Flights.Columns.Add("OperatingCarrierFlightNumberText");
                    Flights.Columns.Add("OriginalCost");

                    Flights.Columns.Add("TotalDuration");

                    List<NDCFlight> flights = ConvertDataTableToList(Flights);

                    var flightDurations = from flight in flights
                                          group flight by flight.Offer_ID into flightGroup
                                          select new
                                          {
                                              Offer_Id = flightGroup.Key,
                                              TotalDuration = FormatDuration(CalculateTotalDuration(flightGroup))
                                          };

                    foreach (DataRow fl in Flights.Rows)
                    {
                        foreach (var itm in flightDurations)
                        {
                            if (fl["Offer_Id"].ToString() == itm.Offer_Id.ToString())
                            {
                                fl["TotalDuration"] = itm.TotalDuration;
                            }
                        }

                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["OperatingCarrierInfo"].Rows)
                        {
                            if (dr["PaxSegment_Id"].ToString() == dr1["PaxSegment_Id"].ToString())
                            {
                                dr["OperatingCarrierFlightNumberText"] = dr1["OperatingCarrierFlightNumberText"];
                                dr["CarrierName"] = dr1["CarrierName"];
                                dr["CarrierDesigCode"] = dr1["CarrierDesigCode"];
                            }
                        }
                    }

                    foreach (DataRow ro in Flights.Rows)
                    {
                        decimal grandTotal = 0;
                        ro["OriginalCost"] = ro["TotalAmount_Text"];

                        decimal[] TotalAmt = _AddMarkup(Convert.ToDecimal(ro["EquivAmount_Text"]), Convert.ToDecimal(ro["Child_TotalPrice_Id"]), Convert.ToDecimal(ro["Inf_TotalPrice_Id"]), adults, children, infants, ro["CarrierDesigCode"].ToString(), userid);

                        //decimal[] TotalAmt = _AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Childs, Infants, AirlineCode);

                        //totalfare = TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                        grandTotal = TotalAmt[2] + TotalAmt[3] + TotalAmt[4] + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                        ro["TotalAmount_Text"] = grandTotal + Convert.ToDecimal(ro["Tax"]);
                    }

                    Flights.Columns["FlightType"].SetOrdinal(0);
                    Flights.Columns["Direct"].SetOrdinal(1);

                    int journeyId = 0;

                    if (Flights.Rows.Count > 0)
                    {
                        journeyId = Convert.ToInt32(Flights.Rows[0]["PaxJourney_ID"]);
                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        //if (dr["PaxJourney_ID"].ToString() == journeyId.ToString())
                        {
                            if (dr["Dep_IATA_LocationCode"].ToString() == from && dr["Arr_IATA_LocationCode"].ToString() == to)
                            {
                                dr["FlightType"] = "DM";
                                dr["Direct"] = 1;
                            }
                            else if (dr["Dep_IATA_LocationCode"].ToString() == from && dr["Arr_IATA_LocationCode"].ToString() != to)
                            {
                                dr["FlightType"] = "DM";
                                dr["Direct"] = 0;
                            }
                            else if ((dr["Dep_IATA_LocationCode"].ToString() == to && dr["Arr_IATA_LocationCode"].ToString() == from))
                            {
                                dr["FlightType"] = "RM";
                                dr["Direct"] = 1;
                            }
                            else if ((dr["Dep_IATA_LocationCode"].ToString() == to && dr["Arr_IATA_LocationCode"].ToString() != from))
                            {
                                dr["FlightType"] = "RM";
                                dr["Direct"] = 0;
                            }
                            else
                            {
                                journeyId += 1;
                            }
                        }
                    }

                    for (int s = 0; s < Flights.Rows.Count; s++)
                    {
                        DataRow currentRow = Flights.Rows[s];

                        if (currentRow[0].ToString() == "DM")
                        {
                            // Check for the next rows until the first non-blank row
                            int j = s + 1;
                            while (j < Flights.Rows.Count && string.IsNullOrWhiteSpace(Flights.Rows[j][0].ToString()))
                            {
                                Flights.Rows[j][0] = "DC";
                                j++;
                            }
                            s = j - 1; // Set 'i' to the last processed row
                        }
                        else if (currentRow[0].ToString() == "RM")
                        {
                            // Check for the next rows until the first non-blank row
                            int j = s + 1;
                            while (j < Flights.Rows.Count && string.IsNullOrWhiteSpace(Flights.Rows[j][0].ToString()))
                            {
                                Flights.Rows[j][0] = "RC";
                                j++;
                            }
                            s = j - 1; // Set 'i' to the last processed row
                        }
                    }


                    //Log

                    foreach (DataRow dr in Flights.Rows)
                    {
                        string[] str;
                        string[] _str;
                        str = dr["PaxSegmentRefID_Text"].ToString().Split('~');
                        _str = dr["PaxSegmentID"].ToString().Split('~');

                        dr["PaxSegmentRefID_Text"] = str[0];
                        dr["PaxSegmentID"] = _str[0];
                    }

                    Flights.Columns.Add("ShoppingResponseRefID");
                    Flights.Columns.Add("weight");
                    Flights.Columns.Add("unitOfMeasureCode");

                    Flights.Columns["ShoppingResponseRefID"].SetOrdinal(0);

                    string CityName = "";

                    foreach (DataRow dr in Flights.Rows)
                    {
                        dr["ShoppingResponseRefID"] = ds.Tables["ShoppingResponse"].Rows[0][0];
                        CityName = getCityName(dr["Dep_StationName"].ToString());

                        if (CityName != null && CityName != "")
                        {
                            dr["Dep_StationName"] = CityName;
                        }
                    }

                    CityName = null;

                    string fileName = @"C:\hallmark\doAirShopping_2A1C1I_req.txt";  //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Res.txt";
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


                    fileName = @"C:\hallmark\doAirShopping_2A1C1I_res.txt";  //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Res.txt";
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




                    // Commnent above 9 lines before publishing

                    Guid TransactionId = Guid.NewGuid();

                    Flights.Columns.Add("TransactionId");
                    foreach (DataRow dr in Flights.Rows)
                    {
                        dr["TransactionId"] = TransactionId;
                    }

                    //DataView dv = Flights.DefaultView;
                    //// dv.Sort = "GrandTotal";
                    //Flights = dv.ToTable();

                    //MemoryStream stream = new MemoryStream();
                    //ds.WriteXml(stream);
                    //byte[] hititResponse = stream.ToArray();
                    //stream = null;

                    //MemoryStream streamFL = new MemoryStream();
                    //dtFareList.TableName = "dtFareList";
                    //dtFareList.WriteXml(streamFL);
                    //byte[] fareList = streamFL.ToArray();
                    //streamFL = null;

                    //MemoryStream streamCI = new MemoryStream();
                    //dtCityInfo.TableName = "dtCityInfo";
                    //dtCityInfo.WriteXml(streamCI);
                    //byte[] cityInfo = streamCI.ToArray();
                    //streamCI = null;

                    //   ds.Tables["flightNotes"].WriteXml(stream);
                    //   byte[] flightNotes = stream.ToArray();

                    //MemoryStream streamFligts = new MemoryStream();
                    //Flights.TableName = "flights";
                    //Flights.WriteXml(streamFligts);
                    //byte[] finalResult = streamFligts.ToArray();
                    //streamFligts = null;


                    //HitItResult hr = new HitItResult();
                    //hr.TransactionId = TransactionId;
                    //hr.Origin = from;
                    //hr.Destination = to;
                    //hr.TotalPax = adults + children + infants;
                    //hr.StartDate = Dept;
                    //hr.HititResponse = hititResponse;
                    //// hr.FareList = fareList;
                    ////  hr.CityInfo = cityInfo;
                    //// hr.FlightNotes = flightNotes;
                    //hr.FinalResult = finalResult;
                    //hr.Active = true;
                    //sql.HitItResults.Add(hr);
                    //sql.SaveChanges();

                    string JSONString = "something went wrong";
                    //if (Flights != null)
                    //{
                    //    JSONString = JsonConvert.SerializeObject(Flights);
                    //}

                    // Parse the JSON string into a JObject for further processing
                    //  JObject jsonObject = JObject.Parse(jsonString);

                    JSONString = DataTableToJSONWithStringBuilder(Flights);
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

        [HttpPost("hititbooking")]
        public string HitItBooking([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                var frm = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                //string flightsList = frm?.TotalFlights; // JsonConvert.DeserializeObject<dynamic>(frm?.TotalFlights);

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
                            string flightCode = flightToken["FlightType"]?.ToString();
                            string segmentID = flightToken["PaxSegment_Id"]?.ToString();

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

                    int UserID = frm?.userID?.Value != null ? Convert.ToInt32(frm?.userID?.Value) : 1; // Convert.ToInt32(Session["UserID"]);
                    Random random = new Random();
                    string num = random.Next(9).ToString();


                    //  int UserID = user.intUserID;
                    //data.firstName_0.Value
                    int Adults = frm?.Adults?.Value != null ? Convert.ToInt32(frm?.Adults?.Value) : 1;
                    int Children = frm?.Children?.Value != null ? Convert.ToInt32(frm?.Children?.Value) : 0;
                    int Infants = frm?.Infants?.Value != null ? Convert.ToInt32(frm?.Infants?.Value) : 0;
                    decimal OriginalCost = frm?.OriginalCost?.Value != null ? Convert.ToDecimal(frm?.OriginalCost?.Value) : 0;

                    if (OriginalCost == 0)
                    {
                        OriginalCost = frm?.originalCost?.Value != null ? Convert.ToDecimal(frm?.originalCost?.Value) : 0;
                    }

                    decimal? grandtotal = frm?.GrandTotal?.Value != null ? Convert.ToDecimal(frm?.GrandTotal?.Value) : 0;

                    string CancelPenalty = frm?.CancelPenalty?.Value != null ? Convert.ToString(frm?.CancelPenalty?.Value) : "0";
                    string ChangePenalty = frm?.ChangePenalty?.Value != null ? Convert.ToString(frm?.ChangePenalty?.Value) : "0";
                    string penalty = frm.Penalty;

                    string from = frm?.from?.Value;
                    string to = frm?.to?.Value;
                    string totaltime0 = frm?.totaltime0?.Value;
                    string totaltime1 = frm?.totaltime1?.Value;
                    string stops0 = frm?.stops0?.Value;
                    string stops1 = frm?.stops1?.Value;
                    string amount = frm?.amount0?.Value;
                    string taxes = frm?.taxes0?.Value;
                    string total = frm?.total0?.Value;
                    string currency = frm?.currency0?.Value;
                    //string serviceclass = ""; //frm?.cabin.Value;
                    string optionID = ""; // frm?.Option_Id.Value;
                                          //  Guid TransactionId = Guid.Parse(frm?.TransactionId?.Value);
                    DateTime Arr_AircraftScheduledDateTime = DateTime.MinValue; //frm?["Arr_AircraftScheduledDateTime"].ToString();
                    DateTime Dep_AircraftScheduledDateTime = DateTime.MinValue;

                    string arrdateTimeString = frm?.Arr_AircraftScheduledDateTime?.Value.ToString();

                    if (!string.IsNullOrEmpty(arrdateTimeString))
                    {
                        Arr_AircraftScheduledDateTime = DateTime.Parse(arrdateTimeString);
                    }

                    string depdateTimeString = frm?.Dep_AircraftScheduledDateTime?.Value.ToString();

                    if (!string.IsNullOrEmpty(depdateTimeString))
                    {
                        Dep_AircraftScheduledDateTime = DateTime.Parse(depdateTimeString);
                    }

                    string Departure = frm?.Dep_locationCode?.Value;

                    string Arrival = frm?.Arr_locationCode?.Value;
                    string resBookDesigCode = frm?.ResBookDesigCode?.Value;
                    string ShoppingResponseRefID = frm?.ShoppingResponseRefID?.Value;
                    string OfferRefID = frm?.OfferID?.Value?.ToString();
                    string Cabin = frm?.Cabin?.Value?.ToString();

                    //if (Cabin == null || Cabin == "")
                    //{
                    //    Cabin = !string.IsNullOrEmpty(frm?.RBD_Code.Value) ? frm?.RBD_Code.Value.ToString() : "";

                    //}

                    string fareComponentGroupList_Id = frm?.FareComponentGroupList_Id?.Value;

                    string leadfirstname = !string.IsNullOrEmpty(frm?.firstName_0.Value) ? frm?.firstName_0.Value.ToString().ToUpper() : "";
                    string leadlastname = !string.IsNullOrEmpty(frm?.lastName_0.Value) ? frm?.lastName_0.Value.ToString().ToUpper() : "";
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

                    string flightType = !string.IsNullOrEmpty(frm?.flightType?.Value) ? frm?.flightType?.Value.ToString().ToUpper() : "";

                    string fareReferenceID = !string.IsNullOrEmpty(frm?.fareReferenceID?.Value) ? frm?.fareReferenceID?.Value.ToString() : "";

                    string resBookDesigCode1 = "";
                    string fareComponentGroupList_Id1 = "";
                    string optionID_r = "";
                    //Guid TransactionId = Guid.Parse(frm?.TransactionId_r.Value);
                    DateTime Arr_AircraftScheduledDateTime_r = DateTime.Now;
                    string Departure_r = "";
                    DateTime Dep_AircraftScheduledDateTime_r = DateTime.Now;
                    string Arrival_r = "";
                    string resBookDesigCode_r = "";
                    string Cabin_r = "";
                    string fareComponentGroupList_Id_r = "0"; // frm?.FareComponentGroupList_Id_r.Value;

                    if (flightType == "RETURN")
                    {
                        fareComponentGroupList_Id_r = frm?.FareComponentGroupList_Id_r?.Value;
                        resBookDesigCode1 = frm?.ResBookDesigCode_r?.Value;
                        fareComponentGroupList_Id1 = frm?.FareComponentGroupList_Id_r?.Value;
                        optionID_r = frm?.Option_Id_r?.Value;
                        Arr_AircraftScheduledDateTime_r = frm?.Arr_AircraftScheduledDateTime_r?.Value;
                        Departure_r = frm?.Dep_locationCode_r?.Value;
                        Dep_AircraftScheduledDateTime_r = frm?.Dep_AircraftScheduledDateTime_r?.Value;
                        Arrival_r = frm?.Arr_locationCode_r?.Value;
                        resBookDesigCode_r = frm?.ResBookDesigCode_r?.Value;
                        Cabin_r = frm?.Cabin_r?.Value;
                    }




                    string _ticket = ""; // frm?.chkTicket.Value;

                    DateTime datet = DateTime.Now.Date;
                    string Dat = datet.ToString("yyyy-MM-dd");

                    DateTime dt1 = DateTime.UtcNow;
                    string tstamp = dt1.ToString("s") + "Z";
                    //string AirlineCode = "PK";
                    //bool error = false;
                    string email = !string.IsNullOrEmpty(frm?.email_0?.Value) ? frm?.email_0?.Value.ToString() : "";
                    string contact = !string.IsNullOrEmpty(frm?.MobileNO_0?.Value) ? frm?.MobileNO_0?.Value.ToString() : "";



                    #region XML Request

                    string xmlRequest = "";
                    StringBuilder strReq = new StringBuilder();
                    title = "Mr";
                    string PaxRefID = "";

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

                            title = "Mr";
                            if (sex == "F")
                            {
                                sex = "F";
                                title = "Ms";
                            }
                            else
                            {
                                sex = "M";
                            }

                            if (i == 0)
                            {
                                strReq.Append(@"<ContactInfoList>");
                                strReq.Append(@"<ContactInfo>");
                                strReq.Append(@"<ContactInfoID>Contact-1</ContactInfoID>");
                                strReq.Append(@"<EmailAddress>");
                                strReq.Append(@"<EmailAddressText>" + email + "</EmailAddressText>");
                                strReq.Append(@"</EmailAddress>");
                                strReq.Append(@"<Phone>");
                                strReq.Append(@"<AreaCodeNumber>" + mobile.Substring(0, 3) + "</AreaCodeNumber>");
                                strReq.Append(@"<CountryDialingCode>92</CountryDialingCode>");
                                strReq.Append(@"<PhoneNumber>" + mobile.Substring(3, 7) + "</PhoneNumber>");
                                strReq.Append(@"</Phone>");
                                strReq.Append(@"</ContactInfo>");
                                strReq.Append(@"</ContactInfoList>");
                                strReq.Append(@"<PaxList>");
                            }


                            strReq.Append(@"<Pax>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ContactInfoRefID>Contact-1</ContactInfoRefID>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID > ");
                            strReq.Append(@"<IdentityDocTypeCode>NATIONAL_ID</IdentityDocTypeCode >");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID>");
                            strReq.Append(@"<IdentityDocTypeCode>PASSPORT</IdentityDocTypeCode>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ExpiryDate>" + expiry + "</ExpiryDate>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<Individual>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<IndividualID>IND-ADT" + i + "</IndividualID>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"<TitleName>" + title + "</TitleName>");
                            strReq.Append(@"</Individual>");
                            strReq.Append(@"<PaxID>PAX-ADT" + i + "</PaxID>");
                            strReq.Append(@"<PTC>ADT</PTC>");
                            strReq.Append(@"</Pax>");

                            PaxRefID = PaxRefID + "PAX-ADT" + i + " ";
                        }
                    }

                    if (Children > 0)
                    {
                        for (int c = 1; c <= Children; c++)
                        {
                            sex = frm?["sexC_" + c.ToString()]?.ToString();
                            document = frm?["documentC_" + c.ToString()]?.ToString();
                            documenttype = frm?["documenttypeC_" + c.ToString()]?.ToString();
                            expiry = frm?["expiryC_" + c.ToString()]?.ToString();
                            nationality = frm?["nationalityC_" + c.ToString()]?.ToString();
                            firstName = frm?["firstNameC_" + c.ToString()]?.ToString();
                            lastName = frm?["lastNameC_" + c.ToString()]?.ToString();
                            DateTime dtDOB = Convert.ToDateTime(frm?["dobC_" + c.ToString()]?.ToString());
                            //TimeSpan t = DateTime.Now - dtDOB;
                            dob = frm?["dobC_" + c.ToString()]?.ToString().ToString();

                            title = "Mr";
                            if (sex == "F")
                            {
                                sex = "F";
                                title = "Ms";
                            }
                            else
                            {
                                sex = "M";
                            }
                            strReq.Append(@"<Pax>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ContactInfoRefID>Contact-1</ContactInfoRefID>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID > ");
                            strReq.Append(@"<IdentityDocTypeCode>NATIONAL_ID</IdentityDocTypeCode >");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID>");
                            strReq.Append(@"<IdentityDocTypeCode>PASSPORT</IdentityDocTypeCode>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ExpiryDate>" + expiry + "</ExpiryDate>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<Individual>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<IndividualID>IND-CHD" + c + "</IndividualID>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"<TitleName>" + title + "</TitleName>");
                            strReq.Append(@"</Individual>");
                            strReq.Append(@"<PaxID>PAX-CHD" + c + "</PaxID>");
                            strReq.Append(@"<PTC>CHD</PTC>");
                            strReq.Append(@"</Pax>");

                            PaxRefID = PaxRefID + "PAX-CHD" + c + " ";
                        }
                    }


                    if (Infants > 0)
                    {
                        for (int i = 1; i <= Infants; i++)
                        {

                            dob = frm?["dobI_" + i.ToString()]?.ToString();
                            sex = frm?["sexI_" + i.ToString()]?.ToString();
                            document = frm?["documentI_" + i.ToString()]?.ToString();
                            documenttype = frm?["documenttypeI_" + i.ToString()]?.ToString();
                            expiry = frm?["expiryI_" + i.ToString()]?.ToString();
                            nationality = frm?["nationalityI_" + i.ToString()]?.ToString();
                            firstName = frm?["firstNameI_" + i.ToString()]?.ToString();
                            lastName = frm?["lastNameI_" + i.ToString()]?.ToString();

                            title = "Mr";
                            if (sex == "F")
                            {
                                sex = "F";
                                title = "Ms";
                            }
                            else
                            {
                                sex = "M";
                            }


                            strReq.Append(@"<Pax>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ContactInfoRefID>Contact-1</ContactInfoRefID>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID > ");
                            strReq.Append(@"<IdentityDocTypeCode>NATIONAL_ID</IdentityDocTypeCode >");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID>");
                            strReq.Append(@"<IdentityDocTypeCode>PASSPORT</IdentityDocTypeCode>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ExpiryDate>" + expiry + "</ExpiryDate>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<Individual>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<IndividualID>IND-INF" + i + "</IndividualID>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"<TitleName>" + title + "</TitleName>");
                            strReq.Append(@"</Individual>");
                            strReq.Append(@"<PaxID>PAX-INF" + i + "</PaxID>");
                            strReq.Append(@"<PaxRefID>PAX-ADT0</PaxRefID>");
                            strReq.Append(@"<PTC>INF</PTC>");
                            strReq.Append(@"</Pax>");

                            PaxRefID = PaxRefID + "PAX-INF" + i + " ";
                        }
                    }

                    xmlRequest = OneWayBookingXML(strReq.ToString(), dtFlights, null, resBookDesigCode, ShoppingResponseRefID, null, OfferRefID, PaxRefID.Trim());

                    if (flightType == "ONEWAY")
                    {


                    }
                    else if (flightType == "RETURN")
                    {


                        //xmlRequest = ReturnBookingXML(strReq.ToString(), item, fareDetails, Returnitem, ReturnfareDetails, DepartureCity, ArrivalCity, resBookDesigCode, deiCode, explanation, note, resBookDesigCode1, deiCode1, explanation1, note1);
                    }

                    #endregion

                    //Call Booking endpoint
                    string result1 = HitItDepartureBooking(xmlRequest).Result;

                    if (result1 == "something went wrong")
                    {
                        return "something went wrong";
                    }

                    //XmlDocument doc = new XmlDocument();
                    //doc.Load("C:\\Users\\Hp\\Downloads\\ret-reposne.xml");
                    //string result1 = doc.OuterXml;

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(result1);
                    XmlReader xmlReader = new XmlNodeReader(xml);

                    DataSet ds = new DataSet();
                    ds.ReadXml(xmlReader);

                    if (ds.Tables.Count < 1)
                    {
                        return "something went wrong";
                    }

                    DataTable dtAirPricingInfo;

                    //  string AirPricingInfoKey = "1";

                    // ds2.ReadXml(AirPricingSolution);
                    List<string> Order = new List<string>();

                    if (ds.Tables.Contains("Order"))
                    {
                        foreach (DataRow ro in ds.Tables["Order"].Rows)
                        {
                            Order.Add(ro["OrderID"].ToString());
                        }
                        dtAirPricingInfo = (DataTable)ds.Tables["Order"];
                    }
                    else
                    {
                        Order.Add(ds.Tables["Order"].Rows[0]["ID"].ToString());
                        dtAirPricingInfo = (DataTable)ds.Tables["Order"];
                    }

                    string ID1 = ds.Tables["Order"].Rows[0]["Order_ID"].ToString();
                    string PnrForTicket = ds.Tables["Order"].Rows[0]["OrderID"].ToString();

                    if (Order.Count > 0)
                    {
                        #region Booking Info


                        string fsID = flightSegmentIDsForFlightDM.ToString();

                        var Fares = (from f in dtFlights.AsEnumerable().Where(f => f.Field<string?>("PaxSegment_Id") == fsID)
                                     select new
                                     {
                                         AdultTotalPrice = f.Field<string?>("TotalAmount_Text"),
                                         AdultBasePrice = f.Field<string?>("EquivAmount_Text"),
                                         AdultTaxes = f.Field<string?>("Tax"),
                                         ChildTotalPrice = 0,//f.Field<string?>("ChildTotalPrice"),
                                         ChildBasePrice = 0,//f.Field<string?>("ChildBasePrice"),
                                         ChildTaxes = 0,//f.Field<string?>("ChildTaxes"),
                                         InfantTotalPrice = 0,//f.Field<string?>("InfantTotalPrice"),
                                         InfantBasePrice = 0,//f.Field<string?>("InfantBasePrice"),
                                         InfantTaxes = 0,//f.Field<string?>("InfantTaxes"),
                                     }).FirstOrDefault();



                        decimal? Adultsfare = Convert.ToDecimal(Fares.AdultBasePrice);
                        decimal? Childfare = Convert.ToDecimal(Fares.ChildBasePrice);
                        decimal? Infantfare = Convert.ToDecimal(Fares.InfantBasePrice);
                        decimal? AdultTaxes = Convert.ToDecimal(Fares.AdultTaxes);
                        decimal? ChildTaxes = Convert.ToDecimal(Fares.ChildTaxes);
                        decimal? InfnatTaxes = Convert.ToDecimal(Fares.InfantTaxes);
                        decimal? totalfare = 0;
                        decimal? totaltaxes = 0;




                        DateTime dttrans = DateTime.Now.ToUniversalTime();

                        // decimal[] TotalAmt = 1000; //_AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Children, Infants, AirlineCode);

                        totaltaxes = AdultTaxes + ChildTaxes + InfnatTaxes;
                        totalfare = Adultsfare + Childfare + Infantfare; //TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                                                                         //grandtotal = totalfare + totaltaxes + 0;// + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                                                                         //if (Adults > 0) { Adultsfare = TotalAmt[2] / Adults; }
                                                                         //if (Children > 0) { Childfare = TotalAmt[3] / Children; }
                                                                         //if (Infants > 0) { Infatfare = TotalAmt[4] / Infants; }


                        // List<MalikExpressMVC.Models.XML.OriginDestinationOption> odSegments = (List<MalikExpressMVC.Models.XML.OriginDestinationOption>)Session["odSegments"];

                        int BookingTypeID = 0; //odSegments.Count();


                        if (flightType == "RETURN")
                        {
                            BookingTypeID = 1;
                        }

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

                        string ExpiryDateTime = ds.Tables["OrderItem"].Rows[0]["PaymentTimeLimitDateTime"].ToString();

                        string ID = ds.Tables["Order"].Rows[0][1].ToString();
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
                            FlightBookingID = lastBookingId + 1,
                            ExpiryDateTime = ExpiryDateTime,
                            Origin = from,
                            Destination = to,
                            DepartureDate = Convert.ToDateTime(Dep_AircraftScheduledDateTime),
                            ArrivalDate = Convert.ToDateTime(Arr_AircraftScheduledDateTime),
                            Vendor = "NDC",
                            OriginalCost = OriginalCost,
                            CancelPenalty = CancelPenalty,
                            ChangePenalty = ChangePenalty,
                            Penalty = penalty
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
                                TicketNum = ds.Tables["Ticket"].Rows[0]["TicketNumber"].ToString();
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
                            DateTime _age = Convert.ToDateTime(frm?["dobI_" + i.ToString()]?.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(frm?["expiryI" + i.ToString()]);
                            sex = frm?["GenderI_" + i.ToString()];
                            document = frm?["documentI_" + i.ToString()]?.ToString().ToUpper();
                            documenttype = frm?["documenttypeI_" + i.ToString()]?.ToString().ToUpper();

                            if (frm?["expiry_I" + i.ToString()] != null && frm?["expiryI_" + i.ToString()] != "")
                            {
                                expiry = frm?["expiryI_" + i.ToString()];
                            }

                            Age = CalculatePaxAge(_age);

                            nationality = frm?["nationalityI_" + i.ToString()]?.ToString(); // data?.nationality_i?.Value.ToString();

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

                            dob = frm?["dobI_" + i.ToString()]?.ToString();
                            title = frm?["titleI_" + i.ToString()]?.ToString(); // data?.title?.ToUpper();
                            firstName = frm?["firstNameI_" + i.ToString()]?.ToString(); // data?.firstName_i?.Value.ToString().ToUpper();
                            lastName = frm?["lastNameI_" + i.ToString()]?.ToString();  // data?.lastName_i?.Value.ToString().ToUpper();
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
                            DateTime _age = Convert.ToDateTime(frm?["dobC_" + i.ToString()]?.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(frm?["expiryC_" + i.ToString()]);
                            sex = frm?["GenderC_" + i.ToString()];
                            document = frm?["documentC_" + i.ToString()]?.ToString().ToUpper();
                            documenttype = frm?["documenttypeC_" + i.ToString()]?.ToString().ToUpper();

                            if (frm?["expiryC_" + i.ToString()] != null && frm?["expiryC_" + i.ToString()] != "")
                            {
                                expiry = frm?["expiryC_" + i.ToString()];
                            }

                            Age = CalculatePaxAge(_age);

                            nationality = frm?["nationalityC_" + i.ToString()]?.ToString(); // data?.nationality_i?.Value.ToString();

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

                            dob = frm?["dobC_" + i.ToString()]?.ToString();
                            title = frm?["titleC_" + i.ToString()]?.ToString(); // data?.title?.ToUpper();
                            firstName = frm?["firstNameC_" + i.ToString()]?.ToString(); // data?.firstName_i?.Value.ToString().ToUpper();
                            lastName = frm?["lastNameC_" + i.ToString()]?.ToString();  // data?.lastName_i?.Value.ToString().ToUpper();
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

                        if (flightType == "OneWay")
                        {
                            segmenttype = 1;
                        }
                        else
                        {
                            segmenttype = 2;
                        }

                        foreach (DataRow segment in dtFlights.Rows)
                        {
                            string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

                            string AirEquipType = "";
                            if (dtFlights.Columns.Contains("airEquipTypeModel"))
                            {
                                AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
                            }
                            bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
                            DateTime DepDate = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
                            string DepTime = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";
                            DateTime ArrDate = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
                            string ArrTime = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";

                            string MarketingAirlineCode = "PK";// segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
                            string OperatingAirlineCode = "PK"; // segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
                            string FlightNumber = segment["OperatingCarrierFlightNumberText"].ToString(); //FlightNumber.ToString();
                            string CompanyShortName = segment != null && segment["CarrierDesigCode"].ToString() != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
                            string OriginCode = segment["Dep_IATA_LocationCode"].ToString();  //segment["Origin"].ToString();
                            string DestinationCode = segment["Arr_IATA_LocationCode"].ToString(); // segment["Destination"].ToString();
                                                                                                  // string serviceclass = resBookDesigCode.ToString();
                            int cabin = sql.ServiceClasses.Where(x => x.ClassName == segment["CabintypeName"]).Select(x => x.ServiceClassID).FirstOrDefault(); int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                            //int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                            int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
                            int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
                            int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

                            string ArrGMTOffset = segment != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null ? segment["Arr_AircraftScheduledDateTime"].ToString().Substring(segment["Arr_AircraftScheduledDateTime"].ToString().Length - 6) : "";
                            string DptGMTOffset = segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? segment["Dep_AircraftScheduledDateTime"].ToString().Substring(segment["Dep_AircraftScheduledDateTime"].ToString().Length - 6) : "";

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
                                ResBookDesigCode = segment["RBD_Code"].ToString(),
                                Baggage = "", //segment["weight"] + " " + segment["unitOfMeasureCode"],
                                Way = segmenttype,
                                SegmentType = segment["FlightType"].ToString()
                            };
                            sql.FlightSegments.Add(flightseg);
                            sql.SaveChanges();
                            int FlightSegmentID = flightseg.FlightSegmentID;

                            //segmenttype++;
                        }

                        if (ds.Tables.Contains("Order"))
                        {
                            if (ds.Tables["Order"].Rows.Count > 0)
                            {
                                string jsonData = "";


                                //if (frm?.Option_Id_r != null && frm?.Option_Id_r != "")
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


        [HttpPost("b2bhititbooking")]
        public string B2BHitItBooking([FromBody] NDCBooking frm)
        {
            try
            {
                // string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                //var frm = JsonConvert.DeserializeObject<NDCBooking>(json.ToString());

                //string flightsList = frm?.TotalFlights; // JsonConvert.DeserializeObject<dynamic>(frm?.TotalFlights);
                List<NDCFlight> totalFlights = frm.TotalFlights;
                List<NDCPassenger> adultPassengers = frm.AdultPassengers;
                List<NDCPassenger> childPassengers = frm.ChildPassengers;
                List<NDCPassenger> infantPassengers = frm.InfantPassengers;

                int flightSegmentIDsForFlightDM = -1;
                int flightSegmentIDsForFlightRM = -1;
                int flightSegmentIDsForFlightDC = -1;
                int flightSegmentIDsForFlightRC = -1;

                DataTable dtFlights = null;

                if (frm != null)
                {
                    JArray totalFlightsArray = JArray.FromObject(totalFlights);


                    if (totalFlightsArray != null)
                    {
                        dtFlights = ConvertJArrayToDataTable(totalFlightsArray);

                        // Convert the JArray to a List<JToken> or another suitable type
                        List<JToken> totalFlightsList = totalFlightsArray.ToList();

                        //List<string> segmentIDsForFlightDM = new List<string>();

                        foreach (JToken flightToken in totalFlightsList)
                        {
                            string flightCode = flightToken["FlightType"]?.ToString();
                            string segmentID = flightToken["PaxSegment_Id"]?.ToString();

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

                    int UserID = Convert.ToInt32(frm.UserID); // Convert.ToInt32(Session["UserID"]);
                    Random random = new Random();
                    string num = random.Next(9).ToString();


                    //  int UserID = user.intUserID;
                    //data.firstName_0
                    int Adults = Convert.ToInt32(frm.Adults);
                    int Children = Convert.ToInt32(frm.Children);
                    int Infants = Convert.ToInt32(frm.Infants);
                    decimal OriginalCost = Convert.ToDecimal(frm.OriginalCost);

                    if (OriginalCost == 0)
                    {
                        OriginalCost = Convert.ToDecimal(frm.OriginalCost);
                    }

                    decimal grandtotal = Convert.ToDecimal(frm.GrandTotal);

                    string CancelPenalty = ""; // frm.CancelPenalty != null ? Convert.ToString(frm.CancelPenalty) : "0";
                    string ChangePenalty = ""; // frm.ChangePenalty != null ? Convert.ToString(frm.ChangePenalty) : "0";
                    string penalty = frm.Penalty;

                    string from = frm.From;
                    string to = frm.To;
                    string totaltime0 = frm.Totaltime0;
                    string totaltime1 = frm.Totaltime1;
                    string stops0 = frm.Stops0;
                    string stops1 = frm.Stops1;
                    string amount = frm.Amount0;
                    string taxes = frm.Taxes0;
                    string total = frm.Total0;
                    string currency = frm.Currency0;
                    //string serviceclass = ""; //frm.cabin;
                    string optionID = ""; // frm.Option_Id;
                                          //  Guid TransactionId = Guid.Parse(frm.TransactionId);
                    DateTime Arr_AircraftScheduledDateTime = DateTime.MinValue; //frm["Arr_AircraftScheduledDateTime"].ToString();
                    DateTime Dep_AircraftScheduledDateTime = DateTime.MinValue;

                    string arrdateTimeString = frm.Arr_AircraftScheduledDateTime.ToString();

                    if (!string.IsNullOrEmpty(arrdateTimeString))
                    {
                        Arr_AircraftScheduledDateTime = DateTime.Parse(arrdateTimeString);
                    }

                    string depdateTimeString = frm.Dep_AircraftScheduledDateTime.ToString();

                    if (!string.IsNullOrEmpty(depdateTimeString))
                    {
                        Dep_AircraftScheduledDateTime = DateTime.Parse(depdateTimeString);
                    }

                    string Departure = frm.Dep_locationCode;

                    string Arrival = frm.Arr_locationCode;
                    string resBookDesigCode = frm.ResBookDesigCode;
                    string ShoppingResponseRefID = frm.ShoppingResponseRefID;
                    string OfferRefID = frm.OfferID.ToString();
                    string Cabin = frm.Cabin.ToString();

                    //if (Cabin == null || Cabin == "")
                    //{
                    //    Cabin = !string.IsNullOrEmpty(frm.RBD_Code)  frm.RBD_Code.ToString() : "";

                    //}

                    string fareComponentGroupList_Id = frm.FareComponentGroupList_Id;

                    string leadfirstname = !string.IsNullOrEmpty(frm.firstName_0) ? frm.firstName_0.ToString().ToUpper() : "";
                    string leadlastname = !string.IsNullOrEmpty(frm.lastName_0) ? frm.lastName_0.ToString().ToUpper() : "";
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

                    string flightType = !string.IsNullOrEmpty(frm.FlightType) ? frm.FlightType.ToString().ToUpper() : "";

                    string fareReferenceID = !string.IsNullOrEmpty(frm.fareReferenceID) ? frm.fareReferenceID.ToString() : "";

                    string resBookDesigCode1 = "";
                    string fareComponentGroupList_Id1 = "";
                    string optionID_r = "";
                    //Guid TransactionId = Guid.Parse(frm.TransactionId_r);
                    string Arr_AircraftScheduledDateTime_r;
                    string Departure_r = "";
                    string Dep_AircraftScheduledDateTime_r;
                    string Arrival_r = "";
                    string resBookDesigCode_r = "";
                    string Cabin_r = "";
                    string fareComponentGroupList_Id_r = "0"; // frm.FareComponentGroupList_Id_r;

                    if (flightType == "RETURN")
                    {
                        fareComponentGroupList_Id_r = frm.FareComponentGroupList_Id_r;
                        resBookDesigCode1 = frm.ResBookDesigCode_r;
                        fareComponentGroupList_Id1 = frm.FareComponentGroupList_Id_r;
                        optionID_r = frm.Option_Id_r;
                        Arr_AircraftScheduledDateTime_r = DateTime.ParseExact(frm.Arr_AircraftScheduledDateTime_r, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        Departure_r = frm.Dep_locationCode_r;
                        Dep_AircraftScheduledDateTime_r = DateTime.ParseExact(frm.Dep_AircraftScheduledDateTime_r, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                        Arrival_r = frm.Arr_locationCode_r;
                        resBookDesigCode_r = frm.ResBookDesigCode_r;
                        Cabin_r = frm.Cabin_r;
                    }

                    // Define possible date formats
                    string[] formats = { "dd/MM/yyyy", "MM/dd/yyyy" };

                    string _ticket = ""; // frm?.chkTicket;

                    DateTime datet = DateTime.Now.Date;
                    string Dat = datet.ToString("yyyy-MM-dd");

                    DateTime dt1 = DateTime.UtcNow;
                    string tstamp = dt1.ToString("s") + "Z";
                    //string AirlineCode = "PK";
                    //bool error = false;
                    string email = !string.IsNullOrEmpty(frm.email_0) ? frm?.email_0?.ToString() : "";
                    string contact = !string.IsNullOrEmpty(frm.MobileNO_0) ? frm?.MobileNO_0?.ToString() : "";



                    #region XML Request

                    string xmlRequest = "";
                    StringBuilder strReq = new StringBuilder();
                    title = "Mr";
                    string PaxRefID = "";

                    if (Adults > 0)
                    {
                        for (int i = 0; i < adultPassengers.Count; i++)
                        {
                            DateTime _age = Convert.ToDateTime(adultPassengers[i].DOB.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(adultPassengers[i].Expiry.ToString());
                            sex = adultPassengers[i].Gender.ToString();
                            document = adultPassengers[i].Document.ToString().ToUpper();
                            documenttype = adultPassengers[i].DocumentType.ToString().ToUpper();

                            if (adultPassengers[i].Expiry.ToString() != null && adultPassengers[i].Expiry.ToString() != "")
                            {
                                string eoriginalDateString = adultPassengers[i].Expiry.ToString();
                                //string eformattedDateString;

                                //DateTime edate;

                                //// Try parsing the date string using the defined formats
                                //bool success = DateTime.TryParseExact(
                                //    eoriginalDateString,
                                //    formats,
                                //    CultureInfo.InvariantCulture,
                                //    DateTimeStyles.None,
                                //    out edate);

                                //eformattedDateString = edate.ToString("yyyy-MM-dd");                               

                                expiry = eoriginalDateString;
                            }


                            nationality = adultPassengers[i].Nationality.ToString(); // data?.nationality_i?.ToString();

                            string originalDateString = adultPassengers[i].DOB.ToString();
                            //string formattedDateString;

                            //DateTime date = DateTime.ParseExact(originalDateString, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            //formattedDateString = date.ToString("yyyy-MM-dd");

                            dob = originalDateString;


                            title = adultPassengers[i].Title.ToString(); // data?.title?.ToUpper();
                            firstName = adultPassengers[i].FirstName.ToString().ToUpper(); // data?.firstName_i?.ToString().ToUpper();
                            lastName = adultPassengers[i].LastName.ToString().ToUpper();  // data?.lastName_i?.ToString().ToUpper();                            
                            title = adultPassengers[i].Title.ToString().ToUpper();
                            mobile = contact;

                            title = "Mr";
                            if (sex == "F")
                            {
                                sex = "F";
                                title = "Ms";
                            }
                            else
                            {
                                sex = "M";
                            }

                            if (i == 0)
                            {
                                strReq.Append(@"<ContactInfoList>");
                                strReq.Append(@"<ContactInfo>");
                                strReq.Append(@"<ContactInfoID>Contact-1</ContactInfoID>");
                                strReq.Append(@"<EmailAddress>");
                                strReq.Append(@"<EmailAddressText>" + email + "</EmailAddressText>");
                                strReq.Append(@"</EmailAddress>");
                                strReq.Append(@"<Phone>");
                                strReq.Append(@"<AreaCodeNumber>" + mobile.Substring(0, 3) + "</AreaCodeNumber>");
                                strReq.Append(@"<CountryDialingCode>92</CountryDialingCode>");
                                strReq.Append(@"<PhoneNumber>" + mobile.Substring(3, 7) + "</PhoneNumber>");
                                strReq.Append(@"</Phone>");
                                strReq.Append(@"</ContactInfo>");
                                strReq.Append(@"</ContactInfoList>");
                                strReq.Append(@"<PaxList>");
                            }


                            strReq.Append(@"<Pax>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ContactInfoRefID>Contact-1</ContactInfoRefID>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID > ");
                            strReq.Append(@"<IdentityDocTypeCode>NATIONAL_ID</IdentityDocTypeCode >");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID>");
                            strReq.Append(@"<IdentityDocTypeCode>PASSPORT</IdentityDocTypeCode>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ExpiryDate>" + expiry + "</ExpiryDate>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<Individual>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<IndividualID>IND-ADT" + i + "</IndividualID>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"<TitleName>" + title + "</TitleName>");
                            strReq.Append(@"</Individual>");
                            strReq.Append(@"<PaxID>PAX-ADT" + i + "</PaxID>");
                            strReq.Append(@"<PTC>ADT</PTC>");
                            strReq.Append(@"</Pax>");

                            PaxRefID = PaxRefID + "PAX-ADT" + i + " ";
                        }
                    }

                    if (Children > 0)
                    {
                        for (int c = 0; c < childPassengers.Count; c++)
                        {
                            DateTime _age = Convert.ToDateTime(childPassengers[c].DOB.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(childPassengers[c].Expiry.ToString());
                            sex = childPassengers[c].Gender.ToString();
                            document = childPassengers[c].Document.ToString().ToUpper();
                            documenttype = childPassengers[c].DocumentType.ToString().ToUpper();

                            if (childPassengers[c].Expiry.ToString() != null && childPassengers[c].Expiry.ToString() != "")
                            {
                                string eoriginalDateString = childPassengers[c].Expiry.ToString();
                                //string eformattedDateString;

                                //DateTime edate = DateTime.ParseExact(eoriginalDateString, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                //eformattedDateString = edate.ToString("yyyy-MM-dd");

                                expiry = eoriginalDateString; ;
                            }


                            nationality = childPassengers[c].Nationality.ToString(); // data?.nationality_i?.ToString();

                            string originalDateString = childPassengers[c].DOB.ToString();
                            //string formattedDateString;

                            //DateTime date = DateTime.ParseExact(originalDateString, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            //formattedDateString = date.ToString("yyyy-MM-dd");

                            dob = originalDateString;

                            title = childPassengers[c].Title.ToString(); // data?.title?.ToUpper();
                            firstName = childPassengers[c].FirstName.ToString().ToUpper(); // data?.firstName_i?.ToString().ToUpper();
                            lastName = childPassengers[c].LastName.ToString().ToUpper();  // data?.lastName_i?.ToString().ToUpper();                            
                            title = childPassengers[c].Title.ToString().ToUpper();
                            title = "Mr";
                            if (sex == "F")
                            {
                                sex = "F";
                                title = "Ms";
                            }
                            else
                            {
                                sex = "M";
                            }
                            strReq.Append(@"<Pax>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ContactInfoRefID>Contact-1</ContactInfoRefID>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID > ");
                            strReq.Append(@"<IdentityDocTypeCode>NATIONAL_ID</IdentityDocTypeCode >");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID>");
                            strReq.Append(@"<IdentityDocTypeCode>PASSPORT</IdentityDocTypeCode>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ExpiryDate>" + expiry + "</ExpiryDate>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<Individual>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<IndividualID>IND-CHD" + c + "</IndividualID>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"<TitleName>" + title + "</TitleName>");
                            strReq.Append(@"</Individual>");
                            strReq.Append(@"<PaxID>PAX-CHD" + c + "</PaxID>");
                            strReq.Append(@"<PTC>CHD</PTC>");
                            strReq.Append(@"</Pax>");

                            PaxRefID = PaxRefID + "PAX-CHD" + c + " ";
                        }
                    }


                    if (Infants > 0)
                    {
                        for (int i = 0; i < infantPassengers.Count; i++)
                        {

                            DateTime _age = Convert.ToDateTime(infantPassengers[i].DOB.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(infantPassengers[i].Expiry.ToString());
                            sex = infantPassengers[i].Gender.ToString();
                            document = infantPassengers[i].Document.ToString().ToUpper();
                            documenttype = infantPassengers[i].DocumentType.ToString().ToUpper();

                            if (infantPassengers[i].Expiry.ToString() != null && infantPassengers[i].Expiry.ToString() != "")
                            {
                                string eoriginalDateString = infantPassengers[i].Expiry.ToString();
                                //string eformattedDateString;

                                //DateTime edate = DateTime.ParseExact(eoriginalDateString, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                                //eformattedDateString = edate.ToString("yyyy-MM-dd");

                                expiry = eoriginalDateString; ;
                            }


                            nationality = infantPassengers[i].Nationality.ToString(); // data?.nationality_i?.ToString();

                            string originalDateString = infantPassengers[i].DOB.ToString();
                            //string formattedDateString;

                            //DateTime date = DateTime.ParseExact(originalDateString, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            //formattedDateString = date.ToString("yyyy-MM-dd");

                            dob = originalDateString;

                            title = infantPassengers[i].Title.ToString(); // data?.title?.ToUpper();
                            firstName = infantPassengers[i].FirstName.ToString().ToUpper(); // data?.firstName_i?.ToString().ToUpper();
                            lastName = infantPassengers[i].LastName.ToString().ToUpper();  // data?.lastName_i?.ToString().ToUpper();                            
                            title = infantPassengers[i].Title.ToString().ToUpper();
                            title = "Mr";


                            title = "Mr";
                            if (sex == "F")
                            {
                                sex = "F";
                                title = "Ms";
                            }
                            else
                            {
                                sex = "M";
                            }


                            strReq.Append(@"<Pax>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ContactInfoRefID>Contact-1</ContactInfoRefID>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID > ");
                            strReq.Append(@"<IdentityDocTypeCode>NATIONAL_ID</IdentityDocTypeCode >");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID>");
                            strReq.Append(@"<IdentityDocTypeCode>PASSPORT</IdentityDocTypeCode>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ExpiryDate>" + expiry + "</ExpiryDate>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<Individual>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<IndividualID>IND-INF" + i + "</IndividualID>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"<TitleName>" + title + "</TitleName>");
                            strReq.Append(@"</Individual>");
                            strReq.Append(@"<PaxID>PAX-INF" + i + "</PaxID>");
                            strReq.Append(@"<PaxRefID>PAX-ADT0</PaxRefID>");
                            strReq.Append(@"<PTC>INF</PTC>");
                            strReq.Append(@"</Pax>");

                            PaxRefID = PaxRefID + "PAX-INF" + i + " ";
                        }
                    }

                    xmlRequest = OneWayBookingXML(strReq.ToString(), dtFlights, null, resBookDesigCode, ShoppingResponseRefID, null, OfferRefID, PaxRefID.Trim());

                    if (flightType == "ONEWAY")
                    {


                    }
                    else if (flightType == "RETURN")
                    {


                        //xmlRequest = ReturnBookingXML(strReq.ToString(), item, fareDetails, Returnitem, ReturnfareDetails, DepartureCity, ArrivalCity, resBookDesigCode, deiCode, explanation, note, resBookDesigCode1, deiCode1, explanation1, note1);
                    }

                    #endregion

                    ////Call Booking endpoint
                    string result1 = HitItDepartureBooking(xmlRequest).Result;

                    if (result1 == "something went wrong")
                    {
                        return "something went wrong";
                    }

                    //XmlDocument doc = new XmlDocument();
                    ////doc.Load("C:\\Users\\Hp\\Downloads\\ret-reposne.xml");
                    //doc.Load("C:\\hallmark\\ret-reposne.xml");
                    //     string result1 = doc.OuterXml;

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(result1);
                    XmlReader xmlReader = new XmlNodeReader(xml);

                    DataSet ds = new DataSet();
                    ds.ReadXml(xmlReader);

                    if (ds.Tables.Count < 1)
                    {
                        return "something went wrong";
                    }

                    DataTable dtAirPricingInfo;

                    //  string AirPricingInfoKey = "1";

                    // ds2.ReadXml(AirPricingSolution);
                    List<string> Order = new List<string>();

                    if (ds.Tables.Contains("Order"))
                    {
                        foreach (DataRow ro in ds.Tables["Order"].Rows)
                        {
                            Order.Add(ro["OrderID"].ToString());
                        }
                        dtAirPricingInfo = (DataTable)ds.Tables["Order"];
                    }
                    else
                    {
                        Order.Add(ds.Tables["Order"].Rows[0]["ID"].ToString());
                        dtAirPricingInfo = (DataTable)ds.Tables["Order"];
                    }

                    string ID1 = ds.Tables["Order"].Rows[0]["Order_ID"].ToString();
                    string PnrForTicket = ds.Tables["Order"].Rows[0]["OrderID"].ToString();

                    if (Order.Count > 0)
                    {
                        #region Booking Info


                        string fsID = flightSegmentIDsForFlightDM.ToString();

                        var Fares = (from f in dtFlights.AsEnumerable().Where(f => f.Field<string?>("PaxSegment_Id") == fsID)
                                     select new
                                     {
                                         AdultTotalPrice = f.Field<string?>("TotalAmount_Text"),
                                         AdultBasePrice = f.Field<string?>("EquivAmount_Text"),
                                         AdultTaxes = f.Field<string?>("Tax"),
                                         ChildTotalPrice = 0,//f.Field<string?>("ChildTotalPrice"),
                                         ChildBasePrice = 0,//f.Field<string?>("ChildBasePrice"),
                                         ChildTaxes = 0,//f.Field<string?>("ChildTaxes"),
                                         InfantTotalPrice = 0,//f.Field<string?>("InfantTotalPrice"),
                                         InfantBasePrice = 0,//f.Field<string?>("InfantBasePrice"),
                                         InfantTaxes = 0,//f.Field<string?>("InfantTaxes"),
                                     }).FirstOrDefault();



                        decimal? Adultsfare = Convert.ToDecimal(Fares.AdultBasePrice);
                        decimal? Childfare = Convert.ToDecimal(Fares.ChildBasePrice);
                        decimal? Infantfare = Convert.ToDecimal(Fares.InfantBasePrice);
                        decimal? AdultTaxes = Convert.ToDecimal(Fares.AdultTaxes);
                        decimal? ChildTaxes = Convert.ToDecimal(Fares.ChildTaxes);
                        decimal? InfnatTaxes = Convert.ToDecimal(Fares.InfantTaxes);
                        decimal? totalfare = 0;
                        decimal? totaltaxes = 0;




                        DateTime dttrans = DateTime.Now.ToUniversalTime();

                        // decimal[] TotalAmt = 1000; //_AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Children, Infants, AirlineCode);

                        totaltaxes = AdultTaxes + ChildTaxes + InfnatTaxes;
                        totalfare = Adultsfare + Childfare + Infantfare; //TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                                                                         //grandtotal = totalfare + totaltaxes + 0;// + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                                                                         //if (Adults > 0) { Adultsfare = TotalAmt[2] / Adults; }
                                                                         //if (Children > 0) { Childfare = TotalAmt[3] / Children; }
                                                                         //if (Infants > 0) { Infatfare = TotalAmt[4] / Infants; }


                        // List<MalikExpressMVC.Models.XML.OriginDestinationOption> odSegments = (List<MalikExpressMVC.Models.XML.OriginDestinationOption>)Session["odSegments"];

                        int BookingTypeID = 0; //odSegments.Count();


                        if (flightType == "RETURN")
                        {
                            BookingTypeID = 1;
                        }

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

                        string ExpiryDateTime = ds.Tables["OrderItem"].Rows[0]["PaymentTimeLimitDateTime"].ToString();

                        string ID = ds.Tables["Order"].Rows[0][1].ToString();
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
                            FlightBookingID = lastBookingId + 1,
                            ExpiryDateTime = ExpiryDateTime,
                            Origin = from,
                            Destination = to,
                            DepartureDate = Convert.ToDateTime(Dep_AircraftScheduledDateTime),
                            ArrivalDate = Convert.ToDateTime(Arr_AircraftScheduledDateTime),
                            Vendor = "NDC",
                            OriginalCost = OriginalCost,
                            CancelPenalty = CancelPenalty,
                            ChangePenalty = ChangePenalty,
                            Penalty = penalty
                        };

                        sql.Reservations.Add(flight);
                        sql.SaveChanges();

                        int BookingID = flight.Id;

                        string TicketNum = "";
                        #endregion

                        #region Pax Info

                        int Age = 0;
                        string paxType = "";
                        for (int i = 1; i < adultPassengers.Count; i++)
                        {
                            DateTime _age = Convert.ToDateTime(adultPassengers[i].DOB.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(adultPassengers[i].Expiry.ToString());
                            sex = adultPassengers[i].Gender.ToString();
                            document = adultPassengers[i].Document.ToString().ToUpper();
                            documenttype = adultPassengers[i].DocumentType.ToString().ToUpper();

                            if (adultPassengers[i].Expiry.ToString() != null && adultPassengers[i].Expiry.ToString() != "")
                            {
                                expiry = adultPassengers[i].Expiry.ToString();
                            }


                            nationality = adultPassengers[i].Nationality.ToString(); // data?.nationality_i?.ToString();

                            dob = adultPassengers[i].DOB.ToString();
                            title = adultPassengers[i].Title.ToString(); // data?.title?.ToUpper();
                            firstName = adultPassengers[i].FirstName.ToString().ToUpper(); // data?.firstName_i?.ToString().ToUpper();
                            lastName = adultPassengers[i].LastName.ToString().ToUpper();  // data?.lastName_i?.ToString().ToUpper();                            
                            title = adultPassengers[i].Title.ToString().ToUpper();
                            mobile = contact;


                            bool sex1 = false;
                            if (ds.Tables.Contains("Ticket"))
                            {
                                // TicketNum = ds.Tables["Ticket"].Rows[0]["TicketNumber"].ToString();
                                TicketNum = "FakeTicket";
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

                        if (Infants > 0)
                        {
                            for (int i = 0; i < infantPassengers.Count; i++)
                            {
                                bool sex1 = false;
                                DateTime _age = Convert.ToDateTime(infantPassengers[i].DOB.ToString());
                                DateTime dtExpiry = Convert.ToDateTime(infantPassengers[i].Expiry.ToString());
                                sex = infantPassengers[i].Gender.ToString();
                                document = infantPassengers[i].Document.ToString().ToUpper();
                                documenttype = infantPassengers[i].DocumentType.ToString().ToUpper();

                                if (infantPassengers[i].Expiry.ToString() != null && infantPassengers[i].Expiry.ToString() != "")
                                {
                                    expiry = infantPassengers[i].Expiry.ToString();
                                }

                                nationality = infantPassengers[i].Nationality.ToString(); // data?.nationality_i?.ToString();

                                dob = infantPassengers[i].DOB.ToString();
                                title = infantPassengers[i].Title.ToString(); // data?.title?.ToUpper();
                                firstName = infantPassengers[i].FirstName.ToString().ToUpper(); // data?.firstName_i?.ToString().ToUpper();
                                lastName = infantPassengers[i].LastName.ToString().ToUpper();  // data?.lastName_i?.ToString().ToUpper();                            
                                title = infantPassengers[i].Title.ToString().ToUpper();
                                mobile = contact;

                                Age = CalculatePaxAge(_age);

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


                                if (ds.Tables.Contains("Ticket"))
                                {
                                    TicketNum = "FakeTicket";
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
                        }

                        if (Children > 0)
                        {
                            for (int i = 0; i < childPassengers.Count; i++)
                            {
                                bool sex1 = false;
                                DateTime _age = Convert.ToDateTime(childPassengers[i].DOB.ToString());
                                DateTime dtExpiry = Convert.ToDateTime(childPassengers[i].Expiry.ToString());
                                sex = childPassengers[i].Gender.ToString();
                                document = childPassengers[i].Document.ToString().ToUpper();
                                documenttype = childPassengers[i].DocumentType.ToString().ToUpper();

                                if (childPassengers[i].Expiry.ToString() != null && childPassengers[i].Expiry.ToString() != "")
                                {
                                    expiry = childPassengers[i].Expiry.ToString();
                                }

                                nationality = childPassengers[i].Nationality.ToString(); // data?.nationality_i?.ToString();

                                dob = childPassengers[i].DOB.ToString();
                                title = childPassengers[i].Title.ToString(); // data?.title?.ToUpper();
                                firstName = childPassengers[i].FirstName.ToString().ToUpper(); // data?.firstName_i?.ToString().ToUpper();
                                lastName = childPassengers[i].LastName.ToString().ToUpper();  // data?.lastName_i?.ToString().ToUpper();                            
                                title = childPassengers[i].Title.ToString().ToUpper();
                                mobile = contact;

                                Age = CalculatePaxAge(_age);
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



                                if (ds.Tables.Contains("Ticket"))
                                {
                                    //TicketNum = ds.Tables["Ticket"].Rows[i - 1]["TicketNumber"].ToString();
                                    TicketNum = "FakeTicket";
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

                        if (flightType == "OneWay")
                        {
                            segmenttype = 1;
                        }
                        else
                        {
                            segmenttype = 2;
                        }

                        foreach (DataRow segment in dtFlights.Rows)
                        {
                            //if (Convert.ToInt32(segment["PaxSegment_Id"]) == flightSegmentIDsForFlightDM)
                            {
                                string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

                                string AirEquipType = "";
                                if (dtFlights.Columns.Contains("airEquipTypeModel"))
                                {
                                    AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
                                }
                                bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
                                DateTime DepDate = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
                                string DepTime = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";
                                DateTime ArrDate = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
                                string ArrTime = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";

                                string MarketingAirlineCode = "PK";// segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
                                string OperatingAirlineCode = "PK"; // segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
                                string FlightNumber = segment["OperatingCarrierFlightNumberText"].ToString(); //FlightNumber.ToString();
                                string CompanyShortName = segment != null && segment["CarrierDesigCode"].ToString() != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
                                string OriginCode = segment["Dep_IATA_LocationCode"].ToString();  //segment["Origin"].ToString();
                                string DestinationCode = segment["Arr_IATA_LocationCode"].ToString(); // segment["Destination"].ToString();
                                                                                                      // string serviceclass = resBookDesigCode.ToString();
                                int cabin = sql.ServiceClasses.Where(x => x.ClassName == segment["CabintypeName"]).Select(x => x.ServiceClassID).FirstOrDefault(); int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                                //int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                                int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
                                int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
                                int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

                                string ArrGMTOffset = segment != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null ? segment["Arr_AircraftScheduledDateTime"].ToString().Substring(segment["Arr_AircraftScheduledDateTime"].ToString().Length - 6) : "";
                                string DptGMTOffset = segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? segment["Dep_AircraftScheduledDateTime"].ToString().Substring(segment["Dep_AircraftScheduledDateTime"].ToString().Length - 6) : "";

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
                                    ResBookDesigCode = segment["RBD_Code"].ToString(),
                                    Baggage = "", //segment["weight"] + " " + segment["unitOfMeasureCode"],
                                    Way = segmenttype,
                                    SegmentType = "DM"
                                };
                                sql.FlightSegments.Add(flightseg);
                                sql.SaveChanges();
                                int FlightSegmentID = flightseg.FlightSegmentID;
                                dmFlag = true;

                                //  itrCodes++;
                            }


                        }

                        if (ds.Tables.Contains("Order"))
                        {
                            if (ds.Tables["Order"].Rows.Count > 0)
                            {
                                string jsonData = "";


                                //if (frm?.Option_Id_r != null && frm?.Option_Id_r != "")
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
                //string innerErr = "";

                //if (ex.InnerException != null)
                //{
                //    innerErr = ex.InnerException.Message.ToString();
                //}
                //return ex.Message.ToString() + " " + innerErr;
                return "something went wrong";
            }
        }

        //Multi Destination XML
        [NonAction]
        private string HitItMultiXMLReq(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr, string fromM_1, string fromM_2,
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

                int startIndex = 0;
                int endIndex = 0;

                string[] strDates = start.Split(",");

                strReq.Append(@"<soapenv:Envelope xmlns:soapenv = 'http://schemas.xmlsoap.org/soap/envelope/' xmlns = 'http://www.iata.org/IATA/2015/00/2020.1/IATA_AirShoppingRQ'>");
                strReq.Append(@"<soapenv:Header/>");
                strReq.Append(@"<soapenv:Body>");
                strReq.Append(@"<IATA_AirShoppingRQ>");
                strReq.Append(@"<MessageDoc>");
                strReq.Append(@"<Name>NDC GATEWAY</Name>");
                strReq.Append(@"<RefVersionNumber>20.1</RefVersionNumber>");
                strReq.Append(@"</MessageDoc>");
                strReq.Append(@"<Party>");
                strReq.Append(@"<Sender>");
                strReq.Append(@"<TravelAgency>");
                strReq.Append(@"<AgencyID>PSA2746366</AgencyID>");
                strReq.Append(@"</TravelAgency>");
                strReq.Append(@"</Sender>");
                strReq.Append(@"</Party>");
                strReq.Append(@"<Request>");
                strReq.Append(@"<FlightRequest>");

                if ((fromM_1 != null && fromM_1 != "") && (toM_1 != null && toM_1 != "") && (strDates.Length > 0 && strDates[0] != null))
                {
                    startIndex = fromM_1.IndexOf('(');
                    endIndex = fromM_1.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        fromM_1 = fromM_1.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    startIndex = toM_1.IndexOf('(');
                    endIndex = toM_1.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        toM_1 = toM_1.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    strReq.Append(@"<OriginDestCriteria>");
                    strReq.Append(@"<DestArrivalCriteria>");
                    strReq.Append(@"<IATA_LocationCode>" + toM_1 + "</IATA_LocationCode>");
                    strReq.Append(@"</DestArrivalCriteria>");
                    strReq.Append(@"<OriginDepCriteria>");
                    strReq.Append(@"<Date>" + strDates[0] + "</Date>");
                    strReq.Append(@"<IATA_LocationCode>" + fromM_1 + "</IATA_LocationCode>");
                    strReq.Append(@"</OriginDepCriteria>");
                    strReq.Append(@"<PreferredCabinType>");
                    strReq.Append(@"<CabinTypeCode>Y</CabinTypeCode>");
                    strReq.Append(@"</PreferredCabinType>");
                    strReq.Append(@"</OriginDestCriteria>");

                }

                if ((fromM_2 != null && fromM_2 != "") && (toM_2 != null && toM_2 != "") && (strDates.Length > 1 && strDates[1] != null))
                {
                    startIndex = fromM_2.IndexOf('(');
                    endIndex = fromM_2.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        fromM_2 = fromM_2.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    startIndex = toM_2.IndexOf('(');
                    endIndex = toM_2.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        toM_2 = toM_2.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    strReq.Append(@"<OriginDestCriteria>");
                    strReq.Append(@"<DestArrivalCriteria>");
                    strReq.Append(@"<IATA_LocationCode>" + toM_2 + "</IATA_LocationCode>");
                    strReq.Append(@"</DestArrivalCriteria>");
                    strReq.Append(@"<OriginDepCriteria>");
                    strReq.Append(@"<Date>" + strDates[1] + "</Date>");
                    strReq.Append(@"<IATA_LocationCode>" + fromM_2 + "</IATA_LocationCode>");
                    strReq.Append(@"</OriginDepCriteria>");
                    strReq.Append(@"<PreferredCabinType>");
                    strReq.Append(@"<CabinTypeCode>Y</CabinTypeCode>");
                    strReq.Append(@"</PreferredCabinType>");
                    strReq.Append(@"</OriginDestCriteria>");

                }

                if ((fromM_3 != null && fromM_3 != "") && (toM_3 != null && toM_3 != "") && (strDates.Length > 2 && strDates[2] != null))
                {
                    startIndex = fromM_3.IndexOf('(');
                    endIndex = fromM_3.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        fromM_3 = fromM_3.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    startIndex = toM_3.IndexOf('(');
                    endIndex = toM_3.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        toM_3 = toM_3.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    strReq.Append(@"<OriginDestCriteria>");
                    strReq.Append(@"<DestArrivalCriteria>");
                    strReq.Append(@"<IATA_LocationCode>" + toM_3 + "</IATA_LocationCode>");
                    strReq.Append(@"</DestArrivalCriteria>");
                    strReq.Append(@"<OriginDepCriteria>");
                    strReq.Append(@"<Date>" + strDates[2] + "</Date>");
                    strReq.Append(@"<IATA_LocationCode>" + fromM_3 + "</IATA_LocationCode>");
                    strReq.Append(@"</OriginDepCriteria>");
                    strReq.Append(@"<PreferredCabinType>");
                    strReq.Append(@"<CabinTypeCode>Y</CabinTypeCode>");
                    strReq.Append(@"</PreferredCabinType>");
                    strReq.Append(@"</OriginDestCriteria>");

                }

                if ((fromM_4 != null && fromM_4 != "") && (toM_4 != null && toM_4 != "") && (strDates.Length > 3 && strDates[3] != null))
                {
                    startIndex = fromM_4.IndexOf('(');
                    endIndex = fromM_4.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        fromM_4 = fromM_4.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    startIndex = toM_4.IndexOf('(');
                    endIndex = toM_4.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        toM_4 = toM_4.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    strReq.Append(@"<OriginDestCriteria>");
                    strReq.Append(@"<DestArrivalCriteria>");
                    strReq.Append(@"<IATA_LocationCode>" + toM_4 + "</IATA_LocationCode>");
                    strReq.Append(@"</DestArrivalCriteria>");
                    strReq.Append(@"<OriginDepCriteria>");
                    strReq.Append(@"<Date>" + strDates[3] + "</Date>");
                    strReq.Append(@"<IATA_LocationCode>" + fromM_4 + "</IATA_LocationCode>");
                    strReq.Append(@"</OriginDepCriteria>");
                    strReq.Append(@"<PreferredCabinType>");
                    strReq.Append(@"<CabinTypeCode>Y</CabinTypeCode>");
                    strReq.Append(@"</PreferredCabinType>");
                    strReq.Append(@"</OriginDestCriteria>");

                }

                if ((fromM_5 != null && fromM_5 != "") && (toM_5 != null && toM_5 != "") && (strDates.Length > 4 && strDates[4] != null))
                {
                    startIndex = fromM_5.IndexOf('(');
                    endIndex = fromM_5.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        fromM_5 = fromM_5.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    startIndex = toM_5.IndexOf('(');
                    endIndex = toM_5.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        toM_5 = toM_5.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    strReq.Append(@"<OriginDestCriteria>");
                    strReq.Append(@"<DestArrivalCriteria>");
                    strReq.Append(@"<IATA_LocationCode>" + toM_5 + "</IATA_LocationCode>");
                    strReq.Append(@"</DestArrivalCriteria>");
                    strReq.Append(@"<OriginDepCriteria>");
                    strReq.Append(@"<Date>" + strDates[4] + "</Date>");
                    strReq.Append(@"<IATA_LocationCode>" + fromM_5 + "</IATA_LocationCode>");
                    strReq.Append(@"</OriginDepCriteria>");
                    strReq.Append(@"<PreferredCabinType>");
                    strReq.Append(@"<CabinTypeCode>Y</CabinTypeCode>");
                    strReq.Append(@"</PreferredCabinType>");
                    strReq.Append(@"</OriginDestCriteria>");

                }

                if ((fromM_6 != null && fromM_6 != "") && (toM_6 != null && toM_6 != "") && (strDates.Length > 5 && strDates[5] != null))
                {
                    startIndex = fromM_6.IndexOf('(');
                    endIndex = fromM_6.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        fromM_6 = fromM_6.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    startIndex = toM_6.IndexOf('(');
                    endIndex = toM_6.IndexOf(')');

                    // Extract the text inside the parentheses
                    if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                    {
                        toM_6 = toM_6.Substring(startIndex + 1, endIndex - startIndex - 1);
                    }

                    strReq.Append(@"<OriginDestCriteria>");
                    strReq.Append(@"<DestArrivalCriteria>");
                    strReq.Append(@"<IATA_LocationCode>" + toM_6 + "</IATA_LocationCode>");
                    strReq.Append(@"</DestArrivalCriteria>");
                    strReq.Append(@"<OriginDepCriteria>");
                    strReq.Append(@"<Date>" + strDates[5] + "</Date>");
                    strReq.Append(@"<IATA_LocationCode>" + fromM_6 + "</IATA_LocationCode>");
                    strReq.Append(@"</OriginDepCriteria>");
                    strReq.Append(@"<PreferredCabinType>");
                    strReq.Append(@"<CabinTypeCode>Y</CabinTypeCode>");
                    strReq.Append(@"</PreferredCabinType>");
                    strReq.Append(@"</OriginDestCriteria>");

                }


                strReq.Append(@"</FlightRequest>");
                strReq.Append(@"<Paxs>");

                int id = 1;
                for (int a = 1; a <= adults; a++)
                {
                    strReq.Append(@"<Pax>");
                    strReq.Append(@"<PaxID>SH" + id++ + "</PaxID>");
                    strReq.Append(@"<PTC>ADT</PTC>");
                    strReq.Append(@"</Pax>");
                }
                for (int c = 1; c <= children; c++)
                {
                    strReq.Append(@"<Pax>");
                    strReq.Append(@"<PaxID>SH" + id++ + "</PaxID>");
                    strReq.Append(@"<PTC>CHD</PTC>");
                    strReq.Append(@"</Pax>");
                }
                for (int i = 1; i <= infants; i++)
                {
                    strReq.Append(@"<Pax>");
                    strReq.Append(@"<PaxID>SH" + id++ + "</PaxID>");
                    strReq.Append(@"<PTC>INF</PTC>");
                    strReq.Append(@"</Pax>");
                }


                strReq.Append(@"</Paxs>");
                strReq.Append(@"<ResponseParameters>");
                strReq.Append(@"<CurParameter>");
                strReq.Append(@"<RequestedCurCode>PKR</RequestedCurCode>");
                strReq.Append(@"</CurParameter>");
                strReq.Append(@"<LangUsage>");
                strReq.Append(@"<LangCode>EN</LangCode>");
                strReq.Append(@"</LangUsage>");
                strReq.Append(@"</ResponseParameters>");
                strReq.Append(@"<ShoppingCriteria>");
                strReq.Append(@"<ConnectionCriteria>");
                strReq.Append(@"<InterlineInd>true</InterlineInd>");
                strReq.Append(@"</ConnectionCriteria>");
                strReq.Append(@"</ShoppingCriteria>");
                strReq.Append(@"</Request>");
                strReq.Append(@"</IATA_AirShoppingRQ>");
                strReq.Append(@"</soapenv:Body >");
                strReq.Append(@"</soapenv:Envelope>");


                return strReq.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        // string OneWayBookingXML(string strReq1, DataTable Flights, DataTable dtFareList, string resBookDesigCode, ShoppingResponseRefID,  DataTable cityInfo, string OfferRefID)

        //Multi destination
        [HttpGet("hititmultiflights")]
        public async Task<string> HitItMultiFlights(string tripType, string from, string to, int adults, int children, int infants, string Dept, string Arr,
        string fromM_1, string fromM_2, string fromM_3, string fromM_4, string fromM_5, string fromM_6,
        string toM_1, string toM_2, string toM_3, string toM_4, string toM_5, string toM_6, string start, string airline = "", string userid = "-1")
        {
            try
            {
                // MexCoreWebAPI.Model.NDC.Envelope ndcFlights = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                // string url = "https://app.crane.aero/cranendc/v20.1/CraneNDCService";
                string url = "https://app-stage.crane.aero/cranendc/v20.1/CraneNDCService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "");
                request.Headers.Add("Host", "app-stage.crane.aero");
                request.Headers.Add("KeepAlive", "true");
                request.Headers.Add("username", "PSA2746366");
                request.Headers.Add("password", "Pia123"); //Test password
                                                           // request.Headers.Add("password", "Sam7890@#_"); // production password

                var xmlReq = HitItMultiXMLReq("Multi", from, to, adults, children, infants, Dept, Arr, fromM_1, fromM_2, fromM_3, fromM_4, fromM_5, fromM_6, toM_1, toM_2, toM_3, toM_4, toM_5, toM_6, start, airline);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

                DataSet ds = new DataSet();



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

                    //Log

                    //string fileName = @"C:\hallmark\doAirShopping_2A1C1I_req.txt";  //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Res.txt";
                    //FileInfo fi = new FileInfo(fileName);

                    ////////Check if file already exists. If yes, delete it.
                    //if (fi.Exists)
                    //{
                    //    fi.Delete();
                    //}

                    //////Create a new file
                    //using (FileStream fs = fi.Create())
                    //{
                    //    Byte[] txt = new UTF8Encoding(true).GetBytes(xmlReq);
                    //    fs.Write(txt, 0, txt.Length);
                    //}


                    //fileName = @"C:\hallmark\doAirShopping_2A1C1I_res.txt";  //d:\HITITCS\HallMark\Test_Cases\GetAvailability_OW_Res.txt";
                    //FileInfo fi1 = new FileInfo(fileName);

                    ////////Check if file already exists. If yes, delete it.
                    //if (fi1.Exists)
                    //{
                    //    fi1.Delete();
                    //}

                    //////Create a new file
                    //using (FileStream fs = fi1.Create())
                    //{
                    //    Byte[] txt = new UTF8Encoding(true).GetBytes(customerJsonString);
                    //    fs.Write(txt, 0, txt.Length);
                    //}
                    if (!ds.Tables.Contains("Price"))
                    {
                        return "something went wrong";
                    }

                    DataTable dtPrice = ds.Tables["Price"].Copy();
                    dtPrice.Columns.Add("PaxType");

                    int TotalOffers = Convert.ToInt32(ds.Tables["AllOffersSummary"].Rows[0][0].ToString());
                    int TotalPaxTypes = dtPrice.Rows.Count / TotalOffers;

                    for (int s = 0; s < dtPrice.Rows.Count; s++)
                    {
                        if (adults > 0)
                        {
                            dtPrice.Rows[s]["PaxType"] = "ADT";
                            s = s + (TotalPaxTypes - 1);
                        }
                    }

                    if (TotalPaxTypes > 1)
                    {
                        if (children > 0)
                        {
                            for (int s = 1; s < dtPrice.Rows.Count; s++)
                            {
                                if (dtPrice.Rows[s]["PaxType"] != "")
                                {
                                    if (children > 0)
                                    {
                                        dtPrice.Rows[s]["PaxType"] = "CHD";
                                        s = s + (TotalPaxTypes - 1);
                                    }
                                }
                            }
                        }

                        if (infants > 0)
                        {
                            int inf = 0;

                            if (children == 0)
                            {
                                inf = 1;
                            }
                            else
                            {
                                inf = TotalPaxTypes - 1;
                            }

                            for (int s = inf; s < dtPrice.Rows.Count; s++)
                            {
                                if (dtPrice.Rows[s]["PaxType"] != "")
                                {
                                    if (infants > 0)
                                    {
                                        dtPrice.Rows[s]["PaxType"] = "INF";
                                        s = s + (TotalPaxTypes - 1);
                                    }
                                }
                            }
                        }
                    }

                    if (!ds.Tables.Contains("PaxJourneyRefID"))
                    {
                        // Create the table
                        DataTable paxJourneyRefIDTable = new DataTable("PaxJourneyRefID");
                        paxJourneyRefIDTable.Columns.Add("PaxJourneyRefID_Text", typeof(string));
                        paxJourneyRefIDTable.Columns.Add("OriginDest_Id", typeof(int));

                        // Add the table to the DataSet
                        ds.Tables.Add(paxJourneyRefIDTable);
                    }

                    // Retrieve the PaxJourney table
                    DataTable paxJourneyTable = ds.Tables["PaxJourney"];

                    // Populate the PaxJourneyRefID table
                    foreach (DataRow row in paxJourneyTable.Rows)
                    {
                        DataRow newRow = ds.Tables["PaxJourneyRefID"].NewRow();
                        newRow["PaxJourneyRefID_Text"] = row["PaxJourneyID"]; // Assuming 'PaxJourneyID' is a column in 'PaxJourney'
                        newRow["OriginDest_Id"] = "0"; // Insert '0' into the PaxJourneyRefID_Text column
                        ds.Tables["PaxJourneyRefID"].Rows.Add(newRow);
                    }

                    if (!ds.Tables["OriginDest"].Columns.Contains("OriginDest_Id"))
                    {
                        ds.Tables["OriginDest"].Columns.Add("OriginDest_Id", typeof(int));

                        int s = 0;
                        foreach (DataRow row in ds.Tables["OriginDest"].Rows)
                        {
                            row["OriginDest_Id"] = s;
                            s++;
                        }

                    }

                    var OriginDest = (from od in ds.Tables["OriginDest"].AsEnumerable()
                                      join odl in ds.Tables["OriginDestList"].AsEnumerable() on od["OriginDestList_Id"] equals odl["OriginDestList_Id"]
                                      join pjr in ds.Tables["PaxJourneyRefID"].AsEnumerable() on od["OriginDest_Id"] equals pjr["OriginDest_Id"]
                                      join pjl in ds.Tables["PaxJourneyList"].AsEnumerable() on odl["DataLists_Id"] equals pjl["DataLists_Id"]
                                      // join pj in ds.Tables["PaxJourney"].AsEnumerable() on pjl["PaxJourneyList_Id"] equals pj["PaxJourneyList_Id"]

                                      select new
                                      {
                                          OriginDestList_Id = odl["OriginDestList_Id"],
                                          DataLists_Id = odl["DataLists_Id"],
                                          DestCode = od["DestCode"],
                                          OriginCode = od["OriginCode"],
                                          OriginDestID = od["OriginDestID"],
                                          OriginDest_Id = od["OriginDest_Id"],
                                          PaxJourneyRefID_Text = pjr["PaxJourneyRefID_Text"],
                                          PaxJourneyList_Id = pjl["PaxJourneyList_Id"],
                                          // PaxJourneyID = pj["PaxJourneyID"],
                                          // PaxSegmentRefID = pj["PaxSegmentRefID"]
                                      }).ToList();

                    DataTable dtSegments = LINQResultToDataTable(OriginDest);
                    dtSegments.Columns.Add("PaxSegmentRefID");
                    dtSegments.Columns.Add("PaxJourney_ID");

                    foreach (DataRow dr in dtSegments.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["PaxJourney"].Rows)
                        {
                            if (dr["PaxJourneyList_Id"].ToString() == dr1["PaxJourneyList_Id"].ToString())
                            {
                                if (ds.Tables["PaxJourney"].Columns.Contains("PaxSegmentRefID") == true)
                                {
                                    dr["PaxSegmentRefID"] = dr1["PaxSegmentRefID"];
                                }
                                else if (ds.Tables["PaxJourney"].Columns.Contains("PaxJourney_ID") == true)
                                {
                                    dr["PaxJourney_ID"] = dr1["PaxJourney_ID"];
                                }
                            }
                        }
                    }


                    int i = 0;

                    if (ds.Tables.Contains("PaxSegmentRefID"))
                    {
                        foreach (DataRow dr in ds.Tables["PaxSegmentRefID"].Rows)
                        {
                            dr["PaxSegmentRefID_Text"] = dr["PaxSegmentRefID_Text"] + "~" + i++;
                        }
                    }
                    else
                    {
                        DataTable newTable = ds.Tables["PaxJourney"].Copy();
                        newTable.TableName = "PaxSegmentRefID";
                        newTable.Columns.Add("PaxSegmentRefID_Text");
                        newTable.Columns.Add("PaxJourney_Id");
                        ds.Tables.Add(newTable);

                        int s = 0;
                        foreach (DataRow dr in ds.Tables["PaxSegmentRefID"].Rows)
                        {
                            dr["PaxSegmentRefID_Text"] = dr["PaxSegmentRefID"] + "~" + i++;
                            dr["PaxJourney_Id"] = s.ToString();
                            s++;
                        }
                    }

                    i = 0;
                    foreach (DataRow dr in ds.Tables["PaxSegment"].Rows)
                    {
                        dr["PaxSegmentID"] = dr["PaxSegmentID"] + "~" + i++;
                    }


                    var PaxSegment = (from psr in ds.Tables["PaxSegmentRefID"].AsEnumerable()
                                      join ps in ds.Tables["PaxSegment"].AsEnumerable() on psr["PaxSegmentRefID_Text"] equals ps["PaxSegmentID"]
                                      join arr in ds.Tables["Arrival"].AsEnumerable() on ps["PaxSegment_Id"] equals arr["PaxSegment_Id"]
                                      join dep in ds.Tables["Dep"].AsEnumerable() on ps["PaxSegment_Id"] equals dep["PaxSegment_Id"]
                                      select new
                                      {
                                          PaxSegmentRefID_Text = psr["PaxSegmentRefID_Text"],
                                          PaxJourney_Id = psr["PaxJourney_Id"],
                                          PaxSegmentID = ps["PaxSegmentID"],
                                          PaxSegment_Id = ps["PaxSegment_Id"],
                                          Duration = ps["Duration"],
                                          Arr_AircraftScheduledDateTime = arr["AircraftScheduledDateTime"],
                                          Arr_IATA_LocationCode = arr["IATA_LocationCode"],
                                          Arr_StationName = arr["StationName"],
                                          Dep_AircraftScheduledDateTime = dep["AircraftScheduledDateTime"],
                                          Dep_IATA_LocationCode = dep["IATA_LocationCode"],
                                          Dep_StationName = dep["StationName"],
                                      }).ToList();


                    DataTable dtPaxSegment = LINQResultToDataTable(PaxSegment);
                    dtPaxSegment.Columns.Add("PaxJourneyID");

                    dtPaxSegment.Columns["PaxJourneyID"].SetOrdinal(2);


                    foreach (DataRow dr in dtPaxSegment.Rows)
                    {

                        if (!ds.Tables["PaxJourney"].Columns.Contains("PaxJourney_Id"))
                        {

                            foreach (DataRow dr1 in ds.Tables["PaxSegmentRefID"].Rows)
                            {
                                if (dr["PaxJourney_Id"].ToString() == dr1["PaxJourney_Id"].ToString())
                                {
                                    dr["PaxJourneyID"] = dr1["PaxJourneyID"];
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow dr1 in ds.Tables["PaxJourney"].Rows)
                            {
                                if (dr["PaxJourney_Id"].ToString() == dr1["PaxJourney_Id"].ToString())
                                {
                                    dr["PaxJourneyID"] = dr1["PaxJourneyID"];
                                }
                            }
                        }

                    }


                    var offers = (from o in ds.Tables["offer"].AsEnumerable()
                                  join j in ds.Tables["JourneyOverview"].AsEnumerable()
                                  on o.Field<int?>("Offer_Id") equals j.Field<int?>("Offer_Id")
                                  join jp in ds.Tables["JourneyPriceClass"].AsEnumerable()
                                  on j.Field<int?>("JourneyOverview_Id") equals jp.Field<int?>("JourneyOverview_Id")
                                  join OI in ds.Tables["OfferItem"].AsEnumerable()
                                   on o.Field<int?>("Offer_Id") equals OI.Field<int?>("Offer_Id")
                                  join FD in ds.Tables["FareDetail"].AsEnumerable()
                                   on OI.Field<int?>("OfferItem_Id") equals FD.Field<int?>("OfferItem_Id")
                                  join FC in ds.Tables["FareComponent"].AsEnumerable()
                                   on FD.Field<int?>("FareDetail_Id") equals FC.Field<int?>("FareDetail_Id")
                                  join CT in ds.Tables["CabinType"].AsEnumerable()
                                   on FC.Field<int?>("FareComponent_Id") equals CT.Field<int?>("FareComponent_Id")
                                  join RB in ds.Tables["RBD"].AsEnumerable()
                                                       on CT.Field<int?>("FareComponent_Id") equals RB.Field<int?>("FareComponent_Id")
                                  join FP in ds.Tables["FarePriceType"].AsEnumerable()
                                                       on FD.Field<int?>("FareDetail_Id") equals FP.Field<int?>("FareDetail_Id")
                                  join OPT in ds.Tables["OfferItemPaymentTimeLimit"].AsEnumerable()
                                                       on OI.Field<int?>("OfferItem_Id") equals OPT.Field<int?>("OfferItem_Id")
                                  join PTL in ds.Tables["PaymentTimeLimitDate"].AsEnumerable()
                                                       on OPT.Field<int?>("OfferItemPaymentTimeLimit_Id") equals PTL.Field<int?>("OfferItemPaymentTimeLimit_Id")
                                  join Pr in dtPrice.AsEnumerable()
                                                       on OI.Field<int?>("OfferItem_Id") equals Pr.Field<int?>("OfferItem_Id")
                                  join EA in ds.Tables["EquivAmount"].AsEnumerable()
                                                       on Pr.Field<int?>("Price_Id") equals EA.Field<int?>("Price_Id")
                                  select new
                                  {
                                      Offer_Id = o.Field<int?>("Offer_Id"),
                                      PaxJourneyRefID = jp.Field<string>("PaxJourneyRefID"),
                                      OfferID = o.Field<string?>("OfferID"),
                                      OwnerCode = o.Field<string?>("OwnerCode"),
                                      CarrierOffers_Id = o.Field<int?>("CarrierOffers_id"),
                                      JourneyOverview_Id = j.Field<int?>("JourneyOverview_Id"),
                                      OfferItem_Id = OI.Field<int?>("OfferItem_Id"),
                                      MandatoryInd = OI.Field<string?>("MandatoryInd"),
                                      OfferItemID = OI.Field<string>("OfferItemID"),
                                      FareDetail_Id = FD.Field<int?>("FareDetail_Id"),
                                      FareComponent_Id = FC.Field<int?>("FareComponent_Id"),
                                      FareBasisCode = FC.Field<string?>("FareBasisCode"),
                                      FareTypeCode = FC.Field<string?>("FareTypeCode"),
                                      PaxSegmentRefID = FC.Field<string?>("PaxSegmentRefID"),
                                      CabinTypeCode = CT.Field<string?>("CabinTypeCode"),
                                      CabintypeName = CT.Field<string?>("CabinTypeName"),
                                      RBD_Code = RB.Field<string?>("RBD_Code"),
                                      FarePriceTypeCode = FP.Field<string?>("FarePriceTypeCode"),
                                      Price = FP.Field<string?>("Price"),
                                      OfferItemPaymentTimeLimit_Id = OPT.Field<int?>("OfferItemPaymentTimeLimit_Id"),
                                      PaymentTimeLimitDateTime = PTL.Field<string?>("PaymentTimeLimitDateTime"),
                                      Price_Id = Pr.Field<int?>("Price_Id"),
                                      PaxType = Pr.Field<string?>("PaxType"),
                                      CurCode = EA.Field<string?>("CurCode"),
                                      EquivAmount_Text = EA.Field<string?>("EquivAmount_Text"),

                                  }).ToList();

                    DataTable dtOffers = LINQResultToDataTable(offers);
                    dtOffers.Columns.Add("TotalPrice_Id");
                    dtOffers.Columns.Add("Tax");
                    dtOffers.Columns.Add("TotalAmount_Text");


                    foreach (DataRow dr in dtOffers.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["TotalPrice"].Rows)
                        {
                            if (dr["Offer_Id"].ToString() == dr1["Offer_Id"].ToString())
                            {
                                dr["TotalPrice_Id"] = dr1["TotalPrice_Id"];
                            }
                        }
                    }

                    var Taxes = from tp in ds.Tables["TotalPrice"].AsEnumerable()
                                join t in ds.Tables["TaxSummary"].AsEnumerable() on tp.Field<int?>("TotalPrice_ID") equals t.Field<int?>("TotalPrice_Id")
                                join tt in ds.Tables["TotalTaxAmount"].AsEnumerable() on t.Field<int?>("TaxSummary_ID") equals tt.Field<int?>("TaxSummary_Id")
                                select new
                                {
                                    TotalPrice_ID = tp.Field<int?>("TotalPrice_ID"),
                                    Offer_Id = tp.Field<int?>("Offer_Id"),
                                    TaxSummary_ID = t.Field<int?>("TaxSummary_ID"),
                                    CurCode = tt.Field<string?>("CurCode"),
                                    TotalTaxAmount_Text = tt.Field<string?>("TotalTaxAmount_Text")
                                };
                    DataTable dtTaxes = LINQResultToDataTable(Taxes);


                    foreach (DataRow dr in dtOffers.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["TotalAmount"].Rows)
                        {
                            if (dr["TotalPrice_Id"].ToString() == dr1["TotalPrice_Id"].ToString())
                            {
                                dr["TotalAmount_Text"] = dr1["TotalAmount_Text"];
                            }
                        }
                    }

                    foreach (DataRow dr in dtOffers.Rows)
                    {
                        foreach (DataRow dr1 in dtTaxes.Rows)
                        {
                            if (dr["TotalPrice_Id"].ToString() == dr1["TotalPrice_Id"].ToString())
                            {
                                dr["Tax"] = dr1["TotalTaxAmount_Text"];
                            }
                        }
                    }


                    DataTable Flights = new DataTable();
                    Flights = dtPaxSegment.Copy();

                    Flights.Columns.Add("Offer_ID");
                    Flights.Columns.Add("OfferID");
                    Flights.Columns.Add("OwnerCode");
                    Flights.Columns.Add("CarrierOffers_Id");
                    Flights.Columns.Add("JourneyOverview_Id");
                    Flights.Columns.Add("PaxJourneyRefID");
                    Flights.Columns.Add("OfferItem_Id");
                    Flights.Columns.Add("MandatoryInd");
                    Flights.Columns.Add("OfferItemID");
                    Flights.Columns.Add("FareDetail_Id");
                    Flights.Columns.Add("FareComponent_Id");
                    Flights.Columns.Add("FareBasisCode");
                    Flights.Columns.Add("FareTypeCode");
                    Flights.Columns.Add("PaxSegmentRefID");
                    Flights.Columns.Add("CabinTypeCode");
                    Flights.Columns.Add("CabintypeName");
                    Flights.Columns.Add("RBD_Code");
                    Flights.Columns.Add("FarePriceTypeCode");
                    Flights.Columns.Add("Price");
                    Flights.Columns.Add("OfferItemPaymentTimeLimit_Id");
                    Flights.Columns.Add("PaymentTimeLimitDateTime");
                    Flights.Columns.Add("Price_Id");
                    Flights.Columns.Add("CurCode");
                    Flights.Columns.Add("EquivAmount_Text");
                    Flights.Columns.Add("FlightType");
                    Flights.Columns.Add("Direct");
                    Flights.Columns.Add("TotalPrice_Id");
                    Flights.Columns.Add("Tax");
                    Flights.Columns.Add("TotalAmount_Text");
                    Flights.Columns.Add("Child_TotalPrice_Id");
                    Flights.Columns.Add("Inf_TotalPrice_Id");

                    Flights.Columns["PaxJourneyRefID"].SetOrdinal(1);
                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in dtOffers.Rows)
                        {
                            if (dr["PaxJourneyId"].ToString() == dr1["PaxJourneyRefID"].ToString())
                            {
                                if (dr1["PaxType"].ToString() == "ADT")
                                {
                                    dr["Offer_ID"] = dr1["Offer_ID"];
                                    dr["OfferID"] = dr1["OfferID"];
                                    dr["OwnerCode"] = dr1["OwnerCode"];
                                    dr["CarrierOffers_Id"] = dr1["CarrierOffers_Id"];
                                    dr["JourneyOverview_Id"] = dr1["JourneyOverview_Id"];
                                    dr["PaxJourneyRefID"] = dr1["PaxJourneyRefID"];
                                    dr["OfferItem_Id"] = dr1["OfferItem_Id"];
                                    dr["MandatoryInd"] = dr1["MandatoryInd"];
                                    dr["OfferItemID"] = dr1["OfferItemID"];
                                    dr["FareDetail_Id"] = dr1["FareDetail_Id"];
                                    dr["FareComponent_Id"] = dr1["FareComponent_Id"];
                                    dr["FareBasisCode"] = dr1["FareBasisCode"];
                                    dr["FareTypeCode"] = dr1["FareTypeCode"];
                                    dr["PaxSegmentRefID"] = dr1["PaxSegmentRefID"];
                                    dr["CabinTypeCode"] = dr1["CabinTypeCode"];
                                    dr["CabintypeName"] = dr1["CabintypeName"];
                                    dr["RBD_Code"] = dr1["RBD_Code"];
                                    dr["FarePriceTypeCode"] = dr1["FarePriceTypeCode"];
                                    dr["Price"] = dr1["Price"];
                                    dr["OfferItemPaymentTimeLimit_Id"] = dr1["OfferItemPaymentTimeLimit_Id"];
                                    dr["PaymentTimeLimitDateTime"] = dr1["PaymentTimeLimitDateTime"];
                                    dr["Price_Id"] = dr1["Price_Id"];
                                    dr["CurCode"] = dr1["CurCode"];
                                    dr["EquivAmount_Text"] = dr1["EquivAmount_Text"];
                                    dr["TotalPrice_Id"] = dr1["TotalPrice_Id"];
                                    dr["Tax"] = dr1["Tax"];
                                    dr["TotalAmount_Text"] = dr1["TotalAmount_Text"];
                                    dr["Child_TotalPrice_Id"] = 0;
                                    dr["Inf_TotalPrice_Id"] = 0;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in dtOffers.Rows)
                        {
                            if (dr["PaxJourneyId"].ToString() == dr1["PaxJourneyRefID"].ToString())
                            {
                                if (dr1["PaxType"].ToString() == "CHD")
                                {
                                    dr["Child_TotalPrice_Id"] = dr1["EquivAmount_Text"];
                                }
                            }
                        }
                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in dtOffers.Rows)
                        {
                            if (dr["PaxJourneyId"].ToString() == dr1["PaxJourneyRefID"].ToString())
                            {
                                if (dr1["PaxType"].ToString() == "INF")
                                {
                                    dr["Inf_TotalPrice_Id"] = dr1["EquivAmount_Text"];
                                }
                            }
                        }
                    }



                    Flights.Columns.Add("CarrierDesigCode");
                    Flights.Columns.Add("CarrierName");
                    Flights.Columns.Add("OperatingCarrierFlightNumberText");
                    Flights.Columns.Add("OriginalCost");
                    Flights.Columns.Add("TotalDuration");

                    List<NDCFlight> flights = ConvertDataTableToList(Flights);

                    var flightDurations = from flight in flights
                                          group flight by flight.Offer_ID into flightGroup
                                          select new
                                          {
                                              Offer_Id = flightGroup.Key,
                                              TotalDuration = FormatDuration(CalculateTotalDuration(flightGroup))
                                          };

                    foreach (DataRow fl in Flights.Rows)
                    {
                        foreach (var itm in flightDurations)
                        {
                            if (fl["Offer_Id"].ToString() == itm.Offer_Id.ToString())
                            {
                                fl["TotalDuration"] = itm.TotalDuration;
                            }
                        }

                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        foreach (DataRow dr1 in ds.Tables["OperatingCarrierInfo"].Rows)
                        {
                            if (dr["PaxSegment_Id"].ToString() == dr1["PaxSegment_Id"].ToString())
                            {
                                dr["OperatingCarrierFlightNumberText"] = dr1["OperatingCarrierFlightNumberText"];
                                dr["CarrierName"] = dr1["CarrierName"];
                                dr["CarrierDesigCode"] = dr1["CarrierDesigCode"];
                            }
                        }
                    }

                    foreach (DataRow dr in Flights.Rows)
                    {
                        if (fromM_1.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_1.Contains(dr["Arr_IATA_LocationCode"].ToString()))
                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 1;
                        }
                        else if (fromM_1.Contains(dr["Dep_IATA_LocationCode"].ToString()) && !toM_1.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 0;
                        }
                        else if (!fromM_1.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_1.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DC";
                            dr["Direct"] = 0;
                        }
                        else if (fromM_2.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_2.Contains(dr["Arr_IATA_LocationCode"].ToString()))
                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 1;
                        }
                        else if (fromM_2.Contains(dr["Dep_IATA_LocationCode"].ToString()) && !toM_2.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 0;
                        }
                        else if (!fromM_2.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_2.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DC";
                            dr["Direct"] = 0;
                        }
                        else if (fromM_3.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_3.Contains(dr["Arr_IATA_LocationCode"].ToString()))
                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 1;
                        }
                        else if (fromM_3.Contains(dr["Dep_IATA_LocationCode"].ToString()) && !toM_3.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 0;
                        }
                        else if (!fromM_3.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_3.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DC";
                            dr["Direct"] = 0;
                        }
                        else if (fromM_4.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_4.Contains(dr["Arr_IATA_LocationCode"].ToString()))
                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 1;
                        }
                        else if (fromM_4.Contains(dr["Dep_IATA_LocationCode"].ToString()) && !toM_4.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 0;
                        }
                        else if (!fromM_4.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_4.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DC";
                            dr["Direct"] = 0;
                        }
                        else if (fromM_5.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_5.Contains(dr["Arr_IATA_LocationCode"].ToString()))
                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 1;
                        }
                        else if (fromM_5.Contains(dr["Dep_IATA_LocationCode"].ToString()) && !toM_5.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 0;
                        }
                        else if (!fromM_5.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_5.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DC";
                            dr["Direct"] = 0;
                        }
                        else if (fromM_6.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_6.Contains(dr["Arr_IATA_LocationCode"].ToString()))
                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 1;
                        }
                        else if (fromM_6.Contains(dr["Dep_IATA_LocationCode"].ToString()) && !toM_6.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DM";
                            dr["Direct"] = 0;
                        }
                        else if (!fromM_6.Contains(dr["Dep_IATA_LocationCode"].ToString()) && toM_6.Contains(dr["Arr_IATA_LocationCode"].ToString()))

                        {
                            dr["FlightType"] = "DC";
                            dr["Direct"] = 0;
                        }
                        else
                        {
                            dr["FlightType"] = "DC";
                            dr["Direct"] = 0;
                        }
                    }



                    // Use a list to collect rows that need to be deleted
                    List<DataRow> rowsToDelete = new List<DataRow>();

                    foreach (DataRow ro in Flights.Rows)
                    {
                        ro["OriginalCost"] = ro["TotalAmount_Text"];

                        // Check if "EquivAmount_Text" is null or empty
                        if (ro["EquivAmount_Text"] == null || ro["EquivAmount_Text"].ToString() == "")
                        {
                            rowsToDelete.Add(ro);
                        }
                    }

                    // Remove the collected rows after the iteration
                    foreach (DataRow ro in rowsToDelete)
                    {
                        Flights.Rows.Remove(ro);
                    }

                    Flights.AcceptChanges();

                    foreach (DataRow ro in Flights.Rows)
                    {
                        decimal grandTotal = 0;
                        ro["OriginalCost"] = ro["TotalAmount_Text"];
                      
                            decimal[] TotalAmt = _AddMarkup(Convert.ToDecimal(ro["EquivAmount_Text"]), Convert.ToDecimal(ro["Child_TotalPrice_Id"]), Convert.ToDecimal(ro["Inf_TotalPrice_Id"]), adults, children, infants, ro["CarrierDesigCode"].ToString(), userid);

                            //decimal[] TotalAmt = _AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Childs, Infants, AirlineCode);

                            //totalfare = TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                            grandTotal = TotalAmt[2] + TotalAmt[3] + TotalAmt[4] + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                            ro["TotalAmount_Text"] = grandTotal + Convert.ToDecimal(ro["Tax"]);
                        
                    }

                    Flights.Columns["FlightType"].SetOrdinal(0);
                    Flights.Columns["Direct"].SetOrdinal(1);

                    foreach (DataRow dr in Flights.Rows)
                    {
                        string[] str;
                        string[] _str;
                        str = dr["PaxSegmentRefID_Text"].ToString().Split('~');
                        _str = dr["PaxSegmentID"].ToString().Split('~');

                        dr["PaxSegmentRefID_Text"] = str[0];
                        dr["PaxSegmentID"] = _str[0];
                    }

                    Flights.Columns.Add("ShoppingResponseRefID");
                    Flights.Columns.Add("weight");
                    Flights.Columns.Add("unitOfMeasureCode");

                    Flights.Columns["ShoppingResponseRefID"].SetOrdinal(0);

                    string CityName = "";

                    foreach (DataRow dr in Flights.Rows)
                    {
                        dr["ShoppingResponseRefID"] = ds.Tables["ShoppingResponse"].Rows[0][0];
                        CityName = getCityName(dr["Dep_StationName"].ToString());

                        if (CityName != null && CityName != "")
                        {
                            dr["Dep_StationName"] = CityName;
                        }
                    }

                    CityName = null;

                    // Commnent above 9 lines before publishing

                    Guid TransactionId = Guid.NewGuid();

                    Flights.Columns.Add("TransactionId");
                    foreach (DataRow dr in Flights.Rows)
                    {
                        dr["TransactionId"] = TransactionId;
                    }

                    DataView dv = Flights.DefaultView;
                    // dv.Sort = "GrandTotal";
                    Flights = dv.ToTable();

                    //MemoryStream stream = new MemoryStream();
                    //ds.WriteXml(stream);
                    //byte[] hititResponse = stream.ToArray();
                    //stream = null;

                    //MemoryStream streamFL = new MemoryStream();
                    //dtFareList.TableName = "dtFareList";
                    //dtFareList.WriteXml(streamFL);
                    //byte[] fareList = streamFL.ToArray();
                    //streamFL = null;

                    //MemoryStream streamCI = new MemoryStream();
                    //dtCityInfo.TableName = "dtCityInfo";
                    //dtCityInfo.WriteXml(streamCI);
                    //byte[] cityInfo = streamCI.ToArray();
                    //streamCI = null;

                    //   ds.Tables["flightNotes"].WriteXml(stream);
                    //   byte[] flightNotes = stream.ToArray();

                    //MemoryStream streamFligts = new MemoryStream();
                    //Flights.TableName = "flights";
                    //Flights.WriteXml(streamFligts);
                    //byte[] finalResult = streamFligts.ToArray();
                    //streamFligts = null;


                    //HitItResult hr = new HitItResult();
                    //hr.TransactionId = TransactionId;
                    //hr.Origin = from;
                    //hr.Destination = to;
                    //hr.TotalPax = adults + children + infants;
                    //hr.StartDate = Dept;
                    //hr.HititResponse = hititResponse;
                    //// hr.FareList = fareList;
                    ////  hr.CityInfo = cityInfo;
                    //// hr.FlightNotes = flightNotes;
                    //hr.FinalResult = finalResult;
                    //hr.Active = true;
                    //sql.HitItResults.Add(hr);
                    //sql.SaveChanges();

                    string JSONString = "something went wrong";
                    //if (Flights != null)
                    //{
                    //    JSONString = JsonConvert.SerializeObject(Flights);
                    //}

                    // Parse the JSON string into a JObject for further processing
                    //  JObject jsonObject = JObject.Parse(jsonString);

                    JSONString = DataTableToJSONWithStringBuilder(Flights);
                    return JSONString;

                }
                return "something went wrong";
            }
            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message;
                return ex.Message.ToString();// "something went wrong";
            }
        }

        [NonAction]
        public async Task<string> HitItMultiBooking(string req)
        {
            try
            {
                var doc = XDocument.Parse(req);
                DataSet ds = new DataSet();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //string url = "https://app.crane.aero/cranendc/v20.1/CraneNDCService";
                string url = "https://app-stage.crane.aero/cranendc/v20.1/CraneNDCService";


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
                // request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");
                request.Headers.Add("username", "PSA2746366");
                 request.Headers.Add("password", "Pia123"); //Test password
                //request.Headers.Add("password", "Sam7890@#_"); // production password

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

                //result1 = result1.Replace("universal:", "");
                //result1 = result1.Replace("common_v42_0:", "");
                //result1 = result1.Replace("air:", "");

                ////Log

                string fileName = @"C:\hallmark\doOrderCreate_2A1C1I_req.txt";
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

                fileName = @"C:\hallmark\doOrderCreate_2A1C1I_res.txt";
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
                        if (!ds.Tables.Contains("DataLists"))
                        {
                            // ViewBag.Message = "something went wrong."; //ds.Tables["Fault"].Rows[0]["faultstring"].ToString();
                            return "something went wrong";
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

                        return "something went wrong";
                    }

                }
                return "something went wrong";
            }
            catch (Exception ex)
            {
                // throw new Exception;
                return ex.Message.ToString();
            }
        }

        [NonAction]
        protected string OneWayBookingXML(string strReq1, DataTable Flights, DataTable dtFareList, string resBookDesigCode,
                string ShoppingResponseRefID, DataTable cityInfo, string OfferRefID, string PaxRefID)
        {
            try
            {
                StringBuilder strReq = new StringBuilder();
                //  Guid TraceId = Guid.NewGuid();

                strReq.Append(@"<soapenv:Envelope xmlns:soapenv = 'http://schemas.xmlsoap.org/soap/envelope/' xmlns = 'http://www.iata.org/IATA/2015/00/2020.1/IATA_OrderCreateRQ'>");
                strReq.Append(@"<soapenv:Header/>");
                strReq.Append(@"<soapenv:Body>");
                strReq.Append(@"<IATA_OrderCreateRQ>");
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
                strReq.Append(@"<CreateOrder>");
                strReq.Append(@"<SelectedOffer>" + ShoppingResponseRefID);
                strReq.Append(@"<OfferRefID>" + OfferRefID + "</OfferRefID>");
                strReq.Append(@"<OwnerCode>PK</OwnerCode>");
                strReq.Append(@"<SelectedOfferItem>");
                strReq.Append(@"<OfferItemRefID>OFF1</OfferItemRefID>");
                strReq.Append(@"<PaxRefID>" + PaxRefID + "</PaxRefID>");
                strReq.Append(@"</SelectedOfferItem>");
                strReq.Append(@"<ShoppingResponseRefID>" + ShoppingResponseRefID + "</ShoppingResponseRefID>");
                strReq.Append(@"<TotalOfferPriceAmount CurCode='PKR'>" + Flights.Rows[0]["TotalAmount_Text"].ToString() + "</TotalOfferPriceAmount>");
                strReq.Append(@"</SelectedOffer>");
                strReq.Append(@"</CreateOrder>");
                strReq.Append(@"<DataLists>");
                strReq.Append(strReq1);
                strReq.Append(@"</PaxList>");
                strReq.Append(@"</DataLists>");
                strReq.Append(@"<OrderCreateParameters>");
                strReq.Append(@"<CurParameter>");
                strReq.Append(@"<CurCode>PKR</CurCode>");
                strReq.Append(@"</CurParameter>");
                strReq.Append(@"</OrderCreateParameters>");
                strReq.Append(@"</Request>");
                strReq.Append(@"</IATA_OrderCreateRQ>");
                strReq.Append(@"</soapenv:Body>");
                strReq.Append(@"</soapenv:Envelope>");

                return strReq.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpPost("hititmultibooking")]
        public string HitItMultiBooking([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                var frm = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                //string flightsList = frm?.TotalFlights; // JsonConvert.DeserializeObject<dynamic>(frm?.TotalFlights);

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
                            string flightCode = flightToken["FlightType"]?.ToString();
                            string segmentID = flightToken["PaxSegment_Id"]?.ToString();

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

                    int UserID = frm?.userID?.Value != null ? Convert.ToInt32(frm?.userID?.Value) : 1; // Convert.ToInt32(Session["UserID"]);
                    Random random = new Random();
                    string num = random.Next(9).ToString();


                    //  int UserID = user.intUserID;
                    //data.firstName_0.Value
                    int Adults = frm?.Adults?.Value != null ? Convert.ToInt32(frm?.Adults?.Value) : 1;
                    int Children = frm?.Children?.Value != null ? Convert.ToInt32(frm?.Children?.Value) : 0;
                    int Infants = frm?.Infants?.Value != null ? Convert.ToInt32(frm?.Infants?.Value) : 0;
                    decimal OriginalCost = frm?.OriginalCost?.Value != null ? Convert.ToDecimal(frm?.OriginalCost?.Value) : 0;

                    if (OriginalCost == 0)
                    {
                        OriginalCost = frm?.originalCost?.Value != null ? Convert.ToDecimal(frm?.originalCost?.Value) : 0;
                    }

                    decimal? grandtotal = frm?.GrandTotal?.Value != null ? Convert.ToDecimal(frm?.GrandTotal?.Value) : 0;

                    string CancelPenalty = frm?.CancelPenalty?.Value != null ? Convert.ToString(frm?.CancelPenalty?.Value) : "0";
                    string ChangePenalty = frm?.ChangePenalty?.Value != null ? Convert.ToString(frm?.ChangePenalty?.Value) : "0";
                    string penalty = frm.Penalty;

                    string from = frm?.from?.Value;
                    string to = frm?.to?.Value;
                    string totaltime0 = frm?.totaltime0?.Value;
                    string totaltime1 = frm?.totaltime1?.Value;
                    string stops0 = frm?.stops0?.Value;
                    string stops1 = frm?.stops1?.Value;
                    string amount = frm?.amount0?.Value;
                    string taxes = frm?.taxes0?.Value;
                    string total = frm?.total0?.Value;
                    string currency = frm?.currency0?.Value;
                    //string serviceclass = ""; //frm?.cabin.Value;
                    string optionID = ""; // frm?.Option_Id.Value;
                                          //  Guid TransactionId = Guid.Parse(frm?.TransactionId?.Value);
                    DateTime Arr_AircraftScheduledDateTime = DateTime.MinValue; //frm?["Arr_AircraftScheduledDateTime"].ToString();
                    DateTime Dep_AircraftScheduledDateTime = DateTime.MinValue;

                    string arrdateTimeString = frm?.Arr_AircraftScheduledDateTime?.Value.ToString();

                    if (!string.IsNullOrEmpty(arrdateTimeString))
                    {
                        Arr_AircraftScheduledDateTime = DateTime.Parse(arrdateTimeString);
                    }

                    string depdateTimeString = frm?.Dep_AircraftScheduledDateTime?.Value.ToString();

                    if (!string.IsNullOrEmpty(depdateTimeString))
                    {
                        Dep_AircraftScheduledDateTime = DateTime.Parse(depdateTimeString);
                    }

                    string Departure = frm?.Dep_locationCode?.Value;

                    string Arrival = frm?.Arr_locationCode?.Value;
                    string resBookDesigCode = frm?.ResBookDesigCode?.Value;
                    string ShoppingResponseRefID = frm?.ShoppingResponseRefID?.Value;
                    string OfferRefID = frm?.OfferID?.Value?.ToString();
                    string Cabin = frm?.Cabin?.Value?.ToString();

                    //if (Cabin == null || Cabin == "")
                    //{
                    //    Cabin = !string.IsNullOrEmpty(frm?.RBD_Code.Value) ? frm?.RBD_Code.Value.ToString() : "";

                    //}

                    string fareComponentGroupList_Id = frm?.FareComponentGroupList_Id?.Value;

                    string leadfirstname = !string.IsNullOrEmpty(frm?.firstName_0.Value) ? frm?.firstName_0.Value.ToString().ToUpper() : "";
                    string leadlastname = !string.IsNullOrEmpty(frm?.lastName_0.Value) ? frm?.lastName_0.Value.ToString().ToUpper() : "";
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

                    string flightType = !string.IsNullOrEmpty(frm?.flightType?.Value) ? frm?.flightType?.Value.ToString().ToUpper() : "";

                    string fareReferenceID = !string.IsNullOrEmpty(frm?.fareReferenceID?.Value) ? frm?.fareReferenceID?.Value.ToString() : "";

                    string resBookDesigCode1 = "";
                    string fareComponentGroupList_Id1 = "";
                    string optionID_r = "";
                    //Guid TransactionId = Guid.Parse(frm?.TransactionId_r.Value);
                    DateTime Arr_AircraftScheduledDateTime_r = DateTime.Now;
                    string Departure_r = "";
                    DateTime Dep_AircraftScheduledDateTime_r = DateTime.Now;
                    string Arrival_r = "";
                    string resBookDesigCode_r = "";
                    string Cabin_r = "";
                    string fareComponentGroupList_Id_r = "0"; // frm?.FareComponentGroupList_Id_r.Value;

                    if (flightType == "RETURN")
                    {
                        fareComponentGroupList_Id_r = frm?.FareComponentGroupList_Id_r?.Value;
                        resBookDesigCode1 = frm?.ResBookDesigCode_r?.Value;
                        fareComponentGroupList_Id1 = frm?.FareComponentGroupList_Id_r?.Value;
                        optionID_r = frm?.Option_Id_r?.Value;
                        Arr_AircraftScheduledDateTime_r = frm?.Arr_AircraftScheduledDateTime_r?.Value;
                        Departure_r = frm?.Dep_locationCode_r?.Value;
                        Dep_AircraftScheduledDateTime_r = frm?.Dep_AircraftScheduledDateTime_r?.Value;
                        Arrival_r = frm?.Arr_locationCode_r?.Value;
                        resBookDesigCode_r = frm?.ResBookDesigCode_r?.Value;
                        Cabin_r = frm?.Cabin_r?.Value;
                    }




                    string _ticket = ""; // frm?.chkTicket.Value;

                    DateTime datet = DateTime.Now.Date;
                    string Dat = datet.ToString("yyyy-MM-dd");

                    DateTime dt1 = DateTime.UtcNow;
                    string tstamp = dt1.ToString("s") + "Z";
                    //string AirlineCode = "PK";
                    //bool error = false;
                    string email = !string.IsNullOrEmpty(frm?.email_0?.Value) ? frm?.email_0?.Value.ToString() : "";
                    string contact = !string.IsNullOrEmpty(frm?.MobileNO_0?.Value) ? frm?.MobileNO_0?.Value.ToString() : "";



                    #region XML Request

                    string xmlRequest = "";
                    StringBuilder strReq = new StringBuilder();
                    title = "Mr";
                    string PaxRefID = "";

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

                            title = "Mr";
                            if (sex == "F")
                            {
                                sex = "F";
                                title = "Ms";
                            }
                            else
                            {
                                sex = "M";
                            }

                            if (i == 0)
                            {
                                strReq.Append(@"<ContactInfoList>");
                                strReq.Append(@"<ContactInfo>");
                                strReq.Append(@"<ContactInfoID>Contact-1</ContactInfoID>");
                                strReq.Append(@"<EmailAddress>");
                                strReq.Append(@"<EmailAddressText>" + email + "</EmailAddressText>");
                                strReq.Append(@"</EmailAddress>");
                                strReq.Append(@"<Phone>");
                                strReq.Append(@"<AreaCodeNumber>" + mobile.Substring(0, 3) + "</AreaCodeNumber>");
                                strReq.Append(@"<CountryDialingCode>92</CountryDialingCode>");
                                strReq.Append(@"<PhoneNumber>" + mobile.Substring(3, 7) + "</PhoneNumber>");
                                strReq.Append(@"</Phone>");
                                strReq.Append(@"</ContactInfo>");
                                strReq.Append(@"</ContactInfoList>");
                                strReq.Append(@"<PaxList>");
                            }


                            strReq.Append(@"<Pax>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ContactInfoRefID>Contact-1</ContactInfoRefID>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID > ");
                            strReq.Append(@"<IdentityDocTypeCode>NATIONAL_ID</IdentityDocTypeCode >");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID>");
                            strReq.Append(@"<IdentityDocTypeCode>PASSPORT</IdentityDocTypeCode>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ExpiryDate>" + expiry + "</ExpiryDate>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<Individual>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<IndividualID>IND-ADT" + i + "</IndividualID>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"<TitleName>" + title + "</TitleName>");
                            strReq.Append(@"</Individual>");
                            strReq.Append(@"<PaxID>PAX-ADT" + i + "</PaxID>");
                            strReq.Append(@"<PTC>ADT</PTC>");
                            strReq.Append(@"</Pax>");

                            PaxRefID = PaxRefID + "PAX-ADT" + i + " ";
                        }
                    }

                    if (Children > 0)
                    {
                        for (int c = 1; c <= Children; c++)
                        {
                            sex = frm?["sexC_" + c.ToString()]?.ToString();
                            document = frm?["documentC_" + c.ToString()]?.ToString();
                            documenttype = frm?["documenttypeC_" + c.ToString()]?.ToString();
                            expiry = frm?["expiryC_" + c.ToString()]?.ToString();
                            nationality = frm?["nationalityC_" + c.ToString()]?.ToString();
                            firstName = frm?["firstNameC_" + c.ToString()]?.ToString();
                            lastName = frm?["lastNameC_" + c.ToString()]?.ToString();
                            DateTime dtDOB = Convert.ToDateTime(frm?["dobC_" + c.ToString()]?.ToString());
                            //TimeSpan t = DateTime.Now - dtDOB;
                            dob = frm?["dobC_" + c.ToString()]?.ToString().ToString();

                            title = "Mr";
                            if (sex == "F")
                            {
                                sex = "F";
                                title = "Ms";
                            }
                            else
                            {
                                sex = "M";
                            }
                            strReq.Append(@"<Pax>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ContactInfoRefID>Contact-1</ContactInfoRefID>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID > ");
                            strReq.Append(@"<IdentityDocTypeCode>NATIONAL_ID</IdentityDocTypeCode >");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID>");
                            strReq.Append(@"<IdentityDocTypeCode>PASSPORT</IdentityDocTypeCode>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ExpiryDate>" + expiry + "</ExpiryDate>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<Individual>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<IndividualID>IND-CHD" + c + "</IndividualID>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"<TitleName>" + title + "</TitleName>");
                            strReq.Append(@"</Individual>");
                            strReq.Append(@"<PaxID>PAX-CHD" + c + "</PaxID>");
                            strReq.Append(@"<PTC>CHD</PTC>");
                            strReq.Append(@"</Pax>");

                            PaxRefID = PaxRefID + "PAX-CHD" + c + " ";
                        }
                    }


                    if (Infants > 0)
                    {
                        for (int i = 1; i <= Infants; i++)
                        {

                            dob = frm?["dobI_" + i.ToString()]?.ToString();
                            sex = frm?["sexI_" + i.ToString()]?.ToString();
                            document = frm?["documentI_" + i.ToString()]?.ToString();
                            documenttype = frm?["documenttypeI_" + i.ToString()]?.ToString();
                            expiry = frm?["expiryI_" + i.ToString()]?.ToString();
                            nationality = frm?["nationalityI_" + i.ToString()]?.ToString();
                            firstName = frm?["firstNameI_" + i.ToString()]?.ToString();
                            lastName = frm?["lastNameI_" + i.ToString()]?.ToString();

                            title = "Mr";
                            if (sex == "F")
                            {
                                sex = "F";
                                title = "Ms";
                            }
                            else
                            {
                                sex = "M";
                            }


                            strReq.Append(@"<Pax>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ContactInfoRefID>Contact-1</ContactInfoRefID>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID > ");
                            strReq.Append(@"<IdentityDocTypeCode>NATIONAL_ID</IdentityDocTypeCode >");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<IdentityDoc>");
                            strReq.Append(@"<IdentityDocID>" + document + "</IdentityDocID>");
                            strReq.Append(@"<IdentityDocTypeCode>PASSPORT</IdentityDocTypeCode>");
                            strReq.Append(@"<Birthdate>" + dob + "</Birthdate>");
                            strReq.Append(@"<CitizenshipCountryCode>PK</CitizenshipCountryCode>");
                            strReq.Append(@"<ExpiryDate>" + expiry + "</ExpiryDate>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"</IdentityDoc>");
                            strReq.Append(@"<Individual>");
                            strReq.Append(@"<GenderCode>" + sex + "</GenderCode>");
                            strReq.Append(@"<GivenName>" + firstName + "</GivenName>");
                            strReq.Append(@"<IndividualID>IND-INF" + i + "</IndividualID>");
                            strReq.Append(@"<Surname>" + lastName + "</Surname>");
                            strReq.Append(@"<TitleName>" + title + "</TitleName>");
                            strReq.Append(@"</Individual>");
                            strReq.Append(@"<PaxID>PAX-INF" + i + "</PaxID>");
                            strReq.Append(@"<PaxRefID>PAX-ADT0</PaxRefID>");
                            strReq.Append(@"<PTC>INF</PTC>");
                            strReq.Append(@"</Pax>");

                            PaxRefID = PaxRefID + "PAX-INF" + i + " ";
                        }
                    }

                    xmlRequest = OneWayBookingXML(strReq.ToString(), dtFlights, null, resBookDesigCode, ShoppingResponseRefID, null, OfferRefID, PaxRefID.Trim());

                    if (flightType == "ONEWAY")
                    {


                    }
                    else if (flightType == "RETURN")
                    {


                        //xmlRequest = ReturnBookingXML(strReq.ToString(), item, fareDetails, Returnitem, ReturnfareDetails, DepartureCity, ArrivalCity, resBookDesigCode, deiCode, explanation, note, resBookDesigCode1, deiCode1, explanation1, note1);
                    }

                    #endregion

                    //Call Booking endpoint
                    string result1 = HitItMultiBooking(xmlRequest).Result;

                    if (result1 == "something went wrong")
                    {
                        return "something went wrong";
                    }

                    //XmlDocument doc = new XmlDocument();
                    //doc.Load("C:\\Users\\Hp\\Downloads\\ret-reposne.xml");
                    //string result1 = doc.OuterXml;

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(result1);
                    XmlReader xmlReader = new XmlNodeReader(xml);

                    DataSet ds = new DataSet();
                    ds.ReadXml(xmlReader);

                    if (ds.Tables.Count < 1)
                    {
                        return "something went wrong";
                    }

                    DataTable dtAirPricingInfo;

                    //  string AirPricingInfoKey = "1";

                    // ds2.ReadXml(AirPricingSolution);
                    List<string> Order = new List<string>();

                    if (ds.Tables.Contains("Order"))
                    {
                        foreach (DataRow ro in ds.Tables["Order"].Rows)
                        {
                            Order.Add(ro["OrderID"].ToString());
                        }
                        dtAirPricingInfo = (DataTable)ds.Tables["Order"];
                    }
                    else
                    {
                        Order.Add(ds.Tables["Order"].Rows[0]["ID"].ToString());
                        dtAirPricingInfo = (DataTable)ds.Tables["Order"];
                    }

                    string ID1 = ds.Tables["Order"].Rows[0]["Order_ID"].ToString();
                    string PnrForTicket = ds.Tables["Order"].Rows[0]["OrderID"].ToString();

                    if (Order.Count > 0)
                    {
                        #region Booking Info


                        string fsID = flightSegmentIDsForFlightDM.ToString();

                        var Fares = (from f in dtFlights.AsEnumerable().Where(f => f.Field<string?>("PaxSegment_Id") == fsID)
                                     select new
                                     {
                                         AdultTotalPrice = f.Field<string?>("TotalAmount_Text"),
                                         AdultBasePrice = f.Field<string?>("EquivAmount_Text"),
                                         AdultTaxes = f.Field<string?>("Tax"),
                                         ChildTotalPrice = 0,//f.Field<string?>("ChildTotalPrice"),
                                         ChildBasePrice = 0,//f.Field<string?>("ChildBasePrice"),
                                         ChildTaxes = 0,//f.Field<string?>("ChildTaxes"),
                                         InfantTotalPrice = 0,//f.Field<string?>("InfantTotalPrice"),
                                         InfantBasePrice = 0,//f.Field<string?>("InfantBasePrice"),
                                         InfantTaxes = 0,//f.Field<string?>("InfantTaxes"),
                                     }).FirstOrDefault();



                        decimal? Adultsfare = Convert.ToDecimal(Fares.AdultBasePrice);
                        decimal? Childfare = Convert.ToDecimal(Fares.ChildBasePrice);
                        decimal? Infantfare = Convert.ToDecimal(Fares.InfantBasePrice);
                        decimal? AdultTaxes = Convert.ToDecimal(Fares.AdultTaxes);
                        decimal? ChildTaxes = Convert.ToDecimal(Fares.ChildTaxes);
                        decimal? InfnatTaxes = Convert.ToDecimal(Fares.InfantTaxes);
                        decimal? totalfare = 0;
                        decimal? totaltaxes = 0;




                        DateTime dttrans = DateTime.Now.ToUniversalTime();

                        // decimal[] TotalAmt = 1000; //_AddMarkup(Adultsfare, Childfare, Infatfare, Adults, Children, Infants, AirlineCode);

                        totaltaxes = AdultTaxes + ChildTaxes + InfnatTaxes;
                        totalfare = Adultsfare + Childfare + Infantfare; //TotalAmt[2] + TotalAmt[3] + TotalAmt[4];
                                                                         //grandtotal = totalfare + totaltaxes + 0;// + TotalAmt[5] - TotalAmt[0] - TotalAmt[1];
                                                                         //if (Adults > 0) { Adultsfare = TotalAmt[2] / Adults; }
                                                                         //if (Children > 0) { Childfare = TotalAmt[3] / Children; }
                                                                         //if (Infants > 0) { Infatfare = TotalAmt[4] / Infants; }


                        // List<MalikExpressMVC.Models.XML.OriginDestinationOption> odSegments = (List<MalikExpressMVC.Models.XML.OriginDestinationOption>)Session["odSegments"];

                        int BookingTypeID = 0; //odSegments.Count();


                        if (flightType == "RETURN")
                        {
                            BookingTypeID = 1;
                        }

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

                        string ExpiryDateTime = ds.Tables["OrderItem"].Rows[0]["PaymentTimeLimitDateTime"].ToString();

                        string ID = ds.Tables["Order"].Rows[0][1].ToString();
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
                            FlightBookingID = lastBookingId + 1,
                            ExpiryDateTime = ExpiryDateTime,
                            Origin = from,
                            Destination = to,
                            DepartureDate = Convert.ToDateTime(Dep_AircraftScheduledDateTime),
                            ArrivalDate = Convert.ToDateTime(Arr_AircraftScheduledDateTime),
                            Vendor = "NDC",
                            OriginalCost = OriginalCost,
                            CancelPenalty = CancelPenalty,
                            ChangePenalty = ChangePenalty,
                            Penalty = penalty
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
                                TicketNum = ds.Tables["Ticket"].Rows[0]["TicketNumber"].ToString();
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
                            DateTime _age = Convert.ToDateTime(frm?["dobI_" + i.ToString()]?.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(frm?["expiryI" + i.ToString()]);
                            sex = frm?["GenderI_" + i.ToString()];
                            document = frm?["documentI_" + i.ToString()]?.ToString().ToUpper();
                            documenttype = frm?["documenttypeI_" + i.ToString()]?.ToString().ToUpper();

                            if (frm?["expiry_I" + i.ToString()] != null && frm?["expiryI_" + i.ToString()] != "")
                            {
                                expiry = frm?["expiryI_" + i.ToString()];
                            }

                            Age = CalculatePaxAge(_age);

                            nationality = frm?["nationalityI_" + i.ToString()]?.ToString(); // data?.nationality_i?.Value.ToString();

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

                            dob = frm?["dobI_" + i.ToString()]?.ToString();
                            title = frm?["titleI_" + i.ToString()]?.ToString(); // data?.title?.ToUpper();
                            firstName = frm?["firstNameI_" + i.ToString()]?.ToString(); // data?.firstName_i?.Value.ToString().ToUpper();
                            lastName = frm?["lastNameI_" + i.ToString()]?.ToString();  // data?.lastName_i?.Value.ToString().ToUpper();
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
                            DateTime _age = Convert.ToDateTime(frm?["dobC_" + i.ToString()]?.ToString());
                            DateTime dtExpiry = Convert.ToDateTime(frm?["expiryC_" + i.ToString()]);
                            sex = frm?["GenderC_" + i.ToString()];
                            document = frm?["documentC_" + i.ToString()]?.ToString().ToUpper();
                            documenttype = frm?["documenttypeC_" + i.ToString()]?.ToString().ToUpper();

                            if (frm?["expiryC_" + i.ToString()] != null && frm?["expiryC_" + i.ToString()] != "")
                            {
                                expiry = frm?["expiryC_" + i.ToString()];
                            }

                            Age = CalculatePaxAge(_age);

                            nationality = frm?["nationalityC_" + i.ToString()]?.ToString(); // data?.nationality_i?.Value.ToString();

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

                            dob = frm?["dobC_" + i.ToString()]?.ToString();
                            title = frm?["titleC_" + i.ToString()]?.ToString(); // data?.title?.ToUpper();
                            firstName = frm?["firstNameC_" + i.ToString()]?.ToString(); // data?.firstName_i?.Value.ToString().ToUpper();
                            lastName = frm?["lastNameC_" + i.ToString()]?.ToString();  // data?.lastName_i?.Value.ToString().ToUpper();
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

                        if (flightType == "OneWay")
                        {
                            segmenttype = 1;
                        }
                        else
                        {
                            segmenttype = 2;
                        }

                        foreach (DataRow segment in dtFlights.Rows)
                        {
                            string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

                            string AirEquipType = "";
                            if (dtFlights.Columns.Contains("airEquipTypeModel"))
                            {
                                AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
                            }
                            bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
                            DateTime DepDate = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
                            string DepTime = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";
                            DateTime ArrDate = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
                            string ArrTime = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";

                            string MarketingAirlineCode = "PK";// segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
                            string OperatingAirlineCode = "PK"; // segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
                            string FlightNumber = segment["OperatingCarrierFlightNumberText"].ToString(); //FlightNumber.ToString();
                            string CompanyShortName = segment != null && segment["CarrierDesigCode"].ToString() != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
                            string OriginCode = segment["Dep_IATA_LocationCode"].ToString();  //segment["Origin"].ToString();
                            string DestinationCode = segment["Arr_IATA_LocationCode"].ToString(); // segment["Destination"].ToString();
                                                                                                  // string serviceclass = resBookDesigCode.ToString();
                            int cabin = sql.ServiceClasses.Where(x => x.ClassName == segment["CabintypeName"]).Select(x => x.ServiceClassID).FirstOrDefault(); int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                            //int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
                            int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
                            int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
                            int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

                            string ArrGMTOffset = segment != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null ? segment["Arr_AircraftScheduledDateTime"].ToString().Substring(segment["Arr_AircraftScheduledDateTime"].ToString().Length - 6) : "";
                            string DptGMTOffset = segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? segment["Dep_AircraftScheduledDateTime"].ToString().Substring(segment["Dep_AircraftScheduledDateTime"].ToString().Length - 6) : "";

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
                                ResBookDesigCode = segment["RBD_Code"].ToString(),
                                Baggage = "", //segment["weight"] + " " + segment["unitOfMeasureCode"],
                                Way = segmenttype,
                                SegmentType = segment["FlightType"].ToString()
                            };
                            sql.FlightSegments.Add(flightseg);
                            sql.SaveChanges();
                            int FlightSegmentID = flightseg.FlightSegmentID;

                            //segmenttype++;
                        }

                        if (ds.Tables.Contains("Order"))
                        {
                            if (ds.Tables["Order"].Rows.Count > 0)
                            {
                                string jsonData = "";


                                //if (frm?.Option_Id_r != null && frm?.Option_Id_r != "")
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

                string url = "https://app.crane.aero/cranendc/v20.1/CraneNDCService";
                // string url = "https://app-stage.crane.aero/cranendc/v20.1/CraneNDCService";


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
                // request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");
                request.Headers.Add("username", "PSA2746366");
                // request.Headers.Add("password", "Test1234"); //Test password
                request.Headers.Add("password", "Sam7890@#_"); // production password

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

                //result1 = result1.Replace("universal:", "");
                //result1 = result1.Replace("common_v42_0:", "");
                //result1 = result1.Replace("air:", "");

                ////Log

                string fileName = @"C:\hallmark\doOrderCreate_2A1C1I_req.txt";
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

                fileName = @"C:\hallmark\doOrderCreate_2A1C1I_res.txt";
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
                        if (!ds.Tables.Contains("DataLists"))
                        {
                            // ViewBag.Message = "something went wrong."; //ds.Tables["Fault"].Rows[0]["faultstring"].ToString();
                            return "something went wrong";
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

                        return "something went wrong";
                    }

                }
                return "something went wrong";
            }
            catch (Exception ex)
            {
                // throw new Exception;
                return ex.Message.ToString();
            }
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
                                    orderby x.Id descending
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
                                        Vendor = x.Vendor,
                                        OriginalCost = x.OriginalCost,
                                        UserID = x.UserID,
                                        CancelPenalty = x.CancelPenalty,
                                        ChangePenalty = x.ChangePenalty,
                                        Penalty = x.Penalty,
                                        Segments = (from t in sql.FlightSegments
                                                    join originCity in sql.Cities on t.OriginID equals originCity.cityID
                                                    join destinationCity in sql.Cities on t.DestinationID equals destinationCity.cityID
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
                                                        SegmentType = t.SegmentType,
                                                        OriginCityName = originCity.cityName, // Add OriginCityName property
                                                        DestinationCityName = destinationCity.cityName // Add DestinationCityName property
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
                                       Vendor = x.Vendor,
                                       OriginalCost = x.OriginalCost,
                                       ChangePenalty = x.ChangePenalty,
                                       CancelPenalty = x.CancelPenalty,
                                       Penalty = x.Penalty
                                   }).ToList();

                //var fs = sql.FlightSegments.Where(f => f.FlightBookingID == bookingID).Select(f => f.ServiceClassID).FirstOrDefault();


                var flightSegments = (from x in sql.FlightSegments.AsEnumerable()
                                      where x.FlightBookingID == bookingID
                                      join originCity in sql.Cities on x.OriginID equals originCity.cityID
                                      join destinationCity in sql.Cities on x.DestinationID equals destinationCity.cityID
                                      select new FlightSegment
                                      {
                                          ArrivalDate = x.ArrivalDate,
                                          ArrivalTime = x.ArrivalTime,
                                          ArrivalGMTOffset = x.ArrivalGMTOffset,
                                          DepartureDate = x.DepartureDate,
                                          DepartureTime = x.DepartureTime,
                                          DepartureGMTOffset = x.DepartureGMTOffset,
                                          ServiceClassID = x.ServiceClassID ?? 0,
                                          OriginID = x.OriginID ?? 0,
                                          DestinationID = x.DestinationID ?? 0,
                                          FlightNumber = x.FlightNumber,
                                          MarketingAirlineCode = x.MarketingAirlineCode,
                                          OperatingAirlineCode = x.OperatingAirlineCode,
                                          MarketingAirlineID = x.MarketingAirlineID ?? 0,
                                          OperatingAirlineID = x.OperatingAirlineID ?? 0,
                                          OperatingAirlineName = x.OperatingAirlineName ?? "",
                                          FlightBookingID = x.FlightBookingID,
                                          ETicket = x.ETicket ?? false,
                                          ResBookDesigCode = x.ResBookDesigCode ?? "",
                                          Way = x.Way,
                                          Baggage = x.Baggage ?? "",
                                          SegmentType = x.SegmentType,
                                          OriginCityName = originCity.cityName, // Add OriginCityName property
                                          DestinationCityName = destinationCity.cityName // Add DestinationCityName property
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
        private string OrderCancelPreview(string orderID)
        {
            try
            {
                MexCoreWebAPI.Model.EncryptionManager enc = new MexCoreWebAPI.Model.EncryptionManager();
                string de = enc.Decrypt("tm/Qojgg8CqtjTJfUGJpLfvuFPW87JNQqw7gkDHqHmk=");
                string TA = enc.Encrypt("sw86n9&U");

                StringBuilder strReq = new StringBuilder();
                Guid TraceId = Guid.NewGuid();

                strReq.Append(@"<soapenv:Envelope xmlns:soapenv = 'http://schemas.xmlsoap.org/soap/envelope/' xmlns = 'http://www.iata.org/IATA/2015/00/2020.1/IATA_OrderReshopRQ'>");
                strReq.Append(@"<soapenv:Header/>");
                strReq.Append(@"<soapenv:Body>");
                strReq.Append(@"<IATA_OrderReshopRQ>");
                strReq.Append(@"<Party>");
                strReq.Append(@"<Sender>");
                strReq.Append(@"<TravelAgency>");
                strReq.Append(@"<AgencyID>PSA2746366</AgencyID>");
                strReq.Append(@"<Name>NDC GATEWAY</Name>");
                strReq.Append(@"</TravelAgency>");
                strReq.Append(@"</Sender>");
                strReq.Append(@"</Party>");
                strReq.Append(@"<Request>");
                strReq.Append(@"<OrderRefID>" + orderID + "</OrderRefID>");
                strReq.Append(@"<ReshopParameters>");
                strReq.Append(@"<CurParameter>");
                strReq.Append(@"<RequestedCurCode>PKR</RequestedCurCode>");
                strReq.Append(@"</CurParameter>");
                strReq.Append(@"<Device>");
                strReq.Append(@"<DeviceOwnerTypeCode>SL</DeviceOwnerTypeCode>");
                strReq.Append(@"</Device>");
                strReq.Append(@"<LangUsage>");
                strReq.Append(@"<LangCode>EN</LangCode>");
                strReq.Append(@"</LangUsage>");
                strReq.Append(@"</ReshopParameters>");
                strReq.Append(@"<UpdateOrder>");
                strReq.Append(@"<CancelOrder>");
                strReq.Append(@"<OrderRefID>" + orderID + "</OrderRefID>");
                strReq.Append(@"</CancelOrder>");
                strReq.Append(@"</UpdateOrder>");
                strReq.Append(@"</Request>");
                strReq.Append(@"</IATA_OrderReshopRQ>");
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
        private string OrderTicketPreview(string orderID, string price)
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
                strReq.Append(@"<IATA_OrderChangeRQ xsi:schemaLocation= 'xmldsig-core-schema.xsd'  xmlns='http://www.iata.org/IATA/2015/00/2020.1/IATA_OrderChangeRQ' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>");
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
                strReq.Append(@"<PaymentFunctions>");
                strReq.Append(@"<PaymentProcessingDetails>");
                strReq.Append(@"<Amount CurCode='PKR'>" + price + "</Amount>");
                strReq.Append(@"<PaymentMethod>");
                strReq.Append(@"<AccountableDoc>");
                strReq.Append(@"<DocType>MCO</DocType>");
                strReq.Append(@"<TicketID>4000007627</TicketID>");
                strReq.Append(@"</AccountableDoc>");
                strReq.Append(@"</PaymentMethod>");
                strReq.Append(@"<PaymentRefID>PaymentInfo1</PaymentRefID>");
                strReq.Append(@"<TypeCode>MCO</TypeCode>");
                strReq.Append(@"</PaymentProcessingDetails>");
                strReq.Append(@"</PaymentFunctions>");
                strReq.Append(@"</Request>");
                strReq.Append(@"</IATA_OrderChangeRQ>");
                strReq.Append(@"</soapenv:Body>");
                strReq.Append(@"</soapenv:Envelope>");

                return strReq.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        [HttpGet("ticketpreview")]
        public async Task<string> TicketPreview(string orderID, string price)
        {
            try
            {
                MexCoreWebAPI.Model.NDC.Envelope ndcFlights = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string url = "https://app.crane.aero/cranendc/v20.1/CraneNDCService";

                //    string url = "https://app.crane.aero/craneota/CraneOTAService";
                //  string url = "https://app-stage.crane.aero/cranendc/v20.1/CraneNDCService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "cranendc/doOrderChange");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");
                request.Headers.Add("username", "PSA2746366");
                request.Headers.Add("password", "Sam7890@#_");

                var xmlReq = OrderTicketPreview(orderID, price);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

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

                if (ds.Tables.Contains("Ticket"))
                {
                    DataTable dtTicket = ds.Tables["Ticket"].Copy();
                    var FlightBookingID = sql.Reservations.Where(a => a.PNR == orderID).Select(a => a.FlightBookingID).FirstOrDefault();
                    var PassengerIDs = sql.PassengerDetails.Where(b => b.FlightBookingID == FlightBookingID).Select(b => b.PassengerDetailID).ToList();

                    for (int f = 0; f < PassengerIDs.Count; f++)
                    {
                        int? pdID = PassengerIDs[f];
                        // using (var db = new MexDbContext())
                        {
                            var PassengerDetails = sql.PassengerDetails.SingleOrDefault(b => b.FlightBookingID == FlightBookingID && b.PassengerDetailID == pdID);
                            if (PassengerDetails != null)
                            {

                                try
                                {
                                    if (dtTicket.Rows[f] != null)
                                    {
                                        PassengerDetails.TicketNo = dtTicket.Rows[f]["TicketNumber"].ToString();
                                        sql.PassengerDetails.Attach(PassengerDetails);
                                        sql.Entry(PassengerDetails).State = EntityState.Modified;
                                        sql.SaveChanges();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }
                            }
                        }
                    }

                    var reservation = sql.Reservations.Where(x => x.PNR == orderID).FirstOrDefault();
                    if (reservation != null)
                    {
                        // reservation.IsDeleted = true;
                        reservation.BookingStatus = "Ticketed";
                        sql.SaveChanges();
                    }

                    var TicketNumbers = ds.Tables["Ticket"].AsEnumerable()
                        .Select(row => new
                        {
                            TicketNumber = row.Field<string>("TicketNumber"),
                        })
                        .Distinct()
                        .ToList();

                    string jsonString = System.Text.Json.JsonSerializer.Serialize(TicketNumbers);

                    return jsonString;
                }
                return "something went wrong";
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpGet("ticketvoid")]
        public async Task<string> TicketVoid(string orderID)
        {
            string jsonString;
            try
            {
                MexCoreWebAPI.Model.NDC.Envelope ndcFlights = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string url = "https://app.crane.aero/cranendc/v20.1/CraneNDCService";
                // string url = "https://app-stage.crane.aero/cranendc/v20.1/CraneNDCService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "cranendc/doVoidTicket");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");
                request.Headers.Add("username", "PSA2746366");
                request.Headers.Add("password", "Sam7890@#_");

                var xmlReq = OrderTicketVoid(orderID);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

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

                if (ds.Tables.Contains("Error"))
                {
                    jsonString = System.Text.Json.JsonSerializer.Serialize(ds.Tables["Error"].Rows[0]["DescText"].ToString());

                    return jsonString;
                }
                else
                {
                    if ((int)response.StatusCode == 200)
                    {
                        var reservation = sql.Reservations.Where(x => x.PNR == orderID).FirstOrDefault();
                        if (reservation != null)
                        {
                            // reservation.IsDeleted = true;
                            reservation.BookingStatus = "Void Ticket";
                            sql.SaveChanges();
                        }
                    }

                    jsonString = System.Text.Json.JsonSerializer.Serialize("Ticket voided successfully.");

                    return jsonString;
                }

                jsonString = System.Text.Json.JsonSerializer.Serialize("Something went wrong.");
                return jsonString;
            }
            catch (Exception e)
            {
                jsonString = System.Text.Json.JsonSerializer.Serialize("Something went wrong.");
                return jsonString;
            }
        }

        [HttpGet("cancelorderFinal")]
        public async Task<HttpResponseMessage> CancelOrder(string orderID)
        {
            try
            {
                MexCoreWebAPI.Model.NDC.Envelope ndcFlights = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                // string url = "https://app.crane.aero/craneota/CraneOTAService";
                string url = "https://app.crane.aero/cranendc/v20.1/CraneNDCService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("SOAPAction", "cranendc/doOrderCancelCommit");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("KeepAlive", "true");
                request.Headers.Add("username", "PSA2746366");
                request.Headers.Add("password", "Sam7890@#_");

                var xmlReq = OrderCancelReq(orderID);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

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
                return response;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [NonAction]
        private string OrderCancelReq(string orderID)
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
                strReq.Append(@"<ChangeOrder>");
                strReq.Append(@"<CancelOrder>");
                strReq.Append(@"<OrderRefID>" + orderID + "</OrderRefID>");
                strReq.Append(@"</CancelOrder>");
                strReq.Append(@"</ChangeOrder>");
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

        [HttpGet("cancelorder")]
        public async Task<string> CancelOrderPreview(string orderID)
        {
            string jsonString;
            try
            {
                //MexCoreWebAPI.Model.NDC.Envelope ndcFlights = null;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                // string url = "https://app.crane.aero/craneota/CraneOTAService";
                string url = "https://app.crane.aero/cranendc/v20.1/CraneNDCService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("SOAPAction", "cranendc/doOrderCancelPreview");
                request.Headers.Add("KeepAlive", "true");
                request.Headers.Add("username", "PSA2746366");
                request.Headers.Add("password", "Sam7890@#_");

                var xmlReq = OrderCancelPreview(orderID);
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

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

                if ((int)response.StatusCode == 200)
                {

                    var Cresponse = await CancelOrder(orderID);

                    var str3 = Cresponse.Content.Headers;
                    // Get the response
                    var JsonString = await response.Content.ReadAsStringAsync();

                    DataSet ds2 = new DataSet();

                    DataTable FlightSegmentss = null;
                    DataTable dtt;

                    XmlDocument xmll = new XmlDocument();
                    xml.LoadXml(custome‌​rJsonString);
                    XmlReader xmlReaderr = new XmlNodeReader(xmll);
                    ds.ReadXml(xmlReaderr);

                    if (ds.Tables.Contains("Error"))
                    {
                        jsonString = System.Text.Json.JsonSerializer.Serialize(ds.Tables["Error"].Rows[0]["DescText"].ToString());

                        return jsonString;
                    }
                    else
                    {
                        if ((int)Cresponse.StatusCode == 200)
                        {
                            var reservation = sql.Reservations.Where(x => x.PNR == orderID).FirstOrDefault();
                            if (reservation != null)
                            {
                                // reservation.IsDeleted = true;
                                reservation.BookingStatus = "Cancelled";
                                sql.SaveChanges();
                            }
                        }
                        jsonString = System.Text.Json.JsonSerializer.Serialize("Booking successfully cancelled.");
                        return jsonString;
                    }

                }

                jsonString = System.Text.Json.JsonSerializer.Serialize("Something went wrong.");
                return jsonString;
            }
            catch (Exception e)
            {
                jsonString = System.Text.Json.JsonSerializer.Serialize("Something went wrong.");
                return jsonString;
            }
        }

        [HttpGet("penalties")]
        public async Task<string> CancellationCharges(string Origin, string Destination, string FareBasisCode)
        {
            string jsonString;
            try
            {
                MexCoreWebAPI.Model.EncryptionManager enc = new MexCoreWebAPI.Model.EncryptionManager();
                string de = enc.Decrypt("tm/Qojgg8CqtjTJfUGJpLfvuFPW87JNQqw7gkDHqHmk=");
                string TA = enc.Encrypt("sw86n9&U");

                StringBuilder strReq = new StringBuilder();
                Guid TraceId = Guid.NewGuid();

                strReq.Append(@"<soapenv:Envelope xmlns:soapenv = 'http://schemas.xmlsoap.org/soap/envelope/' xmlns = 'http://www.iata.org/IATA/2015/00/2020.1/IATA_OrderRulesRQ'>");
                strReq.Append(@"<soapenv:Header/>");
                strReq.Append(@"<soapenv:Body>");
                strReq.Append(@"<IATA_OrderRulesRQ>");
                strReq.Append(@"<Party>");
                strReq.Append(@"<Sender>");
                strReq.Append(@"<TravelAgency>");
                strReq.Append(@"<AgencyID>PSA2746366</AgencyID>");
                strReq.Append(@"<Name>NDC GATEWAY</Name> ");
                strReq.Append(@"</TravelAgency>");
                strReq.Append(@"</Sender>");
                strReq.Append(@"</Party>");
                strReq.Append(@"<PayloadAttributes>");
                strReq.Append(@"<PrimaryLangID>EN</PrimaryLangID>");
                strReq.Append(@"</PayloadAttributes>");
                strReq.Append(@"<Request>");
                strReq.Append(@"<CoreRequest>");
                strReq.Append(@"<FareRef>");
                strReq.Append(@"<AirlineDesigCode>PK</AirlineDesigCode>");
                strReq.Append(@"<Arrival>");
                strReq.Append(@"<IATA_LocationCode>" + Destination + "</IATA_LocationCode>");
                strReq.Append(@"</Arrival>");
                strReq.Append(@"<Dep>");
                strReq.Append(@"<IATA_LocationCode>" + Origin + "</IATA_LocationCode>");
                strReq.Append(@"</Dep>");
                strReq.Append(@"<FareBasisCode>" + FareBasisCode + "</FareBasisCode>");
                strReq.Append(@"</FareRef>");
                strReq.Append(@"</CoreRequest>");
                strReq.Append(@"<ResponseParameters>");
                strReq.Append(@"<CurParameter>");
                strReq.Append(@"<CurCode>PKR</CurCode>");
                strReq.Append(@"</CurParameter>");
                strReq.Append(@"<Device>");
                strReq.Append(@"<DeviceOwnerTypeCode>SL</DeviceOwnerTypeCode>");
                strReq.Append(@"</Device>");
                strReq.Append(@"<LangUsage>");
                strReq.Append(@"<LangCode>EN</LangCode>");
                strReq.Append(@"</LangUsage>");
                strReq.Append(@"</ResponseParameters>");
                strReq.Append(@"</Request>");
                strReq.Append(@"</IATA_OrderRulesRQ>");
                strReq.Append(@" </soapenv:Body>");
                strReq.Append(@"</soapenv:Envelope>");

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                // string url = "https://app.crane.aero/craneota/CraneOTAService";
                string url = "https://app.crane.aero/cranendc/v20.1/CraneNDCService";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient client = new HttpClient(handler);

                request.Headers.Add("ContentType", "text/xml");
                request.Headers.Add("Method", "Post");
                request.Headers.Add("Host", "app.crane.aero");
                request.Headers.Add("SOAPAction", "cranendc/doOrderFare");
                request.Headers.Add("KeepAlive", "true");
                request.Headers.Add("username", "PSA2746366");
                request.Headers.Add("password", "Sam7890@#_");

                var xmlReq = strReq.ToString();
                var stringContent = new StringContent(xmlReq, Encoding.UTF8, "text/xml");
                request.Content = stringContent;

                var response = await client.SendAsync(request);

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

                if ((int)response.StatusCode == 200)
                {

                    if (ds.Tables.Contains("Error"))
                    {
                        jsonString = System.Text.Json.JsonSerializer.Serialize(ds.Tables["Error"].Rows[0]["DescText"].ToString());

                        return jsonString;
                    }
                    else
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(custome‌​rJsonString);

                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                        nsmgr.AddNamespace("ns18", "http://www.iata.org/IATA/2015/00/2020.1/IATA_OrderRulesRS");

                        XmlNodeList nodeList = doc.SelectNodes("//ns18:FareRuleText/ns18:RemarkText", nsmgr);
                        foreach (XmlNode node in nodeList)
                        {
                            string text = node.InnerText;
                            if (text.Contains("CHANGES/CANCELLATIONS"))
                            {
                                string pattern = @"CHARGE (\w{3}) (\d+\.\d+) FOR NO-SHOW\.";
                                Match match = Regex.Match(text, pattern);

                                if (match.Success)
                                {
                                    string line = match.Value;
                                    string amount = match.Groups[1].Value;
                                    jsonString = System.Text.Json.JsonSerializer.Serialize(line);
                                    return jsonString;

                                }

                            }
                        }


                    }

                }

                jsonString = System.Text.Json.JsonSerializer.Serialize("Something went wrong.");
                return jsonString;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

    }

    public class NDCBooking
    {
        public string? firstName_0 { get; set; }
        public string? lastName_0 { get; set; }
        public string? TransactionId { get; set; }
        public string? Country_0 { get; set; }
        public string? OfferID { get; set; }
        public string? email_0 { get; set; }
        public string? Address_0 { get; set; }
        public string? MobileNO_0 { get; set; }
        public string? LandlineNo { get; set; }
        public string? BookingTitle { get; set; }
        public string? Gender_0 { get; set; }
        public string? Document_0 { get; set; }
        public string? DocumentType_0 { get; set; }
        public string? expiry_0 { get; set; }
        public string? Nationality_0 { get; set; }
        public DateTime? dob_0 { get; set; }
        public string? Title_0 { get; set; }
        public string? Supplier { get; set; }
        public string? Adults { get; set; }
        public string? Children { get; set; }
        public string? Infants { get; set; }
        public string? ShoppingResponseRefID { get; set; }
        public string? BookingID { get; set; }
        public string? Type { get; set; }
        public string? Count { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
        public string? FlightNumber { get; set; }
        public string? Serviceclass0 { get; set; }
        public string? Stops0 { get; set; }
        public string? Total0 { get; set; }
        public string? Amount0 { get; set; }
        public string? Taxes0 { get; set; }
        public string? Currency0 { get; set; }
        public string? Totaltime0 { get; set; }
        public string? Totaltime1 { get; set; }
        public string? Stops1 { get; set; }
        public string? AdultBasePrice { get; set; }
        public string? AdultTotalPrice { get; set; }
        public string? AdultTaxes { get; set; }
        public string? ChildTotalPrice { get; set; }
        public string? ChildBasePrice { get; set; }
        public string? ChildTaxes { get; set; }
        public string? InfantTotalPrice { get; set; }
        public string? InfantTaxes { get; set; }
        public string? AirlineCode { get; set; }
        public string? Cabin { get; set; }
        public string? CurCode { get; set; }
        public string? CarrierDesigCode { get; set; }
        public string? GrandTotal { get; set; }
        public string? Arr_AircraftScheduledDateTime { get; set; }
        public string? Dep_AircraftScheduledDateTime { get; set; }
        public string? ResBookDesigCode { get; set; }
        public string? FlightType { get; set; }
        public string? OriginalCost { get; set; }
        public string? UserID { get; set; }
        public string? Dep_locationCode { get; set; }
        public string? Arr_locationCode { get; set; }

        public string? Penalty { get; set; }
        public string? FareComponentGroupList_Id { get; set; }
        public string? ResBookDesigCode_r { get; set; }
        public string? FareComponentGroupList_Id_r { get; set; }
        public string? Option_Id_r { get; set; }
        public string? Arr_AircraftScheduledDateTime_r { get; set; }
        public string? Dep_locationCode_r { get; set; }
        public string? Dep_AircraftScheduledDateTime_r { get; set; }
        public string? Arr_locationCode_r { get; set; }
        public string? Cabin_r { get; set; }
        public string? fareReferenceID { get; set; }
        public List<NDCFlight>? TotalFlights { get; set; }
        public List<NDCPassenger>? AdultPassengers { get; set; }
        public List<NDCPassenger>? ChildPassengers { get; set; }
        public List<NDCPassenger>? InfantPassengers { get; set; }
    }

    public class NDCFlight
    {
        public string? ShoppingResponseRefID { get; set; }
        public string? FlightType { get; set; }
        public string? Direct { get; set; }
        public string? PaxSegmentRefID_Text { get; set; }
        public string? PaxJourneyRefID { get; set; }
        public string? PaxJourney_Id { get; set; }
        public string? PaxJourneyID { get; set; }
        public string? PaxSegmentID { get; set; }
        public string? PaxSegment_Id { get; set; }
        public string? Duration { get; set; }
        public string? Arr_AircraftScheduledDateTime { get; set; }
        public string? Arr_IATA_LocationCode { get; set; }
        public string? Arr_StationName { get; set; }
        public string? Dep_AircraftScheduledDateTime { get; set; }
        public string? Dep_IATA_LocationCode { get; set; }
        public string? Dep_StationName { get; set; }
        public string? Offer_ID { get; set; }
        public string? OfferID { get; set; }
        public string? OwnerCode { get; set; }
        public string? CarrierOffers_Id { get; set; }
        public string? JourneyOverview_Id { get; set; }
        public string? OfferItem_Id { get; set; }
        public string? MandatoryInd { get; set; }
        public string? OfferItemID { get; set; }
        public string? FareDetail_Id { get; set; }
        public string? FareComponent_Id { get; set; }
        public string? FareBasisCode { get; set; }
        public string? FareTypeCode { get; set; }
        public string? PaxSegmentRefID { get; set; }
        public string? CabinTypeCode { get; set; }
        public string? CabintypeName { get; set; }
        public string? RBD_Code { get; set; }
        public string? FarePriceTypeCode { get; set; }
        public string? Price { get; set; }
        public string? OfferItemPaymentTimeLimit_Id { get; set; }
        public string? fareReferenceID { get; set; }
        public string? PaymentTimeLimitDateTime { get; set; }
        public string? Price_Id { get; set; }
        public string? CurCode { get; set; }
        public string? EquivAmount_Text { get; set; }
        public string? TotalPrice_Id { get; set; }
        public string? Tax { get; set; }
        public string? TotalAmount_Text { get; set; }
        public string? Child_TotalPrice_Id { get; set; }
        public string? Inf_TotalPrice_Id { get; set; }
        public string? CarrierDesigCode { get; set; }
        public string? CarrierName { get; set; }
        public string? OperatingCarrierFlightNumberText { get; set; }
        public string? OriginalCost { get; set; }
        public string? Weight { get; set; }
        public string? UnitOfMeasureCode { get; set; }
        public string? TransactionId { get; set; }
    }

    public class NDCPassenger
    {
        public string? Gender { get; set; }
        public string? Document { get; set; }
        public string? DocumentType { get; set; }
        public string? Expiry { get; set; }
        public string? Nationality { get; set; }
        public string? DOB { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}



//if (flightSegmentIDsForFlightDM != -1 && dmFlag == false)
//{

//    //  if (Convert.ToInt32(segment["PaxSegment_Id"]) == flightSegmentIDsForFlightDM)
//    {

//        dmFlag = true;

//        //  itrCodes++;
//    }
//}
//else if (flightSegmentIDsForFlightDC != -1 && dcFlag == false)
//{
//    //if (Convert.ToInt32(segment["PaxSegment_Id"]) == flightSegmentIDsForFlightDC)
//    {
//        string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

//        string AirEquipType = "";
//        if (dtFlights.Columns.Contains("airEquipTypeModel"))
//        {
//            AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
//        }
//        bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
//        DateTime DepDate = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
//        string DepTime = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";
//        DateTime ArrDate = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
//        string ArrTime = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";

//        string MarketingAirlineCode = "PK";// segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
//        string OperatingAirlineCode = "PK"; // segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
//        string FlightNumber = segment["OperatingCarrierFlightNumberText"].ToString(); //FlightNumber.ToString();
//        string CompanyShortName = segment != null && segment["CarrierDesigCode"].ToString() != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
//        string OriginCode = segment["Dep_IATA_LocationCode"].ToString();  //segment["Origin"].ToString();
//        string DestinationCode = segment["Arr_IATA_LocationCode"].ToString(); // segment["Destination"].ToString();
//                                                                              // string serviceclass = resBookDesigCode.ToString();
//        int cabin = sql.ServiceClasses.Where(x => x.ClassName == segment["CabintypeName"]).Select(x => x.ServiceClassID).FirstOrDefault(); int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
//        // int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
//        int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
//        int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
//        int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

//        string ArrGMTOffset = segment != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null ? segment["Arr_AircraftScheduledDateTime"].ToString().Substring(segment["Arr_AircraftScheduledDateTime"].ToString().Length - 6) : "";
//        string DptGMTOffset = segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? segment["Dep_AircraftScheduledDateTime"].ToString().Substring(segment["Dep_AircraftScheduledDateTime"].ToString().Length - 6) : "";

//        if (!IsDepart)
//        {
//            IsDepart = true;
//            MrktingAirline = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineName).FirstOrDefault();
//            DeptFrom = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityName).FirstOrDefault();
//            ArrvlTo = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityName).FirstOrDefault();

//            DeptDate = DepDate.ToString("MMM dd, ddd");
//            DeptTime = DepTime;

//        }

//        FlightSegment flightseg = new FlightSegment()
//        {
//            ArrivalDate = ArrDate,
//            ArrivalTime = ArrTime,
//            ArrivalGMTOffset = ArrGMTOffset,
//            DepartureDate = DepDate,
//            DepartureTime = DepTime,
//            DepartureGMTOffset = DptGMTOffset,
//            ServiceClassID = cabin,
//            OriginID = Origin,
//            DestinationID = Dest,
//            FlightNumber = FlightNumber,
//            MarketingAirlineCode = MarketingAirlineCode,
//            OperatingAirlineCode = OperatingAirlineCode,
//            MarketingAirlineID = MarketingAirlineID,
//            OperatingAirlineID = OperatingAirlineID,
//            OperatingAirlineName = CompanyShortName,
//            FlightBookingID = lastBookingId + 1,
//            AirEquipType = AirEquipType,
//            ETicket = ETicket,
//            ResBookDesigCode = segment["RBD_Code"].ToString(),
//            Baggage = "", // segment["weight"].ToString() + " " + segment["unitOfMeasureCode"].ToString(),
//            Way = segmenttype,
//            SegmentType = "DC"
//        };
//        sql.FlightSegments.Add(flightseg);
//        sql.SaveChanges();
//        int FlightSegmentID = flightseg.FlightSegmentID;

//        //  dcFlag = true;
//        //  itrCodes++;
//    }
//}
//else if (flightSegmentIDsForFlightRM != -1 && rmFlag == false)
//{
//    if (Convert.ToInt32(segment["PaxSegment_Id"]) == flightSegmentIDsForFlightRM)
//    {
//        string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

//        string AirEquipType = "";
//        if (dtFlights.Columns.Contains("airEquipTypeModel"))
//        {
//            AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
//        }
//        bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
//        DateTime DepDate = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
//        string DepTime = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";
//        DateTime ArrDate = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
//        string ArrTime = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";

//        string MarketingAirlineCode = "PK";// segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
//        string OperatingAirlineCode = "PK"; // segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
//        string FlightNumber = segment["OperatingCarrierFlightNumberText"].ToString(); //FlightNumber.ToString();
//        string CompanyShortName = segment != null && segment["CarrierDesigCode"].ToString() != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
//        string OriginCode = segment["Dep_IATA_LocationCode"].ToString();  //segment["Origin"].ToString();
//        string DestinationCode = segment["Arr_IATA_LocationCode"].ToString(); // segment["Destination"].ToString();
//                                                                              // string serviceclass = resBookDesigCode.ToString();
//        int cabin = sql.ServiceClasses.Where(x => x.ClassName == segment["CabintypeName"]).Select(x => x.ServiceClassID).FirstOrDefault(); int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
//        int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
//        int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
//        int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

//        string ArrGMTOffset = segment != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null ? segment["Arr_AircraftScheduledDateTime"].ToString().Substring(segment["Arr_AircraftScheduledDateTime"].ToString().Length - 6) : "";
//        string DptGMTOffset = segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? segment["Dep_AircraftScheduledDateTime"].ToString().Substring(segment["Dep_AircraftScheduledDateTime"].ToString().Length - 6) : "";

//        if (!IsDepart)
//        {
//            IsDepart = true;
//            MrktingAirline = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineName).FirstOrDefault();
//            DeptFrom = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityName).FirstOrDefault();
//            ArrvlTo = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityName).FirstOrDefault();

//            DeptDate = DepDate.ToString("MMM dd, ddd");
//            DeptTime = DepTime;

//        }


//        FlightSegment flightseg = new FlightSegment()
//        {
//            ArrivalDate = ArrDate,
//            ArrivalTime = ArrTime,
//            ArrivalGMTOffset = ArrGMTOffset,
//            DepartureDate = DepDate,
//            DepartureTime = DepTime,
//            DepartureGMTOffset = DptGMTOffset,
//            ServiceClassID = cabin,
//            OriginID = Origin,
//            DestinationID = Dest,
//            FlightNumber = FlightNumber,
//            MarketingAirlineCode = MarketingAirlineCode,
//            OperatingAirlineCode = OperatingAirlineCode,
//            MarketingAirlineID = MarketingAirlineID,
//            OperatingAirlineID = OperatingAirlineID,
//            OperatingAirlineName = CompanyShortName,
//            FlightBookingID = lastBookingId + 1,
//            AirEquipType = AirEquipType,
//            ETicket = ETicket,
//            ResBookDesigCode = segment["RBD_Code"].ToString(),
//            Baggage = "", //segment["weight"].ToString() + " " + segment["unitOfMeasureCode"].ToString(),
//            Way = segmenttype,
//            SegmentType = "RM"
//        };
//        sql.FlightSegments.Add(flightseg);
//        sql.SaveChanges();
//        int FlightSegmentID = flightseg.FlightSegmentID;
//        rmFlag = true;
//        //  itrCodes++;
//    }
//}
//else if (flightSegmentIDsForFlightRC != -1)
//{
//    if (Convert.ToInt32(segment["PaxSegment_Id"]) == flightSegmentIDsForFlightRC)
//    {
//        // string BookingCode = optionID; // segment["SegmentID"].ToString() != null ? segment["SegmentID"].ToString() : "";

//        string AirEquipType = "";
//        if (dtFlights.Columns.Contains("airEquipTypeModel"))
//        {
//            AirEquipType = segment != null && segment["airEquipTypeModel"].ToString() != null && segment["airEquipTypeModel"].ToString() != null ? segment["airEquipTypeModel"].ToString() : "";
//        }
//        bool ETicket = false; //Convert.ToBoolean(segment != null && segment["TPA_Extensions"].ToString() != null && segment["TPA_Extensions"].ToString() != null ? segment["ETicket"].ToString() : false);
//        DateTime DepDate = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
//        string DepTime = Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";
//        DateTime ArrDate = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]); // segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"].ToString()) : new DateTime();
//        string ArrTime = Convert.ToDateTime(segment["Arr_AircraftScheduledDateTime"]).ToString("HH:mm:ss"); //segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? Convert.ToDateTime(segment["Dep_AircraftScheduledDateTime"]).ToString("hh:mm tt") : "";

//        string MarketingAirlineCode = "PK";// segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
//        string OperatingAirlineCode = "PK"; // segment != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
//        string FlightNumber = segment["OperatingCarrierFlightNumberText"].ToString(); //FlightNumber.ToString();
//        string CompanyShortName = segment != null && segment["CarrierDesigCode"].ToString() != null && segment["CarrierDesigCode"].ToString() != null ? segment["CarrierDesigCode"].ToString() : "";
//        string OriginCode = segment["Dep_IATA_LocationCode"].ToString();  //segment["Origin"].ToString();
//        string DestinationCode = segment["Arr_IATA_LocationCode"].ToString(); // segment["Destination"].ToString();
//                                                                              // string serviceclass = resBookDesigCode.ToString();
//        int cabin = sql.ServiceClasses.Where(x => x.ClassName == segment["CabintypeName"]).Select(x => x.ServiceClassID).FirstOrDefault();

//        int Origin = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityID).FirstOrDefault();
//        int Dest = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityID).FirstOrDefault();
//        int OperatingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == OperatingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();
//        int MarketingAirlineID = 3871; // sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineID).FirstOrDefault();

//        string ArrGMTOffset = segment != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null && segment["Arr_AircraftScheduledDateTime"].ToString() != null ? segment["Arr_AircraftScheduledDateTime"].ToString().Substring(segment["Arr_AircraftScheduledDateTime"].ToString().Length - 6) : "";
//        string DptGMTOffset = segment != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null && segment["Dep_AircraftScheduledDateTime"].ToString() != null ? segment["Dep_AircraftScheduledDateTime"].ToString().Substring(segment["Dep_AircraftScheduledDateTime"].ToString().Length - 6) : "";

//        if (!IsDepart)
//        {
//            IsDepart = true;
//            MrktingAirline = sql.Airlines.Where(x => x.IataCode == MarketingAirlineCode).Select(x => x.AirlineName).FirstOrDefault();
//            DeptFrom = sql.Cities.Where(x => x.IATA == OriginCode).Select(x => x.cityName).FirstOrDefault();
//            ArrvlTo = sql.Cities.Where(x => x.IATA == DestinationCode).Select(x => x.cityName).FirstOrDefault();

//            DeptDate = DepDate.ToString("MMM dd, ddd");
//            DeptTime = DepTime;

//        }

//        FlightSegment flightseg = new FlightSegment()
//        {
//            ArrivalDate = ArrDate,
//            ArrivalTime = ArrTime,
//            ArrivalGMTOffset = ArrGMTOffset,
//            DepartureDate = DepDate,
//            DepartureTime = DepTime,
//            DepartureGMTOffset = DptGMTOffset,
//            ServiceClassID = cabin,
//            OriginID = Origin,
//            DestinationID = Dest,
//            FlightNumber = FlightNumber,
//            MarketingAirlineCode = MarketingAirlineCode,
//            OperatingAirlineCode = OperatingAirlineCode,
//            MarketingAirlineID = MarketingAirlineID,
//            OperatingAirlineID = OperatingAirlineID,
//            OperatingAirlineName = CompanyShortName,
//            FlightBookingID = lastBookingId + 1,
//            AirEquipType = AirEquipType,
//            ETicket = ETicket,
//            ResBookDesigCode = segment["RBD_Code"].ToString(),
//            Baggage = "", //segment["weight"].ToString() + " " + segment["unitOfMeasureCode"].ToString(),
//            Way = segmenttype,
//            SegmentType = "RC"
//        };
//        sql.FlightSegments.Add(flightseg);
//        sql.SaveChanges();
//        int FlightSegmentID = flightseg.FlightSegmentID;

//        rcFlag = true;

//        //itrCodes++;
//    }
//}