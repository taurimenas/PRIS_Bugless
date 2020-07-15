using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            var students = await _repository.Query<Student>()
                .Include(x => x.ConversationResult)
                .Include(x => x.Result)
                .ThenInclude(x => x.Exam)
                .Include(x => x.StudentCourses)
                .ThenInclude(x => x.Course)
                .Where(x => x.PassedExam == true)
                .ToListAsync();
            var invitationToStudy = new List<StudentInvitationToStudyViewModel>();
                students.ForEach(x => invitationToStudy
                .Add(StudentInvitationToStudyMappings
                .StudentInvitationToStudyToViewModel(x, x.ConversationResult, x.StudentCourses.FirstOrDefault(y => y.Priority == 1), x.Result)));

            return View(invitationToStudy);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InvitationToStudy(int[] HasInvitedToStudy)
        {
            if (ModelState.IsValid)
            {
                var students = await _repository.Query<Student>()
                    .Include(x => x.Result)
                    .Include(x => x.ConversationResult)
                    .Where(x => x.Id > 0)
                    //pataisyti examid == ???
                    //.Where(x => x.Result.Exam.Id > 1)
                    .ToListAsync();
                students.ForEach(x => x.InvitedToStudy = false);
                for (int i = 0; i < HasInvitedToStudy.Length; i++)
                {
                    var findStudents = students.FirstOrDefault(x => x.Id == HasInvitedToStudy[i]);
                    //null exception
                    findStudents.InvitedToStudy = true;
                }
                await _repository.SaveAsync();
                return RedirectToAction("Index", "StudentInvitationToStudy");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
