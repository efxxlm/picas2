using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class FuenteFinanciacion
    {
        public FuenteFinanciacion()
        {
            ControlRecurso = new HashSet<ControlRecurso>();
            CuentaBancaria = new HashSet<CuentaBancaria>();
            VigenciaAporte = new HashSet<VigenciaAporte>();
        }

        public int FuenteFinanciacionId { get; set; }
        public int AportanteId { get; set; }
        public string FuenteRecursosCodigo { get; set; }
        public decimal ValorFuente { get; set; }
        public int CantVigencias { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual CofinanciacionAportante Aportante { get; set; }
        public virtual ICollection<ControlRecurso> ControlRecurso { get; set; }
        public virtual ICollection<CuentaBancaria> CuentaBancaria { get; set; }
        public virtual ICollection<VigenciaAporte> VigenciaAporte { get; set; }
    }
}
