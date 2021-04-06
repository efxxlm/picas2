using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalTemp
    {
        public int SeguimientoSemanalTempId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int? AjusteProgramaionId { get; set; }
        public int NumeroSemana { get; set; }
        public bool Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoSeguimientoSemanalCodigo { get; set; }
        public bool? RegistroCompletoMuestras { get; set; }
        public DateTime? FechaRegistroCompletoInterventor { get; set; }
        public bool? TieneObservacionApoyo { get; set; }
        public bool? RegistroCompletoVerificar { get; set; }
        public bool? TieneObservacionSupervisor { get; set; }
        public bool? RegistroCompletoAvalar { get; set; }
        public string EstadoMuestrasCodigo { get; set; }
        public DateTime? FechaModificacionVerificar { get; set; }
        public DateTime? FechaModificacionAvalar { get; set; }
        public DateTime? FechaRegistroCompletoApoyo { get; set; }
        public DateTime? FechaRegistroCompletoSupervisor { get; set; }

        public virtual AjusteProgramacion AjusteProgramaion { get; set; }
        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
    }
}
