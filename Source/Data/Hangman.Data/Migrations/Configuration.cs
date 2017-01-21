namespace Hangman.Data.Migrations
{
    using System;
    using System.Collections.Generic;
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
                for (int i = 1; i <= 3; i++)
                {
                    var userEmail = string.Format("user{0}@mail.com", i);
                    var userUserName = userEmail;
                    var userPassword = userEmail;
                    CreateUsers(context, userEmail, userUserName, userPassword);
                }

                GenerateUserStatistics(context);
                GenerateCategoriesWords(context);
                context.SaveChanges();
            }
        }

        private void CreateUsers(HangmanDbContext context, string userEmail, string userName, string userPassword)
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
        }

        private void GenerateUserStatistics(HangmanDbContext context)
        {
            var allUsersDefined = context.Users.ToList();
            //if ef has generated already the ids of the users in the Scores table -> comment the above line and use the one below
            //get all entries and set default values for statistics
            var random = new Random();
            foreach (var user in allUsersDefined)
            {
                var score = new Score
                {
                    Id = user.Id,
                    LettersGussed = random.Next(10, 20),
                    Lost = random.Next(20, 30),
                    Won = random.Next(3, 15),
                    WordsGussed = random.Next(10),
                    User = user
                };

                context.Score.Add(score);
            }
        }

        private void GenerateCategoriesWords(HangmanDbContext context)
        {
            context.Categories.Add(new Category()
            {
                Name = "Animals",
                Words = new HashSet<Word>()
                {
                    new Word() { TheWord = "Lion", Description = "The king of all animals" },
                    new Word() { TheWord = "Elephant", Description = "It has a big proboscis" },
                    new Word() { TheWord = "Horse", Description = "You can use it for a ride" }
                }
            });
            context.Categories.Add(new Category()
            {
                Name = "Cars",
                Words = new HashSet<Word>()
                {
                    new Word() { TheWord = "Lamborghini", Description = "Italian superfast car, part og VW group" },
                    new Word() { TheWord = "Toyota", Description = "Japan's most selling car" },
                    new Word() { TheWord = "Mercedes", Description = "German's pride with a star in the logo" }
                }
            });
        }
    }
}
