using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ArchivoCargue
    {
        public ArchivoCargue()
        {
            TemporalProyecto = new HashSet<TemporalProyecto>();
        }

        public int ArchivoCargueId { get; set; }
        public int? OrigenId { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string Tamano { get; set; }
        public int? CantidadRegistros { get; set; }
        public int? CantidadRegistrosValidos { get; set; }
        public int? CantidadRegistrosInvalidos { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<TemporalProyecto> TemporalProyecto { get; set; }
        public virtual ICollection<TempOrdenLegibilidad> TempOrdenLegibilidad { get; set; }
    }
}
