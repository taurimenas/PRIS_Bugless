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
        [Display(Name = "Vardas")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Pavardė")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "El. paštas")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Tel. numeris")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Lytis")]
        public Gender Gender { get; set; }
        [Display(Name = "Komentaras")]
        public string Comment { get; set; }
    }
}
