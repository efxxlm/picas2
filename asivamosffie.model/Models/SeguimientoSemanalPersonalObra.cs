using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalPersonalObra
    {
        public int SeguimientoSemanalPersonalObraId { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public int? CantidadPersonal { get; set; }
        public bool Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
         

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
    }
}
