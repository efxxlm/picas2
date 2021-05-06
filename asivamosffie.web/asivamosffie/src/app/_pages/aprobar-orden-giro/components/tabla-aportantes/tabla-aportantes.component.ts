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
            const valorUso = [];
            const saldoActualUso = [];

            registro.fuentes[ registro.fuentes.length -1 ].aportante.forEach( aportante => {
                valorUso.push( aportante.valorUso[ 0 ].valor )
                saldoActualUso.push( aportante.valorUso[ 0 ].valorActual )
            } )

            const registroObj = {
                nombreUso: registro.nombreUso,
                fuentes: registro.fuentes,
                aportante: registro.fuentes[ registro.fuentes.length -1 ].aportante,
                valorUso,
                saldoActualUso
            }

            this.dataTable.push( registroObj );
        } )

        this.dataSource = new MatTableDataSource( this.dataTable );
    }

}
