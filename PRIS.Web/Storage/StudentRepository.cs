using PRIS.Core.Library.Entities;
using PRIS.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Storage
{
    public class StudentRepository : Repository<Student, ApplicationDbContext>
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
