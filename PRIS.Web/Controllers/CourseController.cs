using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
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
        public async Task<IActionResult> Index(string examId, int? courseId, int? cityId, string searchString, string sortOrder)
        {
            AddSorting(sortOrder);

            var exams = await _repository.Query<Exam>()
                .Include(x => x.Results)
                .ToListAsync();
            var stringAcceptancePeriods = new List<SelectListItem>();
            var filteredExams = exams.DistinctBy(x => x.AcceptancePeriod).ToList();
            foreach (var ed in filteredExams)
            {
                stringAcceptancePeriods.Add(new SelectListItem { Value = filteredExams.FindIndex(a => a == ed).ToString(), Text = ed.AcceptancePeriod });
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
                stringCourses.Add(new SelectListItem { Value = courses.FindIndex(a => a == p).ToString(), Text = $"{p.Title} {p.StartYear.Year}" });
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
            List<StudentEvaluationViewModel> studentEvaluations = AddFilterOnViewModel(examId, courseId, cityId, searchString, cities, courses, ref students);

            studentEvaluations = SortOnSwitch(sortOrder, studentEvaluations);
            //TODO:
            //var SelectedAcceptancePeriod = "";
            //if (examId != null)
            //    SelectedAcceptancePeriod = stringAcceptancePeriods.ElementAt(0).Text; //TODO: vietoj 0 examId is kazkur turi ateit. Gal firstOrdefault pagal stringa is POSTO
            //var SelectedCity = "";
            //if (cityId != null)
            //    SelectedCity = stringCities.ElementAt((int)cityId).Text;
            //var SelectedCourse = "";
            //if (courseId != null)
            //    SelectedCourse = stringCourses.ElementAt((int)courseId).Text;

            var model = CourseMappings.ToViewModel(studentEvaluations);

            model.AcceptancePeriods = stringAcceptancePeriods;
            var selectedAcceptancePeriods = stringAcceptancePeriods.FirstOrDefault(x => x.Text == examId);
            if (selectedAcceptancePeriods == null)
                model.SelectedAcceptancePeriod = null;
            else
                model.SelectedAcceptancePeriod = selectedAcceptancePeriods.Text;

            model.Cities = stringCities;
            var selectedCity = stringCities.FirstOrDefault(x => x.Value == cityId.ToString());
            if (selectedCity == null)
                model.SelectedCity = null;
            else
                model.SelectedCity = selectedCity.Text;

            model.Courses = stringCourses;
            var selectedCourse = stringCourses.FirstOrDefault(x => x.Value == cityId.ToString());
            if (selectedCourse == null)
                model.SelectedPriority = null;
            else
                model.SelectedPriority = selectedCourse.Text;
            return View(model);
        }



        public async Task<IActionResult> LockingOfStudentData(string examId, int? courseId, int? cityId, string searchString, string sortOrder)
        {
            AddSorting(sortOrder);

            var exams = await _repository.Query<Exam>()
                .Include(x => x.Results)
                .ToListAsync();
            var stringAcceptancePeriods = new List<SelectListItem>();
            var filteredExams = exams.DistinctBy(x => x.AcceptancePeriod).ToList();
            foreach (var ed in filteredExams)
            {
                stringAcceptancePeriods.Add(new SelectListItem { Value = filteredExams.FindIndex(a => a == ed).ToString(), Text = ed.AcceptancePeriod });
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
                stringCourses.Add(new SelectListItem { Value = courses.FindIndex(a => a == p).ToString(), Text = $"{p.Title} {p.StartYear.Year}" });
            }

            var students = await _repository.Query<Student>()
                .Include(x => x.ConversationResult)
                .Include(x => x.Result)
                .ThenInclude(x => x.Exam)
                .Include(x => x.StudentCourses)
                .ThenInclude(x => x.Course)
                .Where(x => x.InvitedToStudy == true)
                .ToListAsync();

            List<StudentLockDataViewModel> studentDataLocking = AddFilterOnStudentLockDataViewModel(examId, courseId, cityId, searchString, cities, courses, ref students);

            //var studentDataLocking = new List<StudentLockDataViewModel>();

            //students.ForEach(x => studentDataLocking
            //    .Add(CourseMappings
            //    .StudentLockDataToViewModel(x, x.ConversationResult, x.StudentCourses, x.Result)));

            studentDataLocking = SortOnSwitch(sortOrder, studentDataLocking);

            var model = CourseMappings.StudentLockDataListToViewModel(studentDataLocking);
            var selectedAcceptancePeriods = stringAcceptancePeriods.FirstOrDefault(x => x.Text == examId);
            if (selectedAcceptancePeriods == null)
                model.SelectedAcceptancePeriod = null;
            else
                model.SelectedAcceptancePeriod = selectedAcceptancePeriods.Text;

            model.Cities = stringCities;
            var selectedCity = stringCities.FirstOrDefault(x => x.Value == cityId.ToString());
            if (selectedCity == null)
                model.SelectedCity = null;
            else
                model.SelectedCity = selectedCity.Text;

            model.Courses = stringCourses;
            var selectedCourse = stringCourses.FirstOrDefault(x => x.Value == cityId.ToString());
            if (selectedCourse == null)
                model.SelectedPriority = null;
            else
                model.SelectedPriority = selectedCourse.Text;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockingOfStudentData(int[] studentId, int[] HasSignedAContract, int[] HasStudentDataLocked)
        {
            if (ModelState.IsValid)
            {

                var students = new List<Student>();
                for (int i = 0; i < studentId.Length; i++)
                {
                    students.Add(await _repository.FindByIdAsync<Student>(studentId[i]));
                }
                students.ForEach(x => x.SignedAContract = false);
                for (int i = 0; i < HasSignedAContract.Length; i++)
                {
                    var findStudents = students.FirstOrDefault(x => x.Id == HasSignedAContract[i]);
                    findStudents.SignedAContract = true;
                }

                var studentsForDataLock = new List<Student>();
                for (int i = 0; i < studentId.Length; i++)
                {
                    studentsForDataLock.Add(await _repository.FindByIdAsync<Student>(studentId[i]));
                }
                studentsForDataLock.ForEach(x => x.StudentDataLocked = false);
                for (int i = 0; i < HasStudentDataLocked.Length; i++)
                {
                    foreach (var student in studentsForDataLock)
                    {
                        if (student.SignedAContract == true)
                        {
                            var findStudents = studentsForDataLock.FirstOrDefault(x => x.Id == HasStudentDataLocked[i]);
                            if (findStudents.SignedAContract == true)
                            {
                                findStudents.StudentDataLocked = true;
                            }
                            else
                            {
                                ModelState.AddModelError("StudentDelete", "Studentas turi būti pasirašęs sutartį, kad būtų galima užrakinti studento duomenis");
                                TempData["ErrorMessage"] = "Studentas turi būti pasirašęs sutartį, kad būtų galima užrakinti studento duomenis";
                            }
                        }
                    }
                }
                await _repository.SaveAsync();
                return RedirectToAction("LockingOfStudentData", "Course");
            }
            return RedirectToAction("Index", "Home");
        }

        private static List<StudentEvaluationViewModel> SortOnSwitch(string sortOrder, List<StudentEvaluationViewModel> studentEvaluations)
        {
            studentEvaluations = sortOrder switch
            {
                "PercentageGrade" => studentEvaluations.OrderByDescending(s => s.PercentageGrade).ToList(),
                "ConversationGrade" => studentEvaluations.OrderByDescending(s => s.ConversationGrade).ToList(),
                "FinalAverageGrade" => studentEvaluations.OrderBy(s => s.FinalAverageGrade).ToList(),
                "Priority" => studentEvaluations.OrderByDescending(s => s.Priority).ToList(),
                _ => studentEvaluations.OrderByDescending(s => s.FinalAverageGrade).ToList(),
            };
            return studentEvaluations;
        }

        private static List<StudentLockDataViewModel> SortOnSwitch(string sortOrder, List<StudentLockDataViewModel> studentEvaluations)
        {
            studentEvaluations = sortOrder switch
            {
                "PercentageGrade" => studentEvaluations.OrderByDescending(s => s.PercentageGrade).ToList(),
                "ConversationGrade" => studentEvaluations.OrderByDescending(s => s.ConversationGrade).ToList(),
                "FinalAverageGrade" => studentEvaluations.OrderBy(s => s.FinalAverageGrade).ToList(),
                "Priority" => studentEvaluations.OrderByDescending(s => s.Priority).ToList(),
                _ => studentEvaluations.OrderByDescending(s => s.FinalAverageGrade).ToList(),
            };
            return studentEvaluations;
        }

        private static List<StudentEvaluationViewModel> AddFilterOnViewModel(string examId, int? courseId, int? cityId, string searchString, List<City> cities, List<Course> courses, ref List<Student> students)
        {
            if (examId != null)
            {
                students = students.Where(x => x.Result.Exam.AcceptancePeriod == examId).ToList();
            }

            var studentEvaluations = new List<StudentEvaluationViewModel>();
            students.ForEach(x => studentEvaluations.Add(CourseMappings.ToViewModel(x, x.ConversationResult, x.StudentCourses, x.Result)));

            if (!string.IsNullOrEmpty(searchString))
            {
                if (studentEvaluations.Where(s => s.LastName.Contains(searchString)).Count() == 0)
                    studentEvaluations = studentEvaluations.Where(s => s.FirstName.Contains(searchString)).ToList();
                else studentEvaluations = studentEvaluations.Where(s => s.LastName.Contains(searchString)).ToList();
            }
            if (cityId != null)
            {
                studentEvaluations = studentEvaluations.Where(e => e.CityId == cities.ElementAt((int)cityId).Id).ToList();
            }
            if (courseId != null)
            {
                studentEvaluations = studentEvaluations.Where(e => e.CourseId == courses.ElementAt((int)courseId).Id).ToList();
            }

            return studentEvaluations;
        }

        private static List<StudentLockDataViewModel> AddFilterOnStudentLockDataViewModel(string examId, int? courseId, int? cityId, string searchString, List<City> cities, List<Course> courses, ref List<Student> students)
        {
            if (examId != null)
            {
                students = students.Where(x => x.Result.Exam.AcceptancePeriod == examId).ToList();
            }

            var studentEvaluations = new List<StudentLockDataViewModel>();
            students.ForEach(x => studentEvaluations.Add(CourseMappings.StudentLockDataToViewModel(x, x.ConversationResult, x.StudentCourses, x.Result)));

            if (!string.IsNullOrEmpty(searchString))
            {
                if (studentEvaluations.Where(s => s.LastName.Contains(searchString)).Count() == 0)
                    studentEvaluations = studentEvaluations.Where(s => s.FirstName.Contains(searchString)).ToList();
                else studentEvaluations = studentEvaluations.Where(s => s.LastName.Contains(searchString)).ToList();
            }
            if (cityId != null)
            {
                studentEvaluations = studentEvaluations.Where(e => e.CityId == cities.ElementAt((int)cityId).Id).ToList();
            }
            if (courseId != null)
            {
                studentEvaluations = studentEvaluations.Where(e => e.CourseId == courses.ElementAt((int)courseId).Id).ToList();
            }

            return studentEvaluations;
        }

        private void AddSorting(string sortOrder)
        {
            ViewBag.PercentageGradeSort = string.IsNullOrEmpty(sortOrder) ? "PercentageGrade" : "";
            ViewBag.ConversationGradeSort = string.IsNullOrEmpty(sortOrder) ? "ConversationGrade" : "";
            ViewBag.FinalAverageGradeSort = string.IsNullOrEmpty(sortOrder) ? "FinalAverageGrade" : "";
            ViewBag.PrioritySort = string.IsNullOrEmpty(sortOrder) ? "Priority" : "";
        }
    }
}
