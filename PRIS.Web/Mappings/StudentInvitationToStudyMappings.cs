using PRIS.Core.Library.Entities;
using PRIS.Web.Models.InvitationToStudyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PRIS.Web.Mappings
{
    public class StudentInvitationToStudyMappings
    {
        public static StudentInvitationToStudyViewModel StudentInvitationToStudyToViewModel(Student student, ConversationResult conversationResult, StudentCourse studentCourse, Result result)
        {
            double? finalAverageGrade = 0;
            double? finalTestPoints = JsonSerializer.Deserialize<double[]>(result.Tasks).Sum(x => x);
            double? maxPoints = JsonSerializer.Deserialize<double[]>(result.Exam.Tasks).Sum(x => x);
            double? percentageGrade = finalTestPoints * 100 / maxPoints;
            if (percentageGrade == null || conversationResult == null)
                finalAverageGrade = null;
            else finalAverageGrade = (percentageGrade / 10 + conversationResult.Grade) / 2;

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
                Priority = studentCourse?.Course.Title,
                InvitedToStudy = student.InvitedToStudy,
            };
        }

        public static StudentInvitationToStudyListViewModel ToListViewModel(List<StudentInvitationToStudyViewModel> studentInvitationToStudy)
        {
            return new StudentInvitationToStudyListViewModel
            {
                StudentInvitationToStudy = studentInvitationToStudy
            };
        }
    }
}
