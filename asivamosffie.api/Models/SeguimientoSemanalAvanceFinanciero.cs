﻿using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class SeguimientoSemanalAvanceFinanciero
    {
        public int SeguimientoSemanalAvanceFinancieroId { get; set; }
        public int SeguimientoSemanalId { get; set; }

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
    }
}