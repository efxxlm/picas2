using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class RepresentanteEtcrecorrido
    {
        public int RepresentanteEtcid { get; set; }
        public int ProyectoEntregaEtcid { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public string Dependencia { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ProyectoEntregaEtc ProyectoEntregaEtc { get; set; }
    }
}
