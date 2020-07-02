﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CofinanciacionDocumento
    {
        public int CofinanciacionDocumentoId { get; set; }
        public int CofinanciacionAportanteId { get; set; }
        public int VigenciaAporteId { get; set; }
        public decimal ValorDocumento { get; set; }
        public int TipoDocumentoId { get; set; }
        public int? NumeroActa { get; set; }
        public DateTime? FechaActa { get; set; }
        public int? NumeroAcuerdo { get; set; }
        public DateTime? FechaAcuerdo { get; set; }
        public decimal? ValorTotalAportante { get; set; }
        public bool Eliminado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual CofinanciacionAportante CofinanciacionAportante { get; set; }
    }
}
