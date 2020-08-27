import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-detalle-compromiso',
  templateUrl: './tabla-detalle-compromiso.component.html',
  styleUrls: ['./tabla-detalle-compromiso.component.scss']
})
export class TablaDetalleCompromisoComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @Input() dataTable;
  displayedColumns: string[] = [ 'fechaRegistro', 'gestionRealizada', 'estadoCompromiso' ];
  ELEMENT_DATA: any[] = [
    {titulo: 'Fecha de registro', name: 'fechaRegistro'},
    { titulo: 'Gestión realizada', name: 'gestionRealizada' }
  ];

  constructor () {
  }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( [this.dataTable] );
  }

}
