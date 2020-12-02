using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmServiciosV2.Models
{
    public class DireccionCLS
    {
        public int IdDireccion { get; set; }
        public int IdUbicacion { get; set; }
        public int IdCliente { get; set; }
        public string Direccion1 { get; set; }
    }
}