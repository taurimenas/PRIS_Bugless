using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIS.Core.Library;

namespace PRIS.Web.Models
{
    public class StudentPrograms : EntityBase
    {
        public int StudentId { get; set; }
        public int ProgramId { get; set; }

        public Students Student { get; set; }
    }
}
