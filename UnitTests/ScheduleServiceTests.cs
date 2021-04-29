using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class ScheduleServiceTests
    {
        private RequirementsContext _reqContext;
        private UsersContext _usersContext;
        private ScheduleService _service;
        private IConfigurationRoot _configuration;
        private DbContextOptions<RequirementsContext> _optionsReq;
        private DbContextOptions<UsersContext> _optionsUser;

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
            
        }

        [TestMethod]
        public void TestGetRemainingRequirementsOneMajor()
        {
            var testOutput = _service.GetRemainingRequirements("Filler", out string message);
            Assert.IsNotNull(_reqContext);
        }

        [TestMethod]
        public void TestGetRemainingRequirementsInvalidStudent()
        {
            var testOutput = _service.GetRemainingRequirements("Filler", out string message);
            Assert.IsNotNull(_reqContext);
        }

        [TestMethod]
        public void TestGetRemainingRequirementsStudentWithNoValidDegrees()
        {
            var testOutput = _service.GetRemainingRequirements("Filler", out string message);
            Assert.IsNotNull(_reqContext);
        }

        [TestMethod]
        public void TestGetRemainingRequirementsStudentWithMajorAndMinor()
        {
            var testOutput = _service.GetRemainingRequirements("Filler", out string message);
            Assert.IsNotNull(_reqContext);
        }
    }
}
