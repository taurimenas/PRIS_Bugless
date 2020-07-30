using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRIS.Core.Library.Entities;
using PRIS.Web.Mappings;
using PRIS.Web.Models;
using PRIS.Web.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Controllers
{
    [Authorize]
    public class ProgramsController : Controller
    {
        private readonly IRepository _repository;
        private readonly ILogger<ProgramsController> _logger;
        private readonly string _user;

        public ProgramsController(IRepository repository, ILogger<ProgramsController> logger, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _logger = logger;
            _user = httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
        }

        public async Task<IActionResult> Index()
        {
            ProgramViewModel programViewModel = new ProgramViewModel();

            var programResult = await _repository.Query<Core.Library.Entities.Program>().ToListAsync();
            List<ProgramCreateModel> programsList = new List<ProgramCreateModel>();
            programResult.ForEach(program => programsList.Add(ProgramMappings.ToProgramListViewModel(program)));
            programViewModel.ProgramNames = programsList;

            var cities = await _repository.Query<City>().ToListAsync();
            List<CityCreateModel> citiesList = new List<CityCreateModel>();
            cities.ForEach(city => citiesList.Add(ProgramMappings.ToCityListViewModel(city)));
            programViewModel.CityNames = citiesList;

            _logger.LogInformation($"{programsList.Count()} programs and {citiesList.Count()} cities exist in view. User {_user}");
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
                var beforeCreatedProgam = _repository.Query<Core.Library.Entities.Program>().FirstOrDefault(x => x.Name == result.Name);
                if (beforeCreatedProgam != null)
                {
                    _logger.LogWarning($"User {_user} tried to create existing program {result.Name}");
                    TempData["ErrorMessage"] = "Ši programa jau yra sukurta";
                    return RedirectToAction(nameof(Create));
                }
                await _repository.InsertAsync<Core.Library.Entities.Program>(result);
                _logger.LogInformation($"User {_user} created program {result.Name}");
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError($"Error with ModelState validation. User {_user} ");
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
                var beforeCreatedCity = _repository.Query<City>().FirstOrDefault(x => x.Name == result.Name);
                if (beforeCreatedCity != null)
                {
                    _logger.LogWarning($"User {_user} tried to create existing city {result.Name}");
                    TempData["ErrorMessage"] = "Šis miestas jau yra sukurtas";
                    return RedirectToAction(nameof(CreateNewCity));
                }
                await _repository.InsertAsync<City>(result);
                _logger.LogInformation($"User {_user} created city {result.Name}");
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError($"Error with ModelState validation. User {_user} ");
            return View(cityCreateModel);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError($"Program was not found. User {_user} ");
                return NotFound();
            }
            var program = await _repository.Query<Core.Library.Entities.Program>()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (program == null)
            {
                _logger.LogError($"Program with {id} id was not found. User {_user} ");
                return NotFound();
            }
            var programById = await _repository.FindByIdAsync<Core.Library.Entities.Program>(id);
            var course = await _repository.Query<Course>().FirstOrDefaultAsync(x => x.Title == program.Name);
            if (course != null)
            {

                _logger.LogWarning($"{program.Name} program with {id} id was impossible to delete because it has students. User {_user} ");
                return await BadRequest(course, "Programos negalima ištrinti, nes prie jos jau yra priskirta kandidatų.");
            }
            else
            {
                await _repository.DeleteAsync<Core.Library.Entities.Program>(programById.Id);
                _logger.LogInformation($"{program.Name} program with {id} id was deleted. User {_user} ");
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                _logger.LogError($"City was not found. User {_user} ");
                return NotFound();
            }
            var city = await _repository.FindByIdAsync<City>(id);
            if (city == null)
            {
                _logger.LogError($"City with {id} id was not found. User {_user} ");
                return NotFound();
            }

            var exam = await _repository.Query<Exam>().FirstOrDefaultAsync(x => x.CityId == city.Id);
            if (exam != null)
            {
                var exams = await _repository.Query<Exam>().ToListAsync();
                var studentCourse = exams.Where(x => x.Id == exam.Id);
                if (studentCourse.Any())
                {
                    TempData["ErrorMessage"] = "Miesto negalima ištrinti, nes prie jo jau yra priskirta kandidatų.";
                }
                _logger.LogWarning($"{city.Name} city with {id} id was impossible to delete because it has students. User {_user} ");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogInformation($"{city.Name} city with {id} id was deleted. User {_user} ");
                await _repository.DeleteAsync<City>(city.Id);
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<IActionResult> BadRequest(Course course, string errorMessage)
        {
            var studentsCourses = await _repository.Query<StudentCourse>().ToListAsync();
            var studentCourse = studentsCourses.Where(x => x.CourseId == course.Id);
            if (studentCourse.Any())
            {
                ModelState.AddModelError("AssignedStudent", errorMessage);
                TempData["ErrorMessage"] = errorMessage;
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
