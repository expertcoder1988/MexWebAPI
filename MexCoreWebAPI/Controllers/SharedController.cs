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

namespace MexCoreWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SharedController : ControllerBase
    {
        private readonly MexDbContext sql;

        public SharedController(MexDbContext context)
        {
            sql = context;
        }
        //string ipcc = ConfigurationManager.AppSettings["ipcc"].ToString();
        // GET: Shared




        [HttpGet("_CountriesList")]        
        public List<Country> _CountriesList(string countryid, string cityid)
        {
            // List<SelectListItem> lst = (from country in sql.Countries select new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() }).ToList();

            List<Country> lst = (from country in sql.Countries orderby country.CountryName select new Country() { CountryName = country.CountryName, CountryID = country.CountryID }).ToList();

          
            return lst;
        }

        [HttpPost("_CountriesList")]     
      
        public List<Country> CountriesList(string countryid, string cityid)
        {
            List<Country> lst = (from country in sql.Countries orderby country.CountryName ascending select new Country() { CountryName = country.CountryName, CountryID = country.CountryID }).ToList();
            

            return lst;
        }


        [HttpPost]
        [ActionName("CitiesList")]
       
        public List<City> CitiesList(string id, string cityid)
        {
            List<City> cities = new List<City>();
            if (!string.IsNullOrEmpty(id))
            {
                int CountryID = Convert.ToInt32(id);
                if (!string.IsNullOrEmpty(cityid))
                {
                    int CityID = Convert.ToInt32(cityid);
                    cities = (from c in sql.Cities where c.countryID == CountryID select new City() { cityName = c.cityName, cityID = c.cityID }).ToList();
                   // cities.First(x => x.CityID == CityID) = true;
                }
                else
                {
                    cities = (from c in sql.Cities where c.countryID == CountryID select new City() { cityName = c.cityName, cityID = c.cityID }).ToList();
                }
            }

            return cities;
        }

    }
}