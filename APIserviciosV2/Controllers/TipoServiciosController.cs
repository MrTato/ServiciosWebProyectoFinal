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
    public class TipoServiciosController : ApiController
    {
        private BDServicioV2EntitiesTipoServicio db = new BDServicioV2EntitiesTipoServicio();

        // GET: api/TipoServicios
        public IQueryable<TipoServicio> GetTipoServicio()
        {
            return db.TipoServicio;
        }

        // GET: api/TipoServicios/5
        [ResponseType(typeof(TipoServicio))]
        public IHttpActionResult GetTipoServicio(int id)
        {
            TipoServicio tipoServicio = db.TipoServicio.Find(id);
            if (tipoServicio == null)
            {
                return NotFound();
            }

            return Ok(tipoServicio);
        }

        // PUT: api/TipoServicios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoServicio(int id, TipoServicio tipoServicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoServicio.IdTipoServicio)
            {
                return BadRequest();
            }

            db.Entry(tipoServicio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoServicioExists(id))
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

        // POST: api/TipoServicios
        [ResponseType(typeof(TipoServicio))]
        public IHttpActionResult PostTipoServicio(TipoServicio tipoServicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TipoServicio.Add(tipoServicio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoServicio.IdTipoServicio }, tipoServicio);
        }

        // DELETE: api/TipoServicios/5
        [ResponseType(typeof(TipoServicio))]
        public IHttpActionResult DeleteTipoServicio(int id)
        {
            TipoServicio tipoServicio = db.TipoServicio.Find(id);
            if (tipoServicio == null)
            {
                return NotFound();
            }

            db.TipoServicio.Remove(tipoServicio);
            db.SaveChanges();

            return Ok(tipoServicio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoServicioExists(int id)
        {
            return db.TipoServicio.Count(e => e.IdTipoServicio == id) > 0;
        }
    }
}