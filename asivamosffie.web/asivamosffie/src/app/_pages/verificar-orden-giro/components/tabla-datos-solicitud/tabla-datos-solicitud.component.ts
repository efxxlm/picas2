import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-datos-solicitud',
  templateUrl: './tabla-datos-solicitud.component.html',
  styleUrls: ['./tabla-datos-solicitud.component.scss']
})
export class TablaDatosSolicitudComponent implements OnInit {

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
        this.dataSource = new MatTableDataSource( [
            {
                llaveMen: 'LL457326',
                tipoIntervencion: 'Remodelación',
                departamento: 'Boyacá',
                municipio: 'Susacón',
                institucionEducativa: 'I.E Nuestra Señora Del Carmen',
                sede: 'Única sede'
            }
        ] );
    }

}
