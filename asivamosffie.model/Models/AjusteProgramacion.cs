using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class AjusteProgramacion
    {
        public AjusteProgramacion()
        {
            AjustePragramacionObservacion = new HashSet<AjustePragramacionObservacion>();
            AjusteProgramacionFlujo = new HashSet<AjusteProgramacionFlujo>();
            AjusteProgramacionObra = new HashSet<AjusteProgramacionObra>();
            SeguimientoSemanalTemp = new HashSet<SeguimientoSemanalTemp>();
        }

        public int AjusteProgramacionId { get; set; }
        public int? NovedadContractualId { get; set; }
        public int? ContratacionProyectoId { get; set; }
        public string EstadoCodigo { get; set; }
        public int? ArchivoCargueIdProgramacionObra { get; set; }
        public int? ArchivoCargueIdFlujoInversion { get; set; }
        public bool? TieneObservacionesProgramacionObra { get; set; }
        public bool? TieneObservacionesFlujoInversion { get; set; }
        public int? ObservacionDevolucionIdProgramacionObra { get; set; }
        public int? ObservacionDevolucionFlujoInversion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? RegistroCompletoValidacion { get; set; }

        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual NovedadContractual NovedadContractual { get; set; }
        public virtual ICollection<AjustePragramacionObservacion> AjustePragramacionObservacion { get; set; }
        public virtual ICollection<AjusteProgramacionFlujo> AjusteProgramacionFlujo { get; set; }
        public virtual ICollection<AjusteProgramacionObra> AjusteProgramacionObra { get; set; }
        public virtual ICollection<SeguimientoSemanalTemp> SeguimientoSemanalTemp { get; set; }
    }
}
