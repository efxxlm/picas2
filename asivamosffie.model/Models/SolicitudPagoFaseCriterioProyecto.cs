using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoFaseCriterioProyecto
    {
        public int SolicitudPagoFaseCriterioProyectoId { get; set; }
        public int SolicitudPagoFaseCriterioId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public decimal? ValorFacturado { get; set; }

        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual SolicitudPagoFaseCriterio SolicitudPagoFaseCriterio { get; set; }
    }
}
