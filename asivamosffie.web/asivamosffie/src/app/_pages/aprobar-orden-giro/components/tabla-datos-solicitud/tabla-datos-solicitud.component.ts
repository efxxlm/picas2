import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-datos-solicitud',
  templateUrl: './tabla-datos-solicitud.component.html',
  styleUrls: ['./tabla-datos-solicitud.component.scss']
})
export class TablaDatosSolicitudComponent implements OnInit {

    @Input() listProyectos
    dataSource = new MatTableDataSource();
    displayedColumns: string[] = [
      'llaveMen',
      'tipoIntervencion',
      'departamento',
      'municipio',
      'institucionEducativa',
      'sede'
    ];

    constructor() { }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource( this.listProyectos );
    }

}
