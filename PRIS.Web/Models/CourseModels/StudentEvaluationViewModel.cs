using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.CourseModels
{
    public class StudentEvaluationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double? FinalTestPoints { get; set; }
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? PercentageGrade { get; set; }
        [Display(Name = "Pokalbio įvertinimas")]
        public double? ConversationGrade { get; set; }
        public double FinalAverageGrade { get; set; }
        public string Priority { get; set; }
    }
}
