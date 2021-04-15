import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-liquidacion-rlc',
  templateUrl: './tabla-liquidacion-rlc.component.html',
  styleUrls: ['./tabla-liquidacion-rlc.component.scss']
})
export class TablaLiquidacionRlcComponent implements OnInit {

    @Input() listaAcordeonLiquidado: any[] = [];
    displayedColumns: string[] = ['fechaSolicitud', 'numeroSolicitud', 'numeroContrato', 'tipoContrato', 'estadoRegistro', 'estadoDocumento', 'gestion'];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    constructor(private router: Router) { }

    ngOnInit(): void {
        this.dataSource = new MatTableDataSource( this.listaAcordeonLiquidado );
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
