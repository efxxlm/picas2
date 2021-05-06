import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-datos-ddp-gog',
  templateUrl: './tabla-datos-ddp-gog.component.html',
  styleUrls: ['./tabla-datos-ddp-gog.component.scss']
})
export class TablaDatosDdpGogComponent implements OnInit {

    @Input() tablaUsoFuenteAportante: any[] = [];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = ['uso', 'fuente', 'aportante', 'valorUso', 'saldoActualUso'];
    dataTable: any[] = [];

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
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
