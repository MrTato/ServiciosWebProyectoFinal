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
using APIserviciosV2.Models;

namespace APIserviciosV2.Controllers
{
    [Authorize]
    public class UbicacionsController : ApiController
    {
        private BDServicioV2EntitiesUbicacion db = new BDServicioV2EntitiesUbicacion();

        // GET: api/Ubicacions
        public IQueryable<Ubicacion> GetUbicacions()
        {
            return db.Ubicacions;
        }

        // GET: api/Ubicacions/5
        [ResponseType(typeof(Ubicacion))]
        public IHttpActionResult GetUbicacion(int id)
        {
            Ubicacion ubicacion = db.Ubicacions.Find(id);
            if (ubicacion == null)
            {
                return NotFound();
            }

            return Ok(ubicacion);
        }

        // PUT: api/Ubicacions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUbicacion(int id, Ubicacion ubicacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ubicacion.IdUbicacion)
            {
                return BadRequest();
            }

            db.Entry(ubicacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UbicacionExists(id))
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

        // POST: api/Ubicacions
        [ResponseType(typeof(Ubicacion))]
        public IHttpActionResult PostUbicacion(Ubicacion ubicacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ubicacions.Add(ubicacion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ubicacion.IdUbicacion }, ubicacion);
        }

        // DELETE: api/Ubicacions/5
        [ResponseType(typeof(Ubicacion))]
        public IHttpActionResult DeleteUbicacion(int id)
        {
            Ubicacion ubicacion = db.Ubicacions.Find(id);
            if (ubicacion == null)
            {
                return NotFound();
            }

            db.Ubicacions.Remove(ubicacion);
            db.SaveChanges();

            return Ok(ubicacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UbicacionExists(int id)
        {
            return db.Ubicacions.Count(e => e.IdUbicacion == id) > 0;
        }
    }
}