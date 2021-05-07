import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-info-fuenterec-gog',
  templateUrl: './tabla-info-fuenterec-gog.component.html',
  styleUrls: ['./tabla-info-fuenterec-gog.component.scss']
})
export class TablaInfoFuenterecGogComponent implements OnInit {

    @Input() tablaInformacionFuenteRecursos: any[];
    dataSource = new MatTableDataSource();
    displayedColumns: string[] = [
        'nombreAportante',
        'fuenteRecursos',
        'saldoActualFRecursos',
        'saldoValorFacturado'
    ];

    constructor() { }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource( this.tablaInformacionFuenteRecursos );
    }

}
