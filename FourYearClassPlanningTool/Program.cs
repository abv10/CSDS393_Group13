using FourYearClassPlanningTool.Areas.Identity.Data;
using FourYearClassPlanningTool.Data;
using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
          
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    RequirementsContext requirementsContext = 
                        new RequirementsContext(services.GetRequiredService<DbContextOptions<RequirementsContext>>());
                    UsersContext usersContext =
                        new UsersContext(services.GetRequiredService<DbContextOptions<UsersContext>>());
                    AccountsContext accountsContext =
                        new AccountsContext(services.GetRequiredService<DbContextOptions<AccountsContext>>());
                    SeedRequirementsData.Initialize(requirementsContext);
                    SeedUsersData.Initialize(usersContext);
                    SeedDataAccountsDb.Initialize(accountsContext);

                }
                catch(Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured seeding the DB.");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
