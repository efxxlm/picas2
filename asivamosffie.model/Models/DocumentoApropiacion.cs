using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DocumentoApropiacion
    {
        public int DocumentoApropiacionId { get; set; }
        public int AportanteId { get; set; }
        public string VigenciaAporteCodigo { get; set; }
        public decimal Valor { get; set; }
        public string TipoDocumentoCodigo { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
