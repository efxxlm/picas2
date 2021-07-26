import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-detalle-factura-proyectos',
  templateUrl: './detalle-factura-proyectos.component.html',
  styleUrls: ['./detalle-factura-proyectos.component.scss']
})
export class DetalleFacturaProyectosComponent implements OnInit {

    @Input() contrato: any = undefined;
    @Input() listaMenusId: any;
    @Input() registrarSolicitudPagoObs: any;
    @Input() esVerDetalle = false;
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

}
