import { Router, ActivatedRoute } from '@angular/router';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-obs-registrar-solicitud-pago',
  templateUrl: './obs-registrar-solicitud-pago.component.html',
  styleUrls: ['./obs-registrar-solicitud-pago.component.scss']
})
export class ObsRegistrarSolicitudPagoComponent implements OnInit {

    @Input() contrato: any;
    @Input() esVerDetalle = false;
    @Input() aprobarSolicitudPagoId: any;
    @Input() registrarSolicitudPago: any;
    @Output() estadoSemaforoRegistroSolicitud = new EventEmitter<string>();
    solicitudPagoObservacionId = 0;
    solicitudPago: any;
    solicitudPagoFase: any;
    solicitudPagoCargarFormaPago: any;
    manejoAnticipoRequiere: boolean;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'faseContrato',
        'pagosRealizados',
        'valorFacturado',
        'porcentajeFacturado',
        'saldoPorPagar',
        'porcentajePorPagar'
    ];
    addressForm: FormGroup;
    editorStyle = {
      height: '45px',
      overflow: 'auto'
    };
    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };
    estadoSemaforosAcordeonesPrincipales = {
        estadoSemaforoObsPrincipal: 'sin-diligenciar',
        semaforoAcordeonFase: 'sin-diligenciar',
        semaforoAcordeonDescuentosTecnica: 'sin-diligenciar'
    }
    estadoSemaforos = {
        semaforoAcordeonCriterios: 'sin-diligenciar',
        semaforoAcordeonAmortizacion: 'sin-diligenciar',
        semaforoAcordeonDetalleFacturaProyecto: 'sin-diligenciar',
        semaforoAcordeonDatosFactura: 'sin-diligenciar'
    }
    estaEditando = false;

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private activatedRoute: ActivatedRoute,
        private dialog: MatDialog,
        private obsMultipleSvc: ObservacionesMultiplesCuService )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.contrato !== undefined ) {
            this.solicitudPago = this.contrato.solicitudPagoOnly;
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];

            this.manejoAnticipoRequiere = this.contrato.contratoConstruccion.length > 0 ? this.contrato.contratoConstruccion[0].manejoAnticipoRequiere : false;

            if ( this.contrato.solicitudPago.length > 1 ) {
                this.solicitudPagoCargarFormaPago = this.contrato.solicitudPago[0].solicitudPagoCargarFormaPago[0];
            } else {
                this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
            }

            if ( this.solicitudPagoFase.esPreconstruccion === true ) {
                // Validacion si es preconstruccion eliminar el campo de amortizacion de los semaforos.
                delete this.estadoSemaforos.semaforoAcordeonAmortizacion;
            }
            this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                this.aprobarSolicitudPagoId,
                this.solicitudPago.solicitudPagoId,
                this.solicitudPagoFase.solicitudPagoFaseId,
                this.registrarSolicitudPago.registrarSolicitudPagoCodigo )
                .subscribe(
                    response => {
                        const obsSupervisor = response.filter( obs => obs.archivada === false )[0];
                        
                        if ( obsSupervisor !== undefined ) {
                            this.solicitudPagoObservacionId = obsSupervisor.solicitudPagoObservacionId;
                            if ( obsSupervisor.registroCompleto === false ) {
                                this.estadoSemaforosAcordeonesPrincipales.estadoSemaforoObsPrincipal = 'en-proceso';
                            }
                            if ( obsSupervisor.registroCompleto === true ) {
                                this.estadoSemaforosAcordeonesPrincipales.estadoSemaforoObsPrincipal = 'completo';
                            }
                            this.getSemaforoStatus();
                            this.estaEditando = true;
                            this.addressForm.markAllAsTouched();
                            this.addressForm.setValue(
                                {
                                    fechaCreacion: obsSupervisor.fechaCreacion,
                                    tieneObservaciones: obsSupervisor.tieneObservacion !== undefined ? obsSupervisor.tieneObservacion : null,
                                    observaciones: obsSupervisor.observacion !== undefined ? ( obsSupervisor.observacion.length > 0 ? obsSupervisor.observacion : null ) : null
                                }
                            );
                        }
                    }
                );
        }

        this.dataSource = new MatTableDataSource( this.contrato.vContratoPagosRealizados );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    crearFormulario() {
      return this.fb.group({
        fechaCreacion: [ null ],
        tieneObservaciones: [null, Validators.required],
        observaciones:[null, Validators.required],
      })
    }

    getSemaforoStatus( estadoAcordeon?: string, tipoAcordeon?: string ) {
        // Get semaforos acordeon preconstruccion
        if ( this.solicitudPagoFase.esPreconstruccion === true ) {
            if ( tipoAcordeon === 'criteriosPago' ) {
                this.estadoSemaforos.semaforoAcordeonCriterios = estadoAcordeon;
            }
        }
        // Get semaforos acordeon construccion
        if ( this.solicitudPagoFase.esPreconstruccion === false ) {
            // Get semaforo criterios de pago
            if ( tipoAcordeon === 'criteriosPago' ) {
                this.estadoSemaforos.semaforoAcordeonCriterios = estadoAcordeon;
            }
            // Get semaforo amortizacion del anticipo
            if ( tipoAcordeon === 'amortizacion' ) {
                this.estadoSemaforos.semaforoAcordeonAmortizacion = estadoAcordeon;
            }
            // Get semaforo detalle factura para proyectos asociados
            if ( tipoAcordeon === 'detalleFactura' ) {
                this.estadoSemaforos.semaforoAcordeonDetalleFacturaProyecto = estadoAcordeon;
            }
            // Get semaforo datos de la factura
            if ( tipoAcordeon === 'datosFactura' ) {
                this.estadoSemaforos.semaforoAcordeonDatosFactura = estadoAcordeon;
            }
            // Get semaforo fase
            const sinDiligenciar = Object.values( this.estadoSemaforos ).includes( 'sin-diligenciar' );
            const enProceso = Object.values( this.estadoSemaforos ).includes( 'en-proceso' );
            const completo = Object.values( this.estadoSemaforos ).includes( 'completo' );

            if ( enProceso === true ) {
                this.estadoSemaforosAcordeonesPrincipales.semaforoAcordeonFase = 'en-proceso';
            }
            if ( sinDiligenciar === true && completo === true ) {
                this.estadoSemaforosAcordeonesPrincipales.semaforoAcordeonFase = 'en-proceso';
            }
            if ( sinDiligenciar === false && enProceso === false && completo === true ) {
                this.estadoSemaforosAcordeonesPrincipales.semaforoAcordeonFase = 'completo';
            }
            // Get semaforo descuentos direccion tecnica
            if ( tipoAcordeon === 'descuentosTecnica' ) {
                this.estadoSemaforosAcordeonesPrincipales.semaforoAcordeonDescuentosTecnica = estadoAcordeon;
            }
            // Get semaforo registrar solicitud de pago
            const sinDiligenciarPrincipal = Object.values( this.estadoSemaforosAcordeonesPrincipales ).includes( 'sin-diligenciar' );
            const enProcesoPrincipal = Object.values( this.estadoSemaforosAcordeonesPrincipales ).includes( 'en-proceso' );
            const completoPrincipal = Object.values( this.estadoSemaforosAcordeonesPrincipales ).includes( 'completo' );

            if ( sinDiligenciarPrincipal === true && enProcesoPrincipal === false && completoPrincipal === false ) {
                this.estadoSemaforoRegistroSolicitud.emit( 'sin-diligenciar' );
            }
            if ( enProcesoPrincipal === true ) {
                this.estadoSemaforoRegistroSolicitud.emit( 'en-proceso' );
            }
            if ( sinDiligenciarPrincipal === true && completoPrincipal === true ) {
                this.estadoSemaforoRegistroSolicitud.emit( 'en-proceso' );
            }
            if ( sinDiligenciarPrincipal === false && enProcesoPrincipal === false && completoPrincipal === true ) {
                this.estadoSemaforoRegistroSolicitud.emit( 'completo' );
            }
        }
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        if ( this.addressForm.get( 'tieneObservaciones' ).value !== null && this.addressForm.get( 'tieneObservaciones' ).value === false ) {
            this.addressForm.get( 'observaciones' ).setValue( '' );
        }

        const pSolicitudPagoObservacion = {
            solicitudPagoObservacionId: this.solicitudPagoObservacionId,
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            observacion: this.addressForm.get( 'observaciones' ).value !== null ? this.addressForm.get( 'observaciones' ).value : this.addressForm.get( 'observaciones' ).value,
            tipoObservacionCodigo: this.registrarSolicitudPago.registrarSolicitudPagoCodigo,
            menuId: this.aprobarSolicitudPagoId,
            idPadre: this.solicitudPagoFase.solicitudPagoFaseId,
            tieneObservacion: this.addressForm.get( 'tieneObservaciones' ).value !== null ? this.addressForm.get( 'tieneObservaciones' ).value : this.addressForm.get( 'tieneObservaciones' ).value
        };

        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion( pSolicitudPagoObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/verificarSolicitudPago/aprobacionSolicitud',  this.activatedRoute.snapshot.params.idContrato, this.activatedRoute.snapshot.params.idSolicitudPago
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            )
    }

}
