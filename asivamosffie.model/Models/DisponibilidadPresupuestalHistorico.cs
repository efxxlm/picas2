using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DisponibilidadPresupuestalHistorico
    {
        public int DisponibilidadPresupuestalHistoricoId { get; set; }
        public decimal ValorSolicitud { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }

        public virtual DisponibilidadPresupuestal DisponibilidadPresupuestal { get; set; }
    }
}
