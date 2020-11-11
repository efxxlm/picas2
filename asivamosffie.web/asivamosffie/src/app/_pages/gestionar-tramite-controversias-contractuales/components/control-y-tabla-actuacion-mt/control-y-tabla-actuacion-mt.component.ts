import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-control-y-tabla-actuacion-mt',
  templateUrl: './control-y-tabla-actuacion-mt.component.html',
  styleUrls: ['./control-y-tabla-actuacion-mt.component.scss']
})
export class ControlYTablaActuacionMtComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaActualizacion',
    'actuacion',
    'numeroActuacion',
    'estadoRegistro',
    'estadoActuacion',
    'gestion',
  ];
  constructor() { }

  ngOnInit(): void {
  }

}
