using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmServiciosV2.Models
{
    public class ServicioCLS
    {
        public int IdServicio { get; set; }
        public int IdTipoServicio { get; set; }
        public string Nombre { get; set; }
        public Nullable<decimal> CostoBase { get; set; }
    }
}