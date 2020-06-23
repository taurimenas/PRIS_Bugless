
namespace PRIS.Core.Library.Entities
{
    public class City : EntityBase
    {
        public string Name { get; set; }

        public Course Course { get; set; }
        public Exam Exam { get; set; }
    }
}
