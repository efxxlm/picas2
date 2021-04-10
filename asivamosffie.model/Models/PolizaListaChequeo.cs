using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class PolizaListaChequeo
    {
        public int PolizaListaChequeoId { get; set; }
        public int ContratoPolizaId { get; set; }
        public bool? CumpleDatosAseguradoBeneficiario { get; set; }
        public bool? CumpleDatosBeneficiarioGarantiaBancaria { get; set; }
        public bool? CumpleDatosTomadorAfianzado { get; set; }
        public bool? TieneReciboPagoDatosRequeridos { get; set; }
        public bool? TieneCondicionesGeneralesPoliza { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ContratoPoliza ContratoPoliza { get; set; }
    }
}
