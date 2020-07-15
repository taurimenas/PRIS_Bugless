using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Index(int exam, string searchString, string sortOrder)
        {
            ViewBag.PercentageGradeSort = string.IsNullOrEmpty(sortOrder) ? "PercentageGrade" : "";
            ViewBag.ConversationGradeSort = string.IsNullOrEmpty(sortOrder) ? "ConversationGrade" : "";
            ViewBag.FinalAverageGradeSort = string.IsNullOrEmpty(sortOrder) ? "FinalAverageGrade" : "";
            ViewBag.PrioritySort = string.IsNullOrEmpty(sortOrder) ? "Priority" : "";

            var examDates = await _repository.Query<Exam>().ToListAsync();

            var stringExamDates = new List<SelectListItem>();
            foreach (var ed in examDates)
            {
                stringExamDates.Add(new SelectListItem { Value = examDates.FindIndex(a => a == ed).ToString(), Text = ed.Date.ToString() });
            }
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

            if (!string.IsNullOrEmpty(searchString))
            {
                if (invitationToStudy.Where(s => s.LastName.Contains(searchString)).Count() == 0)
                    invitationToStudy = invitationToStudy.Where(s => s.FirstName.Contains(searchString)).ToList();
                else invitationToStudy = invitationToStudy.Where(s => s.LastName.Contains(searchString)).ToList();
            }


            invitationToStudy = sortOrder switch
            {
                "PercentageGrade" => invitationToStudy.OrderByDescending(s => s.PercentageGrade).ToList(),
                "ConversationGrade" => invitationToStudy.OrderByDescending(s => s.ConversationGrade).ToList(),
                "FinalAverageGrade" => invitationToStudy.OrderBy(s => s.FinalAverageGrade).ToList(),
                "Priority" => invitationToStudy.OrderByDescending(s => s.Priority).ToList(),
                _ => invitationToStudy.OrderByDescending(s => s.FinalAverageGrade).ToList(),
            };

            var model = StudentInvitationToStudyMappings.ToListViewModel(invitationToStudy);
            model.Exams = stringExamDates;

            return View(model);
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
