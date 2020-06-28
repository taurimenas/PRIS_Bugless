using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIS.Web.Data;
using PRIS.Web.Mappings;
using PRIS.Web.Models;

namespace PRIS.Web.Controllers
{
    public class TaskParametersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskParametersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            TaskParameterViewModel taskParameterViewModel = new TaskParameterViewModel();
            var taskParameterResult = await _context.Exams.ToListAsync();
            taskParameterViewModel.Tasks = taskParameterResult;
            return View(taskParameterViewModel);
        }
        // GET: vvv/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tasks = await _context.Exams.FindAsync(id);
            if(tasks == null)
            {
                return NotFound();
            }
            return View(TaskParametersMappings.ToTaskParameterViewModel(tasks));
        }
        // POST: vvv/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Task1_1,Task1_2,Task1_3,Task2_1,Task2_2,Task2_3,Task3_1,Task3_2,Task3_3,Task3_4")]SetTaskParameterModel setTaskParameterModel)
        {
            var tasks = TaskParametersMappings.ToTaskParametersEntity(setTaskParameterModel);

            if(id != tasks.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasksExists(tasks.Id))
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
            return View(tasks);
        }
        private bool TasksExists(int id)
        {
            return _context.Results.Any(e => e.Id == id);
        }
    }
}