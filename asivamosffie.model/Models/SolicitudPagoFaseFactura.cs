﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SolicitudPagoFaseFactura
    {
        public int SolicitudPagoFaseFacturaId { get; set; }
        public int? SolicitudPagoFaseId { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? ValorFacturado { get; set; }
        public int? Numero { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual SolicitudPagoFase SolicitudPagoFase { get; set; }
    }
}
