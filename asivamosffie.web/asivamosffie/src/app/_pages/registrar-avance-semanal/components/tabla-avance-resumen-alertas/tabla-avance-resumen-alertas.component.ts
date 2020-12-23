import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { DialogTablaAvanceResumenComponent } from '../dialog-tabla-avance-resumen/dialog-tabla-avance-resumen.component';

@Component({
  selector: 'app-tabla-avance-resumen-alertas',
  templateUrl: './tabla-avance-resumen-alertas.component.html',
  styleUrls: ['./tabla-avance-resumen-alertas.component.scss']
})
export class TablaAvanceResumenAlertasComponent implements OnInit {

    tablaAvanceResumen = new MatTableDataSource();
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    displayedColumns: string[]  = [
      'fechaReporte',
      'cantidadPersonalProgramado',
      'cantidadPersonalTrabajando',
      'horasRetraso',
      'observaciones',
      'totalHorasRetraso'
    ];
    dataTable: any[] = [
        {
            fechaReporte: '05/07/2020',
            cantidadPersonalProgramado: 24,
            cantidadPersonalTrabajando: 19,
            horasRetraso: 6,
            observaciones: 'Cinco personas se reportaron incapacitadas por enfermedad.',
            totalHorasRetraso: 6
        }
    ];

    constructor( private dialog: MatDialog ) { }

    ngOnInit(): void {
        this.tablaAvanceResumen = new MatTableDataSource( this.dataTable );
    }

    openDialogObservaciones( observacion: string ) {
        this.dialog.open( DialogTablaAvanceResumenComponent, {
            width: '60em',
            data : observacion
        } );
    }

}
