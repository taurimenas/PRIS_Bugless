using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.CourseModels
{
    public class StudentLockDataListViewModel
    {
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> AcceptancePeriods { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }
        public int? CurrentSelectedCityId { get; set; }
        public int? CurrentSelectedAcceptancePeriodId { get; set; }
        public int? CurrentSelectedPriorityId { get; set; }
        public string SeletectedCity { get; set; }
        public string SelectedAcceptancePeriod { get; set; }
        public string SelectedPriority { get; set; }
        public List<StudentLockDataViewModel> StudentDataLocking { get; set; }
    }
}
