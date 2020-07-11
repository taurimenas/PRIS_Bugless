using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Mappings
{
    public class StudentCourseMappings
    {
        public static StudentsCourseViewModel ToViewModel(StudentCourse entity)
        {
            return new StudentsCourseViewModel
            {
                CourseId = entity.CourseId,
                Priority = entity.Priority,
                StudentId = entity.StudentId
            };
        }
    }
}
