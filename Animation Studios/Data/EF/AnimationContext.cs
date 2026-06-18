using Animation_Studios.Models;
using System.Data.Entity;

namespace Animation_Studios.Data.EF
{
    public class AnimationContext : DbContext
    {
        public AnimationContext()
            : base("name=AnimationStudiosDb")
        {
            // Create DB if not exists
            Database.SetInitializer(new CreateDatabaseIfNotExists<AnimationContext>());
        }

        public DbSet<Studio> Studios { get; set; }
        public DbSet<Show> Shows { get; set; }
    }
}
