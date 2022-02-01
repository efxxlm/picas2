using System;
using System.Collections.Generic;

namespace asivamosffie.model.Models
{
    public partial class VFichaProyectoBusquedaProyectoTabla
    {
        public int ProyectoId { get; set; }
        public string LlaveMen { get; set; }
        public string DepartamentoMunicipio { get; set; }
        public string InstitucionEducativaSede { get; set; }
        public string TipoIntervencion { get; set; }
        public string CodigoTipoIntervencion { get; set; }
        public int? Vigencia { get; set; }
    }
}
