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
        public string Miembro { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual SesionComiteTecnicoCompromiso SesionComiteTecnicoCompromiso { get; set; }
        public virtual TemaCompromiso TemaCompromiso { get; set; }
    }
}
