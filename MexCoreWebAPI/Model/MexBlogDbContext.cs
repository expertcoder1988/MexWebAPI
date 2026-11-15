using Microsoft.EntityFrameworkCore;

namespace MexCoreWebAPI.Model
{
    public class MexBlogDbContext : DbContext
    {
        public MexBlogDbContext(DbContextOptions<MexBlogDbContext> options) : base(options) { }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
