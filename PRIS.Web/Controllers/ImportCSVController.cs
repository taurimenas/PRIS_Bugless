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
            using (var fileStream = new FileStream(Path.Combine(dir,"file.csv"), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }
            using (StreamReader streamReader = new StreamReader("file.csv"))
            {
                string studentDataFromCSV;
                while ((studentDataFromCSV = streamReader.ReadLine()) != null)
                {
                    Console.WriteLine(studentDataFromCSV);
                    
                    var result = studentDataFromCSV.Split(";");

                    foreach (var item in result)
                    {
                        Console.WriteLine(item);
                    }
                }
               
            };

            return RedirectToAction("Index");
        }

    }
}

