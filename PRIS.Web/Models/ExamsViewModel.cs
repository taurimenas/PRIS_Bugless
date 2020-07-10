using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ExamsViewModel
    {
        public IEnumerable<ExamViewModel> ExamViewModels { get; set; }
        public string SelectedAcceptancePeriod { get; set; }
        public IEnumerable<SelectListItem> AcceptancePeriod { get; set; }
    }
}
