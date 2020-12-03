using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class TempFlujoInversion
    {
        public int TempFlujoInversionId { get; set; }
        public int ArchivoCargueId { get; set; }
        public bool EstaValidado { get; set; }
        public int ContratoConstruccionId { get; set; }
        public string Semana { get; set; }
        public decimal? Valor { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int? MesEjecucionId { get; set; }
        public int? ProgramacionId { get; set; }

        public virtual ArchivoCargue ArchivoCargue { get; set; }
        public virtual ContratoConstruccion ContratoConstruccion { get; set; }
    }
}
