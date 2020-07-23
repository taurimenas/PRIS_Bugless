using PRIS.Core.Library.Entities;
using PRIS.Web.Models.InvitationToStudyModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PRIS.Web.Mappings
{
    public class StudentInvitationToStudyMappings
    {
        public static StudentInvitationToStudyViewModel StudentInvitationToStudyToViewModel(Student student, ConversationResult conversationResult, IEnumerable<StudentCourse> studentCourse, Result result)
        {
            double? finalAverageGrade = 0;
            double? finalTestPoints = JsonSerializer.Deserialize<double[]>(result.Tasks).Sum(x => x);
            double? maxPoints = JsonSerializer.Deserialize<double[]>(result.Exam.Tasks).Sum(x => x);
            double? percentageGrade = finalTestPoints * 100 / maxPoints;
            if (percentageGrade == null || conversationResult == null)
                finalAverageGrade = null;
            else finalAverageGrade = (finalTestPoints + conversationResult.Grade) / 2;

            if (studentCourse.Count() > 0)
                studentCourse = studentCourse.Where(q => q.Priority != null);

            var priorities = "";
            priorities += studentCourse.Count() >= 1 ? "1) " + studentCourse?.FirstOrDefault(x => x?.Priority == 1).Course?.Title : "";
            priorities += studentCourse.Count() >= 2 ? "  2) " + studentCourse?.FirstOrDefault(x => x?.Priority == 2).Course?.Title : "";
            priorities += studentCourse.Count() >= 3 ? "  3) " + studentCourse?.FirstOrDefault(x => x?.Priority == 3).Course?.Title : "";

            return new StudentInvitationToStudyViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                FinalTestPoints = finalTestPoints,
                PercentageGrade = percentageGrade,
                ConversationGrade = conversationResult?.Grade,
                FinalAverageGrade = finalAverageGrade,
                Priority = priorities,
                InvitedToStudy = student.InvitedToStudy,
                CityId = result?.Exam.City.Id,
                ExamId = result?.Exam.Id,
                CourseId = studentCourse.Count() >= 1 ? studentCourse?.FirstOrDefault(x => x?.Priority == 1).Course?.Id : null,
            };
        }

        public static StudentInvitationToStudyListViewModel ToListViewModel(List<StudentInvitationToStudyViewModel> studentInvitationToStudy)
        {
            return new StudentInvitationToStudyListViewModel
            {
                StudentInvitationToStudy = studentInvitationToStudy,
            };
        }
        public static Student EditEntity(Student student, StudentComment studentFromModel)
        {
            student.Id = studentFromModel.Id;
            student.Comment = studentFromModel.Comment;
            return student;
        }
    }
}
