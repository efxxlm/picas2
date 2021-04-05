using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratoPolizaActualizacionSeguro
    {
        public int ContratoPolizaActualizacionSeguroId { get; set; }
        public int ContratoPolizaActualizacionId { get; set; }
        public string TipoSeguroCodigo { get; set; }
        public bool? TieneFechaSeguro { get; set; }
        public DateTime? FechaSeguro { get; set; }
        public bool? TieneFechaVigenciaAmparo { get; set; }
        public DateTime? FechaVigenciaAmparo { get; set; }
        public bool? TieneFechaValorAmparo { get; set; }
        public bool? RegistroCompletoActualizacion { get; set; }
        public bool? RegistroCompletoSeguro { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public decimal? ValorAmparo { get; set; }

        public virtual ContratoPolizaActualizacion ContratoPolizaActualizacion { get; set; }
    }
}
