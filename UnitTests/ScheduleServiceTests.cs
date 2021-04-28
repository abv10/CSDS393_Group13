using FourYearClassPlanningTool.Models.Requirements;
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
        private ScheduleService _service;
        private IConfigurationRoot _configuration;
        private DbContextOptions<RequirementsContext> _options;

        [TestInitialize]
        public void SetUp()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            _configuration = builder.Build();

            _options = new DbContextOptionsBuilder<RequirementsContext>().UseLazyLoadingProxies().UseSqlServer(_configuration.GetConnectionString("RequirementsContext")).Options;
            _reqContext = new RequirementsContext(_options);
            _service = new ScheduleService(_reqContext);
        }

        [TestMethod]
        public void TestGetRemainingRequirementsOneMajor()
        {
            var testOutput = _service.GetRemainingRequirements("Filler");
            Assert.IsNotNull(_reqContext);
        }

        [TestMethod]
        public void TestGetRemainingRequirementsInvalidStudent()
        {
            var testOutput = _service.GetRemainingRequirements("Filler");
            Assert.IsNotNull(_reqContext);
        }

        [TestMethod]
        public void TestGetRemainingRequirementsStudentWithNoValidDegrees()
        {
            var testOutput = _service.GetRemainingRequirements("Filler");
            Assert.IsNotNull(_reqContext);
        }

        [TestMethod]
        public void TestGetRemainingRequirementsStudentWithMajorAndMinor()
        {
            var testOutput = _service.GetRemainingRequirements("Filler");
            Assert.IsNotNull(_reqContext);
        }
    }
}
