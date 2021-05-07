using FourYearClassPlanningTool.Models.Requirements;
using FourYearClassPlanningTool.Models.Requirements.Entities;
using FourYearClassPlanningTool.Models.Users;
using FourYearClassPlanningTool.Models.Users.Entities;
using FourYearClassPlanningTool.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class ScheduleServiceTests
    {
        private bool needReset = true;
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
        public void TestGetRemainingRequirementsOneMajor()
        {
            var remainingRequirements = _service.GetRemainingRequirements("pfg23@case.edu", out string message);
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
            var testOutput = _service.GetRemainingRequirements("jhp74@case.edu", out string message);
            Assert.IsNull(testOutput);
            Assert.AreEqual(message, "Invalid Degree Id: " + "Computer Science");
        }

        [TestMethod]
        public void TestGetRemainingRequirementsStudentWithMajorAndMinorNoMaxOverlap()
        {
            var remainingRequirements = _service.GetRemainingRequirements("abv10@case.edu", out string message);
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
            var remainingRequirements = _service.GetRemainingRequirements("bht6@case.edu", out string message);
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
            Assert.AreEqual(mathMinor.CreditsRequired, 9);
        }

        /**
         * Right now, abv10@case.edu is the only user with for sure valid schedules
         */
        [TestMethod]
        public void TestValidateScheduleWithValidSchedule()
        {
            var courseIds = new string[] { "POSC325", "PHED2", "USSO000" };
            var courses = _usersContext.Courses.Where(c => courseIds.Contains(c.CourseId)).ToList();
            Schedule toAdd = new Schedule
            {
                ScheduleId = "Fall2022abv10@case.edu",
                Semester = "Fall2020",
                Courses = courses
            };
            bool isValid = _service.ValidateSchedule("abv10@case.edu", new List<Schedule>() { toAdd }, out string message);
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
            bool isValid = _service.ValidateSchedule("abv10@case.edu", new List<Schedule>() { toAdd }, out string message);
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
            bool isValid = _service.ValidateSchedule("abv10@case.edu", new List<Schedule>() { toAdd }, out string message);
            Assert.IsFalse(isValid);
            Assert.AreEqual(message, "Compiler Design does not have prerequisite of CSDS234");
        }

        [TestMethod]
        public void TestAddScheduleToUser()
        {
            var courseIds = new string[] { "POSC325", "PHED2", "USSO000" };
            var courses = _usersContext.Courses.Where(c => courseIds.Contains(c.CourseId)).ToList();
            Schedule toAdd = new Schedule
            {
                ScheduleId = "Fall2022abv",
                Semester = "Fall2020",
                Courses = courses
            };
            if (_service.ValidateSchedule("abv10@case.edu", new List<Schedule>() { toAdd }, out string message))
            {
                _service.AddScheduleToUser("abv10@case.edu", toAdd);
            }
            var updatedUser = _usersContext.Users.Find(new object[] { "abv10@case.edu" });
            Assert.IsTrue(updatedUser.Schedules.Contains(toAdd));

            //Cleanup
            SeedUsersData.reset = true;

        }

        [TestMethod]
        public void TestAddCoursesFromSchedule()
        {
            _service.AddCoursesFromSchedule("abv10@case.edu", "Fall2019abv");
            var ids = new string[] { "CSDS132", "CSDS302", "PHYS121", "FSNA000" };

            var modifiedUser = _usersContext.Users.Find(new object[] { "abv10@case.edu" });
            foreach (var id in ids)
            {
                Assert.IsTrue(modifiedUser.CompletedCourses.Any(c => c.CourseId.Equals(id)));
            }

            Assert.IsFalse(modifiedUser.Schedules.Any(c => c.ScheduleId.Equals("Fall2019abv")));

            //Cleanup
            SeedUsersData.reset = true;

        }

        [TestMethod]
        public void TestAddCourseToCourseGroup()
        {
            var newCourse = new FourYearClassPlanningTool.Models.Requirements.Entities.Course()
            {
                CourseId = "ABCD123",
                Name = "Testing Course",
                Credits = 3,
                SemestersOffered = "Both",
                Prequisites = "none"
            };

            _service.AddCourseToCourseGroup("CSEng", newCourse);
            Assert.IsTrue(_reqContext.CourseGroups.Find("CSEng").Courses.Contains(newCourse));
            SeedRequirementsData.reset = true;
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddCourseToCourseGroupNullCourse()
        {
            _service.AddCourseToCourseGroup("CSEng", null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddCourseToCourseGroupNullGroup()
        {
            var newCourse = new FourYearClassPlanningTool.Models.Requirements.Entities.Course()
            {
                CourseId = "ABCD123",
                Name = "Testing Course",
                Credits = 3,
                SemestersOffered = "Both",
                Prequisites = "none"
            };
            _service.AddCourseToCourseGroup("GroupNull", newCourse);
        }

        [TestMethod]
        public void TestAddCourseToDegree()
        {
            var newCourse = new FourYearClassPlanningTool.Models.Requirements.Entities.Course()
            {
                CourseId = "ABCD123",
                Name = "Testing Course",
                Credits = 3,
                SemestersOffered = "Both",
                Prequisites = "none"
            };

            _service.AddCourseToDegree("CSAI", newCourse);
            Assert.IsTrue(_reqContext.Degrees.Find("CSAI").Courses.Contains(newCourse));
            SeedRequirementsData.reset = true;
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddCourseToDegreeNullCourse()
        {
            _service.AddCourseToDegree("CSAI", null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddCourseToNullDegree()
        {
            var newCourse = new FourYearClassPlanningTool.Models.Requirements.Entities.Course()
            {
                CourseId = "ABCD123",
                Name = "Testing Course",
                Credits = 3,
                SemestersOffered = "Both",
                Prequisites = "none"
            };
            _service.AddCourseToDegree("GroupNull", newCourse);
        }

        [TestMethod]
        public void TestAddCourseGroupToDegree()
        {
            var newGroup = new CourseGroup()
            {
                CourseGroupId = "ABCD123",
                Name = "Testing Course Group",
                CreditsRequired = 3,
                CoursesRequired = 1
            };

            _service.AddCourseGroupToDegree("CSAI", newGroup);
            Assert.IsTrue(_reqContext.Degrees.Find("CSAI").CourseGroups.Contains(newGroup));
            SeedRequirementsData.reset = true;
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddCourseGroupToDegreeNullCourseGroup()
        {
            _service.AddCourseGroupToDegree("CSAI", null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddCourseGroupToNullDegree()
        {
            var newGroup = new CourseGroup()
            {
                CourseGroupId = "ABCD123",
                Name = "Testing Course Group",
                CreditsRequired = 3,
                CoursesRequired = 1
            };
            _service.AddCourseGroupToDegree("GroupNull", newGroup);
        }

        [TestMethod]
        public void TestSearchDegrees()
        {
            var output = _service.SearchDegrees("CS");
            Assert.AreEqual(output.Count(), 6);
            Assert.IsTrue(output.Contains("CSAI, Computer Science, Artificial Intelligence"));

            output = _service.SearchDegrees("Computer");
            Assert.AreEqual(output.Count(), 6);
            Assert.IsTrue(output.Contains("CSAI, Computer Science, Artificial Intelligence"));

            output = _service.SearchDegrees("Artificial");
            Assert.AreEqual(output.Count(), 1);
            Assert.IsTrue(output.Contains("CSAI, Computer Science, Artificial Intelligence"));

            var fail = _service.SearchDegrees("This will fail");
            Assert.AreEqual(fail.Count(), 0);
        }


        [TestMethod]
        public void TestAddDegree()
        {
            _service.AddDegreeToUser("abv10@case.edu", "MAMI");
            var updatedUser = _usersContext.Users.Find("abv10@case.edu");

            Assert.AreEqual(updatedUser.Major, "CSAI;PSMI;MAMI");
            SeedUsersData.reset = true;
        }

        [TestMethod]
        public void TestRemoveDegree()
        {
            _service.RemoveDegreeFromUser("abv10@case.edu", "PSMI");
            var updatedUser = _usersContext.Users.Find("abv10@case.edu");

            Assert.AreEqual(updatedUser.Major, "CSAI");
            SeedUsersData.reset = true;
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void TestRemoveDegreeInvalidUser()
        {
            _service.RemoveDegreeFromUser("wont work", "PSMI");
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void TestRemoveDegreeInvalidDegree()
        {
            _service.RemoveDegreeFromUser("abv10@case.edu", "wont work");
        }


        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void TestAddDegreeNullUser()
        {
            _service.AddDegreeToUser("null", "MAMI");
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void TestAddDegreeNullDegree()
        {
            _service.AddDegreeToUser("abv10@case.edu", "null");
        }

        [TestMethod]
        public void TestAdjustRequirements()
        {
            var degree = new Degree()
            {
                DegreeId = "TEST",
                MaxOverlap = -1,
                CourseGroups = new List<CourseGroup>()
                {
                   new CourseGroup()
                   {
                       CourseGroupId = "GROUP",
                       CoursesRequired = 3,
                       Courses = new List<FourYearClassPlanningTool.Models.Requirements.Entities.Course>()
                       {
                           new FourYearClassPlanningTool.Models.Requirements.Entities.Course()
                           {
                               CourseId = "COURSE1"
                           },
                           new FourYearClassPlanningTool.Models.Requirements.Entities.Course()
                           {
                               CourseId = "COURSE2"
                           },
                           new FourYearClassPlanningTool.Models.Requirements.Entities.Course()
                           {
                               CourseId = "COURSE3"
                           },
                       }
                   }

                },
                Courses = new List<FourYearClassPlanningTool.Models.Requirements.Entities.Course>()
                       {
                           new FourYearClassPlanningTool.Models.Requirements.Entities.Course()
                           {
                               CourseId = "COURSE4"
                           },
                           new FourYearClassPlanningTool.Models.Requirements.Entities.Course()
                           {
                               CourseId = "COURSE5"
                           },
                           new FourYearClassPlanningTool.Models.Requirements.Entities.Course()
                           {
                               CourseId = "COURSE6"
                           },
                       }

            };
            var schedule = new Schedule()
            {
                ScheduleId = "SCHEDULE",
                Courses = new List<FourYearClassPlanningTool.Models.Users.Entities.Course>()
                {
                    new FourYearClassPlanningTool.Models.Users.Entities.Course()
                    {
                        CourseId = "COURSE1"
                    },
                    new FourYearClassPlanningTool.Models.Users.Entities.Course()
                    {
                        CourseId = "COURSE2"
                    },
                    new FourYearClassPlanningTool.Models.Users.Entities.Course()
                    {
                        CourseId = "COURSE4"
                    }
                }
            };
            var updated = _service.AdjustRemainingRequirements(new List<Degree>() { degree }, new List<Schedule>() { schedule });
            Assert.IsNotNull(updated);
            Assert.AreEqual(updated.First().Courses.Count, 2);
            Assert.AreEqual(updated.First().CourseGroups.First().CoursesRequired, 1);
            Assert.AreEqual(updated.First().CourseGroups.First().Courses.First().CourseId, "COURSE3");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAdjustRequirementsNull()
        {
            _service.AdjustRemainingRequirements(new List<Degree>(), new List<Schedule>());
        }

        [TestCleanup]
        public void CleanUp()
        {
            SeedRequirementsData.Initialize(_reqContext);
            SeedUsersData.Initialize(_usersContext);
        }
    }
}