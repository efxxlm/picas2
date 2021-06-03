import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatTableDataSource } from '@angular/material/table';
import { GuardadoParcialAvanceSemanalService } from 'src/app/core/_services/guardadoParcialAvanceSemanal/guardado-parcial-avance-semanal.service';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';

@Component({
  selector: 'app-gestion-social',
  templateUrl: './gestion-social.component.html',
  styleUrls: ['./gestion-social.component.scss']
})
export class GestionSocialComponent implements OnInit, OnDestroy {

    @Input() esRegistroNuevo: boolean;
    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() tipoObservacionSocial: any;
    @Output() tieneObservacion = new EventEmitter();
    obsApoyo: any;
    obsSupervisor: any;
    seRealizoPeticion = false;
    formGestionSocial: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    gestionSocial: any;
    seguimientoSemanalGestionObraSocialId = 0;
    tablaHistorial = new MatTableDataSource();
    dataHistorial: any[] = [];
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    booleanosEnsayosLaboratorio: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];
    editorStyle = {
        height: '45px'
    };
    config = {
      toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
      ]
    };
    configTema = {
        toolbar: []
    };

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private guardadoParcialAvanceSemanalSvc: GuardadoParcialAvanceSemanalService )
    {
        this.crearFormulario();
        this.totalEmpleos();

    }

    ngOnDestroy(): void {
        if ( this.formGestionSocial.dirty === true && this.seRealizoPeticion === false ) {
            this.guardadoParcialAvanceSemanalSvc.getDataGestionSocial( this.guardadoParcial(), this.seRealizoPeticion )
        } else {
            this.guardadoParcialAvanceSemanalSvc.getDataGestionSocial( undefined )
        }
    }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;

            if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSocial.length > 0 )
            {
                this.gestionSocial = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraSocial[0];
                if ( this.gestionSocial !== undefined ) {
                    this.seguimientoSemanalGestionObraSocialId = this.gestionSocial.seguimientoSemanalGestionObraSocialId;
                    if ( this.esVerDetalle === false ) {
                        this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.seguimientoSemanalGestionObraSocialId, this.tipoObservacionSocial )
                            .subscribe(
                                response => {
                                    this.obsApoyo = response.find( obs => obs.archivada === false && obs.esSupervisor === false && obs.tieneObservacion === true );
                                    this.obsSupervisor  = response.find( obs => obs.archivada === false && obs.esSupervisor === true && obs.tieneObservacion === true );
                                    this.dataHistorial = response.filter( obs => obs.tieneObservacion === true );

                                    if ( this.obsApoyo !== undefined || this.obsSupervisor !== undefined ) {
                                        this.tieneObservacion.emit();
                                    }

                                    this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
                                }
                            );
                    }
                    this.formGestionSocial.setValue(
                        {
                            cantidadEmpleosDirectos:    this.gestionSocial.cantidadEmpleosDirectos !== undefined ?
                                                        String( this.gestionSocial.cantidadEmpleosDirectos ) : '',
                            cantidadEmpleosIndirectos:  this.gestionSocial.cantidadEmpleosIndirectos !== undefined ?
                                                        String( this.gestionSocial.cantidadEmpleosIndirectos ) : '',
                            cantidadTotalEmpleos:   this.gestionSocial.cantidadTotalEmpleos !== undefined ?
                                                    String( this.gestionSocial.cantidadTotalEmpleos ) : '',
                            seRealizaronReuniones:  this.gestionSocial.seRealizaronReuniones !== undefined ?
                                                    this.gestionSocial.seRealizaronReuniones : null,
                            ObservacionRealizaronReuniones:  this.gestionSocial.ObservacionRealizaronReuniones !== undefined ?
                                                    this.gestionSocial.ObservacionRealizaronReuniones : null,
                            temaComunidad:  this.gestionSocial.temaComunidad !== undefined ?
                                            this.gestionSocial.temaComunidad : null,
                            conclusion: this.gestionSocial.conclusion !== undefined ?
                                        this.gestionSocial.conclusion : null,
                            urlSoporteGestion:  this.gestionSocial.urlSoporteGestion !== undefined ?
                                                this.gestionSocial.urlSoporteGestion : ''
                        }
                    );
                }
            }
        }
        if (!this.esRegistroNuevo) this.formGestionSocial.markAllAsTouched();
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

    totalEmpleos() {
        let empleosDirectos = 0;
        let empleosIndirectos = 0;
        this.formGestionSocial.get( 'cantidadEmpleosDirectos' ).valueChanges
            .subscribe(
                response => {
                    if ( response.length > 0 ) {
                        empleosDirectos = Number( response );
                        if ( empleosDirectos >= 0 || empleosIndirectos >= 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                    } else {
                        empleosDirectos = 0;
                        if ( empleosDirectos >= 0 || empleosIndirectos >= 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                        if ( empleosDirectos === 0 && empleosIndirectos === 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( '0' );
                        }
                    }
                }
            );
        this.formGestionSocial.get( 'cantidadEmpleosIndirectos' ).valueChanges
            .subscribe(
                response => {
                    if ( response.length > 0 ) {
                        empleosIndirectos = Number( response );
                        if ( empleosDirectos >= 0 || empleosIndirectos >= 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                    } else {
                        empleosIndirectos = 0;
                        if ( empleosDirectos >= 0 || empleosIndirectos >= 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                        if ( empleosDirectos === 0 && empleosIndirectos === 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( '0' );
                        }
                    }
                }
            );
    }

    crearFormulario() {
        this.formGestionSocial = this.fb.group({
            cantidadEmpleosDirectos: [ '' ],
            cantidadEmpleosIndirectos: [ '' ],
            cantidadTotalEmpleos: [ '' ],
            seRealizaronReuniones: [ null ],
            ObservacionRealizaronReuniones: [ null ],
            temaComunidad: [ null ],
            conclusion: [ null ],
            urlSoporteGestion: [ '' ]
        });
    }

    validateNumber( value: string, campoForm: string ) {
        if ( isNaN( Number( value ) ) === true ) {
            this.formGestionSocial.get( campoForm ).setValue( '' );
        }
        if ( isNaN( Number( value ) ) === false ) {
            if ( Number( value ) < 0 ) {
                this.formGestionSocial.get( campoForm ).setValue( '0' );
            }
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        this.seRealizoPeticion = true;
        const pSeguimientoSemanal = this.seguimientoSemanal;
        const seguimientoSemanalGestionObra = [
            {
                seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                seguimientoSemanalGestionObraSocial: [
                    {
                        seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                        seguimientoSemanalGestionObraSocialId: this.seguimientoSemanalGestionObraSocialId,
                        cantidadEmpleosDirectos:    this.formGestionSocial.get( 'cantidadEmpleosDirectos' ).value.length > 0 ?
                                                    this.formGestionSocial.get( 'cantidadEmpleosDirectos' ).value : '',
                        cantidadEmpleosIndirectos:  this.formGestionSocial.get( 'cantidadEmpleosIndirectos' ).value.length > 0 ?
                                                    this.formGestionSocial.get( 'cantidadEmpleosIndirectos' ).value : '',
                        cantidadTotalEmpleos:   this.formGestionSocial.get( 'cantidadTotalEmpleos' ).value.length > 0 ?
                                                this.formGestionSocial.get( 'cantidadTotalEmpleos' ).value : '',
                        seRealizaronReuniones:  this.formGestionSocial.get( 'seRealizaronReuniones' ).value !== null ?
                                                this.formGestionSocial.get( 'seRealizaronReuniones' ).value : null,
                        ObservacionRealizaronReuniones:  this.formGestionSocial.get( 'ObservacionRealizaronReuniones' ).value !== null ?
                                                this.formGestionSocial.get( 'ObservacionRealizaronReuniones' ).value : null,
                        temaComunidad:  this.formGestionSocial.get( 'temaComunidad' ).value !== null ?
                                        this.formGestionSocial.get( 'temaComunidad' ).value : null,
                        conclusion: this.formGestionSocial.get( 'conclusion' ).value !== null ?
                                    this.formGestionSocial.get( 'conclusion' ).value : null,
                        urlSoporteGestion:  this.formGestionSocial.get( 'urlSoporteGestion' ).value.length > 0 ?
                                            this.formGestionSocial.get( 'urlSoporteGestion' ).value : '',
                    }
                ]
            }
        ];

        if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
            if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraSocial = [
                    {
                        seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                        seguimientoSemanalGestionObraSocialId: this.seguimientoSemanalGestionObraSocialId,
                        cantidadEmpleosDirectos:    this.formGestionSocial.get( 'cantidadEmpleosDirectos' ).value.length > 0 ?
                                                    this.formGestionSocial.get( 'cantidadEmpleosDirectos' ).value : '',
                        cantidadEmpleosIndirectos:  this.formGestionSocial.get( 'cantidadEmpleosIndirectos' ).value.length > 0 ?
                                                    this.formGestionSocial.get( 'cantidadEmpleosIndirectos' ).value : '',
                        cantidadTotalEmpleos:   this.formGestionSocial.get( 'cantidadTotalEmpleos' ).value.length > 0 ?
                                                this.formGestionSocial.get( 'cantidadTotalEmpleos' ).value : '',
                        seRealizaronReuniones:  this.formGestionSocial.get( 'seRealizaronReuniones' ).value !== null ?
                                                this.formGestionSocial.get( 'seRealizaronReuniones' ).value : null,
                        ObservacionRealizaronReuniones:  this.formGestionSocial.get( 'ObservacionRealizaronReuniones' ).value !== null ?
                                                this.formGestionSocial.get( 'ObservacionRealizaronReuniones' ).value : null,
                        temaComunidad:  this.formGestionSocial.get( 'temaComunidad' ).value !== null ?
                                        this.formGestionSocial.get( 'temaComunidad' ).value : null,
                        conclusion: this.formGestionSocial.get( 'conclusion' ).value !== null ?
                                    this.formGestionSocial.get( 'conclusion' ).value : null,
                        urlSoporteGestion:  this.formGestionSocial.get( 'urlSoporteGestion' ).value.length > 0 ?
                                            this.formGestionSocial.get( 'urlSoporteGestion' ).value : '',
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
                async response => {
                    if ( this.obsApoyo !== undefined ) {
                        this.obsApoyo.archivada = !this.obsApoyo.archivada;
                        await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyo ).toPromise();
                    }
                    if ( this.obsSupervisor !== undefined ) {
                        this.obsSupervisor.archivada = !this.obsSupervisor.archivada;
                        await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisor ).toPromise();
                    }

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
        return [
            {
                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                seguimientoSemanalGestionObraSocialId: this.seguimientoSemanalGestionObraSocialId,
                cantidadEmpleosDirectos:    this.formGestionSocial.get( 'cantidadEmpleosDirectos' ).value.length > 0 ?
                                            this.formGestionSocial.get( 'cantidadEmpleosDirectos' ).value : '',
                cantidadEmpleosIndirectos:  this.formGestionSocial.get( 'cantidadEmpleosIndirectos' ).value.length > 0 ?
                                            this.formGestionSocial.get( 'cantidadEmpleosIndirectos' ).value : '',
                cantidadTotalEmpleos:   this.formGestionSocial.get( 'cantidadTotalEmpleos' ).value.length > 0 ?
                                        this.formGestionSocial.get( 'cantidadTotalEmpleos' ).value : '',
                seRealizaronReuniones:  this.formGestionSocial.get( 'seRealizaronReuniones' ).value !== null ?
                                        this.formGestionSocial.get( 'seRealizaronReuniones' ).value : null,
                ObservacionRealizaronReuniones:  this.formGestionSocial.get( 'ObservacionRealizaronReuniones' ).value !== null ?
                                        this.formGestionSocial.get( 'ObservacionRealizaronReuniones' ).value : null,
                temaComunidad:  this.formGestionSocial.get( 'temaComunidad' ).value !== null ?
                                this.formGestionSocial.get( 'temaComunidad' ).value : null,
                conclusion: this.formGestionSocial.get( 'conclusion' ).value !== null ?
                            this.formGestionSocial.get( 'conclusion' ).value : null,
                urlSoporteGestion:  this.formGestionSocial.get( 'urlSoporteGestion' ).value.length > 0 ?
                                    this.formGestionSocial.get( 'urlSoporteGestion' ).value : '',
            }
        ]
    }

}
