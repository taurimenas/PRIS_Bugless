using System;

namespace PRIS.Core.Library.Entities
{
    public class EntityBase : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
