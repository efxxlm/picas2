using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ComponenteUsoNovedadHistorico
    {
        public int ComponenteUsoNovedadHistoricoId { get; set; }
        public int ComponenteUsoNovedadId { get; set; }
        public decimal ValorUso { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Liberado { get; set; }

        public virtual ComponenteFuenteNovedad ComponenteUsoNovedad { get; set; }
    }
}
