using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccionProponente
    {
        [Key]
        public int ProcesoSeleccionProponenteId { get; set; }
        public int ProcesoSeleccionId { get; set; }
        [StringLength(100)]
        public string TipoProponenteCodigo { get; set; }
        [StringLength(1000)]
        public string NombreProponente { get; set; }
        [StringLength(100)]
        public string TipoIdentificacionCodigo { get; set; }
        [StringLength(50)]
        public string NumeroIdentificacion { get; set; }
        [StringLength(10)]
        public string LocalizacionIdMunicipio { get; set; }
        [StringLength(500)]
        public string DireccionProponente { get; set; }
        [StringLength(20)]
        public string TelefonoProponente { get; set; }
        [StringLength(100)]
        public string EmailProponente { get; set; }

        [ForeignKey(nameof(ProcesoSeleccionId))]
        [InverseProperty("ProcesoSeleccionProponente")]
        public virtual ProcesoSeleccion ProcesoSeleccion { get; set; }
    }
}
