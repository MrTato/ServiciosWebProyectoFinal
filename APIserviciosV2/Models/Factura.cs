//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APIserviciosV2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Factura
    {
        public int IdFactura { get; set; }
        public string Numero { get; set; }
        public int IdCliente { get; set; }
        public int IdDireccion { get; set; }
        public System.DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
