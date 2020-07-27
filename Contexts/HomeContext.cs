using ExamOne.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamOne.Contexts
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options) : base(options){}

            public DbSet<User> Users {get; set;}

            public DbSet<Venture> Ventures {get; set;}
            public DbSet<Rsvp> Rsvps {get; set;}
    }
}