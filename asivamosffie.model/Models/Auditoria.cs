using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class Auditoria
    {
        public int AuditoriaId { get; set; }
        public string UsuarioId { get; set; }
        public int AccionId { get; set; }
        public int MensajesValidacionesId { get; set; }
        public DateTime? Fecha { get; set; }
        public string Observacion { get; set; }

        public virtual Dominio Accion { get; set; }
        public virtual MensajesValidaciones MensajesValidaciones { get; set; }
    }
}
