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
        }

        public int ContratoConstruccionId { get; set; }
        public int ContratoId { get; set; }
        public bool? EsInformeDiagnostico { get; set; }
        public string RutaInforme { get; set; }
        public decimal? CostoDirecto { get; set; }
        public decimal? Administración { get; set; }
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
        public bool? PlanInventarioArboreo { get; set; }
        public DateTime? InventarioArboreoFechaRadicado { get; set; }
        public DateTime? InventarioArboreoFechaAprobacion { get; set; }
        public bool? InventarioArboreoConObservaciones { get; set; }
        public bool? PlanAprovechamientoForestal { get; set; }
        public DateTime? AprovechamientoForestalApropiacionFechaRadicado { get; set; }
        public DateTime? AprovechamientoForestalFechaAprobacion { get; set; }
        public bool? AprovechamientoForestalConObservaciones { get; set; }
        public bool? PlanManejoAguasLluvias { get; set; }
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

        public virtual Contrato Contrato { get; set; }
        public virtual ICollection<ConstruccionCargue> ConstruccionCargue { get; set; }
        public virtual ICollection<ConstruccionObservacion> ConstruccionObservacion { get; set; }
        public virtual ICollection<ConstruccionPerfil> ConstruccionPerfil { get; set; }
    }
}
