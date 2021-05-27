using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Contratacion
    {
        public Contratacion()
        {
            ContratacionObservacion = new HashSet<ContratacionObservacion>();
            ContratacionProyecto = new HashSet<ContratacionProyecto>();
            Contrato = new HashSet<Contrato>();
            DisponibilidadPresupuestal = new HashSet<DisponibilidadPresupuestal>();
            LiquidacionContratacionObservacion = new HashSet<LiquidacionContratacionObservacion>();
        }

        public int ContratacionId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string NumeroSolicitud { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
        public int? ContratistaId { get; set; }
        public bool? EsObligacionEspecial { get; set; }
        public string ConsideracionDescripcion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaEnvioDocumentacion { get; set; }
        public string Observaciones { get; set; }
        public string RutaMinuta { get; set; }
        public DateTime? FechaTramite { get; set; }
        public bool? Estado { get; set; }
        public bool? EsMultiProyecto { get; set; }
        public string TipoContratacionCodigo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public bool? RegistroCompleto1 { get; set; }
        public DateTime? FechaTramiteLiquidacion { get; set; }
        public DateTime? FechaFirmaContratista { get; set; }
        public DateTime? FechaFirmaFiduciaria { get; set; }
        public string ObservacionesLiquidacion { get; set; }
        public string UrlDocumentoLiquidacion { get; set; }
        public bool? RegistroCompletoLiquidacion { get; set; }
        public DateTime? FechaFirmaEnvioContratista { get; set; }
        public DateTime? FechaFirmaEnvioFiduciaria { get; set; }
        public string EstadoValidacionLiquidacionCodigo { get; set; }
        public DateTime? FechaValidacionLiquidacion { get; set; }
        public string EstadoAprobacionLiquidacionCodigo { get; set; }
        public DateTime? FechaAprobacionLiquidacion { get; set; }
        public string EstadoTramiteLiquidacion { get; set; }
        public bool? RegistroCompletoVerificacionLiquidacion { get; set; }
        public bool? RegistroCompletoAprobacionLiquidacion { get; set; }
        public bool? RegistroCompletoTramiteLiquidacion { get; set; }
        public DateTime? FechaTramiteLiquidacionControl { get; set; }
        public string NumeroSolicitudLiquidacion { get; set; }
        public DateTime? FechaTramiteGestionar { get; set; }
        public string ObservacionGestionar { get; set; }
        public string UrlSoporteGestionar { get; set; }
        public bool? RegistroCompletoGestionar { get; set; }

        public virtual Contratista Contratista { get; set; }
        public virtual ICollection<ContratacionObservacion> ContratacionObservacion { get; set; }
        public virtual ICollection<ContratacionProyecto> ContratacionProyecto { get; set; }
        public virtual ICollection<Contrato> Contrato { get; set; }
        public virtual ICollection<DisponibilidadPresupuestal> DisponibilidadPresupuestal { get; set; }
        public virtual ICollection<LiquidacionContratacionObservacion> LiquidacionContratacionObservacion { get; set; }
    }
}
