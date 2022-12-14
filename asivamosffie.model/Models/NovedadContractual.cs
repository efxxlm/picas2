using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class NovedadContractual
    {
        public NovedadContractual()
        {
            AjusteProgramacion = new HashSet<AjusteProgramacion>();
            NovedadContractualAportante = new HashSet<NovedadContractualAportante>();
            NovedadContractualDescripcion = new HashSet<NovedadContractualDescripcion>();
            NovedadContractualObservaciones = new HashSet<NovedadContractualObservaciones>();
            NovedadContractualRegistroPresupuestal = new HashSet<NovedadContractualRegistroPresupuestal>();
        }

        public int NovedadContractualId { get; set; }
        public DateTime? FechaSolictud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string InstanciaCodigo { get; set; }
        public DateTime? FechaSesionInstancia { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? EsAplicadaAcontrato { get; set; }
        public int? ContratoId { get; set; }
        public int? ProyectoId { get; set; }
        public string UrlSoporte { get; set; }
        public int? ObervacionSupervisorId { get; set; }
        public bool? TieneObservacionesApoyo { get; set; }
        public bool? TieneObservacionesSupervisor { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public DateTime? FechaValidacion { get; set; }
        public bool? RegistroCompletoVerificacion { get; set; }
        public string EstadoCodigo { get; set; }
        public string CausaRechazo { get; set; }
        public bool? RegistroCompletoTramiteNovedades { get; set; }
        public DateTime? FechaEnvioGestionContractual { get; set; }
        public string EstadoProcesoCodigo { get; set; }
        public int? AbogadoRevisionId { get; set; }
        public bool? DeseaContinuar { get; set; }
        public DateTime? FechaEnvioActaContratistaObra { get; set; }
        public DateTime? FechaFirmaActaContratistaObra { get; set; }
        public DateTime? FechaEnvioActaContratistaInterventoria { get; set; }
        public DateTime? FechaFirmaContratistaInterventoria { get; set; }
        public DateTime? FechaEnvioActaApoyo { get; set; }
        public DateTime? FechaFirmaApoyo { get; set; }
        public DateTime? FechaEnvioActaSupervisor { get; set; }
        public DateTime? FechaFirmaSupervisor { get; set; }
        public string UrlSoporteFirmas { get; set; }
        public int? ObservacionesDevolucionId { get; set; }
        public string RazonesNoContinuaProceso { get; set; }
        public bool? RegistroCompletoValidacion { get; set; }
        public DateTime? FechaAprobacionGestionContractual { get; set; }
        public DateTime? FechaTramiteGestionar { get; set; }
        public string ObservacionGestionar { get; set; }
        public string UrlSoporteGestionar { get; set; }
        public bool? RegistroCompletoGestionar { get; set; }
        public string NumeroOtroSi { get; set; }
        public DateTime? FechaEnvioFirmaContratista { get; set; }
        public DateTime? FechaFirmaContratista { get; set; }
        public DateTime? FechaEnvioFirmaFiduciaria { get; set; }
        public DateTime? FechaFirmaFiduciaria { get; set; }
        public string ObservacionesTramite { get; set; }
        public string UrlDocumentoSuscrita { get; set; }
        public bool? RegistroCompletoTramite { get; set; }

        public virtual Contrato Contrato { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<AjusteProgramacion> AjusteProgramacion { get; set; }
        public virtual ICollection<NovedadContractualAportante> NovedadContractualAportante { get; set; }
        public virtual ICollection<NovedadContractualDescripcion> NovedadContractualDescripcion { get; set; }
        public virtual ICollection<NovedadContractualObservaciones> NovedadContractualObservaciones { get; set; }
        public virtual ICollection<NovedadContractualRegistroPresupuestal> NovedadContractualRegistroPresupuestal { get; set; }
    }
}
