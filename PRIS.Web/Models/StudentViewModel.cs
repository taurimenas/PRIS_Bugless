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
        [Required(ErrorMessage = "Klaida")]
        [Display(Name = "El. paštas", Prompt = "Studentas@email.com")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Tel. numeris", Prompt = "Tel. numeris")]
        [Range(0, 10, ErrorMessage = "Klaida")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Lytis")]
        public Gender Gender { get; set; }
        [Display(Name = "Komentaras", Prompt = "Komentaras")]
        public string Comment { get; set; }
        public bool PassedExam { get; set; } = false;
        public string ErrorMessage { get; set; }
    } 
}
