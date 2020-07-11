using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using PRIS.Web.Models.CourseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Mappings
{
    public class CourseMappings
    {
        public static StudentsInCourseViewModel ToViewModel(List<StudentViewModel> students, List<ConversationResultViewModel> conversationResults, List<CourseViewModel> courses, List<ResultViewModel> results)
        {
            var finalAverageGrades = new List<double?>();
            for (int i = 0; i < students.Count; i++)
            {
                if (results.ElementAt(i).PercentageGrade == null || conversationResults.ElementAt(i).Grade == null)
                    finalAverageGrades.Add(0);
                else finalAverageGrades.Add(Math.Round((double)((results.ElementAt(i).PercentageGrade / 10 + conversationResults.ElementAt(i).Grade) / 2), 1));
            }
            return new StudentsInCourseViewModel
            {
                ConversationResults = conversationResults,
                Students = students,
                Courses = courses,
                Results = results,
                FinalAverageGrade = finalAverageGrades
            };
        }
        public static CourseViewModel ToViewModel(Course entity)
        {
            if (entity == null)
            {
                return new CourseViewModel
                {
                    Title = null,
                };
            }
            return new CourseViewModel
            {
                CityId = entity.CityId,
                Title = entity.Title,
                StartYear = entity.StartYear,
                EndYear = entity.EndYear,
                ProgramId = entity.ProgramId
            };
        }
    }
}
