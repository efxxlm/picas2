import { Usuario } from '../core/_services/autenticacion/autenticacion.service';

export interface DevolverActa {
  comiteTecnicoId: number;
  fecha: Date;
  sesionComentarioId: number;
  observacion: string;
}

export interface SesionComentario{
  fecha?: Date,
  observacion?: string,
  miembroSesionParticipante?: Usuario
}