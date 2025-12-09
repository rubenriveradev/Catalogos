using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs
{
    public class EnvioModelDTO
    {
        public int IdDespacho { get; set; }
        public decimal PercentageCommission { get; set; }
        public string FechaDespacho { get; set; }
        public int IdEstado { get; set; }
        public int UsserId { get; set; }
        public string FechaCreacion { get; set; }
    }
}
