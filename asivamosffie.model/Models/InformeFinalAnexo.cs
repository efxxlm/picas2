using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class InformeFinalAnexo
    {
        public InformeFinalAnexo()
        {
            InformeFinalInterventoria = new HashSet<InformeFinalInterventoria>();
        }

        public int InformeFinalAnexoId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string TipoAnexo { get; set; }
        public int? NumRadicadoSac { get; set; }
        public DateTime? FechaRadicado { get; set; }
        public string UrlSoporte { get; set; }

        public virtual ICollection<InformeFinalInterventoria> InformeFinalInterventoria { get; set; }
    }
}
