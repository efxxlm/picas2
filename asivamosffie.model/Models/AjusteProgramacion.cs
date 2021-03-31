using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class AjusteProgramacion
    {
        public AjusteProgramacion()
        {
            AjusteProgramacionFlujo = new HashSet<AjusteProgramacionFlujo>();
            AjusteProgramacionObra = new HashSet<AjusteProgramacionObra>();
        }

        public int AjusteProgramacion1 { get; set; }
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

        public virtual AjustePragramacionObservacion AjustePragramacionObservacion { get; set; }
        public virtual ICollection<AjusteProgramacionFlujo> AjusteProgramacionFlujo { get; set; }
        public virtual ICollection<AjusteProgramacionObra> AjusteProgramacionObra { get; set; }
    }
}
