import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-general-rapg',
  templateUrl: './tabla-general-rapg.component.html',
  styleUrls: ['./tabla-general-rapg.component.scss']
})
export class TablaGeneralRapgComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaActualizacion',
    'numContrato',
    'contratista',
    'numeroPoliza',
    'numeroActualizacion',
    'estadoActualizacionPoliza',
    'estadoRegistro',
    'gestion'
  ];
  dataTable: any[] = [
    {
      fechaActualizacion: '19/01/2021',
      numContrato: 'N801801',
      contratista: 'Interventores S.A',
      numeroPoliza: '0000-9868',
      numeroActualizacion: 'Act_0001',
      estadoActualizacionPoliza: 'En revisión de actualización de póliza',
      estadoRegistro: 'Incompleto',
      gestion: 1
    },
  ];
  constructor(private routes: Router) { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }
  actualizarPoliza(id){
    
  }
}
