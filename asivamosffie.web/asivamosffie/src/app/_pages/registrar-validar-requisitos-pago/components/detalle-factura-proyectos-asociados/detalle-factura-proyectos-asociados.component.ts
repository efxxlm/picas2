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

    //Get proyectos
    get projects() {
        return this.formProject.get( 'projects' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private registrarPagoSvc: RegistrarRequisitosPagoService )
    { }

    ngOnInit(): void {
        this.registrarPagoSvc.getProyectosByIdContrato( this.solicitudPago.contratoId )
        .subscribe(
            response => {
                if ( response[1].length > 1 ) {
                    this.esMultiProyecto = true;
                    for( const proyecto of response[1] ) {
                        proyecto.check = null;
                    }
                }
                this.dataSource = new MatTableDataSource( response[1] );
                this.dataSource.paginator = this.paginator;
                this.dataSource.sort = this.sort;
            }
        );
    };

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    };

    projectSelect() {
        const projectsArray: any = this.dataSource.data;
        this.projects.clear();
        for ( const project of projectsArray ) {
            console.log( project );
            if ( project.check === true ) {
                this.projects.push(
                    this.fb.group(
                        {
                            contratacionProyectoId: 0,
                            llaveMen: project.llaveMen,
                            solicitudPagoFaseCriterioProyectoId: 0,
                            solicitudPagoFaseCriterioId: 0,
                            valorFacturado: 0
                        }
                    )
                );
            }
        }
    }

    guardar() {
        console.log( this.projects.value );
    }

}
