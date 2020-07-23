using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ConversationFormViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Pokalbio anketa")]
        public string ConversationFormName { get; set; }
        public string[] Fields { get; set; }
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ExamId { get; set; }
        public int ConversationResultId { get; set; }
    }
}
