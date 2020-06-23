using System;
using System.Collections.Generic;

namespace PRIS.Core.Library.Entities
{
    public class Course : EntityBase
    {
        public string Title { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
        public int ProgramId { get; set; }
        public int CityId { get; set; }

        public List<StudentCourse> StudentsCourses { get; set; }
        public List<Entities.Program> Programs { get; set; }
        public List<City> Cities { get; set; }
    }
}
