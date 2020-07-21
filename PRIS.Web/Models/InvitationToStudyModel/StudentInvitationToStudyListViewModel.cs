using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.InvitationToStudyModel
{
    public class StudentInvitationToStudyListViewModel
    {
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> AcceptancePeriods { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }
        public IEnumerable<SelectListItem> Exams { get; set; }
        public int? CurrentSelectedCityId { get; set; }
        public int? CurrentSelectedAcceptancePeriodId { get; set; }
        public int? CurrentSelectedPriorityId { get; set; }
        public List<StudentInvitationToStudyViewModel> StudentInvitationToStudy { get; set; }
        public string SelectedCity { get; set; }
        public string SelectedAcceptancePeriod { get; set; }
        public string SelectedPriority { get; set; }
    }
}
