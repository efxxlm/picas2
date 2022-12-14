using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValorFlujoInversionXseguimientoSemanal
    {
        public int SeguimientoSemanalId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public decimal? Valor { get; set; }
        public int NumeroMes { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal? ValorEjecutado { get; set; }
    }
}
