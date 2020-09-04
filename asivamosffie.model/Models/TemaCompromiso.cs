using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class TemaCompromiso
    {
        public TemaCompromiso()
        {
            CompromisoSeguimiento = new HashSet<CompromisoSeguimiento>();
        }

        public int TemaCompromisoId { get; set; }
        public int SesionTemaId { get; set; }
        public string Tarea { get; set; }
        public int Responsable { get; set; }
        public DateTime FechaCumplimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public string EstadoCodigo { get; set; }

        public virtual SesionParticipante ResponsableNavigation { get; set; }
        public virtual SesionComiteTema SesionTema { get; set; }
        public virtual ICollection<CompromisoSeguimiento> CompromisoSeguimiento { get; set; }
    }
}
