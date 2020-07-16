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
        public async Task<IActionResult> Index(int? examId, int? courseId, int? cityId, string searchString, string sortOrder)
        {
            ViewBag.PercentageGradeSort = string.IsNullOrEmpty(sortOrder) ? "PercentageGrade" : "";
            ViewBag.ConversationGradeSort = string.IsNullOrEmpty(sortOrder) ? "ConversationGrade" : "";
            ViewBag.FinalAverageGradeSort = string.IsNullOrEmpty(sortOrder) ? "FinalAverageGrade" : "";
            ViewBag.PrioritySort = string.IsNullOrEmpty(sortOrder) ? "Priority" : "";

            var exams = await _repository.Query<Exam>().ToListAsync();
            var stringAcceptancePeriods = new List<SelectListItem>();
            foreach (var ed in exams)
            {
                stringAcceptancePeriods.Add(new SelectListItem { Value = exams.FindIndex(a => a == ed).ToString(), Text = ed.AcceptancePeriod });
            }


            var cities = await _repository.Query<City>().ToListAsync();
            var stringCities = new List<SelectListItem>();
            foreach (var c in cities)
            {
                stringCities.Add(new SelectListItem { Value = cities.FindIndex(a => a == c).ToString(), Text = c.Name });
            }

            var courses = await _repository.Query<Course>().ToListAsync();
            var stringCourses = new List<SelectListItem>();
            foreach (var p in courses)
            {
                stringCourses.Add(new SelectListItem { Value = courses.FindIndex(a => a == p).ToString(), Text = p.Title });
            }

            var students = await _repository.Query<Student>()
                .Include(x => x.ConversationResult)
                .Include(x => x.Result)
                .ThenInclude(x => x.Exam)
                .ThenInclude(x => x.City)
                .Include(x => x.StudentCourses)
                .ThenInclude(x => x.Course)
                .Where(x => x.PassedExam == true)
                .ToListAsync();

            var invitationToStudy = new List<StudentInvitationToStudyViewModel>();
            students.ForEach(x => invitationToStudy
                .Add(StudentInvitationToStudyMappings
                .StudentInvitationToStudyToViewModel(x, x.ConversationResult, x.StudentCourses, x.Result)));

            if (!string.IsNullOrEmpty(searchString))
            {
                if (invitationToStudy.Where(s => s.LastName.Contains(searchString)).Count() == 0)
                    invitationToStudy = invitationToStudy.Where(s => s.FirstName.Contains(searchString)).ToList();
                else invitationToStudy = invitationToStudy.Where(s => s.LastName.Contains(searchString)).ToList();
            }
            if (examId != null)
            {
                invitationToStudy = invitationToStudy.Where(e => e.ExamId == exams.ElementAt((int)examId).Id).ToList();
            }
            if (cityId != null)
            {
                invitationToStudy = invitationToStudy.Where(e => e.CityId == cities.ElementAt((int)cityId).Id).ToList();
            }
            if (courseId != null)
            {
                invitationToStudy = invitationToStudy.Where(e => e.CourseId == courses.ElementAt((int)courseId).Id).ToList();
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
            model.AcceptancePeriods = stringAcceptancePeriods;
            model.Cities = stringCities;
            model.Courses = stringCourses;

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
    }
}
