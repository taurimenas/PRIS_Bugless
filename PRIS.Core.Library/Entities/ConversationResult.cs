using System.Collections.Generic;

namespace PRIS.Core.Library.Entities
{
    public class ConversationResult : EntityBase
    {
        public int? Grade { get; set; }
        public string Comment { get; set; }

        public int? StudentForeignKey { get; set; }
        public Student Student { get; set; }
    }
}
