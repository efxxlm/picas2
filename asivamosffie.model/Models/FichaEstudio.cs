using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class FichaEstudio
    {
        public int FichaEstudioId { get; set; }
        public int DefensaJudicialId { get; set; }
        public string Antecedentes { get; set; }
        public string HechosRelevantes { get; set; }
        public string JurisprudenciaDoctrina { get; set; }
        public string DecisionComiteDirectrices { get; set; }
        public string AnalisisJuridico { get; set; }
        public string Recomendaciones { get; set; }
        public bool? EsPresentadoAnteComiteFfie { get; set; }
        public DateTime? FechaComiteDefensa { get; set; }
        public string RecomendacionFinalComite { get; set; }
        public bool? EsAprobadoAperturaProceso { get; set; }
        public string TipoActuacionCodigo { get; set; }
        public bool? EsActuacionTramiteComite { get; set; }
        public string Abogado { get; set; }
        public string RutaSoporte { get; set; }
        public bool EsCompleto { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual DefensaJudicial DefensaJudicial { get; set; }
    }
}
