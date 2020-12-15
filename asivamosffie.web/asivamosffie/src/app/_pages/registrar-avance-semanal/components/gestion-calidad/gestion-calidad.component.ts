import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-gestion-calidad',
  templateUrl: './gestion-calidad.component.html',
  styleUrls: ['./gestion-calidad.component.scss']
})
export class GestionCalidadComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    formGestionCalidad: FormGroup;
    seRealizoPeticion = false;
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    booleanosEnsayosLaboratorio: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];
    tipoEnsayos: any[] = [];
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

    get ensayosLaboratorio() {
        return this.formGestionCalidad.get( 'ensayosLaboratorio' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private commonSvc: CommonService,
        private avanceSemanalSvc: RegistrarAvanceSemanalService )
    {
        this.commonSvc.listaTipoEnsayos()
            .subscribe(
                tipo => {
                    this.tipoEnsayos = tipo;
                    console.log( this.tipoEnsayos );
                }
            );
        this.crearFormulario();
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
        if ( value.length > 0 ) {
            if ( Number( value ) <= 0 ) {
                this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( '1' );
            }
            if ( Number( value ) > 10 ) {
                this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( '10' );
            }
        }
    }

    validateNumber( value: string, index: number, campoForm: string ) {
        if ( isNaN( Number( value ) ) === true ) {
            this.ensayosLaboratorio.at( index ).get( campoForm ).setValue( '' );
        }
    }

    getGestionCalidad() {
        let gestionObraCalidad: any;
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;
        }
        if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad.length > 0 )
        {
            gestionObraCalidad = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad[0];
            if ( gestionObraCalidad.seRealizaronEnsayosLaboratorio !== undefined ) {
                this.formGestionCalidad.get( 'seRealizaronEnsayosLaboratorio' )
                    .setValue( gestionObraCalidad.seRealizaronEnsayosLaboratorio );
                this.formGestionCalidad.markAsDirty();
            }
        }
        this.getCantidadEnsayos( gestionObraCalidad );
    }

    getCantidadEnsayos( gestionObraCalidad: any ) {
        console.log( gestionObraCalidad, 'linea 108' );
        if ( gestionObraCalidad === undefined ) {
            this.formGestionCalidad.get( 'cantidadEnsayos' ).valueChanges
            .subscribe(
                value => {
                    if ( Number( value ) < 0 ) {
                        this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( '0' );
                    }
                    if ( Number( value ) > 0 ) {
                        this.ensayosLaboratorio.clear();
                        for ( let i = 0; i < Number( value ); i++ ) {
                            this.ensayosLaboratorio.push(
                                this.fb.group({
                                    tipoEnsayo: [ null ],
                                    numeroMuestras: [ '' ],
                                    fechaToma: [ null ],
                                    fechaProyectadaEntrega: [ null ],
                                    seRealizoControl: [ null ],
                                    observaciones: [ null ],
                                    urlSoporte: [ '' ]
                                })
                            );
                        }
                    }
                }
            );
        } else {

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

    eliminarEnsayo( numeroPerfil: number ) {
        this.openDialogTrueFalse( '', '¿Está seguro de eliminar esta información?' )
          .subscribe( value => {
            if ( value === true ) {
              this.ensayosLaboratorio.removeAt( numeroPerfil );
              this.formGestionCalidad.patchValue({
                cantidadEnsayos: `${ this.ensayosLaboratorio.length }`
              });
              this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
            }
          } );
    }

    getRegistrarResultados() {
        this.routes.navigate( [ `${ this.routes.url }/registroResultadosEnsayo`, 5 ] );
    }

    getVerDetalleMuestras() {
        this.routes.navigate( [ `${ this.routes.url }/verDetalleMuestras`, 6 ] );
    }

    guardar() {
        console.log( this.formGestionCalidad.value );
        const pSeguimientoSemanal = this.seguimientoSemanal;
        this.seRealizoPeticion = true;
        const gestionCalidad =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
        this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad
        : [];
        const seguimientoSemanalGestionObra = [
            {
                seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                seguimientoSemanalGestionObraCalidad: [
                    {
                        SeguimientoSemanalGestionObraCalidadId: gestionCalidad.length > 0 ?
                                                                gestionCalidad[0].SeguimientoSemanalGestionObraCalidadId : 0,
                        SeguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                        SeRealizaronEnsayosLaboratorio: this.formGestionCalidad.get( 'seRealizaronEnsayosLaboratorio' ).value
                    }
                ]
            }
        ];

        pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
        console.log( pSeguimientoSemanal );
        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    console.log( this.routes.url );
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
