import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router, ActivatedRoute } from '@angular/router';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-detalle-factura-proyectos',
  templateUrl: './detalle-factura-proyectos.component.html',
  styleUrls: ['./detalle-factura-proyectos.component.scss']
})
export class DetalleFacturaProyectosComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() aprobarSolicitudPagoId: any
    @Input() criteriosPagoProyectoCodigo: string;
    @Input() solicitudPagoCargarFormaPago: any;
    @Input() esPreconstruccion = true;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoObservacionId = 0;
    esMultiProyecto = false;
    proyectos: any;
    listaCriterios: Dominio[] = [];
    criteriosArraySeleccionados: Dominio[] = [];
    solicitudPagoFaseCriterio: any;
    solicitudPagoFaseCriterioProyecto: any;
    solicitudPagoFase: any;
    dataSource = new MatTableDataSource();
    montoMaximo: { valorMaximoProyecto: number, valorPendienteProyecto: number };
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'llaveMen',
      'tipoIntervencion',
      'departamento',
      'municipio',
      'institucionEducativa',
      'sede'
    ];
    addressForm: FormGroup;

    get projects() {
        return this.addressForm.get( 'projects' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        this.getProyectos();
    };

    getProyectos() {
        if ( this.solicitudPago !== undefined ) {
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

            this.solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio;

            if ( this.solicitudPagoFase.esPreconstruccion === true ) {
                this.registrarPagosSvc.getCriterioByFormaPagoCodigo( this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo )
                    .subscribe(
                        criterios => {
                            this.registrarPagosSvc.getProyectosByIdContrato( this.solicitudPago.contratoId )
                                .subscribe(
                                    async proyectos => {
                                        const criteriosArray = [];
                                        const criteriosSeleccionados = [];
                                        let criteriosDiligenciados = [];
                                        this.listaCriterios = criterios;
                                        this.solicitudPagoFaseCriterio.forEach( criterio => {
                                            this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                        } );
                                        if ( proyectos[1].length > 1 ) {
                                            this.esMultiProyecto = true;
                                            this.solicitudPagoFaseCriterio.forEach( criterioValue => {
                                                const criterioSelect = criterios.filter( value => value.codigo === criterioValue.tipoCriterioCodigo );
                                                if ( criterioSelect.length > 0 ) {
                                                    criterioSelect.forEach( value => {
                                                        value[ 'solicitudPagoFaseCriterioId' ] = criterioValue.solicitudPagoFaseCriterioId;
                                                        criteriosArray.push( value );
                                                    } );
                                                }
                                                if ( criterioValue.solicitudPagoFaseCriterioProyecto.length > 0 ) {
                                                    criteriosDiligenciados.push( criterios.filter( value => value.codigo === criterioValue.tipoCriterioCodigo )[0] );
                                                    criterioValue.solicitudPagoFaseCriterioProyecto.forEach( criterioProyectoValue => {
                                                        criterioSelect.forEach( value => {
                                                            criteriosSeleccionados.push(
                                                                this.fb.group(
                                                                    {
                                                                        tipoCriterioCodigo: [ value.codigo ],
                                                                        nombre: [ value.nombre ],
                                                                        contratacionProyectoId: [ criterioProyectoValue.contratacionProyectoId ],
                                                                        solicitudPagoFaseCriterioProyectoId: [ criterioProyectoValue.solicitudPagoFaseCriterioProyectoId ],
                                                                        valorFacturado: [ criterioProyectoValue.valorFacturado ]
                                                                    }
                                                                )
                                                            )
                                                        } )
                                                    } );
                                                }
                                            } );
                                            for ( const proyecto of proyectos[1] ) {
                                                const criteriosProyectoSeleccionados = criteriosSeleccionados.filter( criterioValue => criterioValue.value.contratacionProyectoId === proyecto.contratacionProyectoId );
                                                const criterioPagoArray = [];
                                                if ( criteriosSeleccionados.length > 0 ) {
                                                    criteriosSeleccionados.forEach( criterioValue => {
                                                        if ( criterioValue.value.contratacionProyectoId === proyecto.contratacionProyectoId ) {
                                                            proyecto.check = true;
                                                        }
                                                    } );
                                                    if ( proyecto.check === undefined ) {
                                                        proyecto.check = false;
                                                    }
                                                } else {
                                                    proyecto.check = false;
                                                }
                                                criteriosDiligenciados.forEach( value => {
                                                    criteriosProyectoSeleccionados.forEach( criterioValue => {
                                                        if ( value.codigo === criterioValue.value.tipoCriterioCodigo ) {
                                                            criterioPagoArray.push( value );
                                                        }
                                                    } );
                                                } );

                                                const montoMaximo = await this.registrarPagosSvc.getMontoMaximoProyecto( this.solicitudPago.contratoId, proyecto.contratacionProyectoId, 'True' );

                                                this.projects.push(
                                                    this.fb.group(
                                                        {
                                                            check: [ criteriosProyectoSeleccionados.length > 0 ? true : false ],
                                                            listaCriterios: [ criteriosArray ],
                                                            valorMaximoProyecto: [ montoMaximo.valorMaximoProyecto ],
                                                            valorPendienteProyecto: [ montoMaximo.valorPendienteProyecto ],
                                                            contratacionProyectoId: [ proyecto.contratacionProyectoId ],
                                                            llaveMen: [ proyecto.llaveMen ],
                                                            criterioPago: [ criterioPagoArray.length > 0 ? criterioPagoArray : null ],
                                                            solicitudPagoFaseCriterioId: [ 0 ],
                                                            criteriosProyecto: this.fb.array( criteriosProyectoSeleccionados.length > 0 ? criteriosProyectoSeleccionados : [] ),
                                                            tieneObservaciones: [null, Validators.required],
                                                            observaciones:[null, Validators.required]
                                                        }
                                                    )
                                                );
                                            }
                                        }
                                        if ( proyectos[1].length < 2 ) {
                                            this.solicitudPagoFaseCriterio.forEach( criterio => {
                                                this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                            } );
                                            this.proyectos = proyectos[1];
                                            const montoMaximo = await this.registrarPagosSvc.getMontoMaximoProyecto( this.solicitudPago.contratoId, this.proyectos[0].contratacionProyectoId, 'True' );
                                            this.montoMaximo = montoMaximo;
                                        }
                                        this.dataSource = new MatTableDataSource( proyectos[1] );
                                        this.dataSource.paginator = this.paginator;
                                        this.dataSource.sort = this.sort;
                                    }
                                );
                        }
                    );
            }
            if ( this.solicitudPagoFase.esPreconstruccion === false ) {
                this.registrarPagosSvc.getCriterioByFormaPagoCodigo( this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo )
                    .subscribe(
                        criterios => {
                            this.registrarPagosSvc.getProyectosByIdContrato( this.solicitudPago.contratoId )
                                .subscribe(
                                    async proyectos => {
                                        const criteriosArray = [];
                                        const criteriosSeleccionados = [];
                                        let criteriosDiligenciados = [];
                                        this.listaCriterios = criterios;
                                        this.solicitudPagoFaseCriterio.forEach( criterio => {
                                            this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                        } );
                                        if ( proyectos[1].length > 1 ) {
                                            this.esMultiProyecto = true;
                                            this.solicitudPagoFaseCriterio.forEach( criterioValue => {
                                                const criterioSelect = criterios.filter( value => value.codigo === criterioValue.tipoCriterioCodigo );
                                                if ( criterioSelect.length > 0 ) {
                                                    criterioSelect.forEach( value => {
                                                        value[ 'solicitudPagoFaseCriterioId' ] = criterioValue.solicitudPagoFaseCriterioId;
                                                        criteriosArray.push( value );
                                                    } );
                                                }
                                                if ( criterioValue.solicitudPagoFaseCriterioProyecto.length > 0 ) {
                                                    criteriosDiligenciados.push( criterios.filter( value => value.codigo === criterioValue.tipoCriterioCodigo )[0] );
                                                    criterioValue.solicitudPagoFaseCriterioProyecto.forEach( criterioProyectoValue => {
                                                        criterioSelect.forEach( value => {
                                                            criteriosSeleccionados.push(
                                                                this.fb.group(
                                                                    {
                                                                        tipoCriterioCodigo: [ value.codigo ],
                                                                        nombre: [ value.nombre ],
                                                                        contratacionProyectoId: [ criterioProyectoValue.contratacionProyectoId ],
                                                                        solicitudPagoFaseCriterioProyectoId: [ criterioProyectoValue.solicitudPagoFaseCriterioProyectoId ],
                                                                        valorFacturado: [ criterioProyectoValue.valorFacturado ]
                                                                    }
                                                                )
                                                            )
                                                        } )
                                                    } );
                                                }
                                            } );
                                            for ( const proyecto of proyectos[1] ) {
                                                const criteriosProyectoSeleccionados = criteriosSeleccionados.filter( criterioValue => criterioValue.value.contratacionProyectoId === proyecto.contratacionProyectoId );
                                                const criterioPagoArray = [];
                                                if ( criteriosSeleccionados.length > 0 ) {
                                                    criteriosSeleccionados.forEach( criterioValue => {
                                                        if ( criterioValue.value.contratacionProyectoId === proyecto.contratacionProyectoId ) {
                                                            proyecto.check = true;
                                                        }
                                                    } );
                                                    if ( proyecto.check === undefined ) {
                                                        proyecto.check = false;
                                                    }
                                                } else {
                                                    proyecto.check = false;
                                                }
                                                criteriosDiligenciados.forEach( value => {
                                                    criteriosProyectoSeleccionados.forEach( criterioValue => {
                                                        if ( value.codigo === criterioValue.value.tipoCriterioCodigo ) {
                                                            criterioPagoArray.push( value );
                                                        }
                                                    } );
                                                } );


                                                const montoMaximo = await this.registrarPagosSvc.getMontoMaximoProyecto( this.solicitudPago.contratoId, proyecto.contratacionProyectoId, 'False' );

                                                this.projects.push(
                                                    this.fb.group(
                                                        {
                                                            check: [ criteriosProyectoSeleccionados.length > 0 ? true : false ],
                                                            listaCriterios: [ criteriosArray ],
                                                            valorMaximoProyecto: [ montoMaximo.valorMaximoProyecto ],
                                                            valorPendienteProyecto: [ montoMaximo.valorPendienteProyecto ],
                                                            contratacionProyectoId: [ proyecto.contratacionProyectoId ],
                                                            llaveMen: [ proyecto.llaveMen ],
                                                            criterioPago: [ criterioPagoArray.length > 0 ? criterioPagoArray : null ],
                                                            solicitudPagoFaseCriterioId: [ 0 ],
                                                            criteriosProyecto: this.fb.array( criteriosProyectoSeleccionados.length > 0 ? criteriosProyectoSeleccionados : [] ),
                                                            tieneObservaciones: [null, Validators.required],
                                                            observaciones:[null, Validators.required]
                                                        }
                                                    )
                                                );
                                            }
                                        } else {
                                            this.solicitudPagoFaseCriterio.forEach( criterio => {
                                                this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                            } );
                                            this.proyectos = proyectos[1];
                                            const montoMaximo = await this.registrarPagosSvc.getMontoMaximoProyecto( this.solicitudPago.contratoId, this.proyectos[0].contratacionProyectoId, 'False' );
                                            this.montoMaximo = montoMaximo;
                                        }
                                        this.dataSource = new MatTableDataSource( proyectos[1] );
                                        this.dataSource.paginator = this.paginator;
                                        this.dataSource.sort = this.sort;
                                    }
                                );
                        }
                    );
            }
        }
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
        projects: this.fb.array( [] )
      })
    }

    filterCriterios( tipoCriterioCodigo: string ) {
        if ( this.listaCriterios.length > 0 ) {
            const criterio = this.listaCriterios.filter( criterio => criterio.codigo === tipoCriterioCodigo );
            return criterio[0].nombre;
        }
    }

    criteriosProyecto( i: number ) {
        return this.projects.controls[i].get( 'criteriosProyecto' ) as FormArray;
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

}
