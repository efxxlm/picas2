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
    SeguimientoSemanalGestionObraCalidadId = 0;
    gestionObraCalidad: any;
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
        this.getCantidadEnsayos();
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
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;
        }
        if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad.length > 0 )
        {
            this.gestionObraCalidad = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraCalidad[0];
            if ( this.gestionObraCalidad.seRealizaronEnsayosLaboratorio !== undefined ) {
                console.log( this.gestionObraCalidad );
                this.SeguimientoSemanalGestionObraCalidadId = this.gestionObraCalidad.seguimientoSemanalGestionObraCalidadId;
                this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( `${ this.gestionObraCalidad.gestionObraCalidadEnsayoLaboratorio.length }` );
                this.formGestionCalidad.get( 'seRealizaronEnsayosLaboratorio' )
                    .setValue( this.gestionObraCalidad.seRealizaronEnsayosLaboratorio );
                this.formGestionCalidad.markAsDirty();
            }
        }
    }

    getCantidadEnsayos() {
        this.formGestionCalidad.get( 'cantidadEnsayos' ).valueChanges
            .subscribe(
                value => {
                    console.log( this.gestionObraCalidad, 'linea 113' );
                    if ( this.gestionObraCalidad !== undefined && this.gestionObraCalidad.gestionObraCalidadEnsayoLaboratorio.length > 0 ) {
                        this.ensayosLaboratorio.clear();
                        for ( const ensayo of this.gestionObraCalidad.gestionObraCalidadEnsayoLaboratorio ) {
                            this.ensayosLaboratorio.push(
                                this.fb.group(
                                    {
                                        gestionObraCalidadEnsayoLaboratorioId: ensayo.gestionObraCalidadEnsayoLaboratorioId,
                                        seguimientoSemanalGestionObraCalidadId: ensayo.seguimientoSemanalGestionObraCalidadId,
                                        tipoEnsayoCodigo: ensayo.tipoEnsayoCodigo !== undefined ? ensayo.tipoEnsayoCodigo : null,
                                        numeroMuestras: ensayo.numeroMuestras !== undefined ? String( ensayo.numeroMuestras ) : '',
                                        fechaTomaMuestras:  ensayo.fechaTomaMuestras !== undefined
                                                            ? new Date( ensayo.fechaTomaMuestras ) : null,
                                        fechaEntregaResultados: ensayo.fechaEntregaResultados !== undefined
                                                                ? new Date( ensayo.fechaEntregaResultados ) : null,
                                        realizoControlMedicion: ensayo.realizoControlMedicion !== undefined
                                                                ? ensayo.realizoControlMedicion : null,
                                        observacion: ensayo.observacion !== undefined ? ensayo.observacion : null,
                                        urlSoporteGestion: ensayo.urlSoporteGestion !== undefined ? ensayo.urlSoporteGestion : '',
                                        registroCompleto: [ ensayo.registroCompleto ]
                                    }
                                )
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
                            this.ensayosLaboratorio.clear();
                            for ( let i = 0; i < Number( value ); i++ ) {
                                this.ensayosLaboratorio.push(
                                    this.fb.group({
                                        gestionObraCalidadEnsayoLaboratorioId : [ 0 ],
                                        seguimientoSemanalGestionObraCalidadId: [ this.SeguimientoSemanalGestionObraCalidadId ],
                                        tipoEnsayoCodigo: [ null ],
                                        numeroMuestras: [ '' ],
                                        fechaTomaMuestras: [ null ],
                                        fechaEntregaResultados: [ null ],
                                        realizoControlMedicion: [ null ],
                                        observacion: [ null ],
                                        urlSoporteGestion: [ '' ],
                                        registroCompleto: [ false ]
                                    })
                                );
                            }
                        }
                    }
                    if ( this.gestionObraCalidad === undefined ) {
                        if ( Number( value ) < 0 ) {
                            this.formGestionCalidad.get( 'cantidadEnsayos' ).setValue( '0' );
                        }
                        if ( Number( value ) > 0 ) {
                            this.ensayosLaboratorio.clear();
                            for ( let i = 0; i < Number( value ); i++ ) {
                                this.ensayosLaboratorio.push(
                                    this.fb.group({
                                        gestionObraCalidadEnsayoLaboratorioId : [ 0 ],
                                        seguimientoSemanalGestionObraCalidadId: [ this.SeguimientoSemanalGestionObraCalidadId ],
                                        tipoEnsayoCodigo: [ null ],
                                        numeroMuestras: [ '' ],
                                        fechaTomaMuestras: [ null ],
                                        fechaEntregaResultados: [ null ],
                                        realizoControlMedicion: [ null ],
                                        observacion: [ null ],
                                        urlSoporteGestion: [ '' ],
                                        registroCompleto: [ false ]
                                    })
                                );
                            }
                        }
                    }
                }
            );
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
                    console.log( 'Condicion 1', gestionObraCalidadEnsayoLaboratorioId );
                    this.ensayosLaboratorio.removeAt( numeroEnsayo );
                    this.formGestionCalidad.patchValue({
                        cantidadEnsayos: `${ this.ensayosLaboratorio.length }`
                    });
                    this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                } else {
                    console.log( 'Condicion 2', gestionObraCalidadEnsayoLaboratorioId );
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

    getRegistrarResultados( gestionObraCalidadEnsayoLaboratorioId: number ) {
        this.routes.navigate( [ `${ this.routes.url }/registroResultadosEnsayo`, gestionObraCalidadEnsayoLaboratorioId ] );
    }

    getVerDetalleMuestras() {
        this.routes.navigate( [ `${ this.routes.url }/verDetalleMuestras`, 6 ] );
    }

    guardar() {
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
        this.seRealizoPeticion = true;
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
