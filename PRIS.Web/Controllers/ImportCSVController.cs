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
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            };

            return RedirectToAction("Index");
        }

    }
}

