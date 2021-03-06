// <auto-generated />
using FourYearClassPlanningTool.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FourYearClassPlanningTool.Migrations.Users
{
    [DbContext(typeof(UsersContext))]
    [Migration("20210501151551_UpdatedFieldNames")]
    partial class UpdatedFieldNames
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CourseSchedule", b =>
                {
                    b.Property<string>("CoursesCourseId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ScheduleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CoursesCourseId", "ScheduleId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("CourseSchedule");
                });

            modelBuilder.Entity("CourseUser", b =>
                {
                    b.Property<string>("CompletedCoursesCourseId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UsersUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CompletedCoursesCourseId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("CourseUser");
                });

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
                    b.Property<string>("CourseId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.Schedule", b =>
                {
                    b.Property<string>("ScheduleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Semester")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ScheduleId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("FourYearClassPlanningTool.Models.Users.Entities.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Major")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ScheduleUser", b =>
                {
                    b.Property<string>("SchedulesScheduleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UsersUserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SchedulesScheduleId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("ScheduleUser");
                });

            modelBuilder.Entity("CourseSchedule", b =>
                {
                    b.HasOne("FourYearClassPlanningTool.Models.Users.Entities.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FourYearClassPlanningTool.Models.Users.Entities.Schedule", null)
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseUser", b =>
                {
                    b.HasOne("FourYearClassPlanningTool.Models.Users.Entities.Course", null)
                        .WithMany()
                        .HasForeignKey("CompletedCoursesCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FourYearClassPlanningTool.Models.Users.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScheduleUser", b =>
                {
                    b.HasOne("FourYearClassPlanningTool.Models.Users.Entities.Schedule", null)
                        .WithMany()
                        .HasForeignKey("SchedulesScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FourYearClassPlanningTool.Models.Users.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
