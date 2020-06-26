using PRIS.Core.Library.Entities;
using PRIS.Web.Models;

namespace PRIS.Web.Mappings
{
    public class ResultMappings
    {
        public static Result ToResultEntity(ResultViewModel model)
        {
            return new Result
            {
                Id = model.Id,
                Task1_1 = model.Task1_1,
                Task1_2 = model.Task1_2,
                Task1_3 = model.Task1_3,
                Task2_1 = model.Task2_1,
                Task2_2 = model.Task2_2,
                Task2_3 = model.Task2_3,
                Task3_1 = model.Task3_1,
                Task3_2 = model.Task3_2,
                Task3_3 = model.Task3_3,
                Task3_4 = model.Task3_4,
                Comment = model.Comment
            };

        }

        public static ResultViewModel ToResultViewModel(Result entity)
        {
            return new ResultViewModel
            {
                Id = entity.Id,
                Task1_1 = entity.Task1_1,
                Task1_2 = entity.Task1_2,
                Task1_3 = entity.Task1_3,
                Task2_1 = entity.Task2_1,
                Task2_2 = entity.Task2_2,
                Task2_3 = entity.Task2_3,
                Task3_1 = entity.Task3_1,
                Task3_2 = entity.Task3_2,
                Task3_3 = entity.Task3_3,
                Task3_4 = entity.Task3_4,
                Comment = entity.Comment
            };
        }


        public static Student ToStudentEntity(ResultViewModel model)
        {
            return new Student
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

        }

        public static ResultViewModel ToResultViewModel(Student entity)
        {
            return new ResultViewModel
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber
            };
        }
    }
}
