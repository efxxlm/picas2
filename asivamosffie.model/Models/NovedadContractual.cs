using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class NovedadContractual
    {
        public int NovedadContractualId { get; set; }
        public DateTime FechaSolictud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string InstanciaCodigo { get; set; }
        public DateTime FechaSesionInstancia { get; set; }
        public string TipoNovedadCodigo { get; set; }
        public string MotivoNovedadCodigo { get; set; }
        public string ResumenJustificacion { get; set; }
        public bool EsDocumentacionSoporte { get; set; }
        public string ConceptoTecnico { get; set; }
        public DateTime? FechaConcepto { get; set; }
        public DateTime? FechaInicioSuspension { get; set; }
        public DateTime? FechaFinSuspension { get; set; }
        public decimal? PresupuestoAdicionalSolicitado { get; set; }
        public int? PlazoAdicionalDias { get; set; }
        public int? PlazoAdicionalMeses { get; set; }
        public string ClausulaModificar { get; set; }
        public string AjusteClausula { get; set; }
        public int SolicitudId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? EsAplicadaAcontrato { get; set; }
        public int? ContratoId { get; set; }
        public int? ProyectoId { get; set; }
        public string NumeroRadicad { get; set; }
        public string NumeroRadicado { get; set; }
        public string UrlSoporte { get; set; }
        public int? ObervacionSupervisorId { get; set; }
        public bool? TieneObservacionesApoyo { get; set; }
        public bool? TieneObservacionesSupervisor { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public DateTime? FechaValidacion { get; set; }
        public bool? RegistroCompletoVerificacion { get; set; }
        public string EstadoCodigo { get; set; }
        public string CausaRechazo { get; set; }

        public virtual Contrato Contrato { get; set; }
        public virtual Proyecto Proyecto { get; set; }
    }
}
