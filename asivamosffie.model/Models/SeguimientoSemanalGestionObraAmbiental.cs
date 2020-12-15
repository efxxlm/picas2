using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class SeguimientoSemanalGestionObraAmbiental
    {
        public int SeguimientoSemanalGestionObraAmbientalId { get; set; }
        public int SeguimientoSemanalGestionObraId { get; set; }
        public bool? SeEjecutoGestionAmbiental { get; set; }
        public int? ManejoMaterialesInsumoId { get; set; }
        public int? ManejoResiduosPeligrososEspecialesId { get; set; }
        public int? ManejoResiduosConstruccionDemolicionId { get; set; }
        public int? ManejoOtroId { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool? Eliminado { get; set; }
        public bool TieneManejoMaterialesInsumo { get; set; }
        public bool TieneManejoResiduosPeligrososEspeciales { get; set; }
        public bool TieneManejoResiduosConstruccionDemolicion { get; set; }
        public bool TieneManejoOtro { get; set; }

        public virtual ManejoMaterialesInsumos ManejoMaterialesInsumo { get; set; }
        public virtual ManejoOtro ManejoOtro { get; set; }
        public virtual ManejoResiduosConstruccionDemolicion ManejoResiduosConstruccionDemolicion { get; set; }
        public virtual ManejoResiduosPeligrososEspeciales ManejoResiduosPeligrososEspeciales { get; set; }
        public virtual SeguimientoSemanalGestionObra SeguimientoSemanalGestionObra { get; set; }
    }
}
