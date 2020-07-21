    using System.Collections.Generic;

namespace PRIS.Core.Library.Entities
{
    public class ConversationResult : EntityBase
    {
        public double? Grade { get; set; }
        public string Comment { get; set; }

        public int? StudentForeignKey { get; set; }
        public Student Student { get; set; }
        public List<ConversationForm> ConversationForm { get; set; }
    }
}
