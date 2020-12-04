using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SeguridadSaludCausaAccidente
    {
        public int SeguridadSaludCausaAccidentesId { get; set; }
        public int SeguimientoSemanalGestionObraSeguridadSaludId { get; set; }
        public byte[] CausaAccidenteCodigo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual SeguimientoSemanalGestionObraSeguridadSalud SeguimientoSemanalGestionObraSeguridadSalud { get; set; }
    }
}
