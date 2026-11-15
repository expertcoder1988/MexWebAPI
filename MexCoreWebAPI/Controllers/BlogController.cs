using MexCoreWebAPI.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MexCoreWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("ReactPolicy")]
    public class BlogController : Controller
    {

        private readonly MexBlogDbContext db;

        public BlogController(MexBlogDbContext context)
        {
            db = context;
        }

        [HttpGet("list")]
        public string List()
        {
            var lst = (from c in db.Posts where c.isDeleted == false select c).OrderByDescending(c => c.PublishedDate).ToList();

            string json = JsonConvert.SerializeObject(lst);

            return json;
        }

        [HttpGet("post")]
        //[Route("~/post/{postId:int}/{slug}", Name = "postDetailRoute")]
        public string Post(string term)
        {
            var blog = (from c in db.Posts where c.Slug == term && c.isDeleted == false select c).ToList();
            string json = JsonConvert.SerializeObject(blog);

            return json;
        }

        //public string Post(int PostId)
        //{
        //    var blog = (from c in db.Posts where c.PostId == PostId && c.isDeleted == false select c).ToList();
        //    string json = JsonConvert.SerializeObject(blog);

        //    return json;
        //}

        [HttpGet("image")]
        public IActionResult Image(string img)
        {
            var image = "C:\\inetpub\\wwwroot\\mexblog\\uploads\\post\\" + img;
            //string path = image.Path; // could be any absolute path anywhere on your server, for example c:\foo\bar.jpg
            string contentType = "image/jpg"; // image.ContentType; // for example image/jpg
            return File(image, contentType);
        }
    }
}
