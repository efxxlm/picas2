export interface CarguePago {
  CarguePagoId: number
  NombreArchivo: string
  JsonContent: string
  Observaciones: string
  RegistrosValidos: number
  RegistrosInvalidos: number
  TotalRegistros: number
  FechaCargue: Date
}



export interface CarguePagosRendimientos {
  CargaPagosRendimientosId: number
  NombreArchivo: string
  Json: string
  Observaciones: string
  RegistrosValidos: number
  RegistrosInvalidos: number
  TotalRegistros: number
  FechaCargue: Date
}
