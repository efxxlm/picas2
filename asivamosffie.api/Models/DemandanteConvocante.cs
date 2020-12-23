using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class DemandanteConvocante
    {
        public int DemandanteConvocadoId { get; set; }
        public bool? EsConvocante { get; set; }
        public string Nombre { get; set; }
        public string TipoIdentificacionCodigo { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public int? CantDemandados { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int? DefensaJucicialId { get; set; }

        public virtual DefensaJudicial DefensaJucicial { get; set; }
    }
}
