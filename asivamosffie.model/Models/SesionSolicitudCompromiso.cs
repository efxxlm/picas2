using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionSolicitudCompromiso
    {
        public SesionSolicitudCompromiso()
        {
            CompromisoSeguimiento = new HashSet<CompromisoSeguimiento>();
        }

        public int SesionSolicitudCompromisoId { get; set; }
        public int SesionComiteSolicitudId { get; set; }
        public string Tarea { get; set; }
        public int ResponsableSesionParticipanteId { get; set; }
        public DateTime FechaCumplimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public string EstadoCodigo { get; set; }

        public virtual SesionParticipante ResponsableSesionParticipante { get; set; }
        public virtual SesionComiteSolicitud SesionComiteSolicitud { get; set; }
        public virtual ICollection<CompromisoSeguimiento> CompromisoSeguimiento { get; set; }
    }
}
