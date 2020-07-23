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
            for (int i = 0; i < conversationForm.Count(); i++)
            {
                conversationForm.ElementAt(i).Field = model.Fields.ElementAt(i);
            }
        }
        public static ConversationFormViewModel ToConversationFormViewModel(List<ConversationForm> conversationFormEntity, Student student, int? examId)
        {
            return new ConversationFormViewModel
            {
                Fields = conversationFormEntity.Select(x=>x.Field).ToArray(),
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

