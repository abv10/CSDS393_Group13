﻿using FourYearClassPlanningTool.Models.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FourYearClassPlanningTool.Models.Users
{
    
    public static class SeedUsersData
    {
        public static bool reset = false;

        public static void Initialize(UsersContext context)
        {
            
                if (reset)
                {
                    context.RemoveRange(context.Users);
                    context.RemoveRange(context.Admins);
                    context.RemoveRange(context.Schedules);
                    context.RemoveRange(context.Courses);
                    context.SaveChanges();
                    reset = false;
                }
            string parentPath = Directory.GetCurrentDirectory();
            while (!parentPath.EndsWith("FourYearClassPlanningTool"))
            {
                parentPath = parentPath.Substring(0, parentPath.Length - 1);
            }
            if (parentPath.IndexOf("FourYearClassPlanningTool") != (parentPath.LastIndexOf("FourYearClassPlanningTool")))
            {
                parentPath = parentPath.Substring(0, parentPath.LastIndexOf("FourYearClassPlanningTool") - 1);
            }

            parentPath += @"\FourYearClassPlanningTool";
            if (context.Users.Count() <= 0)
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(parentPath + @"\Models\Users\SeedDataForUsersDatabaseCourses.csv");
                    string line;
                    bool firstLine = true;
                    var courses = new List<Course>();
                    while ((line = file.ReadLine()) != null)
                    {
                        if (firstLine == false)
                        {
                            var split = line.Split(',');
                            var course = new Course()
                            {
                                CourseId = split[0],
                                Name = split[1],
                                Credits = int.Parse(split[2]),
                            };
                            courses.Add(course);
                        }
                        else
                        {
                            firstLine = false;
                        }
                    }
                    var searchableCourses = courses.AsQueryable();
                    file = new System.IO.StreamReader(parentPath + @"\Models\Users\SeedDataForUsersDatabaseSchedules.csv");
                    firstLine = true;
                    var schedules = new List<Schedule>();
                    while ((line = file.ReadLine()) != null)
                    {
                        if (firstLine == false)
                        {
                            var split = line.Split(',');
                            var schedule = new Schedule()
                            {
                                ScheduleId = split[0],
                                Semester = split[1],
                                Courses = new List<Course>(),
                                Users = new List<User>()
                            };
                            var splitCourses = split[2].Split(';');
                            foreach (string id in splitCourses)
                            {
                                var courseToAdd = searchableCourses.Where(c => c.CourseId.Equals(id)).FirstOrDefault();
                                if (courseToAdd != null)
                                {
                                    schedule.Courses.Add(courseToAdd);
                                }
                            }
                            schedules.Add(schedule);
                        }
                        else
                        {
                            firstLine = false;
                        }
                    }
                    var searchableSchedules = schedules.AsQueryable();
                    file = new System.IO.StreamReader(parentPath + @"\Models\Users\SeedDataForUsersDatabaseUsers.csv");
                    firstLine = true;
                    var users = new List<User>();
                    while ((line = file.ReadLine()) != null)
                    {
                        if (firstLine == false)
                        {
                            var split = line.Split(',');
                            var user = new User()
                            {
                                UserId = split[0],
                                Password = split[1],
                                Name = split[2],
                                Major = split[3],
                                Year = int.Parse(split[4]),
                                CompletedCourses = new List<Course>(),
                                Schedules = new List<Schedule>()
                                
                            };
                            var splitCourses = split[5].Split(';');
                            var splitSchedules = split[6].Split(';');

                            foreach (string id in splitCourses)
                            {
                                var courseToAdd = searchableCourses.Where(c => c.CourseId.Equals(id)).FirstOrDefault();
                                if (courseToAdd != null)
                                {
                                    user.CompletedCourses.Add(courseToAdd);
                                }
                            }
                            foreach (string id in splitSchedules)
                            {
                                var scheduleToAdd = searchableSchedules.Where(c => c.ScheduleId.Equals(id)).FirstOrDefault();
                                if (scheduleToAdd != null)
                                {
                                    user.Schedules.Add(scheduleToAdd);
                                }
                            }
                            
                            users.Add(user);
                        }
                        else
                        {
                            firstLine = false;
                        }
                    }

                    context.AddRange(courses);
                    context.AddRange(users);
                    context.SaveChanges();
                }

            }
        }
    
}