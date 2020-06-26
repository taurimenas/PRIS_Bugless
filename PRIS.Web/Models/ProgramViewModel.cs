using PRIS.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ProgramViewModel
    {
        public List<PRIS.Core.Library.Entities.Program> ProgramNames { get; set; }
        public List<City> CityNames { get; set; }
    }
}
