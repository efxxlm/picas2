using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VigenciaAporte
    {
        public int VigenciaAporteId { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public string TipoVigenciaCodigo { get; set; }
        public decimal? ValorAporte { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
    }
}
