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
            var studentViewModels = new List<StudentViewModel>();
            students.ForEach(x => studentViewModels.Add(StudentsMappings.ToViewModel(x)));
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
                    ExamId = ExamId
                };
                result = await _resultRepository.InsertAsync(result);
                student.Result = result;
                await _repository.InsertAsync(student);
                return RedirectToAction("Index", "Students", new { id = ExamId });
            }
            return RedirectToAction("Index", "Students", new { id = ExamId });
        }

        public IActionResult AddResult(int? id)
        {
            var studentEntity = _context.Students.FindAsync(id).Result;
            if (studentEntity.ResultId != null)
            {
                TempData["ErrorMessage"] = "Rezultatai kandidatui jau yra pridėti";
                return RedirectToAction(nameof(Index));
            }
            var studentViewModel = StudentsMappings.ToStudentsResultViewModel(studentEntity);
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddResult(int id, [Bind("Id, FirstName, LastName, Email, PhoneNumber, Gender, Comment, PassedExam, Task1_1,Task1_2,Task1_3,Task2_1,Task2_2,Task2_3,Task3_1,Task3_2,Task3_3,Task3_4, ResultComment, ResultExamId")] StudentsResultViewModel studentResultViewModel)
        {
            if (ModelState.IsValid)
            {
                var studentEntity = _context.Students.FindAsync(id).Result;

                var result = StudentsMappings.ToResultEntity(studentResultViewModel);
                _context.Add(result);
                await _context.SaveChangesAsync();

                var studentIdforResult = _context.Results.Where(s => s.Id == result.Id).FirstOrDefault();
                studentIdforResult.StudentForeignKey = id;
                var resultIdForStudent = _context.Students.Where(r => r.Id == id).FirstOrDefault();
                resultIdForStudent.ResultId = result.Id;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
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
    }
}
