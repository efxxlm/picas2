using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class EnsayoLaboratorioMuestra
    {
        public int EnsayoLaboratorioMuestraId { get; set; }
        public int GestionObraCalidadEnsayoLaboratorioId { get; set; }
        public DateTime? FechaEntregaResultado { get; set; }
        public string NombreMuestra { get; set; }
        public string Observacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual GestionObraCalidadEnsayoLaboratorio GestionObraCalidadEnsayoLaboratorio { get; set; }
    }
}
