import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';

@Component({
  selector: 'app-tabla-en-proceso-de-firmas-rlc',
  templateUrl: './tabla-en-proceso-de-firmas-rlc.component.html',
  styleUrls: ['./tabla-en-proceso-de-firmas-rlc.component.scss']
})
export class TablaEnProcesoDeFirmasRlcComponent implements OnInit {

    @Input() listaAcordeonEnProcesoFirma: any[] = [];
    displayedColumns: string[] = ['fechaSolicitud', 'numeroSolicitud', 'numeroContrato', 'tipoContrato', 'estadoRegistro', 'estadoDocumento', 'gestion'];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;

    constructor(private router: Router) { }

    ngOnInit(): void {
        this.listaAcordeonEnProcesoFirma.forEach( registro => registro.fechaSolicitud = moment( registro.fechaSolicitud ).format( 'DD/MM/YYYY' ) );

        this.dataSource = new MatTableDataSource( this.listaAcordeonEnProcesoFirma );
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter(event: Event) {
      const filterValue = (event.target as HTMLInputElement).value;
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    verDetalleEditar(id){
      this.router.navigate(['/registrarLiquidacionContrato/verDetalleEditarSolicitud', id]);
    }

}
