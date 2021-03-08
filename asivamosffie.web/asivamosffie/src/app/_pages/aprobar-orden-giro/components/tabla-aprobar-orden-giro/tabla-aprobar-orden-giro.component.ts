import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogEnviarAprobacionComponent } from '../dialog-enviar-aprobacion/dialog-enviar-aprobacion.component';

@Component({
  selector: 'app-tabla-aprobar-orden-giro',
  templateUrl: './tabla-aprobar-orden-giro.component.html',
  styleUrls: ['./tabla-aprobar-orden-giro.component.scss']
})
export class TablaAprobarOrdenGiroComponent implements OnInit {

    tablaAprobar = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
      'fechaVerificacion',
      'numeroOrden',
      'modalidad',
      'numeroContrato',
      'estadoAprobacion',
      'gestion'
    ];
    dataTable = [
        {
            fechaVerificacion: new Date(),
            numeroOrden: 'ODG_Obr 001',
            modalidad: 'Tipo B',
            numeroContrato: 'N801801',
            estadoAprobacion: 'Sin verificación',
            esExpensas: false,
            id: Math.round( Math.random() * 10 )
        },
        {
            fechaVerificacion: new Date(),
            numeroOrden: 'ODG_Expensas 001',
            modalidad: 'No aplica',
            numeroContrato: 'N326326',
            estadoAprobacion: 'Sin verificación',
            esExpensas: true,
            id: Math.round( Math.random() * 10 )
        },
        {
            fechaVerificacion: new Date(),
            numeroOrden: 'ODG_Otros Costos 001',
            modalidad: 'Tipo B',
            numeroContrato: 'N801801',
            estadoAprobacion: 'Sin verificación',
            esExpensas: false,
            id: Math.round( Math.random() * 10 )
        }
    ];

    constructor(
        private dialog: MatDialog )
    { }

    ngOnInit(): void {
        this.tablaAprobar = new MatTableDataSource( this.dataTable );
        this.tablaAprobar.sort = this.sort;
        this.tablaAprobar.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter( event: Event ) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.tablaAprobar.filter = filterValue.trim().toLowerCase();
    }

    openDialogEnviarAprobacion() {
        this.dialog.open( DialogEnviarAprobacionComponent, {
            width: '80em'
        });
    }

}
