using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmServiciosV2.Models
{
    public class IndicadoresCLS
    {
        public Nullable<int> TotalFacturas { get; set; }
        public Nullable<int> ServiciosFacturados { get; set; }
        public Nullable<int> TotalServicios { get; set; }
        public Nullable<int> TotalClientes { get; set; }
    }
}