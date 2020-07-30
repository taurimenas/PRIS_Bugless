using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using CsvHelper;
using System.Text.RegularExpressions;
using System.Globalization;
using PRIS.Web.Models;
using PRIS.Core.Library;
using PRIS.Web.Mappings;
using PRIS.Core.Library.Entities;
using PRIS.Web.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PRIS.Web.Controllers
{
    public class ImportCSVController : Controller
    {
        private IWebHostEnvironment _environment;
        private readonly IRepository _repository;
        private readonly ILogger<ImportCSVController> _logger;
        private readonly string _user;

        public ImportCSVController(IWebHostEnvironment environment, IRepository repository, ILogger<ImportCSVController> logger, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _repository = repository;
            _logger = logger;
            _user = httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            if (file == null)
            {
                TempData["ErrorMessage"] = "Nėra įkeltas failas.";
                return RedirectToAction("Index", "ImportCSV");
            }
            if (!file.FileName.EndsWith(".csv"))
            {
                TempData["ErrorMessage"] = "Prašome įkelti .csv failą.";
                return RedirectToAction("Index", "ImportCSV");
            }
            int.TryParse(TempData["ExamId"].ToString(), out int examId);

            var programs = await _repository.Query<Core.Library.Entities.Program>().ToListAsync();
            using (StreamReader streamReaderValidation = new StreamReader(file.OpenReadStream()))
            {
                string dataFromCSV;
                while ((dataFromCSV = streamReaderValidation.ReadLine()) != null)
                {
                    var seperatedData = dataFromCSV.Split(";");
                    if (seperatedData.Length > 20)
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile yra per daug stulpelių arba CSV faile tarp duomenų yra naudojamas kabliataškis(;).";
                        return RedirectToAction("ImportCSV", "ImportCSV");
                    }
                    if (seperatedData.Length < 20)
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile yra per mažai stulpelių arba CSV failo skirtuvas yra blogas.";
                        return RedirectToAction("Index", "ImportCSV");
                    }
                    if (seperatedData[1] == "")
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra kandidatas, kuris neturi vardo ir pavardės.";
                        return RedirectToAction("Index", "ImportCSV");
                    }

                    var programFromCSV = programs.FirstOrDefault(x => x.Name == seperatedData[5]);
                    if (programFromCSV == null)
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra programa, kuri neegzistuoja sitemoje. Pirma sukurkite programą, tada kelkite failą.";
                        return RedirectToAction("Index", "ImportCSV");
                    }
                    var firstNameAndLastName = seperatedData[1].Split(" ");

                    if (firstNameAndLastName.First() == "" || firstNameAndLastName.First() == " ")
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra kandidatas, kurio vardo yra negalima aptikti. Patikrinkite ar nėra tarpo prieš vardą.";
                        return RedirectToAction("Index", "ImportCSV");
                    }
                    if (firstNameAndLastName.Last() == "" || firstNameAndLastName.Last() == " ")
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra kandidatas, kurio pavardės negalima aptikti.";
                        return RedirectToAction("Index", "ImportCSV");
                    }
                    if (seperatedData[16].ToLower() != "taip" && seperatedData[17] != "")
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra kandidatas, kuris nėra pakviestas į pokalbį, bet turi pokalbio įvertinimą.";
                        return RedirectToAction("Index", "ImportCSV");
                    }
                    if (seperatedData[16].ToLower() != "taip" && seperatedData[18].ToLower() == "taip")
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra kandidatas, kuris nėra pakviestas į pokalbį, bet yra kviečiamas studijuoti.";
                        return RedirectToAction("Index", "ImportCSV");
                    }
                    if (seperatedData[16].ToLower() != "taip" && seperatedData[19].ToLower() == "taip")
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra kandidatas, kuris nėra pakviestas į pokalbį, bet yra pasirašęs sutartį.";
                        return RedirectToAction("Index", "ImportCSV");
                    }
                    if (seperatedData[18].ToLower() != "taip" && seperatedData[19].ToLower() == "taip")
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra kandidatas, kuris yra pasirašęs sutartį, bet nėra pakviestas studijuoti.";
                        return RedirectToAction("Index", "ImportCSV");
                    }

                }
            }
            using (StreamReader streamReader = new StreamReader(file.OpenReadStream()))
            {
                string studentDataFromCSV;
                while ((studentDataFromCSV = streamReader.ReadLine()) != null)
                {
                    var seperatedData = studentDataFromCSV.Split(";");

                    var firstNameAndLastName = seperatedData[1].Split(" ");
                    var dataFromCSV = new ImportedStudentsDataModel();
                    DataFromCSVMappingToImportStudentsDataModel(seperatedData, dataFromCSV, firstNameAndLastName);

                    Result result = new Result
                    {
                        ExamId = examId,
                    };
                    await _repository.InsertAsync<Result>(result);
                    var student = new Student();
                    await _repository.InsertAsync<Student>(student);
                    if (dataFromCSV.ConversationResult == null)
                    {
                        ImportedCSVMappings.ToEntityWithoutConversationResult(student, result, dataFromCSV);
                    }
                    else
                    {
                        var coversationResult = new ConversationResult();
                        await _repository.InsertAsync<ConversationResult>(coversationResult);
                        ImportedCSVMappings.ToEntity(student, result, coversationResult, dataFromCSV);
                    }
                    var studentsExam = _repository.Query<Exam>().FirstOrDefault(x => x.Id == examId);

                    foreach (var program in programs)
                    {
                        StudentCourse studentCourse = new StudentCourse
                        {
                            StudentId = student.Id,
                            Student = student
                        };

                        var priorityProgram = _repository.Query<Core.Library.Entities.Program>()
                            .Where(x => x.Name == program.Name)
                            .FirstOrDefault();
                        var priorityCourse = _repository.Query<Course>()
                            .Where(x => x.ProgramId == priorityProgram.Id)
                            .Where(x => x.StartYear.Year == studentsExam.Date.Year)
                            .FirstOrDefault();

                        if (priorityCourse != null)
                        {
                            studentCourse.CourseId = priorityCourse.Id;
                        }
                        else
                        {
                            Course course = new Course
                            {
                                StartYear = studentsExam.Date,
                                EndYear = studentsExam.Date.AddYears(1),
                                CityId = studentsExam.CityId,
                                ProgramId = priorityProgram.Id,
                                Title = priorityProgram.Name
                            };
                            studentCourse.CourseId = course.Id;
                            studentCourse.Course = course;
                            await _repository.InsertAsync<Course>(course);
                        }
                        var programFromCSV = programs.FirstOrDefault(x => x.Name == dataFromCSV.Priority);
                        if (programFromCSV.Name == program.Name)
                            studentCourse.Priority = 1;
                        else
                            studentCourse.Priority = null;

                        await _repository.InsertAsync<StudentCourse>(studentCourse);
                        await _repository.SaveAsync();
                    }
                }
            }
            return RedirectToAction("Index", "Students", new { id = examId });
        }
        private static void DataFromCSVMappingToImportStudentsDataModel(string[] seperatedData, ImportedStudentsDataModel dataFromCSV, string[] firstNameAndLastName)
        {
            dataFromCSV.FirstName = firstNameAndLastName.First();
            dataFromCSV.LastName = firstNameAndLastName.Last();
            if (seperatedData[2] == "")
                dataFromCSV.Email = "-";
            else
                dataFromCSV.Email = seperatedData[2];
            if (seperatedData[3] == "")
                dataFromCSV.PhoneNumber = "-";
            else
                dataFromCSV.PhoneNumber = seperatedData[3];
            if (seperatedData[4].ToLower() == "m")
                dataFromCSV.Gender = Gender.Moteris;
            else if (seperatedData[4].ToLower() == "v")
                dataFromCSV.Gender = Gender.Vyras;
            else
                dataFromCSV.Gender = Gender.Kita;
            dataFromCSV.Priority = seperatedData[5];
            string[] tasks = new string[10];
            for (int i = 6; i < 16; i++)
            {
                if (seperatedData[i] == "")
                    tasks[i - 6] = "0";
                else
                    tasks[i - 6] = seperatedData[i].Replace(',', '.');

            }
            dataFromCSV.Tasks = $"[{String.Join(",", tasks)}]";

            if (seperatedData[16].ToLower() == "taip")
                dataFromCSV.PassedExam = true;
            else
                dataFromCSV.PassedExam = false;
            if (double.TryParse(seperatedData[17], out double conversationResult))
                dataFromCSV.ConversationResult = conversationResult;
            else
                dataFromCSV.ConversationResult = null;
            if (seperatedData[18].ToLower() == "taip")
                dataFromCSV.InvitationToStudy = true;
            else
                dataFromCSV.InvitationToStudy = false;
            if (seperatedData[19].ToLower() == "taip")
                dataFromCSV.SignedAContract = true;
            else
                dataFromCSV.SignedAContract = false;
        }
    }
}





