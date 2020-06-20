using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class StudentsCourse
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Priority { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
