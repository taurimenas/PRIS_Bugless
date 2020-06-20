using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<Program> Programs { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentsCourse> StudentsCourses { get; set; }
        public DbSet<TestResult> TestResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<ConversationResult>().ToTable("ConversationResult");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Program>().ToTable("Program");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<StudentsCourse>().ToTable("StudentsCourse");
            modelBuilder.Entity<TestResult>().ToTable("TestResult");
        }
    }
}
