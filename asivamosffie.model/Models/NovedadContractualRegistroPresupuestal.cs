using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class NovedadContractualRegistroPresupuestal
    {
        public NovedadContractualRegistroPresupuestal()
        {
            DisponibilidadPresupuestalObservacion = new HashSet<DisponibilidadPresupuestalObservacion>();
        }

        public int NovedadContractualRegistroPresupuestalId { get; set; }
        public int? NovedadContractualId { get; set; }
        public int? DisponibilidadPresupuestalId { get; set; }
        public string NumeroSolicitud { get; set; }
        public decimal ValorSolicitud { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
        public string Objeto { get; set; }
        public bool Eliminado { get; set; }
        public DateTime? FechaDdp { get; set; }
        public string NumeroDrp { get; set; }
        public int? PlazoMeses { get; set; }
        public int? PlazoDias { get; set; }
        public DateTime? FechaDrp { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual DisponibilidadPresupuestal DisponibilidadPresupuestal { get; set; }
        public virtual NovedadContractual NovedadContractual { get; set; }
        public virtual ICollection<DisponibilidadPresupuestalObservacion> DisponibilidadPresupuestalObservacion { get; set; }
    }
}
