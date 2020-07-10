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
using PRIS.Web.Storage;

namespace PRIS.Web.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly Repository<Exam> _examRepository;
        private readonly Repository<City> _cityRepository;
        private readonly Repository<Student> _studentRepository;
        private readonly Repository<Result> _resultRepository;


        public ExamsController(Repository<Exam> examRepository, Repository<City> cityRepository, Repository<Student> studentRepository, Repository<Result> resultRepository)
        {
            _examRepository = examRepository;
            _cityRepository = cityRepository;
            _studentRepository = studentRepository;
            _resultRepository = resultRepository;

        }

        public async Task<IActionResult> Index(int value, [Bind("SelectedCity")] ExamsViewModel viewModel)
        {
            var exams = await _examRepository.Query<Exam>().Include(exam => exam.City).ToListAsync();
            List<ExamViewModel> examViewModels = new List<ExamViewModel>();
            exams.ForEach(x => examViewModels.Add(ExamMappings.ToViewModel(x)));
            examViewModels = examViewModels.OrderByDescending(x => x.Date).ToList();
            examViewModels.ForEach(x => x.SelectedCity = exams.FirstOrDefault(y => y.Id == x.Id).City.Name);

            List<string> AcceptancePeriod = CalculateAcceptancePeriods(examViewModels);


            var stringAcceptancePeriod = new List<SelectListItem>();
            foreach (var ap in AcceptancePeriod)
            {
                stringAcceptancePeriod.Add(new SelectListItem { Value = AcceptancePeriod.FindIndex(a => a == ap).ToString(), Text = ap });
            }
            viewModel.AcceptancePeriod = stringAcceptancePeriod;
            viewModel.ExamViewModels = examViewModels;
            viewModel.ExamViewModels = examViewModels.Where(x => x.SetAcceptancePeriod == stringAcceptancePeriod.ElementAt(value).Text).ToList();
            viewModel.SelectedAcceptancePeriod = stringAcceptancePeriod.ElementAt(value).Text;
            TempData["SelectedAcceptancePeriod"] = stringAcceptancePeriod.ElementAt(value).Value;

            var selectedExams = examViewModels.Where(x => x.SetAcceptancePeriod == stringAcceptancePeriod.ElementAt(value).Text).ToList();

            var results = await _resultRepository.Query<Result>().ToListAsync();
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
            ExamViewModel examViewModel = new ExamViewModel();
            List<City> cities = await _examRepository.Query<City>().ToListAsync();

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
                var latestDate = _examRepository.Query<Exam>()
                                .OrderBy(x => x.Created)
                                .LastOrDefault();

                var exam = ExamMappings.ToEntity(examViewModel);
                var city = await _cityRepository.Query<City>().FirstOrDefaultAsync(x => x.Name == examViewModel.SelectedCity);
                exam.CityId = city.Id;
                if (latestDate != null)
                {
                    exam.Tasks = latestDate.Tasks;
                    await _examRepository.InsertAsync(exam);
                    return RedirectToAction(nameof(Index));
                }
                await _examRepository.InsertAsync(exam);
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

            var exam = await _examRepository.Query<Exam>()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exam == null)
            {
                return NotFound();
            }
            var examById = await _examRepository.FindByIdAsync(id);
            var result = await _resultRepository.Query<Result>().FirstOrDefaultAsync(x => x.ExamId == examById.Id);
            int.TryParse(TempData["SelectedAcceptancePeriod"].ToString(), out int SelectedAcceptancePeriod);

            if (result != null)
            {
                var studentById = await _studentRepository.FindByIdAsync(result.StudentForeignKey);
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
                var oldestExam = await _examRepository.Query<Exam>().OrderBy(m => m.Date).FirstOrDefaultAsync();
                await RemoveFromExams(examById);
                if (examById.Date == oldestExam.Date)
                    return Redirect($"/Exams/Index?value={SelectedAcceptancePeriod - 1}");
                return Redirect($"/Exams/Index?value={SelectedAcceptancePeriod}");
            }
        }

        private async Task<IActionResult> RemoveFromExams(Exam examById)
        {
            await _examRepository.DeleteAsync(examById.Id);
            return RedirectToAction(nameof(Index));
        }
        private static List<string> CalculateAcceptancePeriods(List<ExamViewModel> examViewModels)
        {
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
            return AcceptancePeriod;
        }
    }
}
