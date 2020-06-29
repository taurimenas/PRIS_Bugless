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
        public double Task1_1 { get; set; }
        public double Task1_2 { get; set; }
        public double Task1_3 { get; set; }
        public double Task2_1 { get; set; }
        public double Task2_2 { get; set; }
        public double Task2_3 { get; set; }
        public double Task3_1 { get; set; }
        public double Task3_2 { get; set; }
        public double Task3_3 { get; set; }
        public double Task3_4 { get; set; }
        [Display(Name = "Testo balai")]
        public double? FinalPoints { get; set; }
        public double? PercentageGrade { get; set; }
        public string CommentResult { get; set; }
        public int? StudentForeignKey { get; set; }
        public int Id { get; set; }
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
