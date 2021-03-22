using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VConfinanciacionReporte
    {
        public int CofinanciacionId { get; set; }
        public int? VigenciaCofinanciacionId { get; set; }
        public int? CofinanciacionAportanteId { get; set; }
        public int? TipoAportanteId { get; set; }
        public string TipoAportante { get; set; }
        public string IdPadre { get; set; }
        public string LocalizacionId { get; set; }
        public int? CofinanciacionDocumentoId { get; set; }
        public int? VigenciaAporte { get; set; }
        public decimal? ValorDocumento { get; set; }
        public int? TipoDocumentoId { get; set; }
        public DateTime? FechaAcuerdo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
