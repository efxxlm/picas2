using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Aportante
    {
        public Aportante()
        {
            ContratacionProyectoAportante = new HashSet<ContratacionProyectoAportante>();
            DocumentoApropiacion = new HashSet<DocumentoApropiacion>();
        }

        public int AportanteId { get; set; }
        public int AcuerdoCofinanciacionId { get; set; }
        public string TipoAportanteCodigo { get; set; }
        public string NombreCodigo { get; set; }
        public string LocalizacionIdMunicipio { get; set; }
        public int CantidadDocumentos { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual AcuerdoCofinanciamiento AcuerdoCofinanciacion { get; set; }
        public virtual Localizacion LocalizacionIdMunicipioNavigation { get; set; }
        public virtual ICollection<ContratacionProyectoAportante> ContratacionProyectoAportante { get; set; }
        public virtual ICollection<DocumentoApropiacion> DocumentoApropiacion { get; set; }
    }
}
