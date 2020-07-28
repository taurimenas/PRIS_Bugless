using PRIS.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ImportedStudentsDataModel
    {
        public string CsvId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Priority { get; set; }
        public string Tasks { get; set; }
        public Gender Gender { get; set; }
        public bool PassedExam { get; set; }
        public double? ConversationResult { get; set; }
        public bool InvitationToStudy { get; set; }
        public bool SignedAContract { get; set; }
    }
}
