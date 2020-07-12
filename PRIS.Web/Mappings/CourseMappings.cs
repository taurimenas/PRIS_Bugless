using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using PRIS.Web.Models.CourseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        public static StudentEvaluationViewModel ToViewModel(Student student, ConversationResult conversationResult, Course course, StudentCourse studentCourse, Result result)
        {
            double? finalAverageGrade = 0;
            double? finalTestPoints = JsonSerializer.Deserialize<double[]>(result.Tasks).Sum(x => x);
            double? maxPoints = JsonSerializer.Deserialize<double[]>(result.Exam.Tasks).Sum(x => x);
            double? percentageGrade = finalTestPoints * 100 / maxPoints;
            if (percentageGrade == null || conversationResult.Grade == null)
                finalAverageGrade = 0;
            else finalAverageGrade = (percentageGrade / 10 + conversationResult.Grade) / 2;

            return new StudentEvaluationViewModel
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                FinalTestPoints = finalTestPoints,
                PercentageGrade = percentageGrade,
                ConversationGrade = conversationResult.Grade,
                FinalAverageGrade = finalAverageGrade,
                Priority = studentCourse.Priority
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
