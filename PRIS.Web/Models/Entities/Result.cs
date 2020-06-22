using PRIS.Core.Library;
using PRIS.Web.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class Result : EntityBase
    {
        public double Task1_1 { get; set; }
        public double Task1_2 { get; set; }
        public double Task1_3 { get; set; }
        public double Task2_1 { get; set; }
        public double Task2_2 { get; set; }
        public double Task2_3 { get; set; }
        public double Task3_1 { get; set; }
        public double Task3_2 { get; set; }
        public double Task3_3 { get; set; }
        public double Task3_4 { get; set; }
        public string Comment { get; set; }

        public List<Student> Students { get; set; }
        public Exam Exam { get; set; }
    }
}
