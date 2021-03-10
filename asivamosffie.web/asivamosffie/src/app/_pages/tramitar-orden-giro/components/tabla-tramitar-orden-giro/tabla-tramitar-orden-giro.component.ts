import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-tramitar-orden-giro',
  templateUrl: './tabla-tramitar-orden-giro.component.html',
  styleUrls: ['./tabla-tramitar-orden-giro.component.scss']
})
export class TablaTramitarOrdenGiroComponent implements OnInit {

    tablaTramitar = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[]  = [
      'fechaAprobacion',
      'numeroOrden',
      'modalidad',
      'numeroContrato',
      'estadoTramite',
      'gestion'
    ];
    dataTable = [
        {
            fechaAprobacion: new Date(),
            numeroOrden: 'ODG_Obr 001',
            modalidad: 'Tipo B',
            numeroContrato: 'N801801',
            estadoTramite: 'Sin tramitar',
            esExpensas: false,
            id: Math.round( Math.random() * 10 )
        },
        {
            fechaAprobacion: new Date(),
            numeroOrden: 'ODG_Expensas 001',
            modalidad: 'No aplica',
            numeroContrato: 'N326326',
            estadoTramite: 'Sin tramitar',
            esExpensas: true,
            id: Math.round( Math.random() * 10 )
        },
        {
            fechaAprobacion: new Date(),
            numeroOrden: 'ODG_Otros Costos 001',
            modalidad: 'Tipo B',
            numeroContrato: 'N801801',
            estadoTramite: 'Sin tramitar',
            esExpensas: false,
            id: Math.round( Math.random() * 10 )
        }
    ];

    constructor(
        private dialog: MatDialog )
    { }

    ngOnInit(): void {
        this.tablaTramitar = new MatTableDataSource( this.dataTable );
        this.tablaTramitar.sort = this.sort;
        this.tablaTramitar.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    applyFilter( event: Event ) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.tablaTramitar.filter = filterValue.trim().toLowerCase();
    }

    openDialogEnviarAprobacion() {
        // this.dialog.open( DialogEnviarAprobacionComponent, {
        //   width: '80em'
        // });
    }

}
