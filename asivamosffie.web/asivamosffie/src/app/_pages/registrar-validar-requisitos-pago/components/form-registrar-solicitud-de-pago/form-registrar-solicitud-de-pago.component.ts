import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { Router } from '@angular/router';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-form-registrar-solicitud-de-pago',
  templateUrl: './form-registrar-solicitud-de-pago.component.html',
  styleUrls: ['./form-registrar-solicitud-de-pago.component.scss']
})
export class FormRegistrarSolicitudDePagoComponent implements OnInit {

    dataSource = new MatTableDataSource();
    @Input() contrato: any;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    solicitudPagoId = 0;
    solicitudPagoRegistrarSolicitudPagoId = 0;
    solicitudPagofaseId = 0;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoFase: any;
    displayedColumns: string[] = [
      'faseContrato',
      'pagosRealizados',
      'valorFacturado',
      'porcentajeFacturado',
      'saldoPorPagar',
      'porcentajePorPagar'
    ];
    dataTable: any[] = [
      {
        faseContrato: 'Fase 1 - Preconstrucción',
        pagosRealizados: '0',
        valorFacturado: '0',
        porcentajeFacturado: '0',
        saldoPorPagar: '$30.000.000',
        porcentajePorPagar: '100%',
      }
    ];
    addressForm = this.fb.group({
      fechaSolicitud: [null, Validators.required],
      numeroRadicado: [null, Validators.required],
      faseContrato: [null, Validators.required]
    });
    fasesArray: Dominio[] = [];
    faseContrato: any = {};
    postConstruccion = '3';
    contratacionProyectoId = 0;
    estadoRegistroCompleto = {
        formRegistroCompleto: false,
        solicitudPagoFaseRegistroCompleto: false
    }
    estadoRegistroCompletoSubAcordeon = {
        criterioRegistroCompleto: false,
        amortizacionRegistroCompleto: false
    }
    

    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
        this.commonSvc.listaFases()
            .subscribe(
                response => {
                    if ( this.contrato.contratacion.contratacionProyecto.length  > 0 && this.contrato.contratacion.contratacionProyecto.length < 2 ) {
                        this.contratacionProyectoId = this.contrato.contratacion.contratacionProyecto[0].contratacionProyectoId;
                    }
                    response.forEach( ( fase, index ) => {
                        if ( fase.codigo === this.postConstruccion ) {
                            response.splice( index, 1 );
                        }
                    } );
                    response.forEach( fase => {
                        if ( fase.codigo === '1' ) {
                            this.faseContrato.preConstruccion = fase.codigo;
                        }
                        if ( fase.codigo === '2' ) {
                            this.faseContrato.construccion = fase.codigo;
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

                            this.addressForm.setValue(
                                {
                                    fechaSolicitud: this.solicitudPagoRegistrarSolicitudPago.fechaSolicitud !== undefined ? new Date( this.solicitudPagoRegistrarSolicitudPago.fechaSolicitud ) : null,
                                    numeroRadicado: this.solicitudPagoRegistrarSolicitudPago.numeroRadicadoSac !== undefined ? this.solicitudPagoRegistrarSolicitudPago.numeroRadicadoSac : null,
                                    faseContrato: faseSeleccionada !== undefined ? faseSeleccionada : null
                                }
                            );
                            // hasValue in Object Form

                            this.estadoRegistroCompleto.formRegistroCompleto = !Object.values( this.addressForm.value ).includes( null );
                            if ( this.estadoRegistroCompleto.formRegistroCompleto === true ) {
                                this.addressForm.get( 'fechaSolicitud' ).disable();
                                this.addressForm.get( 'numeroRadicado' ).disable();
                                this.addressForm.get( 'faseContrato' ).disable();
                            }
                        }
                    }
                    // Tabla pendiente por integrar
                    this.dataSource = new MatTableDataSource(this.dataTable);
                    this.dataSource.paginator = this.paginator;
                    this.dataSource.sort = this.sort;
                }
            );
    };

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    enabledAcordeonFase( registroCompleto: boolean, esPreconstruccion?: boolean ) {
        if ( registroCompleto === undefined || registroCompleto === null ) {
            return 'en-alerta';
        }

        // Acordeon fase preconstruccion
        if ( registroCompleto === true && esPreconstruccion === true ) {

            let semaforoPreConstruccion = 'sin-diligenciar';

            if ( this.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoFase.registroCompleto === false ) {
                    semaforoPreConstruccion = 'en-proceso';
                }
                if ( this.solicitudPagoFase.registroCompleto === true ) {
                    semaforoPreConstruccion = 'completo';
                    this.estadoRegistroCompleto.solicitudPagoFaseRegistroCompleto = true;
                }
            }

            return semaforoPreConstruccion;
        }

        // Acordeon fase construccion
        if ( registroCompleto === true && esPreconstruccion === false ) {

            let semaforoConstruccion = 'sin-diligenciar';

            if ( this.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoFase.registroCompleto === false ) {
                    semaforoConstruccion = 'en-proceso';
                }
                if ( this.solicitudPagoFase.registroCompleto === true ) {
                    semaforoConstruccion = 'completo';
                    this.estadoRegistroCompleto.solicitudPagoFaseRegistroCompleto = true;
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
                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio[0].registroCompleto === true ) {
                            if ( this.contrato.contratacion.contratacionProyecto.length > 1 ) {
                                
                                let totalCriterioRegistroCompleto = 0;

                                if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
    
                                    for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
                                        if ( criterio.registroCompleto === true ) {
                                            totalCriterioRegistroCompleto++;
                                        }
                                    }
                
                                    if ( totalCriterioRegistroCompleto > 0 && totalCriterioRegistroCompleto === this.solicitudPagoFase.solicitudPagoFaseCriterio.length ) {
                                        return 'completo';
                                    }
                                    if ( totalCriterioRegistroCompleto > 0 && totalCriterioRegistroCompleto !== this.solicitudPagoFase.solicitudPagoFaseCriterio.length ) {
                                        return 'en-proceso';
                                    }
                                    if ( totalCriterioRegistroCompleto === 0 ) {
                                        return 'sin-diligenciar';
                                    }
                
                                }

                            } else {
                                return '';
                            }
                        }
                    } else {
                        return 'en-alerta';
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
                        semaforoCriterioPago = 'completo';
                    }

                    return semaforoCriterioPago;
                }
                if ( tipoAcordeon === 'amortizacion' ) {
    
                    if ( this.estadoRegistroCompletoSubAcordeon.criterioRegistroCompleto === false ) {
                        return 'en-alerta';
                    }
                    if ( this.estadoRegistroCompletoSubAcordeon.criterioRegistroCompleto === true ) {
                        
                        const solicitudPagoFaseAmortizacion = this.solicitudPagoFase.solicitudPagoFaseAmortizacion[0];
                        let semaforoAmortizacion = 'sin-diligenciar';
    
                        if ( solicitudPagoFaseAmortizacion.registroCompleto === false ) {
                            semaforoAmortizacion = 'en-proceso';
                        }
                        if ( solicitudPagoFaseAmortizacion.registroCompleto === true ) {
                            semaforoAmortizacion = 'completo';
                            this.estadoRegistroCompletoSubAcordeon.amortizacionRegistroCompleto = true;
                        }
    
                        return semaforoAmortizacion;
                    }
    
                }
                if ( tipoAcordeon === 'detalleFactura' ) {
                    if ( this.estadoRegistroCompletoSubAcordeon.amortizacionRegistroCompleto === true ) {
                        if ( this.solicitudPagoFase.solicitudPagoFaseCriterio[0].registroCompleto === true ) {
                            if ( this.contrato.contratacion.contratacionProyecto.length > 1 ) {
                                
                                let totalCriterioRegistroCompleto = 0;

                                if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
    
                                    for ( const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio ) {
                                        if ( criterio.registroCompleto === true ) {
                                            totalCriterioRegistroCompleto++;
                                        }
                                    }
                
                                    if ( totalCriterioRegistroCompleto > 0 && totalCriterioRegistroCompleto === this.solicitudPagoFase.solicitudPagoFaseCriterio.length ) {
                                        return 'completo';
                                    }
                                    if ( totalCriterioRegistroCompleto > 0 && totalCriterioRegistroCompleto !== this.solicitudPagoFase.solicitudPagoFaseCriterio.length ) {
                                        return 'en-proceso';
                                    }
                                    if ( totalCriterioRegistroCompleto === 0 ) {
                                        return 'sin-diligenciar';
                                    }
                
                                }

                            } else {
                                return '';
                            }
                        }
                    } else {
                        return 'en-alerta';
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
        console.log( pSolicitudPago );
        this.registrarPagosSvc.createEditNewPayment( pSolicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
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
