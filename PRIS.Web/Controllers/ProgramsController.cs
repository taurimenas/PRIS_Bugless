using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRIS.Web.Data;
using PRIS.Web.Mappings;
using PRIS.Web.Models;
using PRIS.Core.Library.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PRIS.Web.Controllers
{
    public class ProgramsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Programs
        public async Task<IActionResult> Index()
        {
            ProgramViewModel programViewModel = new ProgramViewModel();
            var programResult = await _context.Programs.ToListAsync();
            programViewModel.ProgramNames = programResult;
            ////List<ProgramViewModel> programViewModels = new List<ProgramViewModel>();

            var cityResult = await _context.Cities.ToListAsync();
            programViewModel.CityNames = cityResult;

            return View(programViewModel);
        }

        // GET: Programs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Programs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProgramNames")] ProgramViewModel programViewModel)
        {
            if (ModelState.IsValid)
            {

                var result = ProgramMappings.ToProgramEntity(programViewModel);
                _context.Programs.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programViewModel);
        }
        //Create new city
        public IActionResult CreateNewCity()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewCity([Bind("CityNames")] ProgramViewModel programViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = ProgramMappings.ToCityEntity(programViewModel);
                _context.Cities.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(programViewModel);
        }

        //GET: Programs/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var program = await _context.Programs
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (program == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(ProgramMappings.ToProgramViewModel(program));
        //}
        //delete city
        //public async Task<IActionResult> DeleteCity(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var city = await _context.Cities
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (city == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(ProgramMappings.ToCityViewModel(city));
        //}

        // POST: Programs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var program = await _context.Programs.FindAsync(id);
        //    _context.Programs.Remove(program);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ProgramExists(int id)
        //{
        //    return _context.Programs.Any(e => e.Id == id);
        //}
        ////delete city comfirmed
        //[HttpPost, ActionName("DeleteCity")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmedCity(int id)
        //{
        //    var city = await _context.Cities.FindAsync(id);
        //    _context.Cities.Remove(city);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CityExists(int id)
        //{
        //    return _context.Cities.Any(e => e.Id == id);
        //}

    }
}
