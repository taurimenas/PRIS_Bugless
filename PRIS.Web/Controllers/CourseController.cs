using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

            var studentEvaluations = new List<StudentEvaluationViewModel>();
            students.ForEach(x => studentEvaluations.Add(CourseMappings.ToViewModel(x, x.ConversationResult, x.StudentCourses.FirstOrDefault(y => y.Priority == 1), x.Result)));

            studentEvaluations = studentEvaluations.OrderByDescending(x => x.FinalAverageGrade).ToList();

            return View(CourseMappings.ToViewModel(studentEvaluations));
        }
    }
}
