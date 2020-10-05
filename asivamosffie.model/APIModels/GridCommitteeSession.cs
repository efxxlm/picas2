using asivamosffie.model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GridCommitteeSession
    {
        public int? ComiteTecnicoId { get; set; }
        public int SesionComiteTemaId { get; set; }
        public DateTime? FechaDeComite { get; set; }
        public string Tema { get; set; }
        public int? TiempoIntervencion { get; set; }
        public string UrlSoporte { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesDecision { get; set; }
        public string NumeroComite { get; set; }
        public string  Responzable { get; set; }
        public string EstadoComiteCodigo { get; set; }
        public string EstadoComiteText { get; set; }
        public  ICollection<SesionComiteTema> SesionComiteTemaList { get; set; }
    }
}
