using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRIS.Core.Library.Entities;
using PRIS.Web.Mappings;
using PRIS.Web.Models;
using PRIS.Web.Models.CourseModels;
using PRIS.Web.Storage;

namespace PRIS.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly IRepository _repository;

        public CourseController(IRepository repository)
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

            //List<string> AcceptancePeriod = CalculateAcceptancePeriods(examViewModels);
            //var AcceptancePeriods = new List<SelectListItem>();
            //foreach (var ap in AcceptancePeriod)
            //{
            //    AcceptancePeriods.Add(new SelectListItem { Value = AcceptancePeriod.FindIndex(a => a == ap).ToString(), Text = ap });
            //}
            //viewModel.ExamViewModels = examViewModels.Where(x => x.SetAcceptancePeriod == AcceptancePeriods.ElementAt(value).Text).ToList();
            //var SelectedAcceptancePeriod = AcceptancePeriods.ElementAt(value);
            //viewModel.SelectedAcceptancePeriod = SelectedAcceptancePeriod.Text;
            //TempData["SelectedAcceptancePeriod"] = SelectedAcceptancePeriod.Value;

            var students = await _repository.Query<Student>()
                .Include(x => x.ConversationResult)
                .Include(x => x.Result)
                .ThenInclude(x => x.Exam)
                .Include(x => x.StudentCourses)
                .ThenInclude(x => x.Course)
                .Where(x => x.PassedExam == true)
                //.Where(x => x.Result.Exam.Date.ToString() == stringExamDates.ElementAt(exam).Text)
                .ToListAsync();

            var studentEvaluations = new List<StudentEvaluationViewModel>();
            students.ForEach(x => studentEvaluations.Add(CourseMappings.ToViewModel(x, x.ConversationResult, x.StudentCourses.FirstOrDefault(y => y.Priority == 1), x.Result)));

            //studentEvaluations = studentEvaluations.OrderByDescending(x => x.FinalAverageGrade).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (studentEvaluations.Where(s => s.LastName.Contains(searchString)).Count() == 0)
                    studentEvaluations = studentEvaluations.Where(s => s.FirstName.Contains(searchString)).ToList();
                else studentEvaluations = studentEvaluations.Where(s => s.LastName.Contains(searchString)).ToList();
            }


            studentEvaluations = sortOrder switch
            {
                "PercentageGrade" => studentEvaluations.OrderByDescending(s => s.PercentageGrade).ToList(),
                "ConversationGrade" => studentEvaluations.OrderByDescending(s => s.ConversationGrade).ToList(),
                "FinalAverageGrade" => studentEvaluations.OrderBy(s => s.FinalAverageGrade).ToList(),
                "Priority" => studentEvaluations.OrderByDescending(s => s.Priority).ToList(),
                _ => studentEvaluations.OrderByDescending(s => s.FinalAverageGrade).ToList(),
            };
            var model = CourseMappings.ToViewModel(studentEvaluations);
            model.Exams = stringExamDates;

            return View(model);
        }
    }
}
