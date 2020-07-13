using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asivamosffie.model.Models
{
    public partial class ProcesoSeleccion
    {
        public ProcesoSeleccion()
        {
            ProcesoSeleccionCotizacion = new HashSet<ProcesoSeleccionCotizacion>();
            ProcesoSeleccionCronograma = new HashSet<ProcesoSeleccionCronograma>();
            ProcesoSeleccionGrupo = new HashSet<ProcesoSeleccionGrupo>();
            ProcesoSeleccionIntegrante = new HashSet<ProcesoSeleccionIntegrante>();
            ProcesoSeleccionObservacion = new HashSet<ProcesoSeleccionObservacion>();
            ProcesoSeleccionProponente = new HashSet<ProcesoSeleccionProponente>();
        }

        [Key]
        public int ProcesoSeleccionId { get; set; }
        [Required]
        [StringLength(50)]
        public string NumeroProceso { get; set; }
        [Required]
        [StringLength(2000)]
        public string Objeto { get; set; }
        [Required]
        [StringLength(3000)]
        public string AlcanceParticular { get; set; }
        [Required]
        [StringLength(3000)]
        public string Justificacion { get; set; }
        [StringLength(3000)]
        public string CriteriosSeleccion { get; set; }
        [Required]
        [StringLength(100)]
        public string TipoIntervencionCodigo { get; set; }
        [Required]
        [StringLength(100)]
        public string TipoAlcanceCodigo { get; set; }
        [StringLength(100)]
        public string TipoProcesoCodigo { get; set; }
        public bool EsDistribucionGrupos { get; set; }
        public int? CantGrupos { get; set; }
        public int? ResponsableTecnicoUsuarioId { get; set; }
        public int? ResponsableEstructuradorUsuarioid { get; set; }
        [StringLength(3000)]
        public string CondicionesJuridicasHabilitantes { get; set; }
        [StringLength(3000)]
        public string CondicionesFinancierasHabilitantes { get; set; }
        [StringLength(3000)]
        public string CondicionesTecnicasHabilitantes { get; set; }
        [StringLength(3000)]
        public string CondicionesAsignacionPuntaje { get; set; }
        public int? CantidadCotizaciones { get; set; }
        public int? CantidadProponentes { get; set; }
        public bool EsCompleto { get; set; }
        [Required]
        [StringLength(100)]
        public string EstadoProcesoSeleccionCodigo { get; set; }
        [StringLength(100)]
        public string EtapaProcesoSeleccionCodigo { get; set; }
        [StringLength(3000)]
        public string EvaluacionDescripcion { get; set; }
        [StringLength(300)]
        public string UrlSoporteEvaluacion { get; set; }
        [StringLength(100)]
        public string TipoOrdenEligibilidadCodigo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaCreacion { get; set; }
        [Required]
        [StringLength(200)]
        public string UsuarioCreacion { get; set; }
        public bool? Eliminado { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaModificacion { get; set; }

        [InverseProperty("ProcesoSeleccion")]
        public virtual ICollection<ProcesoSeleccionCotizacion> ProcesoSeleccionCotizacion { get; set; }
        [InverseProperty("ProcesoSeleccion")]
        public virtual ICollection<ProcesoSeleccionCronograma> ProcesoSeleccionCronograma { get; set; }
        [InverseProperty("ProcesoSeleccion")]
        public virtual ICollection<ProcesoSeleccionGrupo> ProcesoSeleccionGrupo { get; set; }
        [InverseProperty("ProcesoSeleccion")]
        public virtual ICollection<ProcesoSeleccionIntegrante> ProcesoSeleccionIntegrante { get; set; }
        [InverseProperty("ProcesoSeleccion")]
        public virtual ICollection<ProcesoSeleccionObservacion> ProcesoSeleccionObservacion { get; set; }
        [InverseProperty("ProcesoSeleccion")]
        public virtual ICollection<ProcesoSeleccionProponente> ProcesoSeleccionProponente { get; set; }
    }
}
