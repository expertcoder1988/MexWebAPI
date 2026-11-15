using System.ComponentModel.DataAnnotations.Schema;

namespace MexCoreWebAPI.Model
{
    [Table("Post")]
    public class Post
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? DefaultImage { get; set; }
        public bool? IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? isDeleted { get; set; }
        public string? VideoUrl { get; set; }
        public string? ShortDescriptions { get; set; }
        public string? Tags { get; set; }
        public string? MetaDescription { get; set; }
        public string? Author { get; set; }
        public string? Keywords { get; set; }
    }
}
