using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRequisitosTecnicosInicioConstruccion
    {
        public int? ApoyoId { get; set; }
        public int? InterventorId { get; set; }
        public int? SupervisorId { get; set; }
        public int ContratoId { get; set; }
        public string RutaActaFase1 { get; set; }
        public DateTime? FechaActaInicioFase1 { get; set; }
        public DateTime? FechaAprobacionRequisitosConstruccionInterventor { get; set; }
        public bool? RegistroCompletoConstruccion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoContratoCodigo { get; set; }
        public int? CantidadProyectosAsociados { get; set; }
        public int? CantidadProyectosRequisitosAprobados { get; set; }
        public int? CantidadProyectosRequisitosVerificados { get; set; }
        public int? CantidadProyectosRequisitosValidados { get; set; }
        public int? TieneFasePreconstruccion { get; set; }
        public int? TieneFaseConstruccion { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoNombreVerificacion { get; set; }
        public bool? ExisteRegistro { get; set; }
        public bool? EstaDevuelto { get; set; }
    }
}
