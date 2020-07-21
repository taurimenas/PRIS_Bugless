using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.CourseModels
{
    public class StudentLockDataViewModel
    {
        public int Id { get; set; }
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
        [Display(Name = "Procentai")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? PercentageGrade { get; set; }
        [Display(Name = "Pokalbis")]
        public double? ConversationGrade { get; set; }
        [Display(Name = "Bendras vidurkis")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? FinalAverageGrade { get; set; }
        [Display(Name = "Prioritetas")]
        public string Priority { get; set; }
        public bool InvitedToStudy { get; set; }
        [Display(Name = "Sutartis pasirašyta")]
        public bool SignedAContract { get; set; } = false;
        [Display(Name = "Studento duomenys užrakinti")]
        public bool StudentDataLocked { get; set; }
        public int? CityId { get; set; }
        public int? ExamId { get; set; }
        public int? CourseId { get; set; }
    }
}
