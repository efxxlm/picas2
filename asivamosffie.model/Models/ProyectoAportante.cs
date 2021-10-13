﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ProyectoAportante
    {
        public ProyectoAportante()
        {
            ProyectoAportanteHistorico = new HashSet<ProyectoAportanteHistorico>();
        }

        public int ProyectoAportanteId { get; set; }
        public int ProyectoId { get; set; }
        public int AportanteId { get; set; }
        public decimal? ValorObra { get; set; }
        public decimal? ValorInterventoria { get; set; }
        public decimal? ValorTotalAportante { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public int? CofinanciacionDocumentoId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual CofinanciacionAportante Aportante { get; set; }
        public virtual CofinanciacionDocumento CofinanciacionDocumento { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        public virtual ICollection<ProyectoAportanteHistorico> ProyectoAportanteHistorico { get; set; }
    }
}
