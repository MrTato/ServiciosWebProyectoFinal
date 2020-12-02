using APIserviciosV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace APIserviciosV2.Controllers
{
    [Authorize]
    public class DetailInvoiceController : ApiController
    {
        private BDServicioV2EntitiesSTPR db = new BDServicioV2EntitiesSTPR();

        [ResponseType(typeof(SP_DetailInvoice_Result))]
        public IHttpActionResult GetDetailInvoice(int id)
        {
            SP_DetailInvoice_Result di = db.SP_DetailInvoice_Result(id).FirstOrDefault();
            if(di == null)
            {
                return NotFound();
            }

            return Ok(di);
        }
    }
}
