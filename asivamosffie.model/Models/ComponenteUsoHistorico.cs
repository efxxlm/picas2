using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ComponenteUsoHistorico
    {
        public int ComponenteUsoHistoricoId { get; set; }
        public int ComponenteUsoId { get; set; }
        public decimal ValorUso { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ComponenteUso ComponenteUso { get; set; }
    }
}
