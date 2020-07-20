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
            TempData["ExamId"] = id;

            var passedStudents = await _repository.Query<Student>()
                .Include(i => i.Result)
                .ThenInclude(u => u.Exam)
                .Where(x => x.PassedExam == true)
                .Where(y => y.Result.Exam.Id == id)
                .ToListAsync();
            var conversationResult = new List<ConversationResult>();
            foreach (var student in passedStudents)
            {
                var p = await _repository.FindByIdAsync<ConversationResult>(student.ConversationResultId);
                conversationResult.Add(p);
            }
            List<StudentViewModel> studentViewModels = new List<StudentViewModel>();
            passedStudents.ForEach(x => studentViewModels.Add(ConversationResultMappings.ToStudentViewModel(x)));
            var exam = await _repository.Query<Exam>().Include(x => x.City).FirstOrDefaultAsync(x => x.Id == id);
            List<ConversationResultViewModel> conversationResultViewModel = new List<ConversationResultViewModel>();
            conversationResult.ForEach(x => conversationResultViewModel.Add(ConversationResultMappings.ToConversationResultViewModel(x)));
            conversationResultViewModel.ForEach(x => x.ExamCityAndDate = $"{exam.City.Name}, {exam.Date.ToShortDateString()}");
            ConversationResultMappings.ToStudentAndConversationResultViewModel(studentViewModels, conversationResultViewModel, id);

            return View(ConversationResultMappings.ToStudentAndConversationResultViewModel(studentViewModels, conversationResultViewModel, id));

        }
        //GET
        public async Task<IActionResult> EditConversationResult(int? id, int? examId)
        {
            int.TryParse(TempData["ExamId"].ToString(), out int ExamId);
            var passedStudents = await _repository.Query<Student>()
                .Include(i => i.Result)
                .ThenInclude(u => u.Exam)
                .Where(x => x.PassedExam == true)
                .Where(y => y.Result.Exam.Id == examId)
                .ToListAsync();

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
                conversationResult.Student = student;

                student.ConversationResult = conversationResult;
                conversationResult = await _repository.InsertAsync(conversationResult);
                student.ConversationResultId = conversationResult.Id;
                TempData["ConversationResultId"] = student.ConversationResultId;
                await _repository.SaveAsync();
                ConversationResultViewModel conversationResultViewModel = new ConversationResultViewModel();
                conversationResultViewModel.ConversationResultId = conversationResult.Id;
                TempData["ExamId"] = ExamId;
                examId = ExamId;
                return View(ConversationResultMappings.ToViewModel(student, conversationResult, examId));
            }
            else
            {
                var conversationResult = await _repository.FindByIdAsync<ConversationResult>(student.ConversationResultId);
                ConversationResultViewModel conversationResultViewModel = new ConversationResultViewModel();
                conversationResultViewModel.ConversationResultId = conversationResult.Id;
                TempData["ExamId"] = ExamId;
                examId = ExamId;
                return View(ConversationResultMappings.ToViewModel(student, conversationResult, examId));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConversationResult([Bind("Grade", "ConversationResultComment")] ConversationResultViewModel model)
        {
            int.TryParse(TempData["ConversationResultId"].ToString(), out int conversationResultId);
            int.TryParse(TempData["StudentId"].ToString(), out int studentId);
            int.TryParse(TempData["ExamId"].ToString(), out int examId);
            var studentFromDatabase = await _repository.FindByIdAsync<Student>(studentId);
            if (studentFromDatabase.InvitedToStudy == true)
            {
                ModelState.AddModelError("ConversationResultEdit", "Studentas yra pakviestas studijuoti, pokalbio įvertinimo redaguoti negalima");
                TempData["ErrorMessage"] = "Studentas yra pakviestas studijuoti, pokalbio įvertinimo redaguoti negalima";
                return RedirectToAction("EditConversationResult", "ConversationResults", new { id = studentId });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var studentRequest = _repository.Query<Student>().Include(x => x.ConversationResult).Where(x => x.Id > 0);
                    var conversationResult = await _repository.FindByIdAsync<ConversationResult>(conversationResultId);
                    var student = await studentRequest.FirstOrDefaultAsync(x => x.ConversationResultId == conversationResult.Id);
                    var conversationResultViewModel = ConversationResultMappings.ToViewModel(student, conversationResult, examId);
                    ConversationResultMappings.EditEntity(conversationResult, model);
                    await _repository.UpdateAsync(conversationResult);

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index", "ConversationResults", new { id = examId });
            }
            TempData["ErrorMessage"] = "Pokalbio įvertinimas turi būti nuo 0 iki 10";
            ModelState.AddModelError("ConversationResultRange", "Pokalbio įvertinimas turi būti nuo 0 iki 10");
            return RedirectToAction("EditConversationResult", "ConversationResults", new { id = studentId });
        }

    }
}
