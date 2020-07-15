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

        public async Task<IActionResult> LockingOfStudentData()
        {
            var students = await _repository.Query<Student>()
                .Include(x => x.ConversationResult)
                .Include(x => x.Result)
                .ThenInclude(x => x.Exam)
                .Include(x => x.StudentCourses)
                .ThenInclude(x => x.Course)
                //kuriuos imti studentus is kokio saraso?
                .Where(x => x.InvitedToStudy == true)
                .ToListAsync();
            var studentDataLocking = new List<StudentLockDataViewModel>();

                students.ForEach(x => studentDataLocking.Add(CourseMappings.StudentLockDataToViewModel(x, x.ConversationResult, x.StudentCourses.FirstOrDefault(y => y.Priority == 1), x.Result)));

            return View(studentDataLocking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockingOfStudentData(int[] HasSignedAContract, int[] HasStudentDataLocked)
        {
            //int.TryParse(TempData["ExamId"].ToString(), out int ExamId);
            //var backToExam = RedirectToAction("Index", "Students", new { id = ExamId });
            if (ModelState.IsValid)
            {
                var students = await _repository.Query<Student>()
                    .Include(x => x.Result)
                    .Include(x => x.ConversationResult)
                    .Where(x => x.Id > 0)
                    //pataisyti examid == ???
                    .Where(x => x.Result.Exam.Id == 1)
                    .ToListAsync();
                students.ForEach(x => x.SignedAContract = false);

                for (int i = 0; i < HasSignedAContract.Length; i++)
                {
                    var findStudents = students.FirstOrDefault(x => x.Id == HasSignedAContract[i]);
                    findStudents.SignedAContract = true;
                }

                //studento uzrakinimas jei studentas pasirase sutarti
                var studentsHasSignedAContract = await _repository.Query<Student>()
                    .Include(x => x.Result)
                    .Include(x => x.ConversationResult)
                    .Where(x => x.Id > 0)
                    //pataisyti examid == ???
                    .Where(x => x.Result.Exam.Id == 1)
                    .ToListAsync();
                studentsHasSignedAContract.ForEach(x => x.StudentDataLocked = false);
                for (int i = 0; i < HasStudentDataLocked.Length; i++)
                {
                    foreach (var student in studentsHasSignedAContract)
                    {
                        if (student.SignedAContract == true)
                        {
                            var findStudents = studentsHasSignedAContract.FirstOrDefault(x => x.Id == HasStudentDataLocked[i]);
                            if(findStudents.SignedAContract == true)
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
    }
}
