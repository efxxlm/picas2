using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFechasValidacionAjusteProgramacion
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int ProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public decimal? AvanceFisicoSemanal { get; set; }
    }
}
