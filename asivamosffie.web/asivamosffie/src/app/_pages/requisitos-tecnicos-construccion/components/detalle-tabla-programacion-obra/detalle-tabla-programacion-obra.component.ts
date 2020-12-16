import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogObservacionesProgramacionComponent } from '../dialog-observaciones-programacion/dialog-observaciones-programacion.component';

@Component({
  selector: 'app-detalle-tabla-programacion-obra',
  templateUrl: './detalle-tabla-programacion-obra.component.html',
  styleUrls: ['./detalle-tabla-programacion-obra.component.scss']
})
export class DetalleTablaProgramacionObraComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 
    'fechaCargue',
    'totalRegistros',
    'registrosValidos',
    'registrosInvalidos',
    'gestion'
  ];
  dataTable: any [] = [
    {
      fechaCargue: '10/08/2020',
      totalRegistros: '5',
      registrosValidos: '3',
      registrosInvalidos: '2',
      gestion: 1,
    }
  ]
  constructor(private dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.dataSource                        = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator              = this.paginator;
    this.dataSource.sort                   = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  
  addObservaciones(){
    const dialogCargarProgramacion = this.dialog.open( DialogObservacionesProgramacionComponent, {
      width: '75em'
    });

    dialogCargarProgramacion.afterClosed().subscribe( resp => {
      console.log( resp );
    } );
  }

}
