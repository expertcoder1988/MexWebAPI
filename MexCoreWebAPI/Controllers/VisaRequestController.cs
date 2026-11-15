
using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VISA.Models;

namespace MexCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisaRequestController : Controller
    {
        private readonly VisaDbContext db;

        public VisaRequestController(VisaDbContext visadb)
        {
            db = visadb;
        }

        [HttpPost("AddVisaRequest")]
        public IActionResult AddRequest([FromForm] VisaRequest data, [FromForm] List<IFormFile> files )
        {
            VisaRequests request = new VisaRequests();
            request.VisaId = data.VisaId;
            request.CountryId = data.CountryId;
            request.FirstName = data.FirstName;
            request.MiddleName = data.MiddleName;
            request.LastName = data.LastName;
            request.DOB = data.DOB;
            request.BirthCountry = data.BirthCountry;
            request.Email = data.Email;
            request.Mobile = data.Mobile;
            request.BirthCity = data.BirthCity;
            request.BirthState = data.BirthState;
            request.Gender = data.Gender;
            request.HomeAddress1 = data.HomeAddress1;
            request.HomeAddress2 = data.HomeAddress2;
            request.PostalCode = data.PostalCode; 
            request.ResidenceContact = data.ResidenceContact;
            request.ResidenceCity = data.ResidenceCity;
            request.ResidenceCountry = data.ResidenceCountry;
            request.OfficeAddress1 = data.OfficeAddress1;
            request.OfficeAddress2 = data.OfficeAddress2;
            request.OfficePostalCode = data.OfficePostalCode;
            request.OfficeContact = data.OfficeContact;
            request.OfficeCity = data.OfficeCity;
            request.OfficeCountry = data.OfficeCountry;
            request.AppliedDate = DateTime.Now;
            request.IsOpened = 0;
            db.VisaRequests.Add(request);
            db.SaveChanges();


            foreach (var item in files)
            {
                var model = new VisaRequestDocuments();
                model.VisaId = request.VisaId;
                model.CountryId = request.CountryId;
                model.VisaRequestId = request.Id;

                if (item != null || item.Length != 0)
                {
                    using var ms = new MemoryStream();
                    item.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    var fileExtension = Path.GetExtension(item.FileName);
                    model.DocumentName = item.FileName;
                    var contentType = item.ContentType;
                    model.ContentType = item.ContentType;
                    model.FileExtension = fileExtension;
                    model.DocumentImage = fileBytes;
                }

                db.VisaRequestDocuments.Add(model);
                db.SaveChanges();
            }
            return Ok();
        }
    }
}
