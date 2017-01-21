using System.Data.Entity;
using Hangman.Data;

namespace Hangman.Web
{
    public class DbConfig
    {
        public static void Intialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HangmanDbContext, Data.Migrations.Configuration>());
        }
    }
}