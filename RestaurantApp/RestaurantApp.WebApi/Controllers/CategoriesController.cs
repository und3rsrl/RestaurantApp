using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.WebApi.Entities;
using RestaurantApp.WebApi.Models;

namespace RestaurantApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public IEnumerable<Categorie> GetCategorie()
        {
            return _context.Categories;
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategorie([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categorie = await _context.Categories.FindAsync(id);

            if (categorie == null)
            {
                return NotFound();
            }

            return Ok(categorie);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategorie([FromRoute] int id, [FromBody] Categorie categorie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categorie.Id)
            {
                return BadRequest();
            }

            _context.Entry(categorie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> PostCategorie([FromBody] Categorie categorie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Categories.Where(x => x.Name.Equals(categorie.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() != null)
                return BadRequest("Categorie already exists.");

            _context.Categories.Add(categorie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategorie", new { id = categorie.Id }, categorie);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorie([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categorie = await _context.Categories.FindAsync(id);
            if (categorie == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categorie);
            await _context.SaveChangesAsync();

            return Ok(categorie);
        }

        private bool CategorieExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}