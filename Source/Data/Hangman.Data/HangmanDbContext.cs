using System.Data.Entity;
using Hangman.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Hangman.Data
{
    public class HangmanDbContext : IdentityDbContext<User>, IHangmanDbContext
    {
        public HangmanDbContext()
            : base("HangmanConnection", throwIfV1Schema: false)
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<IdentityUser>().ToTable("Users");

        //    base.OnModelCreating(modelBuilder);
        //}

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Game> Games { get; set; }

        //public virtual IDbSet<GameState> GameState { get; set; }

        public virtual IDbSet<Score> Score { get; set; }

        public virtual IDbSet<Word> Words { get; set; }

        public static HangmanDbContext Create()
        {
            return new HangmanDbContext();
        }
    }
}
