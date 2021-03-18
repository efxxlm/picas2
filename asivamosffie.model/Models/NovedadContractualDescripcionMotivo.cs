using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class NovedadContractualDescripcionMotivo
    {
        public int NovedadContractualDescripcionMotivoId { get; set; }
        public int? NovedadContractualDescripcionId { get; set; }
        public string MotivoNovedadCodigo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual NovedadContractualDescripcion NovedadContractualDescripcion { get; set; }
    }
}
