using AuthorizationTest.JwtHelpers;
using asivamosffie.model.Models;

namespace asivamosffie.model.APIModels
{
    public class ProyectoGrilla
    {
        public int ProyectoId { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public InstitucionEducativaSede InstitucionEducativa { get; set; }
        public InstitucionEducativaSede Sede { get; set; }
        public string EstadoRegistro { get; set; }
        public string EstadoJuridicoPredios { get; set; } 
    }
}