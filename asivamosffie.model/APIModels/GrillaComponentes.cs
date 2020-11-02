using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaComponentes
    {
        public int ComponenteUsoId { get; set; }
        public int ComponenteAportanteId { get; set; }
        public string Componente { get; set; }
        public string Uso { get; set; }
        public decimal ValorUso{ get; set; }
        public decimal ValorTotal { get; set; }

    }
}
