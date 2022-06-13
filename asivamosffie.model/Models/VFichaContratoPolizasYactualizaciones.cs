using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaContratoPolizasYactualizaciones
    {
        public bool? EsDrpOriginal { get; set; }
        public int ContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public string NombreContratista { get; set; }
        public string TipoContrato { get; set; }
        public string NumeroPoliza { get; set; }
        public string NombreAseguradora { get; set; }
        public string NumeroCertificado { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public string PolizasSeguros { get; set; }
        public DateTime? Vigencia { get; set; }
        public DateTime? VigenciaAmparo { get; set; }
        public decimal? ValorAmparo { get; set; }
    }
}
