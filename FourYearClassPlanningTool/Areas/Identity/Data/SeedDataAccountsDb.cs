using FourYearClassPlanningTool.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Areas.Identity.Data
{
    public static class SeedDataAccountsDb
    {
        public static bool reset = true;
        public static void Initialize(AccountsContext context)
        {
            if (reset)
            {
                context.RemoveRange(context.Users);
                context.SaveChanges();
                reset = false;
            }

            if (!context.Users.Any())
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                var configuration = builder.Build();

                var parentPath = configuration.GetConnectionString("SeedData");
                string line;
                var file = new System.IO.StreamReader(parentPath + @"\Users\SeedDataForUsersDatabaseUsers.csv");
                var firstLine = true;
                var users = new List<IdentityUser>();
                while ((line = file.ReadLine()) != null)
                {
                    if (firstLine == false)
                    {
                        var split = line.Split(',');
                        var user = new IdentityUser()
                        {
                            Email = split[0],
                            UserName = split[0],
                            LockoutEnabled = true,
                            //PhoneNumberConfirmed = true,
                            EmailConfirmed = true,
                            NormalizedEmail = split[0].ToUpper(),
                            NormalizedUserName = split[0].ToUpper(),
                        };
                        PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
                        var hashed = passwordHasher.HashPassword(user, split[1]);
                        user.PasswordHash = hashed;

                        users.Add(user);
                    }
                    else
                    {
                        firstLine = false;
                    }
                }

                context.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
