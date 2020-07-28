using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DisponibilidadPresupuestal
    {
        public DisponibilidadPresupuestal()
        {
            DisponibilidadPresupuestalProyecto = new HashSet<DisponibilidadPresupuestalProyecto>();
        }

        public int DisponibilidadPresupuestalId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string NumeroSolicitud { get; set; }
        public string OpcionContratarCodigo { get; set; }
        public decimal ValorSolicitud { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
        public string Objeto { get; set; }
        public bool Eliminado { get; set; }
        public DateTime? FechaDdp { get; set; }
        public string NumeroDdp { get; set; }
        public string RutaDdp { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ICollection<DisponibilidadPresupuestalProyecto> DisponibilidadPresupuestalProyecto { get; set; }
    }
}
