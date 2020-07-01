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
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                Comment = model.Comment,
                PassedExam = model.PassedExam
            };
        }

        public static void ToEntity(Student student, StudentViewModel model)
        {
            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.Email = model.Email;
            student.PhoneNumber = model.PhoneNumber;
            student.Gender = model.Gender;
            student.Comment = model.Comment;
        }

        public static Result ToResultEntity(StudentsResultViewModel model)
        {
            return new Result
            {
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
                Comment = model.CommentResult,
                StudentForeignKey = model.StudentForeignKey
            };

        }
        public static void ToResultEntity(Result result, StudentsResultViewModel model)
        {
            result.Task1_1 = model.Task1_1;
            result.Task1_2 = model.Task1_2;
            result.Task1_3 = model.Task1_3;
            result.Task2_1 = model.Task2_1;
            result.Task2_2 = model.Task2_2;
            result.Task2_3 = model.Task2_3;
            result.Task3_1 = model.Task3_1;
            result.Task3_2 = model.Task3_2;
            result.Task3_3 = model.Task3_3;
            result.Task3_4 = model.Task3_4;
            result.Comment = model.CommentResult;
        }
        public static StudentViewModel ToViewModel(Student entity)
        {
            return new StudentViewModel
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Gender = entity.Gender,
                Comment = entity.Comment,
                PassedExam = entity.PassedExam
            };
        }
        public static StudentsResultViewModel ToStudentsResultViewModel(Student entity)
        {
            return new StudentsResultViewModel
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Gender = entity.Gender,
                Comment = entity.Comment,
                PassedExam = entity.PassedExam
            };
        }

        public static StudentsResultViewModel ToViewModel(Student studentEntity, Result resultEntity)
        {
            return new StudentsResultViewModel
            {
                Id = studentEntity.Id,
                FirstName = studentEntity.FirstName,
                LastName = studentEntity.LastName,
                Email = studentEntity.Email,
                PhoneNumber = studentEntity.PhoneNumber,
                Gender = studentEntity.Gender,
                Comment = studentEntity.Comment,
                PassedExam = studentEntity.PassedExam,
                ResultId = resultEntity.Id,
                Task1_1 = resultEntity.Task1_1,
                Task1_2 = resultEntity.Task1_2,
                Task1_3 = resultEntity.Task1_3,
                Task2_1 = resultEntity.Task2_1,
                Task2_2 = resultEntity.Task2_2,
                Task2_3 = resultEntity.Task2_3,
                Task3_1 = resultEntity.Task3_1,
                Task3_2 = resultEntity.Task3_2,
                Task3_3 = resultEntity.Task3_3,
                Task3_4 = resultEntity.Task3_4,
                CommentResult = resultEntity.Comment,
                StudentForeignKey = resultEntity.StudentForeignKey
            };
        }
    }
}
