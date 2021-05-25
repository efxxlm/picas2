import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-infofrecursos-gbftrec',
  templateUrl: './tabla-infofrecursos-gbftrec.component.html',
  styleUrls: ['./tabla-infofrecursos-gbftrec.component.scss']
})
export class TablaInfofrecursosGbftrecComponent implements OnInit {

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
