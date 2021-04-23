﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class InformeFinalInterventoria
    {
        public InformeFinalInterventoria()
        {
            InformeFinalInterventoriaObservaciones = new HashSet<InformeFinalInterventoriaObservaciones>();
        }

        public int InformeFinalInterventoriaId { get; set; }
        public int InformeFinalId { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string CalificacionCodigo { get; set; }
        public int? InformeFinalAnexoId { get; set; }
        public int InformeFinalListaChequeoId { get; set; }
        public bool? TieneObservacionSupervisor { get; set; }
        public string ValidacionCodigo { get; set; }
        public string AprobacionCodigo { get; set; }
        public bool? TieneModificacionInterventor { get; set; }
        public bool? TieneModificacionApoyo { get; set; }

        public virtual InformeFinal InformeFinal { get; set; }
        public virtual InformeFinalAnexo InformeFinalAnexo { get; set; }
        public virtual ListaChequeoListaChequeoItem InformeFinalListaChequeo { get; set; }
        public virtual ICollection<InformeFinalInterventoriaObservaciones> InformeFinalInterventoriaObservaciones { get; set; }
    }
}
