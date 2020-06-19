using PRIS.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class Courses : EntityBase
    {
        public string Title { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
        public int ProgramId { get; set; }
        public string CityId { get; set; }
    }
}
