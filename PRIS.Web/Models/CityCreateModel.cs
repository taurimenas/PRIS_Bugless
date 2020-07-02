using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class CityCreateModel
    {
        [Required(ErrorMessage = "Įveskite miesto pavadinimą")]
        [Display(Name = "Miestas", Prompt = "Miestas")]
        public string CityName { get; set; }
        public int CityNameId { get; set; }
    }
}
