using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ProcesoSeleccionObservacion
    {
        public int ProcesoSeleccionObservacionId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
    }
}
