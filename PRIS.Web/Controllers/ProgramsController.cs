using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRIS.Web.Data;
using PRIS.Web.Mappings;
using PRIS.Web.Models;
using PRIS.Core.Library.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace PRIS.Web.Controllers
{
    [Authorize]
    public class ProgramsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ProgramViewModel programViewModel = new ProgramViewModel();
            var programResult = await _context.Programs.ToListAsync();
            programViewModel.ProgramNames = programResult;

            programViewModel.CityNames = await _context.Cities.ToListAsync();

            return View(programViewModel);
        }

        public IActionResult Create()
        {
            var model = new ProgramCreateModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProgramName")] ProgramCreateModel programCreateModel)
        {
            if (ModelState.IsValid)
            {
                var result = ProgramMappings.ToProgramEntity(programCreateModel);
                _context.Programs.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programCreateModel);
        }
        public IActionResult CreateNewCity()
        {
            var model = new CityCreateModel();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewCity([Bind("CityName")] CityCreateModel cityCreateModel)
        {
            if (ModelState.IsValid)
            {
                var result = ProgramMappings.ToCityEntity(cityCreateModel);
                _context.Cities.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cityCreateModel);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var program = await _context.Programs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (program == null)
            {
                return NotFound();
            }
            var programById = await _context.Programs.FindAsync(id);
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.ProgramId == programById.Id);
            if (course != null)
            {
                return await BadRequest(course);
            }
            else
            {
                _context.Programs.Remove(programById);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            var cityById = await _context.Cities.FindAsync(id);
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.CityId == cityById.Id);
            if (course != null)
            {
                return await BadRequest(course);
            }
            else
            {
                _context.Cities.Remove(cityById);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
        private bool ProgramExists(int id)
        {
            return _context.Programs.Any(e => e.Id == id);
        }
        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }

        private async Task<IActionResult> BadRequest(Course course)
        {
            var studentsCourses = await _context.StudentsCourses.ToListAsync();
            var studentCourse = studentsCourses.Where(x => x.CourseId == course.Id);
            if (studentCourse.Any())
            {
                ModelState.AddModelError("AssignedStudent", "Programos negalima ištrinti, nes prie jos jau yra priskirta kandidatų.");
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
