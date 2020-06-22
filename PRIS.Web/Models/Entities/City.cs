using PRIS.Core.Library;
using PRIS.Web.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.DbModels
{
    public class City : EntityBase
    {
        public string Name { get; set; }

        public Course Course { get; set; }
        public Exam Exam { get; set; }
    }
}
