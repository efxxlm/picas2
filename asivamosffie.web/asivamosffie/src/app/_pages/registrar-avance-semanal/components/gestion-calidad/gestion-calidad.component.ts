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
    booleanosEnsayosLaboratorio: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];
    tipoEnsayos: any[] = [
        { codigo: 1, viewValue: 'Muestra de tierra' }
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
    seRealizoPeticion = false;

    get ensayosLaboratorio() {
        return this.formGestionCalidad.get( 'ensayosLaboratorio' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.crearFormulario();
        this.getCantidadEnsayos();
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        this.formGestionCalidad = this.fb.group({
            esEnsayosLaboratorio: [ null ],
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

    getCantidadEnsayos() {
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
        this.seRealizoPeticion = true;
    }

}
