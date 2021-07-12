﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoFaseCriterio
    {
        public SolicitudPagoFaseCriterio()
        {
            SolicitudPagoFaseCriterioConceptoPago = new HashSet<SolicitudPagoFaseCriterioConceptoPago>();
        }

        public int SolicitudPagoFaseCriterioId { get; set; }
        public string TipoPagoCodigo { get; set; }
        public decimal? ValorFacturado { get; set; }
        public int SolicitudPagoFaseId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string TipoCriterioCodigo { get; set; }
        public bool? RegistroCompletoSupervisor { get; set; }
        public bool? RegistroCompletoCoordinador { get; set; }

        public virtual SolicitudPagoFase SolicitudPagoFase { get; set; }
        public virtual ICollection<SolicitudPagoFaseCriterioConceptoPago> SolicitudPagoFaseCriterioConceptoPago { get; set; }
    }
}
