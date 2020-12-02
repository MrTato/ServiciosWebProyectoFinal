using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmServiciosV2.Models
{
    public class FacturaCLS
    {
        public int IdFactura { get; set; }
        public string Numero { get; set; }
        public int IdCliente { get; set; }
        public int IdDireccion { get; set; }
        public System.DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}