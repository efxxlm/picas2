using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class TemaCompromisoSeguimiento
    {
        public int TemaCompromisoSeguimientoId { get; set; }
        public int TemaCompromisoId { get; set; }
        public string Tarea { get; set; }
        public string EstadoCodigo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public virtual TemaCompromiso TemaCompromiso { get; set; }
    }
}
