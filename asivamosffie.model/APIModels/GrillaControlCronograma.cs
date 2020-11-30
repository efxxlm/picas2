using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaControlCronograma
    {
        public int ProcesoSeleccionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string TipoProcesoCodigo { get; set; }
        public string TipoProcesoCodigoText { get; set; }
        public string NumeroProceso { get; set; }
        public string EtapaProcesoSeleccionCodigo { get; set; }
        public string EEtapaProcesoSeleccionText { get; set; }
        public string EstadoProcesoSeleccionCodigo { get; set; }
        public string EstadoProcesoSeleccionText { get; set; }
        public bool? EsCompleto { get; set; }
        public string EsCompletoText { get; set; }


    }
}
