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

namespace PRIS.Web.Controllers
{
    public class ImportCSVController : Controller
    {
        private IWebHostEnvironment _environment;
        private readonly IRepository _repository;

        public ImportCSVController(IWebHostEnvironment environment, IRepository repository)
        {
            _environment = environment;
            _repository = repository;
        }

        public IActionResult Index()
        {
            var returnPath = HttpContext.Request.Headers["Referer"].ToString();
            TempData["ReturnPath"] = returnPath;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            var previousUrl = TempData["ReturnPath"].ToString();

            if (!file.FileName.EndsWith(".csv"))
            {
                TempData["ErrorMessage"] = "Prašome įkelti .csv failą.";
                return RedirectToAction("ImportCSV", "ImportCSV");
            }
            int.TryParse(TempData["ExamId"].ToString(), out int examId);
            var dir = _environment.ContentRootPath;
            using (var fileStream = new FileStream(Path.Combine(dir, "file.csv"), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
            var programs = await _repository.Query<Core.Library.Entities.Program>().ToListAsync();
            using (StreamReader streamReaderValidation = new StreamReader("file.csv"))
            {
                var csv = new CsvReader(streamReaderValidation, CultureInfo.CurrentCulture)
                string dataFromCSV;
                while ((dataFromCSV = streamReaderValidation.ReadLine()) != null)
                {
                    var seperatedData = dataFromCSV.Split(";");
                    if (seperatedData.Length > 18)
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile yra per daug stulpelių.";
                        return RedirectToAction("ImportCSV", "ImportCSV");
                    }
                    if (seperatedData.Length < 18)
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile yra per mažai stulpelių.";
                        return RedirectToAction("ImportCSV", "ImportCSV");
                    }
                    if (seperatedData[1] == "")
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra kandidatas, kuris neturi vardo ir pavardės.";
                        return RedirectToAction("ImportCSV", "ImportCSV");
                    }
                    
                    var programFromCSV = programs.FirstOrDefault(x => x.Name == seperatedData[2]);
                    if (programFromCSV == null)
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra programa, kuri neegzistuoja sitemoje. Pirma sukurkite programą, tada kelkite failą.";
                        return RedirectToAction("ImportCSV", "ImportCSV");
                    }
                    var firstNameAndLastName = seperatedData[1].Split(" ");
                    
                    if (firstNameAndLastName.First() == "" || firstNameAndLastName.First() == " ") 
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra kandidatas, kurio vardo yra negalima aptikti. Patikrinkite ar nėra tarpo prieš vardą.";
                        return RedirectToAction("ImportCSV", "ImportCSV");
                    }
                    if (firstNameAndLastName.Last() == "" || firstNameAndLastName.Last() == " ")
                    {
                        TempData["ErrorMessage"] = $"Jūsų faile {seperatedData[0]} eilutėje yra kandidatas, kurio pavardės negalima aptikti.";
                        return RedirectToAction("ImportCSV", "ImportCSV");
                    }
                }
            }
            using (StreamReader streamReader = new StreamReader("file.csv"))
            {

                #region for CsvHelper

                //using (var csv = new CsvReader(streamReader, CultureInfo.CurrentCulture))
                //{
                //using (var dr = new CsvDataReader(csv))
                //{
                //csv.ReadHeader();
                //dt.Columns.Add("CsvId", typeof(string));
                //dt.Columns.Add("NameSurname", typeof(string));
                //dt.Columns.Add("Priority", typeof(string));
                //dt.Columns.Add("Task1", typeof(string));
                //dt.Columns.Add("Task2", typeof(string));
                //dt.Columns.Add("Task3", typeof(string));
                //dt.Columns.Add("Task4", typeof(string));
                //dt.Columns.Add("Task5", typeof(string));
                //dt.Columns.Add("Task6", typeof(string));
                //dt.Columns.Add("Task7", typeof(string));
                //dt.Columns.Add("Task8", typeof(string));
                //dt.Columns.Add("Task9", typeof(string));
                //dt.Columns.Add("Task10", typeof(string));
                //dt.Columns.Add("Gender", typeof(string));
                //dt.Columns.Add("PassedExam", typeof(string));
                //dt.Columns.Add("ConversationResult", typeof(string));
                //dt.Columns.Add("InvitationToStudy", typeof(string));
                //dt.Columns.Add("SignedAContract", typeof(string));
                //while (csv.Read())
                //{
                //    DataRow row = dt.NewRow();
                //    foreach (DataColumn column in dt.Columns)
                //    {
                //        row[column.ColumnName]= csv.GetField(column.DataType, column.ColumnName);

                //    }
                //    dt.Rows.Add(row);
                //}
                // }


                //var reco = csv.GetRecords<ImportStudentsData>();
                //foreach (var item in reco)
                //{
                //Console.WriteLine(item);

                //}
                #endregion
                string studentDataFromCSV;
                while ((studentDataFromCSV = streamReader.ReadLine()) != null)
                {
                    var seperatedData = studentDataFromCSV.Split(";");

                    var firstNameAndLastName = seperatedData[1].Split(" ");
                    var dataFromCSV = new ImportStudentsDataModel();
                    DataFromCSVMappingToImportStudentsDataModel(seperatedData, dataFromCSV, firstNameAndLastName);

                    Result result = new Result
                    {
                        ExamId = examId,
                    };
                    await _repository.InsertAsync<Result>(result);
                    var student = new Student();
                    var coversationResult = new ConversationResult();
                    await _repository.InsertAsync<Student>(student);
                    await _repository.InsertAsync<ConversationResult>(coversationResult);
                    ImportedCSVMappings.ToEntity(student, result, coversationResult, dataFromCSV);

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
            return Redirect(previousUrl);
        }
        private static void DataFromCSVMappingToImportStudentsDataModel(string[] seperatedData, ImportStudentsDataModel dataFromCSV, string[] firstNameAndLastName)
        {
            dataFromCSV.FirstName = firstNameAndLastName.First();
            dataFromCSV.LastName = firstNameAndLastName.Last();
            dataFromCSV.Priority = seperatedData[2];
            string[] tasks = new string[10];
            for (int i = 3; i < 13; i++)
            {
                if (seperatedData[i] == "")
                {
                    tasks[i - 3] = "0";
                }
                else
                {
                    tasks[i - 3] = seperatedData[i].Replace(',', '.');
                }
            }
            dataFromCSV.Tasks = $"[{String.Join(",", tasks)}]";
            if (seperatedData[13].ToLower() == "m")
                dataFromCSV.Gender = Gender.Moteris;
            else if (seperatedData[13].ToLower() == "v")
                dataFromCSV.Gender = Gender.Vyras;
            else
                dataFromCSV.Gender = Gender.Kita;

            if (seperatedData[14] == "Taip")
                dataFromCSV.PassedExam = true;
            else
                dataFromCSV.PassedExam = false;
            if (double.TryParse(seperatedData[15], out double conversationResult))
                dataFromCSV.ConversationResult = conversationResult;
            else
                dataFromCSV.ConversationResult = null;
            if (seperatedData[16] == "Taip")
                dataFromCSV.InvitationToStudy = true;
            else
                dataFromCSV.InvitationToStudy = false;
            if (seperatedData[17] == "Taip")
                dataFromCSV.SignedAContract = true;
            else
                dataFromCSV.SignedAContract = false;
        }
    }
}





