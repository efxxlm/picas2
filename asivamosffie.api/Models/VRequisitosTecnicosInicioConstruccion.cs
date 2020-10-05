using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class VRequisitosTecnicosInicioConstruccion
    {
        public int ContratoConstruccionId { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public string NumeroContrato { get; set; }
        public int CantidadProyectosAsociados { get; set; }
        public int CantidadProyectosRequisitosAprobados { get; set; }
        public int CantidadProyectosRequisitosPendientes { get; set; }
        public string EstadoRequisitos { get; set; }
    }
}
