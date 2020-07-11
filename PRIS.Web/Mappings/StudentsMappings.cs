using System.Text.Json;
using System.Text.Json.Serialization;
using PRIS.Core.Library.Entities;
using PRIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRIS.Core.Library;

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
                Gender = (Gender)model.Gender,
                Comment = model.Comment,
                PassedExam = (bool)model.PassedExam
            };
        }

        public static void ToEntity(Student student, StudentViewModel model)
        {
            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.Email = model.Email;
            student.PhoneNumber = model.PhoneNumber;
            student.Gender = (Gender)model.Gender;
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
                Tasks = JsonSerializer.Deserialize<double[]>(entity.Exam.Tasks)
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
                Tasks = resultEntity.Tasks == null ? new double[] { 0 } : JsonSerializer.Deserialize<double[]>(resultEntity.Tasks),
                CommentResult = resultEntity.Comment,
                ExamId = resultEntity.ExamId,
                StudentForeignKey = resultEntity.StudentForeignKey
            };
        }
        public static StudentsResultViewModel ToStudentsResultViewModel(double[] tasks)
        {
            return new StudentsResultViewModel
            {
                Tasks = tasks
            };
        }
    }
}
