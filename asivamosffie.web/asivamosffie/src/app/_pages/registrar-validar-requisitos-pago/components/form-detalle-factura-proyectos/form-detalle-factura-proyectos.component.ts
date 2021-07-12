import { FormArray, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-form-detalle-factura-proyectos',
  templateUrl: './form-detalle-factura-proyectos.component.html',
  styleUrls: ['./form-detalle-factura-proyectos.component.scss']
})
export class FormDetalleFacturaProyectosComponent implements OnInit {

    @Input() contrato: any;
    @Input() listaMenusId: any;
    @Input() registrarSolicitudPagoObs: any;
    solicitudPago: any;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    formProject = this.fb.group(
        {
            projects: this.fb.array( [] )
        }
    );
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

    //Get proyectos
    get projects() {
        return this.formProject.get( 'projects' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
        this.getProyectos();
    }

    async getProyectos() {
        this.solicitudPago = this.contrato.solicitudPagoOnly
        const getProyectosByIdContrato = await this.registrarPagosSvc.getProyectosByIdContrato( this.contrato.contratoId ).toPromise()
        const LISTA_PROYECTOS: any[] = getProyectosByIdContrato[ 1 ]

        LISTA_PROYECTOS.forEach( proyecto => {
            proyecto.check = null

            this.projects.push(
                this.fb.group(
                    {
                        check: [ null ],
                        contratacionProyectoId: [ proyecto.contratacionProyectoId ],
                        llaveMen: [ proyecto.llaveMen ],
                        tipoIntervencion: [ proyecto.tipoIntervencion ],
                        valorTotal: [ proyecto.valorTotal ]
                    }
                )
            );
        } )

        console.log( LISTA_PROYECTOS );
        this.dataSource = new MatTableDataSource( LISTA_PROYECTOS );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

    projectSelect() {
        const projectsArray: any = this.dataSource.data;
        for ( const project of projectsArray ) {
            this.projects.controls.forEach( projectValue => {
                if ( projectValue.value.contratacionProyectoId === project.contratacionProyectoId ) {
                    if ( project.check === true ) {
                        projectValue.get( 'check' ).setValue( true )
                    }
                    if ( project.check === false ) {
                        projectValue.get( 'check' ).setValue( false )
                    }
                }
            } );
        }
    }

}
