import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-observaciones-item-listchequeo',
  templateUrl: './dialog-observaciones-item-listchequeo.component.html',
  styleUrls: ['./dialog-observaciones-item-listchequeo.component.scss']
})
export class DialogObservacionesItemListchequeoComponent implements OnInit {

    addressForm = this.fb.group({
      observaciones: [null, Validators.required]
    });
    editorStyle = {
      height: '50px'
    };
    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        public matDialogRef: MatDialogRef<DialogObservacionesItemListchequeoComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any )
    { }

    ngOnInit(): void {
        this.addressForm.get( 'observaciones' ).setValue( this.data.registro.observacion );
    }

    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
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

    onSubmit() {
      this.estaEditando = true;
      this.matDialogRef.close({obs : this.addressForm.get( 'observaciones' ).value, estaEditando: true});
    }

}
