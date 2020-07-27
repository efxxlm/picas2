
using AuthorizationTest.JwtHelpers;
using asivamosffie.model.Models;

namespace asivamosffie.model.APIModels
{
    public class DisponibilidadPresupuestalGrilla
    {
        public int DisponibilidadPresupuestalId { get; set; }
        public string FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public string EstadoRegistro { get; set; } 
    }
}