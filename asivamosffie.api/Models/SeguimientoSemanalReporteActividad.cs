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

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
    }
}
