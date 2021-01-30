using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class NovedadContractualClausula
    {
        public int NovedadContractualClausulaId { get; set; }
        public int? NovedadContractualId { get; set; }
        public string ClausulaAmodificar { get; set; }
        public string AjusteSolicitadoAclausula { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual NovedadContractual NovedadContractual { get; set; }
    }
}
