using MexCoreWebAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using MexCoreWebAPI.Model;
using Newtonsoft.Json;

namespace MexCoreWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [Canonicalize(ForceHttps = true, RedirectToWwwHost = true, ForceLowerCase = true)]
    public class HomeController : ControllerBase
    {
        private readonly MexDbContext sql;

        public HomeController(MexDbContext context)
        {
            sql = context;
        }
        EncryptionManager enc = new EncryptionManager();


        [HttpPost]
        public async Task<IActionResult> Flight_Search(FormCollection form)
        {

            string HfWay = !string.IsNullOrEmpty(form["hfWay"]) ? form["hfWay"].ToString() : "";

            string strFrom = "", strTo = "", DateFrom = form["StartDate"], DateTo = "", service, type, airline, flexdate = "0", directconecting = "0", multipleshow = "";
            string fromM_1 = "", fromM_2 = "", fromM_3 = "", fromM_4 = "", fromM_5 = "", fromM_6 = "";
            string toM_1 = "", toM_2 = "", toM_3 = "", toM_4 = "", toM_5 = "", toM_6 = "";
            int Adults = 0, Children = 0, Infants = 0;
            // string[] lstDate = DateFrom.Split(',');
            string fromDate = DateFrom; // lstDate[0].ToString();          

            if (HfWay == "2")
            {
                type = "Return";
                service = !string.IsNullOrEmpty(form["service0"]) ? form["service0"].ToString() : "";
                strFrom = !string.IsNullOrEmpty(form["FirstCity"]) ? form["FirstCity"].ToString() : "";
                strTo = !string.IsNullOrEmpty(form["SecondCity"]) ? form["SecondCity"].ToString() : "";
                DateFrom = !string.IsNullOrEmpty(form["StartDate"]) ? form["StartDate"].ToString() : "";
                DateTo = !string.IsNullOrEmpty(form["EndDate"]) ? form["EndDate"].ToString() : "";
                airline = !string.IsNullOrEmpty(form["airline0"]) ? form["airline0"].ToString() : "";
                flexdate = !string.IsNullOrEmpty(form["flexdate0"]) ? form["flexdate0"].ToString() : "";
                directconecting = !string.IsNullOrEmpty(form["directconecting0"]) ? form["directconecting0"].ToString() : "";

                int Adults1 = !string.IsNullOrEmpty(form["adults1"]) ? Convert.ToInt32(form["adults1"].ToString()) : 0;
                int Children1 = !string.IsNullOrEmpty(form["children1"]) ? Convert.ToInt32(form["children1"].ToString()) : 0;
                int Infants1 = !string.IsNullOrEmpty(form["infants1"]) ? Convert.ToInt32(form["infants1"].ToString()) : 0;

                if (Adults1 > 3)
                {
                    Adults = !string.IsNullOrEmpty(form["adults"]) ? Convert.ToInt32(form["adults"].ToString()) : 0;
                }
                else
                    Adults = Adults1 > 0 ? Adults1 : 1;

                if (Children1 > 3)
                {
                    Children = !string.IsNullOrEmpty(form["children"]) ? Convert.ToInt32(form["children"].ToString()) : 0;
                }
                else
                    Children = Children1;
                if (Infants1 > 3)
                {
                    Infants = !string.IsNullOrEmpty(form["infants"]) ? Convert.ToInt32(form["infants"].ToString()) : 0;
                }
                else
                    Infants = Infants1;
            }
            else if (HfWay == "3")
            {
                type = "Other";

                service = !string.IsNullOrEmpty(form["serviceM_1"]) ? form["serviceM_1"].ToString() : "";

                DateFrom = !string.IsNullOrEmpty(form["start"]) ? form["start"].ToString() : "";
                airline = !string.IsNullOrEmpty(form["airline2"]) ? form["airline2"].ToString() : "";
                flexdate = !string.IsNullOrEmpty(form["flexdate2"]) ? form["flexdate2"].ToString() : "";
                directconecting = !string.IsNullOrEmpty(form["directconecting2"]) ? form["directconecting2"].ToString() : "";

                int Adults1 = !string.IsNullOrEmpty(form["adults1M_1"]) ? Convert.ToInt32(form["adults1M_1"].ToString()) : 0;
                int Children1 = !string.IsNullOrEmpty(form["children1M_1"]) ? Convert.ToInt32(form["children1M_1"].ToString()) : 0;
                int Infants1 = !string.IsNullOrEmpty(form["infants1M_1"]) ? Convert.ToInt32(form["infants1M_1"].ToString()) : 0;
                multipleshow = !string.IsNullOrEmpty(form["multipleshow"]) ? form["multipleshow"].ToString() : "2";


                if (Adults1 > 3)
                {
                    Adults = !string.IsNullOrEmpty(form["adultsM_1"]) ? Convert.ToInt32(form["adultsM_1"].ToString()) : 0;
                }
                else
                    Adults = Adults1 > 0 ? Adults1 : 1;

                if (Children1 > 3)
                {
                    Children = !string.IsNullOrEmpty(form["childrenM_1"]) ? Convert.ToInt32(form["childrenM_1"].ToString()) : 0;
                }
                else
                    Children = Children1;
                if (Infants1 > 3)
                {
                    Infants = !string.IsNullOrEmpty(form["infantsM_1"]) ? Convert.ToInt32(form["infantsM_1"].ToString()) : 0;
                }
                else
                    Infants = Infants1;

                fromM_1 = form["fromM_1"];
                fromM_2 = form["fromM_2"];
                fromM_3 = form["fromM_3"];
                fromM_4 = form["fromM_4"];
                fromM_5 = form["fromM_5"];
                fromM_6 = form["fromM_6"];

                toM_1 = form["toM_1"];
                toM_2 = form["toM_2"];
                toM_3 = form["toM_3"];
                toM_4 = form["toM_4"];
                toM_5 = form["toM_5"];
                toM_6 = form["toM_6"];
                return RedirectToAction("FlightResults", "FlightAvail", new { fromM_1 = fromM_1, fromM_2 = fromM_2, fromM_3 = fromM_3, fromM_4 = fromM_4, fromM_5 = fromM_5, fromM_6 = fromM_6, toM_1 = toM_1, toM_2 = toM_2, toM_3 = toM_3, toM_4 = toM_4, toM_5 = toM_5, toM_6 = toM_6, start = DateFrom, adults = Adults, children = Children, infants = Infants, service = service, type = type, airline = airline, flexdate = flexdate, directconecting = directconecting, multipleshow = multipleshow });
            }
            else
            {
                type = "OneWay";
                service = !string.IsNullOrEmpty(form["service0"]) ? form["service0"].ToString() : "";
                strFrom = !string.IsNullOrEmpty(form["FirstCity"]) ? form["FirstCity"].ToString() : "";
                strTo = !string.IsNullOrEmpty(form["SecondCity"]) ? form["SecondCity"].ToString() : "";
                DateFrom = !string.IsNullOrEmpty(form["StartDate"]) ? form["StartDate"].ToString() : "";
                DateTo = !string.IsNullOrEmpty(form["EndDate"]) ? form["EndDate"].ToString() : "";
                airline = !string.IsNullOrEmpty(form["airline0"]) ? form["airline0"].ToString() : "";
                flexdate = !string.IsNullOrEmpty(form["flexdate0"]) ? form["flexdate0"].ToString() : "";
                directconecting = !string.IsNullOrEmpty(form["directconecting0"]) ? form["directconecting0"].ToString() : "";

                int Adults1 = !string.IsNullOrEmpty(form["adults"]) ? Convert.ToInt32(form["adults"].ToString()) : 0;
                int Children1 = !string.IsNullOrEmpty(form["children"]) ? Convert.ToInt32(form["children"].ToString()) : 0;
                int Infants1 = !string.IsNullOrEmpty(form["infants"]) ? Convert.ToInt32(form["infants"].ToString()) : 0;

                if (Adults1 > 3)
                {
                    Adults = !string.IsNullOrEmpty(form["adults0"]) ? Convert.ToInt32(form["adults0"].ToString()) : 0;
                }
                else
                    Adults = Adults1 > 0 ? Adults1 : 1;

                if (Children1 > 3)
                {
                    Children = !string.IsNullOrEmpty(form["children0"]) ? Convert.ToInt32(form["children0"].ToString()) : 0;
                }
                else
                    Children = Children1;
                if (Infants1 > 3)
                {
                    Infants = !string.IsNullOrEmpty(form["infants0"]) ? Convert.ToInt32(form["infants0"].ToString()) : 0;
                }
                else
                    Infants = Infants1;
            }

            return RedirectToAction("FlightResults", "FlightAvail", new { from = strFrom, to = strTo, start = DateFrom, end = DateTo, adults = Adults, children = Children, infants = Infants, service = service, type = type, airline = airline, flexdate = flexdate, directconecting = directconecting });
        }

        
    }
}
public class SearchTypeAheadEntity
{
    public string ShortCode { get; set; }
    public string Name { get; set; }
}

public class LowFareRequest
{
    public int Id {get; set;}
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? MobileNum { get; set; }
    public string? URL { get; set; }
    public decimal LowFare { get; set; }
    public bool Status { get; set; }
    public DateTime EntryTime   {get; set; }
    public bool MobileRequest {get; set; }
    public string TokenId { get; set; }

}