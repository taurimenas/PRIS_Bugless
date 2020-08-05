using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PRIS.Core.Library.Entities;
using PRIS.Web.Data;
using PRIS.Web.Mappings;
using PRIS.Web.Storage;
using System;
using System.Threading.Tasks;

namespace PRIS.Web.Controllers
{
    public class TaskParametersController : Controller
    {
        private readonly IRepository _repository;
        private readonly ILogger<ExamsController> _logger;

        public TaskParametersController(IRepository repository, ILogger<ExamsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("ExamId not found. User {User}.", User.Identity.Name);
                return NotFound();
            }
            var tasks = await _repository.FindByIdAsync<Exam>(id);
            if (tasks == null)
            {
                _logger.LogWarning("Tasks not found. User {User}.", User.Identity.Name);
                return NotFound();
            }
            var setTaskParameterModel = TaskParametersMappings.ToTaskParameterViewModel(tasks);
            setTaskParameterModel.TaskString = new string[setTaskParameterModel.Tasks.Length];
            for (int i = 0; i < setTaskParameterModel.TaskString.Length; i++)
            {
                setTaskParameterModel.TaskString[i] = setTaskParameterModel.Tasks[i].ToString().Replace(",", ".");
            }
            return View(setTaskParameterModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, string[] tasksString)
        {
            int.TryParse(TempData["Count"].ToString(), out int studentsCountInAcceptancePeriod);
            int.TryParse(TempData["SelectedAcceptancePeriod"].ToString(), out int SelectedAcceptancePeriod);
            TempData["ExamId"] = id;
            double[] tasks = new double[tasksString.Length];
            if (studentsCountInAcceptancePeriod > 0)
            {
                TempData["ErrorMessage"] = "Šablono keisti negalima, nes prie jo jau yra priskirta kandidatų.";
                _logger.LogWarning("Can not change Tasks, {Count} students found in this period. User {User}.", studentsCountInAcceptancePeriod, User.Identity.Name);
                return RedirectToAction("Edit", "TaskParameters", new { id });
            }
            else
            {
                var exam = await _repository.FindByIdAsync<Exam>(id);
                if (id != exam.Id)
                {
                    _logger.LogWarning("Can not find Exam by id = {Id}. User {User}.", id, User.Identity.Name);
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    for (int i = 0; i < tasksString.Length; i++)
                    {
                        tasks[i] = double.Parse(tasksString[i], System.Globalization.CultureInfo.InvariantCulture);
                    }
                    TaskParametersMappings.EditTaskParametersEntity(exam, tasks);
                    await _repository.SaveAsync();
                    _logger.LogWarning("Can not find Exam by id = {Id}. User {User}.", id, User.Identity.Name);
                    return Redirect($"/Exams/Index?value={SelectedAcceptancePeriod}");
                }
                _logger.LogError("Something went wrong with modelstate. User {User}.", User.Identity.Name);
                return View(TaskParametersMappings.ToTaskParameterViewModel(tasks));
            }
        }
    }
}