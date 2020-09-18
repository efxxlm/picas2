export interface Contrato {
  contratoId: number;
  contratacionId: number;
  numeroContrato: string;
  fechaEnvioFirma?: Date;
  fechaFirmaContratista?: Date;
  fechaFirmaFiduciaria?: Date;
  fechaFirmaContrato?: Date;
  observaciones?: string;
  pFile?: any;
}