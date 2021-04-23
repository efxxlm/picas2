using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ListaChequeoListaChequeoItem
    {
        public ListaChequeoListaChequeoItem()
        {
            InformeFinalInterventoria = new HashSet<InformeFinalInterventoria>();
        }

        public int ListaChequeoListaChequeoItemId { get; set; }
        public int ListaChequeoId { get; set; }
        public int ListaChequeoItemId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public int? Orden { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string Mensaje { get; set; }

        public virtual ListaChequeo ListaChequeo { get; set; }
        public virtual ListaChequeoItem ListaChequeoItem { get; set; }
        public virtual ICollection<InformeFinalInterventoria> InformeFinalInterventoria { get; set; }
    }
}
