using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class SesionComiteTemaMapped
    {
        [NotMapped]
        public string NombreResponsable { get; set; }

        [NotMapped]
        public bool? RegistroCompletoActa { get; set; }
        [NotMapped] 
        public int SesionTemaId { get; set; }
        [NotMapped]  
        public string Tema { get; set; }
        [NotMapped]
        public string ResponsableCodigo { get; set; }
        [NotMapped]
        public int? TiempoIntervencion { get; set; }
        [NotMapped]
        public string RutaSoporte { get; set; }
        [NotMapped]
        public string Observaciones { get; set; }
        [NotMapped]
        public bool? EsAprobado { get; set; }
        [NotMapped]
        public string ObservacionesDecision { get; set; }
        [NotMapped]
        public DateTime FechaCreacion { get; set; }
        [NotMapped]
        public string UsuarioCreacion { get; set; }
        [NotMapped]
        public DateTime? FechaModificacion { get; set; }
        [NotMapped]
        public string UsuarioModificacion { get; set; }
        [NotMapped]
        public bool? Eliminado { get; set; }
        [NotMapped]
        public int? ComiteTecnicoId { get; set; }
        [NotMapped]
        public bool? EsProposicionesVarios { get; set; }
        [NotMapped]
        public bool? RequiereVotacion { get; set; }
        [NotMapped]
        public string EstadoTemaCodigo { get; set; }
        [NotMapped]
        public bool? GeneraCompromiso { get; set; }
        [NotMapped]
        public int? CantCompromisos { get; set; }
        [NotMapped]
        public bool? RegistroCompleto { get; set; }
        [NotMapped]
        public virtual ICollection<SesionTemaVoto> SesionTemaVoto { get; set; }
        [NotMapped]
        public virtual ICollection<TemaCompromiso> TemaCompromiso { get; set; }


    }

}
