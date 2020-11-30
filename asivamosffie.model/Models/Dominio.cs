using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Dominio
    {
        public Dominio()
        {
            Auditoria = new HashSet<Auditoria>();
            CofinanciacionAportanteNombreAportante = new HashSet<CofinanciacionAportante>();
            CofinanciacionAportanteTipoAportante = new HashSet<CofinanciacionAportante>();
            CofinanciacionDocumento = new HashSet<CofinanciacionDocumento>();
        }

        public int DominioId { get; set; }
        public int TipoDominioId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool? Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual TipoDominio TipoDominio { get; set; }
        public virtual ICollection<Auditoria> Auditoria { get; set; }
        public virtual ICollection<CofinanciacionAportante> CofinanciacionAportanteNombreAportante { get; set; }
        public virtual ICollection<CofinanciacionAportante> CofinanciacionAportanteTipoAportante { get; set; }
        public virtual ICollection<CofinanciacionDocumento> CofinanciacionDocumento { get; set; }
    }
}
