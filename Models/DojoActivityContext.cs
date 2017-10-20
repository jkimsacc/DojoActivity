using Microsoft.EntityFrameworkCore;

namespace DojoActivity.Models
{
    public class DojoActivityContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public DojoActivityContext(DbContextOptions<DojoActivityContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}