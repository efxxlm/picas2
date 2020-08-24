using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionComiteTecnicoCompromiso
    {
        public SesionComiteTecnicoCompromiso()
        {
            CompromisoSeguimiento = new HashSet<CompromisoSeguimiento>();
        }

        public int SesionComiteTecnicoCompromisoId { get; set; }
        public int SesionComiteTecnicoId { get; set; }
        public string Tarea { get; set; }
        public string Responsable { get; set; }
        public DateTime FechaCumplimiento { get; set; }
        public string EstadoCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual SesionComiteTecnico SesionComiteTecnico { get; set; }
        public virtual ICollection<CompromisoSeguimiento> CompromisoSeguimiento { get; set; }
    }
}
