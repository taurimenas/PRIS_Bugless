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
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public string SelectedCity { get; set; }
    }
}
