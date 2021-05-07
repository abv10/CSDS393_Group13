using FourYearClassPlanningTool.Controllers;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class CourseImportingControllerTests
    {
        private bool needReset = true;
        private UsersContext _usersContext;
        private IConfigurationRoot _configuration;
        private DbContextOptions<UsersContext> _optionsUser;
        private CourseImportingController controller;

        public CourseImportingController CreateCourseImportingControllerAs(string userName) 
        {
            var fakeContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity(userName);
            var fakeUser = new ClaimsPrincipal(fakeIdentity);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = fakeUser
                }
            };

            controller = new CourseImportingController(_usersContext)
            {
                ControllerContext = context
            };
            return controller;
        }

        [TestInitialize]
        public void SetUp()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            _configuration = builder.Build();
            _optionsUser = new DbContextOptionsBuilder<UsersContext>().UseLazyLoadingProxies().UseSqlServer(_configuration.GetConnectionString("UsersContext")).Options;
            _usersContext = new UsersContext(_optionsUser);

            if (needReset)
            {
                SeedUsersData.reset = true;
                needReset = false;
            }
            else
            {
                SeedUsersData.reset = false;
            }
            SeedUsersData.Initialize(_usersContext);

            controller = CreateCourseImportingControllerAs("abv10@case.edu");
        }

        [TestMethod]
        public void TestIndex()
        {
            var result = controller.Index() as ViewResult;
            var viewName = result.ViewName;
            Assert.IsTrue(result.ViewData.Model is IEnumerable<FourYearClassPlanningTool.Models.Users.Entities.Course>);
        }

        [TestMethod]
        public void TestAdd()
        {
            
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });
            controller.Add("CSDS293");
            var course = user.CompletedCourses.Where(c => c.CourseId.Equals("CSDS293")).FirstOrDefault();
            Assert.IsTrue(course.CourseId == "CSDS293");
        }
    }
}
