using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSeguimientoSemanalXflujoInversion
    {
        public int SeguimientoSemanalId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public decimal? Valor { get; set; }
        public int Numero { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
