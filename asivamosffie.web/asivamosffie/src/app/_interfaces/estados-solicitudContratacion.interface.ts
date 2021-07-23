export interface EstadoSolicitudContratacion {
  Sin_Registro                    :string;
  En_Revision                     :string;
  En_tramite                      :string
  Enviadas_a_la_Fiduciaria        :string
  En_proceso_de_firmas            :string
  Registrados                     :string
  Registradas_por_la_fiduciaria   :string
  Firmado                         :string
  No_aplica                       :string
  En_Firma_Del_Contratista        :string;
  Rechazado                       :string;
  AprobadoComiteTecnico           :string;
  AprobadoComiteFiduciario        :string;
  RechazadoComiteTecnico          :string;
  RechazadoComiteFiduciario       :string;
  DevueltoComiteTecnico           :string;
  DevueltoComiteFiduciario        :string;
  LiquidacionEnProcesoDeFirma     :string;
  En_firma_fiduciaria             :string;
  Liquidado                       :string;
  Sin_tramitar_ante_fiduciaria    :string;
  Rechazada_por_validacion_presupuestal    :string;
  Cancelado_por_generacion_presupuestal    :string;
  DevueltaProcesoContractual    :string;
  DevueltaLiquidacionProcesoContractual    :string;
}

export enum EstadoSolicitudContratacionCodigo {
  Sin_Registro = "1",
  En_Revision = "2",
  En_tramite = "3",
  Enviadas_a_la_Fiduciaria = "4",
  En_proceso_de_firmas = "5",
  Registrados = "6",
  Registradas_por_la_fiduciaria = "7",
  Firmado = "8",
  No_aplica = "9",
  En_Firma_Del_Contratista ="10",
  Rechazado = "11",
  AprobadoComiteTecnico = "12",
  AprobadoComiteFiduciario = "13",
  RechazadoComiteTecnico = "14",
  RechazadoComiteFiduciario = "15",
  DevueltoComiteTecnico = "16",
  DevueltoComiteFiduciario = "17",
  LiquidacionEnProcesoDeFirma = "18",
  En_firma_fiduciaria = "19",
  Liquidado = "20",
  Sin_tramitar_ante_fiduciaria = "21",
  Rechazada_por_validacion_presupuestal = "22",
  Cancelado_por_generacion_presupuestal = "23",
  DevueltaProcesoContractual = "24",
  DevueltaLiquidacionProcesoContractual = "25"

}
