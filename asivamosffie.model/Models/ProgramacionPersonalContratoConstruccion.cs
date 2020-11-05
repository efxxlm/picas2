using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProgramacionPersonalContratoConstruccion
    {
        public int ProgramacionPersonalContratoConstruccionId { get; set; }
        public int ContratoConstruccionId { get; set; }
        public int NumeroSemana { get; set; }
        public int? CantidadPersonal { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual ContratoConstruccion ContratoConstruccion { get; set; }
    }
}
