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
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'drp',
      'numDrp',
      'valor',
      'saldo'
    ];
    addressForm = this.fb.group({
        criterios: this.fb.array( [] )
    });
    listaCriterios: Dominio[] = [];
    formaPagoArray: Dominio[] = [];
    criteriosArraySeleccionados: Dominio[] = [];
    tipoSolicitudCodigo: any = {};
    contrato: any;
    solicitudPagoCargarFormaPago: any;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoFase: any;
    

    get criterios() {
        return this.addressForm.get( 'criterios' ) as FormArray;
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
                                this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
                                this.solicitudPagoRegistrarSolicitudPago = this.contrato.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0];
                                this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];
                                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0].esPreconstruccion === true ) {
                                    const fasePreConstruccionFormaPagoCodigo = this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo;
                                    this.registrarPagosSvc.getCriterioByFormaPagoCodigo( fasePreConstruccionFormaPagoCodigo )
                                        .subscribe(
                                            response => {
                                                this.listaCriterios = response;
                                                this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( criterio => {
                                                    this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                                } );
                                                const criteriosArray = [];
                                                this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];
                        
                                                if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                                                    this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( async criterio => {
                                                        // GET Criterio seleccionado
                                                        const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                                        criteriosArray.push( criterioSeleccionado[0] );
                                                        // GET tipos de pago
                                                        const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                                        const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                                        // GET conceptos de pago
                                                        const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                                        const conceptoDePago = conceptosDePago.filter( value => value.codigo === criterio.conceptoPagoCriterio );
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
                                                                    conceptoPago: [ conceptoDePago.length > 0 ? conceptoDePago[0] : null ],
                                                                    valorFacturado: [ criterio.valorFacturado !== undefined ? criterio.valorFacturado : null ]
                                                                }
                                                            )
                                                        );
                                                    } );
                                                }
                                            }
                                        );
                                }
                                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0].esPreconstruccion === false ) {
                                    const faseConstruccionFormaPagoCodigo = this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo;
                                    this.registrarPagosSvc.getCriterioByFormaPagoCodigo( faseConstruccionFormaPagoCodigo )
                                        .subscribe(
                                            response => {
                                                this.listaCriterios = response;
                                                this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( criterio => {
                                                    this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                                } );
                                                const criteriosArray = [];
                                                this.solicitudPagoFase = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase[0];
                                                if ( this.solicitudPagoFase.solicitudPagoFaseCriterio.length > 0 ) {
                                                    this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( async criterio => {
                                                        // GET Criterio seleccionado
                                                        const criterioSeleccionado = response.filter( value => value.codigo === criterio.tipoCriterioCodigo );
                                                        criteriosArray.push( criterioSeleccionado[0] );
                                                        // GET tipos de pago
                                                        const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo( criterio.tipoCriterioCodigo );
                                                        const tipoDePago = tiposDePago.filter( value => value.codigo === criterio.tipoPagoCodigo );
                                                        // GET conceptos de pago
                                                        const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo( criterio.tipoPagoCodigo );
                                                        const conceptoDePago = conceptosDePago.filter( value => value.codigo === criterio.conceptoPagoCriterio );
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
                                                                    conceptoPago: [ conceptoDePago[0], Validators.required ],
                                                                    valorFacturado: [ criterio.valorFacturado, Validators.required ]
                                                                }
                                                            )
                                                        );
                                                    } );
                                                }
                                            }
                                        );
                                }
    
                                this.dataSource = new MatTableDataSource( this.contrato.contratacion.disponibilidadPresupuestal );
                                this.dataSource.paginator = this.paginator;
                                this.dataSource.sort = this.sort;
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

    filterCriterios( tipoCriterioCodigo: string ) {
        if ( this.listaCriterios.length > 0 ) {
            const criterio = this.listaCriterios.filter( criterio => criterio.codigo === tipoCriterioCodigo );
            return criterio[0].nombre;
        }
    }

}
