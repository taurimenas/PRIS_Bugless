using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace PRIS.Web.Models.CourseModels
{
    public class StudentEvaluationListViewModel
    {
        public IEnumerable<SelectListItem> Exams { get; set; }
        public List<StudentEvaluationViewModel> StudentEvaluations { get; set; }
        public string CurrentSelectedExam { get; set; }
        public string SeletectedExam { get; set; }
    }
}
