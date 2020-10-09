using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ConstruccionPerfil
    {
        public ConstruccionPerfil()
        {
            ConstruccionPerfilNumeroRadicado = new HashSet<ConstruccionPerfilNumeroRadicado>();
            ConstruccionPerfilObservacion = new HashSet<ConstruccionPerfilObservacion>();
        }

        public int ConstruccionPerfilId { get; set; }
        public int ContratoConstruccionId { get; set; }
        public string PerfilCodigo { get; set; }
        public int CantidadHvRequeridas { get; set; }
        public int CantidadHvRecibidas { get; set; }
        public int CantidadHvAprobadas { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string NumeroRadicadoFfie { get; set; }
        public string NumeroRadicadoFfie1 { get; set; }
        public string NumeroRadicadoFfie2 { get; set; }
        public string NumeroRadicadoFfie3 { get; set; }
        public string RutaSoporte { get; set; }
        public bool? ConObervacionesSupervision { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool RegistroCompleto { get; set; }
        public bool Eliminado { get; set; }

        public virtual ContratoConstruccion ContratoConstruccion { get; set; }
        public virtual ICollection<ConstruccionPerfilNumeroRadicado> ConstruccionPerfilNumeroRadicado { get; set; }
        public virtual ICollection<ConstruccionPerfilObservacion> ConstruccionPerfilObservacion { get; set; }
    }
}
