using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ComiteTecnicoProyecto
    {
        public int ComiteTecnicoProyectoId { get; set; }
        public int ComiteTecnicoId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ComiteTecnico ComiteTecnico { get; set; }
        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
    }
}
