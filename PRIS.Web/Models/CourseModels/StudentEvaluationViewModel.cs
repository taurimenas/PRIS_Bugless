using System;
using System.ComponentModel.DataAnnotations;

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
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double? FinalAverageGrade { get; set; }
        public string Priority { get; set; }
        public string Priority2 { get; set; }
        public string Priority3 { get; set; }
        public int? CityId { get; set; }
        public int? ExamId { get; set; }
        public int? CourseId { get; set; }
    }
}
