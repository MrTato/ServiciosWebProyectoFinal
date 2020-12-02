using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmServiciosV2.Models
{
    public class ListClientCLS
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int IdFactura { get; set; }
        public string Numero { get; set; }
        public decimal Total { get; set; }
        public string Direccion { get; set; }
    }
}