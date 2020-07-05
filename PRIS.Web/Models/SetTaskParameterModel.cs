using PRIS.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class SetTaskParameterModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "1.1")]
        [Required(ErrorMessage = "Įveskite {0} užduoties maksimalų balą")]
        public int[] Tasks { get; set; }
    }
}
