using System;

namespace PRIS.Core.Library.Entities
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
