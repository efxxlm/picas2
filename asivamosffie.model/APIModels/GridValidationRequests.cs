using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GridValidationRequests
    {
        public int ComiteTecnicoId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public int SesionComiteSolicitudId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string TipoSolicitudText { get; set; }
        public string NumeroComite { get; set; }
        public DateTime FechaComiteTecnico { get; set; }
        public bool? TemaRequiereVotacion { get; set; }

        public List<SesionParticipanteVoto> sesionParticipanteVoto { get; set; }
    }
}
