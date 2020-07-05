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
            TaskParameterListViewModel taskParameterViewModel = new TaskParameterListViewModel();
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
            if (tasks == null)
            {
                return NotFound();
            }
            return View(TaskParametersMappings.ToTaskParameterViewModel(tasks));
        }
        // POST: vvv/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, int[] tasks)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (id != exam.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                TaskParametersMappings.EditTaskParametersEntity(exam, tasks);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Exams");
            }
            return View(TaskParametersMappings.ToTaskParameterViewModel(tasks));
        }
    }
}