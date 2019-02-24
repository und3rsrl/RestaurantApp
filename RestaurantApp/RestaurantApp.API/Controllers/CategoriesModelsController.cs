using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RestaurantApp.API.Models;

namespace RestaurantApp.API.Controllers
{
    public class CategoriesModelsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CategoriesModels
        public IQueryable<CategoriesModel> GetCategoriesModels()
        {
            return db.CategoriesModels;
        }

        // GET: api/CategoriesModels/5
        [ResponseType(typeof(CategoriesModel))]
        public IHttpActionResult GetCategoriesModel(int id)
        {
            CategoriesModel categoriesModel = db.CategoriesModels.Find(id);
            if (categoriesModel == null)
            {
                return NotFound();
            }

            return Ok(categoriesModel);
        }

        // PUT: api/CategoriesModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategoriesModel(int id, CategoriesModel categoriesModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoriesModel.Identifier)
            {
                return BadRequest();
            }

            db.Entry(categoriesModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriesModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CategoriesModels
        [ResponseType(typeof(CategoriesModel))]
        public IHttpActionResult PostCategoriesModel(CategoriesModel categoriesModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CategoriesModels.Add(categoriesModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = categoriesModel.Identifier }, categoriesModel);
        }

        // DELETE: api/CategoriesModels/5
        [ResponseType(typeof(CategoriesModel))]
        public IHttpActionResult DeleteCategoriesModel(int id)
        {
            CategoriesModel categoriesModel = db.CategoriesModels.Find(id);
            if (categoriesModel == null)
            {
                return NotFound();
            }

            db.CategoriesModels.Remove(categoriesModel);
            db.SaveChanges();

            return Ok(categoriesModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoriesModelExists(int id)
        {
            return db.CategoriesModels.Count(e => e.Identifier == id) > 0;
        }
    }
}