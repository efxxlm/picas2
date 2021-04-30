import { Router, ActivatedRoute } from '@angular/router';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-obs-criterio-pagos',
  templateUrl: './obs-criterio-pagos.component.html',
  styleUrls: ['./obs-criterio-pagos.component.scss']
})
export class ObsCriterioPagosComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() esPreconstruccion = true;
    @Input() aprobarSolicitudPagoId: any;
    @Input() criteriosPagoFacturaCodigo: string;
    @Input() solicitudPagoCargarFormaPago: any;
    @Output() estadoSemaforo = new EventEmitter<string>();
    solicitudPagoObservacionId = 0;
    solicitudPagoRegistrarSolicitudPago: any;
    montoMaximoPendiente: { montoMaximo: number, valorPendientePorPagar: number };
    criteriosArray: { codigo: string, nombre: string }[] = [];
    listaCriterios: Dominio[] = [];
    criteriosArraySeleccionados: Dominio[] = [];
    addressForm: FormGroup;
    solicitudPagoFase: any;
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
    estaEditando = false;

    get criterios() {
        return this.addressForm.get( 'criterios' ) as FormArray;
    }

    getConceptos( index: number ) {
        return this.criterios.controls[ index ].get( 'conceptos' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private activatedRoute: ActivatedRoute,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private dialog: MatDialog,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private commonSvc: CommonService )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        this.getCriterios();
        if ( this.solicitudPago !== undefined ) {
            this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                this.aprobarSolicitudPagoId,
                this.solicitudPago.solicitudPagoId,
                this.solicitudPagoFase.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioId,
                this.criteriosPagoFacturaCodigo )
                .subscribe(
                    response => {
                        const obsSupervisor = response.filter( obs => obs.archivada === false )[0];

                        if ( obsSupervisor !== undefined ) {  
                            if ( obsSupervisor.registroCompleto === false ) {
                                this.estadoSemaforo.emit( 'en-proceso' );
                            }
                            if ( obsSupervisor.registroCompleto === true ) {
                                this.estadoSemaforo.emit( 'completo' );
                            }
                            this.estaEditando = true;
                            this.addressForm.markAllAsTouched();
                            this.solicitudPagoObservacionId = obsSupervisor.solicitudPagoObservacionId;
                            this.addressForm.get( 'fechaCreacion' ).setValue( obsSupervisor.fechaCreacion );
                            this.addressForm.get( 'tieneObservaciones' ).setValue( obsSupervisor.tieneObservacion !== undefined ? obsSupervisor.tieneObservacion : null );
                            this.addressForm.get( 'observaciones' ).setValue( obsSupervisor.observacion !== undefined ? ( obsSupervisor.observacion.length > 0 ? obsSupervisor.observacion : null ) : null );
                        }
                    }
                );
        }
    }

    getCriterios() {
        this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];
        if ( this.esPreconstruccion === true ) {
            if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                    for ( const solicitudPagoFase of this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                        if ( solicitudPagoFase.esPreconstruccion === true ) {
                            this.solicitudPagoFase = solicitudPagoFase;
                        }
                    }
                }
            }
        }
        if ( this.esPreconstruccion === false ) {
            if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                    for ( const solicitudPagoFase of this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                        if ( solicitudPagoFase.esPreconstruccion === false ) {
                            this.solicitudPagoFase = solicitudPagoFase;
                        }
                    }
                }
            }
        }

        if ( this.esPreconstruccion === true ) {
            const fasePreConstruccionFormaPagoCodigo = this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo;

            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( fasePreConstruccionFormaPagoCodigo )
            .subscribe(
                async response => {
                    const criteriosSeleccionadosArray = [];

                    if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                        for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
                            // GET Criterio seleccionado
                            const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                            criteriosSeleccionadosArray.push( criterioSeleccionado[0] );
                            // GET tipos de pago
                            const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                            const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                            // GET conceptos de pago
                            const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                            const conceptoDePagoArray = [];
                            const conceptosDePagoSeleccionados = [];
                            // Get conceptos de pago
                            if ( criterio.solicitudPagoFaseCriterioConceptoPago.length > 0 ) {
                                criterio.solicitudPagoFaseCriterioConceptoPago.forEach( solicitudPagoFaseCriterioConceptoPago => {
                                    if ( conceptosDePago.filter( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ).length > 0 ) {
                                        conceptosDePagoSeleccionados.push( conceptosDePago.filter( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio )[0] );
                                    }
                                    conceptoDePagoArray.push(
                                        this.fb.group(
                                            {
                                                solicitudPagoFaseCriterioConceptoPagoId: [ solicitudPagoFaseCriterioConceptoPago.solicitudPagoFaseCriterioConceptoPagoId ],
                                                solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                                conceptoPagoCriterioNombre: [ conceptosDePago.filter( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio )[0].nombre ],
                                                conceptoPagoCriterio: [ solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ],
                                                valorFacturadoConcepto: [ solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto !== undefined ? solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto : null ]
                                            }
                                        )
                                    );
                                } );
                            }
                            this.criterios.push(
                                this.fb.group(
                                    {
                                        solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                        solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                        tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                        nombreCriterio: [ criterioSeleccionado[0].nombre ],
                                        tiposDePago: [ tiposDePago ],
                                        tipoPago: [ tipoDePago.length > 0 ? tipoDePago[0] : null ],
                                        conceptosDePago: [ conceptosDePago ],
                                        conceptoPago: [ conceptosDePagoSeleccionados, Validators.required ],
                                        conceptos: this.fb.array( conceptoDePagoArray ),
                                        valorFacturado: [ { value: criterio.valorFacturado !== undefined ? criterio.valorFacturado : null, disabled: true }, Validators.required ]
                                    }
                                )
                            );
                        }
                    }
                    this.criteriosArray = response;
                    this.criteriosArraySeleccionados = criteriosSeleccionadosArray;
                }
            );
        }
        if ( this.esPreconstruccion === false ) {
            const faseConstruccionFormaPagoCodigo = this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo;

            this.registrarPagosSvc.getCriterioByFormaPagoCodigo( faseConstruccionFormaPagoCodigo )
            .subscribe(
                async response => {
                    const criteriosSeleccionadosArray = [];

                    if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                        for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
                            // GET Criterio seleccionado
                            const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                            criteriosSeleccionadosArray.push( criterioSeleccionado[0] );
                            // GET tipos de pago
                            const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                            const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                            // GET conceptos de pago
                            const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                            const conceptoDePagoArray = [];
                            const conceptosDePagoSeleccionados = [];
                            // Get conceptos de pago
                            if ( criterio.solicitudPagoFaseCriterioConceptoPago.length > 0 ) {
                                criterio.solicitudPagoFaseCriterioConceptoPago.forEach( solicitudPagoFaseCriterioConceptoPago => {
                                    if ( conceptosDePago.filter( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ).length > 0 ) {
                                        conceptosDePagoSeleccionados.push( conceptosDePago.filter( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio )[0] );
                                    }
                                    conceptoDePagoArray.push(
                                        this.fb.group(
                                            {
                                                solicitudPagoFaseCriterioConceptoPagoId: [ solicitudPagoFaseCriterioConceptoPago.solicitudPagoFaseCriterioConceptoPagoId ],
                                                solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                                conceptoPagoCriterioNombre: [ conceptosDePago.filter( concepto => concepto.codigo === solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio )[0].nombre ],
                                                conceptoPagoCriterio: [ solicitudPagoFaseCriterioConceptoPago.conceptoPagoCriterio ],
                                                valorFacturadoConcepto: [ solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto !== undefined ? solicitudPagoFaseCriterioConceptoPago.valorFacturadoConcepto : null ]
                                            }
                                        )
                                    );
                                } );
                            }
                            this.criterios.push(
                                this.fb.group(
                                    {
                                        solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                                        solicitudPagoFaseCriterioId: [ criterio.solicitudPagoFaseCriterioId ],
                                        tipoCriterioCodigo: [ criterio.tipoCriterioCodigo ],
                                        nombreCriterio: [ criterioSeleccionado[0].nombre ],
                                        tiposDePago: [ tiposDePago ],
                                        tipoPago: [ tipoDePago[0], Validators.required ],
                                        conceptosDePago: [ conceptosDePago, Validators.required ],
                                        conceptoPago: [ conceptosDePagoSeleccionados, Validators.required ],
                                        conceptos: this.fb.array( conceptoDePagoArray ),
                                        valorFacturado: [ { value: criterio.valorFacturado !== undefined ? criterio.valorFacturado : null, disabled: true }, Validators.required ]
                                    }
                                )
                            );
                        }
                    }
                    this.criteriosArray = response;
                    this.criteriosArraySeleccionados = criteriosSeleccionadosArray;
                }
            );
        }
    }

    crearFormulario() {
      return this.fb.group({
        fechaCreacion: [ null ],
        tieneObservaciones: [null, Validators.required],
        observaciones:[null, Validators.required],
        criterios: this.fb.array( [] )
      })
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

    filterCriterios( tipoCriterioCodigo: string ) {
        if ( this.listaCriterios.length > 0 ) {
            const criterio = this.listaCriterios.filter( criterio => criterio.codigo === tipoCriterioCodigo );
            return criterio[0].nombre;
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
            tipoObservacionCodigo: this.criteriosPagoFacturaCodigo,
            menuId: this.aprobarSolicitudPagoId,
            idPadre: this.solicitudPagoFase.solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioId,
            tieneObservacion: this.addressForm.get( 'tieneObservaciones' ).value !== null ? this.addressForm.get( 'tieneObservaciones' ).value : this.addressForm.get( 'tieneObservaciones' ).value
        };

        console.log( pSolicitudPagoObservacion );
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
