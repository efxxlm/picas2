using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ComponenteUsoNovedad
    {
        public int ComponenteUsoNovedadId { get; set; }
        public int ComponenteAportanteNovedadId { get; set; }
        public string TipoUsoCodigo { get; set; }
        public decimal ValorUso { get; set; }
        public bool? Eliminado { get; set; }
        public int? Activo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ComponenteAportanteNovedad ComponenteAportanteNovedad { get; set; }
    }
}
