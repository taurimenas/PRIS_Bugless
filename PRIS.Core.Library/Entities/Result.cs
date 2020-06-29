using System.Collections.Generic;

namespace PRIS.Core.Library.Entities
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
        public int? ExamId { get; set; }

        public int? StudentForeignKey { get; set; }
        public Student Student { get; set; }
        public Exam Exam { get; set; }
    }
}
