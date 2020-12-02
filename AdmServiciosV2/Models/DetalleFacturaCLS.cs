using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmServiciosV2.Models
{
    public class DetalleFacturaCLS
    {
        public int IdDetalleFactura { get; set; }
        public int IdFactura { get; set; }
        public int IdServicio { get; set; }
        public int Cantidad { get; set; }
        public Nullable<bool> Entregado { get; set; }
    }
}