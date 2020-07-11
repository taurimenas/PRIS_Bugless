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
                .Include(i => i.Result)
                .ThenInclude(u => u.Exam)
                .Include(x => x.ConversationResult)
                .Where(x => x.PassedExam == true)
                .ToListAsync();

            var conversationResults = new List<ConversationResult>();
            var courses = new List<Course>();

            foreach (var student in students)
            {
                var conversationResult = await _repository.FindByIdAsync<ConversationResult>(student.ConversationResultId);
                conversationResults.Add(conversationResult);

                var course = await _repository.Query<Course>()
                    .FirstOrDefaultAsync(x => x.StudentsCourses.FirstOrDefault(y => y.StudentId == student.Id && y.Priority == 1).StudentId == student.Id);
                courses.Add(course);

                //var course = await _courseRepository.Query<>()
            }

            var conversationResultViewModels = new List<ConversationResultViewModel>();
            var studentViewModels = new List<StudentViewModel>();
            var courseViewModels = new List<CourseViewModel>();
            var resultViewModels = new List<ResultViewModel>();

            students.ForEach(x => studentViewModels.Add(StudentsMappings.ToViewModel(x)));
            conversationResults.ForEach(x => conversationResultViewModels.Add(ConversationResultMappings.ToConversationResultViewModel(x)));
            courses.ForEach(x => courseViewModels.Add(CourseMappings.ToViewModel(x)));
            students.ForEach(x => resultViewModels.Add(ResultMappings.ToViewModel(x.Result)));

            return View(CourseMappings.ToViewModel(studentViewModels, conversationResultViewModels, courseViewModels, resultViewModels));
        }
    }
}
