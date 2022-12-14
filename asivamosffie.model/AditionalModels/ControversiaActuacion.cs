using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace asivamosffie.model.Models
{
    public partial class ControversiaActuacion
    {
        [NotMapped]
        public string NumeroActuacionFormat { get; set; }

        [NotMapped]
        public int ActuacionSeguimientoId { get; set; }

        [NotMapped]
        public string NumeroContrato { get; set; }

        [NotMapped]
        public string TipoControversia { get; set; }
        [NotMapped]
        public string EstadoActuacionReclamacionString { get; set; }

        [NotMapped]
        public List<string> ObservacionesComites { get; set; }

        [NotMapped]
        public List<string> ObservacionesFiduciaria { get; set; }

        [NotMapped]
        public string ProximaActuacionCodigoString { get; set; }

        [NotMapped]
        public string ActuacionAdelantadaString { get; set; }
    }
}
