import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-gestion-calidad',
  templateUrl: './gestion-calidad.component.html',
  styleUrls: ['./gestion-calidad.component.scss']
})
export class GestionCalidadComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoObservacionCalidad: any;
    formGestionCalidad: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    formEnsayo: FormGroup = this.fb.group(
        {
            ensayos: this.fb.array( [] )
        }
    );
    tablaHistorial = new MatTableDataSource();
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    dataHistorial: any[] = [];
    seRealizoPeticion = false;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    seguimientoSemanalGestionObraCalidadId = 0;
    seguimientoSemanalObservacionId = 0;
    gestionObraCalidad: any;
    tipoEnsayos: any[] = [];
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
    get ensayos() {
        return this.formEnsayo.get( 'ensayos' ) as FormArray;
    }

    constructor(
        private dialog: MatDialog,
        private routes: Router,
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private registrarAvanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService )
    {
        this.commonSvc.listaTipoEnsayos()
            .subscribe( tipo => this.tipoEnsayos = tipo );
    }

    ngOnInit(): void {
        this.getGestionCalidad();
        this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
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
                    this.seguimientoSemanalGestionObraCalidadId = this.gestionObraCalidad.seguimientoSemanalGestionObraCalidadId;
                    //GET gestion de calidad
                    if ( this.gestionObraCalidad.observacionApoyoId !== undefined ) {
                        this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalGestionObraCalidadId, this.tipoObservacionCalidad.gestionCalidadCodigo )
                            .subscribe(
                                response => {
                                    const observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                    if ( observacionApoyo[0].observacion !== undefined ) {
                                        if ( observacionApoyo[0].observacion.length > 0 ) {
                                            this.formGestionCalidad.get( 'observaciones' ).setValue( observacionApoyo[0].observacion );
                                        }
                                    }
                                    this.dataHistorial = response.filter( obs => obs.archivada === true );
                                    this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                    this.seguimientoSemanalObservacionId = observacionApoyo[0].seguimientoSemanalObservacionId;
                                    this.formGestionCalidad.get( 'tieneObservaciones' ).setValue( this.gestionObraCalidad.tieneObservacionApoyo );
                                    this.formGestionCalidad.get( 'fechaCreacion' ).setValue( observacionApoyo[0].fechaCreacion );
                                }
                            );
                    }
                }
                // GET ensayos de laboratorio
                if ( this.gestionObraCalidad.gestionObraCalidadEnsayoLaboratorio.length > 0 ) {
                    for ( const ensayo of this.gestionObraCalidad.gestionObraCalidadEnsayoLaboratorio ) {
                        this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, ensayo.gestionObraCalidadEnsayoLaboratorioId, this.tipoObservacionCalidad.ensayosLaboratorio )
                            .subscribe(
                                response => {
                                    const observacionApoyo = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                    const historial = response.filter( obs => obs.archivada === true );
                                    let estadoSemaforo = 'sin-diligenciar';
                                    if ( ensayo.registroCompletoObservacionApoyo === false ) {
                                        estadoSemaforo = 'en-proceso';
                                    }
                                    if ( ensayo.registroCompletoObservacionApoyo === true ) {
                                        estadoSemaforo = 'completo';
                                    }
                                    this.ensayos.push( this.fb.group(
                                        {
                                            estadoSemaforo,
                                            tipoEnsayoCodigo: ensayo.tipoEnsayoCodigo,
                                            numeroMuestras: ensayo.numeroMuestras,
                                            fechaTomaMuestras: ensayo.fechaTomaMuestras,
                                            fechaEntregaResultados: ensayo.fechaEntregaResultados,
                                            realizoControlMedicion: ensayo.realizoControlMedicion,
                                            observacion: ensayo.observacion,
                                            urlSoporteGestion: ensayo.urlSoporteGestion,
                                            registroCompletoMuestras: ensayo.registroCompletoMuestras,
                                            gestionObraCalidadEnsayoLaboratorioId: ensayo.gestionObraCalidadEnsayoLaboratorioId,
                                            tieneObservaciones: [ ensayo.tieneObservacionApoyo !== undefined ? ensayo.tieneObservacionApoyo : null, Validators.required ],
                                            observacionEnsayo: observacionApoyo[0].observacion !== undefined ? ( observacionApoyo[0].observacion.length > 0 ? observacionApoyo[0].observacion : null ) : null,
                                            fechaCreacion: observacionApoyo.length > 0 ? observacionApoyo[0].fechaCreacion : null,
                                            seguimientoSemanalObservacionId: ensayo.observacionApoyoId !== undefined ? ensayo.observacionApoyoId : 0,
                                            historial: [ historial ]
                                        }
                                    ) );
                                }
                            );
                    }
                }
            }
        }
    }

    getHistorialEnsayo( historial: any[] ) {
        return new MatTableDataSource( historial );
    }

    getTipoEnsayo( tipoEnsayoCodigo: string ) {
        if ( this.tipoEnsayos.length > 0 && this.gestionObraCalidad !== undefined ) {
            const tipoEnsayo = this.tipoEnsayos.filter( ensayo => ensayo.codigo === tipoEnsayoCodigo );
            return tipoEnsayo[0].nombre;
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
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    getVerDetalleMuestras( gestionObraCalidadEnsayoLaboratorioId: number ) {
        this.routes.navigate( [ `${ this.routes.url }/verDetalleMuestras`, gestionObraCalidadEnsayoLaboratorioId ] );
    }

    guardar() {
        if ( this.formGestionCalidad.get( 'tieneObservaciones' ).value === false && this.formGestionCalidad.get( 'observaciones' ).value !== null ) {
            this.formGestionCalidad.get( 'observaciones' ).setValue( '' );
        }
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.seguimientoSemanalObservacionId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoObservacionCalidad.gestionCalidadCodigo,
            observacionPadreId: this.seguimientoSemanalGestionObraId,
            observacion: this.formGestionCalidad.get( 'observaciones' ).value,
            tieneObservacion: this.formGestionCalidad.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'False' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                                ]
                                            )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarEnsayo( ensayo: FormGroup ) {
        if ( ensayo.get( 'tieneObservaciones' ).value === false && ensayo.get( 'observacionEnsayo' ).value !== null ) {
            ensayo.get( 'observacionEnsayo' ).setValue( '' );
        }
		const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: ensayo.get( 'seguimientoSemanalObservacionId' ).value,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoObservacionCalidad.ensayosLaboratorio,
            observacionPadreId: ensayo.get( 'gestionObraCalidadEnsayoLaboratorioId' ).value,
            observacion: ensayo.get( 'observacionEnsayo' ).value,
            tieneObservacion: ensayo.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'False' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
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
