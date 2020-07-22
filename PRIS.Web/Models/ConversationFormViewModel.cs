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
        [Display(Name = "1. Ar dirba/studijuoja? Jei planuoja tą tęsti ir mokymosi metu – kiek norės skirti laiko kitoms veikloms, ir kaip tikisi suderinti (studijas/ darbą ir mokslus akademijoje)?", Prompt = "")]
        public string Field1 { get; set; }
        [Display(Name = "2. Kokios IT veiklos sritys labiausiai domina?")]
        public string Field2 { get; set; }
        [Display(Name = "3. Ko tikisi iš IT akademijos? ")]
        public string Field3 { get; set; }
        [Display(Name = "4. Ką norėtų veikti baigęs IT akademiją?")]
        public string Field4 { get; set; }
        [Display(Name = "5. Kas motyvuoja tapti programuotoju/a ar testuotoju/-a? Kodėl nori kurti/testuoti programas?")]
        public string Field5 { get; set; }
        [Display(Name = "6. Kaip domisi IT? Skaito knygas/forumus?")]
        public string Field6 { get; set; }
        [Display(Name = "7. Ar kažką kūrė ar testavo savarankiškai? Žaidimus/puslapius/programėles?")]
        public string Field7 { get; set; }
        [Display(Name = "8. Ar mokėsi mokykloje informatiką? Koks lygis? (optional)")]
        public string Field8 { get; set; }
        [Display(Name = "9. When was the last time you used English - speaking, writing, reading?")]
        public string Field9 { get; set; }
        [Display(Name = "10. Papildomi komentarai")]
        public string Field10 { get; set; }
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ExamId { get; set; }
        public int ConversationResultId { get; set; }
    }
}
