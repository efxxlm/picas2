using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Contratacion
    {
        public Contratacion()
        {
            ContratacionProyecto = new HashSet<ContratacionProyecto>();
            Contrato = new HashSet<Contrato>();
            SesionComiteSolicitud = new HashSet<SesionComiteSolicitud>();
        }

        public int ContratacionId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? ContratistaId { get; set; }
        public bool? EsObligacionEspecial { get; set; }
        public string ConsideracionDescripcion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaEnvioDocumentacion { get; set; }
        public string Observaciones { get; set; }
        public string RutaMinuta { get; set; }
        public DateTime? FechaTramite { get; set; }
        public bool? Estado { get; set; }

        public virtual Contratista Contratista { get; set; }
        public virtual ICollection<ContratacionProyecto> ContratacionProyecto { get; set; }
        public virtual ICollection<Contrato> Contrato { get; set; }
        public virtual ICollection<SesionComiteSolicitud> SesionComiteSolicitud { get; set; }
    }
}
