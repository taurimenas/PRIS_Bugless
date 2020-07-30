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
        public static void ToEntityWithoutConversationResult(Student student, Result result, ImportedStudentsDataModel importedStudentsDataModel)
        {
            student.FirstName = importedStudentsDataModel.FirstName;
            student.LastName = importedStudentsDataModel.LastName;
            student.PassedExam = importedStudentsDataModel.PassedExam;
            student.InvitedToStudy = importedStudentsDataModel.InvitationToStudy;
            student.SignedAContract = importedStudentsDataModel.SignedAContract;
            student.ResultId = result.Id;
            student.Result = result;
            student.Email = importedStudentsDataModel.Email;
            student.PhoneNumber = importedStudentsDataModel.PhoneNumber;
            result.Student = student;
            result.StudentForeignKey = student.Id;
            result.Tasks = importedStudentsDataModel.Tasks;
        }
        public static void ToEntity(Student student, Result result, ConversationResult conversationResult, ImportedStudentsDataModel importedStudentsDataModel)
        {
            student.FirstName = importedStudentsDataModel.FirstName;
            student.LastName = importedStudentsDataModel.LastName;
            student.PassedExam = importedStudentsDataModel.PassedExam;
            student.InvitedToStudy = importedStudentsDataModel.InvitationToStudy;
            student.SignedAContract = importedStudentsDataModel.SignedAContract;
            student.ResultId = result.Id;
            student.Result = result;
            student.ConversationResultId = conversationResult.Id;
            student.ConversationResult = conversationResult;
            student.Email = importedStudentsDataModel.Email;
            student.PhoneNumber = importedStudentsDataModel.PhoneNumber;
            result.Student = student;
            result.StudentForeignKey = student.Id;
            result.Tasks = importedStudentsDataModel.Tasks;
            conversationResult.Grade = importedStudentsDataModel.ConversationResult;
            conversationResult.StudentForeignKey = student.Id;
            conversationResult.Student = student;
        }
    }
}
