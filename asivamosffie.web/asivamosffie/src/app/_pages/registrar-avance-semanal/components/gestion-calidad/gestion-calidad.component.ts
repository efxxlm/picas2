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

    getCantidadEnsayos() {
        this.formGestionCalidad.get( 'cantidadEnsayos' ).valueChanges
            .subscribe(
                value => {
                    console.log( Number( value ) );
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

    textoLimpio(texto: string) {
        if ( texto ){
            const textolimpio = texto.replace(/<[^>]*>/g, '');
            return textolimpio.length;
        }
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
          e.editor.deleteText(n, e.editor.getLength());
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

    btnEnabled( ensayo: FormGroup ) {
        console.log( ensayo );
    }

    getRegistrarResultados() {
        this.routes.navigate( [ `${ this.routes.url }/registroResultadosEnsayo`, 5 ] );
    }

    getVerDetalleMuestras() {
        this.routes.navigate( [ `${ this.routes.url }/verDetalleMuestras`, 6 ] );
    }

    guardar() {
        console.log( this.formGestionCalidad.value );
    }

}
