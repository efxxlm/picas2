using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class NovedadContractualDescripcion
    {
        public NovedadContractualDescripcion()
        {
            NovedadContractualClausula = new HashSet<NovedadContractualClausula>();
            NovedadContractualDescripcionMotivo = new HashSet<NovedadContractualDescripcionMotivo>();
        }

        public int NovedadContractualDescripcionId { get; set; }
        public int NovedadContractualId { get; set; }
        public string TipoNovedadCodigo { get; set; }
        public string MotivoNovedadCodigo { get; set; }
        public string ResumenJustificacion { get; set; }
        public bool? EsDocumentacionSoporte { get; set; }
        public string ConceptoTecnico { get; set; }
        public DateTime? FechaConcepto { get; set; }
        public DateTime? FechaInicioSuspension { get; set; }
        public DateTime? FechaFinSuspension { get; set; }
        public decimal? PresupuestoAdicionalSolicitado { get; set; }
        public decimal? PlazoAdicionalDias { get; set; }
        public decimal? PlazoAdicionalMeses { get; set; }
        public string ClausulaModificar { get; set; }
        public string AjusteClausula { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string NumeroRadicado { get; set; }

        public virtual NovedadContractual NovedadContractual { get; set; }
        public virtual ICollection<NovedadContractualClausula> NovedadContractualClausula { get; set; }
        public virtual ICollection<NovedadContractualDescripcionMotivo> NovedadContractualDescripcionMotivo { get; set; }
    }
}
