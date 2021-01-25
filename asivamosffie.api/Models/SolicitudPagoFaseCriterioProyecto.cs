﻿using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SolicitudPagoFaseCriterioProyecto
    {
        public int SolicitudPagoFaseCriterioProyectoId { get; set; }
        public int SolicitudPagoFaseCriterioId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public decimal? ValorFacturado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual SolicitudPagoFaseCriterio SolicitudPagoFaseCriterio { get; set; }
    }
}
