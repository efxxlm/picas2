import { Component, Input, OnInit, ViewChild } from '@angular/core';
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

    @Input() esVerDetalle = false;
    @Input() seguimientoDiario: any;
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

    constructor( private dialog: MatDialog ) { }

    ngOnInit(): void {
        if ( this.seguimientoDiario !== undefined ) {
            const dataSeguimientoDiario = [];
            const seguimientoDiario = [];
            let sumaTotal = 0;
            if ( this.seguimientoDiario.length > 0 ) {
                for ( const seguimiento of this.seguimientoDiario ) {
                    if (    seguimiento.cantidadPersonalProgramado !== undefined
                            && seguimiento.cantidadPersonalTrabajando !== undefined
                            && seguimiento.numeroHorasRetrasoPersonal !== undefined )
                    {
                        seguimientoDiario.push( seguimiento );
                        sumaTotal += seguimiento.numeroHorasRetrasoPersonal;
                    }
                }
            } else {
                seguimientoDiario.push(
                    {
                        fechaSeguimiento: null,
                        cantidadPersonalProgramado: '---',
                        cantidadPersonalTrabajando: '---',
                        numeroHorasRetrasoPersonal: '---',
                        seguimientoDiarioObservaciones: ''
                    }
                );
            }

            dataSeguimientoDiario.push(
                {
                    totalHorasRetraso: sumaTotal === 0 ? '---' : sumaTotal,
                    resumenAlertas: seguimientoDiario
                }
            );
            this.tablaAvanceResumen = new MatTableDataSource( dataSeguimientoDiario );
        }
    }

    openDialogObservaciones( observacion: string, registro: any ) {
        this.dialog.open( DialogTablaAvanceResumenComponent, {
            width: '60em',
            data : { observacion, registro }
        } );
    }

}
