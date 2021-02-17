﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class InformeFinalObservaciones
    {
        public int InformeFinalObservacionesId { get; set; }
        public int InformeFinalId { get; set; }
        public string Observaciones { get; set; }
        public bool? EsSupervision { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? Archivado { get; set; }
        public bool? EsApoyo { get; set; }
        public bool? EsGrupoNovedades { get; set; }
        public bool? EsGrupoNovedadesInterventoria { get; set; }

        public virtual InformeFinal InformeFinal { get; set; }
    }
}
