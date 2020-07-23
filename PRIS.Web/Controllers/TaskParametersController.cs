using Microsoft.AspNetCore.Mvc;
using PRIS.Web.Data;
using PRIS.Web.Mappings;
using System.Threading.Tasks;

namespace PRIS.Web.Controllers
{
    public class TaskParametersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskParametersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tasks = await _context.Exams.FindAsync(id);
            if (tasks == null)
            {
                return NotFound();
            }
            return View(TaskParametersMappings.ToTaskParameterViewModel(tasks));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, double[] tasks)
        {
            int.TryParse(TempData["Count"].ToString(), out int studentsCountInAcceptancePeriod);
            int.TryParse(TempData["SelectedAcceptancePeriod"].ToString(), out int SelectedAcceptancePeriod);
            TempData["ExamId"] = id;

            if (studentsCountInAcceptancePeriod > 0)
            {
                TempData["ErrorMessage"] = "Šablono keisti negalima, nes prie jo jau yra priskirta kandidatų.";
                ModelState.AddModelError("ExamsError", "Šablono keisti negalima, nes prie jo jau yra priskirta kandidatų.");
                return RedirectToAction("Edit", "TaskParameters", new { id });
            }
            else
            {
                var exam = await _context.Exams.FindAsync(id);
                if (id != exam.Id)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    TaskParametersMappings.EditTaskParametersEntity(exam, tasks);
                    await _context.SaveChangesAsync();
                    return Redirect($"/Exams/Index?value={SelectedAcceptancePeriod}");
                }
                return View(TaskParametersMappings.ToTaskParameterViewModel(tasks));
            }
        }
    }
}