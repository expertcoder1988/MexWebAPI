using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Text;
using static iTextSharp.text.pdf.PdfDocument;

namespace MexCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]

    public class ContactUsController : ControllerBase
    {
        private readonly MexDbContext sql;

        public ContactUsController(MexDbContext context)
        {
            sql = context;
        }

        [HttpPost("savemessage")]
        public string SaveMessage([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                var data = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                string str = "";
                string fullname = string.IsNullOrEmpty(data?.txt_name?.Value) ? "" : data?.txt_name?.Value;
                string Message = string.IsNullOrEmpty(data?.txt_message?.Value) ? "" : data?.txt_message?.Value;
                string PhoneNum = string.IsNullOrEmpty(data?.txt_phone?.Value) ? "" : data?.txt_phone?.Value;
                string email = string.IsNullOrEmpty(data?.txt_email?.Value) ? "" : data?.txt_email?.Value;

                Prospect prospect= new Prospect();

                prospect.FullName = fullname;
                prospect.Email = email;
                prospect.PhoneNumber = PhoneNum;
                prospect.Message = Message;

                sql.Prospects.Add(prospect);
                sql.SaveChanges();

                StringBuilder sb = new StringBuilder();

                sb.Append("Prospect Details");
                sb.Append("\r\n\r\nFull Name: " + fullname);
                sb.Append("\r\n\r\nEmail: " + email);
                sb.Append("\r\n\r\nPhone Number: " + PhoneNum);
                sb.Append("\r\n\r\nMessage: " + Message);

                SmtpClient smtpClient = new SmtpClient("mail.malikexpress.com");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("sales-isb@malikexpress.com", "797qTz@z4");

                // Create the email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("sales-isb@malikexpress.com");
                mailMessage.To.Add("info@malikexpress.com");
                mailMessage.Subject = "New Query Received from Website.";
                mailMessage.Body = sb.ToString();


                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // Send the email
                smtpClient.Send(mailMessage);

                return str;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }

    public class Prospect
    {
        [Key]
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }
        public DateTime? Created { get; set; }
    }
}
