using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.InvitationToStudyModel
{
    public class StudentComment
    {
        public int Id { get; set; }
        [Display(Name = "Komentaras")]
        public string Comment { get; set; }
    }
}
