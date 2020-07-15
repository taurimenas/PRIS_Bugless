using PRIS.Core.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ConversationResultViewModel
    {
        public int StudentId { get; set; }
        [Display(Name = "Vardas", Prompt = "Vardas")]
        public string FirstName { get; set; }
        [Display(Name = "Pavardė", Prompt = "Pavardė")]
        public string LastName { get; set; }
        [Display(Name = "Pokalbio komentaras", Prompt = "Pokalbio komentaras")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public double? FinalPoints { get; set; }
        public double? PercentageGrade { get; set; }
        [Display(Name = "Pokalbio komentaras", Prompt = "Pokalbio komentaras")]
        public string ConversationResultComment { get; set; }
        public int ConversationResultId { get; set; }
        [Display(Name = "Pokalbio įvertinimas", Prompt = "Pokalbio įvertinimas")]
        [Range(0.0, 10.0)]
        public double? Grade { get; set; }
        public int? ExamId { get; set; }
    }
}
