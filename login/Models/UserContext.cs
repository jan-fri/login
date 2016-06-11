using System.Data.Entity;

namespace login.Models
{
    public class UserContext:DbContext
    {
        public UserContext():base("UserContext")
        { }

        public DbSet<User> Users { get; set; }
    }
}