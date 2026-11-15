using LowFareAlerts;
using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.WebPages;
using System.ComponentModel;
using MexCoreWebAPI.Model.CIS;
using iText.Bouncycastleconnector;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using static iTextSharp.text.pdf.PdfDocument;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MexCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Insurance : ControllerBase
    {
        private readonly MexDbContext sql;


        public Insurance(MexDbContext context)
        {
            sql = context;

        }

        [HttpPost("selectpackage")]
        public string SelectPackage([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                var data = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                string Insurancetype = string.IsNullOrEmpty(data?.Insurancetype?.Value) ? "" : data?.Insurancetype?.Value;
                string Country1 = string.IsNullOrEmpty(data?.Country1?.Value) ? "" : data?.Country1?.Value;
                string Country2 = string.IsNullOrEmpty(data?.Country2?.Value) ? "" : data?.Country2?.Value;
                string DepartureDate = string.IsNullOrEmpty(data?.DepartureDate?.Value) ? "" : data?.DepartureDate?.Value;
                string Duration = string.IsNullOrEmpty(data?.Duration?.Value) ? "" : data?.Duration?.Value;
                string Passengertype = string.IsNullOrEmpty(data?.Passengertype?.Value) ? "" : data?.Passengertype?.Value;
                string DOB = string.IsNullOrEmpty(data?.Dob?.Value) ? "" : data?.Dob?.Value;
                string MaxStay = string.IsNullOrEmpty(data?.MaxStay?.Value) ? "" : data?.MaxStay?.Value;

                DateTime depDate = Convert.ToDateTime(DepartureDate);
                //DateTime arrDate = Convert.ToDateTime(ArrivalDate);

                //    TimeSpan timeDifference = arrDate - depDate;

                // Parse the date of birth string to DateTime
                DateTime dobDate;
                int age = 0;
                if (DateTime.TryParseExact(DOB, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dobDate))
                {
                    // Calculate the age
                    age = DateTime.Today.Year - dobDate.Year;
                    // Check if the birthday hasn't occurred yet this year, subtract one year from age
                    if (dobDate > DateTime.Today.AddYears(-age))
                        age--;
                }

                // Extract the total number of days from the TimeSpan
                // int numberOfDays = timeDifference.Days;

                if (Passengertype == "student")
                {
                    Insurancetype = "4";
                }

                string UICCountryCode = "0";

                if (Insurancetype == "1")
                {
                    UICCountryCode = "5";
                }
                else if (Insurancetype == "2")
                {
                    UICCountryCode = "6";
                }
                else if (Insurancetype == "3")
                {
                    UICCountryCode = "7";
                }
                else if (Insurancetype == "4")
                {
                    UICCountryCode = "8";
                }

                var insuranceCSI = sql.TravelInsurances.AsEnumerable()
                                .Where(ti => ti.CountryCode == Insurancetype && ti.MaxStay == MaxStay && ti.Duration == Duration && ti.SingleFee != 0 && ti.Vendor == "CSI")
                                .OrderBy(ti => Convert.ToInt32(ti.Duration))
                                .Take(2);

                var insuranceUIC = sql.TravelInsurances.AsEnumerable()
                              .Where(ti => ti.CountryCode == UICCountryCode && ti.MaxStay == MaxStay && ti.Duration == Duration && ti.SingleFee != 0 && ti.Vendor == "UIC")
                              .OrderBy(ti => Convert.ToInt32(ti.Duration))
                              .Take(2);

                var insurance = insuranceCSI.Concat(insuranceUIC)
                               .OrderBy(ti => Convert.ToString(ti.Duration))
                               .ToList();

                foreach (var item in insurance)
                {
                    // Modify SingleFee
                    if (age >= 66 && age <= 70)
                        item.SingleFee = (decimal)(item.SingleFee * 1.5m); // Increase by 50%
                    else if (age >= 71 && age <= 80)
                        item.SingleFee = (decimal)(item.SingleFee * 1.75m); // Increase by 75%
                    else if (age >= 81 && age <= 85)
                        item.SingleFee = (decimal)(item.SingleFee * 2m); // Increase by 100%

                    // Modify FamilyFee
                    if (age >= 66 && age <= 70)
                        item.FamilyFee = (decimal)(item.FamilyFee * 1.5m); // Increase by 50%
                    else if (age >= 71 && age <= 80)
                        item.FamilyFee = (decimal)(item.FamilyFee * 1.75m); // Increase by 75%
                    else if (age >= 81 && age <= 85)
                        item.FamilyFee = (decimal)(item.FamilyFee * 2m); // Increase by 100%
                }

                // Projecting after modifying fees to include discounted fees
                //var insuranceWithDiscounts = insurance.Select(item => new {
                //    item,
                //    DiscountedSingleFee = item.SingleFee * 0.85m, // 15% discount
                //    DiscountedFamilyFee = item.FamilyFee * 0.85m // 15% discount
                //});

                var JSONString = JsonConvert.SerializeObject(insurance);
                return JSONString;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpPost("insurancebenefits")]
        public string InsuranceBenefits([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                var data = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                string CountryCode = string.IsNullOrEmpty(data?.CountryCode?.Value) ? "" : data?.CountryCode?.Value;
                string InsuranceType = string.IsNullOrEmpty(data?.InsuranceType?.Value) ? "" : data?.InsuranceType?.Value;

                int cCode = -1;

                if (CountryCode != null & CountryCode != "")
                {
                    cCode = Convert.ToInt32(CountryCode);
                }

                var insurance = (from ti in sql.InsuranceBenefits.AsEnumerable()
                                 where ti.CountryCode == cCode && ti.InsuranceType == InsuranceType
                                 select new InsuranceBenefit
                                 {
                                     Currency = ti.Currency,
                                     Benefit = ti.Benefit,
                                     Coverage = ti.Coverage
                                 }).ToList();

                var JSONString = JsonConvert.SerializeObject(insurance);
                return JSONString;
            }
            catch (Exception ex)
            {
                return "something went wrong";
            }
        }

        [HttpPost("insurancebooking")]
        public string InsuranceBooking([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind=", "");
                var data = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                string firstName = string.IsNullOrEmpty(data?.First_name?.Value) ? "" : data?.First_name?.Value;
                string lastName = string.IsNullOrEmpty(data?.Last_name?.Value) ? "" : data?.Last_name?.Value;
                string Cnic = string.IsNullOrEmpty(data?.Cnic?.Value) ? "" : data?.Cnic?.Value;
                string Cnic_date = string.IsNullOrEmpty(data?.Cnic_date?.Value) ? "" : data?.Cnic_date?.Value;
                string Dob = string.IsNullOrEmpty(data?.Dob?.Value) ? "" : data?.Dob?.Value;
                string Mobile_no = string.IsNullOrEmpty(data?.Mobile_no?.Value) ? "" : data?.Mobile_no?.Value;
                string Email = string.IsNullOrEmpty(data?.Email?.Value) ? "" : data?.Email?.Value;
                string Passport_no = string.IsNullOrEmpty(data?.Passport_no?.Value) ? "" : data?.Passport_no?.Value;
                string Purpose_Visit = string.IsNullOrEmpty(data?.Purpose_Visit?.Value) ? "" : data?.Purpose_Visit?.Value;
                string Address = string.IsNullOrEmpty(data?.Address?.Value) ? "" : data?.Address?.Value;
                string City = string.IsNullOrEmpty(data?.City?.Value) ? "" : data?.City?.Value;
                string First_nameB = string.IsNullOrEmpty(data?.First_nameB?.Value) ? "" : data?.First_nameB?.Value;
                string Last_nameB = string.IsNullOrEmpty(data?.Last_nameB?.Value) ? "" : data?.Last_nameB?.Value;
                string RelationB = string.IsNullOrEmpty(data?.RelationB?.Value) ? "" : data?.RelationB?.Value;
                string Mobile_noB = string.IsNullOrEmpty(data?.Mobile_noB?.Value) ? "" : data?.Mobile_noB?.Value;
                string CnicB = string.IsNullOrEmpty(data?.CnicB?.Value) ? "" : data?.CnicB?.Value;
                string AddressB = string.IsNullOrEmpty(data?.AddressB?.Value) ? "" : data?.AddressB?.Value;
                string passengertype = string.IsNullOrEmpty(data?.Passengertype?.Value) ? "" : data?.Passengertype?.Value;
                string InsuranceId = string.IsNullOrEmpty(data?.InsuranceId?.Value) ? "0" : data?.InsuranceId?.Value;
                string DepartureDate = string.IsNullOrEmpty(data?.DepartureDate?.Value) ? "" : data?.DepartureDate?.Value;
                string ArrivalDate = string.IsNullOrEmpty(data?.ArrivalDate?.Value) ? "" : data?.ArrivalDate?.Value;
                string Country1 = string.IsNullOrEmpty(data?.Country1?.Value) ? "" : data?.Country1?.Value;
                string Country2 = string.IsNullOrEmpty(data?.Country2?.Value) ? "" : data?.Country2?.Value;
                string Discount_Price = string.IsNullOrEmpty(data?.Discount_Price?.Value) ? "0" : data?.Discount_Price?.Value;
                string Original_Price = string.IsNullOrEmpty(data?.Original_Price?.Value) ? "0" : data?.Original_Price?.Value;
                string Vendor = string.IsNullOrEmpty(data?.Vendor?.Value) ? "" : data?.Vendor?.Value;
                string AreaShortCode = string.IsNullOrEmpty(data?.AreaShortCode?.Value) ? "" : data?.AreaShortCode?.Value;
                string NoOfDays = string.IsNullOrEmpty(data?.NoOfDays?.Value) ? "" : data?.NoOfDays?.Value;
                string Duration = string.IsNullOrEmpty(data?.Duration?.Value) ? "0" : data?.Duration?.Value;

                string userId = string.IsNullOrEmpty(data?.userId?.Value) ? "1" : data?.userId?.Value;


                int insuranceId = 0;

                if (InsuranceId != "")
                {
                    insuranceId = Convert.ToInt32(InsuranceId);
                }

                MexCoreWebAPI.Model.CIS.InsuranceBooking ib = new MexCoreWebAPI.Model.CIS.InsuranceBooking();
                ib.FirstName = firstName;
                ib.LastName = lastName;
                ib.CNIC = Cnic;
                ib.CNICIssuanceDate = Cnic_date;
                ib.DOB = Dob;
                ib.ContactNumber = Mobile_no;
                ib.EmailAddress = Email;
                ib.PassportNumber = Passport_no;
                ib.PurposeOfVisit = Purpose_Visit;
                ib.ResidentialAddress = Address;
                ib.CityofResidence = City;
                ib.BeneficiaryFirstName = First_nameB;
                ib.BeneficiaryLastName = Last_nameB;
                ib.BeneficiaryRelation = RelationB;
                ib.BeneficiaryContactNumber = Mobile_noB;
                ib.BeneficiaryCNIC = CnicB;
                ib.BeneficiaryAddress = AddressB;
                ib.PassengerType = passengertype;
                ib.DepartureDate = DepartureDate;
                ib.ArrivalDate = ArrivalDate;
                ib.FirstCountry = Country1;
                ib.SecondCountry = Country2;
                ib.InsuranceCode = insuranceId;
                ib.Status = true;
                ib.OriginalPrice = Original_Price;
                ib.DiscountedPrice = Discount_Price;
                ib.EntryDateTime = DateTime.Now;
                ib.PolicyNumber = "";
                ib.Vendor = Vendor;
                ib.AreaShortCode = AreaShortCode;
                ib.NoOfDays = NoOfDays;
                ib.userId = Convert.ToInt32(userId);

                sql.InsuranceBookings.Add(ib);
                sql.SaveChanges();
                int BookingID = ib.Id;

                if (passengertype == "family")
                {
                    string FullNameA_1 = string.IsNullOrEmpty(data?.FullNameA_1?.Value) ? "" : data?.FullNameA_1?.Value;
                    string DobA_1 = string.IsNullOrEmpty(data?.DobA_1?.Value) ? "" : data?.DobA_1?.Value;
                    string PassportA_1 = string.IsNullOrEmpty(data?.PassportA_1?.Value) ? "" : data?.PassportA_1?.Value;

                    if (FullNameA_1 != null && FullNameA_1 != "")
                    {
                        MexCoreWebAPI.Model.CIS.InsurancePax ip = new InsurancePax();

                        ip.FullName = FullNameA_1;
                        ip.DOB = DobA_1;
                        ip.PassportNo = PassportA_1;
                        ip.Type = "Adult";
                        ip.InsuranceBookingId = BookingID;
                        ip.Vendor = "CSI";

                        sql.InsurancePaxs.Add(ip);
                        sql.SaveChanges();
                    }

                    string PassportC_1 = string.IsNullOrEmpty(data?.PassportC_1?.Value) ? "" : data?.PassportC_1?.Value;
                    string FullnameC_1 = string.IsNullOrEmpty(data?.FullnameC_1?.Value) ? "" : data?.FullnameC_1?.Value;
                    string DobC_1 = string.IsNullOrEmpty(data?.DobC_1?.Value) ? "" : data?.DobC_1?.Value;

                    if (FullnameC_1 != null && FullnameC_1 != "")
                    {
                        MexCoreWebAPI.Model.CIS.InsurancePax ip = new InsurancePax();

                        ip.FullName = FullnameC_1;
                        ip.DOB = DobC_1;
                        ip.PassportNo = PassportC_1;
                        ip.Type = "Child";
                        ip.InsuranceBookingId = BookingID;
                        ip.Vendor = "CSI";

                        sql.InsurancePaxs.Add(ip);
                        sql.SaveChanges();
                    }

                    string PassportC_2 = string.IsNullOrEmpty(data?.PassportC_2?.Value) ? "" : data?.PassportC_2?.Value;
                    string FullnameC_2 = string.IsNullOrEmpty(data?.FullnameC_2?.Value) ? "" : data?.FullnameC_2?.Value;
                    string DobC_2 = string.IsNullOrEmpty(data?.DobC_2?.Value) ? "" : data?.DobC_2?.Value;

                    if (FullnameC_2 != null && FullnameC_2 != "")
                    {
                        MexCoreWebAPI.Model.CIS.InsurancePax ip = new InsurancePax();

                        ip.FullName = FullnameC_2;
                        ip.DOB = DobC_2;
                        ip.PassportNo = PassportC_2;
                        ip.Type = "Child";
                        ip.InsuranceBookingId = BookingID;
                        ip.Vendor = "CSI";

                        sql.InsurancePaxs.Add(ip);
                        sql.SaveChanges();
                    }

                    string FullnameC_3 = string.IsNullOrEmpty(data?.FullnameC_3?.Value) ? "" : data?.FullnameC_3?.Value;
                    string PassportC_3 = string.IsNullOrEmpty(data?.PassportC_3?.Value) ? "" : data?.PassportC_3?.Value;
                    string DobC_3 = string.IsNullOrEmpty(data?.DobC_3?.Value) ? "" : data?.DobC_3?.Value;

                    if (FullnameC_3 != null && FullnameC_3 != "")
                    {
                        MexCoreWebAPI.Model.CIS.InsurancePax ip = new InsurancePax();

                        ip.FullName = FullnameC_3;
                        ip.DOB = DobC_3;
                        ip.PassportNo = PassportC_3;
                        ip.Type = "Child";
                        ip.InsuranceBookingId = BookingID;
                        ip.Vendor = "CSI";

                        sql.InsurancePaxs.Add(ip);
                        sql.SaveChanges();
                    }

                    string FullnameC_4 = string.IsNullOrEmpty(data?.FullnameC_4?.Value) ? "" : data?.FullnameC_4?.Value;
                    string PassportC_4 = string.IsNullOrEmpty(data?.PassportC_4?.Value) ? "" : data?.PassportC_4?.Value;
                    string DobC_4 = string.IsNullOrEmpty(data?.DobC_4?.Value) ? "" : data?.DobC_4?.Value;

                    if (FullnameC_4 != null && FullnameC_4 != "")
                    {
                        MexCoreWebAPI.Model.CIS.InsurancePax ip = new InsurancePax();

                        ip.FullName = FullnameC_4;
                        ip.DOB = DobC_4;
                        ip.PassportNo = PassportC_4;
                        ip.Type = "Child";
                        ip.InsuranceBookingId = BookingID;
                        ip.Vendor = "CSI";

                        sql.InsurancePaxs.Add(ip);
                        sql.SaveChanges();
                    }
                }

                var ins = (from x in sql.InsuranceBookings
                           join b in sql.TravelInsurances
                             on x.InsuranceCode equals b.Id into bookings
                           from r in bookings.DefaultIfEmpty()
                           where x.Id == BookingID
                           orderby x.EntryDateTime descending
                           select new InsuranceBookings
                           {
                               Id = x.Id,
                               InsuranceCode = x.InsuranceCode,
                               FirstName = x.FirstName,
                               LastName = x.LastName,
                               ContactNumber = x.ContactNumber,
                               CNIC = x.CNIC,
                               BeneficiaryFirstName = x.BeneficiaryFirstName,
                               BeneficiaryLastName = x.BeneficiaryLastName,
                               BeneficiaryContactNumber = x.BeneficiaryContactNumber,
                               PackageName = r.CountryDescrtiption,
                               InsuranceType = r.InsuranceType,
                               MedicalExpense = r.MedicalExpense,
                               PassengerType = x.PassengerType,
                               Passport = x.PassportNumber,
                               DepartureDate = x.DepartureDate,
                               ArrivalDate = x.ArrivalDate,
                               EmailAddress = x.EmailAddress,
                               BeneficiaryCNIC = x.BeneficiaryCNIC,
                               BeneficiaryAddress = x.BeneficiaryAddress,
                               BeneficiaryRelation = x.BeneficiaryRelation,
                               DOB = x.DOB,
                               PurposeOfVisit = x.PurposeOfVisit,
                               DiscountedPrice = x.DiscountedPrice,
                               ResidentialAddress = x.ResidentialAddress,
                               FirstCountry = x.FirstCountry,
                               CityofResidence = x.CityofResidence,
                               AreaShortCode = x.AreaShortCode,
                               NoOfDays = x.NoOfDays
                           }).FirstOrDefault();

                var insuranceList = (from x in sql.InsuranceBookings
                                     join b in sql.InsurancePaxs
                                       on x.Id equals b.InsuranceBookingId into bookings
                                     from r in bookings.DefaultIfEmpty()
                                     where (r.InsuranceBookingId == BookingID && x.Vendor == "CSI")
                                     orderby x.EntryDateTime descending
                                     select new
                                     {
                                         FullName = r.FullName,
                                         DOB = r.DOB,
                                         PassportNo = r.PassportNo,
                                         PaxType = r.Type
                                     }).ToList();


                var travelInsurance = (from ti in sql.TravelInsurances.AsEnumerable()
                                       where ti.Id == ins.InsuranceCode
                                       select new TravelInsurance
                                       {
                                           Duration = ti.Duration,
                                           CountryDescrtiption = ti.CountryDescrtiption,
                                           InsuranceType = ti.InsuranceType,
                                           MedicalExpense = ti.MedicalExpense,
                                           SingleFee = ti.SingleFee,
                                           FamilyFee = ti.FamilyFee
                                       }).ToList();

                StringBuilder sb = new StringBuilder();

                DateTime depDate = Convert.ToDateTime(ins.DepartureDate);
                // DateTime arrDate = Convert.ToDateTime(ins.ArrivalDate);

                // TimeSpan timeDifference = arrDate - depDate;

                // Extract the total number of days from the TimeSpan
                //  int numberOfDays = Convert.ToInt32(Duration);
                string totalCost = "";

                if (ins.PassengerType == "Family")
                {
                    totalCost = travelInsurance[0].FamilyFee.ToString();
                }
                else
                {
                    totalCost = travelInsurance[0].SingleFee.ToString();
                }


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
                sb.Append("<p class='ft12'><b>NO:&#160;INV-" + DateTime.Now.Year.ToString() + "-" + BookingID + "</b></p>");
                sb.Append("<p class='ft17'><b>Plan: </b>");
                sb.Append("<span class='ft16'>" + travelInsurance[0].CountryDescrtiption + "</span></p>");
                sb.Append("<p class='ft17'><b>Visit&#160;Duration: </b>");
                sb.Append("<span class='ft16'>" + Duration + "</span></p>");
                sb.Append("<p class='ft17'><b>Insurance&#160;Type: </b>");
                sb.Append("<span class='ft16'>" + travelInsurance[0].InsuranceType + "</span></p>");
                sb.Append("<p class='ft17'><b>Medical&#160;Expense: </b>");
                sb.Append("<span class='ft16'>" + travelInsurance[0].MedicalExpense + "</span></p>");
                sb.Append("<p class='ft17'><b>Passenger&#160;Type: </b>");
                sb.Append("<span class='ft16'>" + ins.PassengerType + "</span></p>");
                sb.Append("<p class='ft17'><b>Payment Status&#160;: </b>");
                sb.Append("<span class='ft16' style='color:red;'>DUE</span></p>");
                sb.Append("</td>");
                sb.Append("</tr></table><br />");

                sb.Append("<table style='position:absolute;top:540px;left:55px; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                sb.Append("<tr><th colspan='2' style='white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'>Passenger Details</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> First Name</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.FirstName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Last Name</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.LastName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Departure Date</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.DepartureDate + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Return Date</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ArrivalDate + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Passport Num.</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.Passport + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Cnic </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.CNIC + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> DOB </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.DOB + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Mobile No.</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ContactNumber + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Purpose </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.PurposeOfVisit + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Address</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ResidentialAddress + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> City</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.CityofResidence + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Email</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.EmailAddress + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Country to Visit</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.FirstCountry + "</td>");
                sb.Append("</tr>");
                sb.Append("</table> <br />");

                if (insuranceList.Count > 0)
                {

                    sb.Append("<table style='position:absolute;top:540px;left:55px; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                    sb.Append("<tr><th colspan='4' style='white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'>Family Details</th>");
                    sb.Append("</tr><tr>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Full Name</th>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>DOB </th>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Passport No. </th>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Pax. Type </th>");
                    sb.Append("</tr>");
                    foreach (var itm in insuranceList)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.FullName + "</td>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.DOB + "</td>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.PassportNo + "</td>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.PaxType + "</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table> <br />");
                }

                sb.Append("<table style= 'position:absolute;top:540px;left:52%; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                sb.Append("<tr>");
                sb.Append("<th colspan= '2' style= 'white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'> Beneficiary Details</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> First Name</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryFirstName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Last Name</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryLastName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Relation </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryRelation + "</td>");
                sb.Append("</tr>");
                sb.Append("</table >");
                sb.Append("<table style= 'position:absolute;top:840px;left:52%; border-collapse: collapse;  width: 40%; '>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; background-color: #04a2e2; color: white; padding: 10px;'> Total Amount</td>");
                sb.Append("<td style='white-space:nowrap; background-color: #04a2e2;color: white;  padding: 10px;'> PKR " + ins.DiscountedPrice + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("<p class='ft15'><b>Email: </b>");
                sb.Append("<span class='ft16'>info@malikexpress.com</span></p>");
                sb.Append("<p style='position:absolute;top:912px;left:424px;white-space:nowrap' class='ft18'>Thank&#160;You!</p>");
                sb.Append("<br />");
                sb.Append("<p>Date: " + DateTime.Now.ToString() + "</p>");
                sb.Append("<img width='80' height='80' style='position:absolute;top:40px;left:350px;' src='https://malikexpress.com/images/logo mex.svg' alt='logo' />");
                sb.Append("</div>");
                sb.Append("</body>");
                sb.Append("</html>");

                SmtpClient smtpClient = new SmtpClient("mail.malikexpress.com");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("csi@malikexpress.com", "sF~359sm9");

                // Create the email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("csi@malikexpress.com");
                mailMessage.To.Add("ti@malikexpress.com"); // coo@growbiztech.com" ti@malikexpress.com
                mailMessage.To.Add("sales-isb@malikexpress.com");
                // mailMessage.To.Add(ins.EmailAddress);
                mailMessage.Subject = "New Insurance Booking.";
                mailMessage.Body = sb.ToString();
                mailMessage.IsBodyHtml = true;

                // Attach the image to be embedded.
                // Ensure the path to the image is correct.
                ////Attachment inlineLogo = new Attachment("https://malikexpress.com/images/logo mex.svg");
                ////inlineLogo.ContentDisposition.Inline = true;
                ////inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                ////inlineLogo.ContentId = "LogoID";
                ////inlineLogo.ContentType.MediaType = "image/svg";
                ////inlineLogo.ContentType.Name = Path.GetFileName(inlineLogo.Name);
                ////mailMessage.Attachments.Add(inlineLogo);


                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // Send the email
                smtpClient.Send(mailMessage);


                var JSONString = JsonConvert.SerializeObject(BookingID);
                return JSONString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost("uicinsurancebooking")]
        public string UICInsuranceBooking([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind=", "");
                var data = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                string PDays = string.IsNullOrEmpty(data?.PDays?.Value?.ToString()) ? "0" : data?.PDays?.Value?.ToString();

                // string Duration = string.IsNullOrEmpty(data?.Duration?.Value?.ToString()) ? "" : data?.Duration?.Value?.ToString();
                string DurationUnit = string.IsNullOrEmpty(data?.DurationUnit?.Value?.ToString()) ? "" : data?.DurationUnit?.Value?.ToString();
                string AreaShortCode = string.IsNullOrEmpty(data?.AreaShortCode?.Value?.ToString()) ? "" : data?.AreaShortCode?.Value?.ToString();
                string Area = string.IsNullOrEmpty(data?.Area?.Value?.ToString()) ? "" : data?.Area?.Value?.ToString();
                string Plan = string.IsNullOrEmpty(data?.Plan?.Value?.ToString()) ? "" : data?.Plan?.Value?.ToString();
                string PlanType = string.IsNullOrEmpty(data?.PlanType?.Value?.ToString()) ? "" : data?.PlanType?.Value?.ToString();
                string Premium = string.IsNullOrEmpty(data?.Premium?.Value?.ToString()) ? "" : data?.Premium?.Value?.ToString();
                string AgePremIncrease = string.IsNullOrEmpty(data?.AgePremIncrease?.Value?.ToString()) ? "" : data?.AgePremIncrease?.Value?.ToString();
                string GrossPremium = string.IsNullOrEmpty(data?.GrossPremium?.Value?.ToString()) ? "" : data?.GrossPremium?.Value?.ToString();
                string AdvanceTax = string.IsNullOrEmpty(data?.AdvanceTax?.Value?.ToString()) ? "" : data?.AdvanceTax?.Value?.ToString();
                string DiscountedPrice = string.IsNullOrEmpty(data?.DiscountedPrice?.Value?.ToString()) ? "" : data?.DiscountedPrice?.Value?.ToString();
                string TravelerName = string.IsNullOrEmpty(data?.TravelerName?.Value?.ToString()) ? "" : data?.TravelerName?.Value?.ToString();
                string TravelerNICNo = string.IsNullOrEmpty(data?.TravelerNICNo?.Value?.ToString()) ? "" : data?.TravelerNICNo?.Value?.ToString();
                string TravelerNTNNo = string.IsNullOrEmpty(data?.TravelerNTNNo?.Value?.ToString()) ? "" : data?.TravelerNTNNo?.Value?.ToString();
                string IsTaxPayer = string.IsNullOrEmpty(data?.IsTaxPayer?.Value?.ToString()) ? "" : data?.IsTaxPayer?.Value?.ToString();
                string TotalPayablePremium = string.IsNullOrEmpty(data?.TotalPayablePremium?.Value?.ToString()) ? "" : data?.TotalPayablePremium?.Value?.ToString();
                string TravelerDOB = string.IsNullOrEmpty(data?.TravelerDOB?.Value?.ToString()) ? "" : data?.TravelerDOB?.Value?.ToString();
                string TravelerAge = string.IsNullOrEmpty(data?.TravelerAge?.Value?.ToString()) ? "" : data?.TravelerAge?.Value?.ToString();
                string Covid = string.IsNullOrEmpty(data?.Covid?.Value?.ToString()) ? "" : data?.Covid?.Value?.ToString();
                string Vendor = string.IsNullOrEmpty(data?.Vendor?.Value?.ToString()) ? "" : data?.Vendor?.Value?.ToString();

                string First_nameB = string.IsNullOrEmpty(data?.BeneficiaryFirstName?.Value?.ToString()) ? "" : data?.BeneficiaryFirstName?.Value?.ToString();
                string Last_nameB = string.IsNullOrEmpty(data?.BeneficiaryLastName?.Value?.ToString()) ? "" : data?.BeneficiaryLastName?.Value?.ToString();
                string RelationB = string.IsNullOrEmpty(data?.BeneficiaryRelation?.Value?.ToString()) ? "" : data?.BeneficiaryRelation?.Value?.ToString();
                string Mobile_noB = string.IsNullOrEmpty(data?.BeneficiaryContactNumber?.Value?.ToString()) ? "" : data?.BeneficiaryContactNumber?.Value?.ToString();
                string CnicB = string.IsNullOrEmpty(data?.BeneficiaryCNIC?.Value?.ToString()) ? "" : data?.BeneficiaryCNIC?.Value?.ToString();
                string AddressB = string.IsNullOrEmpty(data?.BeneficiaryAddress?.Value?.ToString()) ? "" : data?.BeneficiaryAddress?.Value?.ToString();

                string MedicalExpense = string.IsNullOrEmpty(data?.MedicalExpense?.Value?.ToString()) ? "" : data?.MedicalExpense?.Value?.ToString();
                string DepartureDate = string.IsNullOrEmpty(data?.DepartureDate?.Value?.ToString()) ? "" : data?.DepartureDate?.Value?.ToString();
                string ArrivalDate = string.IsNullOrEmpty(data?.ArrivalDate?.Value?.ToString()) ? "" : data?.ArrivalDate?.Value?.ToString();
                string FirstCountry = string.IsNullOrEmpty(data?.FirstCountry?.Value?.ToString()) ? "" : data?.FirstCountry?.Value?.ToString();
                string SecondCountry = string.IsNullOrEmpty(data?.SecondCountry?.Value?.ToString()) ? "" : data?.SecondCountry?.Value?.ToString();

                string ContactNumber = string.IsNullOrEmpty(data?.ContactNumber?.Value?.ToString()) ? "" : data?.ContactNumber?.Value?.ToString();
                string ResidentialAddress = string.IsNullOrEmpty(data?.ResidentialAddress?.Value?.ToString()) ? "" : data?.ResidentialAddress?.Value?.ToString();
                string CityofResidence = string.IsNullOrEmpty(data?.CityofResidence?.Value?.ToString()) ? "" : data?.CityofResidence?.Value?.ToString();
                string EmailAddress = string.IsNullOrEmpty(data?.EmailAddress?.Value?.ToString()) ? "" : data?.EmailAddress?.Value?.ToString();
                string Passport = string.IsNullOrEmpty(data?.Passport?.Value?.ToString()) ? "" : data?.Passport?.Value?.ToString();
                string Duration = string.IsNullOrEmpty(data?.Duration?.Value?.ToString()) ? "0" : data?.Duration?.Value?.ToString();



                string PassengerType = "Single";

                if (PlanType != null && PlanType != "S")
                {
                    PassengerType = "Family";
                }

                MexCoreWebAPI.Model.CIS.UicBooking ib = new MexCoreWebAPI.Model.CIS.UicBooking();

                // Parse string values to appropriate types
                ib.PDays = string.IsNullOrEmpty(PDays) ? 0 : int.Parse(PDays);
                ib.Duration = string.IsNullOrEmpty(Duration) ? 0 : int.Parse(Duration);
                ib.DurationUnit = DurationUnit;
                ib.AreaShortCode = AreaShortCode;
                ib.Area = Area;
                ib.Plan = Plan;
                ib.PlanType = PassengerType;
                ib.Premium = string.IsNullOrEmpty(Premium) ? 0m : decimal.Parse(Premium);
                ib.AgePremIncrease = string.IsNullOrEmpty(AgePremIncrease) ? 0m : decimal.Parse(AgePremIncrease);
                ib.GrossPremium = string.IsNullOrEmpty(GrossPremium) ? 0m : decimal.Parse(GrossPremium);
                ib.DiscountedPrice = string.IsNullOrEmpty(DiscountedPrice) ? 0m : decimal.Parse(DiscountedPrice);
                ib.AdvanceTax = string.IsNullOrEmpty(AdvanceTax) ? 0m : decimal.Parse(AdvanceTax);
                ib.TravelerName = TravelerName;
                ib.TravelerNICNo = TravelerNICNo;
                ib.TravelerNTNNo = TravelerNTNNo;
                ib.IsTaxPayer = !string.IsNullOrEmpty(IsTaxPayer) && bool.Parse(IsTaxPayer);
                ib.TotalPayablePremium = string.IsNullOrEmpty(TotalPayablePremium) ? 0m : decimal.Parse(TotalPayablePremium);
                ib.TravelerDOB = string.IsNullOrEmpty(TravelerDOB) ? DateTime.MinValue : DateTime.Parse(TravelerDOB);
                ib.TravelerAge = string.IsNullOrEmpty(TravelerAge) ? 0 : int.Parse(TravelerAge);
                ib.Covid = Covid;
                ib.MedicalExpense = MedicalExpense;
                ib.DepartureDate = DepartureDate;
                ib.ArrivalDate = ArrivalDate;
                ib.FirstCountry = FirstCountry;
                ib.SecondCountry = SecondCountry;
                ib.ContactNumber = ContactNumber;
                ib.CityofResidence = CityofResidence;
                ib.ResidentialAddress = ResidentialAddress;
                ib.EmailAddress = EmailAddress;
                ib.BeneficiaryFirstName = First_nameB;
                ib.BeneficiaryLastName = Last_nameB;
                ib.BeneficiaryRelation = RelationB;
                ib.BeneficiaryContactNumber = Mobile_noB;
                ib.BeneficiaryCNIC = CnicB;
                ib.BeneficiaryAddress = AddressB;
                ib.Passport = Passport;

                sql.UicBookings.Add(ib);
                sql.SaveChanges();
                int BookingID = ib.Id;

                if (PlanType == "F")
                {
                    string FullNameA_1 = string.IsNullOrEmpty(data?.FullNameA_1?.Value) ? "" : data?.FullNameA_1?.Value;
                    string DobA_1 = string.IsNullOrEmpty(data?.DobA_1?.Value) ? "" : data?.DobA_1?.Value;
                    string PassportA_1 = string.IsNullOrEmpty(data?.PassportA_1?.Value) ? "" : data?.PassportA_1?.Value;

                    if (FullNameA_1 != null && FullNameA_1 != "")
                    {
                        MexCoreWebAPI.Model.CIS.InsurancePax ip = new InsurancePax();

                        ip.FullName = FullNameA_1;
                        ip.DOB = DobA_1;
                        ip.PassportNo = PassportA_1;
                        ip.Type = "Adult";
                        ip.InsuranceBookingId = BookingID;
                        ip.Vendor = "UIC";

                        sql.InsurancePaxs.Add(ip);
                        sql.SaveChanges();
                    }

                    string PassportC_1 = string.IsNullOrEmpty(data?.PassportC_1?.Value) ? "" : data?.PassportC_1?.Value;
                    string FullnameC_1 = string.IsNullOrEmpty(data?.FullnameC_1?.Value) ? "" : data?.FullnameC_1?.Value;
                    string DobC_1 = string.IsNullOrEmpty(data?.DobC_1?.Value) ? "" : data?.DobC_1?.Value;

                    if (FullnameC_1 != null && FullnameC_1 != "")
                    {
                        MexCoreWebAPI.Model.CIS.InsurancePax ip = new InsurancePax();

                        ip.FullName = FullnameC_1;
                        ip.DOB = DobC_1;
                        ip.PassportNo = PassportC_1;
                        ip.Type = "Child";
                        ip.InsuranceBookingId = BookingID;
                        ip.Vendor = "UIC";

                        sql.InsurancePaxs.Add(ip);
                        sql.SaveChanges();
                    }

                    string PassportC_2 = string.IsNullOrEmpty(data?.PassportC_2?.Value) ? "" : data?.PassportC_2?.Value;
                    string FullnameC_2 = string.IsNullOrEmpty(data?.FullnameC_2?.Value) ? "" : data?.FullnameC_2?.Value;
                    string DobC_2 = string.IsNullOrEmpty(data?.DobC_2?.Value) ? "" : data?.DobC_2?.Value;

                    if (FullnameC_2 != null && FullnameC_2 != "")
                    {
                        MexCoreWebAPI.Model.CIS.InsurancePax ip = new InsurancePax();

                        ip.FullName = FullnameC_2;
                        ip.DOB = DobC_2;
                        ip.PassportNo = PassportC_2;
                        ip.Type = "Child";
                        ip.InsuranceBookingId = BookingID;
                        ip.Vendor = "UIC";

                        sql.InsurancePaxs.Add(ip);
                        sql.SaveChanges();
                    }

                    string FullnameC_3 = string.IsNullOrEmpty(data?.FullnameC_3?.Value) ? "" : data?.FullnameC_3?.Value;
                    string PassportC_3 = string.IsNullOrEmpty(data?.PassportC_3?.Value) ? "" : data?.PassportC_3?.Value;
                    string DobC_3 = string.IsNullOrEmpty(data?.DobC_3?.Value) ? "" : data?.DobC_3?.Value;

                    if (FullnameC_3 != null && FullnameC_3 != "")
                    {
                        MexCoreWebAPI.Model.CIS.InsurancePax ip = new InsurancePax();

                        ip.FullName = FullnameC_3;
                        ip.DOB = DobC_3;
                        ip.PassportNo = PassportC_3;
                        ip.Type = "Child";
                        ip.InsuranceBookingId = BookingID;
                        ip.Vendor = "UIC";

                        sql.InsurancePaxs.Add(ip);
                        sql.SaveChanges();
                    }

                    string FullnameC_4 = string.IsNullOrEmpty(data?.FullnameC_4?.Value) ? "" : data?.FullnameC_4?.Value;
                    string PassportC_4 = string.IsNullOrEmpty(data?.PassportC_4?.Value) ? "" : data?.PassportC_4?.Value;
                    string DobC_4 = string.IsNullOrEmpty(data?.DobC_4?.Value) ? "" : data?.DobC_4?.Value;

                    if (FullnameC_4 != null && FullnameC_4 != "")
                    {
                        MexCoreWebAPI.Model.CIS.InsurancePax ip = new InsurancePax();

                        ip.FullName = FullnameC_4;
                        ip.DOB = DobC_4;
                        ip.PassportNo = PassportC_4;
                        ip.Type = "Child";
                        ip.InsuranceBookingId = BookingID;
                        ip.Vendor = "UIC";

                        sql.InsurancePaxs.Add(ip);
                        sql.SaveChanges();
                    }
                }

                var ins = (from x in sql.UicBookings
                           where x.Id == BookingID
                           orderby x.EntryDateTime descending
                           select new UicBooking
                           {
                               PDays = x.PDays,
                               Duration = x.Duration,
                               DurationUnit = x.DurationUnit,
                               AreaShortCode = x.AreaShortCode,
                               Area = x.Area,
                               Plan = x.Plan,
                               PlanType = x.PlanType,
                               Premium = x.Premium,
                               AgePremIncrease = x.AgePremIncrease,
                               GrossPremium = x.GrossPremium,
                               AdvanceTax = x.AdvanceTax,
                               TravelerName = x.TravelerName,
                               TravelerNICNo = x.TravelerNICNo,
                               TravelerNTNNo = x.TravelerNTNNo,
                               IsTaxPayer = x.IsTaxPayer,
                               TotalPayablePremium = x.TotalPayablePremium,
                               TravelerDOB = x.TravelerDOB,
                               TravelerAge = x.TravelerAge,
                               Covid = x.Covid,
                               MedicalExpense = x.MedicalExpense,
                               DepartureDate = x.DepartureDate,
                               ArrivalDate = x.ArrivalDate,
                               FirstCountry = x.FirstCountry,
                               SecondCountry = x.SecondCountry,
                               ContactNumber = x.ContactNumber,
                               CityofResidence = x.CityofResidence,
                               ResidentialAddress = x.ResidentialAddress,
                               EmailAddress = x.EmailAddress,
                               BeneficiaryFirstName = x.BeneficiaryFirstName,
                               BeneficiaryLastName = x.BeneficiaryLastName,
                               BeneficiaryContactNumber = x.BeneficiaryContactNumber,
                               BeneficiaryCNIC = x.BeneficiaryCNIC,
                               BeneficiaryAddress = x.BeneficiaryAddress,
                               BeneficiaryRelation = x.BeneficiaryRelation,
                               DiscountedPrice = x.DiscountedPrice,
                               Passport = x.Passport
                           }).FirstOrDefault();

                var insuranceList = (from x in sql.UicBookings
                                     join b in sql.InsurancePaxs
                                       on x.Id equals b.InsuranceBookingId into bookings
                                     from r in bookings.DefaultIfEmpty()
                                     where (r.InsuranceBookingId == BookingID && r.Vendor == "UIC")
                                     orderby x.EntryDateTime descending
                                     select new
                                     {
                                         FullName = r.FullName,
                                         DOB = r.DOB,
                                         PassportNo = r.PassportNo,
                                         PaxType = r.Type
                                     }).ToList();


                StringBuilder sb = new StringBuilder();

                DateTime depDate = Convert.ToDateTime(ins.DepartureDate);
                // DateTime arrDate = Convert.ToDateTime(ins.ArrivalDate);

                //TimeSpan timeDifference = arrDate - depDate;

                // Extract the total number of days from the TimeSpan
                //  int numberOfDays = Convert.ToInt32(Duration); //timeDifference.Days;
                string totalCost = TotalPayablePremium;

                //if (ins.PassengerType == "Family")
                //{
                //    totalCost = travelInsurance[0].FamilyFee.ToString();
                //}
                //else
                //{
                //    totalCost = travelInsurance[0].SingleFee.ToString();
                //}




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
                sb.Append("<p class='ft12'><b>NO:&#160;INV-" + DateTime.Now.Year.ToString() + "-" + BookingID + "</b></p>");
                sb.Append("<p class='ft17'><b>Plan: </b>");
                sb.Append("<span class='ft16'>" + Area + "</span></p>");
                sb.Append("<p class='ft17'><b>Visit&#160;Duration: </b>");
                sb.Append("<span class='ft16'>" + Duration + "</span></p>");
                sb.Append("<p class='ft17'><b>Insurance&#160;Type: </b>");
                sb.Append("<span class='ft16'>" + Plan + "</span></p>");
                sb.Append("<p class='ft17'><b>Medical&#160;Expense: </b>");
                sb.Append("<span class='ft16'>" + MedicalExpense + "</span></p>");
                sb.Append("<p class='ft17'><b>Passenger&#160;Type: </b>");
                sb.Append("<span class='ft16'>" + PassengerType + "</span></p>");
                sb.Append("<p class='ft17'><b>Payment Status&#160;: </b>");
                sb.Append("<span class='ft16' style='color:red;'>DUE</span></p>");
                sb.Append("</td>");
                //sb.Append("<p class='ft11'><b>Bill&#160;To:</b></p>");
                //sb.Append("<p class='ft10'>Estelle&#160;Darcy</p>");
                //sb.Append("<p class='ft10'>+123-456-7890</p>");
                //sb.Append("<p class='ft10'>");
                //sb.Append("123&#160;Anywhere&#160;St.,&#160;Any&#160;City</p>");
                sb.Append("</tr></table><br />");

                sb.Append("<table style='position:absolute;top:540px;left:55px; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                sb.Append("<tr><th colspan='2' style='white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'>Passenger Details</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Full Name</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.TravelerName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Departure Date</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.DepartureDate + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Return Date</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ArrivalDate + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Passport </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.Passport + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Cnic </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.TravelerNICNo + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> DOB </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.TravelerDOB + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Mobile No.</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ContactNumber + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Address</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ResidentialAddress + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> City</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.CityofResidence + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Email</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.EmailAddress + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Country to Visit</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.FirstCountry + "</td>");
                sb.Append("</tr>");
                sb.Append("</table> <br />");

                if (insuranceList.Count > 0)
                {

                    sb.Append("<table style='position:absolute;top:540px;left:55px; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                    sb.Append("<tr><th colspan='4' style='white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'>Family Details</th>");
                    sb.Append("</tr><tr>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Full Name</th>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>DOB </th>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Passport No. </th>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Pax. Type </th>");
                    sb.Append("</tr>");
                    foreach (var itm in insuranceList)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.FullName + "</td>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.DOB + "</td>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.PassportNo + "</td>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.PaxType + "</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table> <br />");
                }

                sb.Append("<table style= 'position:absolute;top:540px;left:52%; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                sb.Append("<tr>");
                sb.Append("<th colspan= '2' style= 'white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'> Beneficiary Details</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> First Name</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryFirstName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Last Name</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryLastName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Relation </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryRelation + "</td>");
                sb.Append("</tr>");
                sb.Append("</table >");
                sb.Append("<table style= 'position:absolute;top:840px;left:52%; border-collapse: collapse;  width: 40%; '>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; background-color: #04a2e2; color: white; padding: 10px;'> Total Amount</td>");
                sb.Append("<td style='white-space:nowrap; background-color: #04a2e2;color: white;  padding: 10px;'> PKR " + ins.DiscountedPrice + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                //sb.Append("<p class='ft111'><b>Payment&nbsp;Information:<br>Bank: </b>");
                //sb.Append("<span class='ft16'>Habib&nbsp;Bank&nbsp;Limited</span></p>");
                //sb.Append("<p class='ft15'><b>No&nbsp;Bank: </b>");
                //sb.Append("<span class='ft16'>123-456-7890</span></p>");
                sb.Append("<p class='ft15'><b>Email: </b>");
                sb.Append("<span class='ft16'>info@malikexpress.com</span></p>");
                sb.Append("<p style='position:absolute;top:912px;left:424px;white-space:nowrap' class='ft18'>Thank&#160;You!</p>");
                sb.Append("<br />");
                sb.Append("<p>Date: " + DateTime.Now.ToString() + "</p>");
                sb.Append("<img width='80' height='80' style='position:absolute;top:40px;left:350px;' src='https://malikexpress.com/images/logo mex.svg' alt='logo' />");
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
                mailMessage.To.Add(ins.EmailAddress); // coo@growbiztech.com" ti@malikexpress.com
                                                      //mailMessage.To.Add("sales-isb@malikexpress.com");
                                                      // mailMessage.To.Add(ins.EmailAddress);
                mailMessage.Subject = "New Insurance Booking.";
                mailMessage.Body = sb.ToString();
                mailMessage.IsBodyHtml = true;

                // Attach the image to be embedded.
                // Ensure the path to the image is correct.
                ////Attachment inlineLogo = new Attachment("https://malikexpress.com/images/logo mex.svg");
                ////inlineLogo.ContentDisposition.Inline = true;
                ////inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                ////inlineLogo.ContentId = "LogoID";
                ////inlineLogo.ContentType.MediaType = "image/svg";
                ////inlineLogo.ContentType.Name = Path.GetFileName(inlineLogo.Name);
                ////mailMessage.Attachments.Add(inlineLogo);


                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // Send the email
                smtpClient.Send(mailMessage);


                var JSONString = JsonConvert.SerializeObject(BookingID);
                return JSONString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("paymentconfirmation")]
        public async Task<string> PaymentConfirmation(int BookingId, bool PaymentStatus = false, string? PaymentMessage = null, string? PolicyNumber = null)
        {
            try
            {
                string policyPrintUrl = "";
                string PolicyNo = "";
                List<UICResponse> apiResponses = new List<UICResponse>();
                var insBooked = sql.InsuranceBookings.SingleOrDefault(b => b.Id == BookingId);

                if (insBooked != null)
                {
                    insBooked.PaymentStatus = PaymentStatus;
                    insBooked.PaymentMessage = PaymentMessage;
                    insBooked.PolicyNumber = PolicyNumber;
                    sql.SaveChanges();
                }

                var ins = (from x in sql.InsuranceBookings
                           join b in sql.TravelInsurances
                             on x.InsuranceCode equals b.Id into bookings
                           from r in bookings.DefaultIfEmpty()
                           where x.Id == BookingId
                           orderby x.EntryDateTime descending
                           select new InsuranceBookings
                           {
                               Id = x.Id,
                               InsuranceCode = x.InsuranceCode,
                               FirstName = x.FirstName,
                               LastName = x.LastName,
                               ContactNumber = x.ContactNumber,
                               CNIC = x.CNIC,
                               BeneficiaryFirstName = x.BeneficiaryFirstName,
                               BeneficiaryLastName = x.BeneficiaryLastName,
                               BeneficiaryContactNumber = x.BeneficiaryContactNumber,
                               PackageName = r.CountryDescrtiption,
                               InsuranceType = r.InsuranceType,
                               MedicalExpense = r.MedicalExpense,
                               PassengerType = x.PassengerType,
                               Passport = x.PassportNumber,
                               DepartureDate = x.DepartureDate,
                               ArrivalDate = x.ArrivalDate,
                               EmailAddress = x.EmailAddress,
                               BeneficiaryCNIC = x.BeneficiaryCNIC,
                               BeneficiaryAddress = x.BeneficiaryAddress,
                               BeneficiaryRelation = x.BeneficiaryRelation,
                               DOB = x.DOB,
                               PurposeOfVisit = x.PurposeOfVisit,
                               DiscountedPrice = x.DiscountedPrice,
                               ResidentialAddress = x.ResidentialAddress,
                               FirstCountry = x.FirstCountry,
                               CityofResidence = x.CityofResidence,
                               Vendor = x.Vendor,
                               PolicyNumber = x.PolicyNumber,
                           }).FirstOrDefault();

                var insuranceList = (from x in sql.InsuranceBookings
                                     join b in sql.InsurancePaxs
                                       on x.Id equals b.InsuranceBookingId into bookings
                                     from r in bookings.DefaultIfEmpty()
                                     where (r.InsuranceBookingId == BookingId && x.Vendor == "CSI")
                                     orderby x.EntryDateTime descending
                                     select new
                                     {
                                         FullName = r.FullName,
                                         DOB = r.DOB,
                                         PassportNo = r.PassportNo,
                                         PaxType = r.Type
                                     }).ToList();


                var travelInsurance = (from ti in sql.TravelInsurances.AsEnumerable()
                                       where ti.Id == ins.InsuranceCode
                                       select new TravelInsurance
                                       {
                                           Duration = ti.Duration,
                                           CountryDescrtiption = ti.CountryDescrtiption,
                                           InsuranceType = ti.InsuranceType,
                                           MedicalExpense = ti.MedicalExpense,
                                           SingleFee = ti.SingleFee,
                                           FamilyFee = ti.FamilyFee
                                       }).ToList();

               

                DateTime depDate = Convert.ToDateTime(ins.DepartureDate);
                // DateTime arrDate = Convert.ToDateTime(ins.ArrivalDate);

                // TimeSpan timeDifference = arrDate - depDate;

                // Extract the total number of days from the TimeSpan
                int numberOfDays = ins.Duration;
                string totalCost = "";

                if (ins.PassengerType == "Family")
                {
                    totalCost = travelInsurance[0].FamilyFee.ToString();
                }
                else
                {
                    totalCost = travelInsurance[0].SingleFee.ToString();
                }

                if (PaymentMessage == "Processed OK" && ins.Vendor == "UIC")
                {
                    string SpouseName = "";
                    string SpousePassport = "";
                    string SpouseDOB = "";
                    string Child1Name = "";
                    string Child1DOB = "";
                    string Child1Passport = "";
                    string Child2Name = "";
                    string Child2DOB = "";
                    string Child2Passport = "";
                    string Child3Name = "";
                    string Child3DOB = "";
                    string Child3Passport = "";

                    if (insuranceList != null && insuranceList.Count > 0)
                    {
                        SpouseName = insuranceList[0].FullName ?? "";
                        SpousePassport = insuranceList[0].PassportNo ?? "";
                        SpouseDOB = insuranceList[0].DOB ?? "";
                        Child1Name = insuranceList.Count > 1 ? insuranceList[1].FullName ?? "" : "";
                        Child1DOB = insuranceList.Count > 1 ? insuranceList[1].DOB ?? "" : "";
                        Child1Passport = insuranceList.Count > 1 ? insuranceList[1].PassportNo ?? "" : "";
                        Child2Name = insuranceList.Count > 2 ? insuranceList[2].FullName ?? "" : "";
                        Child2DOB = insuranceList.Count > 2 ? insuranceList[2].DOB ?? "" : "";
                        Child2Passport = insuranceList.Count > 2 ? insuranceList[2].PassportNo ?? "" : "";
                        Child3Name = insuranceList.Count > 3 ? insuranceList[3].FullName ?? "" : "";
                        Child3DOB = insuranceList.Count > 3 ? insuranceList[3].DOB ?? "" : "";
                        Child3Passport = insuranceList.Count > 3 ? insuranceList[3].PassportNo ?? "" : "";
                    }

                    string AreaShortCode = "SCH";

                    if (travelInsurance[0].CountryDescrtiption == "Rest of The World")
                    {
                        AreaShortCode = "ROW";
                    }
                    else if (travelInsurance[0].CountryDescrtiption == "WorldWide")
                    {
                        AreaShortCode = "WW";
                    }
                    else
                    {
                        AreaShortCode = "SCH";
                    }

                    using (var client = new HttpClient())
                    {
                        try
                        {
                            // Set the base address of the API
                            client.BaseAddress = new Uri("http://travelapi.theunitedsoftware.com/API/Travel/");

                            // Create the URL with query parameters
                            string url = "GenerateUWDocumentWithCovid?UserName=API_MALIK.EXPRESS&Password=AP!*malik85267P&AreaShortCode=" + AreaShortCode +
                                "&TravelerName=" + ins.FirstName + "  " + ins.LastName + "&NoOfDays=" + ins.Duration + "&DOB=" + ins.DOB +
                                "&PassportNo=" + ins.Passport + "&NICNo=&Address=" + ins.ResidentialAddress +
                                "&ContactNo=" + ins.ContactNumber + "&Country=pakistan&Remarks=Travel Insurance&PlanType=s" +
                                "&Plan=" + ins.InsuranceType + "&Premium=" + totalCost.ToString() +
                                "&ntnno=&TravelerEmail=" + ins.EmailAddress + "&SpouseName=" + SpouseName +
                                "&SpousePassport=" + SpousePassport + "&Child1Name=" + Child1Name + "&Child1DOB=" + Child1DOB +
                                "&Child1Passport=" + Child1Passport + "&Child2Name=" + Child2Name +
                                "&Child2DOB=" + Child2DOB + "&Child2Passport=" + Child2Passport + "&Child3Name=" + Child3Name +
                                "&Child3DOB=" + Child3DOB + "&Child3Passport=" + Child3Passport +
                                "&isRequestPolicy=False&Covid=&SrvSrce=UIC&ReferenceNo=&ESystemName=" +
                                "&EUserName=&BeneficiaryName=" + ins.BeneficiaryFirstName + " " + ins.BeneficiaryLastName +
                                "&Relationship=" + ins.BeneficiaryRelation + "&SpouseDOB=" + SpouseDOB +
                                "&TravelDate=" + ins.DepartureDate;

                            // Set the Basic Authentication header
                            var credentials = "TravelAPI:9f0a47f4be114fb19ab454c7c734f332";
                            var base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

                            // Add required headers
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                            client.DefaultRequestHeaders.UserAgent.ParseAdd("PostmanRuntime/7.40.0");
                            client.DefaultRequestHeaders.Add("API_KEY", "f04d8a9a146747ccbc87258f0f1bd77ebffe50d");
                            //client.DefaultRequestHeaders.Add("Postman-Token", "db45ff37-0747-48f9-be97-5e7d01cf0236");
                            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
                            client.DefaultRequestHeaders.Connection.Add("keep-alive");

                            // Send the POST request
                            HttpResponseMessage response = await client.PostAsync(url, null);

                            // Ensure the response is successful
                            response.EnsureSuccessStatusCode();

                            // Read the response content
                            string responseContent = await response.Content.ReadAsStringAsync();

                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                            };

                            var policyResponses = System.Text.Json.JsonSerializer.Deserialize<UICResponse[]>(responseContent, options);

                            // Extract the PolicyPrintUrl
                            policyPrintUrl = policyResponses[0].PolicyPrintUrl;
                            PolicyNo = policyResponses[0].PolicyNo;

                            // return policyPrintUrl;
                        }
                        catch (HttpRequestException e)
                        {

                            //Console.WriteLine("Message :{0} ", e.Message);
                        }

                    }

                }

                #region Build Email Bodies for client

                // this template for client

                StringBuilder sbClient = new StringBuilder();
                sbClient.Append("<table width='600' align='center' cellpadding='0' cellspacing='0' style='border:1px solid #ddd; background-color:#ffffff;'>");
                sbClient.Append("  <tr style='background-color:#000000; color:#ffffff;'>");
                sbClient.Append("    <td style='padding:20px;'>");
                sbClient.Append("      <table width='100%'>");
                sbClient.Append("        <tr>");
                sbClient.Append("          <td><img src='https://malikexpress.com/images/logomex.png' alt='Logo' style='max-height:50px;'></td>");
                sbClient.Append("          <td style='text-align:right; font-size:16px; font-weight:bold;'>");
                sbClient.Append("            <a href='tel:+923111778811' style='color:#ffffff; text-decoration:none;'>+92 311 1778811</a>");
                sbClient.Append("          </td>");
                sbClient.Append("        </tr>");
                sbClient.Append("      </table>");
                sbClient.Append("    </td>");
                sbClient.Append("  </tr>");

                sbClient.Append("  <tr>");
                sbClient.Append("    <td style='padding:20px; font-family:Arial,sans-serif; font-size:14px; color:#333;'>");
                sbClient.Append("      <p>Dear " + ins.FirstName + " " + ins.LastName + ",</p>");
                sbClient.Append("      <p>Thank you for comparing travel insurance at <a href='https://malikexpress.com' style='color:#e5b815;'>malikexpress.com</a></p>");
                sbClient.Append("      <table width='100%' cellpadding='0' cellspacing='0' style='border-collapse:collapse; margin-top:15px; margin-bottom:15px;'>");
                sbClient.Append("        <tr>");
                sbClient.Append("          <th colspan='2' style='background-color:#e5b815; color:#fff; text-align:center; font-size:16px; padding:10px;'>Following are the details of your enquiry</th>");
                sbClient.Append("        </tr>");
                sbClient.Append("        <tr><th style='text-align:left; padding:10px; border:1px solid #ccc;'>Enquiry ID</th><td style='padding:10px; border:1px solid #ccc;'>" + BookingId + "</td></tr>");
                sbClient.Append("        <tr><th style='text-align:left; padding:10px; border:1px solid #ccc;'>Plan</th><td style='padding:10px; border:1px solid #ccc;'>" + travelInsurance[0].CountryDescrtiption + "</td></tr>");
                sbClient.Append("        <tr><th style='text-align:left; padding:10px; border:1px solid #ccc;'>Insurance Type</th><td style='padding:10px; border:1px solid #ccc;'>" + travelInsurance[0].InsuranceType + "</td></tr>");
                sbClient.Append("        <tr><th style='text-align:left; padding:10px; border:1px solid #ccc;'>Passenger Type</th><td style='padding:10px; border:1px solid #ccc;'>" + ins.PassengerType + "</td></tr>");
                sbClient.Append("        <tr><th style='text-align:left; padding:10px; border:1px solid #ccc;'>Departure Date</th><td style='padding:10px; border:1px solid #ccc;'>" + Convert.ToDateTime(ins.DepartureDate).ToString("yyyy-MM-dd") + "</td></tr>");
                sbClient.Append("        <tr><th style='text-align:left; padding:10px; border:1px solid #ccc;'>Return Date</th><td style='padding:10px; border:1px solid #ccc;'>" + Convert.ToDateTime(ins.ArrivalDate).ToString("yyyy-MM-dd") + "</td></tr>");
                sbClient.Append("        <tr><th style='text-align:left; padding:10px; border:1px solid #ccc;'>Country to Visit</th><td style='padding:10px; border:1px solid #ccc;'>" + ins.FirstCountry + "</td></tr>");
                sbClient.Append("        <tr><th style='text-align:left; padding:10px; background-color:#e5b815; color:#fff; border:1px solid #e5b815;'>Total Amount</th><td style='padding:10px; background-color:#e5b815; color:#fff; border:1px solid #e5b815;'>PKR " + ins.DiscountedPrice + "</td></tr>");
                sbClient.Append("      </table>");
                sbClient.Append("      <p>Please wait and relax. Our customer care advisor will be in touch with you shortly to further process the travel insurance policy.</p>");
                sbClient.Append("      <p>In case of any urgent query feel free to call us on <a href='tel:+923111778811' style='color:#e5b815;'>+92 311 1778811</a></p>");
                sbClient.Append("    </td>");
                sbClient.Append("  </tr>");

                sbClient.Append("  <tr>");
                sbClient.Append("    <td style='padding:30px 20px; background-color:#fff6d2;'>");
                sbClient.Append("      <table width='100%' cellpadding='0' cellspacing='0' style='text-align:center;'>");
                sbClient.Append("        <tr>");
                sbClient.Append("          <td style='vertical-align:top; min-width:220px; position:relative;'>");
                sbClient.Append("            <div style='position:absolute; top:-12px; left:50%; transform:translateX(-50%); font-size:13px; font-weight:bold; background-color:#e5b815; color:#fff; padding:10px 35px; border-radius:6px; z-index:1;'>Call</div>");
                sbClient.Append("            <div style='border:1px solid #e5b815; border-radius:6px; padding:35px 15px; margin-top:10px;background-color: #ffffff;'>");
                sbClient.Append("              <a href='tel:051111778800' style='color:#e5b815; font-size:14px; text-decoration:none;'>");
                sbClient.Append("                <img src='https://img.icons8.com/ios-filled/50/e5b815/phone.png' alt='Phone Icon' style='height:18px; vertical-align:middle;' />&nbsp;051-111-778-800");
                sbClient.Append("              </a>");
                sbClient.Append("            </div>");
                sbClient.Append("          </td>");

                sbClient.Append("          <td style='width:20px;'></td>");

                sbClient.Append("          <td style='vertical-align:top; min-width:220px; position:relative;'>");
                sbClient.Append("            <div style='position:absolute; top:-12px; left:50%; transform:translateX(-50%); font-size:13px; font-weight:bold; background-color:#e5b815; color:#fff; padding:10px 20px; border-radius:6px; z-index:1;'>WhatsApp</div>");
                sbClient.Append("            <div style='border:1px solid #e5b815; border-radius:6px; padding:35px 15px; margin-top:10px;background-color: #ffffff;'>");
                sbClient.Append("              <a href='https://wa.me/+923111778811?text=Hi!%20Malik%20Express.' style='color:#e5b815; font-size:14px; text-decoration:none;'>");
                sbClient.Append("                <img src='https://img.icons8.com/ios-filled/50/e5b815/whatsapp.png' alt='WhatsApp Icon' style='height:18px; vertical-align:middle;' />&nbsp;+92&nbsp;311&nbsp;1778811");
                sbClient.Append("              </a>");
                sbClient.Append("            </div>");
                sbClient.Append("          </td>");
                sbClient.Append("        </tr>");
                sbClient.Append("      </table>");
                sbClient.Append("    </td>");
                sbClient.Append("  </tr>");

                sbClient.Append("  <tr>");
                sbClient.Append("    <td style='border-top:1px solid #ccc; border-bottom:1px solid #ccc; padding:10px 20px;'>");
                sbClient.Append("      <table width='100%' style='text-align:center;'>");
                sbClient.Append("        <tr>");
                sbClient.Append("          <td><img src='https://malikexpress.com/images/UIC.png' alt='UIC Logo' style='height:50px;'></td>");
                sbClient.Append("          <td><img src='https://malikexpress.com/images/CSI-Logo.png' alt='CSI Logo' style='height:50px;'></td>");
                sbClient.Append("        </tr>");
                sbClient.Append("      </table>");
                sbClient.Append("    </td>");
                sbClient.Append("  </tr>");

                sbClient.Append("  <tr>");
                sbClient.Append("    <td style='background-color:#e5b815; color:white; text-align:center; padding:10px; font-size:12px;'>");
                sbClient.Append("      © 2025 <a href='https://malikexpress.com/' style='color:#ffffff; text-decoration:none;'>Malik Express </a>. All Rights Reserved");
                sbClient.Append("    </td>");
                sbClient.Append("  </tr>");
                sbClient.Append("</table>");

                #endregion


                #region Email template for admin
                // this template for admin
                StringBuilder sbAdmin = new StringBuilder();
                sbAdmin.Append("<html>");
                sbAdmin.Append("<head><meta charset='UTF-8'></head>");
                sbAdmin.Append("<body>");
                sbAdmin.Append("<table width='600' align='center' cellpadding='0' cellspacing='0' style='border:1px solid #ddd; background-color:#ffffff;'>");

                // Header
                sbAdmin.Append("<tr style='background-color:#000000; color:#ffffff;'>");
                sbAdmin.Append("<td style='padding:20px;'>");
                sbAdmin.Append("<table width='100%'><tr>");
                sbAdmin.Append("<td><img src='https://malikexpress.com/images/logomex.png' alt='Malik Express Logo' style='max-height:50px;'></td>");
                sbAdmin.Append("<td style='text-align:right; font-size:16px;color:#fff;font-weight:bold;'>Enquiry ID:" + BookingId + "</td>");
                sbAdmin.Append("</tr></table>");
                sbAdmin.Append("</td></tr>");

                // Body Content Start
                sbAdmin.Append("<tr><td style='padding:20px; font-family:Arial,sans-serif; font-size:14px; color:#333;'>");
                sbAdmin.Append("<p style='font-size:16px; font-weight:bold;'>You have received a new insurance enquiry email from malikexpress.com</p>");

                // Passenger Details
                sbAdmin.Append("<table width='100%' cellpadding='0' cellspacing='0' style='border-collapse:collapse; margin-top:15px; margin-bottom:15px;'>");
                sbAdmin.Append("<tr><th colspan='2' style='background-color:#e5b815; color:#fff; text-align:center; font-size:16px; padding:10px;'>Passenger Detail</th></tr>");

                void AddRow(string label, string value)
                {
                    sbAdmin.Append("<tr>");
                    sbAdmin.Append("<th style='text-align:left; padding:10px; border:1px solid #ccc;'>" + label + "</th>");
                    sbAdmin.Append("<td style='padding:10px; border:1px solid #ccc;'>" + value + "</td>");
                    sbAdmin.Append("</tr>");
                }

                AddRow("Name", ins.FirstName + " " + ins.LastName);
                AddRow("Departure Date", ins.DepartureDate);
                AddRow("Return Date", ins.ArrivalDate);
                AddRow("Plan", travelInsurance[0].CountryDescrtiption);
                AddRow("Insurance Type", travelInsurance[0].InsuranceType);
                AddRow("Passenger Type", ins.PassengerType);
                AddRow("Passport No", ins.Passport);
                AddRow("Cnic", ins.CNIC);
                AddRow("DOB", ins.DOB);
                AddRow("Mobile No", ins.ContactNumber);
                AddRow("Purpose", ins.PurposeOfVisit);
                AddRow("Address", ins.ResidentialAddress);
                AddRow("City", ins.CityofResidence);
                AddRow("Email", ins.EmailAddress);
                AddRow("Country to Visit", ins.FirstCountry);
                sbAdmin.Append("</table>");

                // Beneficiary Details
                sbAdmin.Append("<table width='100%' cellpadding='0' cellspacing='0' style='border-collapse:collapse; margin-top:15px; margin-bottom:15px;'>");
                sbAdmin.Append("<tr><th colspan='2' style='background-color:#e5b815; color:#fff; text-align:center; font-size:16px; padding:10px;'>Beneficiary Details</th></tr>");

                AddRow("First Name", ins.BeneficiaryFirstName);
                AddRow("Last Name", ins.BeneficiaryLastName);
                AddRow("Relation", ins.BeneficiaryRelation);

                // Styled Total Amount row
                sbAdmin.Append("<tr>");
                sbAdmin.Append("<th style='text-align:left; padding:10px; background-color:#e5b815; color:#fff; border:1px solid #e5b815;'>Total Amount</th>");
                sbAdmin.Append("<td style='padding:10px; background-color:#e5b815; color:#fff; border:1px solid #e5b815;'>PKR " + ins.DiscountedPrice + "</td>");
                sbAdmin.Append("</tr>");

                sbAdmin.Append("</table>");
                sbAdmin.Append("</td></tr>");


                // Footer
                sbAdmin.Append("<tr>");
                sbAdmin.Append("<td style='background-color:#e5b815; color:white; text-align:center; padding:10px; font-size:12px;'>");
                sbAdmin.Append("© 2025 <a href='https://malikexpress.com/' style='color:#ffffff; text-decoration:none;'>Malik Express</a>. All Rights Reserved");
                sbAdmin.Append("</td>");
                sbAdmin.Append("</tr>");

                sbAdmin.Append("</table>");
                sbAdmin.Append("</body></html>");

                #endregion

                #region 
                // Old Template email code comment by Prog Zubair 2025-07-12
                //sb.Append("<html xmlns='http://www.w3.org/1999/xhtml' lang='' xml: lang=''>");
                //sb.Append("<head>");
                //sb.Append("<title></title>");
                //sb.Append("<meta http - equiv='Content-Type' content='text/html; charset=UTF-8' />");
                //sb.Append("<style type='text/css'>");
                //sb.Append("p {");
                //sb.Append("margin: 0;");
                //sb.Append("padding: 0;");
                //sb.Append("}");
                //sb.Append(".ft10 {");
                //sb.Append("font-size: 18px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #6a6a6a;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft11 {");
                //sb.Append("font-size: 23px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #535353;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft12 {");
                //sb.Append("font-size: 19px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #535353;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft13 {");
                //sb.Append("font-size: 14px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #535353;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft14 {");
                //sb.Append("font-size: 12px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #6a6a6a;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft15 {");
                //sb.Append("font-size: 15px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #535353;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft16 {");
                //sb.Append("font-size: 15px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #6a6a6a;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft17 {");
                //sb.Append("font-size: 20px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #535353;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft18 {");
                //sb.Append("font-size: 86px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #04a2e2;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft19 {");
                //sb.Append("font-size: 52px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #6d6e70;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft110 {");
                //sb.Append("font-size: 24px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #6d6e70;");
                //sb.Append("}");
                //sb.Append("");
                //sb.Append(".ft111 {");
                //sb.Append("font-size: 15px;");
                //sb.Append("line-height: 34px;");
                //sb.Append("font-family: Times;");
                //sb.Append("color: #535353;");
                //sb.Append("}");
                //sb.Append("</style>");
                //sb.Append("</head>");
                //sb.Append("<body bgcolor='#fff' vlink='blue' link='blue'>");
                //sb.Append("<div id='page1-div'>");
                //// sb.Append("<img width='80' height='80' style='position:absolute;top:40px;left:350px;' src='https://malikexpress.com/images/logo mex.svg' alt='background image' />");
                //sb.Append("<table style='border:0;'><tr><td>");
                //sb.Append("<p class='ft19'>Malik Air Express</p>");
                //sb.Append("<p class='ft12'><b>NO:&#160;INV-" + DateTime.Now.Year.ToString() + "-" + BookingId + "</b></p>");
                //sb.Append("<p class='ft17'><b>Policy Number: </b>");
                //sb.Append("<span class='ft16'>" + ins.PolicyNumber + "</span></p>");
                //sb.Append("<p class='ft17'><b>Plan: </b>");
                //sb.Append("<span class='ft16'>" + travelInsurance[0].CountryDescrtiption + "</span></p>");
                //sb.Append("<p class='ft17'><b>Visit&#160;Duration: </b>");
                //sb.Append("<span class='ft16'>" + travelInsurance[0].Duration + "</span></p>");
                //sb.Append("<p class='ft17'><b>Insurance&#160;Type: </b>");
                //sb.Append("<span class='ft16'>" + travelInsurance[0].InsuranceType + "</span></p>");
                //sb.Append("<p class='ft17'><b>Medical&#160;Expense: </b>");
                //sb.Append("<span class='ft16'>" + travelInsurance[0].MedicalExpense + "</span></p>");
                //sb.Append("<p class='ft17'><b>Passenger&#160;Type: </b>");
                //sb.Append("<span class='ft16'>" + ins.PassengerType + "</span></p>");
                //sb.Append("<p class='ft17'><b>Payment Status&#160;: </b>");

                //if (PaymentMessage == "Processed OK")
                //{
                //    sb.Append("<span class='ft16' style='color:red;'>Payment Received</span></p>");
                //    sb.Append("<p class='ft17'><b>UIC's Policy Number&#160;: </b>");
                //    sb.Append("<span class='ft16'>" + PolicyNo + "</span></p>");
                //    sb.Append("<p class='ft17'><b>Download URL&#160;: </b>");
                //    sb.Append("<span class='ft16'>" + policyPrintUrl + "</span></p>");                    
                //}
                //else
                //{
                //    sb.Append("<span class='ft16' style='color:red;'>" + PaymentMessage + "</span></p>");
                //}
                //sb.Append("</td>");
                //sb.Append("</tr></table><br />");

                //sb.Append("<table style='position:absolute;top:540px;left:55px; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                //sb.Append("<tr><th colspan='2' style='white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'>Passenger Details</th>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> First Name</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.FirstName + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Last Name</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.LastName + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Departure Date</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.DepartureDate + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Return Date</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ArrivalDate + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Passport Num.</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.Passport + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Cnic </td>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.CNIC + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> DOB </td>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.DOB + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Mobile No.</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ContactNumber + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Purpose </td>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.PurposeOfVisit + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Address</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ResidentialAddress + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>City</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.CityofResidence + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Email</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.EmailAddress + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Country to Visit</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.FirstCountry + "</td>");
                //sb.Append("</tr>");
                //sb.Append("</table> <br />");

                //if (insuranceList.Count > 0)
                //{

                //    sb.Append("<table style='position:absolute;top:540px;left:55px; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                //    sb.Append("<tr><th colspan='4' style='white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'>Family Details</th>");
                //    sb.Append("</tr><tr>");
                //    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Full Name</th>");
                //    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>DOB </th>");
                //    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Passport No. </th>");
                //    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Pax. Type </th>");
                //    sb.Append("</tr>");
                //    foreach (var itm in insuranceList)
                //    {
                //        sb.Append("<tr>");
                //        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.FullName + "</td>");
                //        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.DOB + "</td>");
                //        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.PassportNo + "</td>");
                //        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.PaxType + "</td>");
                //        sb.Append("</tr>");
                //    }
                //    sb.Append("</table> <br />");
                //}

                //sb.Append("<table style= 'position:absolute;top:540px;left:52%; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                //sb.Append("<tr>");
                //sb.Append("<th colspan= '2' style= 'white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'> Beneficiary Details</th>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> First Name</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryFirstName + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Last Name</td>");
                //sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryLastName + "</td>");
                //sb.Append("</tr>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Relation </td>");
                //sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryRelation + "</td>");
                //sb.Append("</tr>");
                //sb.Append("</table >");
                //sb.Append("<table style= 'position:absolute;top:840px;left:52%; border-collapse: collapse;  width: 40%; '>");
                //sb.Append("<tr>");
                //sb.Append("<td style= 'white-space:nowrap; background-color: #04a2e2; color: white; padding: 10px;'> Total Amount</td>");
                //sb.Append("<td style='white-space:nowrap; background-color: #04a2e2;color: white;  padding: 10px;'> PKR " + ins.DiscountedPrice + "</td>");
                //sb.Append("</tr>");
                //sb.Append("</table>");
                //sb.Append("<p class='ft15'><b>Email: </b>");
                //sb.Append("<span class='ft16'>info@malikexpress.com</span></p>");
                //sb.Append("<p style='position:absolute;top:912px;left:424px;white-space:nowrap' class='ft18'>Thank&#160;You!</p>");
                //sb.Append("<br />");
                //sb.Append("<p>Date: " + DateTime.Now.ToString() + "</p>");
                //sb.Append("<img width='80' height='80' style='position:absolute;top:40px;left:350px;' src='https://malikexpress.com/images/logo mex.svg' alt='logo' />");
                //sb.Append("</div>");
                //sb.Append("</body>");
                //sb.Append("</html>");

                // sb.Append("\r\n\r\nPassenger Type: " + passengertype);
                #endregion


                SmtpClient smtpClient = new SmtpClient("mail.malikexpress.com");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("csi@malikexpress.com", "sF~359sm9");

                // Create the email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("csi@malikexpress.com");
                mailMessage.To.Add("ti@malikexpress.com"); // coo@growbiztech.com" ti@malikexpress.com sales-isb@malikexpress.com
                //mailMessage.To.Add("sales-isb@malikexpress.com");
                mailMessage.To.Add(ins.EmailAddress);
                mailMessage.Subject = "Payment Confirmation.";
                mailMessage.Body = sbClient.ToString();
                mailMessage.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // Send the email
                smtpClient.Send(mailMessage);

                #region Send Email to Admin

                MailMessage mailToAdmin = new MailMessage();
                mailToAdmin.From = new MailAddress("csi@malikexpress.com");
                mailToAdmin.To.Add("sales-isb@malikexpress.com"); // Admin email
                mailToAdmin.To.Add("ti@malikexpress.com");        // Optional additional admin
              //  mailToAdmin.To.Add("zubairsoftware.net@gmail.com");
                mailToAdmin.Subject = "New Travel Insurance Enquiry";
                mailToAdmin.Body = sbAdmin.ToString();
                mailToAdmin.IsBodyHtml = true;

                smtpClient.Send(mailToAdmin);

                #endregion

                var JSONString = JsonConvert.SerializeObject(BookingId);
                return JSONString;
            }

            catch (Exception ex)
            {
                var JSONString = JsonConvert.SerializeObject(ex.Message.ToString());
                return JSONString;
            }
        }

        [HttpGet("uicpaymentconfirmation")]
        public string UICPaymentConfirmation(int BookingId, bool PaymentStatus = false, string? PaymentMessage = null, string? PolicyNumber = null)
        {
            try
            {
                var insBooked = sql.UicBookings.SingleOrDefault(b => b.Id == BookingId);

                if (insBooked != null)
                {
                    insBooked.PaymentStatus = PaymentStatus;
                    insBooked.PaymentMessage = PaymentMessage;
                    insBooked.PolicyNumber = PolicyNumber;
                    sql.SaveChanges();
                }

                var ins = (from x in sql.UicBookings
                           where x.Id == BookingId
                           orderby x.EntryDateTime descending
                           select new UicBooking
                           {
                               PDays = x.PDays,
                               Duration = x.Duration,
                               DurationUnit = x.DurationUnit,
                               AreaShortCode = x.AreaShortCode,
                               Area = x.Area,
                               Plan = x.Plan,
                               PlanType = x.PlanType,
                               Premium = x.Premium,
                               AgePremIncrease = x.AgePremIncrease,
                               GrossPremium = x.GrossPremium,
                               AdvanceTax = x.AdvanceTax,
                               TravelerName = x.TravelerName,
                               TravelerNICNo = x.TravelerNICNo,
                               TravelerNTNNo = x.TravelerNTNNo,
                               IsTaxPayer = x.IsTaxPayer,
                               TotalPayablePremium = x.TotalPayablePremium,
                               TravelerDOB = x.TravelerDOB,
                               TravelerAge = x.TravelerAge,
                               Covid = x.Covid,
                               MedicalExpense = x.MedicalExpense,
                               DepartureDate = x.DepartureDate,
                               ArrivalDate = x.ArrivalDate,
                               FirstCountry = x.FirstCountry,
                               SecondCountry = x.SecondCountry,
                               ContactNumber = x.ContactNumber,
                               CityofResidence = x.CityofResidence,
                               ResidentialAddress = x.ResidentialAddress,
                               EmailAddress = x.EmailAddress,
                               BeneficiaryCNIC = x.BeneficiaryCNIC,
                               BeneficiaryAddress = x.BeneficiaryAddress,
                               BeneficiaryRelation = x.BeneficiaryRelation,
                               DiscountedPrice = x.DiscountedPrice,
                               PaymentStatus = x.PaymentStatus,
                               PaymentMessage = x.PaymentMessage,
                               PolicyNumber = x.PolicyNumber,
                               Passport = x.Passport
                           }).FirstOrDefault();

                var insuranceList = (from x in sql.UicBookings
                                     join b in sql.InsurancePaxs
                                       on x.Id equals b.InsuranceBookingId into bookings
                                     from r in bookings.DefaultIfEmpty()
                                     where (r.InsuranceBookingId == BookingId && r.Vendor == "UIC")
                                     orderby x.EntryDateTime descending
                                     select new
                                     {
                                         FullName = r.FullName,
                                         DOB = r.DOB,
                                         PassportNo = r.PassportNo,
                                         PaxType = r.Type
                                     }).ToList();


                StringBuilder sb = new StringBuilder();

                DateTime depDate = Convert.ToDateTime(ins.DepartureDate);
                DateTime arrDate = Convert.ToDateTime(ins.ArrivalDate);

                TimeSpan timeDifference = arrDate - depDate;

                // Extract the total number of days from the TimeSpan
                int numberOfDays = timeDifference.Days;
                string totalCost = "";



                string PassengerType = "Single";

                if (ins.PlanType != null && ins.PlanType != "S")
                {
                    PassengerType = "Family";
                }



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
                // sb.Append("<img width='80' height='80' style='position:absolute;top:40px;left:350px;' src='https://malikexpress.com/images/logo mex.svg' alt='background image' />");
                sb.Append("<table style='border:0;'><tr><td>");
                sb.Append("<p class='ft19'>Malik Air Express</p>");
                sb.Append("<p class='ft12'><b>NO:&#160;INV-" + DateTime.Now.Year.ToString() + "-" + BookingId + "</b></p>");
                sb.Append("<p class='ft17'><b>Policy Number: </b>");
                sb.Append("<span class='ft16'>" + ins.PolicyNumber + "</span></p>");
                sb.Append("<p class='ft17'><b>Plan: </b>");
                sb.Append("<span class='ft16'>" + ins.Area + "</span></p>");
                sb.Append("<p class='ft17'><b>Visit&#160;Duration: </b>");
                sb.Append("<span class='ft16'>" + numberOfDays + "</span></p>");
                sb.Append("<p class='ft17'><b>Insurance&#160;Type: </b>");
                sb.Append("<span class='ft16'>" + ins.Plan + "</span></p>");
                sb.Append("<p class='ft17'><b>Medical&#160;Expense: </b>");
                sb.Append("<span class='ft16'>" + ins.MedicalExpense + "</span></p>");
                sb.Append("<p class='ft17'><b>Passenger&#160;Type: </b>");
                sb.Append("<span class='ft16'>" + PassengerType + "</span></p>");
                sb.Append("<p class='ft17'><b>Payment Status&#160;: </b>");

                if (PaymentMessage == "Transaction completed successfully")
                {
                    sb.Append("<span class='ft16' style='color:red;'>Payment Received</span></p>");
                }
                else
                {
                    sb.Append("<span class='ft16' style='color:red;'>" + ins.PaymentMessage + "</span></p>");
                }
                sb.Append("</td>");
                //sb.Append("<p class='ft11'><b>Bill&#160;To:</b></p>");
                //sb.Append("<p class='ft10'>Estelle&#160;Darcy</p>");
                //sb.Append("<p class='ft10'>+123-456-7890</p>");
                //sb.Append("<p class='ft10'>");
                //sb.Append("123&#160;Anywhere&#160;St.,&#160;Any&#160;City</p>");
                sb.Append("</tr></table><br />");

                sb.Append("<table style='position:absolute;top:540px;left:55px; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                sb.Append("<tr><th colspan='2' style='white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'>Passenger Details</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Full Name</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.TravelerName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Departure Date</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.DepartureDate + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Return Date</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ArrivalDate + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Passport </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.Passport + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Cnic </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.TravelerNICNo + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> DOB </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.TravelerDOB + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Mobile No.</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ContactNumber + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Address</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.ResidentialAddress + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>City</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.CityofResidence + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Email</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.EmailAddress + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Country to Visit</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.FirstCountry + "</td>");
                sb.Append("</tr>");
                sb.Append("</table> <br />");

                if (insuranceList.Count > 0)
                {

                    sb.Append("<table style='position:absolute;top:540px;left:55px; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                    sb.Append("<tr><th colspan='4' style='white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'>Family Details</th>");
                    sb.Append("</tr><tr>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Full Name</th>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>DOB </th>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Passport No. </th>");
                    sb.Append("<th style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>Pax. Type </th>");
                    sb.Append("</tr>");
                    foreach (var itm in insuranceList)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.FullName + "</td>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.DOB + "</td>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.PassportNo + "</td>");
                        sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + itm.PaxType + "</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table> <br />");
                }

                sb.Append("<table style= 'position:absolute;top:540px;left:52%; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                sb.Append("<tr>");
                sb.Append("<th colspan= '2' style= 'white-space:nowrap; border: 1px solid #000; background-color: #04a2e2; color:white ;padding: 15px;'> Beneficiary Details</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'> First Name</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryFirstName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Last Name</td>");
                sb.Append("<td style='white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryLastName + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'> Relation </td>");
                sb.Append("<td style= 'white-space:nowrap; border: 1px solid #000;  padding: 10px;'>" + ins.BeneficiaryRelation + "</td>");
                sb.Append("</tr>");
                sb.Append("</table >");
                sb.Append("<table style= 'position:absolute;top:840px;left:52%; border-collapse: collapse;  width: 40%; '>");
                sb.Append("<tr>");
                sb.Append("<td style= 'white-space:nowrap; background-color: #04a2e2; color: white; padding: 10px;'> Total Amount</td>");
                sb.Append("<td style='white-space:nowrap; background-color: #04a2e2;color: white;  padding: 10px;'> PKR " + ins.DiscountedPrice + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                //sb.Append("<p class='ft111'><b>Payment&nbsp;Information:<br>Bank: </b>");
                //sb.Append("<span class='ft16'>Habib&nbsp;Bank&nbsp;Limited</span></p>");
                //sb.Append("<p class='ft15'><b>No&nbsp;Bank: </b>");
                //sb.Append("<span class='ft16'>123-456-7890</span></p>");
                sb.Append("<p class='ft15'><b>Email: </b>");
                sb.Append("<span class='ft16'>info@malikexpress.com</span></p>");
                sb.Append("<p style='position:absolute;top:912px;left:424px;white-space:nowrap' class='ft18'>Thank&#160;You!</p>");
                sb.Append("<br />");
                sb.Append("<p>Date: " + DateTime.Now.ToString() + "</p>");
                sb.Append("<img width='80' height='80' style='position:absolute;top:40px;left:350px;' src='https://malikexpress.com/images/logo mex.svg' alt='logo' />");
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
                mailMessage.To.Add("ti@malikexpress.com"); // coo@growbiztech.com" ti@malikexpress.com sales-isb@malikexpress.com
                mailMessage.To.Add("sales-isb@malikexpress.com");
                mailMessage.To.Add(ins.EmailAddress);
                mailMessage.Subject = "Payment Confirmation.";
                mailMessage.Body = sb.ToString();
                mailMessage.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // Send the email
                smtpClient.Send(mailMessage);


                var JSONString = JsonConvert.SerializeObject(BookingId);
                return JSONString;
            }

            catch (Exception ex)
            {
                var JSONString = JsonConvert.SerializeObject(ex.Message.ToString());
                return JSONString;
            }
        }

        [HttpPost("insuranceenquiry")]
        public string InsuranceEnquiry([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                var data = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                string Full_Name = string.IsNullOrEmpty(data?.Full_Name?.Value) ? "" : data?.Full_Name?.Value;
                string Mobile_no = string.IsNullOrEmpty(data?.Mobile_no?.Value) ? "" : data?.Mobile_no?.Value;
                string Email = string.IsNullOrEmpty(data?.Email?.Value) ? "" : data?.Email?.Value;
                string City = string.IsNullOrEmpty(data?.City?.Value) ? "" : data?.City?.Value;
                string InsuranceId = string.IsNullOrEmpty(data?.InsuranceId?.Value) ? "" : data?.InsuranceId?.Value;
                string Passengertype = string.IsNullOrEmpty(data?.Passengertype?.Value) ? "" : data?.Passengertype?.Value;

                MexCoreWebAPI.Model.CIS.InsuranceEnquiry ib = new MexCoreWebAPI.Model.CIS.InsuranceEnquiry();
                ib.Full_Name = Full_Name;
                ib.Mobile_no = Mobile_no;
                ib.Email = Email;
                ib.City = City;
                ib.InsuranceId = InsuranceId;
                ib.Passengertype = Passengertype;

                sql.InsuranceEnquiries.Add(ib);
                sql.SaveChanges();

                int insuranceId = 0;

                if (InsuranceId != "")
                {
                    insuranceId = Convert.ToInt32(InsuranceId);
                }

                var travelInsurance = (from ti in sql.TravelInsurances.AsEnumerable()
                                       where ti.Id == insuranceId
                                       select new TravelInsurance
                                       {
                                           Duration = ti.Duration,
                                           CountryDescrtiption = ti.CountryDescrtiption,
                                           InsuranceType = ti.InsuranceType,
                                           MedicalExpense = ti.MedicalExpense,
                                           SingleFee = ti.SingleFee,
                                           FamilyFee = ti.FamilyFee
                                       }).ToList();

                StringBuilder sb = new StringBuilder();

                sb.Append("<html xmlns='http://www.w3.org/1999/xhtml' lang='' xml: lang=''>");
                sb.Append("<head>");
                sb.Append("<title></title>");
                sb.Append("</head>");
                sb.Append("<body>");

                sb.Append("<table style= 'position:absolute;top:540px;left:52%; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                sb.Append("<tr>");
                sb.Append("<td><b>Enquirer Name</b> </td><td>" + Full_Name + "</td>");
                sb.Append("</tr><tr>");
                sb.Append("<td><b>Mobile Num</b> </td><td>" + Mobile_no + "</td>");
                sb.Append("</tr><tr>");
                sb.Append("<td><b>Email</b> </td><td>" + Email + "</td>");
                sb.Append("</tr><tr>");
                sb.Append("<td><b>Insurance Plan</b> </td><td>" + travelInsurance[0].CountryDescrtiption + "</td>");
                sb.Append("</tr><tr>");
                sb.Append("<td><b>Insurance Type</b> </td><td>" + travelInsurance[0].InsuranceType + "</td>");
                sb.Append("</tr><tr>");
                sb.Append("<td><b>Insurance Duration</b> </td><td>" + travelInsurance[0].Duration + "</td>");
                sb.Append("</tr><tr>");
                sb.Append("<td><b>Passenger Type</b> </td><td>" + Passengertype + "</td>");
                sb.Append("</tr></table>");

                sb.Append("</body>");
                sb.Append("<html>");

                SmtpClient smtpClient = new SmtpClient("mail.malikexpress.com");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("csi@malikexpress.com", "sF~359sm9");

                // Create the email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("csi@malikexpress.com");
                mailMessage.To.Add("ti@malikexpress.com"); //asimshahzad78@gmail.com coo@growbiztech.com"
                mailMessage.Subject = "New Insurance Enquiry";
                mailMessage.Body = sb.ToString();
                mailMessage.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // Send the email
                smtpClient.Send(mailMessage);

                var JSONString = JsonConvert.SerializeObject("your request initiated. our rep will contact you shortly.");
                return JSONString;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        [HttpPost("uploadimage")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadModel uploadModel)
        {
            if (uploadModel.File == null || uploadModel.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/paymentimages", uploadModel.File.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await uploadModel.File.CopyToAsync(stream);
            }

            try
            {
                // Set up SMTP client
                using (var client = new SmtpClient("smtp.yourserver.com"))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("username", "password");
                    client.EnableSsl = true;
                    client.Port = 587; // This is typically the port for SSL-enabled SMTP

                    // Create the email message
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("from@example.com"),
                        Subject = "Uploaded Image",
                        Body = $"Here is the uploaded image. Additional Info: {uploadModel.Info}",
                    };
                    mailMessage.To.Add("to@example.com");

                    // Attach the file
                    var attachment = new Attachment(path);
                    mailMessage.Attachments.Add(attachment);

                    // Send the email
                    await client.SendMailAsync(mailMessage);
                }

                return Ok(new { path });
            }
            catch (Exception ex)
            {
                // Return an error message if the email fails to send
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("calltoaction")]
        public string calltoaction([FromBody] dynamic info)
        {
            try
            {
                string json = Convert.ToString(info.ToString()).Replace(" ValueKind = ", "");
                var data = JsonConvert.DeserializeObject<dynamic>(json.ToString());

                string Full_Name = string.IsNullOrEmpty(data?.Full_Name?.Value) ? "" : data?.Full_Name?.Value;
                string Mobile_no = string.IsNullOrEmpty(data?.Mobile_no?.Value) ? "" : data?.Mobile_no?.Value;
                string Date = string.IsNullOrEmpty(data?.Date?.Value) ? "" : data?.Date?.Value;
                string Time = string.IsNullOrEmpty(data?.Time?.Value) ? "" : data?.Time?.Value;

                StringBuilder sb = new StringBuilder();

                sb.Append("<html xmlns='http://www.w3.org/1999/xhtml' lang='' xml: lang=''>");
                sb.Append("<head>");
                sb.Append("<title></title>");
                sb.Append("</head>");
                sb.Append("<body>");

                sb.Append("<table style= 'position:absolute;top:540px;left:52%; border-collapse: collapse; border: 1px solid #000;  width: 40%;'>");
                sb.Append("<tr>");
                sb.Append("<td><b>Enquirer Name</b> </td><td>" + Full_Name + "</td>");
                sb.Append("</tr><tr>");
                sb.Append("<td><b>Mobile Num</b> </td><td>" + Mobile_no + "</td>");
                sb.Append("</tr><tr>");
                sb.Append("<td><b>Preferred Date</b> </td><td>" + Date + "</td>");
                sb.Append("</tr><tr>");
                sb.Append("<td><b>Preferred Time</b> </td><td>" + Time + "</td>");
                sb.Append("</tr></table>");

                sb.Append("</body>");
                sb.Append("<html>");

                SmtpClient smtpClient = new SmtpClient("mail.malikexpress.com");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("csi@malikexpress.com", "sF~359sm9");

                // Create the email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("csi@malikexpress.com");
                mailMessage.To.Add("ti@malikexpress.com"); //asimshahzad78@gmail.com coo@growbiztech.com" ti@malikexpress.com
                mailMessage.Subject = "Call To Action";
                mailMessage.Body = sb.ToString();
                mailMessage.IsBodyHtml = true;

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                // Send the email
                smtpClient.Send(mailMessage);

                var JSONString = JsonConvert.SerializeObject("your request initiated. our rep will contact you shortly.");
                return JSONString;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


        [HttpGet("getinsdetails")]
        public string Details(int id)
        {
            var ins = (from x in sql.InsuranceBookings
                       join b in sql.TravelInsurances
                         on x.InsuranceCode equals b.Id into bookings
                       from r in bookings.DefaultIfEmpty()
                       where x.Id == id
                       orderby x.EntryDateTime descending
                       select new InsuranceBookings
                       {
                           Id = x.Id,
                           InsuranceCode = x.InsuranceCode,
                           FirstName = x.FirstName,
                           LastName = x.LastName,
                           ContactNumber = x.ContactNumber,
                           CNIC = x.CNIC,
                           BeneficiaryFirstName = x.BeneficiaryFirstName,
                           BeneficiaryLastName = x.BeneficiaryLastName,
                           BeneficiaryContactNumber = x.BeneficiaryContactNumber,
                           PackageName = r.CountryDescrtiption,
                           InsuranceType = r.InsuranceType,
                           MedicalExpense = r.MedicalExpense,
                           PassengerType = x.PassengerType,
                           Passport = x.PassportNumber,
                           DepartureDate = x.DepartureDate,
                           EmailAddress = x.EmailAddress,
                           BeneficiaryCNIC = x.BeneficiaryCNIC,
                           BeneficiaryAddress = x.BeneficiaryAddress,
                           BeneficiaryRelation = x.BeneficiaryRelation,
                           DOB = x.DOB,
                           PurposeOfVisit = x.PurposeOfVisit,
                           DiscountedPrice = x.DiscountedPrice,
                           ResidentialAddress = x.ResidentialAddress,
                           FirstCountry = x.FirstCountry,
                           CityofResidence = x.CityofResidence,
                           Vendor = x.Vendor,
                           PolicyNumber = x.PolicyNumber,
                           PaymentMessage = x.PaymentMessage,
                           NoOfDays = x.NoOfDays
                       }).ToList();

            foreach (var booking in ins)
            {
                DateTime depDate = Convert.ToDateTime(booking.DepartureDate);
                DateTime arrDate = Convert.ToDateTime(booking.ArrivalDate);

                TimeSpan timeDifference = arrDate - depDate;
                // Extract the total number of days from the TimeSpan
                booking.Duration = timeDifference.Days;
            }

            var JSONString = JsonConvert.SerializeObject(ins);
            return JSONString;
        }



        [HttpGet("getpdfdetails")]
        public IActionResult PDFDetails(int BookingId)
        {
            var ins = (from x in sql.InsuranceBookings
                       join b in sql.TravelInsurances
                         on x.InsuranceCode equals b.Id into bookings
                       from r in bookings.DefaultIfEmpty()
                       where x.Id == BookingId
                       orderby x.EntryDateTime descending
                       select new InsuranceBookings
                       {
                           Id = x.Id,
                           InsuranceCode = x.InsuranceCode,
                           FirstName = x.FirstName,
                           LastName = x.LastName,
                           ContactNumber = x.ContactNumber,
                           CNIC = x.CNIC,
                           BeneficiaryFirstName = x.BeneficiaryFirstName,
                           BeneficiaryLastName = x.BeneficiaryLastName,
                           BeneficiaryContactNumber = x.BeneficiaryContactNumber,
                           PackageName = r.CountryDescrtiption,
                           InsuranceType = r.InsuranceType,
                           MedicalExpense = r.MedicalExpense,
                           PassengerType = x.PassengerType,
                           Passport = x.PassportNumber,
                           DepartureDate = x.DepartureDate,
                           EmailAddress = x.EmailAddress,
                           BeneficiaryCNIC = x.BeneficiaryCNIC,
                           BeneficiaryAddress = x.BeneficiaryAddress,
                           BeneficiaryRelation = x.BeneficiaryRelation,
                           DOB = x.DOB,
                           PurposeOfVisit = x.PurposeOfVisit,
                           DiscountedPrice = x.DiscountedPrice,
                           ResidentialAddress = x.ResidentialAddress,
                           FirstCountry = x.FirstCountry,
                           CityofResidence = x.CityofResidence,
                           Vendor = x.Vendor,
                           PolicyNumber = x.PolicyNumber,
                           PaymentMessage = x.PaymentMessage,
                           NoOfDays = x.NoOfDays
                       }).FirstOrDefault();

            var paxList = (from x in sql.InsuranceBookings
                           join b in sql.InsurancePaxs
                             on x.Id equals b.InsuranceBookingId into bookings
                           from r in bookings.DefaultIfEmpty()
                           where (r.InsuranceBookingId == BookingId && x.Vendor == "CSI")
                           orderby x.EntryDateTime descending
                           select new InsurancePax
                           {
                               FullName = r.FullName,
                               DOB = r.DOB.ToString(),
                               PassportNo = r.PassportNo,
                               Type = r.Type
                           }).ToList();

            var travelInsurance = (from ti in sql.TravelInsurances.AsEnumerable()
                                   where ti.Id == ins.InsuranceCode
                                   select new TravelInsurance
                                   {
                                       Duration = ti.Duration,
                                       CountryDescrtiption = ti.CountryDescrtiption,
                                       InsuranceType = ti.InsuranceType,
                                       MedicalExpense = ti.MedicalExpense,
                                       SingleFee = ti.SingleFee,
                                       FamilyFee = ti.FamilyFee
                                   }).ToList();

            // Generate PDF
            var document = CreatePdfDocument(ins, paxList, travelInsurance);

            // Convert to byte array
            byte[] pdfFile;
            using (var stream = new MemoryStream())
            {
                var pdfRenderer = new PdfDocumentRenderer();
                pdfRenderer.Document = document;
                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(stream, false);
                pdfFile = stream.ToArray();
            }

            return File(pdfFile, "application/pdf", $"InsuranceBooking_{BookingId}.pdf");
        }

        private Document CreatePdfDocument(InsuranceBookings ins, List<InsurancePax> paxList, List<TravelInsurance> travelInsurance)
        {
            var document = new Document();
            var section = document.AddSection();

            section.AddParagraph("Malik Air Express").Format.Font.Size = 24;
            section.AddParagraph($"NO: INV-{DateTime.Now.Year}-{ins.Id}");
            section.AddParagraph($"Policy Number: {ins.PolicyNumber}");
            section.AddParagraph($"Plan: {travelInsurance[0].CountryDescrtiption}");
            section.AddParagraph($"Visit Duration: {CalculateDays(Convert.ToDateTime(ins.DepartureDate), Convert.ToDateTime(ins.ArrivalDate))} days");
            section.AddParagraph($"Insurance Type: {ins.InsuranceType}");
            section.AddParagraph($"Medical Expense: {ins.MedicalExpense}");
            section.AddParagraph($"Passenger Type: {ins.PassengerType}");
            section.AddParagraph($"Payment Status: {ins.PaymentMessage}");

            section.AddParagraph("\nPassenger Details:");
            var table = section.AddTable();
            table.Borders.Width = 0.75;

            var column = table.AddColumn(Unit.FromCentimeter(4));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(Unit.FromCentimeter(8));
            column.Format.Alignment = ParagraphAlignment.Center;

            var row = table.AddRow();
            row.Cells[0].AddParagraph("Field");
            row.Cells[1].AddParagraph("Value");

            row = table.AddRow();
            row.Cells[0].AddParagraph("First Name");
            row.Cells[1].AddParagraph(ins.FirstName);

            row = table.AddRow();
            row.Cells[0].AddParagraph("Last Name");
            row.Cells[1].AddParagraph(ins.LastName);

            row = table.AddRow();
            row.Cells[0].AddParagraph("Departure Date");
            row.Cells[1].AddParagraph(ins.DepartureDate.ToString());

            row = table.AddRow();
            row.Cells[0].AddParagraph("Duration");
            row.Cells[1].AddParagraph(ins.NoOfDays.ToString());

            row = table.AddRow();
            row.Cells[0].AddParagraph("Passport Number");
            row.Cells[1].AddParagraph(ins.Passport);

            row = table.AddRow();
            row.Cells[0].AddParagraph("CNIC");
            row.Cells[1].AddParagraph(ins.CNIC);

            row = table.AddRow();
            row.Cells[0].AddParagraph("DOB");
            row.Cells[1].AddParagraph(ins.DOB.ToString());

            row = table.AddRow();
            row.Cells[0].AddParagraph("Mobile No.");
            row.Cells[1].AddParagraph(ins.ContactNumber);

            row = table.AddRow();
            row.Cells[0].AddParagraph("Purpose");
            row.Cells[1].AddParagraph(ins.PurposeOfVisit);

            row = table.AddRow();
            row.Cells[0].AddParagraph("Address");
            row.Cells[1].AddParagraph(ins.ResidentialAddress);

            row = table.AddRow();
            row.Cells[0].AddParagraph("City");
            row.Cells[1].AddParagraph(ins.CityofResidence);

            row = table.AddRow();
            row.Cells[0].AddParagraph("Email");
            row.Cells[1].AddParagraph(ins.EmailAddress);

            row = table.AddRow();
            row.Cells[0].AddParagraph("Country to Visit");
            row.Cells[1].AddParagraph(ins.FirstCountry);

            if (paxList.Count > 0)
            {
                section.AddParagraph("\nFamily Details:");
                var familyTable = section.AddTable();
                familyTable.Borders.Width = 0.75;

                familyTable.AddColumn(Unit.FromCentimeter(4));
                familyTable.AddColumn(Unit.FromCentimeter(3));
                familyTable.AddColumn(Unit.FromCentimeter(4));
                familyTable.AddColumn(Unit.FromCentimeter(3));

                var familyRow = familyTable.AddRow();
                familyRow.Cells[0].AddParagraph("Full Name");
                familyRow.Cells[1].AddParagraph("DOB");
                familyRow.Cells[2].AddParagraph("Passport No.");
                familyRow.Cells[3].AddParagraph("Pax Type");

                foreach (var pax in paxList)
                {
                    familyRow = familyTable.AddRow();
                    familyRow.Cells[0].AddParagraph(pax.FullName);
                    familyRow.Cells[1].AddParagraph(pax.DOB);
                    familyRow.Cells[2].AddParagraph(pax.PassportNo);
                    familyRow.Cells[3].AddParagraph(pax.Type);
                }
            }

            section.AddParagraph("\nBeneficiary Details:");
            var beneficiaryTable = section.AddTable();
            beneficiaryTable.Borders.Width = 0.75;

            beneficiaryTable.AddColumn(Unit.FromCentimeter(4));
            beneficiaryTable.AddColumn(Unit.FromCentimeter(8));

            var beneficiaryRow = beneficiaryTable.AddRow();
            beneficiaryRow.Cells[0].AddParagraph("Field");
            beneficiaryRow.Cells[1].AddParagraph("Value");

            beneficiaryRow = beneficiaryTable.AddRow();
            beneficiaryRow.Cells[0].AddParagraph("First Name");
            beneficiaryRow.Cells[1].AddParagraph(ins.BeneficiaryFirstName);

            beneficiaryRow = beneficiaryTable.AddRow();
            beneficiaryRow.Cells[0].AddParagraph("Last Name");
            beneficiaryRow.Cells[1].AddParagraph(ins.BeneficiaryLastName);

            beneficiaryRow = beneficiaryTable.AddRow();
            beneficiaryRow.Cells[0].AddParagraph("Mobile No.");
            beneficiaryRow.Cells[1].AddParagraph(ins.BeneficiaryContactNumber);

            beneficiaryRow = beneficiaryTable.AddRow();
            beneficiaryRow.Cells[0].AddParagraph("CNIC");
            beneficiaryRow.Cells[1].AddParagraph(ins.BeneficiaryCNIC);

            beneficiaryRow = beneficiaryTable.AddRow();
            beneficiaryRow.Cells[0].AddParagraph("Address");
            beneficiaryRow.Cells[1].AddParagraph(ins.BeneficiaryAddress);

            beneficiaryRow = beneficiaryTable.AddRow();
            beneficiaryRow.Cells[0].AddParagraph("Relation");
            beneficiaryRow.Cells[1].AddParagraph(ins.BeneficiaryRelation);

            section.AddParagraph("\nAmount Details:");
            var amountTable = section.AddTable();
            amountTable.Borders.Width = 0.75;

            amountTable.AddColumn(Unit.FromCentimeter(8));
            amountTable.AddColumn(Unit.FromCentimeter(4));

            var amountRow = amountTable.AddRow();
            amountRow.Cells[0].AddParagraph("Total Amount");
            amountRow.Cells[1].AddParagraph($"PKR {ins.DiscountedPrice}");

            section.AddParagraph("\nEmail: sales-isb@malikexpress.com");
            section.AddParagraph("Thank You!");
            section.AddParagraph($"Date: {DateTime.Now}");

            return document;
        }

        [NonAction]
        private int CalculateDays(DateTime startDate, DateTime endDate)
        {
            return (endDate - startDate).Days;
        }
        [HttpGet("getuicdetails")]
        public string UICDetails(int id)
        {
            var insuranceList = (from x in sql.UicBookings
                                 where x.Id == id
                                 orderby x.EntryDateTime descending
                                 select new UicBooking
                                 {
                                     PDays = x.PDays,
                                     Duration = x.Duration,
                                     DurationUnit = x.DurationUnit,
                                     AreaShortCode = x.AreaShortCode,
                                     Area = x.Area,
                                     Plan = x.Plan,
                                     PlanType = x.PlanType,
                                     Premium = x.Premium,
                                     AgePremIncrease = x.AgePremIncrease,
                                     GrossPremium = x.GrossPremium,
                                     AdvanceTax = x.AdvanceTax,
                                     TravelerName = x.TravelerName,
                                     TravelerNICNo = x.TravelerNICNo,
                                     TravelerNTNNo = x.TravelerNTNNo,
                                     IsTaxPayer = x.IsTaxPayer,
                                     TotalPayablePremium = x.TotalPayablePremium,
                                     TravelerDOB = x.TravelerDOB,
                                     TravelerAge = x.TravelerAge,
                                     Covid = x.Covid,
                                     MedicalExpense = x.MedicalExpense,
                                     DepartureDate = x.DepartureDate,
                                     ArrivalDate = x.ArrivalDate,
                                     FirstCountry = x.FirstCountry,
                                     SecondCountry = x.SecondCountry,
                                     ContactNumber = x.ContactNumber,
                                     CityofResidence = x.CityofResidence,
                                     ResidentialAddress = x.ResidentialAddress,
                                     EmailAddress = x.EmailAddress,
                                     BeneficiaryCNIC = x.BeneficiaryCNIC,
                                     BeneficiaryAddress = x.BeneficiaryAddress,
                                     BeneficiaryRelation = x.BeneficiaryRelation,
                                     DiscountedPrice = x.DiscountedPrice,
                                     PaymentStatus = x.PaymentStatus,
                                     PaymentMessage = x.PaymentMessage,
                                     PolicyNumber = x.PolicyNumber,
                                     Passport = x.Passport,
                                     BeneficiaryFirstName = x.BeneficiaryFirstName,
                                     BeneficiaryLastName = x.BeneficiaryLastName,
                                     Vendor = "UIC"
                                 }).ToList();


            foreach (var booking in insuranceList)
            {
                DateTime depDate = Convert.ToDateTime(booking.DepartureDate);
                DateTime arrDate = Convert.ToDateTime(booking.ArrivalDate);

                TimeSpan timeDifference = arrDate - depDate;
                // Extract the total number of days from the TimeSpan
                booking.Duration = timeDifference.Days;
            }

            var JSONString = JsonConvert.SerializeObject(insuranceList);
            return JSONString;

        }

        [HttpGet("getpaxlist")]
        public string PaxList(int id)
        {
            var insuranceList = (from x in sql.InsuranceBookings
                                 join b in sql.InsurancePaxs
                                   on x.Id equals b.InsuranceBookingId into bookings
                                 from r in bookings.DefaultIfEmpty()
                                 where (r.InsuranceBookingId == id && r.Vendor == "CSI")
                                 orderby x.EntryDateTime descending
                                 select new
                                 {
                                     FullName = r.FullName,
                                     DOB = r.DOB,
                                     PassportNo = r.PassportNo,
                                     PaxType = r.Type
                                 }).ToList();

            var JSONString = JsonConvert.SerializeObject(insuranceList);
            return JSONString;
        }

        [HttpGet("getuicpaxlist")]
        public string UICPaxList(int id)
        {
            var insuranceList = (from x in sql.UicBookings
                                 join b in sql.InsurancePaxs
                                   on x.Id equals b.InsuranceBookingId into bookings
                                 from r in bookings.DefaultIfEmpty()
                                 where (r.InsuranceBookingId == id && r.Vendor == "UIC")
                                 orderby x.EntryDateTime descending
                                 select new
                                 {
                                     FullName = r.FullName,
                                     DOB = r.DOB,
                                     PassportNo = r.PassportNo,
                                     PaxType = r.Type
                                 }).ToList();

            var JSONString = JsonConvert.SerializeObject(insuranceList);
            return JSONString;
        }
    }

    public class ImageUploadModel
    {
        public IFormFile File { get; set; }
        public dynamic Info { get; set; }
    }
    public class InsuranceBookings
    {
        public int Id { get; set; }
        public int? InsuranceCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ContactNumber { get; set; }
        public string? CNIC { get; set; }
        public string? BeneficiaryFirstName { get; set; }
        public string? BeneficiaryLastName { get; set; }
        public string? BeneficiaryContactNumber { get; set; }
        public string? BeneficiaryRelation { get; set; }
        public string? BeneficiaryCNIC { get; set; }
        public string? BeneficiaryAddress { get; set; }
        public string? PackageName { get; set; }
        public string? InsuranceType { get; set; }
        public decimal? MedicalExpense { get; set; }
        public string? PassengerType { get; set; }
        public string? Passport { get; set; }
        public string? DepartureDate { get; set; }
        public string? ArrivalDate { get; set; }
        public bool? PaymentReceived { get; set; }
        public string? EmailAddress { get; set; }
        public string? PurposeOfVisit { get; set; }
        public string? DOB { get; set; }
        public string? DiscountedPrice { get; set; }
        public string? ResidentialAddress { get; set; }
        public int Duration { get; set; }
        public DateTime? BookingTime { get; set; }
        public string? OriginalPrice { get; set; }
        public string? CityofResidence { get; set; }
        public string? FirstCountry { get; set; }
        public string? PolicyNumber { get; set; }
        public string? Vendor { get; set; }
        public string? AreaShortCode { get; set; }
        public string? NoOfDays { get; set; }
        public string? PaymentMessage { get; set; }
    }

    public class UICResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string ReferenceID { get; set; }
        public string PolicyNo { get; set; }
        public string TravelerName { get; set; }
        public string Amount { get; set; }
        public string AdvanceTax { get; set; }
        public string TravelerEmail { get; set; }
        public string PolicyPrintUrl { get; set; }
    }
}


//using (var client = new HttpClient())
//{
//    var url = "http://travelapi.theunitedsoftware.com/API/Travel/GenerateUWDocumentWithCovid";
//    var basicAuthUsername = "TravelAPI";
//    var basicAuthPassword = "85aeezx305ae285f5a7959f89a8af9caa5f57";
//    var authToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{basicAuthUsername}:{basicAuthPassword}"));
//    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

//    var requestBody = new FormUrlEncodedContent(new[]
//    {
//            new KeyValuePair<string, string>("UserName", "API_MALIK.EXPRESS"),
//            new KeyValuePair<string, string>("Password", "API!UIC**10596!"),
//            new KeyValuePair<string, string>("AreaShortCode", "SCH"),
//            new KeyValuePair<string, string>("TravelDate", DateTime.Parse(ins.DepartureDate).ToString("yyyy-MM-dd")),
//            new KeyValuePair<string, string>("TravelerName", $"{ins.FirstName} {ins.LastName}"),
//            new KeyValuePair<string, string>("NoOfDays", ins.Duration.ToString()),
//            new KeyValuePair<string, string>("DOB", DateTime.Parse(ins.DOB).ToString("yyyy-MM-dd")),
//            new KeyValuePair<string, string>("PassportNo", ins.Passport),
//            new KeyValuePair<string, string>("NICNo", ins.CNIC),
//            new KeyValuePair<string, string>("Address", ins.ResidentialAddress),
//            new KeyValuePair<string, string>("ContactNo", ins.ContactNumber),
//            new KeyValuePair<string, string>("Country", "pakistan"),
//            new KeyValuePair<string, string>("Remarks", "Test"),
//            new KeyValuePair<string, string>("PlanType", "s"),
//            new KeyValuePair<string, string>("Plan", ins.InsuranceType),
//            new KeyValuePair<string, string>("Premium", totalCost.ToString()),
//            new KeyValuePair<string, string>("ntnno", ""),
//            new KeyValuePair<string, string>("TravelerEmail", ins.EmailAddress),
//            new KeyValuePair<string, string>("SpouseName", SpouseName),
//            new KeyValuePair<string, string>("SpousePassport", SpousePassport),
//            new KeyValuePair<string, string>("Child1Name", Child1Name),
//            new KeyValuePair<string, string>("Child1DOB", !string.IsNullOrEmpty(Child1DOB) ? DateTime.Parse(Child1DOB).ToString("yyyy-MM-dd") : ""),
//            new KeyValuePair<string, string>("Child1Passport", Child1Passport),
//            new KeyValuePair<string, string>("Child2Name", Child2Name),
//            new KeyValuePair<string, string>("Child2DOB", !string.IsNullOrEmpty(Child2DOB) ? DateTime.Parse(Child2DOB).ToString("yyyy-MM-dd") : ""),
//            new KeyValuePair<string, string>("Child2Passport", Child2Passport),
//            new KeyValuePair<string, string>("Child3Name", Child3Name),
//            new KeyValuePair<string, string>("Child3DOB", !string.IsNullOrEmpty(Child3DOB) ? DateTime.Parse(Child3DOB).ToString("yyyy-MM-dd") : ""),
//            new KeyValuePair<string, string>("Child3Passport", Child3Passport),
//            new KeyValuePair<string, string>("isRequestPolicy", "False"),
//            new KeyValuePair<string, string>("Covid", ""),
//            new KeyValuePair<string, string>("SrvSrce", "UIC"),
//            new KeyValuePair<string, string>("ReferenceNo", ""),
//            new KeyValuePair<string, string>("ESystemName", ""),
//            new KeyValuePair<string, string>("EUserName", ""),
//            new KeyValuePair<string, string>("BeneficiaryName", $"{ins.BeneficiaryFirstName} {ins.BeneficiaryLastName}"),
//            new KeyValuePair<string, string>("Relationship", ins.BeneficiaryRelation),
//            new KeyValuePair<string, string>("SpouseDOB", !string.IsNullOrEmpty(SpouseDOB) ? DateTime.Parse(SpouseDOB).ToString("yyyy-MM-dd") : "")
//        });

//    try
//    {
//        HttpResponseMessage response = await client.PostAsync(url, requestBody);
//        response.EnsureSuccessStatusCode();
//        string responseBody = await response.Content.ReadAsStringAsync();
//        apiResponses = System.Text.Json.JsonSerializer.Deserialize<List<UICResponse>>(responseBody);
//    }
//    catch (HttpRequestException e)
//    {
//        Console.WriteLine("\nException Caught!");
//        Console.WriteLine("Message :{0} ", e.Message);
//    }
//}
