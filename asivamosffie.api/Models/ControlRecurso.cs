using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class ControlRecurso
    {
        public int ControlRecursoId { get; set; }
        public int FuenteFinanciacionId { get; set; }
        public int CuentaBancariaId { get; set; }
        public int? RegistroPresupuestalId { get; set; }
        public int VigenciaAporteId { get; set; }
        public DateTime FechaConsignacion { get; set; }
        public decimal ValorConsignacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }

        public virtual CuentaBancaria CuentaBancaria { get; set; }
        public virtual FuenteFinanciacion FuenteFinanciacion { get; set; }
        public virtual RegistroPresupuestal RegistroPresupuestal { get; set; }
    }
}
