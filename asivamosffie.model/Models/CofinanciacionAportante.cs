using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CofinanciacionAportante
    {
        public CofinanciacionAportante()
        {
            AportanteFuenteFinanciacion = new HashSet<AportanteFuenteFinanciacion>();
            CofinanciacionDocumento = new HashSet<CofinanciacionDocumento>();
            FuenteFinanciacion = new HashSet<FuenteFinanciacion>();
            ProyectoAdministrativoAportante = new HashSet<ProyectoAdministrativoAportante>();
            ProyectoAportante = new HashSet<ProyectoAportante>();
            RegistroPresupuestal = new HashSet<RegistroPresupuestal>();
        }

        public int CofinanciacionAportanteId { get; set; }
        public int? CofinanciacionId { get; set; }
        public int TipoAportanteId { get; set; }
        public int? NombreAportanteId { get; set; }
        public int? MunicipioId { get; set; }
        public bool Eliminado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual Cofinanciacion Cofinanciacion { get; set; }
        public virtual ICollection<AportanteFuenteFinanciacion> AportanteFuenteFinanciacion { get; set; }
        public virtual ICollection<CofinanciacionDocumento> CofinanciacionDocumento { get; set; }
        public virtual ICollection<FuenteFinanciacion> FuenteFinanciacion { get; set; }
        public virtual ICollection<ProyectoAdministrativoAportante> ProyectoAdministrativoAportante { get; set; }
        public virtual ICollection<ProyectoAportante> ProyectoAportante { get; set; }
        public virtual ICollection<RegistroPresupuestal> RegistroPresupuestal { get; set; }
    }
}
