import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { DialogTablaAvanceResumenComponent } from '../dialog-tabla-avance-resumen/dialog-tabla-avance-resumen.component';

@Component({
  selector: 'app-tabla-disponibilidad-material',
  templateUrl: './tabla-disponibilidad-material.component.html',
  styleUrls: ['./tabla-disponibilidad-material.component.scss']
})
export class TablaDisponibilidadMaterialComponent implements OnInit {

    @Input() seguimientoDiario: any;
    tablaDisponibilidad = new MatTableDataSource();
    dataSeguimientoDiario: any[] = [];
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    displayedColumns: string[]  = [
      'fechaReporte',
      'causas',
      'horasRetraso',
      'observaciones',
      'totalHorasRetraso'
    ];

    constructor( private dialog: MatDialog ){ }

    ngOnInit(): void {
        if ( this.seguimientoDiario !== undefined && this.seguimientoDiario.length > 0 ) {
            const seguimientoDiario = [];
            let sumaTotal = 0;
            for ( const seguimiento of this.seguimientoDiario ) {
                let totalhorasRetraso = 0;
                totalhorasRetraso += seguimiento.numeroHorasRetrasoMaterial !== undefined ? seguimiento.numeroHorasRetrasoMaterial : 0;
                totalhorasRetraso += seguimiento.numeroHorasRetrasoEquipo !== undefined ? seguimiento.numeroHorasRetrasoEquipo : 0;
                totalhorasRetraso +=    seguimiento.numeroHorasRetrasoProductividad !== undefined ?
                                        seguimiento.numeroHorasRetrasoProductividad : 0;
                seguimiento.totalHorasRetraso = totalhorasRetraso === 0 ? '---' : totalhorasRetraso;
                sumaTotal += totalhorasRetraso;
                seguimientoDiario.push( seguimiento );
            }

            this.dataSeguimientoDiario.push(
                {
                    totalHorasRetraso: sumaTotal,
                    resumenAlertas: seguimientoDiario
                }
            );
            this.tablaDisponibilidad = new MatTableDataSource( this.dataSeguimientoDiario );
        }
    }

    openDialogObservaciones( observacion: string, registro: any ) {
        this.dialog.open( DialogTablaAvanceResumenComponent, {
            width: '60em',
            data : { observacion, registro }
        } );
    }

}
