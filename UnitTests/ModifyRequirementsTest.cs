using FourYearClassPlanningTool.Controllers;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using FourYearClassPlanningTool.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Claims;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class ModifyRequirements
    {
        private bool needReset = true;
        private RequirementsContext _reqContext;
        private UsersContext _usersContext;
        private IConfigurationRoot _configuration;
        private DbContextOptions<RequirementsContext> _optionsReq;
        private DbContextOptions<UsersContext> _optionsUser;

        public ModifyRequirementsController CreateModifyRequirementsControllerAs(string userName)
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

            var controller = new ModifyRequirementsController(_reqContext, _usersContext);
            controller.ControllerContext = context;
            return controller;
        }

        [TestInitialize]
        public void SetUp()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            _configuration = builder.Build();

            _optionsReq = new DbContextOptionsBuilder<RequirementsContext>().UseLazyLoadingProxies().UseSqlServer(_configuration.GetConnectionString("RequirementsContext")).Options;
            _optionsUser = new DbContextOptionsBuilder<UsersContext>().UseLazyLoadingProxies().UseSqlServer(_configuration.GetConnectionString("UsersContext")).Options;
            _reqContext = new RequirementsContext(_optionsReq);
            _usersContext = new UsersContext(_optionsUser);

            if (needReset)
            {
                SeedUsersData.reset = true;
                SeedRequirementsData.reset = true;
                needReset = false;
            }
            else
            {
                SeedUsersData.reset = false;
                SeedRequirementsData.reset = false;
            }
            SeedRequirementsData.Initialize(_reqContext);
            SeedUsersData.Initialize(_usersContext);
        }

        [TestMethod]
        public void NullUserRequiresSignIn()
        {
            var controller = CreateModifyRequirementsControllerAs("nulluser");
            var result = controller.Index() as ViewResult;
            Assert.IsTrue(result.ViewName == "NotAdmin");
        }


        [TestMethod]
        public void NonAdminSignIn()
        {
            var controller = CreateModifyRequirementsControllerAs("abv10@case.edu");
            var result = controller.Index() as ViewResult;
            Assert.IsTrue(result.ViewName == "NotAdmin");
        }

        [TestMethod]
        public void AdminSignIn()
        {
            var controller = CreateModifyRequirementsControllerAs("admin@case.edu");
            var result = controller.Index() as ViewResult;
            Assert.IsTrue(result.ViewName == "Index");
        }

        public void testModifyDegree()
        {
            var controller = CreateModifyRequirementsControllerAs("admin@case.edu");
            var result = controller.Modify() as ViewResult;
            Assert.IsTrue(result.ViewName == "Modify");
        }

        public void testRemoveCourse()
        {
            var controller = CreateModifyRequirementsControllerAs("admin@case.edu");
            var result = controller.Modify() as ModifyController;
            var degree = _reqContext.Find<Degree>("CSAI");
            int numCourses = degree.Courses.Count;
            foreach(CourseGroup cg in degree.CourseGroups)
            {
                numCourses += cg.Courses.Count;
            }

            result.Remove("CSAI", "CSDS343");

            int resultNumCourses = degree.Courses.Count;
            foreach (CourseGroup cg in degree.CourseGroups)
            {
                resultNumCourses += cg.Courses.Count;
            }

            Assert.IsTrue(numCourses == resultNumCourses + 1);


        }
    }
}