using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class DisponibilidadPresupuestalObservacion
    {
        public int DisponibilidadPresupuestalObservacionId { get; set; }
        public int DisponibilidadPresupuestalId { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string EstadoSolicitudCodigo { get; set; }
        public bool? EsNovedad { get; set; }
        public int? NovedadContractualRegistroPresupuestalId { get; set; }

        public virtual DisponibilidadPresupuestal DisponibilidadPresupuestal { get; set; }
        public virtual NovedadContractualRegistroPresupuestal NovedadContractualRegistroPresupuestal { get; set; }
    }
}
