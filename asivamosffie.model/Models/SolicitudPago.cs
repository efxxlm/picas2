using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPago
    {
        public SolicitudPago()
        {
            SolicitudPagoCargarFormaPago = new HashSet<SolicitudPagoCargarFormaPago>();
            SolicitudPagoExpensas = new HashSet<SolicitudPagoExpensas>();
            SolicitudPagoListaChequeo = new HashSet<SolicitudPagoListaChequeo>();
            SolicitudPagoObservacion = new HashSet<SolicitudPagoObservacion>();
            SolicitudPagoOtrosCostosServicios = new HashSet<SolicitudPagoOtrosCostosServicios>();
            SolicitudPagoRegistrarSolicitudPago = new HashSet<SolicitudPagoRegistrarSolicitudPago>();
            SolicitudPagoSoporteSolicitud = new HashSet<SolicitudPagoSoporteSolicitud>();
        }

        public int SolicitudPagoId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public int? ContratoId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoCodigo { get; set; }
        public string NumeroSolicitud { get; set; }
        public bool? TieneObservacion { get; set; }
        public bool? RegistroCompletoSupervisor { get; set; }
        public bool? RegistroCompletoCoordinador { get; set; }
        public int? OrdenGiroId { get; set; }
        public DateTime? FechaAprobacionFinanciera { get; set; }
        public DateTime? FechaRadicacionSacContratista { get; set; }
        public string NumeroRadicacionSacContratista { get; set; }
        public bool? RegistroCompletoVerificar { get; set; }
        public bool? RegistroCompletoAutorizar { get; set; }
        public bool? TieneSubsanacion { get; set; }
        public bool? RegistroCompletoVerificacionFinanciera { get; set; }
        public bool? RegistroCompletoValidacionFinanciera { get; set; }
        public DateTime? FechaRegistroCompletoVerificacionFinanciera { get; set; }
        public DateTime? FechaRegistroCompletoValidacionFinanciera { get; set; }
        public DateTime? FechaRadicacionSacFinanciera { get; set; }
        public string NumeroRadicacionSacFinanciera { get; set; }
        public string UrlSoporteFinanciera { get; set; }
        public bool TieneNoCumpleListaChequeo { get; set; }

        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual Contrato Contrato { get; set; }
        public virtual OrdenGiro OrdenGiro { get; set; }
        public virtual ICollection<SolicitudPagoCargarFormaPago> SolicitudPagoCargarFormaPago { get; set; }
        public virtual ICollection<SolicitudPagoExpensas> SolicitudPagoExpensas { get; set; }
        public virtual ICollection<SolicitudPagoListaChequeo> SolicitudPagoListaChequeo { get; set; }
        public virtual ICollection<SolicitudPagoObservacion> SolicitudPagoObservacion { get; set; }
        public virtual ICollection<SolicitudPagoOtrosCostosServicios> SolicitudPagoOtrosCostosServicios { get; set; }
        public virtual ICollection<SolicitudPagoRegistrarSolicitudPago> SolicitudPagoRegistrarSolicitudPago { get; set; }
        public virtual ICollection<SolicitudPagoSoporteSolicitud> SolicitudPagoSoporteSolicitud { get; set; }
    }
}
