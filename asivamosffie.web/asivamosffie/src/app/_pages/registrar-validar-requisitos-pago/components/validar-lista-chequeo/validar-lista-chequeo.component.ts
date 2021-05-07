import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { DialogObservacionesItemListchequeoComponent } from '../dialog-observaciones-item-listchequeo/dialog-observaciones-item-listchequeo.component';
import { DialogSubsanacionComponent } from '../dialog-subsanacion/dialog-subsanacion.component';
import humanize from 'humanize-plus';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router, ActivatedRoute } from '@angular/router';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';

@Component({
  selector: 'app-validar-lista-chequeo',
  templateUrl: './validar-lista-chequeo.component.html',
  styleUrls: ['./validar-lista-chequeo.component.scss']
})
export class ValidarListaChequeoComponent implements OnInit {

    @Input() contrato: any;
    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() listaChequeoCodigo: string;
    @Input() listaMenusId: any;
    @Output() semaforoObservacion = new EventEmitter<boolean>();
    solicitudPagoModificado: any;
    esExpensas: boolean;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'item',
      'documento',
      'revTecnica',
      'observaciones'
    ];
    displayedColumnsDetalle: string[] = [
        'item',
        'documento',
        'revTecnica'
      ];
    listaRevisionTecnica: Dominio[] = [];
    noCumpleCodigo = '2';
    seDiligencioCampo = false;

    constructor(
        private routes: Router,
        private activatedRoute: ActivatedRoute,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private dialog: MatDialog,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private commonSvc: CommonService )
    {
        this.commonSvc.listaRevisionTecnica()
            .subscribe( listaRevisionTecnica => this.listaRevisionTecnica = listaRevisionTecnica );
    }

    ngOnInit(): void {
        this.getListaChequeo();
    }

    async getListaChequeo() {
        if ( this.contrato === undefined && this.solicitudPago !== undefined ) {
            this.esExpensas = true;
            let completoObservacion = 0;
            for ( const solicitudPagoListaChequeo of this.solicitudPago.solicitudPagoListaChequeo ) {
                for ( const solicitudPagoListaChequeoRespuesta of solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta ) {
                    solicitudPagoListaChequeoRespuesta.respuestaCodigo = solicitudPagoListaChequeoRespuesta.respuestaCodigo !== undefined ? solicitudPagoListaChequeoRespuesta.respuestaCodigo : null;
                    solicitudPagoListaChequeoRespuesta.observacion = solicitudPagoListaChequeoRespuesta.observacion !== undefined ? solicitudPagoListaChequeoRespuesta.observacion : null;
                }

                if ( this.esVerDetalle === false ) {
                    // Get observacion CU autorizar solicitud de pago 4.1.9
                    const listaObservacionCoordinador = await this.obsMultipleSvc.asyncGetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                        this.listaMenusId.autorizarSolicitudPagoId,
                        this.activatedRoute.snapshot.params.id,
                        solicitudPagoListaChequeo.solicitudPagoListaChequeoId,
                        this.listaChequeoCodigo )
                    const observacionCoordinador = listaObservacionCoordinador.find( obs => obs.archivada === false );

                    // Get observacion CU verificar solicitud de pago 4.1.8
                    const listaObservacionApoyoSupervisor = await this.obsMultipleSvc.asyncGetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                        this.listaMenusId.aprobarSolicitudPagoId,
                        this.activatedRoute.snapshot.params.id,
                        solicitudPagoListaChequeo.solicitudPagoListaChequeoId,
                        this.listaChequeoCodigo )
                    const observacionSupervisor = listaObservacionApoyoSupervisor.find( obs => obs.archivada === false );


                    if ( observacionCoordinador !== undefined ) {
                        solicitudPagoListaChequeo.esAutorizar = true;
                        solicitudPagoListaChequeo.observacion = observacionCoordinador;

                        if ( observacionCoordinador.tieneObservacion === true ) {
                            completoObservacion++;
                        }
                    }
                    if ( observacionSupervisor !== undefined ) {
                        solicitudPagoListaChequeo.esAutorizar = false;
                        solicitudPagoListaChequeo.observacion = observacionSupervisor;

                        if ( observacionSupervisor.tieneObservacion === true ) {
                            completoObservacion++;
                        }
                    }
                }
            }

            if ( this.esVerDetalle === false ) {
                if ( completoObservacion > 0 ) {
                    this.semaforoObservacion.emit( true );
                }
            }

            this.solicitudPagoModificado = this.solicitudPago;
            this.dataSource = new MatTableDataSource();
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
        }
        if ( this.contrato !== undefined  && this.solicitudPago === undefined ) {
            this.esExpensas = false;
            let completoObservacion = 0;
            for ( const solicitudPagoListaChequeo of this.contrato.solicitudPagoOnly.solicitudPagoListaChequeo ) {
                for ( const solicitudPagoListaChequeoRespuesta of solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta ) {
                    solicitudPagoListaChequeoRespuesta.respuestaCodigo = solicitudPagoListaChequeoRespuesta.respuestaCodigo !== undefined ? solicitudPagoListaChequeoRespuesta.respuestaCodigo : null;
                    solicitudPagoListaChequeoRespuesta.observacion = solicitudPagoListaChequeoRespuesta.observacion !== undefined ? solicitudPagoListaChequeoRespuesta.observacion : null;
                }

                if ( this.esVerDetalle === false ) {
                    // Get observacion CU autorizar solicitud de pago 4.1.9
                    const listaObservacionCoordinador = await this.obsMultipleSvc.asyncGetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                        this.listaMenusId.autorizarSolicitudPagoId,
                        this.activatedRoute.snapshot.params.idSolicitud,
                        solicitudPagoListaChequeo.solicitudPagoListaChequeoId,
                        this.listaChequeoCodigo )
                    const observacionCoordinador = listaObservacionCoordinador.find( obs => obs.archivada === false );

                    // Get observacion CU verificar solicitud de pago 4.1.8
                    const listaObservacionApoyoSupervisor = await this.obsMultipleSvc.asyncGetObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                        this.listaMenusId.aprobarSolicitudPagoId,
                        this.activatedRoute.snapshot.params.idSolicitud,
                        solicitudPagoListaChequeo.solicitudPagoListaChequeoId,
                        this.listaChequeoCodigo )
                    const observacionSupervisor = listaObservacionApoyoSupervisor.find( obs => obs.archivada === false );


                    if ( observacionCoordinador !== undefined ) {
                        solicitudPagoListaChequeo.esAutorizar = true;
                        solicitudPagoListaChequeo.observacion = observacionCoordinador;

                        if ( observacionCoordinador.tieneObservacion === true ) {
                            completoObservacion++;
                        }
                    }
                    if ( observacionSupervisor !== undefined ) {
                        solicitudPagoListaChequeo.esAutorizar = false;
                        solicitudPagoListaChequeo.observacion = observacionSupervisor;

                        if ( observacionSupervisor.tieneObservacion === true ) {
                            completoObservacion++;
                        }
                    }
                }
            }

            if ( this.esVerDetalle === false ) {
                if ( completoObservacion > 0 ) {
                    this.semaforoObservacion.emit( true );
                }
            }

            this.solicitudPagoModificado = this.contrato.solicitudPagoOnly;
            this.dataSource = new MatTableDataSource();
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
        }
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    getMatTable( solicitudPagoListaChequeoRespuesta: any[] ) {
        return new MatTableDataSource( solicitudPagoListaChequeoRespuesta );
    }

    getRevisionTecnica( respuestaCodigo: string ) {
        if ( this.listaRevisionTecnica.length > 0 ) {
            const revision = this.listaRevisionTecnica.find( revision => revision.codigo === respuestaCodigo );

            if ( revision !== undefined ) {
                return revision.nombre;
            };
        }
    }

    getObservacion( registro: any, index: number, jIndex: number ) {
        const dialogRef = this.dialog.open(DialogObservacionesItemListchequeoComponent, {
            width: '80em',
            data: { dataSolicitud: this.esExpensas === true ? this.solicitudPago : this.contrato, registro, jIndex, esVerDetalle: this.esVerDetalle, esExpensas: this.esExpensas }
        });

        dialogRef.afterClosed()
            .subscribe(
                obs => {
                    if ( this.esVerDetalle === false ) {
                        this.solicitudPagoModificado.solicitudPagoListaChequeo[ index ].solicitudPagoListaChequeoRespuesta[ jIndex ].observacion = obs;
                    }
                }
            );
    }

    observacionSubsanacion( registro: any, jIndex: number ) {
        this.dialog.open( DialogSubsanacionComponent, {
            width: '80em',
            data: { registro, dataSolicitud: this.esExpensas === true ? this.solicitudPago : this.contrato, jIndex, esExpensas: this.esExpensas }
        })
    }

    getEstadoSemaforo( solicitudPagoListaChequeo: any ) {

        let sinDiligenciar = 0;
        let tieneSubsanacion: boolean;

        solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta.forEach( value => {
            if ( value.respuestaCodigo === null ) {
                sinDiligenciar++;
            }
            if ( value.validacionObservacion !== undefined || value.verificacionObservacion !== undefined ) {
                tieneSubsanacion = true;
            }
        } );

        if ( tieneSubsanacion === true ) {
            this.semaforoObservacion.emit( true );

            return 'sin-diligenciar';
        }

        if ( sinDiligenciar === solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta.length ) {
            return 'sin-diligenciar';
        }
        if ( solicitudPagoListaChequeo.registroCompleto === false ) {
            return 'en-proceso';
        }
        if ( solicitudPagoListaChequeo.registroCompleto === true ) {
            return 'completo';
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    disabledBtn( index: number ) {
        this.seDiligencioCampo = true;
        this.solicitudPagoModificado.solicitudPagoListaChequeo[ index ].seDeligencioCampo = true;
    }

    guardar() {
        this.solicitudPagoModificado.solicitudPagoListaChequeo.forEach( solicitudPagoListaChequeo => {
            solicitudPagoListaChequeo.solicitudPagoListaChequeoRespuesta.forEach( solicitudPagoListaChequeoRespuesta => {
                if ( solicitudPagoListaChequeoRespuesta.validacionObservacion !== undefined ) {
                    solicitudPagoListaChequeoRespuesta.validacionObservacion = null
                }

                if ( solicitudPagoListaChequeoRespuesta.verificacionObservacion !== undefined ) {
                    solicitudPagoListaChequeoRespuesta.verificacionObservacion = null
                }
            } );
        } )

        this.registrarPagosSvc.createEditNewPayment( this.solicitudPagoModificado )
        .subscribe(
            response => {
                this.openDialog( '', `<b>${ response.message }</b>` );
                this.solicitudPagoModificado.solicitudPagoListaChequeo.forEach( listaChequeo => {
                    if ( listaChequeo.seDeligencioCampo === true ) {
                        if ( listaChequeo.observacion !== undefined ) {
                            listaChequeo.observacion.archivada = !listaChequeo.observacion.archivada;
                            this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( listaChequeo.observacion ).subscribe();
                        }
                    }
                } );
                if ( this.esExpensas === false ) {
                    this.registrarPagosSvc.getValidateSolicitudPagoId( this.solicitudPagoModificado.solicitudPagoId )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () => this.routes.navigate(
                                        [
                                            '/registrarValidarRequisitosPago/verDetalleEditar',  this.solicitudPagoModificado.contratoId, this.solicitudPagoModificado.solicitudPagoId
                                        ]
                                    )
                                );
                            }
                        );
                } else {
                    this.registrarPagosSvc.getValidateSolicitudPagoId( this.solicitudPagoModificado.solicitudPagoId )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () => this.routes.navigate(
                                        [
                                            '/registrarValidarRequisitosPago/verDetalleEditarExpensas', this.solicitudPagoModificado.solicitudPagoId
                                        ]
                                    )
                                );
                            }
                        );
                }
            },
            err => this.openDialog( '', `<b>${ err.message }</b>` )
        );
    }

}
