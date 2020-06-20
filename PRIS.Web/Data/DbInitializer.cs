using PRIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Programs.Any())
            {
                return;
            }
            var programs = new Models.Programs[]
            {
                new Models.Programs{Name=".NET"},
                new Models.Programs{Name="JAVA"}
            };
            context.SaveChanges();

            if (context.Students.Any())
            {
                return;
            }
            var students = new Student[]
            {
                new Student{Id=1, Email="Asd@asd", Comment= "", FirstName="asdasd", Gender = Core.Library.Gender.Female, LastName= "asdas", PhoneNumber="6544", StudentsCourseId=1},
            };
            context.SaveChanges();
        }
    }
}
