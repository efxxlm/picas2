using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ControversiaMotivo
    {
        public int ControversiaMotivoId { get; set; }
        public int ControversiaContractualId { get; set; }
        public string MotivoSolicitudCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ControversiaContractual ControversiaContractual { get; set; }
    }
}
