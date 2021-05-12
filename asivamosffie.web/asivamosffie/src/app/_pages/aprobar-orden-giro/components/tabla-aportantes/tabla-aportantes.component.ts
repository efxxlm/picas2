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
            const aportantes = []
            const valorUso = [];
            const saldoActualUso = [];

            registro.fuentes.forEach( fuente => {
                aportantes.push( fuente.aportante[ 0 ] )
                valorUso.push( fuente.aportante[ 0 ].valorUso[ 0 ].valor )
                saldoActualUso.push( fuente.aportante[ 0 ].valorUso[ 0 ].valorActual );
            } )

            // registro.fuentes[ registro.fuentes.length -1 ].aportante.forEach( aportante => {
            //     valorUso.push( aportante.valorUso[ 0 ].valor )
            //     saldoActualUso.push( aportante.valorUso[ 0 ].valorActual )
            // } )

            const registroObj = {
                nombreUso: registro.nombreUso,
                fuentes: registro.fuentes,
                aportante: aportantes,
                valorUso,
                saldoActualUso
            }

            this.dataTable.push( registroObj );
        } )

        this.dataSource = new MatTableDataSource( this.dataTable );
    }

}
