using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace asivamosffie.model.Models
{
    public partial class ContratoAsignado
    {
        public int ContratoId { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoAsignacionCodigo { get; set; }
    }
}
