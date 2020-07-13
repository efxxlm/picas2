using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionObservacion
    {
        [Key]
        public int ProcesoSeleccionObservacionId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        [Required]
        [StringLength(1000)]
        public string Observacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaCreacion { get; set; }
        [Required]
        [StringLength(200)]
        public string UsuarioCreacion { get; set; }

        [ForeignKey(nameof(ProcesoSeleccionId))]
        [InverseProperty("ProcesoSeleccionObservacion")]
        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
    }
}
