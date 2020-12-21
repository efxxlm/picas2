import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-gestion-social',
  templateUrl: './gestion-social.component.html',
  styleUrls: ['./gestion-social.component.scss']
})
export class GestionSocialComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    formGestionSocial: FormGroup;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    gestionSocial: any;
    seguimientoSemanalGestionObraSocialId = 0;
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
        private avanceSemanalSvc: RegistrarAvanceSemanalService )
    {
        this.crearFormulario();
        this.totalEmpleos();

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
                        if ( empleosDirectos > 0 || empleosIndirectos > 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                    } else {
                        empleosDirectos = 0;
                        if ( empleosDirectos > 0 || empleosIndirectos > 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                        if ( empleosDirectos === 0 && empleosIndirectos === 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( '' );
                        }
                    }
                }
            );
        this.formGestionSocial.get( 'cantidadEmpleosIndirectos' ).valueChanges
            .subscribe(
                response => {
                    if ( response.length > 0 ) {
                        empleosIndirectos = Number( response );
                        if ( empleosDirectos > 0 || empleosIndirectos > 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                    } else {
                        empleosIndirectos = 0;
                        if ( empleosDirectos > 0 || empleosIndirectos > 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                        if ( empleosDirectos === 0 && empleosIndirectos === 0 ) {
                            this.formGestionSocial.get( 'cantidadTotalEmpleos' ).setValue( '' );
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
        console.log( this.formGestionSocial.value );
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
        pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
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
