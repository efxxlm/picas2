using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VListCompromisosComiteTecnico
    {
        public DateTime? FechaComite { get; set; }
        public string NumeroComite { get; set; }
        public string Compromiso { get; set; }
        public string EstadoCodigo { get; set; }
        public int TipoSolicitud { get; set; }
        public DateTime? FechaCumplimiento { get; set; }
        public int? CompromisoId { get; set; }
        public int UsuarioId { get; set; }
    }
}
