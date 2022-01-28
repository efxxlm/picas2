﻿using System;
using System.Collections.Generic;
using asivamosffie.model.Models;
using asivamosffie.model.APIModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace asivamosffie.model.Models
{
    public partial class VProyectosXcontrato
    {
        [NotMapped]
        public int CantidadProyectosAsosiados { get; set; }

        [NotMapped]
        public List<VFichaProyectoContratacionProcesoSeleccion> ListProcesoSeleccion { get; set; }

        [NotMapped]
        public dynamic IntraestructuraIntervenir { get; set; }

        [NotMapped]
        public dynamic InfoProyectos { get; set; }

        [NotMapped]
        public dynamic InfoContrato { get; set; }

        [NotMapped]
        public DateTime? FechaUltimoSeguimientoDiario { get; set; }

        [NotMapped]
        public int SeguimientoDiarioId { get; set; }

        [NotMapped]
        public bool RegistroCompleto { get; set; }

        [NotMapped]
        public string EstadoCodigo { get; set; }
        
        [NotMapped]
        public string EstadoNombre { get; set; } 

        [NotMapped]
        public bool TieneAlertas { get; set; } 

        [NotMapped]
        public bool? TieneObservaciones { get; set; } 

        [NotMapped]
        public bool? RegistroCompletoTieneObservaciones { get; set; }

        [NotMapped]
        public bool MostrarBitacora { get; set; }
        [NotMapped]
        public bool CumpleCondicionesTai { get; set; }
    }
}
