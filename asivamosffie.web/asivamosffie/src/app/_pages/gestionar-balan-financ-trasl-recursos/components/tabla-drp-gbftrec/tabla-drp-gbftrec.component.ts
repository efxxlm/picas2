import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-drp-gbftrec',
  templateUrl: './tabla-drp-gbftrec.component.html',
  styleUrls: ['./tabla-drp-gbftrec.component.scss']
})
export class TablaDrpGbftrecComponent implements OnInit {

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
