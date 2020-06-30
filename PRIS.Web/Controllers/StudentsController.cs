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

namespace PRIS.Web.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _context.Results.ToListAsync();
            var student = await _context.Students.ToListAsync();
            var studentsResults = await (from s in _context.Students
                                         join r in _context.Results on
                                         s.Result.Id equals r.Id into sr
                                         from studRes in sr.DefaultIfEmpty()
                                         select new StudentsResultViewModel
                                         {
                                             FirstName = s.FirstName,
                                             LastName = s.LastName,
                                             Email = s.Email,
                                             PhoneNumber = s.PhoneNumber,
                                             Gender = s.Gender,
                                             Comment = s.Comment,
                                             ResultId = s.Result.Id,
                                             Id = s.Id,
                                             Task1_1 = studRes.Task1_1,
                                             Task1_2 = studRes.Task1_2,
                                             Task1_3 = studRes.Task1_3,
                                             Task2_1 = studRes.Task2_1,
                                             Task2_2 = studRes.Task2_2,
                                             Task2_3 = studRes.Task2_3,
                                             Task3_1 = studRes.Task3_1,
                                             Task3_2 = studRes.Task3_2,
                                             Task3_3 = studRes.Task3_3,
                                             Task3_4 = studRes.Task3_4
                                         }).ToListAsync(); ;

            foreach (var item in student)
            {
                foreach (var resultViewModel in studentsResults)
                {
                    resultViewModel.FinalPoints = resultViewModel.Task1_1 + resultViewModel.Task1_2 + resultViewModel.Task1_3 + resultViewModel.Task2_1 + resultViewModel.Task2_2 + resultViewModel.Task2_3 + resultViewModel.Task3_1 + resultViewModel.Task3_2 + resultViewModel.Task3_3 + resultViewModel.Task3_4;
                }
            }

            return View(studentsResults);

        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, FirstName, LastName, Email, PhoneNumber, Gender, Comment")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                var student = StudentsMappings.ToEntity(studentViewModel);
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
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
            if (id == null)
            {
                return NotFound();
            }
            if (!examPassed)
            {
                var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
                if (student == null)
                {
                    return NotFound();
                }
                var students = await _context.Students.FindAsync(id);
                if (student.ResultId != null)
                {
                    TempData["ErrorMessage"] = "Kandidato negalima ištrinti, nes jis turi priskirtus rezultatus.";
                    return RedirectToAction(nameof(Index));
                }
                _context.Students.Remove(students);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var result = await _context.Students.ToListAsync();
                var studentViewModels = new List<StudentViewModel>();
                result.ForEach(x => studentViewModels.Add(StudentsMappings.ToViewModel(x)));
                ModelState.AddModelError("StudentDelete", "Į pokalbį pakviesto kandidato ištrinti negalima."); // TODO: Pabandyt padaryt be delete puslapio
                return View(studentViewModels);
            }

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(StudentsMappings.ToViewModel(student));
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FirstName, LastName, Email, PhoneNumber, Gender, Comment")] StudentViewModel studentViewModel)
        {
            var student = StudentsMappings.ToEntity(studentViewModel);

            if (id != student.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
        //GET
        public async Task<IActionResult> EditResult(int? id)
        {
            var studentEntity = _context.Students.FindAsync(id).Result;
            if (studentEntity.ResultId == null)
            {
                TempData["ErrorMessage"] = "Negalima redaguoti. Pridėkite kandidatui rezultatus.";
                return RedirectToAction(nameof(Index));
            }


            var resultEntity = await _context.Results.FindAsync(studentEntity.ResultId);

            if (id == null)
            {
                return NotFound();
            }

            if (studentEntity == null)
            {
                return NotFound();
            }

            return View(StudentsMappings.ToViewModel(studentEntity, resultEntity));
        }

        // POST: Exams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResult(int id, [Bind("Task1_1,Task1_2,Task1_3,Task2_1,Task2_2,Task2_3,Task3_1,Task3_2,Task3_3,Task3_4, CommentResult, StudentForeignKey")] StudentsResultViewModel studentResultViewModel)
        {

            var student = _context.Students.FindAsync(id).Result;
            var result = await _context.Results.FindAsync(student.ResultId);
            var studentResult = StudentsMappings.ToResultEntity(studentResultViewModel);

            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    result.Task1_1 = studentResultViewModel.Task1_1;
                    result.Task1_2 = studentResultViewModel.Task1_2;
                    result.Task1_3 = studentResultViewModel.Task1_3;
                    result.Task2_1 = studentResultViewModel.Task2_1;
                    result.Task2_2 = studentResultViewModel.Task2_2;
                    result.Task2_3 = studentResultViewModel.Task2_3;
                    result.Task3_1 = studentResultViewModel.Task3_1;
                    result.Task3_2 = studentResultViewModel.Task3_2;
                    result.Task3_3 = studentResultViewModel.Task3_3;
                    result.Task3_4 = studentResultViewModel.Task3_4;
                    result.Comment = studentResultViewModel.CommentResult;
                    result.StudentForeignKey = id;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
