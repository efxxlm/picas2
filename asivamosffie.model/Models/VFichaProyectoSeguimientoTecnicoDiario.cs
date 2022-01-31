using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoSeguimientoTecnicoDiario
    {
        public int ProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int SeguimientoDiarioId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int ContratoId { get; set; }
        public DateTime FechaSeguimiento { get; set; }
        public string DisponibilidadPersonal { get; set; }
        public string DisponibilidadMaterial { get; set; }
        public string DisponibilidadEquipo { get; set; }
        public string Productividad { get; set; }
    }
}
