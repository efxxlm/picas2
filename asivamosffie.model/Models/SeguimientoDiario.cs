using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoDiario
    {
        public SeguimientoDiario()
        {
            SeguimientoDiarioObservaciones = new HashSet<SeguimientoDiarioObservaciones>();
        }

        public int SeguimientoDiarioId { get; set; }
        public int ContratacionProyectoId { get; set; }
        public int? ObservacionSupervisorId { get; set; }
        public DateTime FechaSeguimiento { get; set; }
        public bool? DisponibilidadPersonal { get; set; }
        public string DisponibilidadPersonalObservaciones { get; set; }
        public int? CantidadPersonalProgramado { get; set; }
        public int? CantidadPersonalTrabajando { get; set; }
        public bool? SeGeneroRetrasoPersonal { get; set; }
        public int? NumeroHorasRetrasoPersonal { get; set; }
        public string DisponibilidadMaterialCodigo { get; set; }
        public string DisponibilidadMaterialObservaciones { get; set; }
        public string CausaIndisponibilidadMaterialCodigo { get; set; }
        public bool? SeGeneroRetrasoMaterial { get; set; }
        public int? NumeroHorasRetrasoMaterial { get; set; }
        public string DisponibilidadEquipoCodigo { get; set; }
        public string DisponibilidadEquipoObservaciones { get; set; }
        public string CausaIndisponibilidadEquipoCodigo { get; set; }
        public bool? SeGeneroRetrasoEquipo { get; set; }
        public int? NumeroHorasRetrasoEquipo { get; set; }
        public string ProductividadCodigo { get; set; }
        public string ProductividadObservaciones { get; set; }
        public string CausaIndisponibilidadProductividadCodigo { get; set; }
        public bool? SeGeneroRetrasoProductividad { get; set; }
        public int? NumeroHorasRetrasoProductividad { get; set; }
        public string EstadoCodigo { get; set; }
        public bool? TieneObservacionApoyo { get; set; }
        public bool? TieneObservacionSupervisor { get; set; }
        public bool? Eliminado { get; set; }
        public bool? RegistroCompleto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public int SeguimientoSemanalId { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public DateTime? FechaValidacion { get; set; }
        public bool? RegistroCompletoVerificacion { get; set; }
        public bool? RegistroCompletoValidacion { get; set; }

        public virtual ContratacionProyecto ContratacionProyecto { get; set; }
        public virtual SeguimientoDiarioObservaciones ObservacionSupervisor { get; set; }
        public virtual SeguimientoDiarioObservaciones ObservacionDevolucion { get; set; }
        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }


        public virtual ICollection<SeguimientoDiarioObservaciones> SeguimientoDiarioObservaciones { get; set; }
    }
}
