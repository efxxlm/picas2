import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-registrar-resultados-ensayo',
  templateUrl: './registrar-resultados-ensayo.component.html',
  styleUrls: ['./registrar-resultados-ensayo.component.scss']
})
export class RegistrarResultadosEnsayoComponent implements OnInit {

    formMuestra: FormGroup;
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

    constructor(
        private location: Location,
        private fb: FormBuilder,
        private dialog: MatDialog )
    {
        this.crearFormulario();
    }

    ngOnInit(): void {
    }

    getRutaAnterior() {
      this.location.back();
    }

    crearFormulario() {
        this.formMuestra = this.fb.group({
            fechaMuestra: [ null ],
            nombreMuestra: [ '' ],
            observaciones: [ null ]
        });
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
        console.log( this.formMuestra.value );
        this.openDialog( '', '<b>La información ha sido guardada exitosamente.</b>' );
    }

}
