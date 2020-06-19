using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CofinanciacionDocumento
    {
        public int CofinancicacionDocumentoId { get; set; }
        public int CofinanciacionId { get; set; }
        public int VigenciaAporteId { get; set; }
        public string ValorDocumento { get; set; }
        public int TipoDocumentoId { get; set; }
        public string NumeroActa { get; set; }
        public DateTime? FechaActa { get; set; }
        public int? NumeroAcuerdo { get; set; }
        public string FechaAcuerdo { get; set; }
        public string ValorTotalAportante { get; set; }
        public bool Eliminado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaEdicion { get; set; }
        public string UsuarioEdicion { get; set; }

        public virtual Cofinanciacion Cofinanciacion { get; set; }
    }
}
