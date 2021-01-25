using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SeguimientoSemanalReporteActividad
    {
        public int SeguimientoSemanalReporteActividadId { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public string ResumenEstadoContrato { get; set; }
        public bool? RegistroCompletoEstadoContrato { get; set; }
        public string ActividadTecnica { get; set; }
        public string ActividadLegal { get; set; }
        public string ActividadAdministrativaFinanciera { get; set; }
        public bool? RegistroCompletoActividad { get; set; }
        public string ActividadTecnicaSiguiente { get; set; }
        public string ActividadLegalSiguiente { get; set; }
        public string ActividadAdministrativaFinancieraSiguiente { get; set; }
        public bool? RegistroCompletoActividadSiguiente { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? TieneObservacionApoyoEstadoContrato { get; set; }
        public int? ObservacionApoyoIdEstadoContrato { get; set; }
        public bool? TieneObservacionSupervisorEstadoContrato { get; set; }
        public int? ObservacionSupervisorIdEstadoContrato { get; set; }
        public bool? TieneObservacionApoyoActividad { get; set; }
        public int? ObservacionApoyoIdActividad { get; set; }
        public bool? TieneObservacionSupervisorActividad { get; set; }
        public int? ObservacionSupervisorIdActividad { get; set; }
        public bool? TieneObservacionApoyoActividadSiguiente { get; set; }
        public int? ObservacionApoyoIdActividadSiguiente { get; set; }
        public bool? TieneObservacionSupervisorActividadSiguiente { get; set; }
        public int? ObservacionSupervisorIdActividadSiguiente { get; set; }
        public bool? RegistroCompletoObservacionApoyoEstadoContrato { get; set; }
        public bool? RegistroCompletoObservacionSupervisorEstadoContrato { get; set; }
        public bool? RegistroCompletoObservacionApoyoActividad { get; set; }
        public bool? RegistroCompletoObservacionSupervisorActividad { get; set; }
        public bool? RegistroCompletoObservacionApoyoActividadSiguiente { get; set; }
        public bool? RegistroCompletoObservacionSupervisorActividadSiguiente { get; set; }

        public virtual SeguimientoSemanalObservacion ObservacionApoyoIdActividadNavigation { get; set; }
        public virtual SeguimientoSemanalObservacion ObservacionApoyoIdActividadSiguienteNavigation { get; set; }
        public virtual SeguimientoSemanalObservacion ObservacionApoyoIdEstadoContratoNavigation { get; set; }
        public virtual SeguimientoSemanalObservacion ObservacionSupervisorIdActividadNavigation { get; set; }
        public virtual SeguimientoSemanalObservacion ObservacionSupervisorIdActividadSiguienteNavigation { get; set; }
        public virtual SeguimientoSemanalObservacion ObservacionSupervisorIdEstadoContratoNavigation { get; set; }
        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
    }
}
