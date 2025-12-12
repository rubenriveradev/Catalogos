using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class EnvioModel
    {
        public int IdDespacho { get; set; }        
        public string FechaDespacho { get; set; }
        public int IdEstado { get; set; }
        public int UsserId { get; set; }
        public string FechaCreacion { get; set; }
        public string Estado { get; set; }
    }
}
