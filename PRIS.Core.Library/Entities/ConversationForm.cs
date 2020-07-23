using System;
using System.Collections.Generic;
using System.Text;

namespace PRIS.Core.Library.Entities
{
    public class ConversationForm : EntityBase
    {
        public string Field { get; set; }

        public int? ConversationResultId { get; set; }
        public ConversationResult ConversationResult { get; set; }
    }
}
