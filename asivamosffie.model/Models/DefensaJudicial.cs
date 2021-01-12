using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DefensaJudicial
    {
        public DefensaJudicial()
        {
            DefensaJudicialContratacionProyecto = new HashSet<DefensaJudicialContratacionProyecto>();
            DefensaJudicialSeguimiento = new HashSet<DefensaJudicialSeguimiento>();
            DemandadoConvocado = new HashSet<DemandadoConvocado>();
            DemandanteConvocante = new HashSet<DemandanteConvocante>();
            FichaEstudio = new HashSet<FichaEstudio>();
        }

        public int DefensaJudicialId { get; set; }
        public string LegitimacionCodigo { get; set; }
        public string TipoProcesoCodigo { get; set; }
        public string NumeroProceso { get; set; }
        public int CantContratos { get; set; }
        public string EstadoProcesoCodigo { get; set; }
        public int SolicitudId { get; set; }
        public bool EsLegitimacionActiva { get; set; }
        public bool EsCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int InstitucionEducativaSedeId { get; set; }
        public int? LocalizacionIdMunicipio { get; set; }
        public string TipoAccionCodigo { get; set; }
        public string JurisdiccionCodigo { get; set; }
        public string Pretensiones { get; set; }
        public decimal? CuantiaPerjuicios { get; set; }
        public bool? EsRequiereSupervisor { get; set; }
        public DateTime? FechaRadicadoFfie { get; set; }
        public string NumeroRadicadoFfie { get; set; }
        public string CanalIngresoCodigo { get; set; }
        public bool? EsDemandaFfie { get; set; }
        public int? NumeroDemandantes { get; set; }
        public string UrlSoporteProceso { get; set; }
        public int? NumeroDemandados { get; set; }
        public bool? ExisteConocimiento { get; set; }

        public virtual ICollection<DefensaJudicialContratacionProyecto> DefensaJudicialContratacionProyecto { get; set; }
        public virtual ICollection<DefensaJudicialSeguimiento> DefensaJudicialSeguimiento { get; set; }
        public virtual ICollection<DemandadoConvocado> DemandadoConvocado { get; set; }
        public virtual ICollection<DemandanteConvocante> DemandanteConvocante { get; set; }
        public virtual ICollection<FichaEstudio> FichaEstudio { get; set; }
    }
}
