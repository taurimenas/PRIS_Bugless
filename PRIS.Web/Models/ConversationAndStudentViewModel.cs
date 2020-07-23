using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ConversationAndStudentViewModel
    {
        public List<StudentViewModel> Students { get; set; }
        public List<ConversationResultViewModel> ConvResults { get; set; }
        public List<ConversationFormViewModel> ConversationForm { get; set; }
        public int? ExamId { get; set; }
    }
}
