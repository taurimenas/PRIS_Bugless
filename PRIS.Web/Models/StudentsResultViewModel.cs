using PRIS.Core.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class StudentsResultViewModel
    {
        public int ResultId { get; set; }
        [Display(Name = "1.1")]
       [Required(ErrorMessage = "Įveskite {0} užduoties balą")]
        public double Task1_1 { get; set; }
        [Display(Name = "1.2")]
        [Required(ErrorMessage = "Įveskite {0} užduoties balą")]
        public double Task1_2 { get; set; }
        [Display(Name = "1.3")]
        [Required(ErrorMessage = "Įveskite {0} užduoties balą")]
        public double Task1_3 { get; set; }
        [Display(Name = "2.1")]
        [Required(ErrorMessage = "Įveskite {0} užduoties balą")]
        public double Task2_1 { get; set; }
        [Display(Name = "2.2")]
        [Required(ErrorMessage = "Įveskite {0} užduoties balą")]
        public double Task2_2 { get; set; }
        [Display(Name = "2.3")]
        [Required(ErrorMessage = "Įveskite {0} užduoties balą")]
        public double Task2_3 { get; set; }
        [Display(Name = "3.1")]
        [Required(ErrorMessage = "Įveskite {0} užduoties balą")]
        public double Task3_1 { get; set; }
        [Display(Name = "3.2")]
        [Required(ErrorMessage = "Įveskite {0} užduoties balą")]
        public double Task3_2 { get; set; }
        [Display(Name = "3.3")]
        [Required(ErrorMessage = "Įveskite {0} užduoties balą")]
        public double Task3_3 { get; set; }
        [Display(Name = "3.4")]
        [Required(ErrorMessage = "Įveskite {0} užduoties balą")]
        public double Task3_4 { get; set; }
        [Display(Name = "Testo balai")]
        public double? FinalPoints { get; set; }
        [Display(Name = "Procentai")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? PercentageGrade { get; set; }
        [Display(Name = "Komentaras")]
        public string CommentResult { get; set; }
        public int? StudentForeignKey { get; set; }
        public int? ExamId { get; set; }
        public int Id { get; set; }
        [Display(Name = "Vardas", Prompt = "Vardas")]
        public string FirstName { get; set; }
        [Display(Name = "Pavardė", Prompt = "Pavardė")]
        public string LastName { get; set; }
        [Display(Name = "El. paštas", Prompt = "Studentas@email.com")]
        public string Email { get; set; }
        [Display(Name = "Tel. numeris", Prompt = "Tel. numeris")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Lytis")]
        public Gender Gender { get; set; }
        [Display(Name = "Komentaras", Prompt = "Komentaras")]
        public string Comment { get; set; }
        public bool PassedExam { get; set; } = false;
        public string ErrorMessage { get; set; }
    }
}
