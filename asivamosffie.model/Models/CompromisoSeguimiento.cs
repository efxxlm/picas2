using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CompromisoSeguimiento
    {
        public int CompromisoSeguimientoId { get; set; }
        public int? SesionComiteTecnicoCompromisoId { get; set; }
        public int? TemaCompromisoId { get; set; }
        public string DescripcionSeguimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public int? SesionSolicitudCompromisoId { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public int? SesionParticipanteId { get; set; }
        public string EstadoCompromisoCodigo { get; set; }

        public virtual SesionComiteTecnicoCompromiso SesionComiteTecnicoCompromiso { get; set; }
        public virtual Usuario SesionParticipante { get; set; }
        public virtual SesionSolicitudCompromiso SesionSolicitudCompromiso { get; set; }
        public virtual TemaCompromiso TemaCompromiso { get; set; }
    }
}
