using PRIS.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ImportStudentsDataModel
    { 
        public string CsvId { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string Priority { get; set; }
        public string Tasks { get; set; }
        //public string Task1 { get; set; }
        //public string Task2 { get; set; }
        //public string Task3 { get; set; }
        //public string Task4 { get; set; }
        //public string Task5 { get; set; }
        //public string Task6 { get; set; }
        //public string Task7 { get; set; }
        //public string Task8 { get; set; }
        //public string Task9 { get; set; }
        //public string Task10 { get; set; }
        public Gender Gender { get; set; }
        public bool PassedExam { get; set; }
        public double? ConversationResult { get; set; }
        public bool InvitationToStudy { get; set; }
        public bool SignedAContract { get; set; }
        

    }
}
