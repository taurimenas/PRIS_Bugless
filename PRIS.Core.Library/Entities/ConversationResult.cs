    using System.Collections.Generic;

namespace PRIS.Core.Library.Entities
{
    public class ConversationResult : EntityBase
    {
        public int? Grade { get; set; }
        public string Comment { get; set; }

        public List<Student> Students { get; set; }
    }
}
