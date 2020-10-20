import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { FaseUnoConstruccionService } from '../../../../core/_services/faseUnoConstruccion/fase-uno-construccion.service';

@Component({
  selector: 'app-tabla-requisitos-tecnicos',
  templateUrl: './tabla-requisitos-tecnicos.component.html',
  styleUrls: ['./tabla-requisitos-tecnicos.component.scss']
})
export class TablaRequisitosTecnicosComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 
    'fechaAprobacion',
    'numeroContrato',
    'cantidadProyectosAsociados',
    'cantidadProyectosRequisitosAprobados',
    'cantidadProyectosRequisitosPendientes',
    'estadoNombre',
    'gestion'
  ];

  constructor ( private routes: Router,
                private faseUnoConstruccionSvc: FaseUnoConstruccionService )
  {
    this.faseUnoConstruccionSvc.getContractsGrid()
      .subscribe( listas => {
        this.dataSource                        = new MatTableDataSource( listas );
        this.dataSource.paginator              = this.paginator;
        this.dataSource.sort                   = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      } );
  }

  ngOnInit(): void {
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  getForm ( id: number, fechaPoliza: string ) {
    this.routes.navigate( [ '/requisitosTecnicosConstruccion/gestionarInicioContrato', id ], { state: { fechaPoliza } } )
  };

  aprobarInicio ( id: number ) {
    console.log( id );
  };

  verDetalle ( id: number ) {
    this.routes.navigate( [ '/requisitosTecnicosConstruccion/verDetalles', id ] )
  }

};