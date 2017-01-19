using System.Data.Entity;
using Hangman.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Hangman.Data
{
    public class HangmanDbContext : IdentityDbContext<User>
    {
        public HangmanDbContext()
            : base("HangmanConnection", throwIfV1Schema: false)
        {
        }

        public static HangmanDbContext Create()
        {
            return new HangmanDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("Users");
        }
    }
}
