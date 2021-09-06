using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VEjecucionFinancieraXproyecto
    {
        public int ContratoId { get; set; }
        public int ContratacionId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int ProyectoId { get; set; }
        public string Nombre { get; set; }
        public decimal? TotalComprometido { get; set; }
        public decimal? OrdenadoGirarAntesImpuestos { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? PorcentajeEjecucionFinanciera { get; set; }
        public decimal ValorTraslado { get; set; }
        public decimal ValorNetoOdg { get; set; }
    }
}
