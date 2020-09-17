using AuthorizationTest.JwtHelpers;
using asivamosffie.model.Models;

namespace asivamosffie.model.APIModels
{
    public class ProyectoGrilla
    {
        public int ProyectoId { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string InstitucionEducativa { get; set; }
        public string Sede { get; set; }
        public string EstadoRegistro { get; set; }
        public string EstadoJuridicoPredios { get; set; }
        public string Fecha { get; set; }
        public string TipoIntervencion { get; set; }
        public string LlaveMen { get; set; }
        public string Region { get; set; } 
        public bool TieneObra { get; set; }
        public bool TieneInterventoria { get; set; }
    }
}