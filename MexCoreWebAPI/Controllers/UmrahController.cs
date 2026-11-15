using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace MexCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class UmrahController : ControllerBase
    {
        private readonly MexDbContext sql;

        public UmrahController(MexDbContext context)
        {
            sql = context;
        }

        [HttpGet("umrahhotels")]
        public string UmrahHotels(string term)
        {
            //var cityName = (from x in sql.Cities.AsEnumerable()
            //                where x.cityCode == term
            //                select new { Name = x.cityName }).FirstOrDefault();

            try
            {

                var uHotels = (from u in sql.UmrahHotels.AsEnumerable()
                               where u.C_Name == term
                               select (new UmrahHotel
                               {
                                   UH_ID = u.UH_ID,
                                   C_Name = u.C_Name,
                                   U_Hotels = u.U_Hotels,
                                   Cat_Name = u.Cat_Name,
                                   Location = u.Location,
                                   DistanceMeter = u.DistanceMeter,
                                   SharingORQuint = u.SharingORQuint,
                                   Quad = u.Quad,
                                   Triple = u.Triple,
                                   Doublee = u.Doublee,
                                   Images = u.Images
                               })).ToList();


                var json = System.Text.Json.JsonSerializer.Serialize(uHotels);
                return json.ToString();
            }
            catch (Exception)
            {
                return null;
            }



        }



        [HttpPost("umrahinquiry")]
        public string UmrahInquiry([FromBody] UmrahInquriyJson data)
        {
            try
            {

               // string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
               // var data = JsonConvert.DeserializeObject<UmrahInquriyJson>(json.ToString());

                string DepartureAirport = string.IsNullOrEmpty(data?.DepartureAirport) ? "" : data?.DepartureAirport;
                string DepartureDate = string.IsNullOrEmpty(data?.DepartureDate) ? "" : data?.DepartureDate;
                string Adults = string.IsNullOrEmpty(data?.Adults.ToString()) ? "0" : data?.Adults.ToString();
                string Child = string.IsNullOrEmpty(data?.Child.ToString()) ? "0" : data?.Child.ToString();
                string Infant = string.IsNullOrEmpty(data.Infant.ToString()) ? "0" : data?.Infant.ToString();
                string LeadPassengerName = string.IsNullOrEmpty(data?.LeadPassengerName) ? "" : data?.LeadPassengerName;
                string ContactNo = string.IsNullOrEmpty(data?.ContactNo) ? "" : data?.ContactNo;
                string HotelType = string.IsNullOrEmpty(data?.HotelType) ? "" : data?.HotelType;
                string NightsInMakkah = string.IsNullOrEmpty(data?.NightsInMakkah.ToString()) ? "" : data?.NightsInMakkah.ToString();
                string NightsInMadinah = string.IsNullOrEmpty(data?.NightsInMadinah.ToString()) ? "" : data?.NightsInMadinah.ToString();
                string Distance = string.IsNullOrEmpty(data?.Distance) ? "" : data?.Distance;

                MexCoreWebAPI.Model.UmrahInquiry ui = new MexCoreWebAPI.Model.UmrahInquiry();
                ui.DepartureAirport = DepartureAirport;
                ui.DepartureDate = DepartureDate;
                ui.Adults = Convert.ToInt32(Adults);
                ui.Child = Convert.ToInt32(Child);
                ui.Infant = Convert.ToInt32(Infant);
                ui.LeadPassengerName = LeadPassengerName;
                ui.Distance = Distance;
                ui.ContactNo = ContactNo;
                ui.HotelType = HotelType;
                ui.NightsInMakkah = Convert.ToInt32(NightsInMakkah);
                ui.NightsInMadinah = Convert.ToInt32(NightsInMadinah);

                sql.UmrahInquiries.Add(ui);
                sql.SaveChanges();
                int BookingID = ui.Id;

                StringBuilder sb = new StringBuilder();

                sb.Append("<html xmlns='http://www.w3.org/1999/xhtml' lang='' xml: lang=''>");
                sb.Append("<head>");
                sb.Append("<title></title>");
                sb.Append("<meta http - equiv='Content-Type' content='text/html; charset=UTF-8' />");
                sb.Append("<style type='text/css'>");
                sb.Append("p {");
                sb.Append("margin: 0;");
                sb.Append("padding: 0;");
                sb.Append("}");
                sb.Append(".ft10 {");
                sb.Append("font-size: 18px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #6a6a6a;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft11 {");
                sb.Append("font-size: 23px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #535353;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft12 {");
                sb.Append("font-size: 19px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #535353;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft13 {");
                sb.Append("font-size: 14px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #535353;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft14 {");
                sb.Append("font-size: 12px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #6a6a6a;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft15 {");
                sb.Append("font-size: 15px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #535353;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft16 {");
                sb.Append("font-size: 15px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #6a6a6a;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft17 {");
                sb.Append("font-size: 20px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #535353;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft18 {");
                sb.Append("font-size: 86px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #04a2e2;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft19 {");
                sb.Append("font-size: 52px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #6d6e70;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft110 {");
                sb.Append("font-size: 24px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #6d6e70;");
                sb.Append("}");
                sb.Append("");
                sb.Append(".ft111 {");
                sb.Append("font-size: 15px;");
                sb.Append("line-height: 34px;");
                sb.Append("font-family: Times;");
                sb.Append("color: #535353;");
                sb.Append("}");
                sb.Append("</style>");
                sb.Append("</head>");
                sb.Append("<body bgcolor='#fff' vlink='blue' link='blue'>");
                sb.Append("<div id='page1-div'>");

                sb.Append("<table style='border:0;'><tr><td>");
                sb.Append("<p class='ft19'>Malik Air Express</p>");

                sb.Append("<p class='ft17'><b>DepartureAirport: </b>");
                sb.Append("<span class='ft16'>" + DepartureAirport + "</span></p>");
                sb.Append("<p class='ft17'><b>Visit&#160;DepartureDate: </b>");
                sb.Append("<span class='ft16'>" + DepartureDate + "</span></p>");
                sb.Append("<p class='ft17'><b>Adults&#160;Type: </b>");
                sb.Append("<span class='ft16'>" + Adults + "</span></p>");
                sb.Append("<p class='ft17'><b>Children&#160;Expense: </b>");
                sb.Append("<span class='ft16'>" + Child + "</span></p>");
                sb.Append("<p class='ft17'><b>Infants&#160;Type: </b>");
                sb.Append("<span class='ft16'>" + Infant + "</span></p>");
                sb.Append("<p class='ft17'><b>LeadPassengerName&#160;: </b>");
                sb.Append("<span class='ft16'>" + LeadPassengerName + "</span></p>");
                sb.Append("<p class='ft17'><b>Distance&#160;: </b>");
                sb.Append("<span class='ft16'>" + Distance + "</span></p>");
                sb.Append("<p class='ft17'><b>ContactNo&#160;: </b>");
                sb.Append("<span class='ft16'>" + ContactNo + "</span></p>");
                sb.Append("<p class='ft17'><b>HotelType&#160;: </b>");
                sb.Append("<span class='ft16'>" + HotelType + "</span></p>");
                sb.Append("<p class='ft17'><b>NightsInMakkah&#160;: </b>");
                sb.Append("<span class='ft16'>" + NightsInMakkah + "</span></p>");
                sb.Append("<p class='ft17'><b>NightsInMadinah&#160;: </b>");
                sb.Append("<span class='ft16'>" + NightsInMadinah + "</span></p>");
                sb.Append("</td>");
                
                sb.Append("</tr></table>");
              
                sb.Append("</div>");
                sb.Append("</body>");
                sb.Append("</html>");

                // sb.Append("\r\n\r\nPassenger Type: " + passengertype);


                SmtpClient smtpClient = new SmtpClient("mail.malikexpress.com");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("csi@malikexpress.com", "sF~359sm9");

                // Create the email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("csi@malikexpress.com");
                //mailMessage.To.Add("ti@malikexpress.com"); // coo@growbiztech.com" ti@malikexpress.com
                //mailMessage.To.Add("sales-isb@malikexpress.com"); 
                mailMessage.To.Add("umrah@malikexpress.com");
                //mailMessage.To.Add("asimshahzad78@gmail.com");
                mailMessage.Subject = "New Umrah Inquiry.";
                mailMessage.Body = sb.ToString();
                mailMessage.IsBodyHtml = true;

     

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // Send the email
                smtpClient.Send(mailMessage);

                var json1 = System.Text.Json.JsonSerializer.Serialize(BookingID);
                return json1.ToString();
            }
            catch (Exception ex)
            {
                var json1 = System.Text.Json.JsonSerializer.Serialize(ex.Message);
                return json1.ToString();
            }
        }
    }


    public class UmrahInquriyJson
    {
        public string? DepartureAirport { get; set; }
        public string? DepartureDate { get; set; }
        public int? Adults { get; set; }
        public int? Child { get; set; }
        public int? Infant { get; set; }
        public string? LeadPassengerName { get; set; }
        public string? Distance { get; set; }
        public string? ContactNo { get; set; }
        public string? HotelType { get; set; }
        public int? NightsInMakkah { get; set; }
        public int? NightsInMadinah { get; set; }
    }
}
