using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class NovedadContractualRegistroPresupuestalHistorico
    {
        public int NovedadContractualRegistroPresupuestalHistoricoId { get; set; }
        public int NovedadContractualRegistroPresupuestalId { get; set; }
        public decimal ValorSolicitud { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual NovedadContractualRegistroPresupuestal NovedadContractualRegistroPresupuestal { get; set; }
    }
}
