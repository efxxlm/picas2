import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-tabla-programacion-obra',
  templateUrl: './tabla-programacion-obra.component.html',
  styleUrls: ['./tabla-programacion-obra.component.scss']
})
export class TablaProgramacionObraComponent implements OnInit {

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

  constructor() {
  }

  ngOnInit(): void {
    this.dataSource                        = new MatTableDataSource();
    this.dataSource.paginator              = this.paginator;
    this.dataSource.sort                   = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

};