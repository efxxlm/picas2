using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class SourceFundingGrid
    {
        public int TipoAportanteId { get; set; }
        public string TipoAportante {get; set;}
        public string NombreAportante { get; set; }
        public string vigenciaAcuerdoCofinanciacion { get; set; }
        public string FuenteRecurso { get; set; }
    }
}
