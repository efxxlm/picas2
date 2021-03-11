﻿using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    public class GrillaControversiaActuacionEstado
    {
        public int ActuacionId { get; set; }
        public string DescripcionActuacion { get; set; }
        public string FechaActualizacion { get; set; }
        public string NumeroActuacion { get; set; }
        public string RegistroCompletoReclamacion { get; set; }
        public string EstadoActuacion { get; set; }
        public int ControversiaContractualId { get; set; }
        public string ProximaActuacionNombre { get; set; }
        public string ProximaActuacionCodigo { get; set; }
        public string EstadoActuacionCodigo { get; set; }
        public string NumeroActuacionReclamacion { get; set; }
        public string EstadoActuacionReclamacionCodigo { get; set; }
        public string EstadoActuacionReclamacion { get; set; }
        public int ActuacionSeguimientoId { get; set; }
        public string RegistroCompletoActuacion { get; set; }

        public string NumeroMesa { get; set; }
        public string EstadoMesa { get; set; }
        public string EstadoCodigoMesa { get; set; }
        public string MesaId { get; set; }

        public string EstadoAvanceTramite { get; set; }
        public string EstadoAvanceTramiteCodigo { get; set; }
        public bool? RequiereMesaTrabajo { get; set; }
        public string RegistroCompletoMesa { get; set; }
        public string EstadoActuacionGeneral { get; set; }
        public string EstadoActuacionCodigoGeneral { get; set; }
        public bool? RequiereComite { get; set; }
        public bool? EsRequiereComiteReclamacion { get; set; }

    }
}
