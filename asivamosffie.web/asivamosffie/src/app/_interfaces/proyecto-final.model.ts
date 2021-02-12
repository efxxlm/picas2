export interface Report {
    proyecto: {
        informeFinal: [
            {
                informeFinalId: number,
                contratacionProyectoId: number,
                eliminado: boolean
                estadoInforme: string,
                fechaCreacion: string,
                informeFinalInterventoria: [],
                registroCompleto: boolean,
                usuarioCreacion: string,
                urlActa: string,
                fechaSuscripcion: string
                informeFinalObservaciones: InformeFinalObservaciones[]
            }
        ],
        departamentoObj: {
            localizacionId: string,
            descripcion:string,
            idPadre: string,
            nivel: number,
            tipo: string,
            cofinanciacionAportanteDepartamento: [],
            cofinanciacionAportanteMunicipio: [],
            proyecto: []
        },
        municipioObj: {
            localizacionId: string,
            descripcion: string,
            idPadre: string,
            nivel: number,
            tipo: string,
            cofinanciacionAportanteDepartamento: [],
            cofinanciacionAportanteMunicipio: [],
            proyecto: []
        },
        tipoIntervencionString: string,
        fechaInicioEtapaObra: string,
        fechaFinEtapaObra: string,
        plazoEnSemanas: number,
        proyectoId: number,
        fechaSesionJunta: string,
        numeroActaJunta: number,
        tipoIntervencionCodigo: string,
        llaveMen: string,
        localizacionIdMunicipio: string,
        institucionEducativaId: number,
        sedeId: number,
        enConvocatoria: boolean,
        cantPrediosPostulados: number,
        tipoPredioCodigo: string,
        predioPrincipalId: number,
        valorObra: number,
        valorInterventoria: number,
        valorTotal: number,
        estadoProyectoObraCodigo: string,
        estadoProyectoInterventoriaCodigo: string,
        eliminado: boolean,
        fechaCreacion: string,
        usuarioCreacion: string,
        fechaModificacion: string,
        usuarioModificacion: string,
        estadoJuridicoCodigo: string,
        registroCompleto: boolean,
        plazoMesesObra: number,
        plazoDiasObra: number,
        plazoMesesInterventoria: number,
        plazoDiasInterventoria: number,
        coordinacionResponsableCodigo: string,
        institucionEducativa: {
            institucionEducativaSedeId: number,
            nombre: string,
            codigoDane: string,
            localizacionIdMunicipio: string,
            fechaCreacion: string,
            usuarioCreacion: string,
            activo: boolean,
            predio: [],
            proyectoInstitucionEducativa: [],
            proyectoSede: []
        },
        sede: {
            institucionEducativaSedeId: number,
            padreId: number,
            nombre: string,
            localizacionIdMunicipio: string,
            fechaCreacion: string,
            usuarioCreacion: string,
            activo: boolean,
            predio: [],
            proyectoInstitucionEducativa: [],
            proyectoSede: []
        },
        contratacionProyecto: [
            {
                faseConstruccionNotMapped: boolean,
                fasePreConstruccionNotMapped: boolean,
                contratacionProyectoId: number,
                contratacionId: number,
                proyectoId: number,
                fechaCreacion: string,
                usuarioCreacion: string,
                eliminado: boolean,
                esReasignacion: boolean,
                porcentajeAvanceObra: number,
                requiereLicencia: boolean,
                usuarioModificacion: string,
                fechaModificacion: string,
                activo: boolean,
                esAvanceobra: boolean,
                tieneMonitoreoWeb: boolean,
                registroCompleto: boolean,
                contratacion: {
                    contratacionId: number,
                    tipoSolicitudCodigo: string,
                    numeroSolicitud: string,
                    estadoSolicitudCodigo: string,
                    contratistaId: number,
                    esObligacionEspecial: boolean,
                    usuarioCreacion: string,
                    fechaCreacion: string,
                    eliminado: boolean,
                    fechaEnvioDocumentacion: string,
                    observaciones: string,
                    rutaMinuta: string,
                    fechaTramite: string,
                    tipoContratacionCodigo: string,
                    registroCompleto1: boolean,
                    contratista: {
                        contratistaId: number,
                        tipoIdentificacionCodigo: number,
                        numeroIdentificacion: string,
                        nombre: string,
                        representanteLegal: string,
                        numeroInvitacion: string,
                        activo: boolean,
                        fechaCreacion: string,
                        usuarioCreacion: string,
                        representanteLegalNumeroIdentificacion: string,
                        tipoProponenteCodigo: string,
                        contratacion: []
                    },
                    contratacionObservacion: [],
                    contratacionProyecto: [],
                    contrato: [
                        {
                            valorFase1: number,
                            valorFase2: number,
                            tieneFase1: boolean,
                            tieneFase2: boolean,
                            contratacionId: number,
                            fechaTramite: string,
                            numeroContrato: string,
                            estado: boolean,
                            fechaEnvioFirma: string,
                            fechaFirmaContratista: string,
                            fechaFirmaFiduciaria: string,
                            fechaFirmaContrato: string,
                            observaciones: string,
                            rutaDocumento: string,
                            usuarioCreacion: string,
                            fechaCreacion: string,
                            usuarioModificacion: string,
                            fechaModificacion: string,
                            eliminado: boolean,
                            estadoVerificacionCodigo: 9,
                            estadoActa: number,
                            plazoFase1PreMeses: number,
                            plazoFase1PreDias: number,
                            fechaTerminacion: Date,
                            plazoFase2ConstruccionMeses: number,
                            plazoFase2ConstruccionDias: number,
                            registroCompleto: boolean,
                            rutaActaFase1: string,
                            contratoId: number,
                            estaDevuelto: boolean,
                            fechaAprobacionRequisitosInterventor: string,
                            fechaAprobacionRequisitosApoyo: string,
                            fechaAprobacionRequisitosSupervisor: string,
                            modalidadCodigo: number,
                            contratoConstruccion: [],
                            contratoObservacion: [],
                            contratoPerfil: [],
                            contratoPoliza: [],
                            controversiaContractual: [],
                            novedadContractual: [],
                            programacionPersonalContrato: [],
                            solicitudPago: []
                        }
                    ],
                    disponibilidadPresupuestal: []
                },
                contratacionObservacion: [],
                contratacionProyectoAportante: [],
                defensaJudicialContratacionProyecto: [],
                seguimientoDiario: [],
                seguimientoSemanal: [],
                sesionSolicitudObservacionProyecto: [],
                solicitudPago: [],
                solicitudPagoFaseCriterioProyecto: []
            }
        ],
        contratoConstruccion: [],
        contratoPerfil: [],
        disponibilidadPresupuestalProyecto: [],
        infraestructuraIntervenirProyecto: [],
        programacionPersonalContrato: [],
        proyectoAportante: [],
        proyectoFuentes: [],
        proyectoMonitoreoWeb: [],
        proyectoPredio: [],
        proyectoRequisitoTecnico: []
    },
    numeroContratoObra: string,
    nombreContratistaObra: string,
    numeroContratoInterventoria: string,
    nombreContratistaInterventoria: string,
    fechaTerminacionObra: string,
    fechaTerminacionInterventoria: string
}


export interface InformeFinalObservaciones{
    informeFinalObservacionesId?:number,
    informeFinalId?:number,
    observaciones?: string,
    esSupervision?: boolean,
    esCalificacion: boolean
}