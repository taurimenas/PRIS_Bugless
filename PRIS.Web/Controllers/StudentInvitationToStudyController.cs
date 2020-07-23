using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MoreLinq;
using PRIS.Core.Library.Entities;
using PRIS.Web.Mappings;
using PRIS.Web.Models.InvitationToStudyModel;
using PRIS.Web.Storage;

namespace PRIS.Web.Controllers
{
    public class StudentInvitationToStudyController : Controller
    {
        private readonly IRepository _repository;
        public StudentInvitationToStudyController(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index(string examId, string courseId, string cityId, string searchString, string sortOrder)
        {
            var students = await _repository.Query<Student>()
                .Include(x => x.ConversationResult)
                .Include(x => x.Result)
                .ThenInclude(x => x.Exam)
                .ThenInclude(x => x.City)
                .Include(x => x.StudentCourses)
                .ThenInclude(x => x.Course)
                .Where(x => x.PassedExam == true)
                .ToListAsync();

            var model = StudentData.PrepareData(examId, courseId, cityId, searchString, sortOrder, ref students);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int[] studentId, int[] HasInvitedToStudy)
        {
            if (ModelState.IsValid)
            {
                var students = new List<Student>();
                for (int i = 0; i < studentId.Length; i++)
                {
                    students.Add(await _repository.FindByIdAsync<Student>(studentId[i]));
                }
                students.ForEach(x => x.InvitedToStudy = false);
                for (int i = 0; i < HasInvitedToStudy.Length; i++)
                {
                    var findStudents = students.FirstOrDefault(x => x.Id == HasInvitedToStudy[i]);
                    findStudents.InvitedToStudy = true;
                }
                await _repository.SaveAsync();
                return RedirectToAction("Index", "StudentInvitationToStudy");
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> EditComment(int? id)
        {
            var student = await _repository.FindByIdAsync<Student>(id);
            StudentComment studentModel = new StudentComment
            {
                StudentFirstName = student.FirstName,
                StudentLastName = student.LastName,
                Comment = student.Comment,
            };
            return View(studentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(StudentComment model)
        {
            if (ModelState.IsValid)
            {
                var student = await _repository.FindByIdAsync<Student>(model.Id);
                student.Comment = model.Comment;
                await _repository.SaveAsync();
                return RedirectToAction("Index", "StudentInvitationToStudy");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
