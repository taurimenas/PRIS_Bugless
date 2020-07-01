using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ProgramCreateModel
    {
        [Required]
        [Display(Name = "Programa", Prompt = "Programa")]
        public string ProgramName { get; set; }
        public int ProgramNameId { get; set; }

    }
}
