using AuthorizationTest.JwtHelpers;
using asivamosffie.model.Models;
using System.Collections.Generic;

namespace asivamosffie.model.APIModels
{
    public class ProyectoGrilla
    {
        public List<GrillaComponentes> ComponenteGrilla;

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
        public string EstadoProyectoObra { get; set; }
        public string EstadoProyectoInterventoria { get; set; }
        public string EstadoProyectoObraCodigo { get; set; }

        public string EstadoProyectoInterventoriaCodigo { get; set; }
        public bool TieneObra { get; set; }
        public bool TieneInterventoria { get; set; }
        public string NombreAportante { get; set; }
        public decimal? ValorAportante { get; set; }
        public int AportanteID { get; set; }
        public int DisponibilidadPresupuestalProyecto { get; set; }
        public decimal ValorGestionado { get; set; } 
        public string EstadoProgramacion { get; set; }
        public string NumeroContrato { get; set; }     

        public string  URLMonitoreo { get; set; }
        public int? ContratoId { get; set; }
        public List<CofinanicacionAportanteGrilla> Aportantes { get; set; }
        public string CodigoDane { get; set; }
        public string SedeCodigo { get; set; }
        public string NombreContratista { get; set; }
        /*4.2.2*/
        public int ContratacionProyectoId { get; set; } 
        public int? ContratacionId { get; set; }
    }
}