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
        private dialog: MatDialog
     )
    {
        this.crearFormulario();
        this.totalEmpleos();

    }

    ngOnInit(): void {
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
                            this.formGestionSocial.get( 'totalEmpleosGenerados' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                    } else {
                        empleosDirectos = 0;
                        if ( empleosDirectos > 0 || empleosIndirectos > 0 ) {
                            this.formGestionSocial.get( 'totalEmpleosGenerados' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                        if ( empleosDirectos === 0 && empleosIndirectos === 0 ) {
                            this.formGestionSocial.get( 'totalEmpleosGenerados' ).setValue( '' );
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
                            this.formGestionSocial.get( 'totalEmpleosGenerados' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                    } else {
                        empleosIndirectos = 0;
                        if ( empleosDirectos > 0 || empleosIndirectos > 0 ) {
                            this.formGestionSocial.get( 'totalEmpleosGenerados' ).setValue( `${ empleosDirectos + empleosIndirectos }` );
                        }
                        if ( empleosDirectos === 0 && empleosIndirectos === 0 ) {
                            this.formGestionSocial.get( 'totalEmpleosGenerados' ).setValue( '' );
                        }
                    }
                }
            );
    }

    crearFormulario() {
        this.formGestionSocial = this.fb.group({
            cantidadEmpleosDirectos: [ '' ],
            cantidadEmpleosIndirectos: [ '' ],
            totalEmpleosGenerados: [ '' ],
            seRealizaronReuniones: [ null ],
            tema: [ null ],
            conclusiones: [ null ],
            urlSoporte: [ '' ]
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
    }

}
