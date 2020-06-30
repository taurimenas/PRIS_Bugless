using PRIS.Core.Library.Entities;
using PRIS.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PRIS.Web.Storage
{
    public class StudentRepository : Repository<Student, ApplicationDbContext>
    {
        public StudentRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async override Task<List<Student>> GetAllAsync()
        {
            var students = await base.GetAllAsync();
            return students.OrderByDescending(x => x.Id).ToList();
        }
    }
}
