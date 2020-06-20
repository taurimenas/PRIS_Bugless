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
        public DbSet<Models.Program> Programs { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentsCourse> StudentsCourses { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
    }
}
