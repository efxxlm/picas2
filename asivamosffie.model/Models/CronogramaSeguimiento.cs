using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CronogramaSeguimiento
    {
        [Key]
        public int CronogramaSeguimientoId { get; set; }
        public int ProcesoSeleccionCronogramaId { get; set; }
        [Required]
        [StringLength(100)]
        public string EstadoActividadInicialCodigo { get; set; }
        [Required]
        [StringLength(100)]
        public string EstadoActividadFinalCodigo { get; set; }
        [Required]
        [StringLength(800)]
        public string Observacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaCreacion { get; set; }
        [Required]
        [StringLength(200)]
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaModificacion { get; set; }
        [StringLength(200)]
        public string UsuarioModificacion { get; set; }

        public virtual ProcesoSeleccionCronograma ProcesoSeleccionCronograma { get; set; }
    }
}
