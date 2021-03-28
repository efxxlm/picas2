﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class OrdenGiroDetalleEstrategiaPago
    {
        public int OrdenGiroDetalleEstrategiaPagoId { get; set; }
        public string EstrategiaPagoCodigo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public int? OrdenGiroDetalleId { get; set; }

        public virtual OrdenGiroDetalle OrdenGiroDetalle { get; set; }
    }
}
