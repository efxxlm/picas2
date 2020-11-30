using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DefensaJudicialContrato
    {
        public DefensaJudicialContrato()
        {
            DemandanteConvocante = new HashSet<DemandanteConvocante>();
        }

        public int DefensaJudicialContratoId { get; set; }
        public int DefensaJudicialId { get; set; }
        public int ContratoId { get; set; }
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
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual Contrato Contrato { get; set; }
        public virtual DefensaJudicial DefensaJudicial { get; set; }
        public virtual ICollection<DemandanteConvocante> DemandanteConvocante { get; set; }
    }
}
