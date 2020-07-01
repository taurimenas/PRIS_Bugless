using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRIS.Core.Library.Entities;
using PRIS.Web.Data;
using PRIS.Web.Mappings;
using PRIS.Web.Models;

namespace PRIS.Web.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exams
        public async Task<IActionResult> Index()
        {
            var result = await _context.Exams.Include(exam => exam.City).ToListAsync();
            List<ExamViewModel> examViewModels = new List<ExamViewModel>();
            result.ForEach(x => examViewModels.Add(ExamMappings.ToViewModel(x)));
            examViewModels.ForEach(x => x.SelectedCity = result.FirstOrDefault(y => y.Id == x.Id).City.Name);
            return View(examViewModels);
        }

        // GET: Exams/Create
        public async Task<IActionResult> Create()
        {
            ExamViewModel examViewModel = new ExamViewModel();
            List<City> cities = await _context.Cities.ToListAsync();

            var stringCities = new List<SelectListItem>();
            foreach (var city in cities)
            {
                stringCities.Add(new SelectListItem { Value = city.Name, Text = city.Name });
            }
            examViewModel.Cities = stringCities;

            return View(examViewModel);
        }

        // POST: Exams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityId,Date,Comment,Id,Created,SelectedCity")] ExamViewModel examViewModel)
        {
            if (ModelState.IsValid)
            {
                var exam = ExamMappings.ToEntity(examViewModel);
                exam.CityId = _context.Cities.FirstOrDefault(x => x.Name == examViewModel.SelectedCity).Id;
                _context.Add(exam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Exams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exam == null)
            {
                return NotFound();
            }
            var examById = await _context.Exams.FindAsync(id);
            var result = await _context.Results.FirstOrDefaultAsync(x => x.ExamId == examById.Id);

            if (result.ExamId != null)
            {
                TempData["ErrorMessage"] = "Testo negalima ištrinti. Jis turi priskirtų rezultatų.";
                return RedirectToAction(nameof(Index));
            }
            if (result != null)
            {
                var studentById = await _context.Students.FindAsync(result.StudentForeignKey);
                if (studentById != null)
                {
                    TempData["ErrorMessage"] = "Testo negalima ištrinti, nes prie jos jau yra priskirta testą išlaikiusių kandidatų.";
                    ModelState.AddModelError("AssignedStudent", "Testo negalima ištrinti, nes prie jos jau yra priskirta testą išlaikiusių kandidatų.");
                }
                else
                {
                    return await RemoveFromExams(examById);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return await RemoveFromExams(examById);
            }
        }

        private async Task<IActionResult> RemoveFromExams(Exam examById)
        {
            _context.Exams.Remove(examById);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
