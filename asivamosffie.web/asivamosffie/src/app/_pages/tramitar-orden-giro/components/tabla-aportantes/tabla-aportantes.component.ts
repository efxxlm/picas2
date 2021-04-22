import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-aportantes',
  templateUrl: './tabla-aportantes.component.html',
  styleUrls: ['./tabla-aportantes.component.scss']
})
export class TablaAportantesComponent implements OnInit {

    @Input() tablaUsoFuenteAportante: any[] = [];
    dataSource = new MatTableDataSource();
    displayedColumns: string[]  = [
        'uso',
        'fuente',
        'aportante',
        'valorUso',
        'saldoActual'
    ];
    dataTable = [];

    constructor() { }

    ngOnInit(): void {
        this.tablaUsoFuenteAportante.forEach( registro => {
            const registroObj = {
                nombreUso: registro.nombreUso,
                fuentes: registro.fuentes,
                aportante: registro.fuentes[ registro.fuentes.length -1 ].aportante,
                valorUso: registro.fuentes[ registro.fuentes.length -1 ].aportante[ registro.fuentes[ registro.fuentes.length -1 ].aportante.length -1 ].valorUso,
                saldoActualUso: registro.fuentes[ registro.fuentes.length -1 ].aportante[ registro.fuentes[ registro.fuentes.length -1 ].aportante.length -1 ].saldoActualUso
            }

            this.dataTable.push( registroObj );
        } )

        this.dataSource = new MatTableDataSource( this.dataTable );
    }

}
