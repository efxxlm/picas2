using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class InformeFinal
    {
        public InformeFinal()
        {
            InformeFinalInterventoria = new HashSet<InformeFinalInterventoria>();
        }

        public int InformeFinalId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string EstadoInforme { get; set; }
        public bool RegistroCompleto { get; set; }
        public DateTime? FechaSuscripcion { get; set; }
        public string UrlActa { get; set; }
        public bool Eliminado { get; set; }

        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual ICollection<InformeFinalInterventoria> InformeFinalInterventoria { get; set; }
    }
}
