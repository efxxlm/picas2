import { ActualizarPolizasService } from './../../../../core/_services/actualizarPolizas/actualizar-polizas.service';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import humanize from 'humanize-plus';
import { EstadosRevision, PerfilCodigo } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';

@Component({
  selector: 'app-actualizar-poliza-rapg',
  templateUrl: './actualizar-poliza-rapg.component.html',
  styleUrls: ['./actualizar-poliza-rapg.component.scss']
})
export class ActualizarPolizaRapgComponent implements OnInit {

    contratoPoliza: any;
    esRegistroNuevo: boolean = false;
    esVerDetalle: boolean;
    contratoPolizaActualizacion: any;
    estadosRevision = EstadosRevision;
    listaPerfilCodigo = PerfilCodigo;
    listaUsuarios: any[] = [];
    listaTipoSolicitudContrato: Dominio[] = [];
    polizasYSegurosArray: Dominio[] = [];
    listaTipoDocumento: Dominio[] = [];
    responsable: any;
    dataSource = new MatTableDataSource();
    ultimaRevisionAprobada: any;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'polizaYSeguros',
        'responsableAprobacion'
    ];
    semaforoAcordeones = {
        acordeonTipoActualizacion: 'sin-diligenciar',
        acordeonVigenciaValor: 'en-alerta',
        acordeonObsEspecifica: 'en-alerta',
        acordeonListaChequeo: 'en-alerta',
        acordeonRevision: 'en-alerta'
    }
    acordeonRegistroCompleto = {
        acordeonTipoActualizacion: false,
        acordeonVigenciaValor: false,
        acordeonObsEspecifica: false,
        acordeonListaChequeo: false
    }

    constructor(
        private activatedRoute: ActivatedRoute,
        private routes: Router,
        private dialog: MatDialog,
        private actualizarPolizaSvc: ActualizarPolizasService,
        private commonSvc: CommonService )
    {
        this.getContratoPoliza();
    }

    ngOnInit(): void {
    }

    async getContratoPoliza() {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {

            if ( urlSegment.path === 'actualizarPoliza' ) {
                this.esVerDetalle = false;
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'verDetalleEditarPoliza' ) {
                this.esVerDetalle = false;
                this.esRegistroNuevo = false;
                return;
            }
            if ( urlSegment.path === 'verDetallePoliza' ) {
                this.esVerDetalle = true;
                return;
            }

        } )
        this.listaUsuarios = await this.commonSvc.getUsuariosByPerfil( this.listaPerfilCodigo.fiduciaria ).toPromise();
        this.polizasYSegurosArray = await this.commonSvc.listaGarantiasPolizas().toPromise();
        this.listaTipoSolicitudContrato = await this.commonSvc.listaTipoSolicitudContrato().toPromise();
        this.listaTipoDocumento = await this.commonSvc.listaTipodocumento().toPromise();

        this.actualizarPolizaSvc.getContratoPoliza( this.activatedRoute.snapshot.params.id, this.esRegistroNuevo )
            .subscribe(
                response => {
                    this.contratoPoliza = response;
                    this.responsable = this.contratoPoliza.userResponsableAprobacion;
                    console.log( this.contratoPoliza );

                    if ( this.contratoPoliza.polizaObservacion.length > 0 ) {
                        const polizaAprobada: any[] = this.contratoPoliza.polizaObservacion.filter( polizaAprobada => polizaAprobada.estadoRevisionCodigo === this.estadosRevision.aprobacion );

                        if ( polizaAprobada.length > 0 ) {
                            this.ultimaRevisionAprobada = polizaAprobada[ polizaAprobada.length - 1 ];
                        }
                    }

                    if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
                        if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                            this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[ 0 ];
                        }
                    }

                    if ( this.contratoPolizaActualizacion !== undefined ) {
                        this.checkSemaforos();
                    }
                    this.dataSource = new MatTableDataSource( this.contratoPoliza.polizaGarantia );
                }
            )
    }

    checkSemaforos() {
        // Semaforo del acordeon "Razon y tipo de actualizacion"
        if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro !== undefined ) {
            if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro.length > 0 ) {
                const contratoPolizaActualizacionSeguro: any[] = this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro;
                let enProceso = 0;
                let completo = 0;

                for ( const actualizacion of contratoPolizaActualizacionSeguro ) {
                    if ( actualizacion.registroCompletoActualizacion === false ) {
                        enProceso++;
                    }

                    if ( actualizacion.registroCompletoActualizacion === true ) {
                        completo++;
                    }
                }

                if ( enProceso > 0 && enProceso === contratoPolizaActualizacionSeguro.length ) {
                    this.semaforoAcordeones.acordeonTipoActualizacion = 'en-proceso';
                }
                if ( enProceso > 0 && enProceso < contratoPolizaActualizacionSeguro.length ) {
                    this.semaforoAcordeones.acordeonTipoActualizacion = 'en-proceso';
                }
                if ( completo > 0 && completo < contratoPolizaActualizacionSeguro.length ) {
                    this.semaforoAcordeones.acordeonTipoActualizacion = 'en-proceso';
                }
                if ( completo > 0 && completo === contratoPolizaActualizacionSeguro.length ) {
                    if ( this.contratoPolizaActualizacion.razonActualizacionCodigo !== undefined && this.contratoPolizaActualizacion.fechaExpedicionActualizacionPoliza !== undefined ) {
                        this.semaforoAcordeones.acordeonTipoActualizacion = 'completo';
                        this.semaforoAcordeones.acordeonVigenciaValor = 'sin-diligenciar';
                        this.acordeonRegistroCompleto.acordeonTipoActualizacion = true;
                    } else {
                        this.semaforoAcordeones.acordeonTipoActualizacion = 'en-proceso';
                    }
                }
            }
        }
        // Semaforo del acordeon "Vigencias y valor"
        if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro !== undefined && this.acordeonRegistroCompleto.acordeonTipoActualizacion === true ) {
            if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro.length > 0 ) {
                const contratoPolizaActualizacionSeguro: any[] = this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro;
                let enProceso = 0;
                let completo = 0;

                for ( const actualizacion of contratoPolizaActualizacionSeguro ) {
                    if ( actualizacion.registroCompletoSeguro === false ) {
                        enProceso++;
                    }

                    if ( actualizacion.registroCompletoSeguro === true ) {
                        completo++;
                    }
                }

                if ( enProceso > 0 && enProceso === contratoPolizaActualizacionSeguro.length ) {
                    this.semaforoAcordeones.acordeonVigenciaValor = 'en-proceso';
                }
                if ( enProceso > 0 && enProceso < contratoPolizaActualizacionSeguro.length ) {
                    this.semaforoAcordeones.acordeonVigenciaValor = 'en-proceso';
                }
                if ( completo > 0 && completo < contratoPolizaActualizacionSeguro.length ) {
                    this.semaforoAcordeones.acordeonVigenciaValor = 'en-proceso';
                }
                if ( completo > 0 && completo === contratoPolizaActualizacionSeguro.length ) {
                    this.semaforoAcordeones.acordeonVigenciaValor = 'completo';
                    this.semaforoAcordeones.acordeonObsEspecifica = 'sin-diligenciar';
                    this.acordeonRegistroCompleto.acordeonVigenciaValor = true;
                }
            }
        }
        // Semaforo del acordeon "Observaciones especificas"
        if ( this.contratoPolizaActualizacion.tieneObservacionEspecifica !== undefined ) {
            if ( this.contratoPolizaActualizacion.registroCompletoObservacionEspecifica === false ) {
                this.semaforoAcordeones.acordeonObsEspecifica = 'en-proceso';
            }
            if ( this.contratoPolizaActualizacion.registroCompletoObservacionEspecifica === true ) {
                this.semaforoAcordeones.acordeonObsEspecifica = 'completo';
                this.semaforoAcordeones.acordeonListaChequeo = 'sin-diligenciar';
                this.acordeonRegistroCompleto.acordeonObsEspecifica = true;
            }
        }
        // Semaforo del acordeon "Lista de chequeo"
        if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo !== undefined ) {
            if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo.length > 0 ) {
                const contratoPolizaActualizacionListaChequeo = this.contratoPolizaActualizacion.contratoPolizaActualizacionListaChequeo[ 0 ];

                if ( contratoPolizaActualizacionListaChequeo.registroCompleto === false ) {
                    this.semaforoAcordeones.acordeonListaChequeo = 'en-proceso';
                }
                if ( contratoPolizaActualizacionListaChequeo.registroCompleto === true ) {
                    this.semaforoAcordeones.acordeonListaChequeo = 'completo';
                    this.semaforoAcordeones.acordeonRevision = 'sin-diligenciar';
                    this.acordeonRegistroCompleto.acordeonListaChequeo = true;
                }
            }
        }
        // Semaforo del acordeon "Revision y aprobacion"
        if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion !== undefined ) {
            if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion.length > 0 ) {
                const contratoPolizaActualizacionRevisionAprobacionObservacion = this.contratoPolizaActualizacion.contratoPolizaActualizacionRevisionAprobacionObservacion;
                const revision = contratoPolizaActualizacionRevisionAprobacionObservacion.filter( revision => revision.estadoSegundaRevision === this.estadosRevision.aprobacion );

                if ( revision.length > 0 ) {
                    const ultimaRevision = revision[ revision.length - 1 ];

                    if ( contratoPolizaActualizacionRevisionAprobacionObservacion[ contratoPolizaActualizacionRevisionAprobacionObservacion.length - 1 ] === ultimaRevision ) {

                        if ( ultimaRevision.registroCompleto === false ) {
                            this.semaforoAcordeones.acordeonRevision = 'en-proceso';
                        }

                        if ( ultimaRevision.registroCompleto === true ) {
                            this.semaforoAcordeones.acordeonRevision = 'completo';
                        }
                    }
                }
            }
        }
    }

    getTipoSolicitudContrato( tipoSolicitudCodigo: string ) {
        if ( this.listaTipoSolicitudContrato.length > 0 ) {
            const solicitud = this.listaTipoSolicitudContrato.find( solicitud => solicitud.codigo === tipoSolicitudCodigo );

            if ( solicitud !== undefined ) {
                return solicitud.nombre;
            }
        }
    }

    getPoliza( codigo: string ) {
        if ( this.polizasYSegurosArray.length > 0 ) {
            const poliza = this.polizasYSegurosArray.find( poliza => poliza.codigo === codigo );

            if ( poliza !== undefined ) {
                return poliza.nombre;
            }
        }
    }

    getTipoDocumento( codigo: string ) {
        if ( this.listaTipoDocumento.length > 0 ) {
            const documento = this.listaTipoDocumento.find( documento => documento.codigo === codigo );

            if ( documento !== undefined ) {
                return documento.nombre;
            }
        }
    }

    getResponsable( usuarioId: number ) {
        const responsable = this.listaUsuarios.find( usuario => usuario.usuarioId === usuarioId );

        if ( responsable !== undefined ) {
            return `${ this.firstLetterUpperCase( responsable.primerNombre ) } ${ this.firstLetterUpperCase( responsable.primerApellido ) }`;
        }
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

}
