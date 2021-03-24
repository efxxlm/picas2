namespace asivamosffie.services.Helpers.Constant
{
    public enum EnumEstadoSolicitudPago : int
    {
        //Primer caso de uso solicitud de pago
        //4.1.7
        En_proceso_de_registro = 1,
        Con_solicitud_revisada_por_equipo_facturacion = 2,//Completo - lista chequeo tenga items no cumple. (botones devolver solicitud , ver detalle y eliminar)
                                                          //Completo - lista chequeo tenga items todos cumplen o no aplican, (botones enviar a verificación , ver detalle )
        Solicitud_devuelta_por_equipo_facturacion = 20, // 4.1.7
        Solicitud_devuelta_por_apoyo_a_la_supervision = 21,//4.1.8
        Solicitud_devuelta_por_coordinardor = 22,//4.1.9
        Enviada_para_subsanacion_por_verificacion_financiera = 23, //4.3.1
        Enviada_para_subsanacion_por_validaccion_financiera = 24, // 4.3.2
        Solicitud_devuelta_a_equipo_de_facturacion_por_generar_orden_de_giro = 25, //4.3.3 //Orden giro

        Solicitud_Rechazado_por_verificacion_financiera = 26, //4.3.1
        Solicitud_Rechazado_por_validacion_financiera = 27, //4.3.2

        //4.1.8 
        Enviado_para_verificacion = 3, // Sin Verificación
        En_proceso_de_verificacion = 4, //Cuando el registro esta incompleto (Ver Detalle)
                                        //Cuando el registro esta completo (Devolver Solicitud, ver detalle editar, enviar para autorizar)

        //4.1.9
        Enviada_para_autorizacion = 5,  //4.1.9
        En_proceso_de_autorizacion = 6, // mismas condiciones de 4.1.8 (botones enviar a  Verificacion financiera, ver detalle editar)

        //4.3.1
        Enviada_Verificacion_Financiera = 7,
        En_Proceso_Verificacion_Financiera = 8,// Misma Condicion del 4.1.7
                                               // Validar Boton Bool para cuando completo y no cumple los items (devolver )
                                               // Validar Boton Bool para cuando completo y  cumple los items (enviar para validación financiera)
 
        //4.3.2
        Enviada_Validacion_Financiera = 9,
        En_Proceso_Validacion_Financiera = 10,// (no cumple item)- Si tiene subsanación - (boton enviar subsanación)
                                              // (no cumple item)- No tiene subsanación - (boton rechazar) 
                                              // (si cumple) -- 
 
        //Primer caso de uso de orden de giro
        //4.3.3
        Enviada_A_Order_Giro = 11,
        En_Proceso_Generacion = 12, // (si completo (boton enviar para verificar orden de giro))
        Orden_giro_anulado = 28,// si se devuelve algo se habilita el boton
        Orden_de_giro_devuelta_por_verificacion = 29, //4.3.4
        Orden_de_giro_devuelta_por_validacion = 30,
        Orden_de_giro_devuelta_por_tramite_fiduciario = 31,
         
        //4.3.4
        Enviada_Para_Verificacion_Orden_Giro = 13,
        En_Proceso_de_Verificacion_Orden_Giro = 14, // (si existen comentarios devolver si no siguiente) se devuelve 4.3.3

        //4.3.5
        Enviada_Para_Aprobacion_Orden_Giro = 15,
        En_Proceso_de_Aprobacion_Orden_Giro = 16,

        //4.4.2
        Enviada_para_tramite_ante_fiduciaria = 17,
        En_Proceso_de_tramite_ante_fiduciaria = 18,
        Con_Orden_de_Giro_Tramitada = 19, 
    }
}