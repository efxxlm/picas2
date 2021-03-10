import { Dominio } from './../../../../core/_services/common/common.service';
import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-detalle-factura-proyectos-asociados',
  templateUrl: './detalle-factura-proyectos-asociados.component.html',
  styleUrls: ['./detalle-factura-proyectos-asociados.component.scss']
})
export class DetalleFacturaProyectosAsociadosComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumnsMultiProyecto: string[] = [
      'llaveMen',
      'tipoIntervencion',
      'departamento',
      'municipio',
      'institucionEducativa',
      'sede',
      'validar'
    ];
    displayedColumns: string[] = [
        'llaveMen',
        'tipoIntervencion',
        'departamento',
        'municipio',
        'institucionEducativa',
        'sede'
    ];
    formProject = this.fb.group(
        {
            projects: this.fb.array( [] )
        }
    );
    esMultiProyecto = false;
    proyectos: any;
    listaCriterios: Dominio[] = [];
    criteriosArraySeleccionados: Dominio[] = [];
    solicitudPagoFaseCriterio: any;
    solicitudPagoFaseCriterioProyecto: any;
    solicitudPagoCargarFormaPago: any;
    solicitudPagoFase: any;

    //Get proyectos
    get projects() {
        return this.formProject.get( 'projects' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
        this.getProyectos();
    };

    getProyectos() {
        if ( this.solicitudPago !== undefined ) {
            this.solicitudPagoCargarFormaPago = this.solicitudPago.solicitudPagoCargarFormaPago[0];
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
            this.solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio;

            if ( this.solicitudPagoFase.esPreconstruccion === true ) {
                this.registrarPagosSvc.getCriterioByFormaPagoCodigo( this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo )
                    .subscribe(
                        criterios => {
                            this.registrarPagosSvc.getProyectosByIdContrato( this.solicitudPago.contratoId )
                                .subscribe(
                                    proyectos => {
                                        const criteriosArray = [];
                                        const criteriosSeleccionados = [];
                                        let criteriosDiligenciados = [];
                                        this.listaCriterios = criterios;
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
                                                this.projects.push(
                                                    this.fb.group(
                                                        {
                                                            check: [ criteriosProyectoSeleccionados.length > 0 ? true : false ],
                                                            listaCriterios: [ criteriosArray ],
                                                            contratacionProyectoId: [ proyecto.contratacionProyectoId ],
                                                            llaveMen: [ proyecto.llaveMen ],
                                                            criterioPago: [ criterioPagoArray.length > 0 ? criterioPagoArray : null ],
                                                            solicitudPagoFaseCriterioId: [ 0 ],
                                                            criteriosProyecto: this.fb.array( criteriosProyectoSeleccionados.length > 0 ? criteriosProyectoSeleccionados : [] )
                                                        }
                                                    )
                                                );
                                            }
                                        }
                                        if ( proyectos[1].length < 2 || this.esVerDetalle === true ) {
                                            this.solicitudPagoFaseCriterio.forEach( criterio => {
                                                this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                            } );
                                            this.proyectos = proyectos[1];
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
                                    proyectos => {
                                        const criteriosArray = [];
                                        const criteriosSeleccionados = [];
                                        let criteriosDiligenciados = [];
                                        this.listaCriterios = criterios;
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
                                                this.projects.push(
                                                    this.fb.group(
                                                        {
                                                            check: [ criteriosProyectoSeleccionados.length > 0 ? true : false ],
                                                            listaCriterios: [ criteriosArray ],
                                                            contratacionProyectoId: [ proyecto.contratacionProyectoId ],
                                                            llaveMen: [ proyecto.llaveMen ],
                                                            criterioPago: [ criterioPagoArray.length > 0 ? criterioPagoArray : null ],
                                                            solicitudPagoFaseCriterioId: [ 0 ],
                                                            criteriosProyecto: this.fb.array( criteriosProyectoSeleccionados.length > 0 ? criteriosProyectoSeleccionados : [] )
                                                        }
                                                    )
                                                );
                                            }
                                        } else {
                                            this.solicitudPagoFaseCriterio.forEach( criterio => {
                                                this.criteriosArraySeleccionados.push( this.listaCriterios.filter( criterioValue => criterioValue.codigo === criterio.tipoCriterioCodigo )[0] );
                                            } );
                                            this.proyectos = proyectos[1];
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
    };

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);

        return alphanumeric.test(inputChar) ? true : false;
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

    getvalues( criteriosDePago: any[], index: number ) {
        if ( criteriosDePago.length > 0 ) {
            const criteriosDePagoArray = [ ...criteriosDePago ];

            this.criteriosProyecto( index ).controls.forEach( ( criterio, indexValue ) => {
                criteriosDePagoArray.forEach( ( value, index ) => {
                    if ( value.codigo === criterio.value.tipoCriterioCodigo ) {
                        criteriosDePagoArray.splice( index, 1 );
                    }
                } );
                const test = criteriosDePago.filter( value => value.codigo === criterio.value.tipoCriterioCodigo );
                if ( test.length === 0 ) {
                    this.criteriosProyecto( index ).removeAt( indexValue );
                }
            } );

            criteriosDePagoArray.forEach( criterioValue => {
                this.criteriosProyecto( index ).push(
                    this.fb.group(
                        {
                            tipoCriterioCodigo: [ criterioValue.codigo ],
                            nombre: [ criterioValue.nombre ],
                            solicitudPagoFaseCriterioProyectoId: [ 0 ],
                            valorFacturado: [ '' ]
                        }
                    )
                );
            } );
        } else {
            this.criteriosProyecto( index ).clear();
        }
    }

    projectSelect() {
        const projectsArray: any = this.dataSource.data;
        for ( const project of projectsArray ) {
            this.projects.controls.forEach( projectValue => {
                if ( projectValue.value.contratacionProyectoId === project.contratacionProyectoId ) {
                    if ( project.check === true ) {
                        projectValue.get( 'check' ).setValue( true );
                    }
                    if ( project.check === false ) {
                        projectValue.get( 'check' ).setValue( false );
                    }
                }
            } );
        }
    }

    deleteCriterio( criterioProyecto: FormGroup, index: number, jIndex: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                response => {
                    if ( response === true ) {
                        if ( criterioProyecto.get( 'solicitudPagoFaseCriterioProyectoId' ).value === 0 ) {
                            const criteriosSeleccionados = this.projects.controls[ index ].get( 'criterioPago' ).value;
                            if ( criteriosSeleccionados !== null && criteriosSeleccionados.length > 0 ) {
                                criteriosSeleccionados.forEach( ( criterioValue, index ) => {
                                    if ( criterioValue.codigo === criterioProyecto.get( 'tipoCriterioCodigo' ).value ) {
                                        criteriosSeleccionados.splice( index, 1 );
                                    }
                                } );
                            }
                            this.criteriosProyecto( index ).removeAt( jIndex );
                            this.projects.controls[ index ].get( 'criterioPago' ).setValue( criteriosSeleccionados );
                            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                        } else {
                            const criteriosSeleccionados = this.projects.controls[ index ].get( 'criterioPago' ).value;
                            if ( criteriosSeleccionados !== null && criteriosSeleccionados.length > 0 ) {
                                criteriosSeleccionados.forEach( ( criterioValue, index ) => {
                                    if ( criterioValue.codigo === criterioProyecto.get( 'tipoCriterioCodigo' ).value ) {
                                        criteriosSeleccionados.splice( index, 1 );
                                    }
                                } );
                            }
                            this.criteriosProyecto( index ).removeAt( jIndex );
                            this.projects.controls[ index ].get( 'criterioPago' ).setValue( criteriosSeleccionados );
                            this.registrarPagosSvc.deleteSolicitudPagoFaseCriterioProyecto( criterioProyecto.get( 'solicitudPagoFaseCriterioProyectoId' ).value )
                                .subscribe(
                                    () => this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' ),
                                    err => this.openDialog( '', `<b>${ err.message }</b>` )
                                )
                        }
                    }
                }
            );
    }

    deleteLLave( proyecto: FormGroup, index: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
        .subscribe(
            response => {
                if ( response === true ) {
                    const projectsArray: any = this.dataSource.data;
                    projectsArray.forEach( ( project, indexProject ) => {
                        if ( proyecto.get( 'contratacionProyectoId' ).value === project.contratacionProyectoId ) {
                            this.dataSource.data[ indexProject ][ 'check' ] = false;
                        }
                    } );
                    this.projects.controls[ index ].get( 'check' ).setValue( false );
                    this.registrarPagosSvc.deleteSolicitudLlaveCriterioProyecto( proyecto.get( 'contratacionProyectoId' ).value )
                        .subscribe(
                            () => this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' ),
                            err => this.openDialog( '', `<b>${ err.message }</b>` )
                        );
                }
            }
        )
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    guardar() {
        const proyectos = this.projects.controls.filter( control => control.value.check === true );
        const setProyectos = [];
        if ( proyectos.length > 0 ) {
            proyectos.forEach( proyecto => {
                if ( proyecto.value.criteriosProyecto.length > 0 ) {
                    proyecto.value.criteriosProyecto.forEach( criterioValue => {
                        setProyectos.push(
                            {
                                tipoCriterioCodigo: criterioValue.tipoCriterioCodigo,
                                solicitudPagoFaseCriterioProyectoId: criterioValue.solicitudPagoFaseCriterioProyectoId,
                                solicitudPagoFaseCriterioId: proyecto.value.criterioPago.filter( value => criterioValue.tipoCriterioCodigo === value.codigo )[0].solicitudPagoFaseCriterioId,
                                contratacionProyectoId: proyecto.value.contratacionProyectoId,
                                valorFacturado: criterioValue.valorFacturado
                            }
                        )
                    } );
                }
            } );
        }
        this.solicitudPagoFaseCriterio.forEach( criterioValue => {
            criterioValue.solicitudPagoFaseCriterioProyecto = [];
            setProyectos.forEach( value => {
                if ( criterioValue.tipoCriterioCodigo === value.tipoCriterioCodigo ) {
                    criterioValue.solicitudPagoFaseCriterioProyecto.push( value );
                }
            } );
        } );
        console.log( this.solicitudPagoFaseCriterio );
        this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseCriterio = this.solicitudPagoFaseCriterio;
        this.registrarPagosSvc.createEditNewPayment( this.solicitudPago )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.registrarPagosSvc.getValidateSolicitudPagoId( this.solicitudPago.solicitudPagoId )
                    .subscribe(
                        () => {
                            this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                () => this.routes.navigate(
                                    [
                                        '/registrarValidarRequisitosPago/verDetalleEditar',  this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId
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
