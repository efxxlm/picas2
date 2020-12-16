using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class Proyecto
    {
        public Proyecto()
        {
            ContratacionProyecto = new HashSet<ContratacionProyecto>();
            ContratoConstruccion = new HashSet<ContratoConstruccion>();
            ContratoPerfil = new HashSet<ContratoPerfil>();
            DisponibilidadPresupuestalProyecto = new HashSet<DisponibilidadPresupuestalProyecto>();
            InfraestructuraIntervenirProyecto = new HashSet<InfraestructuraIntervenirProyecto>();
            ProgramacionPersonalContrato = new HashSet<ProgramacionPersonalContrato>();
            ProyectoAportante = new HashSet<ProyectoAportante>();
            ProyectoMonitoreoWeb = new HashSet<ProyectoMonitoreoWeb>();
            ProyectoPredio = new HashSet<ProyectoPredio>();
            ProyectoRequisitoTecnico = new HashSet<ProyectoRequisitoTecnico>();
        }

        public int ProyectoId { get; set; }
        public DateTime? FechaSesionJunta { get; set; }
        public int? NumeroActaJunta { get; set; }
        public string TipoIntervencionCodigo { get; set; }
        public string LlaveMen { get; set; }
        public string LocalizacionIdMunicipio { get; set; }
        public int? InstitucionEducativaId { get; set; }
        public int? SedeId { get; set; }
        public bool? EnConvocatoria { get; set; }
        public int? ConvocatoriaId { get; set; }
        public int? CantPrediosPostulados { get; set; }
        public string TipoPredioCodigo { get; set; }
        public int? PredioPrincipalId { get; set; }
        public decimal? ValorObra { get; set; }
        public decimal? ValorInterventoria { get; set; }
        public decimal? ValorTotal { get; set; }
        public string EstadoProyectoCodigo { get; set; }
        public bool? Eliminado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public string EstadoJuridicoCodigo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public bool? TieneEstadoFase1EyD { get; set; }
        public bool? TieneEstadoFase1Diagnostico { get; set; }
        public string UrlMonitoreo { get; set; }
        public string EstadoProgramacionCodigo { get; set; }
        public int? PlazoMesesObra { get; set; }
        public int? PlazoDiasObra { get; set; }
        public int? PlazoMesesInterventoria { get; set; }
        public int? PlazoDiasInterventoria { get; set; }
        public string CoordinacionResponsableCodigo { get; set; }
        public string EstadoObraProyecto { get; set; }

        public virtual InstitucionEducativaSede InstitucionEducativa { get; set; }
        public virtual Localizacion LocalizacionIdMunicipioNavigation { get; set; }
        public virtual Predio PredioPrincipal { get; set; }
        public virtual InstitucionEducativaSede Sede { get; set; }
        public virtual ICollection<ContratacionProyecto> ContratacionProyecto { get; set; }
        public virtual ICollection<ContratoConstruccion> ContratoConstruccion { get; set; }
        public virtual ICollection<ContratoPerfil> ContratoPerfil { get; set; }
        public virtual ICollection<DisponibilidadPresupuestalProyecto> DisponibilidadPresupuestalProyecto { get; set; }
        public virtual ICollection<InfraestructuraIntervenirProyecto> InfraestructuraIntervenirProyecto { get; set; }
        public virtual ICollection<ProgramacionPersonalContrato> ProgramacionPersonalContrato { get; set; }
        public virtual ICollection<ProyectoAportante> ProyectoAportante { get; set; }
        public virtual ICollection<ProyectoMonitoreoWeb> ProyectoMonitoreoWeb { get; set; }
        public virtual ICollection<ProyectoPredio> ProyectoPredio { get; set; }
        public virtual ICollection<ProyectoRequisitoTecnico> ProyectoRequisitoTecnico { get; set; }
    }
}
