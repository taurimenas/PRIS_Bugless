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

        public async Task<IActionResult> LockingOfStudentData()
        {
            var students = await _repository.Query<Student>()
                .Include(x => x.ConversationResult)
                .Include(x => x.Result)
                .ThenInclude(x => x.Exam)
                .Include(x => x.StudentCourses)
                .ThenInclude(x => x.Course)
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
