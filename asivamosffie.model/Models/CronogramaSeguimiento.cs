using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CronogramaSeguimiento
    {
        public int CronogramaSeguimientoId { get; set; }
        public int ProcesoSeleccionCronogramaId { get; set; }
        public string EstadoActividadInicialCodigo { get; set; }
        public string EstadoActividadFinalCodigo { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ProcesoSeleccionCronograma ProcesoSeleccionCronograma { get; set; }
    }
}
