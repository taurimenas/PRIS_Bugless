using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class Query
    {
        public double? MinGrade { get; set; } = 1.5;
        public double? MaxGrade { get; set; } = 10;
        public int? ExamId { get; set; }
        public int? CityId { get; set; }
        public int? CourseId { get; set; }
    }
}
