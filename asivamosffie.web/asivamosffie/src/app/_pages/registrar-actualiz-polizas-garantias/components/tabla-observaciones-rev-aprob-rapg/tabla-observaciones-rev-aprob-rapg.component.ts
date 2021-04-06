import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-observaciones-rev-aprob-rapg',
  templateUrl: './tabla-observaciones-rev-aprob-rapg.component.html',
  styleUrls: ['./tabla-observaciones-rev-aprob-rapg.component.scss']
})
export class TablaObservacionesRevAprobRapgComponent implements OnInit {

    @Input() historialObservaciones: any[] = [];
    displayedColumns: string[] = ['fechaRevision', 'observacion', 'estadoRevisionCodigo'];
    dataSource = new MatTableDataSource();
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    constructor() { }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource( this.historialObservaciones );
    }

}
