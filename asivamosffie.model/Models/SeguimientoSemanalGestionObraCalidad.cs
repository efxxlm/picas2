using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalGestionObraCalidad
    {
        public SeguimientoSemanalGestionObraCalidad()
        {
            GestionObraCalidadEnsayoLaboratorio = new HashSet<GestionObraCalidadEnsayoLaboratorio>();
        }

        public int SeguimientoSemanalGestionObraCalidadId { get; set; }
        public int SeguimientoSemanalGestionObraId { get; set; }
        public bool? SeRealizaronEnsayosLaboratorio { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual ICollection<GestionObraCalidadEnsayoLaboratorio> GestionObraCalidadEnsayoLaboratorio { get; set; }
    }
}
