using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaComponentes
    {
        public int ComponenteUsoId { get; set; }
        public string ComponenteUsoCodigo { get; set; }
        public int ComponenteAportanteId { get; set; }
        public string Componente { get; set; }
        public List<string> Uso { get; set; }
        public List<decimal> ValorUso{ get; set; }
        public decimal ValorTotal { get; set; }
        public int cofinanciacionAportanteId { get; set; }
        public int? FuenteFinanciacionId { get; set; }
        public string? Fase { get; set; }
        public string? Fuente { get; set; }
        public string? Aportante { get; set; }

    }
}
