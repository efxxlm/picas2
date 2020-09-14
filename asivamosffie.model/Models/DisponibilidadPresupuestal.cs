using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class DisponibilidadPresupuestal
    {
        public DisponibilidadPresupuestal()
        {
            DisponibilidadPresupuestalObservacion = new HashSet<DisponibilidadPresupuestalObservacion>();
            DisponibilidadPresupuestalProyecto = new HashSet<DisponibilidadPresupuestalProyecto>();
        }


        public int DisponibilidadPresupuestalId { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public string TipoSolicitudCodigo { get; set; }

        public string NumeroSolicitud { get; set; }

        public string OpcionContratarCodigo { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal ValorSolicitud { get; set; }

        public string EstadoSolicitudCodigo { get; set; }

        public string Objeto { get; set; }
        public bool Eliminado { get; set; }

        public DateTime? FechaDdp { get; set; }

        public string NumeroDdp { get; set; }

        public string RutaDdp { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioModificacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? ContratacionId { get; set; }

        public string NumeroDrp { get; set; }
        public int? PlazoMeses { get; set; }
        public int? PlazoDias { get; set; }
        public bool? CuentaCartaAutorizacion { get; set; }
        public string AportanteId { get; set; }
        public decimal? ValorAportante { get; set; }
        public string NumeroContrato { get; set; }
        public string LimitacionEspecial { get; set; }
        public string NumeroRadicadoSolicitud { get; set; }
        public virtual ICollection<DisponibilidadPresupuestalProyecto> DisponibilidadPresupuestalProyecto { get; set; }
    }
}
