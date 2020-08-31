import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-verificar-cumplimientos',
  templateUrl: './tabla-verificar-cumplimientos.component.html',
  styleUrls: ['./tabla-verificar-cumplimientos.component.scss']
})
export class TablaVerificarCumplimientosComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, {static: true} ) sort: MatSort;
  @Input() data: any[] = [];
  displayedColumns: string[] = [ 'tarea', 'responsable', 'fechaCumplimiento', 'fechaReporte', 'estadoReporte', 'gestionRealizada', 'gestion' ];
  columnas: any[] = [
    { titulo: 'Tarea', name: 'tarea' },
    { titulo: 'Responsable', name: 'responsable' },
    { titulo: 'Fecha de cumplimiento', name: 'fechaCumplimiento' },
    { titulo: 'Fecha del reporte', name: 'fechaReporte' },
    { titulo: 'Estado reportado', name: 'estadoReporte' }
  ];

  estado: any[] = [
    {value: 'sinIniciar', viewValue: 'Sin iniciar'},
    {value: 'enProceso', viewValue: 'En proceso'},
    {value: 'finalizada', viewValue: 'Finalizada'}
  ];

  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.data );
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }

}
