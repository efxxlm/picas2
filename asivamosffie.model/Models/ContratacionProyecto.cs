using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ContratacionProyecto
    {
        public ContratacionProyecto()
        {
            ContratacionProyectoAportante = new HashSet<ContratacionProyectoAportante>();
        }

        public int ContratacionProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int ProyectoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public bool Eliminado { get; set; }
        public bool? EsReasignacion { get; set; }
        public bool? EsAvanceObra { get; set; }
        public decimal? PorcentajeAvanceObra { get; set; }
        public bool? RequiereLicencia { get; set; }
        public bool? LicenciaVigente { get; set; }
        public string NumeroLicencia { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual Contratacion Contratacion { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<ContratacionProyectoAportante> ContratacionProyectoAportante { get; set; }
    }
}
