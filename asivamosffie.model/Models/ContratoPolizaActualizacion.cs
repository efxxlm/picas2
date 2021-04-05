using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratoPolizaActualizacion
    {
        public ContratoPolizaActualizacion()
        {
            ContratoPolizaActualizacionListaChequeo = new HashSet<ContratoPolizaActualizacionListaChequeo>();
            ContratoPolizaActualizacionRevisionAprobacionObservacion = new HashSet<ContratoPolizaActualizacionRevisionAprobacionObservacion>();
            ContratoPolizaActualizacionSeguro = new HashSet<ContratoPolizaActualizacionSeguro>();
        }

        public int ContratoPolizaActualizacionId { get; set; }
        public int? ContratoPolizaId { get; set; }
        public string RazonActualizacionCodigo { get; set; }
        public string TipoActualizacionCodigo { get; set; }
        public DateTime? FechaExpedicionActualizacionPoliza { get; set; }
        public string ObservacionEspecifica { get; set; }
        public bool? RegistroCompletoObservacionEspecifica { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public string NumeroActualizacion { get; set; }
        public string EstadoActualizacion { get; set; }

        public virtual ContratoPoliza ContratoPoliza { get; set; }
        public virtual ICollection<ContratoPolizaActualizacionListaChequeo> ContratoPolizaActualizacionListaChequeo { get; set; }
        public virtual ICollection<ContratoPolizaActualizacionRevisionAprobacionObservacion> ContratoPolizaActualizacionRevisionAprobacionObservacion { get; set; }
        public virtual ICollection<ContratoPolizaActualizacionSeguro> ContratoPolizaActualizacionSeguro { get; set; }
    }
}
