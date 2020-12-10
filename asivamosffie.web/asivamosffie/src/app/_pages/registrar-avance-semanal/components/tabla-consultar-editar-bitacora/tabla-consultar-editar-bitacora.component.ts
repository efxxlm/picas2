import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-tabla-consultar-editar-bitacora',
  templateUrl: './tabla-consultar-editar-bitacora.component.html',
  styleUrls: ['./tabla-consultar-editar-bitacora.component.scss']
})
export class TablaConsultarEditarBitacoraComponent implements OnInit {

    tablaConsultarEditarBitacora = new MatTableDataSource();
    @Input() consultarBitacora: any;
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
        'semanaNumero',
        'periodoReporte',
        'estadoObra',
        'programacionObraAcumulada',
        'avanceFisico',
        'estadoRegistro',
        'estadoReporteSemanal',
        'gestion'
      ];

    constructor(
        private routes: Router )
    { }

    ngOnInit(): void {
        if ( this.consultarBitacora !== undefined ) {
            console.log( this.consultarBitacora );
            this.tablaConsultarEditarBitacora = new MatTableDataSource( this.consultarBitacora );
            this.tablaConsultarEditarBitacora.sort = this.sort;
            this.tablaConsultarEditarBitacora.paginator = this.paginator;
            this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        }
    }

    applyFilter( event: Event ) {
        const filterValue      = (event.target as HTMLInputElement).value;
        this.tablaConsultarEditarBitacora.filter = filterValue.trim().toLowerCase();
    }

    getVerDetalleAvance() {
        this.routes.navigate( [ `${ this.routes.url }/verDetalleAvanceSemanal`, 3 ] );
    }

    enviarVerificacion() {
        console.log( 'Metodo para enviar a verificacion.' );
    }

}
