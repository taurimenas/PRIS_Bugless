using PRIS.Core.Library.Entities;
using PRIS.Web.Data.Migrations;
using PRIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                Grade = conversationResultEntity.Grade == null?0:conversationResultEntity.Grade,
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
        public static ConversationAndStudentViewModel ToStudentAndConversationResultViewModel(List<StudentViewModel> studentViewModels, List<ConversationResultViewModel> conversationResultViewModels, int? examId)
        {
            return new ConversationAndStudentViewModel
            {
                Students = studentViewModels,
                ConvResults = conversationResultViewModels,
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
            if(conversationResultEntity == null)
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

