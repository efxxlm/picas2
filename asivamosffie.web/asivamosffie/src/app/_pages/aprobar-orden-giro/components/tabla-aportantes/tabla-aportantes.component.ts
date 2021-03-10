import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-aportantes',
  templateUrl: './tabla-aportantes.component.html',
  styleUrls: ['./tabla-aportantes.component.scss']
})
export class TablaAportantesComponent implements OnInit {

    dataSource = new MatTableDataSource();
    displayedColumns: string[]  = [
        'uso',
        'aportante',
        'valorUso',
        'saldoActual'
    ];
    dataTable = [
        {
            uso: 'Diseño Obra Complementaria',
            aportantes: [
                {
                    nombre: 'Alcaldía de Susacón',
                    valorUso: 45000000,
                    saldoActualUso: 45000000

                },
                {
                    nombre: 'Fundación Pies Descalzos',
                    valorUso: 30000000,
                    saldoActualUso: 30000000

                }
            ]
        },
        {
            uso: 'Estudios y diseños',
            aportantes: [
                {
                    nombre: 'Alcaldía de Susacón',
                    valorUso: 30000000,
                    saldoActualUso: 15000000

                }
            ]
        }
    ];

    constructor() { }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource( this.dataTable );
    }

}
