﻿// <auto-generated />
using FourYearClassPlanningTool.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FourYearClassPlanningTool.Migrations.Users
{
    [DbContext(typeof(UsersContext))]
    [Migration("20210429153840_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.Admin", b =>
                {
                    b.Property<string>("AdminID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdminID");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.Course", b =>
                {
                    b.Property<string>("CourseID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScheduleID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SemesterOffered")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CourseID");

                    b.HasIndex("ScheduleID");

                    b.HasIndex("UserID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.Schedule", b =>
                {
                    b.Property<string>("ScheduleID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Semester")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ScheduleID");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.User", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AdminID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Major")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("UserID");

                    b.HasIndex("AdminID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.UserSchedule", b =>
                {
                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ScheduleID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("UserSchedules");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.Course", b =>
                {
                    b.HasOne("FourYearClassPlanningTool.Models.Users.Entities.Schedule", null)
                        .WithMany("Courses")
                        .HasForeignKey("ScheduleID");

                    b.HasOne("FourYearClassPlanningTool.Models.Users.Entities.User", null)
                        .WithMany("CompletedCourses")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.User", b =>
                {
                    b.HasOne("FourYearClassPlanningTool.Models.Users.Entities.Admin", null)
                        .WithMany("Users")
                        .HasForeignKey("AdminID");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.Admin", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.Schedule", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.User", b =>
                {
                    b.Navigation("CompletedCourses");
                });
#pragma warning restore 612, 618
        }
    }
}
