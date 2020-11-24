using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdmServiciosV2.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public int ExpirresIn { get; set; }
    }
}