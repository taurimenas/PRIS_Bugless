using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Mappings
{
    public class ConversationResultMappings
    {
        public static ConversationResultViewModel ToViewModel(Student studentEntity, ConversationResult conversationResultEntity)
        {
            return new ConversationResultViewModel
            {
                StudentId = studentEntity.Id,
                FirstName = studentEntity.FirstName,
                LastName = studentEntity.LastName,
                ConversationResultComment = conversationResultEntity.Comment,
                ConversationResultId = conversationResultEntity.Id,
                Grade = conversationResultEntity.Grade
            };
        }
        public static void EditEntity(ConversationResult conversationResult, ConversationResultViewModel model)
        {
            conversationResult.Grade = model.Grade;
            conversationResult.Comment = model.ConversationResultComment;
        }
    }
}
