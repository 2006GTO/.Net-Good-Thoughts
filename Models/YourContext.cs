using bright_idea.Models;
using Microsoft.EntityFrameworkCore;
 
namespace bright_idea.Models
{
    public class YourContext : DbContext
    {
        public YourContext(DbContextOptions<YourContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<Idea> ideas { get; set; }
        public DbSet<Like> likes { get; set; }

    }
}