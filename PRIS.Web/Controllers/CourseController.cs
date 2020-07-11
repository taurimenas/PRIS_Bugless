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
        private readonly Repository<Exam> _examRepository;
        private readonly Repository<City> _cityRepository;
        private readonly Repository<Student> _studentRepository;
        private readonly Repository<Result> _resultRepository;
        private readonly Repository<Course> _courseRepository;
        private readonly Repository<ConversationResult> _conversationResultRepository;


        public CourseController(Repository<Exam> examRepository, Repository<City> cityRepository, Repository<Student> studentRepository, Repository<Result> resultRepository, Repository<Course> courseRepository, Repository<ConversationResult> conversationResultRepository)
        {
            _examRepository = examRepository;
            _cityRepository = cityRepository;
            _studentRepository = studentRepository;
            _resultRepository = resultRepository;
            _courseRepository = courseRepository;
            _conversationResultRepository = conversationResultRepository;
        }
        public async Task<IActionResult> Index()
        {
            var students = await _courseRepository.Query<Student>()
                .Include(i => i.Result)
                .ThenInclude(u => u.Exam)
                .Where(x => x.PassedExam == true)
                .ToListAsync();

            var conversationResults = new List<ConversationResult>();
            foreach (var student in students)
            {
                var conversationResult = await _conversationResultRepository.FindByIdAsync(student.ConversationResultId);
                conversationResults.Add(conversationResult);
            }

            var courses = new List<Course>();
            foreach (var student in students)
            {
                var course = await _courseRepository.Query<Course>()
                    .FirstOrDefaultAsync(x => x.StudentsCourses.FirstOrDefault(y => y.StudentId == student.Id && y.Priority == 1).StudentId == student.Id);
                courses.Add(course);
            }

            var results = new List<Result>();
            foreach (var student in students)
            {
                var result = await _resultRepository.Query<Result>()
                    .FirstOrDefaultAsync(x => x.StudentForeignKey == student.Id);
                results.Add(result);
            }

            var conversationResultViewModels = new List<ConversationResultViewModel>();
            var studentViewModels = new List<StudentViewModel>();
            var courseViewModels = new List<CourseViewModel>();
            var resultViewModels = new List<ResultViewModel>();

            students.ForEach(x => studentViewModels.Add(StudentsMappings.ToViewModel(x)));
            conversationResults.ForEach(x => conversationResultViewModels.Add(ConversationResultMappings.ToConversationResultViewModel(x)));
            courses.ForEach(x => courseViewModels.Add(CourseMappings.ToViewModel(x)));
            results.ForEach(x => resultViewModels.Add(ResultMappings.ToViewModel(x)));

            return View(CourseMappings.ToViewModel(studentViewModels, conversationResultViewModels, courseViewModels, resultViewModels));
        }
    }
}
