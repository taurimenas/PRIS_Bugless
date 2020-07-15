using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.InvitationToStudyModel
{
    public class StudentInvitationToStudyListViewModel
    {
        public IEnumerable<SelectListItem> Exams { get; set; }
        public List<StudentInvitationToStudyViewModel> StudentInvitationToStudy { get; set; }
        public string CurrentSelectedExam { get; set; }
        public string SeletectedExam { get; set; }
    }
}
