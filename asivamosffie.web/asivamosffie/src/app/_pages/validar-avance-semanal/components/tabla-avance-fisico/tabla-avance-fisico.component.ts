import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
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
    @Input() avanceFisicoObs: string;
    tablaAvanceFisico = new MatTableDataSource();
    avanceFisico: any[];
    seRealizoCambio = false;
    seguimientoSemanalId: number;
    seguimientoSemanalAvanceFisicoId: number;
    seguimientoSemanalObservacionId = 0;
    seguimientoSemanalAvanceFisico: any;
    observacionApoyo: any;
    formAvanceFisico: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    displayedColumns: string[]  = [
        'semanaNumero',
        'periodoReporte',
        'programacionSemana',
        'capitulo',
        'programacionCapitulo',
        'avanceFisicoCapitulo',
        'avanceFisicoSemana'
    ];
    tablaHistorial = new MatTableDataSource();
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    dataHistorial: any[] = [];
    editorStyle = {
        height: '100px'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };

    constructor(
        private dialog: MatDialog,
        private datePipe: DatePipe,
        private fb: FormBuilder,
        private routes: Router,
        private registrarAvanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService )
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
            //Get Observacion apoyo y supervisor
            this.seguimientoSemanalAvanceFisico = this.seguimientoDiario.seguimientoSemanalAvanceFisico[0];
            if ( this.seguimientoSemanalAvanceFisico.observacionApoyoId !== undefined || this.seguimientoSemanalAvanceFisico.observacionSupervisorId !== undefined ) {
                this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalAvanceFisicoId, this.avanceFisicoObs )
                    .subscribe(
                        response => {
                            this.observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                            const observacionSupervisor = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                            this.dataHistorial = response.filter( obs => obs.archivada === true );
                            this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                            if ( observacionSupervisor.length > 0 ) {
                                this.seguimientoSemanalObservacionId = observacionSupervisor[0].seguimientoSemanalObservacionId;
                                if ( observacionSupervisor[0].observacion !== undefined ) {
                                    if ( observacionSupervisor[0].observacion.length > 0 ) {
                                        this.formAvanceFisico.get( 'observaciones' ).setValue( observacionSupervisor[0].observacion );
                                    }
                                }
                                this.formAvanceFisico.get( 'tieneObservaciones' ).setValue( this.seguimientoSemanalAvanceFisico.tieneObservacionSupervisor );
                                this.formAvanceFisico.get( 'fechaCreacion' ).setValue( observacionSupervisor[0].fechaCreacion );
                            }
                        }
                    );
            }
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

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        if ( this.formAvanceFisico.get( 'tieneObservaciones' ).value === false && this.formAvanceFisico.get( 'observaciones' ).value !== null ) {
            this.formAvanceFisico.get( 'observaciones' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.avanceFisicoObs,
            observacionPadreId: this.seguimientoSemanalAvanceFisicoId,
            observacion: this.formAvanceFisico.get( 'observaciones' ).value,
            tieneObservacion: this.formAvanceFisico.get( 'tieneObservaciones' ).value,
            esSupervisor: true
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoDiario.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
