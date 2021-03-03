using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class InformeFinal
    {
        public InformeFinal()
        {
            InformeFinalInterventoria = new HashSet<InformeFinalInterventoria>();
            InformeFinalObservaciones = new HashSet<InformeFinalObservaciones>();
            ProyectoEntregaEtc = new HashSet<ProyectoEntregaEtc>();
        }

        public int InformeFinalId { get; set; }
        public int ProyectoId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string EstadoInforme { get; set; }
        public bool RegistroCompleto { get; set; }
        public DateTime? FechaSuscripcion { get; set; }
        public string UrlActa { get; set; }
        public bool Eliminado { get; set; }
        public string EstadoValidacion { get; set; }
        public bool? TieneObservacionesValidacion { get; set; }
        public bool? RegistroCompletoValidacion { get; set; }
        public string EstadoAprobacion { get; set; }
        public bool? TieneObservacionesSupervisor { get; set; }
        public DateTime? FechaEnvioSupervisor { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string EstadoCumplimiento { get; set; }
        public bool? TieneObservacionesCumplimiento { get; set; }
        public string EstadoEntregaEtc { get; set; }
        public bool? RegistroCompletoEntregaEtc { get; set; }
        public bool? TieneObservacionesInterventoria { get; set; }
        public DateTime? FechaEnvioApoyoSupervisor { get; set; }
        public DateTime? FechaEnvioGrupoNovedades { get; set; }
        public DateTime? FechaAprobacionFinal { get; set; }

        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<InformeFinalInterventoria> InformeFinalInterventoria { get; set; }
        public virtual ICollection<InformeFinalObservaciones> InformeFinalObservaciones { get; set; }
        public virtual ICollection<ProyectoEntregaEtc> ProyectoEntregaEtc { get; set; }
    }
}
