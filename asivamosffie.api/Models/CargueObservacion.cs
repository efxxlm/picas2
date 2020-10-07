using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class CargueObservacion
    {
        public int CargueObservacionId { get; set; }
        public int ConstruccionCargueId { get; set; }
        public string TipoObservacionCodigo { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ConstruccionCargue ConstruccionCargue { get; set; }
    }
}
