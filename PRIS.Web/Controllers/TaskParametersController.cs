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
            var exam = await _context.Exams.FindAsync(id);
            var fullModel = TaskParametersMappings.ToTaskParameterViewModel(exam);
            fullModel.CityId = fullModel.CityId;
            fullModel.Date = fullModel.Date;
            fullModel.Id = fullModel.Id;
            var tasks = TaskParametersMappings.ToTaskParametersEntity(setTaskParameterModel);
            fullModel.Task1_1 = tasks.Task1_1;
            fullModel.Task1_2 = tasks.Task1_2;
            fullModel.Task1_3 = tasks.Task1_3;
            fullModel.Task2_1 = tasks.Task2_1;
            fullModel.Task2_2 = tasks.Task2_2;
            fullModel.Task2_3 = tasks.Task2_3;
            fullModel.Task3_1 = tasks.Task3_1;
            fullModel.Task3_2 = tasks.Task3_2;
            fullModel.Task3_3 = tasks.Task3_3;
            fullModel.Task3_4 = tasks.Task3_4;

            if (id != exam.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Exams.Single(u => u.Id == id).Task1_1 = setTaskParameterModel.Task1_1;
                _context.Exams.Single(u => u.Id == id).Task1_2 = setTaskParameterModel.Task1_2;
                _context.Exams.Single(u => u.Id == id).Task1_3 = setTaskParameterModel.Task1_3;
                _context.Exams.Single(u => u.Id == id).Task2_1 = setTaskParameterModel.Task2_1;
                _context.Exams.Single(u => u.Id == id).Task2_2 = setTaskParameterModel.Task2_2;
                _context.Exams.Single(u => u.Id == id).Task2_3 = setTaskParameterModel.Task2_3;
                _context.Exams.Single(u => u.Id == id).Task3_1 = setTaskParameterModel.Task3_1;
                _context.Exams.Single(u => u.Id == id).Task3_2 = setTaskParameterModel.Task3_2;
                _context.Exams.Single(u => u.Id == id).Task3_3 = setTaskParameterModel.Task3_3;
                _context.Exams.Single(u => u.Id == id).Task3_4 = setTaskParameterModel.Task3_4;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Exams");
            }
            return View(fullModel);
        }
    }
}