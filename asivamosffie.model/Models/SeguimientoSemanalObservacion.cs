using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalObservacion
    {
        public SeguimientoSemanalObservacion()
        {
            EnsayoLaboratorioMuestraObservacionApoyo = new HashSet<EnsayoLaboratorioMuestra>();
            EnsayoLaboratorioMuestraObservacionSupervisor = new HashSet<EnsayoLaboratorioMuestra>();
            GestionObraCalidadEnsayoLaboratorioObservacionApoyo = new HashSet<GestionObraCalidadEnsayoLaboratorio>();
            GestionObraCalidadEnsayoLaboratorioObservacionSupervisor = new HashSet<GestionObraCalidadEnsayoLaboratorio>();
            ManejoMaterialesInsumosObservacionApoyo = new HashSet<ManejoMaterialesInsumos>();
            ManejoMaterialesInsumosObservacionSupervisor = new HashSet<ManejoMaterialesInsumos>();
            ManejoOtroObservacionApoyo = new HashSet<ManejoOtro>();
            ManejoOtroObservacionSupervisor = new HashSet<ManejoOtro>();
            ManejoResiduosConstruccionDemolicionObservacionApoyo = new HashSet<ManejoResiduosConstruccionDemolicion>();
            ManejoResiduosConstruccionDemolicionObservacionSupervisor = new HashSet<ManejoResiduosConstruccionDemolicion>();
            ManejoResiduosPeligrososEspecialesObservacionApoyo = new HashSet<ManejoResiduosPeligrososEspeciales>();
            ManejoResiduosPeligrososEspecialesObservacionSupervisor = new HashSet<ManejoResiduosPeligrososEspeciales>();
            SeguimientoSemanalAvanceFinancieroObservacionApoyo = new HashSet<SeguimientoSemanalAvanceFinanciero>();
            SeguimientoSemanalAvanceFinancieroObservacionSupervisor = new HashSet<SeguimientoSemanalAvanceFinanciero>();
            SeguimientoSemanalAvanceFisicoObservacionApoyo = new HashSet<SeguimientoSemanalAvanceFisico>();
            SeguimientoSemanalAvanceFisicoObservacionSupervisor = new HashSet<SeguimientoSemanalAvanceFisico>();
            SeguimientoSemanalGestionObraAlertaObservacionApoyo = new HashSet<SeguimientoSemanalGestionObraAlerta>();
            SeguimientoSemanalGestionObraAlertaObservacionSupervisor = new HashSet<SeguimientoSemanalGestionObraAlerta>();
            SeguimientoSemanalGestionObraAmbientalObservacionApoyo = new HashSet<SeguimientoSemanalGestionObraAmbiental>();
            SeguimientoSemanalGestionObraAmbientalObservacionSupervisor = new HashSet<SeguimientoSemanalGestionObraAmbiental>();
            SeguimientoSemanalGestionObraCalidadObservacionApoyo = new HashSet<SeguimientoSemanalGestionObraCalidad>();
            SeguimientoSemanalGestionObraCalidadObservacionSupervisor = new HashSet<SeguimientoSemanalGestionObraCalidad>();
            SeguimientoSemanalGestionObraSeguridadSaludObservacionApoyo = new HashSet<SeguimientoSemanalGestionObraSeguridadSalud>();
            SeguimientoSemanalGestionObraSeguridadSaludObservacionSupervisor = new HashSet<SeguimientoSemanalGestionObraSeguridadSalud>();
            SeguimientoSemanalGestionObraSocialObservacionApoyo = new HashSet<SeguimientoSemanalGestionObraSocial>();
            SeguimientoSemanalGestionObraSocialObservacionSupervisor = new HashSet<SeguimientoSemanalGestionObraSocial>();
            SeguimientoSemanalRegistrarComiteObraObservacionApoyo = new HashSet<SeguimientoSemanalRegistrarComiteObra>();
            SeguimientoSemanalRegistrarComiteObraObservacionSupervisor = new HashSet<SeguimientoSemanalRegistrarComiteObra>();
            SeguimientoSemanalRegistroFotograficoObservacionApoyo = new HashSet<SeguimientoSemanalRegistroFotografico>();
            SeguimientoSemanalRegistroFotograficoObservacionSupervisor = new HashSet<SeguimientoSemanalRegistroFotografico>();
            SeguimientoSemanalReporteActividadObservacionApoyoIdActividadNavigation = new HashSet<SeguimientoSemanalReporteActividad>();
            SeguimientoSemanalReporteActividadObservacionApoyoIdActividadSiguienteNavigation = new HashSet<SeguimientoSemanalReporteActividad>();
            SeguimientoSemanalReporteActividadObservacionApoyoIdEstadoContratoNavigation = new HashSet<SeguimientoSemanalReporteActividad>();
            SeguimientoSemanalReporteActividadObservacionSupervisorIdActividadNavigation = new HashSet<SeguimientoSemanalReporteActividad>();
            SeguimientoSemanalReporteActividadObservacionSupervisorIdActividadSiguienteNavigation = new HashSet<SeguimientoSemanalReporteActividad>();
            SeguimientoSemanalReporteActividadObservacionSupervisorIdEstadoContratoNavigation = new HashSet<SeguimientoSemanalReporteActividad>();
        }

        public int SeguimientoSemanalObservacionId { get; set; }
        public int? SeguimientoSemanalId { get; set; }
        public string TipoObservacionCodigo { get; set; }
        public int ObservacionPadreId { get; set; }
        public string Observacion { get; set; }
        public bool EsSupervisor { get; set; }
        public bool Archivada { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual SeguimientoSemanal SeguimientoSemanal { get; set; }
        public virtual ICollection<EnsayoLaboratorioMuestra> EnsayoLaboratorioMuestraObservacionApoyo { get; set; }
        public virtual ICollection<EnsayoLaboratorioMuestra> EnsayoLaboratorioMuestraObservacionSupervisor { get; set; }
        public virtual ICollection<GestionObraCalidadEnsayoLaboratorio> GestionObraCalidadEnsayoLaboratorioObservacionApoyo { get; set; }
        public virtual ICollection<GestionObraCalidadEnsayoLaboratorio> GestionObraCalidadEnsayoLaboratorioObservacionSupervisor { get; set; }
        public virtual ICollection<ManejoMaterialesInsumos> ManejoMaterialesInsumosObservacionApoyo { get; set; }
        public virtual ICollection<ManejoMaterialesInsumos> ManejoMaterialesInsumosObservacionSupervisor { get; set; }
        public virtual ICollection<ManejoOtro> ManejoOtroObservacionApoyo { get; set; }
        public virtual ICollection<ManejoOtro> ManejoOtroObservacionSupervisor { get; set; }
        public virtual ICollection<ManejoResiduosConstruccionDemolicion> ManejoResiduosConstruccionDemolicionObservacionApoyo { get; set; }
        public virtual ICollection<ManejoResiduosConstruccionDemolicion> ManejoResiduosConstruccionDemolicionObservacionSupervisor { get; set; }
        public virtual ICollection<ManejoResiduosPeligrososEspeciales> ManejoResiduosPeligrososEspecialesObservacionApoyo { get; set; }
        public virtual ICollection<ManejoResiduosPeligrososEspeciales> ManejoResiduosPeligrososEspecialesObservacionSupervisor { get; set; }
        public virtual ICollection<SeguimientoSemanalAvanceFinanciero> SeguimientoSemanalAvanceFinancieroObservacionApoyo { get; set; }
        public virtual ICollection<SeguimientoSemanalAvanceFinanciero> SeguimientoSemanalAvanceFinancieroObservacionSupervisor { get; set; }
        public virtual ICollection<SeguimientoSemanalAvanceFisico> SeguimientoSemanalAvanceFisicoObservacionApoyo { get; set; }
        public virtual ICollection<SeguimientoSemanalAvanceFisico> SeguimientoSemanalAvanceFisicoObservacionSupervisor { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraAlerta> SeguimientoSemanalGestionObraAlertaObservacionApoyo { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraAlerta> SeguimientoSemanalGestionObraAlertaObservacionSupervisor { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraAmbiental> SeguimientoSemanalGestionObraAmbientalObservacionApoyo { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraAmbiental> SeguimientoSemanalGestionObraAmbientalObservacionSupervisor { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraCalidad> SeguimientoSemanalGestionObraCalidadObservacionApoyo { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraCalidad> SeguimientoSemanalGestionObraCalidadObservacionSupervisor { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraSeguridadSalud> SeguimientoSemanalGestionObraSeguridadSaludObservacionApoyo { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraSeguridadSalud> SeguimientoSemanalGestionObraSeguridadSaludObservacionSupervisor { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraSocial> SeguimientoSemanalGestionObraSocialObservacionApoyo { get; set; }
        public virtual ICollection<SeguimientoSemanalGestionObraSocial> SeguimientoSemanalGestionObraSocialObservacionSupervisor { get; set; }
        public virtual ICollection<SeguimientoSemanalRegistrarComiteObra> SeguimientoSemanalRegistrarComiteObraObservacionApoyo { get; set; }
        public virtual ICollection<SeguimientoSemanalRegistrarComiteObra> SeguimientoSemanalRegistrarComiteObraObservacionSupervisor { get; set; }
        public virtual ICollection<SeguimientoSemanalRegistroFotografico> SeguimientoSemanalRegistroFotograficoObservacionApoyo { get; set; }
        public virtual ICollection<SeguimientoSemanalRegistroFotografico> SeguimientoSemanalRegistroFotograficoObservacionSupervisor { get; set; }
        public virtual ICollection<SeguimientoSemanalReporteActividad> SeguimientoSemanalReporteActividadObservacionApoyoIdActividadNavigation { get; set; }
        public virtual ICollection<SeguimientoSemanalReporteActividad> SeguimientoSemanalReporteActividadObservacionApoyoIdActividadSiguienteNavigation { get; set; }
        public virtual ICollection<SeguimientoSemanalReporteActividad> SeguimientoSemanalReporteActividadObservacionApoyoIdEstadoContratoNavigation { get; set; }
        public virtual ICollection<SeguimientoSemanalReporteActividad> SeguimientoSemanalReporteActividadObservacionSupervisorIdActividadNavigation { get; set; }
        public virtual ICollection<SeguimientoSemanalReporteActividad> SeguimientoSemanalReporteActividadObservacionSupervisorIdActividadSiguienteNavigation { get; set; }
        public virtual ICollection<SeguimientoSemanalReporteActividad> SeguimientoSemanalReporteActividadObservacionSupervisorIdEstadoContratoNavigation { get; set; }
    }
}
