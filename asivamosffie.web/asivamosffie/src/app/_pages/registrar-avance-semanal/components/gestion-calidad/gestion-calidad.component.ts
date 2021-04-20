import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { Router } from '@angular/router';
import { Component, Input, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { GuardadoParcialAvanceSemanalService } from 'src/app/core/_services/guardadoParcialAvanceSemanal/guardado-parcial-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';

@Component({
  selector: 'app-gestion-calidad',
  templateUrl: './gestion-calidad.component.html',
  styleUrls: ['./gestion-calidad.component.scss']
})
export class GestionCalidadComponent implements OnInit, OnDestroy {

    @Input() esRegistroNuevo: boolean;
    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoObservacionCalidad: any;
    @Output() tieneObservacion = new EventEmitter();
    obsApoyo: any;
    obsSupervisor: any;
    formGestionCalidad: FormGroup;
    seRealizoPeticion = false;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    SeguimientoSemanalGestionObraCalidadId = 0;
    gestionObraCalidad: any;
    maxDate: Date;
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    booleanosEnsayosLaboratorio: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];
    tipoEnsayos: any[] = [];
    editorStyle = {
        height: '45px'
    };
    config = {
      toolbar: []
    };
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];

    get ensayosLaboratorio() {
        return this.formGestionCalidad.get( 'ensayosLaboratorio' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private commonSvc: CommonService,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private guardadoParcialAvanceSemanalSvc: GuardadoParcialAvanceSemanalService )
    {
        this.commonSvc.listaTipoEnsayos()
            .subscribe( tipo => this.tipoEnsayos = tipo );
        this.crearFormulario();
        this.getCantidadEnsayos();
    }

    ngOnDestroy(): void {
        if ( this.seRealizoPeticion === false ) {
            if ( this.formGestionCalidad.dirty === true || this.ensayosLaboratorio.dirty === true ) {
                this.guardadoParcialAvanceSemanalSvc.getDataGestionCalidad( this.guardadoParcial(), this.seRealizoPeticion )
            }

            if ( this.formGestionCalidad.dirty === false && this.ensayosLaboratorio.dirty === false ) {
                this.guardadoParcialAvanceSemanalSvc.getDataGestionCalidad( undefined )
            }
        } else {
            this.guardadoParcialAvanceSemanalSvc.getDataGestionCalidad( undefined )
        }
    }

    ngOnInit(): void {
        this.getGestionCalidad();
    }

    crearFormulario() {
        this.formGestionCalidad = this.fb.group({
            seRealizaronEnsayosLaboratorio: [ null ],
            cantidadEnsayos: [ '' ],
            ensayosLaboratorio: this.fb.array( [] )
        });
    }

    valuePending( value: string ) {
        if ( isNaN( Number( value ) ) === true ) {
            this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( '' );
        } else {
            if ( value.length > 0 ) {
                if ( Number( value ) <= 0 ) {
                    this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( '1' );
                }
                if ( Number( value ) > 10 ) {
                    this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( '10' );
                }
            }
        }
    }

    validateNumber( value: string, index: number, campoForm: string ) {
        if ( isNaN( Number( value ) ) === true ) {
            this.ensayosLaboratorio.at( index ).get( campoForm ).setValue( '' );
        }
    }

    semaforoBtnRegistrar( value: boolean ) {
        if ( value === null || value === undefined ) {
            return 'sin-diligenciar';
        }
        if ( value === false ) {
            return 'en-proceso';
        }
        if ( value === true ) {
            return 'completo';
        }
    }

    getGestionCalidad() {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;
            if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad.length > 0 )
            {
                this.gestionObraCalidad = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad[0];
                if ( this.gestionObraCalidad.seRealizaronEnsayosLaboratorio !== undefined ) {
                    this.SeguimientoSemanalGestionObraCalidadId = this.gestionObraCalidad.seguimientoSemanalGestionObraCalidadId;
                    this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( `${ this.gestionObraCalidad.gestionObraCalidadEnsayoLaboratorio.length }` );
                    this.formGestionCalidad.get( 'seRealizaronEnsayosLaboratorio' ).setValue( this.gestionObraCalidad.seRealizaronEnsayosLaboratorio );

                    if ( this.formGestionCalidad.get( 'seRealizaronEnsayosLaboratorio' ).value === false ) {
                        if ( this.esVerDetalle === false ) {
                            this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.SeguimientoSemanalGestionObraCalidadId, this.tipoObservacionCalidad.gestionCalidadCodigo )
                                .subscribe(
                                    response => {
                                        this.obsApoyo  = response.find( obs => obs.archivada === false && obs.esSupervisor === false );
                                        this.obsSupervisor  = response.find( obs => obs.archivada === false && obs.esSupervisor === true );
                                        this.dataHistorial = response;

                                        if ( this.obsApoyo !== undefined || this.obsSupervisor !== undefined ) {
                                            this.tieneObservacion.emit();
                                        }

                                        this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                    }
                                );
                        }
                    }
                }
            }
        }
    }

    getTipoEnsayo( tipoEnsayoCodigo: string ) {
        if ( this.tipoEnsayos.length > 0 && this.gestionObraCalidad !== undefined ) {
            const tipoEnsayo = this.tipoEnsayos.filter( ensayo => ensayo.codigo === tipoEnsayoCodigo );
            return tipoEnsayo[0].nombre;
        }
    }

    getHistorialEnsayo( historial: any[] ) {
        return new MatTableDataSource( historial );
    }

    getCantidadEnsayos() {
        this.formGestionCalidad.get( 'cantidadEnsayos' ).valueChanges
            .subscribe(
                async value => {
                    if ( this.gestionObraCalidad !== undefined && this.gestionObraCalidad.gestionObraCalidadEnsayoLaboratorio.length > 0 ) {
                        this.ensayosLaboratorio.clear();
                        for ( const ensayo of this.gestionObraCalidad.gestionObraCalidadEnsayoLaboratorio ) {
                            let semaforoEnsayo: string;
                            let historial = [];
                            let obsApoyo: any;
                            let obsSupervisor: any;
                            if ( ensayo.registroCompleto === true ) {
                                semaforoEnsayo = 'completo';
                            }

                            if (    ensayo.registroCompleto === false
                                    && (    ensayo.tipoEnsayoCodigo !== undefined
                                            || ensayo.numeroMuestras !== undefined
                                            || ensayo.fechaTomaMuestras !== undefined
                                            || ensayo.fechaEntregaResultados !== undefined
                                            || ensayo.realizoControlMedicion !== undefined
                                            || ensayo.observacion !== undefined ) )
                            {
                                semaforoEnsayo = 'en-proceso';
                            }
                            if ( this.esVerDetalle === false ) {
                                const response = await this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, ensayo.gestionObraCalidadEnsayoLaboratorioId, this.tipoObservacionCalidad.ensayosLaboratorio ).toPromise();
                                obsApoyo = response.find( obs => obs.archivada === false && obs.esSupervisor === false );
                                obsSupervisor = response.find( obs => obs.archivada === false && obs.esSupervisor === true );
                                historial = response;

                                if ( obsApoyo !== undefined || obsSupervisor !== undefined ) {
                                    semaforoEnsayo = 'en-proceso';
                                    this.tieneObservacion.emit();
                                }
                            }
                            this.ensayosLaboratorio.push(
                                this.fb.group(
                                    {
                                        registroCompletoMuestras: [ ensayo.registroCompletoMuestras !== undefined ? ensayo.registroCompletoMuestras : null ],
                                        semaforoEnsayo: [ semaforoEnsayo !== undefined ? semaforoEnsayo : 'sin-diligenciar' ],
                                        obsApoyo: [ obsApoyo !== undefined ? obsApoyo : null ],
                                        obsSupervisor: [ obsSupervisor !== undefined ? obsSupervisor : null ],
                                        gestionObraCalidadEnsayoLaboratorioId: [ ensayo.gestionObraCalidadEnsayoLaboratorioId ],
                                        seguimientoSemanalGestionObraCalidadId: [ ensayo.seguimientoSemanalGestionObraCalidadId ],
                                        tipoEnsayoCodigo: [ ensayo.tipoEnsayoCodigo !== undefined ? ensayo.tipoEnsayoCodigo : null ],
                                        numeroMuestras: [ ensayo.numeroMuestras !== undefined ? String( ensayo.numeroMuestras ) : '' ],
                                        fechaTomaMuestras:  [ ensayo.fechaTomaMuestras !== undefined ? new Date( ensayo.fechaTomaMuestras ) : null ],
                                        fechaEntregaResultados: [ ensayo.fechaEntregaResultados !== undefined ? new Date( ensayo.fechaEntregaResultados ) : null ],
                                        realizoControlMedicion: [ ensayo.realizoControlMedicion !== undefined ? ensayo.realizoControlMedicion : null ],
                                        observacion: [ ensayo.observacion !== undefined ? ensayo.observacion : null ],
                                        urlSoporteGestion: [ ensayo.urlSoporteGestion !== undefined ? ensayo.urlSoporteGestion : '' ],
                                        registroCompleto: [ ensayo.registroCompleto ],
                                        historial: [ historial ]
                                    }
                                )
                            );
                            
                        }
                        this.formGestionCalidad.get( 'cantidadEnsayos' ).setValidators( Validators.min( this.ensayosLaboratorio.length ) );
                        const nuevosEnsayos = Number( value ) - this.ensayosLaboratorio.length;
                        if ( Number( value ) < this.ensayosLaboratorio.length && Number( value ) > 0 ) {
                          this.openDialog(
                            '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                          );
                          this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( String( this.ensayosLaboratorio.length ) );
                          return;
                        }
                        for ( let i = 0; i < nuevosEnsayos; i++ ) {
                            this.ensayosLaboratorio.push(
                                this.fb.group({
                                    registroCompletoMuestras: [ null ],
                                    semaforoEnsayo: [ 'sin-diligenciar' ],
                                    obsApoyo: [ null ],
                                    obsSupervisor: [ null ],
                                    gestionObraCalidadEnsayoLaboratorioId : [ 0 ],
                                    seguimientoSemanalGestionObraCalidadId: [ this.SeguimientoSemanalGestionObraCalidadId ],
                                    tipoEnsayoCodigo: [ null ],
                                    numeroMuestras: [ '' ],
                                    fechaTomaMuestras: [ null ],
                                    fechaEntregaResultados: [ null ],
                                    realizoControlMedicion: [ null ],
                                    observacion: [ null ],
                                    urlSoporteGestion: [ '' ],
                                    registroCompleto: [ false ],
                                    historial: [ [] ]
                                })
                            );
                        }
                    }
                    if (    this.gestionObraCalidad !== undefined
                            && this.gestionObraCalidad.gestionObraCalidadEnsayoLaboratorio.length === 0 )
                    {
                        if ( Number( value ) < 0 ) {
                            this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( '0' );
                        }
                        if ( Number( value ) > 0 ) {
                            if ( this.ensayosLaboratorio.dirty === true ) {
                                this.formGestionCalidad.get( 'cantidadEnsayos' )
                                .setValidators( Validators.min( this.ensayosLaboratorio.length ) );
                                const nuevosEnsayos = Number( value ) - this.ensayosLaboratorio.length;
                                if ( Number( value ) < this.ensayosLaboratorio.length && Number( value ) > 0 ) {
                                  this.openDialog(
                                    '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                                  );
                                  this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( String( this.ensayosLaboratorio.length ) );
                                  return;
                                }
                                for ( let i = 0; i < nuevosEnsayos; i++ ) {
                                    this.ensayosLaboratorio.push(
                                        this.fb.group({
                                            registroCompletoMuestras: [ null ],
                                            semaforoEnsayo: [ 'sin-diligenciar' ],
                                            obsApoyo: [ null ],
                                            obsSupervisor: [ null ],
                                            gestionObraCalidadEnsayoLaboratorioId : [ 0 ],
                                            seguimientoSemanalGestionObraCalidadId: [ this.SeguimientoSemanalGestionObraCalidadId ],
                                            tipoEnsayoCodigo: [ null ],
                                            numeroMuestras: [ '' ],
                                            fechaTomaMuestras: [ null ],
                                            fechaEntregaResultados: [ null ],
                                            realizoControlMedicion: [ null ],
                                            observacion: [ null ],
                                            urlSoporteGestion: [ '' ],
                                            registroCompleto: [ false ],
                                            historial: [ [] ]
                                        })
                                    );
                                }
                            } else {
                                this.ensayosLaboratorio.clear();
                                for ( let i = 0; i < Number( value ); i++ ) {
                                    this.ensayosLaboratorio.push(
                                        this.fb.group({
                                            registroCompletoMuestras: [ null ],
                                            semaforoEnsayo: [ 'sin-diligenciar' ],
                                            obsApoyo: [ null ],
                                            obsSupervisor: [ null ],
                                            gestionObraCalidadEnsayoLaboratorioId : [ 0 ],
                                            seguimientoSemanalGestionObraCalidadId: [ this.SeguimientoSemanalGestionObraCalidadId ],
                                            tipoEnsayoCodigo: [ null ],
                                            numeroMuestras: [ '' ],
                                            fechaTomaMuestras: [ null ],
                                            fechaEntregaResultados: [ null ],
                                            realizoControlMedicion: [ null ],
                                            observacion: [ null ],
                                            urlSoporteGestion: [ '' ],
                                            registroCompleto: [ false ],
                                            historial: [ [] ]
                                        })
                                    );
                                }
                            }
                        }
                    }

                    if ( this.gestionObraCalidad === undefined ) {
                        if ( Number( value ) < 0 ) {
                            this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( '0' );
                        }
                        if ( Number( value ) > 0 ) {
                            if ( this.ensayosLaboratorio.dirty === true ) {
                                this.formGestionCalidad.get( 'cantidadEnsayos' )
                                .setValidators( Validators.min( this.ensayosLaboratorio.length ) );
                                const nuevosEnsayos = Number( value ) - this.ensayosLaboratorio.length;
                                if ( Number( value ) < this.ensayosLaboratorio.length && Number( value ) > 0 ) {
                                  this.openDialog( '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>' );
                                  this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( String( this.ensayosLaboratorio.length ) );
                                  return;
                                }
                                for ( let i = 0; i < nuevosEnsayos; i++ ) {
                                    this.ensayosLaboratorio.push(
                                        this.fb.group({
                                            registroCompletoMuestras: [ null ],
                                            semaforoEnsayo: [ 'sin-diligenciar' ],
                                            obsApoyo: [ null ],
                                            obsSupervisor: [ null ],
                                            gestionObraCalidadEnsayoLaboratorioId : [ 0 ],
                                            seguimientoSemanalGestionObraCalidadId: [ this.SeguimientoSemanalGestionObraCalidadId ],
                                            tipoEnsayoCodigo: [ null ],
                                            numeroMuestras: [ '' ],
                                            fechaTomaMuestras: [ null ],
                                            fechaEntregaResultados: [ null ],
                                            realizoControlMedicion: [ null ],
                                            observacion: [ null ],
                                            urlSoporteGestion: [ '' ],
                                            registroCompleto: [ false ],
                                            historial: [ [] ]
                                        })
                                    );
                                }
                            } else {
                                this.ensayosLaboratorio.clear();
                                for ( let i = 0; i < Number( value ); i++ ) {
                                    this.ensayosLaboratorio.push(
                                        this.fb.group({
                                            registroCompletoMuestras: [ null ],
                                            semaforoEnsayo: [ 'sin-diligenciar' ],
                                            obsApoyo: [ null ],
                                            obsSupervisor: [ null ],
                                            gestionObraCalidadEnsayoLaboratorioId : [ 0 ],
                                            seguimientoSemanalGestionObraCalidadId: [ this.SeguimientoSemanalGestionObraCalidadId ],
                                            tipoEnsayoCodigo: [ null ],
                                            numeroMuestras: [ '' ],
                                            fechaTomaMuestras: [ null ],
                                            fechaEntregaResultados: [ null ],
                                            realizoControlMedicion: [ null ],
                                            observacion: [ null ],
                                            urlSoporteGestion: [ '' ],
                                            registroCompleto: [ false ],
                                            historial: [ [] ]
                                        })
                                    );
                                }
                            }
                        }
                    }
                }
            );
    }

    valuePendingSemaforo( value: boolean ) {
        if ( value === undefined ) {
            return 'sin-diligenciar';
        }
        if ( value === false ) {
            return 'en-proceso';
        }
    }

    convertToNumber( cantidadEnsayos: string ) {
        if ( cantidadEnsayos.length > 0 ) {
            return Number( cantidadEnsayos );
        }
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

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    eliminarEnsayo( gestionObraCalidadEnsayoLaboratorioId: number, numeroEnsayo: number ) {
        this.openDialogTrueFalse( '', '¿Está seguro de eliminar esta información?' )
          .subscribe( value => {
            if ( value === true ) {
                if ( gestionObraCalidadEnsayoLaboratorioId === 0 ) {
                    this.ensayosLaboratorio.removeAt( numeroEnsayo );
                    this.formGestionCalidad.patchValue({
                        cantidadEnsayos: `${ this.ensayosLaboratorio.length }`
                    });
                    this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                } else {
                    this.avanceSemanalSvc.deleteGestionObraCalidadEnsayoLaboratorio( gestionObraCalidadEnsayoLaboratorioId )
                        .subscribe(
                            response => {
                                this.openDialog( '', `<b>${ response.message }</b>` );
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/registrarAvanceSemanal/registroSeguimientoSemanal',
                                                    this.seguimientoSemanal.contratacionProyectoId
                                                ]
                                            )
                                );
                            },
                            err => this.openDialog( '', `<b>${ err.message }</b>` )
                        );
                }
            }
          } );
    }

    getMaxDate( value: Date ) {
        const date = new Date( value );
        const dias = 30;
        date.setDate( date.getDate() + dias );
        this.maxDate = date;
    }

    getRegistrarResultados( gestionObraCalidadEnsayoLaboratorioId: number ) {
        this.routes.navigate( [ `${ this.routes.url }/registroResultadosEnsayo`, gestionObraCalidadEnsayoLaboratorioId ] );
    }

    getVerDetalleMuestras( gestionObraCalidadEnsayoLaboratorioId: number ) {
        this.routes.navigate( [ `${ this.routes.url }/verDetalleMuestras`, gestionObraCalidadEnsayoLaboratorioId ] );
    }

    async guardar() {
        this.ensayosLaboratorio.controls.forEach( value => {
            value.get( 'fechaEntregaResultados' ).setValue(
                value.get( 'fechaEntregaResultados' ).value !== null ?
                new Date( value.get( 'fechaEntregaResultados' ).value ).toISOString() : null
            );
            value.get( 'fechaTomaMuestras' ).setValue(
                value.get( 'fechaTomaMuestras' ).value !== null ?
                new Date( value.get( 'fechaTomaMuestras' ).value ).toISOString() : null
            );
        } );
        const pSeguimientoSemanal = this.seguimientoSemanal;
        const seguimientoSemanalGestionObra = [
            {
                seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                seguimientoSemanalGestionObraCalidad: [
                    {
                        seguimientoSemanalGestionObraCalidadId: this.SeguimientoSemanalGestionObraCalidadId,
                        seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                        seRealizaronEnsayosLaboratorio: this.formGestionCalidad.get( 'seRealizaronEnsayosLaboratorio' ).value,
                        gestionObraCalidadEnsayoLaboratorio: this.formGestionCalidad.get( 'ensayosLaboratorio' ).dirty === true ?
                        this.formGestionCalidad.get( 'ensayosLaboratorio' ).value : null
                    }
                ]
            }
        ];

        if ( this.formGestionCalidad.get( 'seRealizaronEnsayosLaboratorio' ).value !== null ) {
            if ( this.formGestionCalidad.get( 'seRealizaronEnsayosLaboratorio' ).value === false ) {
                if ( this.obsApoyo !== undefined ) {
                    this.obsApoyo.archivada = !this.obsApoyo.archivada;
                    await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyo ).toPromise();
                }
                if ( this.obsSupervisor !== undefined ) {
                    this.obsSupervisor.archivada = !this.obsSupervisor.archivada;
                    await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisor ).toPromise();
                }
            }
        }

        if ( this.ensayosLaboratorio.length > 0 ) {
            this.ensayosLaboratorio.controls.forEach( async control => {
                if ( control.dirty === true ) {
                    if ( control.get( 'obsApoyo' ).value !== null ) {
                        control.get( 'obsApoyo' ).value.archivada = !control.get( 'obsApoyo' ).value.archivada;

                        await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( control.get( 'obsApoyo' ).value ).toPromise();
                    }
                    if ( control.get( 'obsSupervisor' ).value !== null ) {
                        control.get( 'obsSupervisor' ).value.archivada = !control.get( 'obsSupervisor' ).value.archivada;

                        await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( control.get( 'obsSupervisor' ).value ).toPromise();
                    }
                }
            } )
        }

        if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
            if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraCalidad = [
                    {
                        seguimientoSemanalGestionObraCalidadId: this.SeguimientoSemanalGestionObraCalidadId,
                        seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                        seRealizaronEnsayosLaboratorio: this.formGestionCalidad.get( 'seRealizaronEnsayosLaboratorio' ).value,
                        gestionObraCalidadEnsayoLaboratorio: this.formGestionCalidad.get( 'ensayosLaboratorio' ).dirty === true ?
                        this.formGestionCalidad.get( 'ensayosLaboratorio' ).value : null
                    }
                ]
            } else {
                pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
            }
        } else {
            pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
        }

        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                response => {
                    this.seRealizoPeticion = true;
                    this.openDialog( '', `<b>${ response.message }</b>` );
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
        this.ensayosLaboratorio.controls.forEach( value => {
            value.get( 'fechaEntregaResultados' ).setValue(
                value.get( 'fechaEntregaResultados' ).value !== null ?
                new Date( value.get( 'fechaEntregaResultados' ).value ).toISOString() : null
            );
            value.get( 'fechaTomaMuestras' ).setValue(
                value.get( 'fechaTomaMuestras' ).value !== null ?
                new Date( value.get( 'fechaTomaMuestras' ).value ).toISOString() : null
            );
        } )

        return  [
            {
                seguimientoSemanalGestionObraCalidadId: this.SeguimientoSemanalGestionObraCalidadId,
                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                seRealizaronEnsayosLaboratorio: this.formGestionCalidad.get( 'seRealizaronEnsayosLaboratorio' ).value,
                gestionObraCalidadEnsayoLaboratorio: this.formGestionCalidad.get( 'ensayosLaboratorio' ).dirty === true ?
                this.formGestionCalidad.get( 'ensayosLaboratorio' ).value : null
            }
        ]
    }

}
