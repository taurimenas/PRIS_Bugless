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
            ConversationResult conversationResult = new ConversationResult
            {
            };
            ConversationResultViewModel conversationResultViewModel = new ConversationResultViewModel();
            student.ConversationResult = conversationResult;
            conversationResult = await _conversationResult.InsertAsync(conversationResult);
            conversationResultViewModel.ConversationResultId = conversationResult.Id;

            return View(ConversationResultMappings.ToViewModel(student, conversationResult));
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConversationResult(int? id, [Bind("Grade", "ConversationResultComment")] ConversationResultViewModel model)
        {
            ConversationResultViewModel conversationResultViewModel = new ConversationResultViewModel();

            var student = await _studentRepository.FindByIdAsync(model.StudentId);


            var conversation = await _conversationResult.FindByIdAsync(student.ConversationResult.Id);
            if (ModelState.IsValid)
            {
                ConversationResultMappings.EditEntity(conversation, model);
                await _conversationResult.SaveAsync();

                return RedirectToAction("EditConversationResult", "ConversationResult");
            }
            return View();
        }

    }
}

//post
    //, [Bind("Grade", "ConversationResultComment")] ConversationResultViewModel model
    //var conversationResultEntity = await _conversationResult.FindByIdAsync(student.ConversationResult.Id);

    //ConversationResultMappings.EditEntity(conversationResultEntity, model);