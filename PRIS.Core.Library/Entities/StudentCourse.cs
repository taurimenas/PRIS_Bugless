namespace PRIS.Core.Library.Entities
{
    public class StudentCourse : EntityBase
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Priority { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
