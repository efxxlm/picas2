import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-contratos-obra-vrtc',
  templateUrl: './tabla-contratos-obra-vrtc.component.html',
  styleUrls: ['./tabla-contratos-obra-vrtc.component.scss']
})
export class TablaContratosObraVrtcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 
    'fechaAprobacion',
    'numeroContratoObra',
    'proyectosAsociados',
    'proyectosAprobados',
    'proyectosPendientes',
    'estadoRequisito',
    'gestion'
  ];
  dataTable: any [] = [
    {
      fechaAprobacion: '06/08/2020',
      numeroContratoObra: 'C326326',
      proyectosAsociados: '2',
      proyectosAprobados: '2',
      proyectosPendientes: '0',
      estadoRequisito: '1',
      id: 1
    },
    {
      fechaAprobacion: '05/08/2020',
      numeroContratoObra: 'A208208',
      proyectosAsociados: '1',
      proyectosAprobados: '0',
      proyectosPendientes: '1',
      estadoRequisito: '1',
      id: 2
    },
    {
      fechaAprobacion: '01/08/2020',
      numeroContratoObra: 'C801801',
      proyectosAsociados: '1',
      proyectosAprobados: '0',
      proyectosPendientes: '1',
      estadoRequisito: '3',
      id: 3
    }
  ]

  constructor ( private routes: Router ) { }

  ngOnInit(): void {
    this.dataSource                        = new MatTableDataSource( this.dataTable );
    this.dataSource.paginator              = this.paginator;
    this.dataSource.sort                   = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  getForm ( id: number ) {
    this.routes.navigate( [ '/verificarRequisitosTecnicosConstruccion/verificarRequisitosInicio', id ] )
  };

  aprobarInicio ( id: number ) {
    console.log( id );
  };

  verDetalle ( id: number ) {
    this.routes.navigate( [ '/verificarRequisitosTecnicosConstruccion/verDetalleObra', id ] )
  }
}
