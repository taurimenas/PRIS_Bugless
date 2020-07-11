using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.CourseModels
{
    public class StudentsInCourseViewModel
    {
        public List<CourseViewModel> Courses { get; set; }
        public List<StudentViewModel> Students { get; set; }
        public List<ConversationResultViewModel> ConversationResults { get; set; }
        public List<ResultViewModel> Results { get; set; }
        public List<double?> FinalAverageGrade { get; set; }
    }
}
