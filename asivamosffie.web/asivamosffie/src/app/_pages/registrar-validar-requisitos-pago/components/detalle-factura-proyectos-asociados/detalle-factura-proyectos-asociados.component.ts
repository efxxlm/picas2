import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-detalle-factura-proyectos-asociados',
  templateUrl: './detalle-factura-proyectos-asociados.component.html',
  styleUrls: ['./detalle-factura-proyectos-asociados.component.scss']
})
export class DetalleFacturaProyectosAsociadosComponent implements OnInit {

    @Input() solicitudPago: any;
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
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
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
                                        if ( proyectos[1].length > 1 ) {
                                            this.esMultiProyecto = true;
                                            for( const proyecto of proyectos[1] ) {
                                                proyecto.check = false;
                                            }
                                            this.solicitudPagoFaseCriterio.forEach( criterioValue => {
                                                const criterioSelect = criterios.filter( value => value.codigo === criterioValue.tipoCriterioCodigo );
                                                if ( criterioSelect.length > 0 ) {
                                                    criterioSelect[0][ 'solicitudPagoFaseCriterioId' ] = criterioValue.solicitudPagoFaseCriterioId;
                                                    criteriosArray.push( criterioSelect[0] );
                                                }
                                            } );
                                            for ( const proyecto of proyectos[1] ) {
                                                this.projects.push(
                                                    this.fb.group(
                                                        {
                                                            check: [ false ],
                                                            listaCriterios: [ criteriosArray ],
                                                            contratacionProyectoId: [ proyecto.contratacionProyectoId ],
                                                            llaveMen: [ proyecto.llaveMen ],
                                                            criterioPago: [ null ],
                                                            solicitudPagoFaseCriterioProyectoId: [ 0 ],
                                                            solicitudPagoFaseCriterioId: [ 0 ],
                                                            valorFacturado: [ 0 ],
                                                            criteriosProyecto: this.fb.array( [] )
                                                        }
                                                    )
                                                );
                                            }
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
                        criterio => {
                            console.log( criterio );
                        }
                    );
            }
        }
    };

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);

        return alphanumeric.test(inputChar) ? true : false;
    }

    criteriosProyecto( i: number ) {
        return this.projects.controls[i].get( 'criteriosProyecto' ) as FormArray;
    }

    getvalues( criteriosDePago: any[], index: number ) {
        if ( criteriosDePago.length > 0 ) {
            criteriosDePago.forEach( criterioValue => {
                this.criteriosProyecto( index ).push(
                    this.fb.group(
                        {
                            nombre: [ criterioValue.nombre ],
                            valorFacturado: [ '' ]
                        }
                    )
                );
            } );
        }
    }

    projectSelect() {
        const projectsArray: any = this.dataSource.data;
        for ( const project of projectsArray ) {
            this.projects.controls.forEach( projectValue => {
                if ( projectValue.value.contratacionProyectoId === project.contratacionProyectoId ) {
                    if ( project.check === true ) {
                        console.log( 'condicion 1' );
                        projectValue.get( 'check' ).setValue( true );
                    }
                    if ( project.check === false ) {
                        console.log( 'condicion 2' )
                        projectValue.get( 'check' ).setValue( false );
                    }
                }
            } );
        }
    }

    guardar() {
        console.log( this.projects.value );
    }

}
