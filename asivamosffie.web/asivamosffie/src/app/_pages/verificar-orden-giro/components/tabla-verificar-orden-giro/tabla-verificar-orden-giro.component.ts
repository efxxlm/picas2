import { DialogEnviarAprobacionComponent } from './../dialog-enviar-aprobacion/dialog-enviar-aprobacion.component';
import { MatDialog } from '@angular/material/dialog';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-verificar-orden-giro',
  templateUrl: './tabla-verificar-orden-giro.component.html',
  styleUrls: ['./tabla-verificar-orden-giro.component.scss']
})
export class TablaVerificarOrdenGiroComponent implements OnInit {

    tablaVerificar = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
      'fechaGeneracion',
      'numeroOrden',
      'modalidad',
      'numeroContrato',
      'estadoVerificacion',
      'gestion'
    ];
    dataTable = [
        {
            fechaGeneracion: new Date(),
            numeroOrden: 'ODG_Obr 001',
            modalidad: 'Tipo B',
            numeroContrato: 'N801801',
            estadoVerificacion: 'Sin verificación',
            id: Math.round( Math.random() * 10 )
        },
        {
            fechaGeneracion: new Date(),
            numeroOrden: 'ODG_Expensas 001',
            modalidad: 'No aplica',
            numeroContrato: 'N326326',
            estadoVerificacion: 'Sin verificación',
            id: Math.round( Math.random() * 10 )
        },
        {
            fechaGeneracion: new Date(),
            numeroOrden: 'ODG_Otros Costos 001',
            modalidad: 'Tipo B',
            numeroContrato: 'N801801',
            estadoVerificacion: 'Sin verificación',
            id: Math.round( Math.random() * 10 )
        }
    ];

    constructor(
        private dialog: MatDialog )
    { }

    ngOnInit(): void {
        this.tablaVerificar = new MatTableDataSource( this.dataTable );
        this.tablaVerificar.sort = this.sort;
        this.tablaVerificar.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter( event: Event ) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.tablaVerificar.filter = filterValue.trim().toLowerCase();
    }

    openDialogEnviarAprobacion() {
        this.dialog.open( DialogEnviarAprobacionComponent, {
          width: '80em'
        });
    }

}
