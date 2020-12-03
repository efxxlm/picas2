using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanal
    {
        public SeguimientoSemanal()
        {
            FlujoInversion = new HashSet<FlujoInversion>();
            SeguimientoDiario = new HashSet<SeguimientoDiario>();
            SeguimientoSemanalAvanceFinanciero = new HashSet<SeguimientoSemanalAvanceFinanciero>();
            SeguimientoSemanalAvanceFisico = new HashSet<SeguimientoSemanalAvanceFisico>();
            SeguimientoSemanalGestionObra = new HashSet<SeguimientoSemanalGestionObra>();
            SeguimientoSemanalPersonalObra = new HashSet<SeguimientoSemanalPersonalObra>();
            SeguimientoSemanalRegistrarComiteObra = new HashSet<SeguimientoSemanalRegistrarComiteObra>();
            SeguimientoSemanalRegistroFotografico = new HashSet<SeguimientoSemanalRegistroFotografico>();
            SeguimientoSemanalReporteActividad = new HashSet<SeguimientoSemanalReporteActividad>();
        }

        public int SeguimientoSemanalId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int NumeroSemana { get; set; }
        public bool Eliminado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool? RegistroCompleto { get; set; }

        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual ICollection<FlujoInversion> FlujoInversion { get; set; }
        public virtual ICollection<SeguimientoDiario> SeguimientoDiario { get; set; }
        public virtual ICollection<SeguimientoSemanalAvanceFinanciero> SeguimientoSemanalAvanceFinanciero { get; set; }
        public virtual ICollection<SeguimientoSemanalAvanceFisico> SeguimientoSemanalAvanceFisico { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObra> SeguimientoSemanalGestionObra { get; set; }
        public virtual ICollection<SeguimientoSemanalPersonalObra> SeguimientoSemanalPersonalObra { get; set; }
        public virtual ICollection<SeguimientoSemanalRegistrarComiteObra> SeguimientoSemanalRegistrarComiteObra { get; set; }
        public virtual ICollection<SeguimientoSemanalRegistroFotografico> SeguimientoSemanalRegistroFotografico { get; set; }
        public virtual ICollection<SeguimientoSemanalReporteActividad> SeguimientoSemanalReporteActividad { get; set; }
    }
}
