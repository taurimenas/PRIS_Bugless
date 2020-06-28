
using System.Collections.Generic;

namespace PRIS.Core.Library.Entities
{
    public class City : EntityBase
    {
        public string Name { get; set; }

        public Course Course { get; set; }
        public List<Exam> Exam { get; set; }
    }
}
