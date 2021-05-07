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
    public class ProfileControllerTest
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
        public void TestAddDegrees()
        {
            var controller = CreateProfileControllerAs("abv10@case.edu");
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });

            controller.AddDegree("CS");
            Asser.AreEqual("CS", GetDegrees(user.Major));
        }

        [TestMethod]
        public void TestRemoveDegrees()
        {
            var controller = CreateProfileControllerAs("abv10@case.edu");
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });

            controller.AddDegree("CS");
            Assert.AreEqual("CS", GetDegrees(user.Major));
            controller.RemoveDegree("CS");
            Assert.AreEqual("", GetDegrees(user.Major));
        }

        [TestMethod]
        public void TestUpdate()
        {
            var controller = CreateProfileControllerAs("abv10@case.edu");
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });

            controller.Update("Alex", 3);
            Assert.AreEqual(user.Name, "Alex");
            Assert.AreEqual(user.Year, 3);
        }

        [TestMethod]
        public void TestDelete()
        {
            var controller = CreateProfileControllerAs("abv10@case.edu");
            var user = _usersContext.Users.Find(new object[] { "abv10@case.edu" });

            controller.Delete(user.UserID);
            Assert.AreEqual(false, controller.UserExists(user.UserID));
        }

        [TestCleanup]
        public void CleanUp()
        {
            SeedUsersData.Initialize(_usersContext);
        }
    }
}
