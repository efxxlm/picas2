using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class TempProgramacion
    {
        public int TempProgramacionId { get; set; }
        public int ArchivoCargueId { get; set; }
        public bool EstaValidado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string TipoActividadCodigo { get; set; }
        public string Actividad { get; set; }
        public bool EsRutaCritica { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Duracion { get; set; }
        public int ContratoConstruccionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ArchivoCargue ArchivoCargue { get; set; }
        public virtual ContratoConstruccion ContratoConstruccion { get; set; }
    }
}
