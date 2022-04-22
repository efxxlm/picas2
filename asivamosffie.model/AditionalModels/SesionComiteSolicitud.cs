using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class SesionComiteSolicitud
    {
        [NotMapped]
        public string NumeroSolicitud { get; set; }
        [NotMapped]
        public string NumeroHijo { get; set; }
        [NotMapped]
        public DateTime? FechaSolicitud { get; set; }
        [NotMapped]
        public string TipoSolicitud { get; set; }
        [NotMapped]
        public bool? RegistroCompletoActa { get; set; }
        [NotMapped]
        public string EstadoDelRegistro { get; set; }
        [NotMapped]
        public bool EstadoRegistro { get; set; }
        [NotMapped]
        public bool EstaTramitado { get; set; }
        [NotMapped]
        public ProcesoSeleccion ProcesoSeleccion { get; set; }
        [NotMapped]
        public Contratacion Contratacion { get; set; }
        [NotMapped]
        public ProcesoSeleccionMonitoreo ProcesoSeleccionMonitoreo { get; set; }
        [NotMapped]
        public ControversiaContractual ControversiaContractual { get; set; }
        [NotMapped]
        public NovedadContractual ModificacionContractual { get; set; }

        [NotMapped]
        public NovedadContractual NovedadContractual        { get; set; }

        [NotMapped]
        public ControversiaActuacion ControversiaActuacion { get; set; }
        [NotMapped]
        public DefensaJudicial DefensaJudicial { get; set; }
    }

}
