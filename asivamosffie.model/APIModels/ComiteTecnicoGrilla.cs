using AuthorizationTest.JwtHelpers;
using asivamosffie.model.Models;

namespace asivamosffie.model.APIModels
{
    public class ComiteTecnicoGrilla
    {
        public int ComiteTecnicoId { get; set; }
        public string FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string TipoSolicitud { get; set; } 
    }
}