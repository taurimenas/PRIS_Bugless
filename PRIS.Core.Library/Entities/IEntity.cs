using System;
using System.Collections.Generic;
using System.Text;

namespace PRIS.Core.Library.Entities
{
    public interface IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
    }
}
