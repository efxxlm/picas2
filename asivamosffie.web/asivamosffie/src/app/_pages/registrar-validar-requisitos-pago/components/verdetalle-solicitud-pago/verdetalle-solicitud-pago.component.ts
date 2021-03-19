import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { DialogProyectosAsociadosComponent } from '../dialog-proyectos-asociados/dialog-proyectos-asociados.component';

@Component({
  selector: 'app-verdetalle-solicitud-pago',
  templateUrl: './verdetalle-solicitud-pago.component.html',
  styleUrls: ['./verdetalle-solicitud-pago.component.scss']
})
export class VerdetalleSolicitudPagoComponent implements OnInit {

    dataSource = new MatTableDataSource();
    dataSourceRegistrarSolicitud = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'drp',
      'numDrp',
      'valor',
      'saldo'
    ];
    displayedColumnsRegistrarSolicitud: string[] = [
        'faseContrato',
        'pagosRealizados',
        'valorFacturado',
        'porcentajeFacturado',
        'saldoPorPagar',
        'porcentajePorPagar'
      ];
    addressForm = this.fb.group({
        criterios: this.fb.array( [] )
    });
    montoMaximoPendiente: { montoMaximo: number, valorPendientePorPagar: number };
    listaCriterios: Dominio[] = [];
    formaPagoArray: Dominio[] = [];
    modalidadContratoArray: Dominio[] = [];
    criteriosArray: { codigo: string, nombre: string }[] = [];
    criteriosArraySeleccionados: Dominio[] = [];
    tipoSolicitudCodigo: any = {};
    contrato: any;
    solicitudPagoCargarFormaPago: any;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoFase: any;
    

    get criterios() {
        return this.addressForm.get( 'criterios' ) as FormArray;
    }

    getConceptos( index: number ) {
        return this.criterios.controls[ index ].get( 'conceptos' ) as FormArray;
    }

    constructor(
        private activatedRoute: ActivatedRoute,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private commonSvc: CommonService )
    {
        this.commonSvc.formasDePago()
            .subscribe( response => this.formaPagoArray = response );
        this.getContrato();
    }

    ngOnInit(): void {
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    openProyectosAsociados() {
        if ( this.contrato === undefined ) {
            return;
        }

        const dialogRef = this.dialog.open( DialogProyectosAsociadosComponent, {
            width: '80em',
            data: { contrato: this.contrato }
        });
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
                            if ( this.contrato.solicitudPagoOnly.tipoSolicitudCodigo !== this.tipoSolicitudCodigo.otrosCostos ) {
                                if ( this.contrato.solicitudPago.length > 1 ) {
                                    this.solicitudPagoCargarFormaPago = this.contrato.solicitudPago[0].solicitudPagoCargarFormaPago[0];
                                } else {
                                    this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
                                }
                                this.solicitudPagoRegistrarSolicitudPago = this.contrato.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0];
                                this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];
                                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0].esPreconstruccion === true ) {
                                    const fasePreConstruccionFormaPagoCodigo = this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo;
                                    this.registrarPagosSvc.getMontoMaximoMontoPendiente( this.contrato.solicitudPagoOnly.solicitudPagoId, fasePreConstruccionFormaPagoCodigo, 'False' )
                                        .subscribe(
                                            getMontoMaximoMontoPendiente => {
                                                this.montoMaximoPendiente = getMontoMaximoMontoPendiente;
                                                this.registrarPagosSvc.getCriterioByFormaPagoCodigo( fasePreConstruccionFormaPagoCodigo )
                                                    .subscribe(
                                                        async response => {
                                                            const criteriosSeleccionadosArray = [];
                                                            this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];

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
                                        );
                                }
                                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0].esPreconstruccion === false ) {
                                    const faseConstruccionFormaPagoCodigo = this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo;
                                    this.registrarPagosSvc.getMontoMaximoMontoPendiente( this.contrato.solicitudPagoOnly.solicitudPagoId, faseConstruccionFormaPagoCodigo, 'False' )
                                        .subscribe(
                                            getMontoMaximoMontoPendiente => {
                                                this.montoMaximoPendiente = getMontoMaximoMontoPendiente;
                                                this.registrarPagosSvc.getCriterioByFormaPagoCodigo( faseConstruccionFormaPagoCodigo )
                                                    .subscribe(
                                                        async response => {
                                                            const criteriosSeleccionadosArray = [];
                                                            this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];

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
                                        );
                                }
    
                                this.dataSource = new MatTableDataSource( this.contrato.valorFacturadoContrato );
                                this.dataSourceRegistrarSolicitud = new MatTableDataSource( this.contrato.vContratoPagosRealizados );
                            }
                            console.log( this.contrato );
                          }
                        );
                }
            );
    }

    getFormaPago( formaPagoCodigo: string ) {
        if ( this.formaPagoArray.length > 0 ) {
            const forma = this.formaPagoArray.filter( forma => forma.codigo === formaPagoCodigo );
            return forma[0].nombre;
        }
    }

    getModalidadContrato( modalidadCodigo: string ) {
        if ( this.modalidadContratoArray.length > 0 ) {
            const modalidad = this.modalidadContratoArray.filter( modalidad => modalidad.codigo === modalidadCodigo );
            return modalidad[0].nombre;
        }
    }

    filterCriterios( tipoCriterioCodigo: string ) {
        if ( this.listaCriterios.length > 0 ) {
            const criterio = this.listaCriterios.filter( criterio => criterio.codigo === tipoCriterioCodigo );
            return criterio[0].nombre;
        }
    }

}
