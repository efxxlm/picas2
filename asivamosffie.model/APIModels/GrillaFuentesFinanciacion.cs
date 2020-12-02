using System;
using System.Collections.Generic;
using System.Text;

namespace asivamosffie.model.APIModels
{
    /*
    * autor: jflorez
    * descripción: para el listado
    * impacto: CU 3.3.2 y 3.3.3
    */
    public class GrillaFuentesFinanciacion
    {
        public int? FuenteFinanciacionID { get; set; }
        public string Fuente { get; set; }
        public decimal Saldo_actual_de_la_fuente { get; set; }
        public decimal Valor_solicitado_de_la_fuente { get; set; }
        public decimal Nuevo_saldo_de_la_fuente { get; set; }
        public string Estado_de_las_fuentes { get; set; }

        public int? GestionFuenteFinanciacionID { get; set; }
    }
}
