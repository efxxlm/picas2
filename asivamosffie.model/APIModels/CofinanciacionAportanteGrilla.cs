using AuthorizationTest.JwtHelpers;
using System;
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
         * impacto: CU 3.3.2, 3.3.3 y 3.1.2
        */
        public double ValorAportanteAlProyecto{ get; set; }

        public List<GrillaFuentesFinanciacion> FuentesFinanciacion { get; set; }
        public int? Vigencia { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string MunicipioId { get; set; }
        public string DepartamentoId { get; set; }
        public bool? RegistroCompleto { get; set; }
    }
}
