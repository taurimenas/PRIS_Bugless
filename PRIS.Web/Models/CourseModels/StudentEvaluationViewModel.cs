using System;
using System.ComponentModel.DataAnnotations;

namespace PRIS.Web.Models.CourseModels
{
    public class StudentEvaluationViewModel
    {
        [Display(Name = "Vardas")]
        public string FirstName { get; set; }
        [Display(Name = "Pavardė")]
        public string LastName { get; set; }
        [Display(Name = "El. paštas")]
        public string Email { get; set; }
        [Display(Name = "Tel. numeris")]
        public string PhoneNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:F1}")]
        [Display(Name = "Testas")]
        public double? FinalTestPoints { get; set; }
        [DisplayFormat(DataFormatString = "{0:F1}")]
        [Display(Name = "Procentai")]
        public double? PercentageGrade { get; set; }
        [Display(Name = "Pokalbis")]
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
