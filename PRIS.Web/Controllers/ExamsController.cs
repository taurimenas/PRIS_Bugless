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

        public async Task<IActionResult> Index(int value, [Bind("SelectedCity")] ExamsViewModel viewModel)
        {
            var result = await _context.Exams.Include(exam => exam.City).ToListAsync();
            List<ExamViewModel> examViewModels = new List<ExamViewModel>();
            result.ForEach(x => examViewModels.Add(ExamMappings.ToViewModel(x)));
            examViewModels.ForEach(x => x.SelectedCity = result.FirstOrDefault(y => y.Id == x.Id).City.Name);

            DateTime firstExamStart = new DateTime(2020, 03, 1);
            DateTime firstExamEnd = new DateTime(2020, 09, 1);
            List<string> AcceptancePeriod = new List<string>();
            foreach (var examViewModel in examViewModels)
            {
                firstExamStart.AddYears(examViewModel.Date.Year - firstExamStart.Year);
                firstExamEnd.AddYears(examViewModel.Date.Year - firstExamEnd.Year);
                if (examViewModel.Date > firstExamStart && examViewModel.Date < firstExamEnd)
                {
                    if (!AcceptancePeriod.Any(x => x == $"{examViewModel.Date.Year} II pusmetis"))
                        AcceptancePeriod.Add($"{examViewModel.Date.Year} II pusmetis");
                    examViewModel.SetAcceptancePeriod = $"{examViewModel.Date.Year} II pusmetis";
                }
                else
                {
                    if (!AcceptancePeriod.Any(x => x == $"{examViewModel.Date.Year} I pusmetis"))
                        AcceptancePeriod.Add($"{examViewModel.Date.Year} I pusmetis");
                    examViewModel.SetAcceptancePeriod = $"{examViewModel.Date.Year} I pusmetis";
                }
            }

            var stringAcceptancePeriod = new List<SelectListItem>();
            foreach (var ap in AcceptancePeriod)
            {
                stringAcceptancePeriod.Add(new SelectListItem { Value = AcceptancePeriod.FindIndex(a => a == ap).ToString(), Text = ap });
            }
            viewModel.AcceptancePeriod = stringAcceptancePeriod;
            viewModel.ExamViewModels = examViewModels;
            viewModel.ExamViewModels = examViewModels.Where(x => x.SetAcceptancePeriod == stringAcceptancePeriod.ElementAt(value).Text).ToList();
            viewModel.SelectedAcceptancePeriod = stringAcceptancePeriod.ElementAt(value).Text;
            return View(viewModel);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityId,Date,Comment,Id,Created,SelectedCity")] ExamViewModel examViewModel)
        {
            if (ModelState.IsValid)
            {
                var latestDate = _context.Exams
                                .OrderBy(x => x.Created)
                                .LastOrDefault();

                var exam = ExamMappings.ToEntity(examViewModel);
                exam.CityId = _context.Cities.FirstOrDefault(x => x.Name == examViewModel.SelectedCity).Id;
                if (latestDate != null)
                {
                    exam.Tasks = latestDate.Tasks;
                    _context.Add(exam);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                _context.Add(exam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

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

            if (result != null)
            {
                var studentById = await _context.Students.FindAsync(result.StudentForeignKey);
                if (studentById != null)
                {
                    TempData["ErrorMessage"] = "Testo negalima ištrinti, nes prie jo jau yra priskirta testą išlaikiusių kandidatų.";
                    ModelState.AddModelError("AssignedStudent", "Testo negalima ištrinti, nes prie jo jau yra priskirta testą išlaikiusių kandidatų.");
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
