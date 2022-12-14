using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class GestionObraCalidadEnsayoLaboratorio
    {
        public GestionObraCalidadEnsayoLaboratorio()
        {
            EnsayoLaboratorioMuestra = new HashSet<EnsayoLaboratorioMuestra>();
        }

        public int GestionObraCalidadEnsayoLaboratorioId { get; set; }
        public int SeguimientoSemanalGestionObraCalidadId { get; set; }
        public string TipoEnsayoCodigo { get; set; }
        public int? NumeroMuestras { get; set; }
        public DateTime? FechaTomaMuestras { get; set; }
        public DateTime? FechaEntregaResultados { get; set; }
        public bool? RealizoControlMedicion { get; set; }
        public string Observacion { get; set; }
        public string UrlSoporteGestion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompletoMuestras { get; set; }
        public bool? TieneObservacionApoyo { get; set; }
        public int? ObservacionApoyoId { get; set; }
        public bool? TieneObservacionSupervisor { get; set; }
        public int? ObservacionSupervisorId { get; set; }
        public bool? RegistroCompletoObservacionApoyo { get; set; }
        public bool? RegistroCompletoObservacionSupervisor { get; set; }

        public virtual SeguimientoSemanalObservacion ObservacionApoyo { get; set; }
        public virtual SeguimientoSemanalObservacion ObservacionSupervisor { get; set; }
        public virtual SeguimientoSemanalGestionObraCalidad SeguimientoSemanalGestionObraCalidad { get; set; }
        public virtual ICollection<EnsayoLaboratorioMuestra> EnsayoLaboratorioMuestra { get; set; }
    }
}
