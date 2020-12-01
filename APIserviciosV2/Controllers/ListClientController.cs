using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIserviciosV2.Models;

namespace APIserviciosV2.Controllers
{
    [Authorize]
    public class ListClientController : ApiController
    {
        private BDServicioV2EntitiesSTPR db = new BDServicioV2EntitiesSTPR();

        // GET: api/ListClient
        public IQueryable<SP_ListClient_Result> GetListClient()
        {
            return db.SP_ListClient_Result().AsQueryable();
        }
    }
}
