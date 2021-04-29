using FourYearClassPlanningTool.Models.Requirements.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Requirements
{
    public static class SeedRequirementsData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RequirementsContext(serviceProvider.GetRequiredService<DbContextOptions<RequirementsContext>>()))
            {
                bool delete = false;
                if (delete)
                {
                    context.RemoveRange(context.Degrees);
                    context.RemoveRange(context.CourseGroups);
                    context.RemoveRange(context.Courses);
                    context.SaveChanges();
                }
                else if (context.Degrees.Count() <= 0)
                {
                System.IO.StreamReader file = new System.IO.StreamReader(Directory.GetCurrentDirectory() + @"\Models\Requirements\SeedDataForRequirementsDatabase - Courses.csv");
                string line;
                bool firstLine = true;
                var courses = new List<Course>();
                while((line = file.ReadLine()) != null)
                {
                    if(firstLine == false)
                    {
                        var split = line.Split(',');
                        var course = new Course()
                        {
                            CourseId = split[0],
                            Name = split[1],
                            SemestersOffered = split[2],
                            Credits = int.Parse(split[3]),
                            Prequisites = split[4]
                        };
                        courses.Add(course);
                    }
                    else
                    {
                        firstLine = false;
                    }
                }
                var searchableCourses = courses.AsQueryable();
                file = new System.IO.StreamReader(Directory.GetCurrentDirectory() + @"\Models\Requirements\SeedDataForRequirementsDatabase - CourseGroups.csv");
                firstLine = true;
                var courseGroups = new List<CourseGroup>();
                while ((line = file.ReadLine()) != null)
                {
                    if (firstLine == false)
                    {
                        var split = line.Split(',');
                        var courseGroup = new CourseGroup()
                        {
                            CourseGroupId = split[0],
                            Name = split[1],
                            CoursesRequired = int.Parse(split[2]),
                            CreditsRequired = int.Parse(split[3]),
                            Courses = new List<Course>()
                        };
                        var splitCourses = split[4].Split(';');
                        foreach(string id in splitCourses)
                        {
                            var courseToAdd = searchableCourses.Where(c => c.CourseId.Equals(id)).FirstOrDefault();
                            if(courseToAdd != null)
                            {
                                courseGroup.Courses.Add(courseToAdd);
                            }
                        }
                        courseGroups.Add(courseGroup);
                    }
                    else
                    {
                        firstLine = false;
                    }
                }
                var searchableCourseGroups = courseGroups.AsQueryable();
                file = new System.IO.StreamReader(Directory.GetCurrentDirectory() + @"\Models\Requirements\SeedDataForRequirementsDatabase - Degrees.csv");
                firstLine = true;
                var degrees = new List<Degree>();
                while ((line = file.ReadLine()) != null)
                {
                    if (firstLine == false)
                    {
                        var split = line.Split(',');
                        var degree = new Degree()
                        {
                            DegreeId = split[0],
                            Name = split[1],
                            Concentration = split[2],
                            MaxOverlap = int.Parse(split[3]),
                            IsMajor = bool.Parse(split[4]),
                            CourseGroups = new List<CourseGroup>(),
                            Courses = new List<Course>()
                        };
                        var splitCourseGroups = split[5].Split(';');
                        var splitCourses = split[6].Split(';');
                        foreach (string id in splitCourseGroups)
                        {
                            var courseGroupToAdd = searchableCourseGroups.Where(c => c.CourseGroupId.Equals(id)).FirstOrDefault();
                            if (courseGroupToAdd != null)
                            {
                                degree.CourseGroups.Add(courseGroupToAdd);
                            }
                        }
                        foreach (string id in splitCourses)
                        {
                            var courseToAdd = searchableCourses.Where(c => c.CourseId.Equals(id)).FirstOrDefault();
                            if (courseToAdd != null)
                            {
                                degree.Courses.Add(courseToAdd);
                            }
                        }
                        degrees.Add(degree);
                    }
                    else
                    {
                        firstLine = false;
                    }
                }


                    context.AddRange(degrees);
                    context.SaveChanges();
                }

            }
        }
    }
}
