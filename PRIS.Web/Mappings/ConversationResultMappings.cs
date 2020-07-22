using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace PRIS.Web.Mappings
{
    public class ConversationResultMappings
    {
        public static ConversationResultViewModel ToViewModel(Student studentEntity, ConversationResult conversationResultEntity, int? examId)
        {
            return new ConversationResultViewModel
            {
                StudentId = studentEntity.Id,
                FirstName = studentEntity.FirstName,
                LastName = studentEntity.LastName,
                Email = studentEntity.Email,
                PhoneNumber = studentEntity.PhoneNumber,
                ConversationResultComment = conversationResultEntity.Comment,
                ConversationResultId = (int)studentEntity.ConversationResultId,
                Grade = conversationResultEntity.Grade == null ? 0 : conversationResultEntity.Grade,
                ExamId = examId
            };
        }
        public static void EditEntity(ConversationResult conversationResult, ConversationResultViewModel model)
        {
            conversationResult.Grade = model.Grade;
            conversationResult.Comment = model.ConversationResultComment;
        }
        public static Student ToStudentEntity(StudentViewModel model)
        {
            return new Student
            {
                FirstName = model.FirstName,
            };
        }
        public static ConversationAndStudentViewModel ToStudentAndConversationResultViewModel(List<StudentViewModel> studentViewModels, List<ConversationResultViewModel> conversationResultViewModels, int? examId, List<ConversationFormViewModel> conversationFormViewModels)
        {
            return new ConversationAndStudentViewModel
            {
                Students = studentViewModels,
                ConvResults = conversationResultViewModels,
                ExamId = examId,
                ConversationForm = conversationFormViewModels
            };
        }
        public static void EditConversationFormEntity(List<ConversationForm> conversationForm, ConversationFormViewModel model)
        {

            conversationForm.ElementAt(0).Field = model.Field1;
            conversationForm.ElementAt(1).Field = model.Field2;
            conversationForm.ElementAt(2).Field = model.Field3;
            conversationForm.ElementAt(3).Field = model.Field4;
            conversationForm.ElementAt(4).Field = model.Field5;
            conversationForm.ElementAt(5).Field = model.Field6;
            conversationForm.ElementAt(6).Field = model.Field7;
            conversationForm.ElementAt(7).Field = model.Field8;
            conversationForm.ElementAt(8).Field = model.Field9;
            conversationForm.ElementAt(9).Field = model.Field10;

        }
        public static ConversationFormViewModel ToConversationFormViewModel(ConversationForm conversationFormEntity)
        {
            return new ConversationFormViewModel
            {
                Id = conversationFormEntity.Id,
                ConversationFormName = conversationFormEntity.Field,
                Field1 = conversationFormEntity.Field,
                Field2 = conversationFormEntity.Field,
                Field3 = conversationFormEntity.Field,
                Field4 = conversationFormEntity.Field,
                Field5 = conversationFormEntity.Field,
                Field6 = conversationFormEntity.Field,
                Field7 = conversationFormEntity.Field,
                Field8 = conversationFormEntity.Field,
                Field9 = conversationFormEntity.Field,
                Field10 = conversationFormEntity.Field,
            };
        }
        public static ConversationFormViewModel ToConversationFormViewModel(List<ConversationForm> conversationFormEntity, Student student, int? examId)
        {
            return new ConversationFormViewModel
            {
                Field1 = conversationFormEntity.ElementAt(0).Field,
                Field2 = conversationFormEntity.ElementAt(1).Field,
                Field3 = conversationFormEntity.ElementAt(2).Field,
                Field4 = conversationFormEntity.ElementAt(3).Field,
                Field5 = conversationFormEntity.ElementAt(4).Field,
                Field6 = conversationFormEntity.ElementAt(5).Field,
                Field7 = conversationFormEntity.ElementAt(6).Field,
                Field8 = conversationFormEntity.ElementAt(7).Field,
                Field9 = conversationFormEntity.ElementAt(8).Field,
                Field10 = conversationFormEntity.ElementAt(9).Field,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StudentId = student.Id,
                ExamId = examId
            };
        }
        public static StudentViewModel ToStudentViewModel(Student studentEntity)
        {
            return new StudentViewModel
            {
                Id = studentEntity.Id,
                FirstName = studentEntity.FirstName,
                LastName = studentEntity.LastName,
                Email = studentEntity.Email,
                PhoneNumber = studentEntity.PhoneNumber
            };
        }
        public static ConversationResultViewModel ToConversationResultViewModel(ConversationResult conversationResultEntity)
        {
            if (conversationResultEntity == null)
            {
                return new ConversationResultViewModel
                {
                    Grade = null,
                    ConversationResultComment = null,
                };
            }
            return new ConversationResultViewModel
            {
                ConversationResultId = conversationResultEntity.Id,
                Grade = conversationResultEntity?.Grade,
            };
        }
    }
}

