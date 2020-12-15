using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalGestionObraSeguridadSalud
    {
        public SeguimientoSemanalGestionObraSeguridadSalud()
        {
            SeguridadSaludCausaAccidente = new HashSet<SeguridadSaludCausaAccidente>();
        }

        public int SeguimientoSemanalGestionObraSeguridadSaludId { get; set; }
        public int SeguimientoSemanalGestionObraId { get; set; }
        public bool? SeRealizoCapacitacion { get; set; }
        public string TemaCapacitacion { get; set; }
        public bool? SeRealizoRevisionElementosProteccion { get; set; }
        public bool? CumpleRevisionElementosProyeccion { get; set; }
        public bool? SeRealizoRevisionSenalizacion { get; set; }
        public bool? CumpleRevisionSenalizacion { get; set; }
        public string UrlSoporteGestion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public int CantidadAccidentes { get; set; }
        public virtual SeguimientoSemanalGestionObra SeguimientoSemanalGestionObra { get; set; }
        public virtual ICollection<SeguridadSaludCausaAccidente> SeguridadSaludCausaAccidente { get; set; }
    }
}
