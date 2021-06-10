import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { EstadoSolicitudPagoOrdenGiro, EstadosSolicitudPagoOrdenGiro } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { DialogProyectosAsociadosComponent } from '../dialog-proyectos-asociados/dialog-proyectos-asociados.component';

@Component({
  selector: 'app-verdetalle-editar-solicitud-pago',
  templateUrl: './verdetalle-editar-solicitud-pago.component.html',
  styleUrls: ['./verdetalle-editar-solicitud-pago.component.scss']
})
export class VerdetalleEditarSolicitudPagoComponent implements OnInit {

    dataSource = new MatTableDataSource();
    modalidadContratoArray: Dominio[] = [];
    listaEstadoSolicitudPago: EstadoSolicitudPagoOrdenGiro = EstadosSolicitudPagoOrdenGiro;
    menusIdPath: any; // Se obtienen los ID de los respectivos PATH de cada caso de uso que se implementaran observaciones.
    listaTipoObservacionSolicitudes: any; // Interfaz lista tipos de observaciones.
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    contrato: any;
    tipoSolicitudCodigo: any = {};
    // Semaforos
    tieneObservacionSemaforo = false;
    tieneObservacionSemaforoListaChequeo = false;
    tieneObservacionSemaforoSoporte = false;
    semaforoFormaDePago = 'sin-diligenciar';
    // Acordeones habilitados
    registroCompletoAcordeones: any = {
        registroCompletoFormaDePago: false,
        registroCompletoSolicitudPago: false,
        registroCompletoListaChequeo: false,
        registroCompletoOtrosCostos: false
    }
    displayedColumns: string[] = [
      'drp',
      'numDrp',
      'ProyectoLLaveMen',
      'NombreUso',
      'valor',
      'saldo'
    ];
    tieneObservacionOrdenGiro: boolean;

    constructor(
        private activatedRoute: ActivatedRoute,
        public dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private commonSvc: CommonService )
    {
        this.obsMultipleSvc.listaMenu()
            .subscribe( response => this.menusIdPath = response );
        this.obsMultipleSvc.listaTipoObservacionSolicitudes()
            .subscribe( response => {
                this.listaTipoObservacionSolicitudes = response;
            } );
        this.getContrato();
    }

    ngOnInit(): void {
    }

    getContrato() {
        this.registrarPagosSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitud )
            .subscribe(
                response => {
                    this.commonSvc.tiposDeSolicitudes()
                        .subscribe(
                          solicitudes => {
                            this.commonSvc.modalidadesContrato()
                                .subscribe( response => this.modalidadContratoArray = response );
                            for ( const solicitud of solicitudes ) {
                                if ( solicitud.codigo === '1' ) {
                                  this.tipoSolicitudCodigo.contratoObra = solicitud.codigo;
                                }
                                if ( solicitud.codigo === '2' ) {
                                  this.tipoSolicitudCodigo.contratoInterventoria = solicitud.codigo;
                                }
                                if ( solicitud.codigo === '3' ) {
                                  this.tipoSolicitudCodigo.expensas = solicitud.codigo;
                                }
                                if ( solicitud.codigo === '4' ) {
                                  this.tipoSolicitudCodigo.otrosCostos = solicitud.codigo;
                                }
                            }
                            this.contrato = response;
                            // console.log( this.contrato );
                            if ( this.contrato.solicitudPagoOnly.tipoSolicitudCodigo !== this.tipoSolicitudCodigo.otrosCostos ) {
                                console.log( this.contrato.tablaDRP );
                                this.dataSource = new MatTableDataSource( this.contrato.tablaDRP );
                                this.dataSource.paginator = this.paginator;
                                this.dataSource.sort = this.sort;

                                if ( this.contrato.solicitudPagoOnly.estadoCodigo === this.listaEstadoSolicitudPago.solicitudDevueltaPorGenerarOrdenGiroParaEquipoFacturacion && this.contrato.solicitudPagoOnly.observacionDevolucionOrdenGiro !== undefined ) {
                                    this.tieneObservacionOrdenGiro = true;
                                }

                                // Get semaforo forma de pago y registro completo
                                if ( this.contrato.solicitudPago.length > 1 ) {
                                    this.semaforoFormaDePago = 'completo';
                                    //Get registro completo
                                    this.registroCompletoAcordeones.registroCompletoFormaDePago = true;
                                } else {
                                    const solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];

                                    if ( solicitudPagoCargarFormaPago !== undefined ) {
    
                                        // Get semaforo
                                        if ( solicitudPagoCargarFormaPago.registroCompleto === false ) {
                                            this.semaforoFormaDePago = 'en-proceso';
                                        }
                                        if ( solicitudPagoCargarFormaPago.registroCompleto === true ) {
                                            this.semaforoFormaDePago = 'completo';
                                            //Get registro completo
                                            this.registroCompletoAcordeones.registroCompletoFormaDePago = true;
                                        }
                                    }
                                }
                            } else {
                                this.registroCompletoAcordeones.registroCompletoSolicitudPago = true;
                            }
                          }
                        );
                }
            );
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.modalidadContratoArray.length > 0 ) {
            const modalidad = this.modalidadContratoArray.find( modalidad => modalidad.codigo === modalidadCodigo );
            
            if ( modalidad !== undefined ) {
                return modalidad.nombre;
            }
        }
    }

    openProyectosAsociados() {
        if ( this.contrato === undefined ) {
            return;
        }

        this.dialog.open( DialogProyectosAsociadosComponent, {
            width: '80em',
            data: { contrato: this.contrato }
        });
    }

    enabledAcordeon( nombreAcordeon: string, tieneRegistroAnterior: boolean ) {

        // Acordeon solicitud de pago
        if ( nombreAcordeon === 'solicitudDePago' && tieneRegistroAnterior === false ) {
            return 'en-alerta';
        }
        if ( nombreAcordeon === 'solicitudDePago' && tieneRegistroAnterior === true ) {

            const solicitudPagoRegistrarSolicitudPago = this.contrato.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0];
            let semaforoSolicitudPago = 'sin-diligenciar';

            if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
                const solicitudPagoFase = solicitudPagoRegistrarSolicitudPago.solicitudPagoFase;

                if ( solicitudPagoFase.length > 0 ) {
                    if ( solicitudPagoRegistrarSolicitudPago.registroCompleto === false ) {
                        semaforoSolicitudPago = 'en-proceso';
                    }
    
                    if ( solicitudPagoRegistrarSolicitudPago.registroCompleto === true ) {
                        semaforoSolicitudPago = 'completo';
                        this.registroCompletoAcordeones.registroCompletoSolicitudPago = true; 
                    }
                }
            }
            return semaforoSolicitudPago;
        }

        // Acordeon lista de chequeo
        if ( nombreAcordeon === 'listaChequeo' && tieneRegistroAnterior === false ) {
            return 'en-alerta';
        }
        if ( nombreAcordeon === 'listaChequeo' && tieneRegistroAnterior === true ) {
            
            const solicitudPagoListaChequeo: any[] = this.contrato.solicitudPagoOnly.solicitudPagoListaChequeo;
            let semaforoListaChequeo = 'sin-diligenciar';
            let completo = 0;
            let enProceso = 0;
            let sinDiligenciar = 0;

            if ( solicitudPagoListaChequeo.length > 0 ) {
    
                if ( solicitudPagoListaChequeo.length > 0 ) {
                    solicitudPagoListaChequeo.forEach( listaChequeo => {
                        let total = 0;

                        if ( listaChequeo.registroCompleto === true ) {
                            completo++;
                        }

                        if ( listaChequeo.registroCompleto === false ) {
                            enProceso++;
                        }

                        listaChequeo.solicitudPagoListaChequeoRespuesta.forEach( value => {
                            if ( value.respuestaCodigo === null ) {
                                total++;
                            }
                        } );

                        if ( total === listaChequeo.solicitudPagoListaChequeoRespuesta.length ) {
                            sinDiligenciar++;
                        }
                    } );

                    if ( sinDiligenciar !== solicitudPagoListaChequeo.length ) {

                        if ( enProceso > 0 && enProceso < solicitudPagoListaChequeo.length ) {
                            semaforoListaChequeo = 'en-proceso';
                        }
                        if ( completo > 0 && sinDiligenciar > 0 && completo + sinDiligenciar === solicitudPagoListaChequeo.length ) {
                            semaforoListaChequeo = 'en-proceso';
                        }
                        if ( enProceso > 0 && enProceso === solicitudPagoListaChequeo.length ) {
                            semaforoListaChequeo = 'en-proceso';
                        }
                        if ( completo > 0 && completo === solicitudPagoListaChequeo.length ) {
                            semaforoListaChequeo = 'completo';
                            this.registroCompletoAcordeones.registroCompletoListaChequeo = true;
                        }
                        
                    }
                }
            }

            return semaforoListaChequeo;
        }

        // Acordeon soporte de la solicitud
        if ( nombreAcordeon === 'soporteSolicitud' && tieneRegistroAnterior === false ) {
            return 'en-alerta';
        }
        if ( nombreAcordeon === 'soporteSolicitud' && tieneRegistroAnterior === true ) {

            const solicitudPagoSoporteSolicitud = this.contrato.solicitudPagoOnly.solicitudPagoSoporteSolicitud[0];
            let semaforoSolicitudPago = 'sin-diligenciar';

            if ( solicitudPagoSoporteSolicitud !== undefined ) {
                if ( solicitudPagoSoporteSolicitud.registroCompleto === false ) {
                    semaforoSolicitudPago = 'en-proceso';
                }
                if ( solicitudPagoSoporteSolicitud.registroCompleto === true ) {
                    semaforoSolicitudPago = 'completo';
                }
            }

            return semaforoSolicitudPago;
        }
    }

}
