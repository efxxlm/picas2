import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MatTableDataSource } from '@angular/material/table';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogAvanceAcumuladoComponent } from '../dialog-avance-acumulado/dialog-avance-acumulado.component';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-avance-fisico',
  templateUrl: './tabla-avance-fisico.component.html',
  styleUrls: ['./tabla-avance-fisico.component.scss']
})
export class TablaAvanceFisicoComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoDiario: any;
    tablaAvanceFisico = new MatTableDataSource();
    avanceFisico: any[];
    seRealizoCambio = false;
    seguimientoSemanalId: number;
    seguimientoSemanalAvanceFisicoId: number;
    displayedColumns: string[]  = [
        'semanaNumero',
        'periodoReporte',
        'programacionSemana',
        'capitulo',
        'programacionCapitulo',
        'avanceFisicoCapitulo',
        'avanceFisicoSemana'
    ];
    dataTable: any[] = [
        {
            semanaNumero: 1,
            periodoReporte: '04/07/2020 - 11/07/2020',
            programacionSemana: '2',
            capitulo: 'Preliminares',
            programacionCapitulo: '2',
            avanceFisicoCapitulo: null,
            avanceFisicoSemana: ''
        }
    ];

    constructor(
        private dialog: MatDialog,
        private datePipe: DatePipe,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService )
    { }

    ngOnInit(): void {
        this.getDataTable();
    }

    verifyInteger( value: number, esAvanceCapitulo: boolean ) {
        const esEntero = Number.isInteger( value );
        if ( esEntero === true ) {
            return value;
        } else {
            return esAvanceCapitulo === true ? value.toFixed( 1 ) : value.toFixed( 2 );
        }
    }

    getDataTable() {
        if ( this.seguimientoDiario !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoDiario.seguimientoSemanalId;
            this.seguimientoSemanalAvanceFisicoId =  this.seguimientoDiario.seguimientoSemanalAvanceFisico.length > 0 ?
            this.seguimientoDiario.seguimientoSemanalAvanceFisico[0].seguimientoSemanalAvanceFisicoId : 0;
            const flujoInversion = this.seguimientoDiario.flujoInversion;
            if ( flujoInversion.length > 0 ) {
                const avancePorCapitulo = [];
                let totalRangoDias = 0;
                let totalDuracion = 0;
                for ( const flujo of flujoInversion ) {
                    totalRangoDias += flujo.programacion.rangoDias;
                    totalDuracion += flujo.programacion.duracion;
                }
                for ( const flujo of flujoInversion ) {

                    flujo.programacion.avanceFisicoCapitulo = flujo.programacion.avanceFisicoCapitulo !== undefined ?
                        String( this.verifyInteger( Number( flujo.programacion.avanceFisicoCapitulo ), false ) )
                        : '';
                    avancePorCapitulo.push(
                        {
                            programacionId: flujo.programacion.programacionId,
                            capitulo: flujo.programacion.actividad,
                            programacionCapitulo:   this.verifyInteger( flujo.programacion.duracion / totalRangoDias * 100, false ),
                            avanceFisicoCapitulo:   flujo.programacion.avanceFisicoCapitulo !== undefined ?
                                                    String( this.verifyInteger( Number( flujo.programacion.avanceFisicoCapitulo ), true ) )
                                                    : ''
                        }
                    );
                }
                console.log( totalDuracion / totalRangoDias * 100 );
                this.avanceFisico = [
                    {
                        semanaNumero: this.seguimientoDiario.numeroSemana,
                        periodoReporte: `${ this.datePipe.transform( this.seguimientoDiario.fechaInicio, 'dd/MM/yyyy' ) } - ${ this.datePipe.transform( this.seguimientoDiario.fechaFin, 'dd/MM/yyyy' ) }`,
                        programacionSemana: this.verifyInteger( totalDuracion / totalRangoDias * 100, false ),
                        avancePorCapitulo,
                        avanceFisicoSemana: this.seguimientoDiario.seguimientoSemanalAvanceFisico.length > 0 ?
                                            this.seguimientoDiario.seguimientoSemanalAvanceFisico[0].avanceFisicoSemanal : 0
                    }
                ];
            }
        }
        this.tablaAvanceFisico = new MatTableDataSource( this.avanceFisico );
    }

    valuePending( value: string, registro: any ) {
        this.seRealizoCambio = true;
        this.tablaAvanceFisico.data[0]['avanceFisicoSemana'] = 0;
        let totalAvanceFisicoSemana = 0;
        if ( Number( value ) > 100 ) {
            registro.avanceFisicoCapitulo = `${ this.verifyInteger( Number( registro.programacionCapitulo ), true ) }`;
        }
        if ( Number( value ) > Number( registro.programacionCapitulo ) ) {
            registro.avanceFisicoCapitulo = `${ this.verifyInteger( Number( registro.programacionCapitulo ), true ) }`;
        }
        if ( Number( value ) < 0 ) {
            registro.avanceFisicoCapitulo = '0';
        }
        for ( const capitulo of this.tablaAvanceFisico.data[0]['avancePorCapitulo'] ) {
            let avanceValue;
            if ( capitulo.avanceFisicoCapitulo.length > 0 ) {
                avanceValue = this.verifyInteger( Number( capitulo.avanceFisicoCapitulo ), false );
            }
            totalAvanceFisicoSemana += Number( avanceValue );
        }
        console.log( totalAvanceFisicoSemana );
        this.tablaAvanceFisico.data[0]['avanceFisicoSemana'] =  this.verifyInteger( totalAvanceFisicoSemana, false );
    }

    openDialog(modalTitle: string, modalText: string) {
        this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data : { modalTitle, modalText }
        });
    }

    openDialogObservaciones( ) {
        this.dialog.open( DialogAvanceAcumuladoComponent, {
            width: '80em',
            data: { avanceAcumulado: this.seguimientoDiario.avanceAcumulado }
        } );
    }

    guardar() {
        const pSeguimientoSemanal = this.seguimientoDiario;
        for (const flujoInversion of this.seguimientoDiario.flujoInversion ) {
            this.tablaAvanceFisico.data[0][ 'avancePorCapitulo' ].filter( value => {
                if ( flujoInversion.programacion.programacionId === value.programacionId ) {
                    flujoInversion.programacion.avanceFisicoCapitulo = Number( value.avanceFisicoCapitulo );
                }
            } );
        }
        const seguimientoSemanalAvanceFisico = [
            {
                seguimientoSemanalId: this.seguimientoSemanalId,
                seguimientoSemanalAvanceFisicoId: this.seguimientoSemanalAvanceFisicoId,
                avanceFisicoSemanal: this.tablaAvanceFisico.data[0][ 'avanceFisicoSemana' ]
            }
        ];
        pSeguimientoSemanal.seguimientoSemanalAvanceFisico = seguimientoSemanalAvanceFisico;
        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/registrarAvanceSemanal/registroSeguimientoSemanal', this.seguimientoDiario.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
