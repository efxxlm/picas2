using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ProcesoSeleccionCronogramaMonitoreo
    {
        public ProcesoSeleccionCronogramaMonitoreo()
        {
            SesionSolicitudObservacionActualizacionCronograma = new HashSet<SesionSolicitudObservacionActualizacionCronograma>();
        }

        public int ProcesoSeleccionCronogramaMonitoreoId { get; set; }
        public int? NumeroActividad { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaMaxima { get; set; }
        public string EstadoActividadCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int? ProcesoSeleccionMonitoreoId { get; set; }
        public int? ProcesoSeleccionCronogramaId { get; set; }
        public string EtapaActualProcesoCodigo { get; set; }

        public virtual ProcesoSeleccionMonitoreo ProcesoSeleccionMonitoreo { get; set; }
        public virtual ICollection<SesionSolicitudObservacionActualizacionCronograma> SesionSolicitudObservacionActualizacionCronograma { get; set; }
    }
}
