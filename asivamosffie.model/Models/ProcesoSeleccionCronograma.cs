using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionCronograma
    {
        [Key]
        public int ProcesoSeleccionCronogramaId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public int NumeroActividad { get; set; }
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaMaxima { get; set; }
        [Required]
        [StringLength(100)]
        public string EstadoActividadCodigo { get; set; }
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

        [ForeignKey(nameof(ProcesoSeleccionId))]
        [InverseProperty("ProcesoSeleccionCronograma")]
        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
    }
}
