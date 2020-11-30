using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaProcesoSeleccionIntegrante
    {
        public int ProcesoSeleccionIntegranteId { get; set; }
        
        public string NombreIntegrante { get; set; }
        public string TipoProcesoSeleccion { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public int? PorcentajeParticipacion { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
