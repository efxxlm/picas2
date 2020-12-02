using System;
using System.Collections.Generic;

namespace asivamosffie.api.Models
{
    public partial class TemporalProyecto
    {
        public int TemporalProyectoId { get; set; }
        public int ArchivoCargueId { get; set; }
        public bool EstaValidado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaSesionJunta { get; set; }
        public int NumeroActaJunta { get; set; }
        public int TipoIntervencionId { get; set; }
        public string LlaveMen { get; set; }
        public int Departamento { get; set; }
        public int Municipio { get; set; }
        public int InstitucionEducativaId { get; set; }
        public int? CodigoDaneIe { get; set; }
        public int SedeId { get; set; }
        public int? CodigoDaneSede { get; set; }
        public bool EnConvotatoria { get; set; }
        public int? ConvocatoriaId { get; set; }
        public int CantPrediosPostulados { get; set; }
        public int TipoPredioId { get; set; }
        public string UbicacionPredioPrincipalLatitud { get; set; }
        public string DireccionPredioPrincipal { get; set; }
        public int? DocumentoAcreditacionPredioId { get; set; }
        public string NumeroDocumentoAcreditacion { get; set; }
        public string CedulaCatastralPredio { get; set; }
        public int? TipoAportanteId1 { get; set; }
        public int? Aportante1 { get; set; }
        public int? TipoAportanteId2 { get; set; }
        public int? Aportante2 { get; set; }
        public int? TipoAportanteId3 { get; set; }
        public int? Aportante3 { get; set; }
        public int? VigenciaAcuerdoCofinanciacion { get; set; }
        public decimal ValorObra { get; set; }
        public decimal ValorInterventoria { get; set; }
        public decimal ValorTotal { get; set; }
        public int EspacioIntervenirId { get; set; }
        public int Cantidad { get; set; }
        public int? PlazoMesesObra { get; set; }
        public int? PlazoDiasObra { get; set; }
        public int? PlazoMesesInterventoria { get; set; }
        public int? PlazoDiasInterventoria { get; set; }
        public int? CoordinacionResponsableId { get; set; }
        public string UbicacionPredioPrincipalLontitud { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }

        public virtual ArchivoCargue ArchivoCargue { get; set; }
    }
}
