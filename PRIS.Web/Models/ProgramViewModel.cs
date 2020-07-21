using PRIS.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class ProgramViewModel
    {
        public List<ProgramCreateModel> ProgramNames { get; set; }
        public List<CityCreateModel> CityNames { get; set; }
    }
}
