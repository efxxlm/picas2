using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class VistaContratoContratista
    {
        public int IdContratista { get; set; }
        public string NombreContratista { get; set; }
        public string NumeroContrato { get; set; }
        public string PlazoFormat { get; set; }
        public string FechaInicioContrato { get; set; }
        public string FechaFinContrato { get; set; }
        public string TipoDocumentoContratista { get; set; }
        public string TipoIntervencion { get; set; }
        public string TipoIntervencionCodigo { get; set; }
        public string NumeroIdentificacion { get; set; }
    }
}
