using AuthorizationTest.JwtHelpers;
using System.Collections.Generic;

namespace asivamosffie.model.APIModels
{
    public class CofinanicacionAportanteGrilla
    { 
        public int CofinanciacionAportanteId { get; set; }

        public string TipoAportante { get; set; }

        public string Nombre { get; set; }
        /*
         * autor: jflorez
         * descripción: para el listado
         * impacto: CU 3.3.2 y 3.3.3
        */
        public double ValorAportanteAlProyecto{ get; set; }

        public List<GrillaFuentesFinanciacion> FuentesFinanciacion { get; set; }
    }
}
