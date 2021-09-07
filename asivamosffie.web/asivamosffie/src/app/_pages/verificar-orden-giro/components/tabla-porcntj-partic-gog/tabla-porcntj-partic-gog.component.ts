import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-porcntj-partic-gog',
  templateUrl: './tabla-porcntj-partic-gog.component.html',
  styleUrls: ['./tabla-porcntj-partic-gog.component.scss']
})
export class TablaPorcntjParticGogComponent implements OnInit {

    @Input() tablaPorcentajeParticipacion: any[];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = ['drp', 'numeroDRP', 'nombreAportante', 'porcParticipacion'];

    constructor() { }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource( this.tablaPorcentajeParticipacion );
    }
}
