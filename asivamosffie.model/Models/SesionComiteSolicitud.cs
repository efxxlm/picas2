using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SesionComiteSolicitud
    {
        public int SesionComiteSolicitudId { get; set; }
        public int SesionComiteTecnicoId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int SolicitudId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual SesionComiteTecnico SesionComiteTecnico { get; set; }
        public virtual Contratacion Solicitud { get; set; }
        public virtual DefensaJudicial Solicitud1 { get; set; }
        public virtual NovedadContractual Solicitud2 { get; set; }
        public virtual ProcesoSeleccion Solicitud3 { get; set; }
        public virtual ControversiaContractual SolicitudNavigation { get; set; }
    }
}
