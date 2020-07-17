using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class ResourceControlGrid
    {
        public int ControlRecursoId { get; set; }
        public string FuenteFinanciacionText { get; set; }
        public string CuentaBancariaText { get; set; }
        public string RegistroPresupuestalText { get; set; }
        public string VigenciaAporteText { get; set; }
        public DateTime FechaConsignacion { get; set; }
        public decimal ValorConsignacion { get; set; }



    }
}
