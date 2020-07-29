using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ComiteTecnico
    {
        public ComiteTecnico()
        {
            ComiteTecnicoProyecto = new HashSet<ComiteTecnicoProyecto>();
            SesionComiteTecnico = new HashSet<SesionComiteTecnico>();
        }

        public int ComiteTecnicoId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public bool EsMultiProyecto { get; set; }
        public string TipoContratacionCodigo { get; set; }
        public string NumeroSolicitud { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
        public bool Estado { get; set; }
        public int ContratistaId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual Contratista Contratista { get; set; }
        public virtual ICollection<ComiteTecnicoProyecto> ComiteTecnicoProyecto { get; set; }
        public virtual ICollection<SesionComiteTecnico> SesionComiteTecnico { get; set; }
    }
}
