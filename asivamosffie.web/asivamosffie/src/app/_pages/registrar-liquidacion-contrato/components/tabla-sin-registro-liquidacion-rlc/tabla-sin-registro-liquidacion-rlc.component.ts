import { CommonService } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';

@Component({
  selector: 'app-tabla-sin-registro-liquidacion-rlc',
  templateUrl: './tabla-sin-registro-liquidacion-rlc.component.html',
  styleUrls: ['./tabla-sin-registro-liquidacion-rlc.component.scss']
})
export class TablaSinRegistroLiquidacionRlcComponent implements OnInit {

    @Input() listaAcordeonSinRegistro: any[] = [];
    displayedColumns: string[] = ['fechaSolicitud', 'numeroSolicitud', 'numeroContrato', 'tipoContrato', 'estadoRegistro', 'estadoDocumento', 'gestion'];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    constructor(
        private routes: Router,
        private dialog: MatDialog )
    {
    }

    ngOnInit(): void {
        this.listaAcordeonSinRegistro.forEach( registro => registro.fechaSolicitud = moment( registro.fechaSolicitud ).format( 'DD/MM/YYYY' ) );

        this.dataSource = new MatTableDataSource( this.listaAcordeonSinRegistro );
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
