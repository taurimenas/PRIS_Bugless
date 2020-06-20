using PRIS.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ConversationResult : EntityBase
    {
        public int? Grade { get; set; }
        public string Comment { get; set; }

        //public List<Student> Students { get; set; }
    }
}
