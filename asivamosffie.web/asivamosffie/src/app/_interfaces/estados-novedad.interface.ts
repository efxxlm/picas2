export interface TipoNovedad {
    suspension: string;
    prorroga_a_las_Suspension: string;
    adicion: string;
    prorroga: string;
    modificacion_de_Condiciones_Contractuales: string;
    reinicio: string;
}

export enum TipoNovedadCodigo {
    suspension = '1',
    prorroga_a_las_Suspension = '2',
    adicion = '3',
    prorroga = '4',
    modificacion_de_Condiciones_Contractuales = '5',
    reinicio = '6',
}

export interface EstadoSolicitudNovedad {
		En_proceso_de_registro                        :string;
		Con_novedad_aprobada_por_interventor          :string;
		En_proceso_de_verificacion                    :string;
		Sin_validar                                   :string;
		En_proceso_de_validacion                      :string;
		Con_novedad_validada_y_en_trámite             :string;
		Con_novedad_rechazada_por_supervisor          :string;
		Con_observaciones_del_equipo_de_seguimiento   :string;
		Con_novedad_rechazada_por_interventor         :string;
		Sin_tramite                                   :string;
		En_proceso_de_aprobacion                      :string;
		Con_novedad_aprobada_tecnica_y_juridicamente  :string;
		Con_novedad_rechazada                         :string;
		Novedad_Cancelada                             :string;
		Enviada_a_comite_tecnico                      :string;
		Devuelta_para_ajustes_de_supervisión          :string;
		En_registro_de_firmas                         :string;
		Con_observaciones_del_supervisor              :string;
		aprobado_por_comite_tecnico                   :string;
		devuelto_por_comite_tecnico                   :string;
		rechazado_por_comite_tecnico                  :string;
		devuelto_por_comite_fiduciario                :string;
		rechazado_por_comite_fiduciario               :string;
		Enviadas_a_la_Fiduciaria                      :string;
		Firmado                                       :string;
		Registrado                                    :string;
		Aprobada_para_envio_comite                    :string;
		DevueltaProcesoContractual                    :string;
}

export enum EstadoSolicitudNovedadCodigo {
  En_proceso_de_registro                        = "1",
  Con_novedad_aprobada_por_interventor          = "2",
  En_proceso_de_verificacion                    = "3",
  Sin_validar                                   = "4",
  En_proceso_de_validacion                      = "5",
  Con_novedad_validada_y_en_trámite             = "6",
  Con_novedad_rechazada_por_supervisor          = "7",
  Con_observaciones_del_equipo_de_seguimiento   = "8",
  Con_novedad_rechazada_por_interventor         = "9",
  Sin_tramite                                   = "10",
  En_proceso_de_aprobacion                      = "11",
  Con_novedad_aprobada_tecnica_y_juridicamente  = "12",
  Con_novedad_rechazada                         = "13",
  Novedad_Cancelada                             = "14",
  Enviada_a_comite_tecnico                      = "15",
  Devuelta_para_ajustes_de_supervisión          = "16",
  En_registro_de_firmas                         = "17",
  Con_observaciones_del_supervisor              = "18",
  aprobado_por_comite_tecnico                   = "19",
  devuelto_por_comite_tecnico                   = "20",
  rechazado_por_comite_tecnico                  = "21",
  devuelto_por_comite_fiduciario                = "22",
  rechazado_por_comite_fiduciario               = "23",
  Enviadas_a_la_Fiduciaria                      = "24",
  Firmado                                       = "25",
  Registrado                                    = "26",
  Aprobada_para_envio_comite                    = "27",
  DevueltaProcesoContractual                    = "28"
}
