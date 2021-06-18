import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';


@Component({
  selector: 'app-table-detalle-proyectos-de-la-solicitud',
  templateUrl: './table-detalle-proyectos-de-la-solicitud.component.html',
  styleUrls: ['./table-detalle-proyectos-de-la-solicitud.component.scss']
})
export class TableDetalleProyectosDeLaSolicitudComponent implements OnInit {

  dataAportantes = new MatTableDataSource();
  @Input() data: any[] = [];
  @Input() displayedColumns: string[] = [];
  @Input() ELEMENT_DATA: any[] = [];

  constructor() {}

  ngOnInit(): void {
    this.dataAportantes = new MatTableDataSource(this.data);
  }
}
