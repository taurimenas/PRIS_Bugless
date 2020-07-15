using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Memory;
using PRIS.Core.Library.Entities;
using PRIS.Web.Data;
using PRIS.Web.Mappings;
using PRIS.Web.Models;
using PRIS.Web.Storage;

namespace PRIS.Web.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly Repository<Student> _repository;
        private readonly Repository<Result> _resultRepository;
        private readonly Repository<Exam> _examRepository;
        private readonly Repository<PRIS.Core.Library.Entities.Program> _programRepository;
        private readonly Repository<Course> _courseRepository;
        private readonly Repository<StudentCourse> _studentCourseRepository;


        public StudentsController(Repository<Student> repository, Repository<Result> resultRepository, Repository<Exam> examRepository, Repository<PRIS.Core.Library.Entities.Program> programRepository, Repository<Course> courseRepository, Repository<StudentCourse> studentCourseRepository)
        {
            _repository = repository;
            _resultRepository = resultRepository;
            _examRepository = examRepository;
            _programRepository = programRepository;
            _courseRepository = courseRepository;
            _studentCourseRepository = studentCourseRepository;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TempData["ExamId"] = id;
            var studentRequest = _repository.Query<Student>()
                .Include(x => x.Result)
                .Include(x => x.ConversationResult)
                .Where(x => x.Id > 0);
            var students = await studentRequest.Where(x => x.Result.Exam.Id == id).ToListAsync();
            if (students == null)
            {
                return NotFound();
            }
            var studentViewModels = new List<StudentsResultViewModel>();
            students.ForEach(x => studentViewModels.Add(StudentsMappings.ToViewModel(x, x.Result)));
            foreach (var item in students)
            {
                foreach (var student in studentViewModels)
                {
                    student.FinalPoints = student.Tasks?.Sum(x => x) ?? 0;
                    var examDraft = _examRepository.Query<Exam>().Where(e => e.Id == student.ExamId).FirstOrDefault();
                    double maxPoints = TaskParametersMappings.ToTaskParameterViewModel(examDraft).Tasks.Sum(x => x);
                    student.PercentageGrade = student.FinalPoints * 100 / maxPoints;
                }
            }

            studentViewModels = studentViewModels.OrderByDescending(x => x.FinalPoints).ToList();

            return View(studentViewModels);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int[] HasPassedExam)
        {
            int.TryParse(TempData["ExamId"].ToString(), out int ExamId);
            var backToExam = RedirectToAction("Index", "Students", new { id = ExamId });
            if (ModelState.IsValid)
            {
                var students = await _repository.Query<Student>()
                    .Include(x => x.Result)
                    .Include(x => x.ConversationResult)
                    .Where(x => x.Id > 0)
                    .Where(x => x.Result.Exam.Id == ExamId)
                    .ToListAsync();
                students.ForEach(x => x.PassedExam = false);

                for (int i = 0; i < HasPassedExam.Length; i++)
                {
                    var findStudents = students.FirstOrDefault(x => x.Id == HasPassedExam[i]);
                    findStudents.PassedExam = true;
                }

                await _repository.SaveAsync();
                return RedirectToAction("Index", "ConversationResults", new { id = ExamId });
            }
            return backToExam;
        }
        //GET
        public async Task<IActionResult> Create()
        {
            StudentViewModel studentViewModel = new StudentViewModel();
            List<PRIS.Core.Library.Entities.Program> programs = await _programRepository.Query<PRIS.Core.Library.Entities.Program>().ToListAsync();
            var stringPrograms = new List<SelectListItem>();
            foreach (var program in programs)
            {
                stringPrograms.Add(new SelectListItem { Value = program.Name, Text = program.Name });
            }
            studentViewModel.Programs = stringPrograms;
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string[] selectedPriority, [Bind("Id, FirstName, LastName, Email, PhoneNumber, Gender, Comment")] StudentViewModel studentViewModel)
        {
            int.TryParse(TempData["ExamId"].ToString(), out int ExamId);
            var backToExam = RedirectToAction("Index", "Students", new { id = ExamId });
            if (ModelState.IsValid)
            {
                var student = StudentsMappings.ToEntity(studentViewModel);
                Result result = new Result
                {
                    ExamId = ExamId,
                };

                result = await _resultRepository.InsertAsync(result);
                result.Student = student;
                student.Result = result;
                student.ResultId = result.Id;
                await _repository.InsertAsync(student);
                var studentsExam = _examRepository.FindByIdAsync(ExamId).Result;

                await _repository.SaveAsync();
                studentViewModel = StudentsMappings.ToViewModel(student);
                for (int i = 1; i < selectedPriority.Length + 1; i++)
                {
                    if (selectedPriority[i - 1] == null)
                    {
                        return RedirectToAction("Create", "Students"); ;
                    }
                    if (selectedPriority[i] == selectedPriority[i - 1] || selectedPriority[i] == selectedPriority[i + 1] || selectedPriority[i-1] == selectedPriority[i + 1])
                    {
                        TempData["ErrorMessage"] = "Studento prioritetai turi būti skirtingi";
                        return RedirectToAction("Edit", "Students", new { id = student.Id });
                    }
                    StudentCourse studentCourse = new StudentCourse
                    {
                        Priority = i,
                        StudentId = student.Id,
                        Student = student
                    };
                    var priorityProgram = _programRepository.Query<PRIS.Core.Library.Entities.Program>()
                        .Where(x => x.Name == selectedPriority[i - 1])
                        .FirstOrDefault();
                    var priorityCourse = _courseRepository.Query<Course>()
                        .Where(x => x.ProgramId == priorityProgram.Id)
                        .Where(x => x.StartYear.Year == studentsExam.Date.Year)
                        .FirstOrDefault();

                    if (priorityCourse != null)
                    {
                        studentCourse.CourseId = priorityCourse.Id;
                    }
                    else
                    {
                        Course course = new Course
                        {
                            StartYear = studentsExam.Date,
                            EndYear = studentsExam.Date.AddYears(1),
                            CityId = studentsExam.CityId,
                            ProgramId = priorityProgram.Id,
                            Title = priorityProgram.Name
                        };
                        studentCourse.CourseId = course.Id;
                        studentCourse.Course = course;
                        await _courseRepository.InsertAsync(course);
                    }

                    await _studentCourseRepository.InsertAsync(studentCourse);
                }

                return backToExam;
            }
            return backToExam;
        }

        public async Task<IActionResult> Delete(int? id, bool examPassed)
        {
            int.TryParse(TempData["ExamId"].ToString(), out int ExamId);
            if (id == null)
            {
                return NotFound();
            }
            if (!examPassed)
            {
                if (_repository.Exists(id))
                {
                    var student = await _repository.Query<Student>()
                                                     .Include(x => x.Result)
                                                     .SingleOrDefaultAsync(x => x.Id == id);
                    await _repository.DeleteAsync(id);
                    if (student.ResultId != null)
                        await _resultRepository.DeleteAsync(student.ResultId);
                }
                else
                {
                    ModelState.AddModelError("StudentDelete", "Toks studentas neegzistuoja.");
                    TempData["ErrorMessage"] = "Toks studentas neegzistuoja.";
                    return RedirectToAction("Index", "Students", new { id = ExamId });
                }
                return RedirectToAction("Index", "Students", new { id = ExamId });
            }
            else
            {
                ModelState.AddModelError("StudentDelete", "Į pokalbį pakviesto kandidato ištrinti negalima.");
                TempData["ErrorMessage"] = "Į pokalbį pakviesto kandidato ištrinti negalima.";
                return RedirectToAction("Index", "Students", new { id = ExamId });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = await _repository.FindByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            var studentViewModel = StudentsMappings.ToViewModel(student);
            var programs = await _programRepository.Query<PRIS.Core.Library.Entities.Program>().ToListAsync();
            var stringPrograms = new List<SelectListItem>();
            studentViewModel.SelectedPriority = new string[programs.Count()];
            for (int i = 1; i < programs.Count() + 1; i++)
            {
                var studentPriority = _studentCourseRepository.Query<StudentCourse>()
                    .Where(x => x.StudentId == student.Id)
                    .Where(x => x.Priority == i)
                    .FirstOrDefault();
                if (studentPriority == null)
                {
                    stringPrograms.Add(new SelectListItem { Value = programs[i - 1].Name, Text = programs[i - 1].Name });
                    studentViewModel.SelectedPriority[i - 1] = programs[i - 1].Name;
                }
                else
                {
                    var priorityCourse = _courseRepository.Query<Course>()
                   .Where(x => x.Id == studentPriority.CourseId)
                   .FirstOrDefault();
                    var priorityName = programs.Where(x => x.Id == priorityCourse.ProgramId).FirstOrDefault();
                    studentViewModel.SelectedPriority[i - 1] = priorityName.Name;
                    stringPrograms.Add(new SelectListItem { Value = priorityName.Name, Text = priorityName.Name });
                }

            }
            studentViewModel.Programs = stringPrograms;

            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedPriority, [Bind("Id, FirstName, LastName, Email, PhoneNumber, Gender, Comment")] StudentViewModel studentViewModel)
        {
            int.TryParse(TempData["ExamId"].ToString(), out int ExamId);

            var student = await _repository.Query<Student>()
                .Include(x => x.StudentCourses)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Programs)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (id != student.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var course = await _courseRepository.Query<Course>().ToListAsync();

                    StudentsMappings.ToEntity(student, studentViewModel);
                    var studentsExam = _examRepository.FindByIdAsync(ExamId).Result;

                    for (int i = 1; i < selectedPriority.Length + 1; i++)
                    {
                        if (selectedPriority[i] == selectedPriority[i - 1] || selectedPriority[i] == selectedPriority[i + 1] || selectedPriority[i - 1] == selectedPriority[i + 1])
                        {
                            TempData["ErrorMessage"] = "Studento prioritetai turi būti skirtingi";
                            return RedirectToAction("Edit", "Students", new { id = student.Id });
                        }

                        student.StudentCourses.FirstOrDefault(x => x.Priority == i).Course = course.FirstOrDefault(x => x.Title == selectedPriority[i - 1]);

                        student.StudentCourses.FirstOrDefault(x => x.Priority == i).Course.Id = course.FirstOrDefault(x => x.Title == selectedPriority[i - 1]).Id;

                    }
                    await _studentCourseRepository.SaveAsync();

                    await _repository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.Exists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Students", new { id = ExamId });
            }
            return View(student);
        }
        //GET
        public async Task<IActionResult> EditResult(int? id, int? resultId)
        {
            TempData["ResultId"] = resultId;
            int.TryParse(TempData["ExamId"].ToString(), out int ExamId);

            if (id == null)
            {
                return NotFound();
            }
            var studentRequest = _repository.Query<Student>().Include(x => x.Result).Where(x => x.Id == id);
            var studentEntity = await studentRequest.FirstOrDefaultAsync();
            var resultEntity = await _resultRepository.FindByIdAsync(studentEntity.Result.Id);

            if (studentEntity == null)
            {
                return NotFound();
            }
            var exam = await _examRepository.FindByIdAsync(ExamId);
            var studentViewModel = StudentsMappings.ToViewModel(studentEntity, resultEntity);
            if (resultEntity.Tasks == null)
                studentViewModel.Tasks = new double[JsonSerializer.Deserialize<double[]>(exam.Tasks).Length];
            TempData["ExamId"] = ExamId;
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResult(double[] Tasks)
        {
            int.TryParse(TempData["ResultId"].ToString(), out int resultId);
            int.TryParse(TempData["ExamId"].ToString(), out int ExamId);
            if (ModelState.IsValid)
            {
                try
                {
                    var studentRequest = _repository.Query<Student>().Include(x => x.Result).ThenInclude(y => y.Exam).Where(x => x.Id > 0);
                    var result = await _resultRepository.FindByIdAsync(resultId);
                    var student = await studentRequest.FirstOrDefaultAsync(x => x.Id == result.StudentForeignKey);
                    var studentResultViewModel = StudentsMappings.ToStudentsResultViewModel(Tasks);
                    var examTasks = StudentsMappings.ToStudentsResultViewModel(result).Tasks;
                    if (student.PassedExam)
                    {
                        TempData["ErrorMessage"] = "Studentas yra pakviestas į pokalbį, todėl jo duomenų negalima redaguoti.";
                        return RedirectToAction("EditResult", "Students", new { resultId });
                    }

                    var testToDelete = examTasks.Select((x, i) => x < Tasks[i]);

                    var isInvalid = examTasks.Select((x, i) => x < Tasks[i]).Any(x => x);
                    if (isInvalid)
                    {
                        ModelState.AddModelError("EditResult", "Užduoties balas negali būti didesnis nei testo šablono balas");
                        TempData["ErrorMessage"] = "Užduoties balas negali būti didesnis nei testo šablono balas";
                        return RedirectToAction("EditResult", "Students", new { resultId });
                    }

                    StudentsMappings.ToResultEntity(result, studentResultViewModel);
                    await _resultRepository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index", "Students", new { id = ExamId });
            }
            return RedirectToAction("Index", "Students", new { id = ExamId });
        }
    }
}
