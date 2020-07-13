using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionIntegrante
    {
        [Key]
        public int ProcesoSeleccionIntegranteId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        public int PorcentajeParticipacion { get; set; }
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
        [InverseProperty("ProcesoSeleccionIntegrante")]
        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
    }
}
