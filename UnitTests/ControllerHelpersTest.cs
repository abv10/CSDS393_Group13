using FourYearClassPlanningTool.Controllers;
using FourYearClassPlanningTool.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class ControllerHelpersTest
    {
        private bool needReset = true;
        private UsersContext _usersContext;
        private IConfigurationRoot _configuration;
        private DbContextOptions<UsersContext> _optionsUser;

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
        }

        [TestMethod]
        public void TestGetOrCreateUserGet()
        {
            var user = ControllerHelpers.GetOrCreateUser("abv10@case.edu", _usersContext);
            Assert.AreEqual("abv10@case.edu", user.UserId);
            Assert.AreNotEqual(0, user.CompletedCourses.Count);
        }

        [TestMethod]
        public void TestGetOrCreateUserCreate()
        {
            var user = ControllerHelpers.GetOrCreateUser("abv11@case.edu", _usersContext);
            Assert.AreEqual("abv11@case.edu", user.UserId);
            Assert.AreEqual(0, user.CompletedCourses.Count);
        }

        [TestMethod]
        public void TestGetOrCreateUserNull()
        {
            var user = ControllerHelpers.GetOrCreateUser(null, _usersContext);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void TestIsAdminTrue()
        {
            Assert.IsTrue(ControllerHelpers.IsAdmin("admin@case.edu", _usersContext));
        }

        [TestMethod]
        public void TestIsAdminFalse()
        {
            Assert.IsFalse(ControllerHelpers.IsAdmin(null, _usersContext));
        }

        [TestCleanup]
        public void CleanUp()
        {
            SeedUsersData.Initialize(_usersContext);
        }
    }
}
