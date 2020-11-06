
using AuthorizationTest.JwtHelpers;
using asivamosffie.model.Models;
using System.Collections.Generic;

namespace asivamosffie.model.APIModels
{
    public class DisponibilidadPresupuestalGrilla
    {
        public int DisponibilidadPresupuestalId { get; set; }
        public string FechaSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public bool EstadoRegistro { get; set; }
        public string NumeroContrato { get; set; }
        public string FechaFirmaContrato { get; set; }
        public string TipoSolicitudEspecial { get; set; }
    }

    /*autor: jflorez
     descripción: objeto para entregar a front los datos ordenados de disponibilidades
     impacto: CU 3.3.3*/
    public class EstadosDisponibilidad
    {
        public int DominioId { get; set; }
        public string NombreEstado { get; set; }
        public List<DisponibilidadPresupuestalGrilla> DisponibilidadPresupuestal { get; set; }        
    }
}