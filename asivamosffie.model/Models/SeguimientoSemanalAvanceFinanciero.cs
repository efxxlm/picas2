using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalAvanceFinanciero
    {
        public int SeguimientoSemanalAvanceFinancieroId { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public bool? RequiereObservacion { get; set; }
        public string Observacion { get; set; }
        public bool? GenerarAlerta { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
    }
}
