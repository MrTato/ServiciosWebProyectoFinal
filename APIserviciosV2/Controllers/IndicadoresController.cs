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
    public class IndicadoresController : ApiController
    {
        private BDServicioV2EntitiesSTPR db = new BDServicioV2EntitiesSTPR();

        //GET: api/Indicadores
        [ResponseType(typeof(SP_Indicadores_Result))]
        public IHttpActionResult GetIndicadores()
        {
            SP_Indicadores_Result indicaroes = db.SP_Indicadores_Result().FirstOrDefault();

            return Ok(indicaroes);
        }
    }
}
