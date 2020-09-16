using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GridValidationRequests
    {
        public int ComiteTecnicoId { get; set; }
        public int SesionComiteSolicitudId { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public string TipoSolicitudText { get; set; }
        public string NumeroComite { get; set; }
        public DateTime FechaComiteTecnico { get; set; }
        public bool? TemaRequiereVotacion { get; set; }
    }
}
