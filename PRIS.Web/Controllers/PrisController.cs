using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PRIS.Core.Library.Entities;
using PRIS.Web.Storage;

namespace PRIS.Web.Controllers
{
    public abstract class PrisController<TEntity, TRepository> : ControllerBase
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        private readonly TRepository _repository;
        public PrisController(TRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult<List<TEntity>>> Index()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<ActionResult<TEntity>> Get(int id)
        {
            var student = await _repository.FindByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }
        public async Task<IActionResult> Put(int id, TEntity student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }
            await _repository.UpdateAsync(student);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<TEntity>> Post(TEntity student)
        {
            await _repository.InsertAsync(student);
            return CreatedAtAction("Get", new { id = student.Id }, student);
        }
        public async Task<ActionResult<TEntity>> Delete(int? id)
        {
            await _repository.DeleteAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            return NotFound();
        }
    }
}
