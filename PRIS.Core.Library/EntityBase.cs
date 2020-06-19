using System;
using System.Collections.Generic;
using System.Text;

namespace PRIS.Core.Library
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
