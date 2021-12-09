using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VSeguimientoSemanalXSeguimientoSemanalAvanceFisicoProgramacion
    {
        public int? SeguimientoSemanalId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public int? AvanceProgramado {get; set; }
        public decimal? AvanceFisicoCapitulo { get; set; }
        public decimal? ProgramacionCapitulo { get; set; }
        public string Capitulo { get; set; }
    }
}
