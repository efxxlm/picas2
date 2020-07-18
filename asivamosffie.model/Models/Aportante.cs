using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Aportante
    {
        public Aportante()
        {
            DocumentoApropiacion = new HashSet<DocumentoApropiacion>();
            FuenteFinanciacion = new HashSet<FuenteFinanciacion>();
            ProyectoAportante = new HashSet<ProyectoAportante>();
            RegistroPresupuestal = new HashSet<RegistroPresupuestal>();
        }

        public int AportanteId { get; set; }
        public string TipoAportanteCodigo { get; set; }
        public string NombreCodigo { get; set; }
        public string LocalizacionIdMunicipio { get; set; }
        public int CantidadDocumentos { get; set; }
        public decimal ValorTotal { get; set; }
        public int AcuerdoCofinanciacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual AcuerdoCofinanciamiento AcuerdoCofinanciacion { get; set; }
        public virtual Localizacion LocalizacionIdMunicipioNavigation { get; set; }
        public virtual ICollection<DocumentoApropiacion> DocumentoApropiacion { get; set; }
        public virtual ICollection<FuenteFinanciacion> FuenteFinanciacion { get; set; }
        public virtual ICollection<ProyectoAportante> ProyectoAportante { get; set; }
        public virtual ICollection<RegistroPresupuestal> RegistroPresupuestal { get; set; }
    }
}
