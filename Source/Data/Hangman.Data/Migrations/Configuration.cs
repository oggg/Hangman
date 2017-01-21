namespace Hangman.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<HangmanDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(HangmanDbContext context)
        {
            if (!context.Users.Any())
            {
                for (int i = 0; i < 3; i++)
                {
                    var userEmail = string.Format("user{0}@mail.com", i);
                    var userUserName = userEmail;
                    var userPassword = userEmail;
                    CreateUser(context, userEmail, userUserName, userPassword);
                }
            }
        }

        private void CreateUser(HangmanDbContext context, string userEmail, string userName, string userPassword)
        {
            var user = new User
            {
                UserName = userName,
                Email = userEmail
            };
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            var userCreateResult = userManager.Create(user, userPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
            GenerateUserStatistics(context);
        }

        private void GenerateUserStatistics(HangmanDbContext context)
        {
            var lastUserDefined = context.Users.Last().Id;
            //if ef has generated already the ids of the users in the Scores table -> comment the above line and use the one below
            //get all entries and set default values for statistics
            var random = new Random();
            var score = new Score
            {
                Id = lastUserDefined,
                LettersGussed = random.Next(10, 20),
                Lost = random.Next(20, 30),
                Won = random.Next(3, 15),
                WordsGussed = random.Next(10)
            };
        }
    }
}
