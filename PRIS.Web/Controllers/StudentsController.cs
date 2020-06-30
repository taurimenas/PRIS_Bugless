using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIS.Core.Library.Entities;
using PRIS.Web.Data;
using PRIS.Web.Mappings;
using PRIS.Web.Models;
using PRIS.Web.Storage;

namespace PRIS.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IRepository<Student> _repository;

        public StudentsController(IRepository<Student> repository)
        {
            _repository = repository;
        }

        //public StudentsController(StudentRepository repository) : base(repository)
        //{

        //}


        public async Task<IActionResult> Index()
        {
            var result = await _repository.GetAllAsync();
            //var result = await _context.Students.ToListAsync();
            var studentViewModels = new List<StudentViewModel>();
            result.ForEach(x => studentViewModels.Add(StudentsMappings.ToViewModel(x)));
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
            if (ModelState.IsValid)
            {
                var student = StudentsMappings.ToEntity(studentViewModel);
                await _repository.InsertAsync(student);
                //_context.Add(student);
                //await _context.SaveChangesAsync();
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
                //var student = await _repository.FindByIdAsync(id);
                //var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
                //if (student == null)
                //{
                //    return NotFound();
                //}
                await _repository.DeleteAsync(id);
                //var students = await _context.Students.FindAsync(id);
                //_context.Students.Remove(students);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("StudentDelete", "Į pokalbį pakviesto kandidato ištrinti negalima.");
                TempData["ErrorMessage"] = "Į pokalbį pakviesto kandidato ištrinti negalima.";
                return RedirectToAction(nameof(Index));
            }

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = await _repository.FindByIdAsync(id);
            //var student = await _context.Students.FindAsync(id);
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
                    //_context.Update(student);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.CheckIfExists(student.Id))
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
    }
}
