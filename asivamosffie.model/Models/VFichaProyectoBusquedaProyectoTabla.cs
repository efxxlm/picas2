using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoBusquedaProyectoTabla
    {
        public int? ContratacionProyectoId { get; set; }
        public int ProyectoId { get; set; }
        public string LlaveMen { get; set; }
        public string DepartamentoMunicipio { get; set; }
        public string InstitucionEducativaSede { get; set; }
        public string CodigoTipoContrato { get; set; }
        public string NombreTipoContrato { get; set; }
        public int? Vigencia { get; set; }
    }
}
