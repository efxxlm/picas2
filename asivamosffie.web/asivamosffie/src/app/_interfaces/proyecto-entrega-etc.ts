export interface ProyectoEntregaETC {
    proyectoEntregaEtcId?: number,
    informeFinalId?: number,
    fechaRecorridoObra?: Date,
    numRepresentantesRecorrido?: number,
    fechaEntregaDocumentosEtc?: Date, 
    numRadicadoDocumentosEntregaEtc?: string,
    fechaFirmaActaBienesServicios?: Date,
    actaBienesServicios?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,
    fechaFirmaActaEngregaFisica?: Date,
    urlActaEntregaFisica?: string,
    registroCompletoActaBienesServicios?: boolean,
    registroCompletoRecorridoObra?: boolean,
    registroCompletoRemision?: boolean
}


export interface RepresentanteETCRecorrido{
    representanteETCId?:number,
    proyectoEntregaETCId?:number,
    nombre?: string,
    cargo?: string,
    dependencia?: string,
    fechaCreacion?: Date,
    usuarioCreacion?: string,
    fechaModificacion?: Date,
    usuarioModificacion?: string,

}