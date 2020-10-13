using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class ConstruccionCargue
    {
        public ConstruccionCargue()
        {
            CargueObservacion = new HashSet<CargueObservacion>();
        }

        public int ConstruccionCargueId { get; set; }
        public int ContratoConstruccionId { get; set; }
        public string TipoCargueCodigo { get; set; }
        public DateTime FechaCargue { get; set; }
        public int TotalRegistros { get; set; }
        public int CantRegistrosValidos { get; set; }
        public int CantRegistrosInvalidos { get; set; }
        public string EstadoCargueCodigo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public virtual ContratoConstruccion ContratoConstruccion { get; set; }
        public virtual ICollection<CargueObservacion> CargueObservacion { get; set; }
    }
}
