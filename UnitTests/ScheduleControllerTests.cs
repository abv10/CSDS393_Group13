using FourYearClassPlanningTool.Controllers;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Requirements;
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
    public class ScheduleControllerTests
    {
        private bool needReset = true;
        private RequirementsContext _reqContext;
        private UsersContext _usersContext;
        private ScheduleService _service;
        private IConfigurationRoot _configuration;
        private DbContextOptions<RequirementsContext> _optionsReq;
        private DbContextOptions<UsersContext> _optionsUser;

        public ScheduleController CreateScheduleControllerAs(string userName)
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

            var controller = new ScheduleController(_usersContext, _service);
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
            _service = new ScheduleService(_reqContext, _usersContext);

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
        public void IndexReturnsIndex()
        {
            var controller = new ScheduleController(_usersContext, _service);
            var result = controller.Index() as ViewResult;
            Assert.IsTrue(result.ViewName == "Index");
        }

        [TestMethod]
        public void SchedulesReturnsNullUserWhenUserIsNull()
        {
            var controller = CreateScheduleControllerAs("");
            var result = controller.Schedules() as ViewResult;
            Assert.IsTrue(result.ViewName == "NullUser");
        }

        [TestMethod]
        public void CompletedCoursesReturnsNullUserWhenUserIsNull()
        {
            var controller = CreateScheduleControllerAs("");
            var result = controller.CompletedCourses() as ViewResult;
            Assert.IsTrue(result.ViewName == "NullUser");
        }

        [TestMethod]
        public void FillRemainingRequirementsReturnsNullUserWhenUserIsNull()
        {
            var controller = CreateScheduleControllerAs("");
            var result = controller.FillRemainingRequirements() as ViewResult;
            Assert.IsTrue(result.ViewName == "NullUser");
        }

        [TestMethod]
        public void SchedulesReturnsSchedulesWithValidUser()
        {
            var controller = CreateScheduleControllerAs("abv10@case.edu");
            var result = controller.Schedules() as ViewResult;
            Assert.IsTrue(result.ViewName == "Schedules");
        }

        [TestMethod]
        public void CompletedCoursesReturnsSchedulesWithValidUser()
        {
            var controller = CreateScheduleControllerAs("abv10@case.edu");
            var result = controller.CompletedCourses() as ViewResult;
            Assert.IsTrue(result.ViewName == "CompletedCourses");
        }

        [TestMethod]
        public void FillRemainingRequirementsReturnsFillRemainingRequirementsWithValidUser()
        {
            var controller = CreateScheduleControllerAs("abv10@case.edu");
            var result = controller.FillRemainingRequirements() as ViewResult;
            Assert.IsTrue(result.ViewName == "FillRemainingRequirements");
        }

        [TestMethod]
        public void RemoveRemovesValidCourse()
        {
            var controller = CreateScheduleControllerAs("abv10@case.edu");
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });
            var schedule = user.Schedules.Where(s => s.ScheduleId.Equals("Fall2021abv")).FirstOrDefault();
            Assert.IsTrue(schedule.Courses.Count == 3);
            controller.Remove("CSDS393", "Fall2021abv");
            Assert.IsFalse(schedule.Courses.Count == 3);
        }

        [TestMethod]
        public void RemoveDoesNotRemoveInvalidCourse()
        {
            var controller = CreateScheduleControllerAs("abv10@case.edu");
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });
            var schedule = user.Schedules.Where(s => s.ScheduleId.Equals("Fall2021abv")).FirstOrDefault();
            Assert.IsTrue(schedule.Courses.Count == 3);
            controller.Remove("NotARealCourse", "Fall2021abv");
            Assert.IsTrue(schedule.Courses.Count == 3);
        }

        [TestMethod]
        public void AddAddsValidCourse()
        {
            var controller = CreateScheduleControllerAs("abv10@case.edu");
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });
            var schedule = user.Schedules.Where(s => s.ScheduleId.Equals("Fall2021abv")).FirstOrDefault();
            Assert.IsTrue(schedule.Courses.Count == 3);
            controller.Add("Half Semester Physical Education 1", "Fall2021abv");
            Assert.IsTrue(schedule.Courses.Count == 4);
        }

        [TestMethod]
        public void AddDoesNotAddInvalidCourse()
        {
            var controller = CreateScheduleControllerAs("abv10@case.edu");
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });
            var schedule = user.Schedules.Where(s => s.ScheduleId.Equals("Fall2021abv")).FirstOrDefault();
            Assert.IsTrue(schedule.Courses.Count == 3);
            controller.Add("Not Real Course", "Fall2021abv");
            Assert.IsTrue(schedule.Courses.Count == 3);
        }

        [TestMethod]
        public void CompleteCompletesScheduleForValidUser()
        {
            var controller = CreateScheduleControllerAs("abv10@case.edu");
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });
            var schedule = user.Schedules.Where(s => s.ScheduleId.Equals("Fall2020abv")).FirstOrDefault();
            Assert.IsTrue(schedule.Courses.Count == 4);
            controller.Complete("Fall2020abv");
            var scheduleAfterCompleting = user.Schedules.Where(s => s.ScheduleId.Equals("Fall2020abv")).FirstOrDefault();
            Assert.IsNull(scheduleAfterCompleting);
        }

        [TestMethod]
        public void CompleteReturnsNotFoundResultForInvalidUser()
        {
            var controller = CreateScheduleControllerAs("");
            var result = controller.Complete("Fall2019abv");
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void NewCreatesNewScheduleForValidUser()
        {
            var controller = CreateScheduleControllerAs("abv10@case.edu");
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });
            var schedule = user.Schedules.Where(s => s.ScheduleId.Equals("Fall2018abv")).FirstOrDefault();
            Assert.IsNull(schedule);
            controller.New("Fall2018abv");
            var scheduleAfterNew = user.Schedules.Where(s => s.ScheduleId.Equals("Fall2018abv")).FirstOrDefault();
            Assert.AreEqual(scheduleAfterNew.Courses.Count, 0);
        }

        [TestMethod]
        public void NewReturnsNotFoundResultForInvalidUser()
        {
            var controller = CreateScheduleControllerAs("");
            var result = controller.New("Fall2018abv");
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestCleanup]
        public void CleanUp()
        {
            SeedUsersData.Initialize(_usersContext);
        }
    }
}
