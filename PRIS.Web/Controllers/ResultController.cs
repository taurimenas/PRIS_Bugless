using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using PRIS.Core.Library.Entities;
using PRIS.Web.Data;
using PRIS.Web.Mappings;
using PRIS.Web.Models;

namespace PRIS.Web.Controllers
{
    public class ResultController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET:Results

        public async Task<IActionResult> Index()
        {
            
            var result = await _context.Results.ToListAsync();
            var student = await _context.Students.ToListAsync();

            var studentsResults = await (from s in _context.Students
                                         join r in _context.Results on
                                         s.Result.Id equals r.Id
                                         select new ResultViewModel
                                         {
                                             FirstName = s.FirstName,
                                             LastName = s.LastName,
                                             Email = s.Email,
                                             PhoneNumber = s.PhoneNumber,
                                             Task1_1 = r.Task1_1,
                                             Task1_2 = r.Task1_2,
                                             Task1_3 = r.Task1_3,
                                             Task2_1 = r.Task2_1,
                                             Task2_2 = r.Task2_2,
                                             Task2_3 = r.Task2_3,
                                             Task3_1 = r.Task3_1,
                                             Task3_2 = r.Task3_2,
                                             Task3_3 = r.Task3_3,
                                             Task3_4 = r.Task3_4,
                                             Id = s.Result.Id
                                             
                                         }).ToListAsync();

          //  List<ResultViewModel> resultViewModels = new List<ResultViewModel>();
            foreach (var item in student)
            {
                //studentsResults.Add(ResultMappings.ToResultViewModel(item));
                foreach (var resultViewModel in studentsResults)
                {
                    resultViewModel.FinalPoints = resultViewModel.Task1_1 + resultViewModel.Task1_2 + resultViewModel.Task1_3 + resultViewModel.Task2_1 + resultViewModel.Task2_2 + resultViewModel.Task2_3 + resultViewModel.Task3_1 + resultViewModel.Task3_2 + resultViewModel.Task3_3 + resultViewModel.Task3_4;
                }
            }
            //foreach (var item in student)
            //{
            //    studentsResults.Add(ResultMappings.ToResultViewModel(item));
            //}


            return View(studentsResults);

        }
        // GET: Result/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = await _context.Results
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }


            var model = ResultMappings.ToResultViewModel(result);
            return View(model);
        }

        //GET: Result/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Result/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Task1_1,Task1_2,Task1_3,Task2_1,Task2_2,Task2_3,Task3_1,Task3_2,Task3_3,Task3_4,Comment")] ResultViewModel resultViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = ResultMappings.ToResultEntity(resultViewModel);
                _context.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resultViewModel);
        }
        //GET: Result/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = await _context.Results.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(ResultMappings.ToResultViewModel(result));
        }
        //POST:Result/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Task1_1,Task1_2,Task1_3,Task2_1,Task2_2,Task2_3,Task3_1,Task3_2,Task3_3,Task3_4,Comment")] ResultViewModel resultViewModel)
        {
            var result = ResultMappings.ToResultEntity(resultViewModel);

            if (id != result.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultExists(result.Id))
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
            return View(result);
        }
        // GET: Result/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = await _context.Results
                .FirstOrDefaultAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(ResultMappings.ToResultViewModel(result));
        }
        // POST: Result/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _context.Results.FindAsync(id);
         //   var student = await _context.Students.FindAsync(result.Id);
         //   _context.Students.Update(student.Result.Id == null);
            _context.Results.Remove(result);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ResultExists(int id)
        {
            return _context.Results.Any(e => e.Id == id);
        }
    }
}













