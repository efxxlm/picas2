using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Aportante
    {
        public Aportante()
        {
            DocumentoApropiacion = new HashSet<DocumentoApropiacion>();
        }

        public int AportanteId { get; set; }
        public string TipoAportanteCodigo { get; set; }
        public string NombreCodigo { get; set; }
        public int? LocalizacionIdMunicipio { get; set; }
        public int CantidadDocumentos { get; set; }
        public decimal ValorTotal { get; set; }

        public virtual ICollection<DocumentoApropiacion> DocumentoApropiacion { get; set; }
    }
}
