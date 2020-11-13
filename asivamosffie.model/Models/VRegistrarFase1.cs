using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VRegistrarFase1
    {
        public string NumeroSolicitud { get; set; }
        public int ContratoId { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string NumeroContrato { get; set; }
        public int? CantidadProyectosAsociados { get; set; }
        public int CantidadProyectosRequisitosAprobados { get; set; }
        public int? CantidadProyectosConPerfilesPendientes { get; set; }
        public string EstadoCodigo { get; set; }
        public int? EstaDevuelto { get; set; }
        public int RegistroCompleto { get; set; }
    }
}
