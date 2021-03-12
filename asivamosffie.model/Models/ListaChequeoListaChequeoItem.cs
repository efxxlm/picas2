using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ListaChequeoListaChequeoItem
    {
        public int ListaChequeoListaChequeoItemId { get; set; }
        public int ListaChequeoId { get; set; }
        public int ListaChequeoItemId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public bool? Eliminado { get; set; }
        public string Orden { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ListaChequeo ListaChequeo { get; set; }
        public virtual ListaChequeoItem ListaChequeoItem { get; set; }
    }
}
