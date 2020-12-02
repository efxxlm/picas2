using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class AportanteFuenteFinanciacion
    {
        public int AportanteFuenteFinanciacionId { get; set; }
        public int ProyectoAdministrativoAportanteId { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public bool Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioEdicion { get; set; }
        public DateTime? FechaEdicion { get; set; }
        public long? ValorFuente { get; set; }

        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
        public virtual ProyectoAdministrativoAportante ProyectoAdministrativoAportante { get; set; }
    }
}
