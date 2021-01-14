using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VValidarSeguimientoSemanal
    {
        public int ContratacionProyectoId { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public DateTime? FechaReporte { get; set; }
        public string LlaveMen { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoIntervencion { get; set; }
        public string InstitucionEducativa { get; set; }
        public string Sede { get; set; }
        public string EstadoObra { get; set; }
        public string EstadoSeguimientoSemanal { get; set; }
        public string EstadoSeguimientoSemanalCodigo { get; set; }
        public string EstadoMuestras { get; set; }
        public int? CantidadSemanas { get; set; }
        public int NumeroSemana { get; set; }
        public DateTime? FechaModificacionAvalar { get; set; }
        public bool? RegistroCompletoAvalar { get; set; }
        public bool? TieneObservacionSupervisor { get; set; }
        public int? VerAvanceSupervisor { get; set; }
    }
}
