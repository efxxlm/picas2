using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VVerificarSeguimientoSemanal
    {
        public int? SeguimientoSemanalId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int NumeroSemana { get; set; }
        public string LlaveMen { get; set; }
        public string NumeroContrato { get; set; }
        public int? ApoyoId { get; set; }
        public int? SupervisorId { get; set; }
        public int? InterventorId { get; set; }
        public string TipoIntervencion { get; set; }
        public string InstitucionEducativa { get; set; }
        public string Sede { get; set; }
        public int? EstadoSeguimientoSemanalCodigo { get; set; }
        public string EstadoObra { get; set; }
        public string EstadoSeguimientoSemanal { get; set; }
        public string EstadoMuestras { get; set; }
        public DateTime? FechaModificacionVerificar { get; set; }
        public DateTime? FechaReporte { get; set; }
        public int? CantidadSemanas { get; set; }
        public bool? RegistroCompletoVerificar { get; set; }
        public int? VerAvanceApoyo { get; set; }
    }
}
