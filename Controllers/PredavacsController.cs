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
using FakeCET.Models;

namespace FakeCET.Controllers
{
    public class PredavacsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Predavacs
        [Route("api/Predavacs")]
        public IQueryable<Predavac> GetPredavaci()
        {
            return db.Predavaci;
        }

        // GET: api/Predavacs/5
        [Route("api/Predavacs/{id:int}")]
        [ResponseType(typeof(Predavac))]
        public IHttpActionResult GetPredavac(int id)
        {
            Predavac predavac = db.Predavaci.Find(id);
            if (predavac == null)
            {
                return NotFound();
            }

            return Ok(predavac);
        }

        // PUT: api/Predavacs/5
        [Route("api/Predavacs/{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPredavac(int id, Predavac predavac)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != predavac.PredavacID)
            {
                return BadRequest();
            }

            db.Entry(predavac).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PredavacExists(id))
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

        // POST: api/Predavacs
        [Route("api/Predavacs")]
        [ResponseType(typeof(Predavac))]
        public IHttpActionResult PostPredavac(Predavac predavac)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Predavaci.Add(predavac);
            db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + predavac.PredavacID), predavac);
        }

        // DELETE: api/Predavacs/5
        [Route("api/Predavacs/{id:int}")]
        [ResponseType(typeof(Predavac))]
        public IHttpActionResult DeletePredavac(int id)
        {
            Predavac predavac = db.Predavaci.Find(id);
            if (predavac == null)
            {
                return NotFound();
            }

            db.Predavaci.Remove(predavac);
            db.SaveChanges();

            return Ok(predavac);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PredavacExists(int id)
        {
            return db.Predavaci.Count(e => e.PredavacID == id) > 0;
        }
    }
}