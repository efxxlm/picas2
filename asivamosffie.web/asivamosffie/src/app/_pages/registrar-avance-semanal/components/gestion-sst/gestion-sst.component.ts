import { Router } from '@angular/router';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-gestion-sst',
  templateUrl: './gestion-sst.component.html',
  styleUrls: ['./gestion-sst.component.scss']
})
export class GestionSSTComponent implements OnInit {

    @Input() esRegistroNuevo: boolean;
    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoObservacionSst: any;
    formSst: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    seguimientoSemanalGestionObraSeguridadSaludId = 0;
    causasDeAccidentes: Dominio[] = [];
    gestionObraSst: any;
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    editorStyle = {
        height: '45px'
    };
    config = {
      toolbar: []
    };
    booleanosEnsayosLaboratorio: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];
    resultadosRevision: any[] = [
        { value: true, viewValue: 'Cumple' },
        { value: false, viewValue: 'No cumple' }
    ];
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
        this.getGestionSst();
    }

    getGestionSst() {
        this.commonSvc.listaCausaAccidente()
        .subscribe( response => {
            this.causasDeAccidentes = response;
            const causas = [];
            if ( this.seguimientoSemanal !== undefined ) {
                this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
                this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
                this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;

                if ( this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSeguridadSalud.length > 0 ) {
                    this.gestionObraSst = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSeguridadSalud[0];
                    if ( this.gestionObraSst.seguridadSaludCausaAccidente !== undefined && this.gestionObraSst.seguridadSaludCausaAccidente.length > 0 ) {
                        for ( const causa of this.gestionObraSst.seguridadSaludCausaAccidente ) {
                            const causaSeleccionada = this.causasDeAccidentes.filter( value => value.codigo === causa.causaAccidenteCodigo );
                            causas.push( causaSeleccionada[0] );
                        }
                        this.formSst.get( 'seguridadSaludCausaAccidente' ).setValue( causas );
                    }

                    if ( this.esVerDetalle === false ) {
                        this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.gestionObraSst.seguimientoSemanalGestionObraSeguridadSaludId, this.tipoObservacionSst )
                            .subscribe(
                                response => {
                                    this.dataHistorial = response.filter( obs => obs.archivada === true );
                                    this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                }
                            );
                    }

                    if ( this.gestionObraSst.cantidadAccidentes !== undefined ) {
                        this.seguimientoSemanalGestionObraSeguridadSaludId = this.gestionObraSst.seguimientoSemanalGestionObraSeguridadSaludId;
                        this.formSst.get( 'cantidadAccidentes' ).setValue( this.gestionObraSst.cantidadAccidentes !== undefined ? `${ this.gestionObraSst.cantidadAccidentes }` : '' );
                        this.formSst.markAsDirty();
                    }
                    this.formSst.patchValue(
                        {
                            seRealizoCapacitacion:  this.gestionObraSst.seRealizoCapacitacion !== undefined ?
                                                    this.gestionObraSst.seRealizoCapacitacion : null,
                            temaCapacitacion:   this.gestionObraSst.temaCapacitacion !== undefined ?
                                                this.gestionObraSst.temaCapacitacion : null,
                            seRealizoRevisionElementosProteccion:   this.gestionObraSst.seRealizoRevisionElementosProteccion !== undefined ?
                                                                    this.gestionObraSst.seRealizoRevisionElementosProteccion : null,
                            cumpleRevisionElementosProyeccion:  this.gestionObraSst.cumpleRevisionElementosProyeccion !== undefined ?
                                                                this.gestionObraSst.cumpleRevisionElementosProyeccion : null,
                            seRealizoRevisionSenalizacion:  this.gestionObraSst.seRealizoRevisionSenalizacion !== undefined ?
                                                            this.gestionObraSst.seRealizoRevisionSenalizacion : null,
                            cumpleRevisionSenalizacion: this.gestionObraSst.cumpleRevisionSenalizacion !== undefined ?
                                                        this.gestionObraSst.cumpleRevisionSenalizacion : null,
                            urlSoporteGestion:  this.gestionObraSst.urlSoporteGestion !== undefined ?
                                                this.gestionObraSst.urlSoporteGestion : ''
                        }
                    );
                }
            }
        } );
    }

    crearFormulario() {
        this.formSst = this.fb.group({
            cantidadAccidentes: [ '' ],
            seguridadSaludCausaAccidente: [ null ],
            seRealizoCapacitacion: [ null ],
            temaCapacitacion: [ null ],
            seRealizoRevisionElementosProteccion: [ null ],
            cumpleRevisionElementosProyeccion: [ null ],
            seRealizoRevisionSenalizacion: [ null ],
            cumpleRevisionSenalizacion: [ null ],
            urlSoporteGestion: [ '' ]
        });
    }

    getCausasDetalle( causas: any[] ) {
        if ( this.causasDeAccidentes.length > 0 && this.seguimientoSemanal !== undefined ) {
            const causaSeleccion = [];
            causas.forEach( causa => {
                this.causasDeAccidentes.filter( value => {
                    if ( causa.causaAccidenteCodigo === value.codigo ) {
                        causaSeleccion.push( value );
                    }
                } );
            } );
            return causaSeleccion;
        }
    }

    validateNumber( value: string, campoForm: string ) {
        if ( isNaN( Number( value ) ) === true ) {
            this.formSst.get( campoForm ).setValue( '' );
        }
    }

    valuePending( value: string ) {
        if ( value.length > 0 ) {
            if ( Number( value ) < 0 ) {
                this.formSst.get( 'cantidadAccidentes' ).setValue( '0' );
            }
        }
    }

    convertToNumber( value: string ) {
        if ( value.length > 0 ) {
            return Number( value );
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

    guardar() {
        const pSeguimientoSemanal = this.seguimientoSemanal;
        const causas = [];
        const causaSeleccionadas = this.formSst.get( 'seguridadSaludCausaAccidente' ).value;
        if ( causaSeleccionadas !== null && causaSeleccionadas.length > 0 ) {
            for ( const causa of causaSeleccionadas ) {
                causas.push(
                    {
                        seguimientoSemanalGestionObraSeguridadSaludId: this.seguimientoSemanalGestionObraSeguridadSaludId,
                        causaAccidenteCodigo: causa.codigo
                    }
                );
            }
        }

        const seguimientoSemanalGestionObra = [
            {
                seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                seguimientoSemanalGestionObraSeguridadSalud: [
                    {
                        seguimientoSemanalGestionObraSeguridadSaludId: this.seguimientoSemanalGestionObraSeguridadSaludId,
                        seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                        cantidadAccidentes: this.formSst.get( 'cantidadAccidentes' ).value.length > 0 ?
                                            this.formSst.get( 'cantidadAccidentes' ).value : '',
                        seguridadSaludCausaAccidente: causas,
                        seRealizoCapacitacion:  this.formSst.get( 'seRealizoCapacitacion' ).value !== null ?
                                                this.formSst.get( 'seRealizoCapacitacion' ).value : null,
                        temaCapacitacion:   this.formSst.get( 'temaCapacitacion' ).value !== null ?
                                            this.formSst.get( 'temaCapacitacion' ).value : null,
                        seRealizoRevisionElementosProteccion:   this.formSst.get( 'seRealizoRevisionElementosProteccion' ).value !== null ?
                                                                this.formSst.get( 'seRealizoRevisionElementosProteccion' ).value : null,
                        cumpleRevisionElementosProyeccion:  this.formSst.get( 'cumpleRevisionElementosProyeccion' ).value !== null ?
                                                            this.formSst.get( 'cumpleRevisionElementosProyeccion' ).value : null,
                        seRealizoRevisionSenalizacion:  this.formSst.get( 'seRealizoRevisionSenalizacion' ).value !== null ?
                                                        this.formSst.get( 'seRealizoRevisionSenalizacion' ).value : null,
                        cumpleRevisionSenalizacion: this.formSst.get( 'cumpleRevisionSenalizacion' ).value !== null ?
                                                    this.formSst.get( 'cumpleRevisionSenalizacion' ).value : null,
                        urlSoporteGestion:  this.formSst.get( 'urlSoporteGestion' ).value.length > 0 ?
                                            this.formSst.get( 'urlSoporteGestion' ).value : null
                    }
                ]
            }
        ];

        if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
            if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraSeguridadSalud = [
                    {
                        seguimientoSemanalGestionObraSeguridadSaludId: this.seguimientoSemanalGestionObraSeguridadSaludId,
                        seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                        cantidadAccidentes: this.formSst.get( 'cantidadAccidentes' ).value.length > 0 ?
                                            this.formSst.get( 'cantidadAccidentes' ).value : '',
                        seguridadSaludCausaAccidente: causas,
                        seRealizoCapacitacion:  this.formSst.get( 'seRealizoCapacitacion' ).value !== null ?
                                                this.formSst.get( 'seRealizoCapacitacion' ).value : null,
                        temaCapacitacion:   this.formSst.get( 'temaCapacitacion' ).value !== null ?
                                            this.formSst.get( 'temaCapacitacion' ).value : null,
                        seRealizoRevisionElementosProteccion:   this.formSst.get( 'seRealizoRevisionElementosProteccion' ).value !== null ?
                                                                this.formSst.get( 'seRealizoRevisionElementosProteccion' ).value : null,
                        cumpleRevisionElementosProyeccion:  this.formSst.get( 'cumpleRevisionElementosProyeccion' ).value !== null ?
                                                            this.formSst.get( 'cumpleRevisionElementosProyeccion' ).value : null,
                        seRealizoRevisionSenalizacion:  this.formSst.get( 'seRealizoRevisionSenalizacion' ).value !== null ?
                                                        this.formSst.get( 'seRealizoRevisionSenalizacion' ).value : null,
                        cumpleRevisionSenalizacion: this.formSst.get( 'cumpleRevisionSenalizacion' ).value !== null ?
                                                    this.formSst.get( 'cumpleRevisionSenalizacion' ).value : null,
                        urlSoporteGestion:  this.formSst.get( 'urlSoporteGestion' ).value.length > 0 ?
                                            this.formSst.get( 'urlSoporteGestion' ).value : null
                    }
                ];
            } else {
                pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
            }
        } else {
            pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
        }

        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                response => {
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

}
