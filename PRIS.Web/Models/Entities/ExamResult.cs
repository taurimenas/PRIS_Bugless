using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models.Entity
{
    public class ExamResult
    {
        public int ExamId { get; set; }
        public int ResultId { get; set; }

        public Exam Exam { get; set; }
        public Result Result { get; set; }
    }
}
