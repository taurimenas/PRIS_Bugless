using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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

        public StudentsController(Repository<Student> repository, Repository<Result> resultRepository)
        {
            _repository = repository;
            _resultRepository = resultRepository;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TempData["ExamId"] = id;
            var studentRequest = _repository.Query<Student>().Include(x => x.Result).Where(x => x.Id > 0);
            var students = await studentRequest.Where(x => x.Result.Exam.Id == id).ToListAsync();
            if (students == null)
            {
                return NotFound();
            }
            var studentViewModels = new List<StudentsResultViewModel>();
            students.ForEach(x => studentViewModels.Add(StudentsMappings.ToViewModel(x, x.Result)));
            foreach (var item in students)
            {
                studentViewModels.ForEach(y => y.FinalPoints = (y.Task1_1 + y.Task1_2 + y.Task1_3 + y.Task2_1 + y.Task2_2 + y.Task2_3 + y.Task3_1 + y.Task3_2 + y.Task3_3 + y.Task3_4));
            }

            studentViewModels = studentViewModels.OrderByDescending(x => x.FinalPoints).ToList();

            return View(studentViewModels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, FirstName, LastName, Email, PhoneNumber, Gender, Comment")] StudentViewModel studentViewModel)
        {
            int.TryParse(TempData["ExamId"].ToString(), out int ExamId);
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

                return RedirectToAction("Index", "Students", new { id = ExamId });
            }
            return RedirectToAction("Index", "Students", new { id = ExamId });
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
                    await _repository.DeleteAsync(id);
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
            return View(StudentsMappings.ToViewModel(student));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FirstName, LastName, Email, PhoneNumber, Gender, Comment")] StudentViewModel studentViewModel)
        {
            int.TryParse(TempData["ExamId"].ToString(), out int ExamId);
            var student = StudentsMappings.ToEntity(studentViewModel);

            if (id != student.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.UpdateAsync(student);
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

        public async Task<IActionResult> EditResult(int? id, int? resultId)
        {
            TempData["ResultId"] = resultId;

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

            return View(StudentsMappings.ToViewModel(studentEntity, resultEntity));
        }

        // POST: Exams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResult([Bind("Task1_1,Task1_2,Task1_3,Task2_1,Task2_2,Task2_3,Task3_1,Task3_2,Task3_3,Task3_4, CommentResult, StudentForeignKey")] StudentsResultViewModel studentResultViewModel)
        {
            int.TryParse(TempData["ResultId"].ToString(), out int resultId);
            int.TryParse(TempData["ExamId"].ToString(), out int ExamId);

            if (ModelState.IsValid)
            {
               

                try
                {
                    var studentRequest = _repository.Query<Student>().Include(x => x.Result).ThenInclude(y=>y.Exam).Where(x => x.Id > 0);
                    var result = await _resultRepository.FindByIdAsync(resultId);
                    var student = await studentRequest.FirstOrDefaultAsync(x => x.Id == result.StudentForeignKey);

                    //tasks in results table
                    var resultTask1_1 = studentResultViewModel.Task1_1;
                    var resultTask1_2 = studentResultViewModel.Task1_2;
                    var resultTask1_3 = studentResultViewModel.Task1_3;
                    var resultTask2_1 = studentResultViewModel.Task2_1;
                    var resultTask2_2 = studentResultViewModel.Task2_2;
                    var resultTask2_3 = studentResultViewModel.Task2_3;
                    var resultTask3_1 = studentResultViewModel.Task3_1;
                    var resultTask3_2 = studentResultViewModel.Task3_2;
                    var resultTask3_3 = studentResultViewModel.Task3_3;
                    var resultTask3_4 = studentResultViewModel.Task3_4;
                    ////tasks in exams table
                    var examTask1_1 = result.Exam.Task1_1;
                    var examTask1_2 = result.Exam.Task1_2;
                    var examTask1_3 = result.Exam.Task1_3;
                    var examTask2_1 = result.Exam.Task2_1;
                    var examTask2_2 = result.Exam.Task2_2;
                    var examTask2_3 = result.Exam.Task2_3;
                    var examTask3_1 = result.Exam.Task3_1;
                    var examTask3_2 = result.Exam.Task3_2;
                    var examTask3_3 = result.Exam.Task3_3;
                    var examTask3_4 = result.Exam.Task3_4;

                    if (examTask1_1 < resultTask1_1 || examTask1_2 < resultTask1_2 ||
                        examTask1_3 < resultTask1_3 || examTask2_1 < resultTask2_1 ||
                        examTask2_2 < resultTask2_2 || examTask2_3 < resultTask2_3 ||
                        examTask3_1 < resultTask3_1 || examTask3_2 < resultTask3_2 ||
                        examTask3_3 < resultTask3_3 || examTask3_4 < resultTask3_4
                        )
                    {
                        ModelState.AddModelError("EditResult", "Užduoties balas negali būti didesnis nei testo šablono balas");
                        TempData["ErrorMessage"] = "Užduoties balas negali būti didesnis nei testo šablono balas";
                        TempData["ResultId"] = resultId;
                        return RedirectToAction("EditResult", "Students", new { id = resultId });
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
