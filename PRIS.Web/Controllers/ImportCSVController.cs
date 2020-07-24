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

namespace PRIS.Web.Controllers
{
    public class ImportCSVController : Controller
    {
        private IWebHostEnvironment _environment;
        public ImportCSVController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult ImportCSV()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ImportCSV(IFormFile file)
        {
            var dir = _environment.ContentRootPath;
            using (var fileStream = new FileStream(Path.Combine(dir, "file.csv"), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
                    var dt = new DataTable();
            using (StreamReader streamReader = new StreamReader("file.csv"))
            {
# region for CsvHelper

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
                    Console.WriteLine(studentDataFromCSV);

                    var seperatedData = studentDataFromCSV.Split(";");
                    
                    string[] Tasks = new string[10];
                    for (int i = 4; i < 15 ; i++)
                    {
                        Tasks[i - 4] = seperatedData[i];
                    }
                    foreach (var oneColumnData in seperatedData)
                    {
                        Console.WriteLine(oneColumnData);
                    }
                }

            };

            return RedirectToAction("Index");
        }

    }
}

