﻿
namespace asivamosffie.services.Helpers.Constant
{
    internal class ConstantCommonMessages
    {
        #region Mensajes Informativos

        public const string APROBADA_POR = "APROBADA POR ";

        #endregion

        internal static class Performances { 
        
            public const string REGISTRAR_ORDENES_PAGOS = "REGISTRAR ORDENES DE PAGOS";
            public const string VER_DETALLE_PAGOS = "VER DETALLE ORDENES DE PAGOS";
            public const string OBSERVACIONES_PAGOS =  "REGISTRAR OBSERVACIONES PAGOS";
            public const string VER_DETALLE_RENDIMIENTOS = "VER DETALLE RENDIMIENTOS";
            public const string ELIMINAR_ORDENES_PAGOS = "ELIMINAR ORDENE DE PAGO";
            public const string REGISTRAR_RENDIMIENTOS = "REGISTRAR RENDIMIENTOS";
            public const string TRAMITAR_RENDIMIENTOS = "TRAMITAR RENDIMIENTOS";
            public const string ENVIAR_INCONSISTENCIAS = "ENVIAR INCONSISTENCIAS RENDIMIENTOS";
            public const string VER_INCONSISTENCIAS = "VER INCONSISTENCIAS RENDIMIENTOS";
            public const string VER_CONSISTENCIAS = "VER CONSISTENCIAS RENDIMIENTOS";
            public const string DESCARGAR_RESULTADO = "DESCARGAR RESULTADO RENDIMIENTOS";

            public const string NOTIFICAR_INCONSISTENCIAS = "NOTIFICAR INCONSISTENCIAS";
            public const string NOTIFICAR_SOLICITUD_APROBACION = "NOTIFICAR SOLICITUD APROBACION";
            public const string INCORPORAR_RENDIMIENTOS = "INCORPORAR_RENDIMIENTOS";
            public const string GENERAR_ACTA_RENDIMIENTOS = "GENERAR ACTA RENDIMIENTOS";
            public const string CARGAR_ACTA_RENDIMIENTOS = "CARGAR ACTA RENDIMIENTOS";
            public const string VER_RENDIMIENTOS_INCORPORADOS = "VER RENDIMIENTOS INCORPORADOS";
        }
         
        #region Mensajes Acciones ManagementCommitteeReportService
        //
        public const string REGISTRAR_AVANCE_COMPROMISOS = "REGISTRAR AVANCE COMPROMISOS";
        public const string EDITAR_AVANCE_COMPROMISOS = "EDITAR AVANCE COMPROMISOS";
        public const string COMENTAR_Y_DEVOLVER_ACTA = "COMENTAR Y DEVOLVER ACTA";
        public const string EDITAR_COMENTAR_ACTA = "EDITAR COMENTAR ACTA";
        public const string APROBAR_ACTA = "APROBAR ACTA";
        public const string CAMBIAR_ESTADO_SOLICITUD_COMPROMISO = "CAMBIAR ESTADO SOLICITUD COMPROMISO";

        #endregion
         
        public const string CREAR_EDITAR_DDP_ESPECIAL = "CREAR EDITAR DDP ESPECIAL";

        internal static class SpinOrder {
            public const string REGISTRAR_ORDENES_GIRO = "REGISTRAR ORDENES DE GIRO";
            public const string ELIMINAR_APORTANTE_ORDENES_GIRO = "ELIMINAR APORTANTE ORDEN GIRO";
            public const string CAMBIAR_ESTADO_ORDEN_GIRO = "CAMBIAR ESTADO ORDEN GIRO";
            public const string CREAR_EDITAR_OBSERVACION_ORDEN_GIRO = "CREAR EDITAR OBSERVACION ORDEN GIRO";
        }

        internal static class UpdatePolicies
        {
            public const string ELIMINAR_SEGUROS = "ELIMINAR SEGUROS";

            public const string CAMBIAR_ESTADOS_ACTUALIZAR_POLIZA = "CAMBIAR ESTADOS ACTUALIZAR POLIZA";
          
            public const string CREAR_ACTUALIZACION_POLIZA = "CREAR ACTUALIZACION POLIZAS Y GARANTIAS";

            public const string EDITAR_ACTUALIZACION_POLIZA = "EDITAR ACTUALIZACION POLIZAS Y GARANTIAS";

            public const string ELIMINAR_ACTUALIZACION_POLIZA = "ELIMINAR ACTUALIZACION POLIZAS Y GARANTIAS";
        }

        internal static class GuaranteePolicies
        {
            public const string CREAR_EDITAR = "CREAR EDITAR GARANTIAS Y POLIZAS";

            public const string ERROR = "OCURRIO UN ERROR AL PROCESAR LA SOLICITUD";

            public const string CAMBIAR_ESTADO = "CAMBIAR ESTADO POLIZAS Y GARANTIAS";
        }

        internal static class ContractSettlement
        {
            public const string CREAR_LIQUIDACION = "CREAR LIQUIDACION DE CONTRATO";
          }
    }

}
