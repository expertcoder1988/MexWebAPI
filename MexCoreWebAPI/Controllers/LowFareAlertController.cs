using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Cors;

namespace MexCoreWebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]
    public class LowFareAlertController : Controller
    {
     
        [HttpGet("lowafarealertgen")]
        public async Task<string> LowaFareAlertGen(string Email)
        {
            try
            {
                //string connectionString = "data source=DESKTOP-TM6T7R5;initial catalog=MexDB;integrated security=True;MultipleActiveResultSets=True;Encrypt=False;";
               string connectionString = "data source=103.152.255.44;initial catalog=mexdb;user id=sa;password=abc@123;MultipleActiveResultSets=True;Encrypt=False;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    //Console.WriteLine("Connected to SQL Server successfully.");

                    // Perform your database operations here

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string query = "SELECT * FROM LowfareRequests where status = 1 and MobileRequest=1 and Email = '" + Email + "';";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string apiUrl = reader[1].ToString();
                                    decimal LowFare = decimal.Parse(reader[3].ToString());

                                    using (HttpClient client = new HttpClient())
                                    {
                                        // Send an HTTP GET request to the API
                                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                                        // Check the response status
                                        if (response.IsSuccessStatusCode)
                                        {
                                            // Read the response content
                                            string content = await response.Content.ReadAsStringAsync();

                                            List<LowFareAlerts.Flights> flights = JsonConvert.DeserializeObject<List<LowFareAlerts.Flights>>(content);

                                            decimal? smallestPrice = LowFare; // decimal.MaxValue;
                                            foreach (LowFareAlerts.Flights flight in flights)
                                            {
                                                if (flight.GrandTotal < smallestPrice)
                                                {
                                                    smallestPrice = flight.GrandTotal;
                                                }
                                            }

                                            if (smallestPrice < LowFare)
                                            {
                                                string[] arr = apiUrl.Split('&');

                                                string[] _origin = arr[1].Split('=');
                                                string[] _destination = arr[2].Split('=');
                                                string[] _from = arr[6].Split('=');
                                                string[] _to = arr[7].Split('=');

                                                string origin = _origin[1].ToString();
                                                string destination = _destination[1].ToString();
                                                string from = _from[1].ToString();
                                                string to = _to[1].ToString();

                                                query = "SELECT CityName FROM cities where IATA = '" + origin + "'";
                                                using (SqlCommand command1 = new SqlCommand(query, connection))
                                                {
                                                    using (SqlDataReader reader1 = command1.ExecuteReader())
                                                    {
                                                        while (reader1.Read())
                                                        {
                                                            origin = reader1[0].ToString();
                                                        }
                                                    }
                                                }

                                                query = "SELECT CityName FROM cities where IATA = '" + destination + "'";
                                                using (SqlCommand command2 = new SqlCommand(query, connection))
                                                {
                                                    using (SqlDataReader reader2 = command2.ExecuteReader())
                                                    {
                                                        while (reader2.Read())
                                                        {
                                                            destination = reader2[0].ToString();
                                                        }
                                                    }
                                                }


                                                StringBuilder sb = new StringBuilder();

                                                sb.Append("Dear " + reader[6].ToString());

                                                sb.Append("\r\n\r\nI hope this email finds you well. We are thrilled to bring you some exciting news! " +
                                                    "The flight fares for the route you are interested in have gone down significantly, making it the " +
                                                    "perfect time to book your next adventure!");

                                                sb.Append("\r\n\r\nDeparture: " + origin);
                                                sb.Append("\r\n\r\nArrival: " + destination);
                                                sb.Append("\r\n\r\nDeparture Date: " + from);
                                                sb.Append("\r\n\r\nArrival: " + to);

                                                sb.Append("\r\n\r\nAs a valued prospect, we wanted to ensure you're among the first to know about this " +
                                                    "incredible opportunity to save on your upcoming travel. Our team has been diligently monitoring the " +
                                                    "fares, and we are delighted to announce that you can now avail of discounted rates on the flights you " +
                                                    "were considering.\r\n\r\nWhether you're planning a business trip, a relaxing vacation, or visiting your " +
                                                    "loved ones, this price drop allows you to experience the world without breaking the bank.\r\n\r\nIf you " +
                                                    "have any questions or require further information, please don't hesitate to reach out to us. " +
                                                    "Our dedicated team is here to provide you with exceptional service and support throughout your journey." +
                                                    "\r\n\r\nThank you for considering us for your travel plans.We look forward to welcoming you on board soon!" +
                                                    "\r\n\r\nBest regards, ");


                                                //  Console.WriteLine("API response: " + smallestPrice);
                                                SmtpClient smtpClient = new SmtpClient("mail.malikexpress.com");
                                                smtpClient.Port = 587;
                                                smtpClient.EnableSsl = true;
                                                smtpClient.Credentials = new NetworkCredential("sales-isb@malikexpress.com", "797qTz@z4");

                                                // Create the email message
                                                MailMessage mailMessage = new MailMessage();
                                                mailMessage.From = new MailAddress("sales-isb@malikexpress.com");
                                                mailMessage.To.Add(reader[2].ToString());
                                                mailMessage.Subject = "Alert: Low rates are available";
                                                mailMessage.Body = sb.ToString();


                                                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                                                // Send the email
                                                smtpClient.Send(mailMessage);

                                                query = "update LowfareRequests set status=0 where id = " + reader[0];
                                                SqlCommand command3 = new SqlCommand(query, connection);
                                                command3.ExecuteNonQuery();

                                                var jsonObject = new
                                                {
                                                    Departure = origin,
                                                    Arrival = destination,
                                                    DepartureDate = from,
                                                    ArrivalDate = to,
                                                    NewFare = smallestPrice,
                                                    flag = true
                                                };

                                                // Serialize the JSON object to a JSON string
                                                string jsonString = JsonConvert.SerializeObject(jsonObject);

                                                return jsonString;
                                            }
                                            else
                                            {
                                                return "rates are still high";
                                            }


                                        }
                                        else
                                        {
                                            return "API request failed with status code: " + response.StatusCode;
                                        }
                                    }
                                }
                            }
                        }
                    }


                    connection.Close();
                }

                return "no record found";

            }
            catch (Exception ex)
            {
                return "An error occurred while calling the API: " + ex.Message.ToString();
            }
        }
    }

}

namespace LowFareAlerts
{

    public class Rootobject
    {
        public Flights[] Property1 { get; set; }
    }

    public class Flights
    {
        public string? SegmentID { get; set; }
        public string? Availability_Id { get; set; }
        public string? availabilityResultList_Id { get; set; }
        public string? availabilityRouteList_Id { get; set; }
        public string? flexibleFaresOnly { get; set; }
        public DateTime dateList { get; set; }
        public string? availabilityByDateList_Id { get; set; }
        public string? originDestinationOptionList_Id { get; set; }
        public string? fareComponentGroupList_Id { get; set; }
        public string? boundList_Id { get; set; }
        public string? boundCode { get; set; }
        public string? availFlightSegmentList_Id { get; set; }
        public string? addOnSegment { get; set; }
        public string? cabin { get; set; }
        public string? resBookDesigCode { get; set; }
        public string? resBookDesigQuantity { get; set; }
        public string? flightSegment_Id { get; set; }
        public DateTime? departureDateTime { get; set; }
        public DateTime? departureDateTimeUTC { get; set; }
        public DateTime? arrivalDateTime { get; set; }
        public DateTime? arrivalDateTimeUTC { get; set; }
        public string? flightNumber { get; set; }
        public string? ondControlled { get; set; }
        public string? sector { get; set; }
        public string? codeshare { get; set; }
        public string? distance { get; set; }
        public string? flownMileageQty { get; set; }
        public string? journeyDuration { get; set; }
        public string? onTimeRate { get; set; }
        public string? secureFlightDataRequired { get; set; }
        public string? stopQuantity { get; set; }
        public string? code { get; set; }
        public string? companyFullName { get; set; }
        public string? companyShortName { get; set; }
        public string? departureAirport_Id { get; set; }
        public string? dep_codeContext { get; set; }
        public string? dep_language { get; set; }
        public string? dep_locationCode { get; set; }
        public string? dep_locationName { get; set; }
        public string? dep_terminal { get; set; }
        public string? arrivalAirport_Id { get; set; }
        public string? codeContext { get; set; }
        public string? language { get; set; }
        public string? arr_locationCode { get; set; }
        public string? locationName { get; set; }
        public string? terminal { get; set; }
        public string? airEquipType { get; set; }
        public string? airEquipTypeModel { get; set; }
        public string? changeofGauge { get; set; }
        public decimal? AdultTotalPrice { get; set; }
        public decimal? AdultBasePrice { get; set; }
        public decimal? AdultTaxes { get; set; }
        public decimal? ChildTotalPrice { get; set; }
        public decimal? ChildBasePrice { get; set; }
        public decimal? InfantTotalPrice { get; set; }
        public decimal? InfantBasePrice { get; set; }
        public decimal? InfantTaxes { get; set; }
        public decimal? ChildTaxes { get; set; }
        public decimal? GrandTotal { get; set; }

        public int? Option_Id { get; set; }
        public string TotalDuration { get; set; }
    }
}
