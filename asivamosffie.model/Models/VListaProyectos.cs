﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VListaProyectos
    {
        public int ProyectoId { get; set; }
        public string Municipio { get; set; }
        public string Departamento { get; set; }
        public string InstitucionEducativa { get; set; }
        public string Sede { get; set; }
        public string EstadoJuridicoPredios { get; set; }
        public string TipoIntervencion { get; set; }
        public string EstadoProyectoObra { get; set; }
        public string EstadoProyectoInterventoria { get; set; }
        public DateTime? Fecha { get; set; }
        public string EstadoRegistro { get; set; }
        public string CodigoEstadoProyectoObra { get; set; }
        public string CodigoEstadoProyectoInterventoria { get; set; }
    }
}
