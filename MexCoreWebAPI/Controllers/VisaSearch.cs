using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VISA.Models;

namespace MexCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class VisaSearch : ControllerBase
    {
        private readonly VisaDbContext db;

        public VisaSearch(VisaDbContext visadb)
        {
            db = visadb;
        }
        [HttpGet("CountryGet")]
        public string GetDataByCountry(string term)
        {
            VisaDetails details = new VisaDetails();
            details.Country = db.VisaCountries.Where(x => x.Name.StartsWith(term)).FirstOrDefault();
            if (details.Country != null)
            {
                details.visas = db.Visas.Where(x => x.CountryID == details.Country.Id).ToList();
                details.faqs = db.Faqs.Where(x => x.CountryID == details.Country.Id).ToList();
                details.document = db.VisaDocuments.Where(x => x.CountryID == details.Country.Id).ToList();
            }

            var json = System.Text.Json.JsonSerializer.Serialize(details);
            return json.ToString();
        }

    }
    public class VisaDetails
    {
        public VisaCountry Country { get; set; }
        public List<Visa> visas { get; set; }
        public List<Faq> faqs { get; set; }
        public List<VisaDocuments> document { get; set; }
    }
}
