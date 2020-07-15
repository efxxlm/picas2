using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Contratacion
    {
        public Contratacion()
        {
            ContratacionProyecto = new HashSet<ContratacionProyecto>();
        }

        public int ContratacionId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
        public bool Estado { get; set; }
        public int? ContratistaId { get; set; }
        public bool? EsObligacionEspecial { get; set; }
        public string ConsideracionDescripcion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual Contratista Contratista { get; set; }
        public virtual ICollection<ContratacionProyecto> ContratacionProyecto { get; set; }
    }
}
