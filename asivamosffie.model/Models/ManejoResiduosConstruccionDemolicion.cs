﻿using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ManejoResiduosConstruccionDemolicion
    {
        public ManejoResiduosConstruccionDemolicion()
        {
            ManejoResiduosConstruccionDemolicionGestor = new HashSet<ManejoResiduosConstruccionDemolicionGestor>();
            SeguimientoSemanalGestionObraAmbiental = new HashSet<SeguimientoSemanalGestionObraAmbiental>();
        }

        public int ManejoResiduosConstruccionDemolicionId { get; set; }
        public bool? EstaCuantificadoRcd { get; set; }
        public bool? RequiereObservacion { get; set; }
        public string Observacion { get; set; }
        public bool? SeReutilizadorResiduos { get; set; }
        public int? CantidadToneladas { get; set; }
        public bool RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Eliminado { get; set; }
        public bool? TieneObservacionApoyo { get; set; }
        public int? ObservacionApoyoId { get; set; }
        public bool? TieneObservacionSupervisor { get; set; }
        public int? ObservacionSupervisorId { get; set; }
        public bool? RegistroCompletoObservacionApoyo { get; set; }
        public bool? RegistroCompletoObservacionSupervisor { get; set; }

        public virtual SeguimientoSemanalObservacion ObservacionApoyo { get; set; }
        public virtual SeguimientoSemanalObservacion ObservacionSupervisor { get; set; }
        public virtual ICollection<ManejoResiduosConstruccionDemolicionGestor> ManejoResiduosConstruccionDemolicionGestor { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraAmbiental> SeguimientoSemanalGestionObraAmbiental { get; set; }
    }
}
