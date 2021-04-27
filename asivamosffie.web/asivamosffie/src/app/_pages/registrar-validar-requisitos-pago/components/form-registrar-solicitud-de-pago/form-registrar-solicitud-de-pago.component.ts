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
    @Output() tieneObservacionSemaforo = new EventEmitter<boolean>();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    solicitudPagoId = 0;
    solicitudPagoRegistrarSolicitudPagoId = 0;
    solicitudPagofaseId = 0;
    solicitudPagoObservacionId = 0;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoFase: any;
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
    manejoAnticipoRequiere: boolean;
    dataSource = new MatTableDataSource();
    displayedColumns: string[] = [
      'faseContrato',
      'pagosRealizados',
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

                    if ( this.manejoAnticipoRequiere === false ) {
                        this.estadoRegistroCompletoSubAcordeon.amortizacionRegistroCompleto = true;
                    }

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
                            let faseSeleccionada: Dominio;
                            this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];
                            if ( this.solicitudPagoFase !== undefined ) {
                                this.solicitudPagofaseId = this.solicitudPagoFase.solicitudPagoFaseId;
            
                                if ( this.solicitudPagoFase.esPreconstruccion === true ) {
                                    const fase = this.fasesArray.filter( fase => fase.codigo === this.faseContrato.preConstruccion );
                                    faseSeleccionada = fase[0];
                                }
                                if ( this.solicitudPagoFase.esPreconstruccion === false ) {
                                    const fase = this.fasesArray.filter( fase => fase.codigo === this.faseContrato.construccion );
                                    faseSeleccionada = fase[0];
                                }
                            }
                            this.estaEditando = true;
                            this.addressForm.markAllAsTouched();
                            this.addressForm.setValue(
                                {
                                    fechaSolicitud: this.solicitudPagoRegistrarSolicitudPago.fechaSolicitud !== undefined ? new Date( this.solicitudPagoRegistrarSolicitudPago.fechaSolicitud ) : null,
                                    numeroRadicado: this.solicitudPagoRegistrarSolicitudPago.numeroRadicadoSac !== undefined ? this.solicitudPagoRegistrarSolicitudPago.numeroRadicadoSac : null,
                                    faseContrato: faseSeleccionada !== undefined ? faseSeleccionada : null
                                }
                            );
                            // hasValue in Object Form

                            this.estadoRegistroCompleto.formRegistroCompleto = !Object.values( this.addressForm.value ).includes( null );
                            if ( this.estadoRegistroCompleto.formRegistroCompleto === true && this.tieneObservacionOrdenGiro === undefined ) {
                                this.addressForm.get( 'fechaSolicitud' ).disable();
                                this.addressForm.get( 'numeroRadicado' ).disable();
                                this.addressForm.get( 'faseContrato' ).disable();
                            }

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
                                                this.addressForm.get( 'fechaSolicitud' ).enable();
                                                this.addressForm.get( 'numeroRadicado' ).enable();
                                                this.addressForm.get( 'faseContrato' ).enable();
                                                this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                            }
                                        }
                                    }
                                );

                            // Get observacion CU verificar solicitud de pago 4.1.8
                            this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                this.listaMenusId.aprobarSolicitudPagoId,
                                this.contrato.solicitudPagoOnly.solicitudPagoId,
                                this.solicitudPagofaseId,
                                this.registrarSolicitudPago.registrarSolicitudPagoCodigo )
                                .subscribe(
                                    response => {
                                        const observacion = response.find( obs => obs.archivada === false );
                                        if ( observacion !== undefined ) {
                                            this.esAutorizar = false;
                                            this.observacion = observacion;

                                            if ( this.observacion.tieneObservacion === true ) {
                                                this.tieneObservacion = true;
                                                this.tieneObservacionSemaforo.emit( true );;
                                                this.addressForm.get( 'fechaSolicitud' ).enable();
                                                this.addressForm.get( 'numeroRadicado' ).enable();
                                                this.addressForm.get( 'faseContrato' ).enable();
                                                this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                            }
                                        }
                                    }
                                );
                        }
                    }
                    // Tabla pendiente por integrar
                    this.dataSource = new MatTableDataSource( this.contrato.vContratoPagosRealizados );
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                }
            );
    }

    getValueFase( listFaseCodigo: Dominio[] ) {
        const listaFase = [ ...listFaseCodigo ];

        console.log( listaFase );
    }

    enabledAcordeonFase( registroCompleto: boolean, esPreconstruccion?: boolean ) {

        if ( registroCompleto === false && esPreconstruccion === false ) {
            return 'en-alerta';
        }

        if ( registroCompleto === false && esPreconstruccion === true ) {
            return 'en-alerta';
        }

        if ( registroCompleto === undefined || registroCompleto === null ) {
            return 'en-alerta';
        }

        // Acordeon fase preconstruccion
        if ( registroCompleto === true && esPreconstruccion === true ) {

            let semaforoPreConstruccion = 'sin-diligenciar';

            if ( this.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoFase.registroCompleto === false && this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                    semaforoPreConstruccion = 'en-proceso';
                }
                if ( this.solicitudPagoFase.registroCompleto === true && this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                    
                    semaforoPreConstruccion = 'en-proceso';

                    if ( this.solicitudPagoFase.solicitudPagoFaseFactura.length > 0 ) {
                        if ( this.solicitudPagoFase.solicitudPagoFaseFactura[0].numero !== undefined && this.solicitudPagoFase.solicitudPagoFaseFactura[0].fecha !== undefined ) {
                            semaforoPreConstruccion = 'completo';
                            this.estadoRegistroCompleto.solicitudPagoFaseRegistroCompleto = true;
                        }
                    }
                }
            } else {
                semaforoPreConstruccion = 'en-alerta';
            }

            return semaforoPreConstruccion;
        }

        // Acordeon fase construccion
        if ( registroCompleto === true && esPreconstruccion === false ) {

            let semaforoConstruccion = 'sin-diligenciar';

            if ( this.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoFase.registroCompleto === false && this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                    semaforoConstruccion = 'en-proceso';
                }
                if ( this.solicitudPagoFase.registroCompleto === true && this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                    
                    semaforoConstruccion = 'en-proceso';

                    if ( this.solicitudPagoFase.solicitudPagoFaseFactura.length > 0 ) {
                        if ( this.solicitudPagoFase.solicitudPagoFaseFactura[0].numero !== undefined && this.solicitudPagoFase.solicitudPagoFaseFactura[0].fecha !== undefined ) {
                            semaforoConstruccion = 'completo';
                            this.estadoRegistroCompleto.solicitudPagoFaseRegistroCompleto = true;
                        }
                    }
                }
            }

            return semaforoConstruccion;
        }
        // Acordeon datos de la factura
        if ( esPreconstruccion === undefined ) {

            if ( registroCompleto === false ) {
                return 'en-alerta';
            }
            if ( registroCompleto === true ) {

                const solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];
                let semaforoPagoFactura = 'sin-diligenciar';

                if ( solicitudPagoFaseFactura !== undefined ) {
                    if ( solicitudPagoFaseFactura.registroCompleto === false ) {
                        semaforoPagoFactura = 'en-proceso';
                    }
                    if ( solicitudPagoFaseFactura.registroCompleto === true ) {
                        semaforoPagoFactura = 'completo';
                    }
                }

                return semaforoPagoFactura;
            }

        }
    }

    enabledAcordeonSubFase( tipoAcordeon: string, esPreconstruccion: boolean ) {
        if ( this.solicitudPagoFase !== undefined ) {
            if ( esPreconstruccion === true ) {
                if ( tipoAcordeon === 'criterioDePago' ) {

                    let semaforoCriterioPago = 'sin-diligenciar';
    
                    if ( this.solicitudPagoFase.registroCompletoCriterio === false ) {
                        semaforoCriterioPago = 'en-proceso';
                    }
                    if ( this.solicitudPagoFase.registroCompletoCriterio === true ) {
                        this.estadoRegistroCompletoSubAcordeon.criterioRegistroCompleto = true;
                        this.estadoRegistroCompletoSubAcordeon.amortizacionRegistroCompleto = true;
                        semaforoCriterioPago = 'completo';
                    }

                    return semaforoCriterioPago;
                }
                if ( tipoAcordeon === 'detalleFactura' ) {
                    if ( this.estadoRegistroCompletoSubAcordeon.amortizacionRegistroCompleto === true ) {
                        if ( this.contrato.contratacion.contratacionProyecto.length > 1 ) {
                            if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                                const solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio.filter( criterioValue => criterioValue.solicitudPagoFaseCriterioProyecto.length > 0 );
                                let totalCriterioRegistroCompleto = 0;
    
                                for ( const criterio of solicitudPagoFaseCriterio ) {
                                    if ( criterio.registroCompleto === true ) {
                                        totalCriterioRegistroCompleto++;
                                    }
                                }
    
                                if ( totalCriterioRegistroCompleto > 0 && totalCriterioRegistroCompleto === solicitudPagoFaseCriterio.length ) {
                                    this.estadoRegistroCompletoSubAcordeon.detalleFacturaRegistroCompleto = true;
                                    return 'completo';
                                }
                                if ( totalCriterioRegistroCompleto > 0 && totalCriterioRegistroCompleto !== solicitudPagoFaseCriterio.length ) {
                                    return 'en-proceso';
                                }
                                if ( totalCriterioRegistroCompleto === 0 ) {
                                    return 'sin-diligenciar';
                                }
                            } else {
                                return 'sin-diligenciar';
                            }
                        } else {
                            this.estadoRegistroCompletoSubAcordeon.detalleFacturaRegistroCompleto = true;
                            return '';
                        }
                    }
                }

                if ( tipoAcordeon === 'datosFactura' ) {
                    if ( this.estadoRegistroCompletoSubAcordeon.detalleFacturaRegistroCompleto === false ) {
                        return 'en-alerta';
                    }

                    if ( this.estadoRegistroCompletoSubAcordeon.detalleFacturaRegistroCompleto === true ) {
                        if ( this.solicitudPagoFase.solicitudPagoFaseFactura.length === 0 ) {
                            return 'sin-diligenciar';
                        }

                        if ( this.solicitudPagoFase.solicitudPagoFaseFactura[0].numero !== undefined && this.solicitudPagoFase.solicitudPagoFaseFactura[0].fecha !== undefined ) {
                            this.estadoRegistroCompleto.solicitudPagoFaseRegistroCompleto = true;
                            return 'completo';
                        } else {
                            return 'en-proceso';
                        }
                    }
                }
            }

            if ( esPreconstruccion === false ) {

                if ( tipoAcordeon === 'criterioDePago' ) {

                    let semaforoCriterioPago = 'sin-diligenciar';
    
                    if ( this.solicitudPagoFase.registroCompletoCriterio === false ) {
                        semaforoCriterioPago = 'en-proceso';
                    }
                    if ( this.solicitudPagoFase.registroCompletoCriterio === true ) {
                        this.estadoRegistroCompletoSubAcordeon.criterioRegistroCompleto = true;
                        this.estadoRegistroCompletoSubAcordeon.amortizacionRegistroCompleto = true;
                        semaforoCriterioPago = 'completo';
                    }

                    return semaforoCriterioPago;
                }

                if ( this.manejoAnticipoRequiere === true ) {
                    if ( tipoAcordeon === 'amortizacion' ) {
    
                        if ( this.estadoRegistroCompletoSubAcordeon.criterioRegistroCompleto === false ) {
                            return 'en-alerta';
                        }
                        if ( this.estadoRegistroCompletoSubAcordeon.criterioRegistroCompleto === true ) {
                            
                            const solicitudPagoFaseAmortizacion = this.solicitudPagoFase.solicitudPagoFaseAmortizacion[0];
                            let semaforoAmortizacion = 'sin-diligenciar';
        
                            if ( solicitudPagoFaseAmortizacion !== undefined ) {
                                if ( solicitudPagoFaseAmortizacion.registroCompleto === false ) {
                                    semaforoAmortizacion = 'en-proceso';
                                }
                                if ( solicitudPagoFaseAmortizacion.registroCompleto === true ) {
                                    semaforoAmortizacion = 'completo';
                                    this.estadoRegistroCompletoSubAcordeon.amortizacionRegistroCompleto = true;
                                }
                            }
        
                            return semaforoAmortizacion;
                        }
        
                    }
                }

                if ( tipoAcordeon === 'detalleFactura' ) {
                    if ( this.estadoRegistroCompletoSubAcordeon.amortizacionRegistroCompleto === true ) {
                        if ( this.contrato.contratacion.contratacionProyecto.length > 1 ) {
                            if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                                const solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio.filter( criterioValue => criterioValue.solicitudPagoFaseCriterioProyecto.length > 0 );
                                let totalCriterioRegistroCompleto = 0;

                                for ( const criterio of solicitudPagoFaseCriterio ) {
                                    if ( criterio.registroCompleto === true ) {
                                        totalCriterioRegistroCompleto++;
                                    }
                                }

                                if ( totalCriterioRegistroCompleto > 0 && totalCriterioRegistroCompleto === solicitudPagoFaseCriterio.length ) {
                                    this.estadoRegistroCompletoSubAcordeon.detalleFacturaRegistroCompleto = true;
                                    return 'completo';
                                }
                                if ( totalCriterioRegistroCompleto > 0 && totalCriterioRegistroCompleto !== solicitudPagoFaseCriterio.length ) {
                                    return 'en-proceso';
                                }
                                if ( totalCriterioRegistroCompleto === 0 ) {
                                    return 'sin-diligenciar';
                                }
                            } else {
                                return 'sin-diligenciar';
                            }
                        } else {
                            this.estadoRegistroCompletoSubAcordeon.detalleFacturaRegistroCompleto = true;
                            return '';
                        }
                    } else {
                        return 'en-alerta';
                    }
                }

                if ( tipoAcordeon === 'datosFactura' ) {
                    if ( this.estadoRegistroCompletoSubAcordeon.detalleFacturaRegistroCompleto === false ) {
                        return 'en-alerta';
                    }

                    if ( this.estadoRegistroCompletoSubAcordeon.detalleFacturaRegistroCompleto === true ) {
                        if ( this.solicitudPagoFase.solicitudPagoFaseFactura.length === 0 ) {
                            return 'sin-diligenciar';
                        }

                        if ( this.solicitudPagoFase.solicitudPagoFaseFactura[0].numero !== undefined && this.solicitudPagoFase.solicitudPagoFaseFactura[0].fecha !== undefined ) {
                            this.estadoRegistroCompleto.solicitudPagoFaseRegistroCompleto = true;
                            return 'completo';
                        } else {
                            return 'en-proceso';
                        }
                    }
                }

            }
        }
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
        let tieneFasePreConstruccion = false;
        let tieneFaseConstruccion = false;
        const faseSeleccionada: Dominio = this.addressForm.get( 'faseContrato' ).value;
        const pago = () => {
            if ( faseSeleccionada !== null ) {
                const fase: Dominio[] = this.fasesArray.filter( fase => fase.codigo === faseSeleccionada.codigo );
                if ( fase[0].codigo === this.faseContrato.preConstruccion ) {
                    return [
                        {
                            solicitudPagofaseId: this.solicitudPagofaseId,
                            solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                            esPreconstruccion: true
                        }
                    ]
                }
                if ( fase[0].codigo === this.faseContrato.construccion ) {
                    return [
                        {
                            solicitudPagofaseId: this.solicitudPagofaseId,
                            solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                            esPreconstruccion: false
                        }
                    ]
                }
            } else {
                return null;
            }
        };

        if ( faseSeleccionada !== null ) {
            const fase: Dominio[] = this.fasesArray.filter( fase => fase.codigo === faseSeleccionada.codigo );
            if ( fase[0].codigo === this.faseContrato.preConstruccion ) {
                tieneFasePreConstruccion = true;
            }
            if ( fase[0].codigo === this.faseContrato.construccion ) {
                tieneFaseConstruccion = true;
            }
        }
        const pSolicitudPago = {
            solicitudPagoId: this.solicitudPagoId,
            contratoId: this.contrato.contratoId,
            solicitudPagoRegistrarSolicitudPago: [
                {
                  solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId,
                  solicitudPagoId: this.solicitudPagoId,
                  tieneFasePreconstruccion: tieneFasePreConstruccion,
                  tieneFaseConstruccion: tieneFaseConstruccion,
                  fechaSolicitud: new Date( this.addressForm.get( 'fechaSolicitud' ).value ).toISOString(),
                  numeroRadicadoSAC: this.addressForm.get( 'numeroRadicado' ).value,
                  solicitudPagofase: pago()
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
