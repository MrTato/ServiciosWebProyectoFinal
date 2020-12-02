using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmServiciosV2.Models
{
    public class TipoServicioCLS
    {
        public int IdTipoServicio { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}