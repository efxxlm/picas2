using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class CarguePagosRendimientos
    {
        public int CargaPagosRendimientosId { get; set; }
        public string NombreArchivo { get; set; }
        public string Json { get; set; }
        public string Observaciones { get; set; }
        public int TotalRegistros { get; set; }
        public int RegistrosValidos { get; set; }
        public int RegistrosInvalidos { get; set; }
        public string EstadoCargue { get; set; }
        public string TipoCargue { get; set; }
        public DateTime FechaCargue { get; set; }
        public DateTime? FechaTramite { get; set; }
        public string TramiteJson { get; set; }
        public int RegistrosConsistentes { get; set; }
        public int RegistrosInconsistentes { get; set; }
        public bool PendienteAprobacion { get; set; }
        public bool MostrarInconsistencias { get; set; }
        public bool Eliminado { get; set; }
        public bool CargueValido { get; set; }
    }
}
