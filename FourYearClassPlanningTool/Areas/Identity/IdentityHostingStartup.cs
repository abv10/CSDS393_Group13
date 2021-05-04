using System;
using FourYearClassPlanningTool.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FourYearClassPlanningTool.Areas.Identity.IdentityHostingStartup))]
namespace FourYearClassPlanningTool.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AccountsContext>(options =>
                    options.UseLazyLoadingProxies().UseSqlServer(
                        context.Configuration.GetConnectionString("AccountsContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<AccountsContext>();
            });
        }
    }
}