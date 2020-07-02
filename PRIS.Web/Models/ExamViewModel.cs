using Microsoft.AspNetCore.Mvc.Rendering;
using PRIS.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ExamViewModel
    {
        public IEnumerable<SelectListItem> Cities { get; set; }
        public int Id { get; set; }
        public int CityId { get; set; }
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [Display(Name = "Komentaras", Prompt = "Komentaras")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Būtinai turite pasirinkti miestą")]
        [Display(Name = "Miestas")]
        public string SelectedCity { get; set; }
    }
}
