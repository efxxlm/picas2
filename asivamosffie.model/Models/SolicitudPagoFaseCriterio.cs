using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoFaseCriterio
    {
        public SolicitudPagoFaseCriterio()
        {
            SolicitudPagoFaseCriterioProyecto = new HashSet<SolicitudPagoFaseCriterioProyecto>();
        }

        public int SolicitudPagoFaseCriterioId { get; set; }
        public string TipoPagoCodigo { get; set; }
        public string ConceptoPagoCriterio { get; set; }
        public decimal? ValorFacturado { get; set; }
        public int SolicitudPagoFaseId { get; set; }

        public virtual SolicitudPagoFase SolicitudPagoFase { get; set; }
        public virtual ICollection<SolicitudPagoFaseCriterioProyecto> SolicitudPagoFaseCriterioProyecto { get; set; }
    }
}
