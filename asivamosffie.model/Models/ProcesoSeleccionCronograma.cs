using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionCronograma
    {
        public ProcesoSeleccionCronograma()
        {
            CronogramaSeguimiento = new HashSet<CronogramaSeguimiento>();
        }

        public int ProcesoSeleccionCronogramaId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public int NumeroActividad { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaMaxima { get; set; }
        public string EstadoActividadCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
        public virtual ICollection<CronogramaSeguimiento> CronogramaSeguimiento { get; set; }
    }
}
