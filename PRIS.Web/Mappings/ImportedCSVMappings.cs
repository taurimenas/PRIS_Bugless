using Microsoft.EntityFrameworkCore.Internal;
using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Mappings
{
    public class ImportedCSVMappings
    {
        public static void ToEntity(Student student, Result result, ConversationResult conversationResult, ImportStudentsDataModel importStudentsDataModel)
        {
            student.FirstName = importStudentsDataModel.FirstName;
            student.LastName = importStudentsDataModel.LastName;
            student.PassedExam = importStudentsDataModel.PassedExam;
            student.InvitedToStudy = importStudentsDataModel.InvitationToStudy;
            student.SignedAContract = importStudentsDataModel.SignedAContract;
            student.ResultId = result.Id;
            student.Result = result;
            student.ConversationResultId = conversationResult.Id;
            student.ConversationResult = conversationResult;
            student.Email = "-";
            student.PhoneNumber = "-";
            result.Student = student;
            result.StudentForeignKey = student.Id;
            result.Tasks = importStudentsDataModel.Tasks;
            conversationResult.Grade = importStudentsDataModel.ConversationResult;
            conversationResult.StudentForeignKey = student.Id;
            conversationResult.Student = student;
        }
    }
}
