import { FormArray, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
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
    @Input() esVerDetalle = false;
    @Input() idSolicitud: any;
    @Output() estadoSemaforo = new EventEmitter<string>();
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
        const solicitudPagoRegistrarSolicitudPago = this.contrato.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[ 0 ]

        LISTA_PROYECTOS.forEach( proyecto => {
            let check = null

            if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
                if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined && solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                    const fase = solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.find( solicitudPagoFase => solicitudPagoFase.contratacionProyectoId === proyecto.contratacionProyectoId )

                    if ( fase !== undefined ) {
                        check = true
                    }
                }
            }
            
            proyecto.check = check

            this.projects.push(
                this.fb.group(
                    {
                        check: [ check ],
                        registroCompleto: [ null ],
                        contratacionProyectoId: [ proyecto.contratacionProyectoId ],
                        llaveMen: [ proyecto.llaveMen ],
                        tipoIntervencion: [ proyecto.tipoIntervencion ],
                        valorTotal: [ proyecto.valorTotal ]
                    }
                )
            );
        } )

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

    checkRegistroCompleto() {
        const proyectosTrue = this.projects.controls.filter( control => control.get( 'check' ).value === true )

        if ( proyectosTrue.length > 0 ) {
            const registrosCompleto = proyectosTrue.filter( control => control.get( 'registroCompleto' ).value === true )
            const registrosEnProceso = proyectosTrue.filter( control => control.get( 'registroCompleto' ).value === false )

            if ( registrosCompleto.length > 0 ) {
                if ( registrosCompleto.length === proyectosTrue.length ) {
                    this.estadoSemaforo.emit( 'completo' )
                }

                if ( registrosCompleto.length < proyectosTrue.length && registrosEnProceso.length > 0 ) {
                    this.estadoSemaforo.emit( 'en-proceso' )
                }
            }

            if ( registrosEnProceso.length > 0 ) {
                if ( registrosEnProceso.length === proyectosTrue.length ) {
                    this.estadoSemaforo.emit( 'en-proceso' )
                }
            }

            if ( registrosCompleto.length === 0 && registrosEnProceso.length === 0 ) {
                this.estadoSemaforo.emit( 'sin-diligenciar' )
            }
        }
    }

}
