using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSetHistDefensaJudicialContratacionProyecto
    {
        public int DefensaJudicialContratacionProyectoId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public bool EsCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int? DefensaJudicialId { get; set; }
    }
}
