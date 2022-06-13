using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaContratoDefensaJudicial
    {
        public int ContratoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string TipoAccion { get; set; }
        public string Legitimacion { get; set; }
        public string NumeroProceso { get; set; }
        public string EstadoProceso { get; set; }
        public string UrlSoporteProceso { get; set; }
    }
}
