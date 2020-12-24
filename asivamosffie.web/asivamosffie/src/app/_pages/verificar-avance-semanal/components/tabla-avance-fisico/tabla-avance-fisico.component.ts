import { DialogAvanceAcumuladoComponent } from './../dialog-avance-acumulado/dialog-avance-acumulado.component';
import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';

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

    constructor(
        private dialog: MatDialog,
        private datePipe: DatePipe )
    { }

    ngOnInit(): void {
        this.getDataTable();
    }

    verifyInteger( value: number, esAvanceCapitulo: boolean ) {
        const esEntero = Number.isInteger( value );
        if ( value === 0 ) {
            return 0;
        }
        if ( esEntero === true && value > 0 ) {
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
                let totalDuracion = 0;
                for ( const flujo of flujoInversion ) {
                    totalDuracion += flujo.programacion.duracion;
                }
                for ( const flujo of flujoInversion ) {
                    flujo.programacion.avanceFisicoCapitulo =   flujo.programacion.avanceFisicoCapitulo !== undefined ?
                        String( this.verifyInteger( Number( flujo.programacion.avanceFisicoCapitulo ), false ) )
                        : '';
                    avancePorCapitulo.push(
                        {
                            programacionId: flujo.programacion.programacionId,
                            capitulo: flujo.programacion.actividad,
                            programacionCapitulo:   this.verifyInteger( flujo.programacion.duracion /
                                                                        this.seguimientoDiario.cantidadTotalDiasActividades * 100,
                                                                        false ),
                            avanceFisicoCapitulo:   flujo.programacion.avanceFisicoCapitulo !== undefined
                                                    && flujo.programacion.avanceFisicoCapitulo.length > 0 ?
                                                    String( this.verifyInteger( Number( flujo.programacion.avanceFisicoCapitulo ), true ) )
                                                    : ''
                        }
                    );
                }
                this.avanceFisico = [
                    {
                        semanaNumero: this.seguimientoDiario.numeroSemana,
                        periodoReporte: `${ this.datePipe.transform( this.seguimientoDiario.fechaInicio, 'dd/MM/yyyy' ) } - ${ this.datePipe.transform( this.seguimientoDiario.fechaFin, 'dd/MM/yyyy' ) }`,
                        programacionSemana: this.verifyInteger( totalDuracion / this.seguimientoDiario.cantidadTotalDiasActividades * 100,
                                                                false ),
                        avancePorCapitulo,
                        avanceFisicoSemana: this.seguimientoDiario.seguimientoSemanalAvanceFisico.length > 0 ?
                                            this.seguimientoDiario.seguimientoSemanalAvanceFisico[0].avanceFisicoSemanal : 0
                    }
                ];
            }
        }
        this.tablaAvanceFisico = new MatTableDataSource( this.avanceFisico );
    }

    valuePending( value: number, registro: any ) {
        if ( isNaN( Number( value ) ) === true ) {
            registro.avanceFisicoCapitulo = '0';
        } else {
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
                registro.avanceFisicoCapitulo = '';
            }
            for ( const capitulo of this.tablaAvanceFisico.data[0]['avancePorCapitulo'] ) {
                let avanceValue;
                if ( capitulo.avanceFisicoCapitulo > 0 ) {
                    avanceValue = this.verifyInteger( Number( capitulo.avanceFisicoCapitulo ), false );
                }
                if ( capitulo.avanceFisicoCapitulo === 0 ) {
                    avanceValue = 0;
                }
                if ( capitulo.avanceFisicoCapitulo === null ) {
                    avanceValue = 0;
                }
                totalAvanceFisicoSemana += Number( avanceValue );
            }
            this.tablaAvanceFisico.data[0]['avanceFisicoSemana'] =  this.verifyInteger( totalAvanceFisicoSemana, false );
        }
    }

    valuePendingProgramacionObra() {
        if ( this.seguimientoDiario !== undefined ) {
            let totalProgramacionObra = 0;
            const seguimientoSemanal = this.seguimientoDiario;
            if ( seguimientoSemanal.seguimientoSemanalAvanceFisico.length > 0 && seguimientoSemanal.numeroSemana > 1 ) {
                totalProgramacionObra += Number( seguimientoSemanal.seguimientoSemanalAvanceFisico[ 0 ].programacionSemanal );
            }
            if ( this.avanceFisico !== undefined && this.avanceFisico.length > 0 ) {
                totalProgramacionObra += Number( this.avanceFisico[0].programacionSemana );
            }
            return totalProgramacionObra;
        }
    }

    valuePendingAvanceEjecutado() {
        if ( this.seguimientoDiario !== undefined ) {
            let totalAvanceEjecutado = 0;
            const seguimientoSemanal = this.seguimientoDiario;
            if ( seguimientoSemanal.seguimientoSemanalAvanceFisico.length > 0 && seguimientoSemanal.numeroSemana > 1 ) {
                totalAvanceEjecutado += seguimientoSemanal.seguimientoSemanalAvanceFisico[ 0 ].avanceFisicoSemanal;
            }
            if ( this.tablaAvanceFisico.data.length > 0 ) {
                totalAvanceEjecutado += this.tablaAvanceFisico.data[0][ 'avanceFisicoSemana' ];
            }
            return totalAvanceEjecutado;
        }
    }

    openDialogAvanceAcumulado( ) {
        this.dialog.open( DialogAvanceAcumuladoComponent, {
            width: '80em',
            data: { avanceAcumulado: this.seguimientoDiario.avanceAcumulado, seguimientoSemanal: this.seguimientoDiario }
        } );
    }

}
