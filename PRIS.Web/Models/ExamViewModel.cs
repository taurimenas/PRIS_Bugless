using PRIS.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ExamViewModel
    {
        //public string City { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
    }
}
