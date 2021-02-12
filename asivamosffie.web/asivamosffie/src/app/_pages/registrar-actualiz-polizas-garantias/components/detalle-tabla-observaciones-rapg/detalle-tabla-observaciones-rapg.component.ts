import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-detalle-tabla-observaciones-rapg',
  templateUrl: './detalle-tabla-observaciones-rapg.component.html',
  styleUrls: ['./detalle-tabla-observaciones-rapg.component.scss']
})
export class DetalleTablaObservacionesRapgComponent implements OnInit {
  displayedColumns: string[] = ['fechaRevision', 'observacion', 'estadoRevisionCodigo'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataTable: any[] = [{
    fechaRevision: '20/01/2021',
    observacion: '<em> Se devuelve la solicitud porque no se cuenta con la aprobación de la vigencia por parte del responsable </em>',
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
