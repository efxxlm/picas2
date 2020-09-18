
namespace asivamosffie.services.Helpers.Enumerator
{
	/*autor: jflorez
	 control sobre el cambio de estados de una solicitud presupuestal //tipodominio 33
		 */

    public enum EnumeratorEstadoSolicitudPresupuestal : int 
    {
		En_validacion_presupuestal = 1,
		Devuelta_por_validacion_presupuestal = 2,
		Devuelta_por_coordinacion_financiera = 3,
		Con_validacion_presupuestal = 4,
		Con_disponibilidad_presupuestal = 5,
		Rechazada_por_validacion_presupuestal = 6,
		Con_disponibilidad_cancelada = 7

	}
}