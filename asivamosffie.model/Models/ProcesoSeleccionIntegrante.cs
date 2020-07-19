using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionIntegrante
    {
        public int ProcesoSeleccionIntegranteId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public int PorcentajeParticipacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
    }
}
