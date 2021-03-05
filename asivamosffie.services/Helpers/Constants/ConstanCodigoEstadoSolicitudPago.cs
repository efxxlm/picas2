﻿namespace asivamosffie.services.Helpers.Constant
{
    public static class ConstanCodigoEstadoSolicitudPago
    {
        //4.1.7
        public const string En_proceso_de_registro = "1";
        public const string Con_solicitud_revisada_por_equipo_facturacion = "2";  //4.1.8
         
        public const string Con_solicitud_devuelta_por_equipo_de_facturacion = "3";
        public const string Devuelta_por_el_apoyo_supervisor = "4";
        public const string Devuelta_por_coordinacion = "7";  //4.1.9
        public const string Enviada_por_verificador_financiero_para_subsanacion = "8";//4.3.1
        public const string Enviada_por_validador_financiero_para_subsanacion = "9"; //4.3.2
        public const string Rechazada_por_el_equipo_financiero = "10"; //4.3.5 (TODAS LAS GRILLAS LAS PUEDEN VER) 
        public const string Con_solicitud_devuelta_a_facturación_por_generador_orden_de_giro = "13"; // 4.3.3
        //4.1.8
        public const string Enviada_para_autorizacion = "5";  //4.1.9

        //4.1.9
        public const string Aprobado_por_coordinacion = "6"; // 4.3.1

        //4.3.1
        public const string Enviada_para_validacion_por_financiera = "11";  //4.3.2
        public const string Con_solicitud_devuelta_a_verificar_financieramente_por_generador_orden_de_giro = "14"; //4.3.3

        //4.3.2
        public const string Enviada_para_orden_de_giro = "12";  //4.3.3  
        public const string Con_solicitud_devuelta_a_validar_financieramente_por_generador_orden_de_giro = "15"; //4.3.3 se la envia
    }
}