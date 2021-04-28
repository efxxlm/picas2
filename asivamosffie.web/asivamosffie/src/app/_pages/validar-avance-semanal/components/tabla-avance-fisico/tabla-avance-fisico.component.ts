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
import * as moment from 'moment';
@Component({
  selector: 'app-tabla-avance-fisico',
  templateUrl: './tabla-avance-fisico.component.html',
  styleUrls: ['./tabla-avance-fisico.component.scss']
})
export class TablaAvanceFisicoComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() avanceFisicoObs: string;
    tablaAvanceFisico = new MatTableDataSource();
    avanceFisico: any[];
    seRealizoCambio = false;
    seguimientoSemanalId: number;
    seguimientoSemanalAvanceFisicoId: number;
    contadorObservacionApoyo = 0;
    seguimientoSemanalObservacionId = 0;
    seguimientoSemanalAvanceFisico: any;
    observacionApoyo: any[] = [];
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

    groupBy( itemList: any[], actividad: string ) {
        const groupByItems = [];

        itemList.forEach( item => {
            if ( item.capitulo.actividad === actividad ) {
                groupByItems.length === 0 ? groupByItems.push( { actividad, items: [ item ] } ) : groupByItems[ groupByItems.length - 1 ].items.push( item );
            }
        } );

        return groupByItems;
    }

    getDataTable() {
        if ( this.seguimientoSemanal !== undefined ) {
            const actividadesLista = [];

            this.seguimientoSemanal.flujoInversion.forEach( flujo => {
                if ( this.groupBy( this.seguimientoSemanal.listProgramacion, flujo.programacion.actividad ).length > 0 ) {
                    actividadesLista.push( this.groupBy( this.seguimientoSemanal.listProgramacion, flujo.programacion.actividad ) );
                }
            } );
            
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalAvanceFisicoId =  this.seguimientoSemanal.seguimientoSemanalAvanceFisico.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0].seguimientoSemanalAvanceFisicoId : 0;
            const flujoInversion = this.seguimientoSemanal.flujoInversion;
            const seguimientoSemanalAvanceFisico = this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0];

            //Get Observacion apoyo y supervisor
            this.seguimientoSemanalAvanceFisico = this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0];
            if ( this.seguimientoSemanalAvanceFisico.observacionApoyoId !== undefined || this.seguimientoSemanalAvanceFisico.observacionSupervisorId !== undefined ) {
                this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalAvanceFisicoId, this.avanceFisicoObs )
                    .subscribe(
                        response => {
                            this.observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                            const observacionSupervisor = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                            this.dataHistorial = response.filter( obs => obs.archivada === true && obs.tieneObservacion === true );
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
                let duracionProgramacion = 0;
                let cantidadTotalDiasActividades = 0
                for ( const flujo of flujoInversion ) {
                    cantidadTotalDiasActividades += flujo.programacion.duracion;
                };
                for ( const flujo of flujoInversion ) {
                    flujo.seguimientoSemanalAvanceFisicoProgramacionId = 0;
                    flujo.programacion.avanceFisicoCapitulo = null;
                    if ( seguimientoSemanalAvanceFisico !== undefined ) {
                        const seguimientoSemanalAvanceFisicoProgramacion = seguimientoSemanalAvanceFisico.seguimientoSemanalAvanceFisicoProgramacion.filter( programacion => programacion.programacionId === flujo.programacionId );

                        if ( seguimientoSemanalAvanceFisicoProgramacion.length > 0 ) {
                            flujo.seguimientoSemanalAvanceFisicoProgramacionId = seguimientoSemanalAvanceFisicoProgramacion[0].seguimientoSemanalAvanceFisicoProgramacionId;
                            flujo.programacion.avanceFisicoCapitulo = seguimientoSemanalAvanceFisicoProgramacion[0].avanceFisicoCapitulo !== undefined ? seguimientoSemanalAvanceFisicoProgramacion[0].avanceFisicoCapitulo : null;
                        }
                    }

                    const actividadActual = actividadesLista.filter( value => value[ value.length - 1 ].actividad === flujo.programacion.actividad ).length > 0 ?
                                            actividadesLista.filter( value => value[ value.length - 1 ].actividad === flujo.programacion.actividad )[0][0] : undefined;

                    let duracionItem = 0;

                    if ( actividadActual !== undefined ) {
                        const fechaInicio = moment( new Date( this.seguimientoSemanal.fechaInicio ).setHours( 0, 0, 0, 0 ) );
                        const fechaFin = moment( new Date( this.seguimientoSemanal.fechaFin ).setHours( 0, 0, 0, 0 ) );

                        actividadActual.items.forEach( item => {
                            const fechaInicioItem = moment( new Date( item.fechaInicio ).setHours( 0, 0, 0, 0 ) );
                            const fechaFinItem = moment( item.fechaFin );

                            if ( fechaInicioItem < fechaInicio ) {
                                if ( fechaInicio <= fechaFinItem ) {
                                    duracionItem++;
                                } else {
                                    duracionItem += fechaFinItem.diff( fechaInicio, 'days' );
                                }
                            } else {
                                if ( fechaFinItem < fechaFin ) {
                                    duracionItem +=  fechaFinItem.diff( fechaInicioItem, 'days' );
                                } else {
                                    duracionItem += fechaFin.diff( fechaInicioItem, 'days' );
                                }
                            }
                        } );
                    }

                    avancePorCapitulo.push(
                        {
                            programacionId: flujo.programacion.programacionId,
                            capitulo: flujo.programacion.actividad,
                            programacionCapitulo:   this.verifyInteger( ( duracionItem / cantidadTotalDiasActividades ) * 100, false ),
                            avanceFisicoCapitulo: flujo.programacion.avanceFisicoCapitulo !== null ? String( this.verifyInteger( Number( flujo.programacion.avanceFisicoCapitulo ), true ) ) : null
                        }
                    );

                    duracionProgramacion += duracionItem;
                }
                this.avanceFisico = [
                    {
                        semanaNumero: this.seguimientoSemanal.numeroSemana,
                        periodoReporte: `${ this.datePipe.transform( this.seguimientoSemanal.fechaInicio, 'dd/MM/yyyy' ) } - ${ this.datePipe.transform( this.seguimientoSemanal.fechaFin, 'dd/MM/yyyy' ) }`,
                        programacionSemana: this.verifyInteger( ( duracionProgramacion / cantidadTotalDiasActividades ) * 100, false ),
                        avancePorCapitulo,
                        avanceFisicoSemana: this.seguimientoSemanal.seguimientoSemanalAvanceFisico.length > 0 ?
                                            this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0].avanceFisicoSemanal : 0
                    }
                ];
            }
        }
        this.tablaAvanceFisico = new MatTableDataSource( this.avanceFisico );
    }

    openDialogAvanceAcumulado( ) {
        this.dialog.open( DialogAvanceAcumuladoComponent, {
            width: '80em',
            data: { avanceAcumulado: this.seguimientoSemanal.avanceAcumulado, seguimientoSemanal: this.seguimientoSemanal }
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
        if ( this.seguimientoSemanalAvanceFisico.tieneObservacionApoyo === true && this.formAvanceFisico.get( 'tieneObservaciones' ).value === false && this.contadorObservacionApoyo === 0 ) {
            this.contadorObservacionApoyo++;
            this.openDialog( '', '<b>Le recomendamos verificar su respuesta;<br>Tenga en cuenta que el apoyo a la supervisión si tuvo observaciones.</b>' );
            return;
        }
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
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'True' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                        [
                                            '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoSemanalId
                                        ]
                                    )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
