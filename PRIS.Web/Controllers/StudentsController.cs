using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PRIS.Core.Library.Entities;
using PRIS.Web.Data;
using PRIS.Web.Mappings;
using PRIS.Web.Models;

namespace PRIS.Web.Controllers
{
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
                                         s.Id equals r.StudentForeignKey into sr
                                         from studRes in sr.DefaultIfEmpty()
                                         select new StudentViewModel
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
                                         }).ToListAsync();
            foreach (var item in student)
            {
                foreach (var resultViewModel in studentsResults)
                {
                    resultViewModel.FinalPoints = resultViewModel.Task1_1 + resultViewModel.Task1_2 + resultViewModel.Task1_3 + resultViewModel.Task2_1 + resultViewModel.Task2_2 + resultViewModel.Task2_3 + resultViewModel.Task3_1 + resultViewModel.Task3_2 + resultViewModel.Task3_3 + resultViewModel.Task3_4;

                }
            }

            return View(studentsResults);

            //var studentViewModels = new List<StudentViewModel>();
            //result.ForEach(x => studentViewModels.Add(StudentsMappings.ToViewModel(x)));
            //return View(studentViewModels);
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
            var studentViewModel = StudentsMappings.ToViewModel(studentEntity);
            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddResult(int id, [Bind("Task1_1,Task1_2,Task1_3,Task2_1,Task2_2,Task2_3,Task3_1,Task3_2,Task3_3,Task3_4, CommentResult")] StudentViewModel studentViewModel)
        {
            var studentEntity = _context.Students.FindAsync(id).Result;

            var result = StudentsMappings.ToResultEntity(studentViewModel);

            _context.Add(result);


            await _context.SaveChangesAsync();
            var studentIdforResult = (from p in _context.Results
                                      where p.Id == result.Id
                                      select p).FirstOrDefault();
            studentIdforResult.StudentForeignKey = id;
            var resultIdForStudent = (from s in _context.Students
                                      where s.Id == id
                                      select s).FirstOrDefault();
            resultIdForStudent.ResultId = result.Id;
            await _context.SaveChangesAsync();




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
            var resultEntity = _context.Results.FindAsync(studentEntity.ResultId).Result;
           StudentsMappings.ToResultViewModel(resultEntity, studentEntity);
            
            if (id == null)
            {
                return NotFound();
            }
            
            var student = _context.Students.FindAsync(id).Result;
            var result = await _context.Results.FindAsync(student.ResultId);
            
            if (student == null)
            {
                return NotFound();
            }
            return View(StudentsMappings.ToResultViewModel(resultEntity, studentEntity));
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResult(int id, [Bind("Task1_1,Task1_2,Task1_3,Task2_1,Task2_2,Task2_3,Task3_1,Task3_2,Task3_3,Task3_4, CommentResult")] StudentViewModel studentViewModel)
        {

            var student = _context.Students.FindAsync(id).Result;
            //var result = await _context.Results.FindAsync(student.ResultId);
            var result = StudentsMappings.ToResultEntity(studentViewModel);

            if (id != student.Id)
            {
                return NotFound();
            }
            _context.Update(result);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
