using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ConstruccionPerfilNumeroRadicado
    {
        public int ConstruccionPerfilNumeroRadicadoId { get; set; }
        public int? ConstruccionPerfilId { get; set; }
        public string NumeroRadicado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ConstruccionPerfil ConstruccionPerfil { get; set; }
    }
}
