using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.InvitationToStudyModel
{
    public class StudentInvitationToStudyViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Vardas")]
        public string FirstName { get; set; }
        [Display(Name = "Pavardė")]
        public string LastName { get; set; }
        [Display(Name = "El. paštas")]
        public string Email { get; set; }
        [Display(Name = "Tel. numeris")]
        public string PhoneNumber { get; set; }
        public double? FinalTestPoints { get; set; }
        [Display(Name = "Procentai")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? PercentageGrade { get; set; }
        [Display(Name = "Pokalbio įvertinimas")]
        public double? ConversationGrade { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double? FinalAverageGrade { get; set; }
        [Display(Name = "Prioritetas")]
        public string Priority { get; set; }
        [Display(Name = "Pakviestas studijuoti")]
        public bool InvitedToStudy { get; set; }
    }
}
