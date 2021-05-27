using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratacionProyecto
    {
        public ContratacionProyecto()
        {
            AjusteProgramacion = new HashSet<AjusteProgramacion>();
            ContratacionObservacion = new HashSet<ContratacionObservacion>();
            ContratacionProyectoAportante = new HashSet<ContratacionProyectoAportante>();
            DefensaJudicialContratacionProyecto = new HashSet<DefensaJudicialContratacionProyecto>();
            LiquidacionContratacionObservacion = new HashSet<LiquidacionContratacionObservacion>();
            SeguimientoDiario = new HashSet<SeguimientoDiario>();
            SeguimientoSemanal = new HashSet<SeguimientoSemanal>();
            SeguimientoSemanalTemp = new HashSet<SeguimientoSemanalTemp>();
            SesionSolicitudObservacionProyecto = new HashSet<SesionSolicitudObservacionProyecto>();
            SolicitudPago = new HashSet<SolicitudPago>();
            SolicitudPagoFaseCriterioProyecto = new HashSet<SolicitudPagoFaseCriterioProyecto>();
        }

        public int ContratacionProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int ProyectoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool Eliminado { get; set; }
        public bool? EsReasignacion { get; set; }
        public string PorcentajeAvanceObra { get; set; }
        public bool? RequiereLicencia { get; set; }
        public bool? LicenciaVigente { get; set; }
        public string NumeroLicencia { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Activo { get; set; }
        public bool? EsAvanceobra { get; set; }
        public bool? TieneMonitoreoWeb { get; set; }
        public string EstadoRequisitosVerificacionCodigo { get; set; }
        public DateTime? FechaAprobacionRequisitos { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoObraCodigo { get; set; }
        public string RutaCargaActaTerminacionContrato { get; set; }
        public decimal? AvanceFisicoSemanal { get; set; }
        public decimal? ProgramacionSemanal { get; set; }
        public string EstadoValidacionLiquidacionCodigo { get; set; }
        public DateTime? FechaValidacionLiquidacion { get; set; }
        public string EstadoAprobacionLiquidacionCodigo { get; set; }
        public DateTime? FechaAprobacionLiquidacion { get; set; }
        public string EstadoTramiteLiquidacion { get; set; }
        public bool? RegistroCompletoVerificacionLiquidacion { get; set; }
        public bool? RegistroCompletoAprobacionLiquidacion { get; set; }
        public bool? RegistroCompletoTramiteLiquidacion { get; set; }
        public DateTime? FechaTramiteLiquidacion { get; set; }
        public string NumeroSolicitudLiquidacion { get; set; }
        public DateTime? FechaTramiteGestionar { get; set; }
        public string ObservacionGestionar { get; set; }
        public string UrlSoporteGestionar { get; set; }
        public bool? RegistroCompletoGestionar { get; set; }

        public virtual Contratacion Contratacion { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<AjusteProgramacion> AjusteProgramacion { get; set; }
        public virtual ICollection<ContratacionObservacion> ContratacionObservacion { get; set; }
        public virtual ICollection<ContratacionProyectoAportante> ContratacionProyectoAportante { get; set; }
        public virtual ICollection<DefensaJudicialContratacionProyecto> DefensaJudicialContratacionProyecto { get; set; }
        public virtual ICollection<LiquidacionContratacionObservacion> LiquidacionContratacionObservacion { get; set; }
        public virtual ICollection<SeguimientoDiario> SeguimientoDiario { get; set; }
        public virtual ICollection<SeguimientoSemanal> SeguimientoSemanal { get; set; }
        public virtual ICollection<SeguimientoSemanalTemp> SeguimientoSemanalTemp { get; set; }
        public virtual ICollection<SesionSolicitudObservacionProyecto> SesionSolicitudObservacionProyecto { get; set; }
        public virtual ICollection<SolicitudPago> SolicitudPago { get; set; }
        public virtual ICollection<SolicitudPagoFaseCriterioProyecto> SolicitudPagoFaseCriterioProyecto { get; set; }
    }
}
