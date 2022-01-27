using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VProyectosXcontrato
    {
        public string ActaSuscrita { get; set; }
        public string TipoContratoCodigo { get; set; }
        public string NombreTipoContrato { get; set; }
        public string LlaveMen { get; set; }
        public DateTime? FechaRegistroProyecto { get; set; }
        public string NumeroContrato { get; set; }
        public string TipoIntervencion { get; set; }
        public string InstitucionEducativa { get; set; }
        public string Sede { get; set; }
        public string Municipio { get; set; }
        public string Departamento { get; set; }
        public string Region { get; set; }
        public int ContratoId { get; set; }
        public int ProyectoId { get; set; }
        public int ContratacionId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public DateTime? FechaActaInicioFase2 { get; set; }
        public string EstadoActaFase2 { get; set; }
        public decimal? ValorTotal { get; set; }
        public string TipoSolicitudCodigo { get; set; }
        public int? InterventorId { get; set; }
        public int? ApoyoId { get; set; }
        public int? SupervisorId { get; set; }
        public string NombreContratista { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string RepresentanteLegal { get; set; }
        public string NumeroInvitacion { get; set; }
    }
}
