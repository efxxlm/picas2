using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VNovedadContractual
    {
        public int NovedadContractualId { get; set; }
        public DateTime? FechaSolictud { get; set; }
        public string NumeroSolicitud { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoCodigoNombre { get; set; }
        public string EstadoDescripcion { get; set; }
        public bool? TieneObservacionesApoyo { get; set; }
        public bool? TieneObservacionesSupervisor { get; set; }
        public string NovedadesSeleccionadas { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string NumeroContrato { get; set; }
        public bool? RegistroCompletoVerificacion { get; set; }
        public bool? RegistroCompletoValidacion { get; set; }
        public bool? RegistroCompletoTramiteNovedades { get; set; }
        public int? CantidadSoporteSuficienteNovedad { get; set; }
        public int? CantidadDescripcion { get; set; }
        public string LlaveMen { get; set; }
        public int ContratoId { get; set; }
        public int? InterventorId { get; set; }
        public int? ApoyoId { get; set; }
        public int? SupervisorId { get; set; }
        public string NombreContratista { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int ProyectoId { get; set; }
    }
}
