import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { Router } from '@angular/router';
import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';

@Component({
  selector: 'app-form-registrar-solicitud-de-pago',
  templateUrl: './form-registrar-solicitud-de-pago.component.html',
  styleUrls: ['./form-registrar-solicitud-de-pago.component.scss']
})
export class FormRegistrarSolicitudDePagoComponent implements OnInit {

    @Input() listaMenusId: any;
    @Input() registrarSolicitudPago: any;
    @Input() contrato: any;
    @Input() tieneObservacionOrdenGiro: boolean;
    @Input() esVerDetalle = false;
    @Output() tieneObservacionSemaforo = new EventEmitter<boolean>();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    solicitudPagoId = 0;
    solicitudPagoRegistrarSolicitudPagoId = 0;
    solicitudPagofaseId = 0;
    solicitudPagoObservacionId = 0;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoFasePreconstruccion: any;
    solicitudPagoFaseConstruccion: any;
    solicitudPagoCargarFormaPago: any;
    esAutorizar: boolean;
    tieneObservacion: boolean;
    observacion: any;
    fasesArray: Dominio[] = [];
    faseContrato: any = {};
    minDate: Date;
    postConstruccion = '3';
    contratacionProyectoId = 0;
    estaEditando = false;
    tienePreconstruccion = false;
    tieneConstruccion = false;
    manejoAnticipoRequiere: boolean;
    dataSource = new MatTableDataSource();
    displayedColumns: string[] = [
      'faseContrato',
      'valorFacturado',
      'porcentajeFacturado',
      'saldoPorPagar',
      'porcentajePorPagar'
    ];
    // Interfaz verificar semaforos en caso de haber observaciones
    estadoSemaforosObservaciones = {
        observacionCriterios: null,
        observacionAmortizacion: null,
        observacionCriteriosProyecto: null,
        observacionDatosFactura: null,
        observacionDescuentos: null
    }
    estadoSemaforosObservacionesConstruccion = {
        observacionCriterios: null,
        observacionAmortizacion: null,
        observacionCriteriosProyecto: null,
        observacionDatosFactura: null,
        observacionDescuentos: null
    }
    addressForm = this.fb.group({
      fechaSolicitud: [null, Validators.required],
      numeroRadicado: [null, Validators.required],
      faseContrato: [null, Validators.required]
    });
    estadoRegistroCompleto = {
        formRegistroCompleto: false,
        solicitudPagoFaseRegistroCompleto: false
    }
    estadoRegistroCompletoSubAcordeon = {
        criterioRegistroCompleto: false,
        amortizacionRegistroCompleto: false,
        detalleFacturaRegistroCompleto: false
    }
    estadoRegistroCompletoSubAcordeonConstruccion = {
        criterioRegistroCompleto: false,
        amortizacionRegistroCompleto: false,
        detalleFacturaRegistroCompleto: false
    }

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
        this.getRegistroFase();
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    getRegistroFase() {
        this.commonSvc.listaFases()
            .subscribe(
                response => {
                    if ( this.contrato.fechaActaInicioFase1 === undefined ) {
                        this.minDate = new Date( this.contrato.fechaActaInicioFase2 );
                    } else {
                        this.minDate = new Date( this.contrato.fechaActaInicioFase1 );
                    }

                    this.manejoAnticipoRequiere = this.contrato.contratoConstruccion.length > 0 ? this.contrato.contratoConstruccion[0].manejoAnticipoRequiere : false;

                    if ( this.contrato.contratacion.contratacionProyecto.length  > 0 && this.contrato.contratacion.contratacionProyecto.length < 2 ) {
                        this.contratacionProyectoId = this.contrato.contratacion.contratacionProyecto[0].contratacionProyectoId;
                    }

                    response.forEach( fase => {
                        if ( fase.codigo === '1' ) {
                            this.faseContrato.preConstruccion = fase.codigo;
                        }
                        if ( fase.codigo === '2' ) {
                            this.faseContrato.construccion = fase.codigo;
                        }
                    } );

                    if ( this.contrato.solicitudPago.length > 1 ) {
                        this.solicitudPagoCargarFormaPago = this.contrato.solicitudPago[0].solicitudPagoCargarFormaPago[0];
                    } else {
                        this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
                    }

                    response.forEach( ( fase, index ) => {
                        if ( fase.codigo === this.postConstruccion ) {
                            response.splice( index, 1 );
                        }
                        if ( ( this.solicitudPagoCargarFormaPago.tieneFase1 === false || this.solicitudPagoCargarFormaPago.tieneFase1 === undefined ) && this.faseContrato.preConstruccion === fase.codigo ) {
                            response.splice( index, 1 );
                        }
                    } );
                    this.fasesArray = response;

                    if ( this.contrato.solicitudPagoOnly !== undefined ) {
                        this.solicitudPagoId = this.contrato.solicitudPagoOnly.solicitudPagoId;
                        this.solicitudPagoRegistrarSolicitudPago = this.contrato.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0];
            
                        if ( this.solicitudPagoRegistrarSolicitudPago !== undefined ) {
                            this.solicitudPagoRegistrarSolicitudPagoId = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoRegistrarSolicitudPagoId;
                            const faseSeleccionada: string[] = [];

                            if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
                                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                                    for ( const solicitudPagoFase of this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                                        if ( solicitudPagoFase.esPreconstruccion === true ) {
                                            faseSeleccionada.push( this.faseContrato.preConstruccion );
                                            this.tienePreconstruccion = true;

                                            this.solicitudPagoFasePreconstruccion = solicitudPagoFase;
                                        }

                                        if ( solicitudPagoFase.esPreconstruccion === false ) {
                                            faseSeleccionada.push( this.faseContrato.construccion );
                                            this.tieneConstruccion = true;

                                            this.solicitudPagoFaseConstruccion = solicitudPagoFase;
                                        }
                                    }
                                }
                            }

                            this.estaEditando = true;
                            this.addressForm.markAllAsTouched();
                            this.addressForm.setValue(
                                {
                                    fechaSolicitud: this.solicitudPagoRegistrarSolicitudPago.fechaSolicitud !== undefined ? new Date( this.solicitudPagoRegistrarSolicitudPago.fechaSolicitud ) : new Date(),
                                    numeroRadicado: this.solicitudPagoRegistrarSolicitudPago.numeroRadicadoSac !== undefined ? this.solicitudPagoRegistrarSolicitudPago.numeroRadicadoSac : null,
                                    faseContrato: null
                                }
                            );


                            if ( this.esVerDetalle === false ) {
                                // Get observacion CU autorizar solicitud de pago 4.1.9
                                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                    this.listaMenusId.autorizarSolicitudPagoId,
                                    this.contrato.solicitudPagoOnly.solicitudPagoId,
                                    this.solicitudPagofaseId,
                                    this.registrarSolicitudPago.registrarSolicitudPagoCodigo )
                                    .subscribe(
                                        response => {
                                            const observacion = response.find( obs => obs.archivada === false );

                                            if ( observacion !== undefined ) {
                                                this.esAutorizar = true;
                                                this.observacion = observacion;

                                                if ( this.observacion.tieneObservacion === true ) {
                                                    this.tieneObservacion = true;
                                                    this.tieneObservacionSemaforo.emit( true );
                                                    this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                                }
                                            }
                                        }
                                    );

                                // Get observacion CU verificar solicitud de pago 4.1.8
                                this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                    this.listaMenusId.aprobarSolicitudPagoId,
                                    this.contrato.solicitudPagoOnly.solicitudPagoId,
                                    this.solicitudPagoRegistrarSolicitudPagoId,
                                    this.registrarSolicitudPago.registrarSolicitudPagoCodigo )
                                    .subscribe(
                                        response => {
                                            const observacion = response.find( obs => obs.archivada === false );
                                            if ( observacion !== undefined ) {
                                                this.esAutorizar = false;
                                                this.observacion = observacion;

                                                if ( this.observacion.tieneObservacion === true ) {
                                                    this.tieneObservacion = true;
                                                    this.tieneObservacionSemaforo.emit( true );
                                                    this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                                }
                                            }
                                        }
                                    );
                            }
                        }
                    }
                    // Tabla pendiente por integrar
                    this.dataSource = new MatTableDataSource( this.contrato.vContratoPagosRealizados );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                }
            );
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
            width: '28em',
            data: { modalTitle, modalText }
        });
    }

    guardar() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();

        const pSolicitudPago = {
            solicitudPagoId: this.solicitudPagoId,
            contratoId: this.contrato.contratoId,
            solicitudPagoRegistrarSolicitudPago: [
                {
                    solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                    solicitudPagoId: this.solicitudPagoId,
                    tieneFasePreconstruccion: this.tienePreconstruccion,
                    tieneFaseConstruccion: this.tieneConstruccion,
                    fechaSolicitud: new Date( this.addressForm.get( 'fechaSolicitud' ).value ).toISOString(),
                    numeroRadicadoSAC: this.addressForm.get( 'numeroRadicado' ).value
                }
            ]
        }

        this.registrarPagosSvc.createEditNewPayment( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    if ( this.tieneObservacion === true ) {
                        this.observacion.archivada = !this.observacion.archivada;
                        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( this.observacion ).subscribe();
                    }
                    this.registrarPagosSvc.getValidateSolicitudPagoId( this.solicitudPagoId )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () => this.routes.navigate(
                                        [
                                            '/registrarValidarRequisitosPago/verDetalleEditar',  this.contrato.contratoId, this.solicitudPagoId
                                        ]
                                    )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
