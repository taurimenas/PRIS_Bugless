using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRIS.Core.Library.Entities;
using PRIS.Web.Mappings;
using PRIS.Web.Models;
using PRIS.Web.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly IRepository _repository;

        public ExamsController(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(int value, [Bind("SelectedCity")] ExamsViewModel viewModel)
        {
            var exams = await _repository.Query<Exam>()
                .Include(exam => exam.City)
                .ToListAsync();
            List<ExamViewModel> examViewModels = new List<ExamViewModel>();
            exams.ForEach(x => examViewModels.Add(ExamMappings.ToViewModel(x)));
            examViewModels = examViewModels.OrderByDescending(x => x.Date).ToList();

            List<string> AcceptancePeriod = await CalculateAcceptancePeriods(examViewModels);

            var AcceptancePeriods = new List<SelectListItem>();
            foreach (var ap in AcceptancePeriod)
            {
                AcceptancePeriods.Add(new SelectListItem { Value = AcceptancePeriod.FindIndex(a => a == ap).ToString(), Text = ap });
            }
            viewModel.AcceptancePeriod = AcceptancePeriods;
            viewModel.ExamViewModels = examViewModels.Where(x => x.SetAcceptancePeriod == AcceptancePeriods.ElementAt(value).Text).ToList();
            var SelectedAcceptancePeriod = AcceptancePeriods.ElementAt(value);
            viewModel.SelectedAcceptancePeriod = SelectedAcceptancePeriod.Text;
            TempData["SelectedAcceptancePeriod"] = SelectedAcceptancePeriod.Value;

            var selectedExams = examViewModels.Where(x => x.SetAcceptancePeriod == SelectedAcceptancePeriod.Text).ToList();

            var results = await _repository.Query<Result>().ToListAsync();
            int studentsCountInAcceptancePeriod = 0;
            foreach (var selectedExam in selectedExams)
            {
                int examId = exams.FirstOrDefault(x => x.Date == selectedExam.Date).Id;
                studentsCountInAcceptancePeriod += results.Count(x => x.ExamId == examId);
            }
            TempData["Count"] = studentsCountInAcceptancePeriod;
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var returnPath = HttpContext.Request.Headers["Referer"].ToString();
            TempData["ReturnPath"] = returnPath;
            ExamViewModel examViewModel = new ExamViewModel();
            List<City> cities = await _repository.Query<City>().ToListAsync();

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
                var latestDate = _repository.Query<Exam>()
                                .OrderBy(x => x.Created)
                                .LastOrDefault();

                var exam = ExamMappings.ToEntity(examViewModel);
                var city = await _repository.Query<City>().FirstOrDefaultAsync(x => x.Name == examViewModel.SelectedCity);
                exam.CityId = city.Id;
                var previousUrl = TempData["ReturnPath"].ToString();
                if (latestDate != null)
                {
                    exam.Tasks = latestDate.Tasks;
                    await _repository.InsertAsync(exam);
                    return Redirect(previousUrl);
                }
                await _repository.InsertAsync(exam);
                return Redirect(previousUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _repository.Query<Exam>()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exam == null)
            {
                return NotFound();
            }
            var examById = await _repository.FindByIdAsync<Exam>(id);
            var result = await _repository.Query<Result>().FirstOrDefaultAsync(x => x.ExamId == examById.Id);
            int.TryParse(TempData["SelectedAcceptancePeriod"].ToString(), out int SelectedAcceptancePeriod);

            if (result != null)
            {
                var studentById = await _repository.FindByIdAsync<Student>(result.StudentForeignKey);
                if (studentById != null)
                {
                    TempData["ErrorMessage"] = "Testo negalima ištrinti, nes prie jo jau yra priskirta testą išlaikiusių kandidatų.";
                    ModelState.AddModelError("AssignedStudent", "Testo negalima ištrinti, nes prie jo jau yra priskirta testą išlaikiusių kandidatų.");
                }
                else
                {
                    await RemoveFromExams(examById);
                    return Redirect($"/Exams/Index?value={SelectedAcceptancePeriod}");
                }
                return Redirect($"/Exams/Index?value={SelectedAcceptancePeriod}");
            }
            else
            {
                var oldestExam = await _repository.Query<Exam>().OrderBy(m => m.Date).FirstOrDefaultAsync();
                await RemoveFromExams(examById);
                if (examById.Date == oldestExam.Date)
                    return Redirect($"/Exams/Index?value={SelectedAcceptancePeriod - 1}");
                return Redirect($"/Exams/Index?value={SelectedAcceptancePeriod}");
            }
        }

        private async Task<IActionResult> RemoveFromExams(Exam examById)
        {
            await _repository.DeleteAsync<Exam>(examById.Id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<string>> CalculateAcceptancePeriods(List<ExamViewModel> examViewModels)
        {
            DateTime firstExamStart = new DateTime(2020, 03, 1);
            DateTime firstExamEnd = new DateTime(2020, 09, 1);
            List<string> AcceptancePeriod = new List<string>();
            foreach (var examViewModel in examViewModels)
            {
                firstExamStart.AddYears(examViewModel.Date.Year - firstExamStart.Year);
                firstExamEnd.AddYears(examViewModel.Date.Year - firstExamEnd.Year);
                var examEntity = await _repository.FindByIdAsync<Exam>(examViewModel.Id);
                if (examViewModel.Date > firstExamStart && examViewModel.Date < firstExamEnd)
                {
                    if (!AcceptancePeriod.Any(x => x == $"{examViewModel.Date.Year} II pusmetis"))
                        AcceptancePeriod.Add($"{examViewModel.Date.Year} II pusmetis");
                    examViewModel.SetAcceptancePeriod = $"{examViewModel.Date.Year} II pusmetis";
                    examEntity.AcceptancePeriod = $"{examViewModel.Date.Year} II pusmetis";
                }
                else
                {
                    if (!AcceptancePeriod.Any(x => x == $"{examViewModel.Date.Year} I pusmetis"))
                        AcceptancePeriod.Add($"{examViewModel.Date.Year} I pusmetis");
                    examViewModel.SetAcceptancePeriod = $"{examViewModel.Date.Year} I pusmetis";
                    examEntity.AcceptancePeriod = $"{examViewModel.Date.Year} I pusmetis";
                }
                await _repository.SaveAsync();
            }

            return AcceptancePeriod;
        }
    }
}
