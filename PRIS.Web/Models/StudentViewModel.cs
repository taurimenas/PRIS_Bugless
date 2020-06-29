using Microsoft.AspNetCore.Mvc.Rendering;
using PRIS.Core.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Vardas", Prompt = "Vardas")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Pavardė", Prompt = "Pavardė")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "El. paštas", Prompt = "Studentas@email.com")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Tel. numeris", Prompt = "Tel. numeris")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Lytis")]
        public Gender Gender { get; set; }
        [Display(Name = "Komentaras", Prompt = "Komentaras")]
        public string Comment { get; set; }
        public bool PassedExam { get; set; } = false;
        public string ErrorMessage { get; set; }
        

        //public int ResultId { get; set; }
        //[Required(ErrorMessage ="Įrašykite užduoties įvertinimą")]
        //public double Task1_1 { get; set; }
        //[Required(ErrorMessage = "Įrašykite užduoties įvertinimą")]
        //public double Task1_2 { get; set; }
        //[Required(ErrorMessage = "Įrašykite užduoties įvertinimą")]
        //public double Task1_3 { get; set; }
        //[Required(ErrorMessage = "Įrašykite užduoties įvertinimą")]
        //public double Task2_1 { get; set; }
        //[Required(ErrorMessage = "Įrašykite užduoties įvertinimą")]
        //public double Task2_2 { get; set; }
        //[Required(ErrorMessage = "Įrašykite užduoties įvertinimą")]
        //public double Task2_3 { get; set; }
        //[Required(ErrorMessage = "Įrašykite užduoties įvertinimą")]
        //public double Task3_1 { get; set; }
        //[Required(ErrorMessage = "Įrašykite užduoties įvertinimą")]
        //public double Task3_2 { get; set; }
        //[Required(ErrorMessage = "Įrašykite užduoties įvertinimą")]
        //public double Task3_3 { get; set; }
        //[Required(ErrorMessage = "Įrašykite užduoties įvertinimą")]
        //public double Task3_4 { get; set; }
        //[Display(Name = "Testo balai")]
        //public double? FinalPoints { get; set; }
        //public double? PercentageGrade { get; set; }
        //public string CommentResult { get; set; }
        //public int? StudentForeignKey { get; set; }
    }

    //public class ResultViewModel
    //{
    //    public double Task1_1 { get; set; }
    //    public double Task1_2 { get; set; }
    //    public double Task1_3 { get; set; }
    //    public double Task2_1 { get; set; }
    //    public double Task2_2 { get; set; }
    //    public double Task2_3 { get; set; }
    //    public double Task3_1 { get; set; }
    //    public double Task3_2 { get; set; }
    //    public double Task3_3 { get; set; }
    //    public double Task3_4 { get; set; }
    //    public string ResultComment { get; set; }
    //}
}
