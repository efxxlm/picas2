using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratoPoliza
    {
        public ContratoPoliza()
        {
            ContratoPolizaActualizacion = new HashSet<ContratoPolizaActualizacion>();
            PolizaGarantia = new HashSet<PolizaGarantia>();
            PolizaGarantiaActualizacion = new HashSet<PolizaGarantiaActualizacion>();
            PolizaListaChequeo = new HashSet<PolizaListaChequeo>();
            PolizaObservacion = new HashSet<PolizaObservacion>();
        }

        public int ContratoId { get; set; }
        public string DescripcionModificacion { get; set; }
        public string NombreAseguradora { get; set; }
        public string NumeroPoliza { get; set; }
        public string NumeroCertificado { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public bool? IncluyeCondicionesGenerales { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string EstadoPolizaCodigo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public int ContratoPolizaId { get; set; }

        public virtual Contrato Contrato { get; set; }
        public virtual ICollection<ContratoPolizaActualizacion> ContratoPolizaActualizacion { get; set; }
        public virtual ICollection<PolizaGarantia> PolizaGarantia { get; set; }
        public virtual ICollection<PolizaGarantiaActualizacion> PolizaGarantiaActualizacion { get; set; }
        public virtual ICollection<PolizaListaChequeo> PolizaListaChequeo { get; set; }
        public virtual ICollection<PolizaObservacion> PolizaObservacion { get; set; }
    }
}
