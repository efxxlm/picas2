using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class AjusteProgramacionFlujo
    {
        public int AjusteProgramacionFlujoId { get; set; }
        public int AjusteProgramacionId { get; set; }
        public string Semana { get; set; }
        public decimal? Valor { get; set; }
        public int? MesEjecucionId { get; set; }
        public int? ProgramacionId { get; set; }
        public int? SeguimientoSemanalId { get; set; }

        public virtual AjusteProgramacion AjusteProgramacion { get; set; }
    }
}
