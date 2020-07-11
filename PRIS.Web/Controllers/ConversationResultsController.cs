using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIS.Core.Library.Entities;
using PRIS.Web.Mappings;
using PRIS.Web.Models;
using PRIS.Web.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRIS.Web.Controllers
{
    public class ConversationResultsController : Controller
    {
        private readonly IRepository _repository;
        
        public ConversationResultsController(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index(int? id)
        {
            var passedStudents = await _repository.Query<Student>()
                .Include(i => i.Result)
                .ThenInclude(u => u.Exam)
                .Where(x => x.PassedExam == true)
                .Where(y => y.Result.Exam.Id == id)
                .ToListAsync();


            var conversationResult = new List<ConversationResult>();
            var getGradeNull = passedStudents.Where(x => x.ConversationResult.Grade == null);
            if (getGradeNull == null)
            {
                return View();
            }

            foreach (var student in passedStudents)
            {
                var p = await _repository.FindByIdAsync<ConversationResult>(student.ConversationResultId);
                conversationResult.Add(p);
            }
            List<ConversationResultViewModel> conversationResultViewModel = new List<ConversationResultViewModel>();
            List<StudentViewModel> studentViewModels = new List<StudentViewModel>();
            passedStudents.ForEach(x => studentViewModels.Add(ConversationResultMappings.ToStudentViewModel(x)));
            conversationResult.ForEach(x => conversationResultViewModel.Add(ConversationResultMappings.ToConversationResultViewModel(x)));

            ConversationResultMappings.ToStudentAndConversationResultViewModel(studentViewModels, conversationResultViewModel);

            return View(ConversationResultMappings.ToStudentAndConversationResultViewModel(studentViewModels, conversationResultViewModel));

        }
        //GET
        public async Task<IActionResult> EditConversationResult(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = await _repository.FindByIdAsync<Student>(id);
            TempData["ConversationResultId"] = student.ConversationResultId;
            TempData["StudentId"] = student.Id;
            if (student.ConversationResultId == null)
            {
                ConversationResult conversationResult = new ConversationResult();
                conversationResult = await _repository.InsertAsync(conversationResult);
                ConversationResultViewModel conversationResultViewModel = new ConversationResultViewModel();
                conversationResultViewModel.ConversationResultId = conversationResult.Id;
                return View(ConversationResultMappings.ToViewModel(student, conversationResult));
            }
            else
            {
                var conversationResult = await _conversationResult.FindByIdAsync(student.ConversationResultId);
                ConversationResultViewModel conversationResultViewModel = new ConversationResultViewModel();
                conversationResultViewModel.ConversationResultId = conversationResult.Id;
                return View(ConversationResultMappings.ToViewModel(student, conversationResult));
            }
        }
        //POST
        //[System.Web.Mvc.ChildActionOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConversationResult([Bind("Grade", "ConversationResultComment")] ConversationResultViewModel model)
        {
            int.TryParse(TempData["ConversationResultId"].ToString(), out int conversationResultId);
            int.TryParse(TempData["StudentId"].ToString(), out int studentId);
            int.TryParse(TempData["ExamId"].ToString(), out int examId);
            if (ModelState.IsValid)
            {
                try
                {

                    var studentRequest = _studentRepository.Query<Student>().Include(x => x.ConversationResult).Where(x => x.Id > 0);
                    var conversationResult = await _conversationResult.FindByIdAsync(conversationResultId);
                    var student = await studentRequest.FirstOrDefaultAsync(x => x.ConversationResultId == conversationResult.Id);
                    var conversationResultViewModel = ConversationResultMappings.ToViewModel(student, conversationResult);
                    ConversationResultMappings.EditEntity(conversationResult, model);
                    await _conversationResult.UpdateAsync(conversationResult);

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                // TODO: turi redirectinti i sarasa studentu kurie pakviesti pokalbiui ------------ DONE
                return RedirectToAction("Index", "ConversationResults", new { id = examId });
            }
            //TODO: turi redirectinti i ta pati studenta --------- DONE
            TempData["ErrorMessage"] = "Pokalbio įvertinimas turi būti nuo 0 iki 10";
            ModelState.AddModelError("ConversationResultRange", "Pokalbio įvertinimas turi būti nuo 0 iki 10");
            return RedirectToAction("EditConversationResult", "ConversationResults", new { id = studentId });
        }

    }
}
