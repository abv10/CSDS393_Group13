using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Users.Entities;
using FourYearClassPlanningTool.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var remainingRequirements = _service.GetRemainingRequirements("pfg23", out string message);
            Assert.AreEqual(1, remainingRequirements.Count);
            var firstDegree = remainingRequirements[0];
            Assert.AreEqual("CSSE", firstDegree.DegreeId);
            var CSEng = firstDegree.CourseGroups.AsQueryable().Where(c => c.CourseGroupId == "CSEng").FirstOrDefault();
            Assert.AreEqual(CSEng.CoursesRequired, 4);
            Assert.AreEqual(CSEng.CreditsRequired, 10);

            var CSCore = firstDegree.CourseGroups.AsQueryable().Where(c => c.CourseGroupId == "CSCore").FirstOrDefault();
            Assert.AreEqual(CSCore.CoursesRequired, 3);
            Assert.AreEqual(CSCore.CreditsRequired, 10);

            var CSTech = firstDegree.CourseGroups.AsQueryable().Where(c => c.CourseGroupId == "CSTech").FirstOrDefault();
            Assert.AreEqual(CSTech.CoursesRequired, 4);
            Assert.AreEqual(CSTech.CreditsRequired, 0);
        }

        [TestMethod]
        public void TestGetRemainingRequirementsInvalidStudent()
        {
            var testOutput = _service.GetRemainingRequirements("NoStudentWithThisId", out string message);
            Assert.IsNull(testOutput);
            Assert.AreEqual(message, "Unable to find a student with that id");
        }

        [TestMethod]
        public void TestGetRemainingRequirementsStudentWithNoValidDegrees()
        {
            var testOutput = _service.GetRemainingRequirements("jhp74", out string message);
            Assert.IsNull(testOutput);
            Assert.AreEqual(message, "Invalid Degree Id: " + "Computer Science");
        }

        [TestMethod]
        public void TestGetRemainingRequirementsStudentWithMajorAndMinorNoMaxOverlap()
        {
            var remainingRequirements = _service.GetRemainingRequirements("abv", out string message);
            Assert.AreEqual(2, remainingRequirements.Count);
            var firstDegree = remainingRequirements[0];
            Assert.AreEqual("CSAI", firstDegree.DegreeId);
            var CSCore = firstDegree.CourseGroups.AsQueryable().Where(c => c.CourseGroupId == "CSCore").FirstOrDefault();
            Assert.AreEqual(CSCore.CoursesRequired, 3);

            var minor = remainingRequirements[1];
            Assert.AreEqual("PSMI", minor.DegreeId);
            var PoscMinor300 = minor.CourseGroups.AsQueryable().Where(c => c.CourseGroupId == "POSC300Level").FirstOrDefault();
            var PoscAll = minor.CourseGroups.AsQueryable().Where(c => c.CourseGroupId == "AllPOSC").FirstOrDefault();
            Assert.AreEqual(PoscMinor300.CoursesRequired, 2);
            Assert.AreEqual(PoscAll.CoursesRequired, 3);
        }

        [TestMethod]
        public void TestGetRemainingRequirementsStudentWithMajorAndMinorWithMaxOverlap()
        {
            var remainingRequirements = _service.GetRemainingRequirements("bht6", out string message);
            Assert.AreEqual(2, remainingRequirements.Count);
            var firstDegree = remainingRequirements[0];
            Assert.AreEqual("CSSE", firstDegree.DegreeId);
            var CSCore = firstDegree.CourseGroups.AsQueryable().Where(c => c.CourseGroupId == "CSCore").FirstOrDefault();
            Assert.AreEqual(CSCore.CoursesRequired, 1);
            var CSBreadth = firstDegree.CourseGroups.AsQueryable().Where(c => c.CourseGroupId == "CSBreadth").FirstOrDefault();
            Assert.IsNull(CSBreadth);

            var minor = remainingRequirements[1];
            Assert.AreEqual("MAMI", minor.DegreeId);
            var mathMinor = minor.CourseGroups.AsQueryable().Where(c => c.CourseGroupId == "MathMinor17").FirstOrDefault();
            Assert.AreEqual(mathMinor.CoursesRequired, 0);
            Assert.AreEqual(mathMinor.CreditsRequired, 10);
        }

        /**
         * Right now, abv is the only user with for sure valid schedules
         */
        [TestMethod]
        public void TestValidateScheduleWithValidSchedule()
        {
            var courseIds = new string[] { "POSC325", "PHED2", "USSO000" };
            var courses = _usersContext.Courses.Where(c => courseIds.Contains(c.CourseId)).ToList();
            Schedule toAdd = new Schedule
            {
                ScheduleId = "Fall2022abv",
                Semester = "Fall2020",
                Courses = courses
            };
            bool isValid = _service.ValidateSchedule("abv", new List<Schedule>() { toAdd }, out string message);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void TestValidateScheduleWithInValidSemester()
        {
            var courseIds = new string[] { "CSDS442", "PHED2", "USSO000" };
            var courses = _usersContext.Courses.Where(c => courseIds.Contains(c.CourseId)).ToList();
            Schedule toAdd = new Schedule
            {
                ScheduleId = "Fall2022abv",
                Semester = "Fall2020",
                Courses = courses
            };
            bool isValid = _service.ValidateSchedule("abv", new List<Schedule>() { toAdd }, out string message);
            Assert.AreEqual("You cannot take Causal Learning from Datain semester: " + toAdd.Semester, message);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void TestValidateScheduleWithPreRequisitiesNotMet()
        {
            var courseIds = new string[] { "CSDS337", "PHED2", "USSO000" };
            var courses = _usersContext.Courses.Where(c => courseIds.Contains(c.CourseId)).ToList();
            Schedule toAdd = new Schedule
            {
                ScheduleId = "Fall2022abv",
                Semester = "Fall2020",
                Courses = courses
            };
            bool isValid = _service.ValidateSchedule("abv", new List<Schedule>() { toAdd }, out string message);
            Assert.IsFalse(isValid);
            Assert.AreEqual(message, "Compiler Design does not have prequisite of CSDS234");
        }
    }
}
