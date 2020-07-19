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
        public int? Id { get; set; }
        [Required(ErrorMessage = "Įveskite vardą")]
        [Display(Name = "Vardas", Prompt = "Vardas")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Įveskite pavardę")]
        [Display(Name = "Pavardė", Prompt = "Pavardė")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Įveskite el. pašto adresą")]
        [Display(Name = "El. paštas", Prompt = "Studentas@email.com")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Įveskite telefono numerį")]
        [Display(Name = "Tel. numeris", Prompt = "Tel. numeris")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Būtina nurodyti lytį")]
        [Display(Name = "Lytis")]
        public Gender? Gender { get; set; }
        [Display(Name = "Komentaras", Prompt = "Komentaras")]
        public string Comment { get; set; }
        public bool? PassedExam { get; set; } = false;
        public string ErrorMessage { get; set; }
        public int ExamId { get; set; }
        public List<SelectListItem> Programs { get; set; }
        
        public string[] SelectedPriority { get; set; }
    } 
}
