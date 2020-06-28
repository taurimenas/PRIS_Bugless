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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SetTaskParameters()
        {
            var model = new SetTaskParameterModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetTaskParameters([Bind("Task1.1, Task1.2, Task1.3, Task2.1, Task2.2, Task2.3, Task3.1, Task3.2, Task3.3, Task3.4")] SetTaskParameterModel setTaskParameterModel)
        {
            if (ModelState.IsValid)
            {
                var result = TaskParametersMappings.ToTaskParametersEntity(setTaskParameterModel);
                _context.Exams.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(setTaskParameterModel);
        }
    }
}