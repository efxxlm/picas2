import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-gestion-sst',
  templateUrl: './gestion-sst.component.html',
  styleUrls: ['./gestion-sst.component.scss']
})
export class GestionSSTComponent implements OnInit {

    @Input() esVerDetalle = false;
    formSst: FormGroup;
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
    causasDeAccidentes: any[] = [
        { codigo: 1, viewValue: 'Incumplimiento del uso de los EPP' }
    ];
    resultadosRevision: any[] = [
        { codigo: 1, viewValue: 'Cumple' }
    ];

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        this.formSst = this.fb.group({
            cantidadAccidentes: [ '' ],
            principalesCausasAccidente: [ null ],
            seRealizaronCapacitaciones: [ null ],
            temaCapacitacion: [ null ],
            seRealizaronRevisiones: [ null ],
            resultadoRevision: [ null ],
            seRealizoRevisionSeñalizacion: [ null ],
            urlSoporte: [ '' ]
        });
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
        console.log( this.formSst.value );
    }

}
