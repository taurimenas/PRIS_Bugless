using System.Collections.Generic;

namespace PRIS.Core.Library.Entities
{
    public class Result : EntityBase
    {
        public string Tasks { get; set; }
        public string Comment { get; set; }
        public int? ExamId { get; set; }

        public int? StudentForeignKey { get; set; }
        public Student Student { get; set; }
        public Exam Exam { get; set; }
    }
}
