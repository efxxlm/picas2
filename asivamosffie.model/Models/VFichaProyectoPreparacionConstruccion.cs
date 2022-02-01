using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoPreparacionConstruccion
    {
        public int ContratoConstruccionId { get; set; }
        public int ProyectoId { get; set; }
        public int ContratoId { get; set; }
        public int ContratacionId { get; set; }
        public int ConstruccionPerfilId { get; set; }
        public string NombreTipoContrato { get; set; }
        public string CodigoTipoContrato { get; set; }
        public string Perfil { get; set; }
        public int? CantidadHvRequeridas { get; set; }
        public int? CantidadHvRecibidas { get; set; }
        public int? CantidadHvAprobadas { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string Observaciones { get; set; }
        public string RutaSoporte { get; set; }
    }
}
