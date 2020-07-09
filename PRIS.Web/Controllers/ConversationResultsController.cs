using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIS.Core.Library.Entities;
using PRIS.Web.Storage;
using PRIS.Web.Controllers;
using System.Security.Cryptography.X509Certificates;
using PRIS.Web.Mappings;
using PRIS.Web.Models;
using MvcContrib.Filters;

namespace PRIS.Web.Controllers
{
    public class ConversationResultsController : Controller
    {
        private readonly Repository<Student> _studentRepository;
        private readonly Repository<ConversationResult> _conversationResult;

        public ConversationResultsController(Repository<Student> studentRepository, Repository<ConversationResult> conversationResult)
        {
            _studentRepository = studentRepository;
            _conversationResult = conversationResult;
        }
        //GET
        public async Task<IActionResult> EditConversationResult(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var student = await _studentRepository.FindByIdAsync(id);
            TempData["ConversationResultId"] = student.ConversationResultId;
            TempData["StudentId"] = student.Id;
            if (student.ConversationResultId == null)
            {

                ConversationResult conversationResult = new ConversationResult();
                conversationResult = await _conversationResult.InsertAsync(conversationResult);
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
        [System.Web.Mvc.ChildActionOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConversationResult([Bind("Grade", "ConversationResultComment")] ConversationResultViewModel model)
        {
            int.TryParse(TempData["ConversationResultId"].ToString(), out int conversationResultId);
            int.TryParse(TempData["StudentId"].ToString(), out int studentId);
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
                // TODO: turi redirectinti i sarasa studentu kurie pakviesti pokalbiui
                return RedirectToAction("Index", "Exams");
            }
            //TODO: turi redirectinti i ta pati studenta --------- DONE
            TempData["ErrorMessage"] = "Pokalbio įvertinimas turi būti nuo 0 iki 10";
            ModelState.AddModelError("ConversationResultRange", "Pokalbio įvertinimas turi būti nuo 0 iki 10");
            return RedirectToAction("EditConversationResult", "ConversationResults", new { id = studentId });
        }

    }
}
