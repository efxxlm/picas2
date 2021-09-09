import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MatTableDataSource } from '@angular/material/table';
import { Component, Input, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogAvanceAcumuladoComponent } from '../dialog-avance-acumulado/dialog-avance-acumulado.component';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import * as moment from 'moment';
import { GuardadoParcialAvanceSemanalService } from 'src/app/core/_services/guardadoParcialAvanceSemanal/guardado-parcial-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Console } from 'console';

@Component({
  selector: 'app-tabla-avance-fisico',
  templateUrl: './tabla-avance-fisico.component.html',
  styleUrls: ['./tabla-avance-fisico.component.scss']
})
export class TablaAvanceFisicoComponent implements OnInit, OnDestroy {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() avanceFisicoObs: string;
    @Output() estadoSemaforoAlerta = new EventEmitter<string>();
    @Output() tieneObservacion = new EventEmitter();
    sinRegistros = false;
    tablaAvanceFisico = new MatTableDataSource();
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    avanceFisico: any[];
    obsApoyo: any;
    obsSupervisor: any;
    seRealizoCambio = false;
    seRealizoPeticion = false;
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
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    primeraSemana = 1;

    constructor(
        private dialog: MatDialog,
        private datePipe: DatePipe,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private guardadoParcialAvanceSemanalSvc: GuardadoParcialAvanceSemanalService )
    { }

    ngOnDestroy(): void {
        if ( this.seRealizoCambio === true && this.seRealizoPeticion === false ) {
            this.guardadoParcialAvanceSemanalSvc.getDataAvanceFisico( this.guardadoParcial(), this.seRealizoPeticion )
        } else {
            this.guardadoParcialAvanceSemanalSvc.getDataAvanceFisico( undefined );
        }
    }

    ngOnInit(): void {
        this.getDataTable();
    }

    verifyInteger( value: number, esAvanceCapitulo: boolean ): number {
        const esEntero = Number.isInteger( value );
        if ( value === 0 ) {
            return 0;
        }
        if ( esEntero === true && value > 0 ) {
            return value;
        } else {
            return esAvanceCapitulo === true ? Number( value.toFixed( 1 ) ) : Number( value.toFixed( 2 ) );
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

            if ( this.esVerDetalle === false ) {
                this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalAvanceFisicoId, this.avanceFisicoObs )
                    .subscribe(
                        response => {
                            this.obsApoyo = response.find( obs => obs.archivada === false && obs.esSupervisor === false && obs.tieneObservacion === true );
                            this.obsSupervisor = response.find( obs => obs.archivada === false && obs.esSupervisor === true && obs.tieneObservacion === true );
                            this.dataHistorial = response.filter( obs => obs.tieneObservacion === true );

                            if ( this.obsApoyo !== undefined || this.obsSupervisor !== undefined ) {
                                this.tieneObservacion.emit();
                            }

                            this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
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
                let i = 0;
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

                    if ( this.verifyInteger( ( duracionItem / (cantidadTotalDiasActividades == 0 ? 1 : (cantidadTotalDiasActividades + 1)) ) * 100, false ) > 0 ) {
                        if (this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0]) {
                            avancePorCapitulo.push(
                                {
                                    programacionId: flujo.programacion.programacionId,
                                    capitulo: flujo.programacion.actividad,
                                    programacionCapitulo: this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0].seguimientoSemanalAvanceFisicoProgramacion[i].programacionCapitulo,
                                    avanceFisicoCapitulo: flujo.programacion.avanceFisicoCapitulo !== null ? String( this.verifyInteger( Number( flujo.programacion.avanceFisicoCapitulo ), true ) ) : null
                                }
                            );
                        }
                        else {
                            avancePorCapitulo.push(
                                {
                                    programacionId: flujo.programacion.programacionId,
                                    capitulo: flujo.programacion.actividad,
                                    avanceFisicoCapitulo: flujo.programacion.avanceFisicoCapitulo !== null ? String( this.verifyInteger( Number( flujo.programacion.avanceFisicoCapitulo ), true ) ) : null
                                }
                            );
                        }
                    }

                    duracionProgramacion += duracionItem;
                    i++;
                }
                if ( avancePorCapitulo.length > 0 ) {
                    this.avanceFisico = [
                        {
                            semanaNumero: this.seguimientoSemanal.numeroSemana,
                            periodoReporte: `${ this.datePipe.transform( this.seguimientoSemanal.fechaInicio, 'dd/MM/yyyy' ) } - ${ this.datePipe.transform( this.seguimientoSemanal.fechaFin, 'dd/MM/yyyy' ) }`,
                            programacionSemana: this.verifyInteger( ( duracionProgramacion / (cantidadTotalDiasActividades == 0 ? 1 : (cantidadTotalDiasActividades + 1)) ) * 100, false ),
                            avancePorCapitulo,
                            avanceFisicoSemana: this.seguimientoSemanal.seguimientoSemanalAvanceFisico.length > 0 ?
                                                this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0].avanceFisicoSemanal : 0
                        }
                    ];
                }
            }
        }
        if ( this.avanceFisico !== undefined ) {
            this.tablaAvanceFisico = new MatTableDataSource( this.avanceFisico );
        } else {
            this.sinRegistros = true;
            this.estadoSemaforoAlerta.emit( 'en-alerta' );
        }
    }

    valuePending( value: number, registro: any )
     { 
       
 
        if ( isNaN( Number( value ) ) === true ) {
            registro.avanceFisicoCapitulo = '0';
        } else {
         
            if ( Number( value ) < 0 ) {
                registro.avanceFisicoCapitulo = '0';
                return;
            }
            this.seRealizoCambio = true;
            this.tablaAvanceFisico.data[0]['avanceFisicoSemana'] = 0;
            let totalAvanceFisicoSemana = 0;
            if ( Number( value ) > 100 ) {
                registro.avanceFisicoCapitulo = 100;
                return;
            }
            /*  
            if ( Number( value ) > 100 ) {
                registro.avanceFisicoCapitulo = `${ this.verifyInteger( Number( registro.programacionCapitulo ), true ) }`;
            }
            if ( Number( value ) > Number( registro.programacionCapitulo ) ) {
                registro.avanceFisicoCapitulo = `${ this.verifyInteger( Number( registro.programacionCapitulo ), true ) }`;
            }
            if ( Number( value ) < 0 ) {
                registro.avanceFisicoCapitulo = null;
            }*/
            for ( const capitulo of this.tablaAvanceFisico.data[0]['avancePorCapitulo'] ) {
                
                let avanceValue = 0;
                if ( capitulo.avanceFisicoCapitulo > 0 ) {
                    avanceValue = this.verifyInteger( Number( capitulo.avanceFisicoCapitulo ), false );
                }
                if ( capitulo.avanceFisicoCapitulo === 0 ) {
                    avanceValue = 0;
                }
                if ( capitulo.avanceFisicoCapitulo === null || capitulo.avanceFisicoCapitulo === undefined ) {
                    avanceValue = 0;
                }
                totalAvanceFisicoSemana += Number( avanceValue );
            }
            this.tablaAvanceFisico.data[0]['avanceFisicoSemana'] =  this.verifyInteger( totalAvanceFisicoSemana, false ); 
        }
 
    }

    valuePendingProgramacionObra() {
        if ( this.seguimientoSemanal !== undefined ) {
            let totalProgramacionObra = 0;
            const seguimientoSemanal = this.seguimientoSemanal;
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
        if ( this.seguimientoSemanal !== undefined ) {
            let totalAvanceEjecutado = 0;
            const seguimientoSemanal = this.seguimientoSemanal;
            if ( seguimientoSemanal.seguimientoSemanalAvanceFisico.length > 0 && seguimientoSemanal.numeroSemana > 1 ) {
                totalAvanceEjecutado += seguimientoSemanal.seguimientoSemanalAvanceFisico[ 0 ].avanceFisicoSemanal;
            }
            if ( this.tablaAvanceFisico.data.length > 0 ) {
                totalAvanceEjecutado += this.tablaAvanceFisico.data[0][ 'avanceFisicoSemana' ];
            }
            return totalAvanceEjecutado;
        }
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
            data: { avanceAcumulado: this.seguimientoSemanal.avanceAcumulado, seguimientoSemanal: this.seguimientoSemanal }
        } );
    }

    textoLimpio(texto: number) {
        if (texto !== null) {
          const textolimpio = texto.toString().replace(/[^\w\s]/gi, '');
          return textolimpio.length;
        } else {
            return 0;
        }
    }

    guardar() {
        const pSeguimientoSemanal = this.seguimientoSemanal;
        const getSeguimientoSemanalAvanceFisicoProgramacion = () => {
            const seguimientoSemanalAvanceFisicoProgramacion = [];
            for (const flujoInversion of this.seguimientoSemanal.flujoInversion ) {
                const programacion: any[] = this.tablaAvanceFisico.data[ 0 ][ 'avancePorCapitulo' ].filter( programacion => programacion.programacionId === flujoInversion.programacionId );

                if ( programacion.length > 0 ) {
                    seguimientoSemanalAvanceFisicoProgramacion.push(
                        {
                            seguimientoSemanalAvanceFisicoProgramacionId: flujoInversion.seguimientoSemanalAvanceFisicoProgramacionId,
                            seguimientoSemanalAvanceFisicoId: this.seguimientoSemanalAvanceFisicoId,
                            programacionId: flujoInversion.programacionId,
                            avanceFisicoCapitulo: programacion[0].avanceFisicoCapitulo
                        }
                    );
                }
            }
            
            return seguimientoSemanalAvanceFisicoProgramacion;
        }
        const seguimientoSemanalAvanceFisico = [
            {
                seguimientoSemanalId: this.seguimientoSemanalId,
                seguimientoSemanalAvanceFisicoId: this.seguimientoSemanalAvanceFisicoId,
                seguimientoSemanalAvanceFisicoProgramacion: getSeguimientoSemanalAvanceFisicoProgramacion(),
                programacionSemanal: this.avanceFisico[ 0 ].programacionSemana,
                avanceFisicoSemanal: this.tablaAvanceFisico.data[ 0 ][ 'avanceFisicoSemana' ]
            }
        ];

        pSeguimientoSemanal.seguimientoSemanalAvanceFisico = seguimientoSemanalAvanceFisico;


        // pSeguimientoSemanal.seguimientoSemanalAvanceFisico.forEach(element => {
        //     element.programacionSemanal
        // });

        
        for (let index = 0; index < this.avanceFisico[0].avancePorCapitulo.length; index++) {
            const element = this.avanceFisico[0].avancePorCapitulo[index];
            pSeguimientoSemanal.seguimientoSemanalAvanceFisico[0].seguimientoSemanalAvanceFisicoProgramacion[index].programacionCapitulo = element.programacionCapitulo
        }

        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                async response => {
                    if ( this.obsApoyo !== undefined ) {
                        this.obsApoyo.archivada = !this.obsApoyo.archivada;
                        await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyo ).toPromise();
                    }
                    if ( this.obsSupervisor !== undefined ) {
                        this.obsSupervisor.archivada = !this.obsSupervisor.archivada;
                        await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisor ).toPromise();
                    }

                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.seRealizoPeticion = true;
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/registrarAvanceSemanal/registroSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardadoParcial() {
        const getSeguimientoSemanalAvanceFisicoProgramacion = () => {
            const seguimientoSemanalAvanceFisicoProgramacion = [];
            for (const flujoInversion of this.seguimientoSemanal.flujoInversion ) {
                const programacion: any[] = this.tablaAvanceFisico.data[ 0 ][ 'avancePorCapitulo' ].filter( programacion => programacion.programacionId === flujoInversion.programacionId );

                if ( programacion.length > 0 ) {
                    seguimientoSemanalAvanceFisicoProgramacion.push(
                        {
                            seguimientoSemanalAvanceFisicoProgramacionId: flujoInversion.seguimientoSemanalAvanceFisicoProgramacionId,
                            seguimientoSemanalAvanceFisicoId: this.seguimientoSemanalAvanceFisicoId,
                            programacionId: flujoInversion.programacionId,
                            avanceFisicoCapitulo: programacion[0].avanceFisicoCapitulo
                        }
                    );
                }
            }
            
            return seguimientoSemanalAvanceFisicoProgramacion;
        }
        const seguimientoSemanalAvanceFisico = [
            {
                seguimientoSemanalId: this.seguimientoSemanalId,
                seguimientoSemanalAvanceFisicoId: this.seguimientoSemanalAvanceFisicoId,
                seguimientoSemanalAvanceFisicoProgramacion: getSeguimientoSemanalAvanceFisicoProgramacion(),
                programacionSemanal: this.avanceFisico[ 0 ].programacionSemana,
                avanceFisicoSemanal: this.tablaAvanceFisico.data[ 0 ][ 'avanceFisicoSemana' ]
            }
        ];

        return seguimientoSemanalAvanceFisico;
    }

}
