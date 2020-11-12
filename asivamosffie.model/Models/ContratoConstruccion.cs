using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratoConstruccion
    {
        public ContratoConstruccion()
        {
            ConstruccionCargue = new HashSet<ConstruccionCargue>();
            ConstruccionObservacion = new HashSet<ConstruccionObservacion>();
            ConstruccionPerfil = new HashSet<ConstruccionPerfil>();
            FlujoInversion = new HashSet<FlujoInversion>();
            Programacion = new HashSet<Programacion>();
            TempFlujoInversion = new HashSet<TempFlujoInversion>();
            TempProgramacion = new HashSet<TempProgramacion>();
        }

        public int ContratoConstruccionId { get; set; }
        public int ContratoId { get; set; }
        public bool? EsInformeDiagnostico { get; set; }
        public string RutaInforme { get; set; }
        public decimal? CostoDirecto { get; set; }
        public decimal? Imprevistos { get; set; }
        public decimal? Utilidad { get; set; }
        public decimal? ValorTotalFaseConstruccion { get; set; }
        public bool? RequiereModificacionContractual { get; set; }
        public string NumeroSolicitudModificacion { get; set; }
        public bool? PlanLicenciaVigente { get; set; }
        public DateTime? LicenciaFechaRadicado { get; set; }
        public DateTime? LicenciaFechaAprobacion { get; set; }
        public bool? LicenciaConObservaciones { get; set; }
        public bool? PlanCambioConstructorLicencia { get; set; }
        public DateTime? CambioFechaRadicado { get; set; }
        public DateTime? CambioFechaAprobacion { get; set; }
        public bool? CambioConObservaciones { get; set; }
        public bool? PlanActaApropiacion { get; set; }
        public DateTime? ActaApropiacionFechaRadicado { get; set; }
        public DateTime? ActaApropiacionFechaAprobacion { get; set; }
        public bool? ActaApropiacionConObservaciones { get; set; }
        public bool? PlanResiduosDemolicion { get; set; }
        public DateTime? ResiduosDemolicionFechaRadicado { get; set; }
        public DateTime? ResiduosDemolicionFechaAprobacion { get; set; }
        public bool? ResiduosDemolicionConObservaciones { get; set; }
        public bool? PlanManejoTransito { get; set; }
        public DateTime? ManejoTransitoFechaRadicado { get; set; }
        public DateTime? ManejoTransitoFechaAprobacion { get; set; }
        public bool? ManejoTransitoConObservaciones1 { get; set; }
        public bool? PlanManejoAmbiental { get; set; }
        public DateTime? ManejoAmbientalFechaRadicado { get; set; }
        public DateTime? ManejoAmbientalFechaAprobacion { get; set; }
        public bool? ManejoAmbientalConObservaciones { get; set; }
        public bool? PlanAseguramientoCalidad { get; set; }
        public DateTime? AseguramientoCalidadFechaRadicado { get; set; }
        public DateTime? AseguramientoCalidadFechaAprobacion { get; set; }
        public bool? AseguramientoCalidadConObservaciones { get; set; }
        public bool? PlanProgramaSeguridad { get; set; }
        public DateTime? ProgramaSeguridadFechaRadicado { get; set; }
        public DateTime? ProgramaSeguridadFechaAprobacion { get; set; }
        public bool? ProgramaSeguridadConObservaciones { get; set; }
        public bool? PlanProgramaSalud { get; set; }
        public DateTime? ProgramaSaludFechaRadicado { get; set; }
        public DateTime? ProgramaSaludFechaAprobacion { get; set; }
        public bool? ProgramaSaludConObservaciones { get; set; }
        public int? PlanInventarioArboreo { get; set; }
        public DateTime? InventarioArboreoFechaRadicado { get; set; }
        public DateTime? InventarioArboreoFechaAprobacion { get; set; }
        public bool? InventarioArboreoConObservaciones { get; set; }
        public int? PlanAprovechamientoForestal { get; set; }
        public DateTime? AprovechamientoForestalApropiacionFechaRadicado { get; set; }
        public DateTime? AprovechamientoForestalFechaAprobacion { get; set; }
        public bool? AprovechamientoForestalConObservaciones { get; set; }
        public int? PlanManejoAguasLluvias { get; set; }
        public DateTime? ManejoAguasLluviasFechaRadicado { get; set; }
        public DateTime? ManejoAguasLluviasFechaAprobacion { get; set; }
        public bool? ManejoAguasLluviasConObservaciones { get; set; }
        public string PlanRutaSoporte { get; set; }
        public bool? ManejoAnticipoRequiere { get; set; }
        public bool? ManejoAnticipoPlanInversion { get; set; }
        public bool? ManejoAnticipoCronogramaAmortizacion { get; set; }
        public string ManejoAnticipoRutaSoporte { get; set; }
        public bool? ManejoAnticipoConObservaciones { get; set; }
        public int? CantidadHojasVidaContratistaObra { get; set; }
        public int? CantidadPerfilesInterventoria { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int ProyectoId { get; set; }
        public decimal? Administracion { get; set; }
        public string LicenciaObservaciones { get; set; }
        public string CambioObservaciones { get; set; }
        public string ActaApropiacionObservaciones { get; set; }
        public string ResiduosDemolicionObservaciones { get; set; }
        public string ManejoTransitoObservaciones { get; set; }
        public string ManejoAmbientalObservaciones { get; set; }
        public string AseguramientoCalidadObservaciones { get; set; }
        public string ProgramaSeguridadObservaciones { get; set; }
        public string ProgramaSaludObservaciones { get; set; }
        public string InventarioArboreoObservaciones { get; set; }
        public string AprovechamientoForestalObservaciones { get; set; }
        public string ManejoAguasLluviasObservaciones { get; set; }
        public bool? TieneObservacionesDiagnosticoApoyo { get; set; }
        public bool? TieneObservacionesDiagnosticoSupervisor { get; set; }
        public bool? TieneObservacionesPlanesProgramasApoyo { get; set; }
        public bool? TieneObservacionesPlanesProgramasSupervisor { get; set; }
        public bool? TieneObservacionesManejoAnticipoApoyo { get; set; }
        public bool? TieneObservacionesManejoAnticipoSupervisor { get; set; }
        public bool? TieneObservacionesProgramacionObraApoyo { get; set; }
        public bool? TieneObservacionesProgramacionObraSupervisor { get; set; }
        public bool? TieneObservacionesFlujoInversionApoyo { get; set; }
        public bool? TieneObservacionesFlujoInversionSupervisor { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? ArchivoCargueIdProgramacionObra { get; set; }
        public int? ArchivoCargueIdFlujoInversion { get; set; }
        public bool? RegistroCompletoVerificacion { get; set; }
        public int? ObservacionDiagnosticoSupervisorId { get; set; }
        public bool? RegistroCompletoDiagnostico { get; set; }
        public int? ObservacionPlanesProgramasSupervisorId { get; set; }
        public bool? RegistroCompletoPlanesProgramas { get; set; }


        public virtual Contrato Contrato { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<ConstruccionCargue> ConstruccionCargue { get; set; }
        public virtual ICollection<ConstruccionObservacion> ConstruccionObservacion { get; set; }
        public virtual ICollection<ConstruccionPerfil> ConstruccionPerfil { get; set; }
        public virtual ICollection<FlujoInversion> FlujoInversion { get; set; }
        public virtual ICollection<Programacion> Programacion { get; set; }
        public virtual ICollection<TempFlujoInversion> TempFlujoInversion { get; set; }
        public virtual ICollection<TempProgramacion> TempProgramacion { get; set; }
    }
}
