using PRIS.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ProgramViewModel
    {
        public int ProgramId { get; set; }
        public string ProgramName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }

    }
}
