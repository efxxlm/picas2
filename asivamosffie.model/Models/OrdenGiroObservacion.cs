﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroObservacion
    {
        public OrdenGiroObservacion()
        {
            OrdenGiroDetalle = new HashSet<OrdenGiroDetalle>();
        }

        public int OrdenGiroObservacionId { get; set; }
        public string Observacion { get; set; }
        public bool RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<OrdenGiroDetalle> OrdenGiroDetalle { get; set; }
    }
}
