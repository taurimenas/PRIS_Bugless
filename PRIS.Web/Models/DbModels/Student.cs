using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIS.Core.Library;

namespace PRIS.Web.Models
{
    public class Student : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string Comment { get; set; }
        public int StudentsCourseId { get; set; }

        //public TestResult TestResult { get; set; }
        //public ConversationResult ConversationResult { get; set; }
        //[System.ComponentModel.DataAnnotations.Schema.NotMapped]
        //public List<StudentsCourse> StudentsCourses { get; set; }
    }
}
