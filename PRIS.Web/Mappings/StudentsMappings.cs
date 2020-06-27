using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Mappings
{
    public class StudentsMappings
    {
        public static Student ToEntity(StudentViewModel model)
        {
            return new Student
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                Comment = model.Comment
            };
        }
        public static StudentViewModel ToViewModel(Student entity)
        {
            return new StudentViewModel
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Gender = entity.Gender,
                Comment = entity.Comment,
                Id = entity.Id
            };
        }
    }
}
