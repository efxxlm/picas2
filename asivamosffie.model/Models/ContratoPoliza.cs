using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratoPoliza
    {
        public ContratoPoliza()
        {
            PolizaGarantia = new HashSet<PolizaGarantia>();
            PolizaObservacion = new HashSet<PolizaObservacion>();
        }

        public int ContratoId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string TipoModificacionCodigo { get; set; }
        public string DescripcionModificacion { get; set; }
        public string NombreAseguradora { get; set; }
        public string NumeroPoliza { get; set; }
        public string NumeroCertificado { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public DateTime? Vigencia { get; set; }
        public string VigenciaAmparo { get; set; }
        public decimal? ValorAmparo { get; set; }
        public string Observaciones { get; set; }
        public bool? CumpleDatosAsegurado { get; set; }
        public bool? CumpleDatosBeneficiario { get; set; }
        public bool? CumpleDatosTomador { get; set; }
        public bool? IncluyeReciboPago { get; set; }
        public bool? IncluyeCondicionesGenerales { get; set; }
        public string ObservacionesRevisionGeneral { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string ResponsableAprobacion { get; set; }
        public bool? Estado { get; set; }
        public string EstadoPolizaCodigo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public int ContratoPolizaId { get; set; }

        public virtual Contrato Contrato { get; set; }
        public virtual ICollection<PolizaGarantia> PolizaGarantia { get; set; }
        public virtual ICollection<PolizaObservacion> PolizaObservacion { get; set; }
    }
}
