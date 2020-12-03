using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ProgramacionPersonalContrato
    {
        public int ProgramacionPersonalContratoId { get; set; }
        public int ContratoId { get; set; }
        public int NumeroSemana { get; set; }
        public int? CantidadPersonal { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Eliminado { get; set; }
        public int ProyectoId { get; set; }

        public virtual Contrato Contrato { get; set; }
        public virtual Proyecto Proyecto { get; set; }
    }
}
