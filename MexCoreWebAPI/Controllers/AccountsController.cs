using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace MexCoreWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]
    public class AccountsController : ControllerBase
    {
        private readonly MexOldDBContext sql;
        private static readonly char[] _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();


        public AccountsController(MexOldDBContext context)
        {
            sql = context;
        }

        //[ValidateAntiForgeryToken]
        [HttpPost("login")]
        public string Login([FromBody] dynamic info)
        {
            try
            {
                string param = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                var frm = JsonConvert.DeserializeObject<dynamic>(param.ToString());

                if (frm != null)
                {
                    string username = !string.IsNullOrEmpty(frm.username.Value) ? frm.username.Value.ToString() : "";
                    string password = !string.IsNullOrEmpty(frm.password.Value) ? frm.password.Value.ToString() : "";

                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                        var user = (from u in sql.Users
                                    where u.UserName == username && u.Password == password
                                    join c in sql.Customers on u.CustomerID equals c.CustomerID
                                    join ca in sql.CustomerAccounts on c.CustomerID equals ca.CustomerID
                                    select new
                                    {
                                        FirstName = u.FirstName ?? "",
                                        LastName = u.LastName ?? "",
                                        UserName = u.UserName ?? "",
                                        Status = u.Status ?? 0,
                                        Password = u.Password ?? "",
                                        DateCreated = u.DateCreated ?? null,
                                        CustomerID = u.CustomerID ?? 0,
                                        UserID = u.UserID,
                                        UserTypeID = u.UserTypeID ?? 0,
                                        ProfilePic = u.ProfilePic ?? "",
                                        ParentID = c.ParentID ?? 0,
                                        CurrencyID = ca.CurrencyID ?? 0,
                                        AccountID = ca.AccountID,
                                        PaymentMode1 = ca.PaymentMode.PaymentMode1 ?? "",
                                        CreditLimit = ca.CreditLimit ?? 0,
                                        GaurantedAmount = ca.GaurantedAmount ?? 0,
                                        CurrencyName = ca.Currency.CurrencyName ?? "",
                                        PayNow = u.PayNow,
                                        PayLater = u.PayLater,
                                        PayLaterMarkup = u.PayLaterMarkup
                                    }).FirstOrDefault();
                        if (user != null && !string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
                        {
                            if (user.Status == 1)
                            {
                                string json = JsonConvert.SerializeObject(user);

                                return json;

                            }
                            else if (user.Status == 0)
                            {
                                return "Your request is pending. Please wait.";

                            }
                            else if (user.Status == 2)
                            {
                                return "Your credentials has been disabled.";

                            }
                            else if (user.Status == 3)
                            {
                                return "An activation link has been sent to your email address.";

                            }
                            else
                            {
                                return "Your account has been disabled.";
                            }
                        }
                        else
                        {
                            return "Invalid email address or password.";
                        }
                    }
                    else
                    {
                        return "Please provide email address and password.";
                    }
                }
                return "something went wrong";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterModel frm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string firstname = frm.FirstName;
                    string lastname = frm.LastName;
                    string contact = frm.Contact;
                    string email = frm.Email;
                    string password = frm.Password;
                    byte usertype = frm.UserType.HasValue ? frm.UserType.Value : (byte)4;
                    string companyname = frm.CompanyName;
                    string regno = frm.RegNo;
                    string address = frm.Address;
                    string licno = frm.LicNo;
                    int? city = frm.City;

                    if (usertype == 4)
                    {
                        companyname = $"{firstname} {lastname} Member";
                    }

                    int checkaccount = sql.Users.Count(x => x.UserName == email || x.Email == email);
                    if (checkaccount == 0)
                    {
                        Customer c = new Customer()
                        {
                            AddressLine1 = address,
                            CityID = city,
                            CompanyName = companyname,
                            DateCreated = DateTime.UtcNow,
                            Email = email,
                            LicenceNumber = licno,
                            RegestrationNumber = regno,
                            Status = 0,
                            Telephone = contact,
                            ParentID = 3
                        };

                        sql.Customers.Add(c);
                        sql.SaveChanges();

                        int CustomerID = c.CustomerID;
                        //string ActivationCode = Accessories.GenerateActivationCode();
                        string ActivationCode = Generate(10); // Generate a random string of length 16


                        User u = new User()
                        {
                            ActivationCode = ActivationCode,
                            AddressLine1 = address,
                            CityID = city,
                            CustomerID = CustomerID,
                            DateCreated = DateTime.UtcNow,
                            Email = email,
                            FirstName = firstname,
                            LastName = lastname,
                            MobileNumber = contact,
                            Password = password,
                            Status = 0,
                            Telephone = contact,
                            UserName = email,
                            UserTypeID = usertype,
                            IsAdmin = true,
                            Gender = true
                        };

                        sql.Users.Add(u);
                        sql.SaveChanges();

                        int userid = u.UserID;
                        if (userid > 0)
                        {
                            return Ok(new { Success = "Congratulation! Your account has been created. We will send you an activation email shortly. Thank you for choosing Malik Express." });
                        }
                    }
                    else
                    {
                        return Conflict(new { Warning = "This User / Email Address already exists." });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Error = ex.Message });
                }
            }

            return BadRequest(new { Error = "Something went wrong." });
        }


        public static string Generate(int length)
        {
            byte[] data = new byte[length];
            _rng.GetBytes(data);
            StringBuilder result = new StringBuilder(length);

            foreach (byte b in data)
            {
                result.Append(_chars[b % _chars.Length]);
            }

            return result.ToString();
        }

        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(string email, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var user = (from u in sql.Users where u.Email == email select u).FirstOrDefault();
                    if (user != null)
                    {
                        string UserName = user.FirstName + " " + user.LastName;
                        user.Password = password;
                        user.Status = 1;
                        sql.Entry(user).State = EntityState.Modified;
                        sql.SaveChanges();

                        return Ok(new { Success = "Password updated successfully." });
                    }
                    else
                    {
                        return NotFound(new { Error = "User not found." });
                    }
                }
                else
                {
                    return BadRequest(new { Error = "Email cannot be empty." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

    }

    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte? UserType { get; set; }
        public string CompanyName { get; set; }
        public string RegNo { get; set; }
        public string Address { get; set; }
        public string LicNo { get; set; }
        public int? City { get; set; }
    }
}
