using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRIS.Core.Library.Entities;
using PRIS.Web.Mappings;
using PRIS.Web.Models;
using PRIS.Web.Storage;

namespace PRIS.Web.Controllers
{
    public class ExamsToConversationsController : Controller
    {
        private readonly IRepository _repository;
        private readonly ILogger<ExamsToConversationsController> _logger;
        private readonly string _user;


        public ExamsToConversationsController(IRepository repository, ILogger<ExamsToConversationsController> logger, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _logger = logger;
            _user = httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
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
            _logger.LogInformation("Found {Count} records. At {Time}. User {User}.", viewModel.ExamViewModels.Count(), DateTime.UtcNow, _user);
            return View(viewModel);
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
