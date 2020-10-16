using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DefensaJudicial
    {
        public DefensaJudicial()
        {
            DefensaJudicialContrato = new HashSet<DefensaJudicialContrato>();
            DefensaJudicialSeguimiento = new HashSet<DefensaJudicialSeguimiento>();
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

        public virtual ICollection<DefensaJudicialContrato> DefensaJudicialContrato { get; set; }
        public virtual ICollection<DefensaJudicialSeguimiento> DefensaJudicialSeguimiento { get; set; }
        public virtual ICollection<FichaEstudio> FichaEstudio { get; set; }
    }
}
