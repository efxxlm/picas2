import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { DialogObservacionesFlujoRecursosComponent } from '../dialog-observaciones-flujo-recursos/dialog-observaciones-flujo-recursos.component';

@Component({
  selector: 'app-tabla-inversion-recursos',
  templateUrl: './tabla-inversion-recursos.component.html',
  styleUrls: ['./tabla-inversion-recursos.component.scss']
})
export class TablaInversionRecursosComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 
    'fechaCargue',
    'totalRegistros',
    'registrosValidos',
    'registrosInvalidos',
    'estadoCargue',
    'gestion'
  ];
  dataTable: any [] = [
    {
      fechaCargue: '10/08/2020',
      totalRegistros: '5',
      registrosValidos: '3',
      registrosInvalidos: '2',
      estadoCargue: 'Validos',
      gestion: 1,
    }
  ]
  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource                        = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator              = this.paginator;
    this.dataSource.sort                   = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  
  addObservaciones(){
   const dialogCargarProgramacion = this.dialog.open( DialogObservacionesFlujoRecursosComponent, {
      width: '75em'
    });

    dialogCargarProgramacion.afterClosed().subscribe( resp => {
      console.log( resp );
    } );
  }
}
