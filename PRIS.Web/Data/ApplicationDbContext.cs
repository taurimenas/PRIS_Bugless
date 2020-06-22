using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIS.Web.Models;
using PRIS.Web.Models.DbModels;

namespace PRIS.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<ConversationResult> ConversationResults { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Models.Program> Programs { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentsCourses { get; set; }
        public DbSet<TestResult> TestResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<ConversationResult>().ToTable("ConversationResult");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Models.Program>().ToTable("Program");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<StudentCourse>().ToTable("StudentsCourse");
            modelBuilder.Entity<TestResult>().ToTable("TestResult");

            modelBuilder.Entity<StudentCourse>()
            .HasKey(t => new { t.StudentId, t.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentsCourses)
                .HasForeignKey(sc => sc.CourseId);
        }
    }
}
