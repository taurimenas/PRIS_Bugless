using PRIS.Core.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class Program : EntityBase
    {
        public string Name { get; set; }

        public Course Course { get; set; }
    }
}
