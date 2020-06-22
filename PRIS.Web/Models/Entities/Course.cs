using PRIS.Core.Library;
using PRIS.Web.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Models
{
    public class Course : EntityBase
    {
        public string Title { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
        public int ProgramId { get; set; }
        public int CityId { get; set; }

        public List<StudentCourse> StudentsCourses { get; set; }
        public List<Models.Program> Programs { get; set; }
        public List<City> Cities { get; set; }
    }
}
