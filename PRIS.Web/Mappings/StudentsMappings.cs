using System.Text.Json;
using System.Text.Json.Serialization;
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

        public static void ToResultEntity(Result result, StudentsResultViewModel model)
        {
            result.Tasks = JsonSerializer.Serialize(model.Tasks);
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

        public static StudentsResultViewModel ToStudentsResultViewModel(Result entity)
        {
            return new StudentsResultViewModel
            {
                Tasks = JsonSerializer.Deserialize<int[]>(entity.Exam.Tasks)
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
                Tasks = JsonSerializer.Deserialize<int[]>(resultEntity.Tasks),
                CommentResult = resultEntity.Comment,
                ExamId = resultEntity.ExamId,
                StudentForeignKey = resultEntity.StudentForeignKey
            };
        }
        public static StudentsResultViewModel ToStudentsResultViewModel(int[] tasks)
        {
            return new StudentsResultViewModel
            {
                Tasks = tasks
            };
        }
    }
}
