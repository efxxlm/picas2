using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VGetListAportanteByTipoAportanteId
    {
        public int? TipoAportanteId { get; set; }
        public int NumeroAcuerdo { get; set; }
        public int CofinanciacionAportanteId { get; set; }
        public string Nombre { get; set; }
        public string TipoAportante { get; set; }
        public int? Vigencia { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string MunicipioId { get; set; }
        public string DepartamentoId { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? TieneFuentes { get; set; }
    }
}
