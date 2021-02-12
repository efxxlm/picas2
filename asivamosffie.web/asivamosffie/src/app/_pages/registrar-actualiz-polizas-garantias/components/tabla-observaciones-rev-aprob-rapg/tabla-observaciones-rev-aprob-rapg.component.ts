import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-observaciones-rev-aprob-rapg',
  templateUrl: './tabla-observaciones-rev-aprob-rapg.component.html',
  styleUrls: ['./tabla-observaciones-rev-aprob-rapg.component.scss']
})
export class TablaObservacionesRevAprobRapgComponent implements OnInit {
  displayedColumns: string[] = ['fechaRevision', 'observacion', 'estadoRevisionCodigo'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataTable: any[] = [{
    fechaRevision: '20/01/2021',
    observacion: 'Se devuelve la solicitud porque no se cuenta con la aprobación de la vigencia por parte del responsable',
    estadoRevisionCodigo: '1'
  }];

  constructor() { }

  ngOnInit(): void {
    this.cargarTablaDeDatos();
  }
  cargarTablaDeDatos(){
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
}
}
