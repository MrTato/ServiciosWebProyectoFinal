using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmServiciosV2.Models
{
    public class DetailInvoiceCLS
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public int IdFactura { get; set; }
        public decimal Total { get; set; }
        public System.DateTime Fecha { get; set; }
        public int Cantidad { get; set; }
        public string NombreServicio { get; set; }
        public Nullable<decimal> CostoBase { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }
    }
}