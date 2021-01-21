using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalRegistroFotografico
    {
        public int SeguimientoSemanalRegistroFotograficoId { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public string UrlSoporteFotografico { get; set; }
        public string Descripcion { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool? TieneObservacionApoyo { get; set; }
        public int? ObservacionApoyoId { get; set; }
        public bool? TieneObservacionSupervisor { get; set; }
        public int? ObservacionSupervisorId { get; set; }
        public bool? RegistroCompletoObservacionSupervisor { get; set; }
        public bool? RegistroCompletoObservacionApoyo { get; set; }

        public virtual SeguimientoSemanalObservacion ObservacionApoyo { get; set; }
        public virtual SeguimientoSemanalObservacion ObservacionSupervisor { get; set; }
        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
    }
}
